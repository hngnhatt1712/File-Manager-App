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
                        await HandleDownloadAsync();
                        break;
                    case ProtocolCommands.DELETE_FILE:
                         await HandleDeleteFileAsync();
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
                                return;
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
                            ReceiveFileFromStream(savePath, metadata.Size);

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
                            _writer.WriteLine(ProtocolCommands.UPLOAD_FAIL + "|" + ex.Message);
                            _writer.Flush();
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

    // Hàm nhận dữ liệu
    private async Task ReceiveFileFromStream(string filePath, long totalBytes)
    {
        // Tạo file mới để ghi
        using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        {
            byte[] buffer = new byte[8192]; // Bộ đệm 8KB
            long bytesReceived = 0;
            int read;
            while (bytesReceived < totalBytes)
            {
                // Tính toán số byte cần đọc 
                int toRead = (int)Math.Min(buffer.Length, totalBytes - bytesReceived);

                // Đọc từ NetworkStream 
                read = _client.GetStream().Read(buffer, 0, toRead);
                if (read == 0) throw new Exception("Mất kết nối khi đang nhận file!");
                fileStream.Write(buffer, 0, read);
                bytesReceived += read;
            }
        }
    }
    private async Task HandleDownloadAsync()
    {
        // BƯỚC 1: Kiểm tra xác thực
        if (!_isAuthenticated)
        {
            await _writer.WriteLineAsync(ProtocolCommands.ACCESS_DENIED);
            await _writer.FlushAsync();
            return;
        }

        try
        {
            // BƯỚC 2: Đọc tên file client muốn tải
            // (Client sẽ gửi lệnh DOWNLOAD)
            string fileNameReq = await _reader.ReadLineAsync();

            if (string.IsNullOrEmpty(fileNameReq)) return;

            Console.WriteLine($"[Download Req] User {_authenticatedUid} muon tai: {fileNameReq}");

            // BƯỚC 3: Tìm đường dẫn vật lý của file trên Server
            // Logic: ServerStorage / UserID / FileName
            string userFolder = Path.Combine(_storagePath, _authenticatedUid);
            string fullPath = Path.Combine(userFolder, fileNameReq);

            // BƯỚC 4: Kiểm tra file có tồn tại không
            if (!File.Exists(fullPath))
            {
                Console.WriteLine($"[Download Error] Khong tim thay file: {fullPath}");
                await _writer.WriteLineAsync(ProtocolCommands.FILE_NOT_FOUND);
                await _writer.FlushAsync();
                return;
            }

            // BƯỚC 5: Chuẩn bị gửi
            long fileSize = new FileInfo(fullPath).Length;

            // Gửi lệnh: DOWNLOADING|Kích_thước (Dùng dấu | ngăn cách cho dễ tách)
            // Ví dụ gửi: "DOWNLOADING|5242880"
            await _writer.WriteLineAsync($"{ProtocolCommands.DOWNLOADING}|{fileSize}");
            await _writer.FlushAsync();

            // BƯỚC 6: Bơm dữ liệu file qua NetworkStream
            using (FileStream fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer = new byte[8192];
                int bytesRead;

                // Đọc từ ổ cứng Server -> Đẩy ra Client
                while ((bytesRead = await fs.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    await _client.GetStream().WriteAsync(buffer, 0, bytesRead);
                }
            }
            await _client.GetStream().FlushAsync();
            Console.WriteLine($"[Download Success] Da gui xong file {fileNameReq}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Download Exception] {ex.Message}");
            // Chỉ gửi lỗi nếu kết nối còn sống
            try
            {
                await _writer.WriteLineAsync(ProtocolCommands.DOWNLOAD_FAIL);
                await _writer.FlushAsync();
            }
            catch { }
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
}
