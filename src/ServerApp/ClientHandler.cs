using FirebaseAdmin.Auth;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Newtonsoft.Json;
using ServerApp;
using ServerApp.Controllers;
using SharedLibrary; 
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Google.Rpc.Context.AttributeContext.Types;

public class ClientHandler
{
    private readonly TcpClient _client;
    private readonly NetworkStream _stream;
    private readonly StreamReader _reader; 
    private readonly StreamWriter _writer; 

    private readonly FirebaseAdminService _firestoreService;
    private FirestoreDb _firestoreDb;
    private readonly string _storagePath;
    private readonly FileController _fileController;
    private readonly UserController _userController;
    private bool _isAuthenticated = false;
    private string _authenticatedUid = null;
    public ClientHandler(TcpClient client, string storagePath, FirebaseAdminService firestoreService, FirestoreDb firestoreDb)
    {
        _client = client;
        _stream = _client.GetStream();
        _reader = new StreamReader(_stream, Encoding.UTF8);
        _writer = new StreamWriter(_stream, Encoding.UTF8) { AutoFlush = true };
        _firestoreService = firestoreService;
        _firestoreDb = firestoreDb;
        _storagePath = storagePath;
        _fileController = new FileController(_firestoreDb);
        _userController = new UserController(firestoreService);
    }

    public async Task HandleClientAsync()
    {
        try
        {
            Console.WriteLine("[TCP] Client ket noi. Dang cho lenh...");
            // VÒNG LẶP LỆNH 
            string command;
            while ((command = await _reader.ReadLineAsync()) != null)
            {
                string[] parts = command.Split('|');
                string cmdType = parts[0].ToUpper();
                // Phân luồng lệnh
                switch (cmdType)
                {
                    case "SYNC_USER":
                        {
                            if (parts.Length >= 4)
                            {
                                string payloadToken = parts[1];
                                string email = parts[2];
                                string phone = parts[3];

                                // 1. Gọi Controller (Trả về: "SYNC_OK|Uid123" hoặc "SYNC_FAIL|Error")
                                string response = await _userController.SyncUser(payloadToken, email, phone);

                                // 2. Kiểm tra kết quả (SỬA LỖI 1: Dùng StartsWith)
                                if (response.StartsWith("SYNC_OK"))
                                {
                                    // Tách lấy UID để lưu vào session của Handler này
                                    string[] resParts = response.Split('|');
                                    if (resParts.Length > 1)
                                    {
                                        _isAuthenticated = true;
                                        _authenticatedUid = resParts[1]; // Lưu UID quan trọng này
                                        Console.WriteLine($"[TCP Session] Da gan UID: {_authenticatedUid}");
                                    }

                                    // 3. Gửi phản hồi thành công về Client (SỬA LỖI 2: Chỉ gửi 1 lần)
                                    // Client chỉ cần biết là OK, không cần UID trả về nữa
                                    await _writer.WriteLineAsync("SYNC_OK");
                                }
                                else
                                {
                                    // Nếu thất bại, gửi nguyên văn thông báo lỗi (VD: SYNC_FAIL|Token het han)
                                    await _writer.WriteLineAsync(response);
                                }

                                // 4. Đẩy dữ liệu đi ngay
                                await _writer.FlushAsync();
                            }
                            break;
                        }
                    case ProtocolCommands.QUIT:
                        return; 
                    case ProtocolCommands.PING:
                        await _writer.WriteLineAsync(ProtocolCommands.PONG);
                        break;
                    case ProtocolCommands.DOWNLOAD:
                        await HandleDownloadAsync(parts);
                        break;
                    case ProtocolCommands.LIST_FILES:
                         await HandleListFilesAsync(parts);
                        break;
                    case ProtocolCommands.UPLOAD_REQ:
                        try
                        {
                            // 1. Tách lấy phần JSON (Bỏ phần lệnh "UPLOAD_REQ|" ở đầu)
                            // Client gửi: UPLOAD_REQ|{"fileId":"...", "fileName":"abc.txt", ...}
                            string jsonReceived = command.Substring(command.IndexOf('|') + 1);

                            Console.WriteLine($"[Upload] Nhận JSON Metadata: {jsonReceived}"); // Debug xem có tên file chưa

                            // 2. Deserialize ra đối tượng FileMetadata 
                            var metadata = JsonConvert.DeserializeObject<FileMetadata>(jsonReceived);

                            // --- KIỂM TRA QUAN TRỌNG ---
                            if (metadata == null || string.IsNullOrEmpty(metadata.FileName))
                            {
                                Console.WriteLine("[Error] Metadata thiếu tên file!");
                                await _writer.WriteLineAsync(ProtocolCommands.UPLOAD_FAIL + "|Metadata không hợp lệ");
                                await _writer.FlushAsync();
                                break;
                            }

                            // --- KIỂM TRA QUOTA ---
                            bool quotaOK = await _fileController.CheckStorageQuotaAsync(_authenticatedUid, metadata.Size);
                            if (!quotaOK)
                            {
                                Console.WriteLine($"[Upload] Từ chối: Dung lượng không đủ!");
                                // ✅ Gửi lỗi quota thôi, không cần gửi storageInfo
                                await _writer.WriteLineAsync(ProtocolCommands.QUOTA_EXCEEDED + "|Dung lượng bộ nhớ không đủ");
                                await _writer.FlushAsync();
                                break; // Dừng xử lý upload này
                            }

                            // 3. Cập nhật các thông tin phía Server (mà Client không biết)
                            string userFolder = Path.Combine(_storagePath, _authenticatedUid);
                            Directory.CreateDirectory(userFolder);

                            string savePath = Path.Combine(userFolder, metadata.FileName);

                            // Cập nhật lại đường dẫn thực tế và người sở hữu vào object
                            metadata.OwnerUid = _authenticatedUid;
                            metadata.StoragePath = savePath;
                            metadata.UploadedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                            // 4. Gửi tín hiệu READY
                            _writer.WriteLine(ProtocolCommands.READY_FOR_UPLOAD);
                            _writer.Flush();
                            Console.WriteLine($"[Upload] Sẵn sàng nhận file {metadata.FileName} ({metadata.Size} bytes)...");

                            // 5. Nhận luồng file vật lý
                            await ReceiveFileFromStream(savePath, metadata.Size);

                            // 6. Lưu Metadata vào Firestore 
                            // LƯU Ý: Truyền nguyên cục 'metadata' đã đầy đủ thông tin vào hàm lưu
                            // Bạn cần sửa hàm SaveFileMetadata để nhận đối tượng (Xem Bước 2 bên dưới)
                            await _fileController.SaveFileMetadata(metadata);

                            // 7. Báo thành công
                            _writer.WriteLine(ProtocolCommands.UPLOAD_SUCCESS);
                            _writer.Flush();
                            Console.WriteLine("[Upload] Hoàn tất và đã lưu DB!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[Upload Error] {ex.Message}");
                            
                            // ✅ Xóa file rác nếu upload fail
                            try
                            {
                                string userFolder = Path.Combine(_storagePath, _authenticatedUid);
                                string savePath = Path.Combine(userFolder, "temp_file");
                                // Find and delete the incomplete file
                                var dir = new DirectoryInfo(userFolder);
                                if (dir.Exists)
                                {
                                    foreach (var file in dir.GetFiles())
                                    {
                                        if (file.Length == 0) // Xóa file rỗng
                                        {
                                            file.Delete();
                                            Console.WriteLine($"[Upload] Đã xóa file rỗng: {file.Name}");
                                        }
                                    }
                                }
                            }
                            catch { }
                            
                            await _writer.WriteLineAsync(ProtocolCommands.UPLOAD_FAIL + "|" + ex.Message);
                            await _writer.FlushAsync();
                        }
                        break;
                    case ProtocolCommands.SEARCH_REQ: // tìm kiếm file
                        // z: 1. Đọc từ khóa tìm kiếm
                        string keyword = await _reader.ReadLineAsync();
                        // z: 2. Gọi hàm tìm trong Database
                        var filesFound = await _firestoreService.SearchFilesAsync(_authenticatedUid, keyword);
                        // z: 3. Chuyển kết quả thành chuỗi JSON và gửi về cho Client
                        string json = JsonConvert.SerializeObject(filesFound);
                        await _writer.WriteLineAsync(ProtocolCommands.SEARCH_SUCCESS);
                        await _writer.WriteLineAsync(json);
                        break;
                    case "GET_TRASH_FILES": // Lệnh lấy danh sách file trong thùng rác
                        if (!_isAuthenticated)
                        {
                            await _writer.WriteLineAsync(ProtocolCommands.ACCESS_DENIED);
                        }
                        else
                        {
                            await HandleGetTrashFilesAsync();
                        }
                        break;
                    case ProtocolCommands.RENAME_FILE:
                        Console.WriteLine($"[DEBUG] Đã nhận lệnh RENAME từ User {_authenticatedUid}");
                        await HandleRenameFileAsync(parts);
                        break;

                    case ProtocolCommands.STAR_FILE:
                        Console.WriteLine($"[DEBUG] Đã nhận lệnh STAR từ User {_authenticatedUid}");
                        await HandleStarFileAsync(parts);
                        break;
                    case "DELETE_ACCOUNT":
                        await HandleDeleteAccountAsync();
                        break;
                    case "UPDATE_EMAIL":
                        if (parts.Length >= 3)
                        {
                            string token = parts[1];
                            string newEmail = parts[2];
                            // Gọi hàm xử lý riêng để code gọn gàng hơn
                            await HandleUpdateEmailRequestAsync(token, newEmail);
                        }
                        else
                        {
                            await _writer.WriteLineAsync("UPDATE_EMAIL_FAIL|Thiếu dữ liệu");
                            await _writer.FlushAsync();
                        }
                        break;
                    case ProtocolCommands.GET_STORAGE_INFO:
                        if (!_isAuthenticated)
                        {
                            Console.WriteLine($"[Storage Error] User chưa xác thực!");
                            await _writer.WriteLineAsync(ProtocolCommands.ACCESS_DENIED);
                            await _writer.FlushAsync();
                        }
                        else
                        {
                            await HandleGetStorageInfoAsync();
                        }
                        break;
                    case ProtocolCommands.MOVE_TO_TRASH:
                        {
                            string fileId = await _reader.ReadLineAsync(); // Đọc FileId gửi kèm
                                                                           // Chuyển isDeleted thành TRUE (vào thùng rác)
                            bool success = await _firestoreService.UpdateFileDeleteStatusAsync(fileId, true);
                            await _writer.WriteLineAsync(success ? ProtocolCommands.MOVE_TO_TRASH_SUCCESS : "FAIL");
                            break;
                        }

                    case ProtocolCommands.RESTORE_FILE:
                        {
                            string fileId = await _reader.ReadLineAsync();
                            // Chuyển isDeleted thành FALSE (khôi phục)
                            bool success = await _firestoreService.UpdateFileDeleteStatusAsync(fileId, false);
                            await _writer.WriteLineAsync(success ? ProtocolCommands.RESTORE_SUCCESS : "FAIL");
                            break;
                        }
                    case ProtocolCommands.DELETE_FILE:
                        {
                            string fileId = await _reader.ReadLineAsync(); // Đọc FileId cần xóa

                            // Gọi hàm xóa vĩnh viễn trong DB
                            bool success = await _firestoreService.DeleteFilePermanentlyAsync(fileId);

                            // Phản hồi về Client
                            await _writer.WriteLineAsync(success ? ProtocolCommands.DELETE_SUCCESS : ProtocolCommands.DELETE_FAIL);
                            break;
                        }
                    default:
                        await _writer.WriteLineAsync(ProtocolCommands.UNKNOWN_COMMAND);
                        break;
                    }
                }
            }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi handler: {ex.Message}");
        }
        finally
        {
            _client.Close();
        }
    }
    private async Task HandleDeleteAccountAsync()
    {
        try
        {
            // 1. Gọi FirebaseAdminService để xoá (Bước 5)
            bool success = await _firestoreService.DeleteUserCompletelyAsync(_authenticatedUid);

            if (success)
            {
                await _writer.WriteLineAsync("DELETE_SUCCESS");
                _client.Close(); // Ngắt kết nối luôn sau khi xoá
            }
            else
            {
                await _writer.WriteLineAsync("DELETE_FAIL");
            }
        }
        catch
        {
            await _writer.WriteLineAsync("DELETE_FAIL");
        }
    }
    private async Task HandleUpdateEmailRequestAsync(string idToken, string newEmail)
    {
      
    }

    private async Task<bool> AuthenticateClientAsync()
    {
        try
        {
            string token = await _reader.ReadLineAsync();
            if (string.IsNullOrEmpty(token)) return false;

            FirebaseToken decodedToken = await _firestoreService.VerifyTokenAsync(token);
            if (decodedToken != null)
            {
                _authenticatedUid = decodedToken.Uid;
                return true;
            }
            return false;
        }
        catch { return false; }
    }
    // Các hàm nghiệp vụ 
    // Lấy danh sách file
    private async Task HandleListFilesAsync(string[] parts)
    {
        string path = parts.Length > 2 ? parts[2] : "/";
        string response = await _fileController.HandleListFilesAsync(_authenticatedUid, path);

        // Gửi phản hồi về Client
        await _writer.WriteLineAsync(response);
        await _writer.FlushAsync();
    }

    // tiếp nhân và xử lí yêu cầu xóa file
    private async Task HandleDeleteFileAsync()
    {
        if (!_isAuthenticated)
        {
            await _writer.WriteLineAsync(ProtocolCommands.ACCESS_DENIED);
            return;
        }

        try
        {
            // 1. Đọc file path client muốn xóa
            string filePath = await _reader.ReadLineAsync();

            await _writer.WriteLineAsync(ProtocolCommands.DELETE_SUCCESS);
        }
        catch (Exception ex)
        {
            await _writer.WriteLineAsync(ProtocolCommands.DELETE_FAIL);
        }
    }

    // Hàm nhận dữ liệu (sử dụng async để tránh block thread)
    private async Task ReceiveFileFromStream(string filePath, long totalBytes)
    {
        // Tạo file mới để ghi
        using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, FileOptions.SequentialScan | FileOptions.Asynchronous))
        {
            byte[] buffer = new byte[8192]; // Bộ đệm 8KB
            long bytesReceived = 0;
            int read;
            
            Console.WriteLine($"[ReceiveFile] Bắt đầu nhận file {totalBytes} bytes...");
            
            while (bytesReceived < totalBytes)
            {
                // Tính toán số byte cần đọc 
                int toRead = (int)Math.Min(buffer.Length, totalBytes - bytesReceived);

                // ✅ Thêm timeout 30 giây cho ReadAsync
                var readTask = _stream.ReadAsync(buffer, 0, toRead);
                var timeoutTask = Task.Delay(30000); // 30s timeout
                var completedTask = await Task.WhenAny(readTask, timeoutTask);
                
                if (completedTask == timeoutTask)
                {
                    Console.WriteLine($"[ReceiveFile] ✗ Timeout! Client không gửi dữ liệu. Đã nhận {bytesReceived}/{totalBytes} bytes");
                    throw new Exception($"Timeout nhận file! Đã nhận {bytesReceived}/{totalBytes} bytes");
                }
                
                read = await readTask;
                
                if (read == 0)
                {
                    Console.WriteLine($"[ReceiveFile] ✗ Mất kết nối! Đã nhận {bytesReceived}/{totalBytes} bytes");
                    throw new Exception($"Mất kết nối khi đang nhận file! Đã nhận {bytesReceived}/{totalBytes} bytes");
                }
                
                // Ghi vào file (async)
                await fileStream.WriteAsync(buffer, 0, read);
                bytesReceived += read;
                
                // Log tiến độ mỗi 100KB
                if (bytesReceived % 102400 == 0 || bytesReceived == totalBytes)
                {
                    Console.WriteLine($"[ReceiveFile] Tiến độ: {bytesReceived}/{totalBytes} bytes ({(bytesReceived * 100 / totalBytes)}%)");
                }
            }
            
            await fileStream.FlushAsync();
            Console.WriteLine($"[ReceiveFile] ✓ Hoàn tất! Nhận đủ {bytesReceived} bytes");
        }
    }
    //dowload file

    private async Task HandleDownloadAsync(string[] parts)
    {
        string fileNameReq = "Unknown";
        try
        {
            Console.WriteLine("[Download] ---------------------------------------------");

            // 1. Kiểm tra tham số đầu vào
            if (parts.Length < 2)
            {
                Console.WriteLine("[Download Error] Client gửi lệnh thiếu tên file!");
                return;
            }

            fileNameReq = parts[1].Trim();
            Console.WriteLine($"[Download Req] Yêu cầu tải file: {fileNameReq}");

            // 2. Kiểm tra xác thực
            if (!_isAuthenticated)
            {
                Console.WriteLine("[Download Error] User chưa đăng nhập. Từ chối.");
                await _writer.WriteLineAsync(ProtocolCommands.ACCESS_DENIED);
                return;
            }

            // 3. Kiểm tra file vật lý
            string userFolder = Path.Combine(_storagePath, _authenticatedUid);
            string fullPath = Path.Combine(userFolder, fileNameReq);

            if (!File.Exists(fullPath))
            {
                Console.WriteLine($"[Download Error] Không tìm thấy file tại: {fullPath}");
                await _writer.WriteLineAsync(ProtocolCommands.FILE_NOT_FOUND);
                return;
            }

            long fileSize = new FileInfo(fullPath).Length;
            Console.WriteLine($"[Download] Đã tìm thấy file. Kích thước: {fileSize} bytes");

            // 4. Gửi Header báo kích thước
            // Gửi: DOWNLOADING|102400
            await _writer.WriteLineAsync($"{ProtocolCommands.DOWNLOADING}|{fileSize}");
            await _writer.FlushAsync();

            // 5. HANDSHAKE (BẮT TAY): Chờ Client báo "READY" mới gửi dữ liệu
            Console.WriteLine("[Download] Đang chờ Client báo sẵn sàng...");
            try
            {
                // Tạo timeout 10 giây, nếu Client đơ thì hủy luôn
                var readTask = _reader.ReadLineAsync();
                var timeoutTask = Task.Delay(10000);

                var completedTask = await Task.WhenAny(readTask, timeoutTask);
                if (completedTask == timeoutTask)
                {
                    Console.WriteLine("[Download Error] Timeout! Client không phản hồi 'READY'.");
                    return;
                }

                string ack = await readTask;
                if (ack != "READY")
                {
                    Console.WriteLine($"[Download Error] Client phản hồi lạ: {ack}");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Download Handshake Error] {ex.Message}");
                return;
            }

            // 6. Bắt đầu gửi dữ liệu (Stream)
            Console.WriteLine("[Download] Client đã sẵn sàng. Bắt đầu bơm dữ liệu...");

            using (FileStream fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer = new byte[8192]; // 8KB Chunk
                int bytesRead;
                long totalSent = 0;

                while ((bytesRead = await fs.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    await _client.GetStream().WriteAsync(buffer, 0, bytesRead);
                    totalSent += bytesRead;
                }
            }

            // Đẩy nốt dữ liệu còn sót trong buffer mạng đi
            await _client.GetStream().FlushAsync();

            Console.WriteLine($"[Download Success] Đã gửi xong file: {fileNameReq}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Download Exception] Lỗi nghiêm trọng khi gửi file {fileNameReq}: {ex.Message}");
            // Cố gắng báo lỗi cho client nếu còn kết nối (để client không treo)
            try { await _writer.WriteLineAsync(ProtocolCommands.DOWNLOAD_FAIL); } catch { }
        }
        finally
        {
            Console.WriteLine("[Download] ---------------------------------------------");
        }
    }

    // lấy danh sách file trong thùng rác hiển thị cho người dùng 
    private async Task HandleGetTrashFilesAsync()
    {
        try
        {
            Console.WriteLine($"[Trash Request] User {_authenticatedUid} đang lấy danh sách thùng rác...");

            var trashFiles = await _firestoreService.GetTrashFilesAsync(_authenticatedUid);

            // 2. Chuyển danh sách (List<FileMetadata>) thành chuỗi JSON
            string json = JsonConvert.SerializeObject(trashFiles);

            // 3. Gửi chuỗi JSON về cho Client qua NetworkStream
            await _writer.WriteLineAsync(json);
            await _writer.FlushAsync();

            Console.WriteLine($"[Trash Success] Đã gửi {trashFiles.Count} file rác.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Trash Error] Lỗi khi lấy thùng rác: {ex.Message}");
            await _writer.WriteLineAsync("[]");
            await _writer.FlushAsync();
        }
    }

    //Rename file

    private async Task HandleRenameFileAsync(string[] parts)
    {
        Console.WriteLine("[Rename] ---------------------------------------------");

        // 1. Kiểm tra đầu vào
        if (parts.Length < 4) return;

        string fileId = parts[1];
        string oldName = parts[2];
        string newNameRaw = parts[3]; 

        Console.WriteLine($"[Rename Req] Đổi {oldName} -> {newNameRaw}");

        try
        {
            string userFolder = Path.Combine(_storagePath, _authenticatedUid);
            string oldPath = Path.Combine(userFolder, oldName);

            // A. XỬ LÝ TÊN VÀ ĐUÔI FILE 
            string extension = Path.GetExtension(oldName); // Lấy đuôi gốc (.txt, .pdf...)
            string newName = newNameRaw;

            // Nếu tên mới chưa có đuôi giống tên cũ -> Tự động gắn vào
            if (!string.IsNullOrEmpty(extension) && !newNameRaw.EndsWith(extension, StringComparison.OrdinalIgnoreCase))
            {
                newName = newNameRaw + extension;
                Console.WriteLine($"[Rename Auto] Đã tự động thêm đuôi file: {newName}");
            }

            string newPath = Path.Combine(userFolder, newName);

            if (File.Exists(oldPath))
            {
                // Kiểm tra xem tên mới có bị trùng file khác không
                if (File.Exists(newPath) && newPath != oldPath)
                {
                    Console.WriteLine($"[Rename Error] Tên file mới đã tồn tại: {newName}");
                    await _writer.WriteLineAsync(ProtocolCommands.RENAME_FAIL);
                    return;
                }

                File.Move(oldPath, newPath);
                Console.WriteLine($"[Rename Disk] Đã đổi tên trên ổ cứng thành công.");
            }
            else
            {
                Console.WriteLine($"[Rename Warning] Không tìm thấy file trên ổ cứng (File ảo). Chỉ đổi trong DB.");
            }

            // C. CẬP NHẬT DATABASE (Chỉ làm khi bước trên OK)
            bool dbOk = await _firestoreService.RenameFileDBAsync(fileId, newName);

            if (dbOk)
            {
                Console.WriteLine("[Rename Success] --> Database đã cập nhật.");
                await _writer.WriteLineAsync(ProtocolCommands.RENAME_SUCCESS);
            }
            else
            {
                Console.WriteLine("[Rename Error] Lỗi khi cập nhật Database!");
                await _writer.WriteLineAsync(ProtocolCommands.RENAME_FAIL);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Rename Exception] Lỗi: {ex.Message}");
            // Có thể file đang bị khóa bởi tiến trình khác (đang upload/download dở)
            await _writer.WriteLineAsync(ProtocolCommands.RENAME_FAIL);
        }
        finally
        {
            Console.WriteLine("[Rename] ---------------------------------------------");
        }
    }

    //Đánh dấu file
    private async Task HandleStarFileAsync(string[] parts)
    {
        // Format nhận: STAR_FILE|FileID|TrạngTháiHiệnTại
        if (parts.Length < 3) return;

        string fileId = parts[1];
        string statusStr = parts[2];

        Console.WriteLine($"[Star Req] ID: {fileId} | Đang là sao? {statusStr}");

        try
        {
            if (bool.TryParse(statusStr, out bool currentStar))
            {
                // Gọi DB để đảo ngược trạng thái (True -> False, False -> True)
                bool dbOk = await _firestoreService.ToggleStarDBAsync(fileId, currentStar);

                if (dbOk)
                {
                    Console.WriteLine($"[Star Success] Đã đổi trạng thái thành công.");
                    // Gửi phản hồi OK về cho Client (để Client biết mà đổi màu)
                    await _writer.WriteLineAsync(ProtocolCommands.STAR_SUCCESS);
                }
                else
                {
                    Console.WriteLine($"[Star Error] Lỗi DB.");
                    await _writer.WriteLineAsync("STAR_FAIL");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Star Exception] {ex.Message}");
        }
    }
    // Lấy thông tin dung lượng bộ nhớ (đã dùng/còn trống)
    private async Task HandleGetStorageInfoAsync()
    {
        try
        {
            Console.WriteLine($"[Storage Request] User {_authenticatedUid} đang kiểm tra dung lượng...");

            // Gọi hàm từ FileController để lấy thông tin dung lượng
            string response = await _fileController.HandleGetStorageInfoAsync(_authenticatedUid);

            // Gửi phản hồi về Client
            await _writer.WriteLineAsync(response);
            await _writer.FlushAsync();

            Console.WriteLine($"[Storage Success] Đã gửi thông tin dung lượng.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Storage Error] Lỗi khi lấy thông tin dung lượng: {ex.Message}");
            await _writer.WriteLineAsync(ProtocolCommands.GET_STORAGE_INFO_FAIL + "|" + ex.Message);
            await _writer.FlushAsync();
        }
    }

    
}
