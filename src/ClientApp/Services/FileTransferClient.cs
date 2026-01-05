using Newtonsoft.Json;
using SharedLibrary;
using System;
using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SharedLibrary;
using System.Collections.Generic;

public class FileTransferClient
{
    private TcpClient _tcpClient;
    private NetworkStream _stream;
    private StreamReader _reader;
    private StreamWriter _writer;

    public FileTransferClient()
    {
    }
    public class UserServerPayload
    {
        public string Uid { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
    public bool IsConnected
    {
        get { return _tcpClient != null && _tcpClient.Connected; }
    }
    public async Task ConnectAsync()
    {
        if (IsConnected) return;

        try
        {
            _tcpClient = new TcpClient();

            // SỬA: Dùng ServerConfig thay vì hardcode chuỗi
            await _tcpClient.ConnectAsync(ServerConfig.SERVER_IP, ServerConfig.PORT);

            _stream = _tcpClient.GetStream();
            _reader = new StreamReader(_stream, Encoding.UTF8);
            _writer = new StreamWriter(_stream, Encoding.UTF8) { AutoFlush = true };

            Console.WriteLine("Đã kết nối đến Server TCP");
        }
        catch (Exception ex)
        {
            throw new Exception($"Không thể kết nối Server: {ex.Message}");
        }
    }

    public async Task EnsureConnectedAsync()
    {
        if (!IsConnected) await ConnectAsync();
    }

    public async Task DisconnectAsync()
    {
        if (!IsConnected) return;
        try
        {
            await _writer.WriteLineAsync(ProtocolCommands.QUIT);
        }
        catch { }
        finally
        {
            _tcpClient?.Close();
            _tcpClient = null;
        }
    }

    public async Task<bool> DeleteAccountAsync()
    {
        try
        {
            await _writer.WriteLineAsync("DELETE_ACCOUNT");
            await _writer.FlushAsync();

            string response = await _reader.ReadLineAsync();
            return response == "DELETE_SUCCESS";
        }
        catch { return false; }
    }

    // Trong file FileTransferClient.cs
    public async Task SyncUpdateEmailAsync(string token, string newEmail)
    {
        try
        {
            // 1. Đảm bảo đã kết nối TCP
            if (!IsConnected) await ConnectAsync();

            // 2. Tạo đối tượng yêu cầu (phải khớp với định dạng Server mong đợi)
            var updateRequest = new
            {
                Command = "UPDATE_USER_EMAIL", // Lệnh do bạn tự quy định với Server
                Token = token,
                NewEmail = newEmail
            };

            // 3. Chuyển thành JSON và gửi đi
            string json = JsonConvert.SerializeObject(updateRequest);
            byte[] data = Encoding.UTF8.GetBytes(json + "<EOF>"); // Thêm ký tự kết thúc nếu cần

            await _stream.WriteAsync(data, 0, data.Length);
            await _stream.FlushAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Lỗi đồng bộ TCP: " + ex.Message);
        }
    }


    public async Task<List<FileMetadata>> GetTrashFilesAsync()
    {
        try
        {
            // Kiểm tra kết nối
            if (_writer == null || _reader == null)
                throw new Exception("Kết nối Server bị mất");

            // 1. Gửi lệnh yêu cầu lấy file trong thùng rác
            await _writer.WriteLineAsync("GET_TRASH_FILES");
            await _writer.FlushAsync();

            // 2. Đợi Server gửi chuỗi JSON chứa danh sách file
            string jsonResponse = await _reader.ReadLineAsync();

            if (string.IsNullOrEmpty(jsonResponse)) 
                return new List<FileMetadata>();

            // 3. Chuyển chuỗi JSON thành List trong C#
            var result = JsonConvert.DeserializeObject<List<FileMetadata>>(jsonResponse);
            return result ?? new List<FileMetadata>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Lỗi] GetTrash: {ex.Message}");
            return new List<FileMetadata>();
        }
    }

    // gửi lênh để xóa file vĩnh viễn
    public async Task<bool> DeletePermanentlyAsync(string fileId)
    {
        // Gửi lệnh "DELETE_PERMANENT" kèm fileId lên Server
        string command = $"DELETE_PERMANENT|{fileId}";
        return await SendCommandAsync(command);
    }


    // để gửi tên file lên Server
    public async Task<List<FileMetadata>> SearchFilesAsync(string query)
    {
        await EnsureConnectedAsync();
        await _writer.WriteLineAsync(ProtocolCommands.SEARCH_REQ);
        await _writer.WriteLineAsync(query);

        string response = await _reader.ReadLineAsync();
        if (response == ProtocolCommands.SEARCH_SUCCESS)
        {
            string json = await _reader.ReadLineAsync();
            return JsonConvert.DeserializeObject<List<FileMetadata>>(json);
        }
        return new List<FileMetadata>();
    }

    // Lấy thông tin dung lượng bộ nhớ từ Server
    public async Task<StorageInfo> GetStorageInfoAsync()
    {
        try
        {
            await EnsureConnectedAsync();

            // Kiểm tra null trước khi sử dụng
            if (_writer == null || _reader == null)
                throw new Exception("Kết nối Server bị mất");

            // 1. Gửi lệnh GET_STORAGE_INFO lên Server
            await _writer.WriteLineAsync(ProtocolCommands.GET_STORAGE_INFO);
            await _writer.FlushAsync();

            // 2. Đợi phản hồi từ Server (dạng: GET_STORAGE_INFO_SUCCESS|{JSON})
            string response = await _reader.ReadLineAsync();

            if (string.IsNullOrEmpty(response)) 
                throw new Exception("Không nhận được phản hồi từ Server");

            // 3. Kiểm tra phản hồi
            string[] parts = response.Split('|');
            if (parts[0] == ProtocolCommands.GET_STORAGE_INFO_SUCCESS && parts.Length > 1)
            {
                // 4. Deserialize JSON thành object StorageInfo
                string json = parts[1];
                var storageInfo = JsonConvert.DeserializeObject<StorageInfo>(json);
                
                if (storageInfo != null)
                {
                    Console.WriteLine($"[Storage] Đã dùng: {storageInfo.TotalUsed} bytes, Còn trống: {storageInfo.TotalRemaining} bytes ({storageInfo.UsagePercent}%)");
                    return storageInfo;
                }
                else
                {
                    throw new Exception("Không thể parse JSON thông tin dung lượng");
                }
            }
            else if (parts[0] == ProtocolCommands.GET_STORAGE_INFO_FAIL)
            {
                string errorMsg = parts.Length > 1 ? parts[1] : "Unknown error";
                throw new Exception($"Lỗi lấy thông tin dung lượng: {errorMsg}");
            }
            else
            {
                throw new Exception($"Phản hồi không hợp lệ: {response}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Lỗi] GetStorageInfo: {ex.Message}");
            throw;
        }
    }

    // gửi lệnh để chuyển file vào thùng rác
    public async Task<bool> MoveToTrashAsync(string fileId)
    {
        string command = $"MOVE_TO_TRASH|{fileId}";
        return await SendCommandAsync(command);
    }

    // gửi lệnh khôi phục file đã chuyển vào thùng rác
    public async Task<bool> RestoreFileAsync(string fileId)
    {
        string command = $"RESTORE_FILE|{fileId}";
        return await SendCommandAsync(command);
    }

    // vẫn chuyển lệnh từ client sang server 
    private async Task<bool> SendCommandAsync(string command)
    {
        try
        {
            await _writer.WriteLineAsync(command);
            await _writer.FlushAsync();

            string response = await _reader.ReadLineAsync();

            return response == "SUCCESS";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi gửi lệnh: {ex.Message}");
            return false;
        }
    }

    //  Xây dựng API (Phần gọi từ Client)
    #region API Authentication (Xác thực người dùng)
    // Đăng ký user lên Server
    public async Task<bool> SyncUserAsync(string token, string email, string phone)
    {
        await EnsureConnectedAsync();

        string command = $"SYNC_USER|{token}|{email}|{phone}";
        await _writer.WriteLineAsync(command);
        await _writer.FlushAsync();

        string response = await _reader.ReadLineAsync();

        if (response != null && response.StartsWith("SYNC_OK")) return true;

        throw new Exception($"Lỗi đồng bộ Server: {response}");
    }
    #endregion

    #region API File Metadata (Quản lý thông tin file)

    public async Task<string> GetFileListAsync(string uid, string path = "/")
    {
        await EnsureConnectedAsync();

        await _writer.WriteLineAsync($"{ProtocolCommands.LIST_FILES}|{uid}|{path}");

        string response = await _reader.ReadLineAsync();
        if (string.IsNullOrEmpty(response)) return "[]";

        string[] parts = response.Split('|');
        if (parts[0] == "LIST_FILES_OK" && parts.Length > 1) return parts[1];

        return "[]";
    }

    //Code chức năng Rename
    // sẽ tạo nhánh 'feature/chuc-nang-rename'
    // Trong FileTransferClient.cs

    public async Task<bool> RenameFileAsync(string fileId, string oldName, string newName)
    {
        try
        {
            if (!IsConnected) return false;

            // Gửi: RENAME_FILE|ID|TenCu|TenMoi
            string command = $"{ProtocolCommands.RENAME_FILE}|{fileId}|{oldName}|{newName}";

            await _writer.WriteLineAsync(command);
            await _writer.FlushAsync();

            // Chờ phản hồi: RENAME_SUCCESS
            string response = await _reader.ReadLineAsync();

            return response == ProtocolCommands.RENAME_SUCCESS;
        }
        catch
        {
            return false;
        }
    }

    // đánh dấu file
    public async Task<bool> ToggleStarAsync(string fileId, bool currentStatus)
    {
        try
        {
            if (!IsConnected) return false;

            // Gửi lệnh: STAR_FILE|ID|True
            string command = $"{ProtocolCommands.STAR_FILE}|{fileId}|{currentStatus}";
            await _writer.WriteLineAsync(command);
            await _writer.FlushAsync();

            // Đợi Server trả lời STAR_SUCCESS
            string response = await _reader.ReadLineAsync();
            return response == ProtocolCommands.STAR_SUCCESS;
        }
        catch
        {
            return false;
        }
    }
    #endregion

    // Upload/Download File (TCP Server)
    #region TCP File Transfer (Truyền tải file)

    // Code chức năng Upload File
    // sẽ tạo nhánh 'feature/chuc-nang-upload'
    public async Task UploadFileAsync(string localFilePath, string currentDirectory = "/")
    {
        try
        {
            await EnsureConnectedAsync();

            // Kiểm tra null cho các stream
            if (_writer == null || _reader == null || _stream == null)
                throw new Exception("Kết nối Server bị mất");

            FileInfo fi = new FileInfo(localFilePath);
            if (!fi.Exists) throw new FileNotFoundException("File not found");

            // --- BƯỚC 0: KIỂM TRA QUOTA TRƯỚC KHI UPLOAD ---
            Console.WriteLine("[Upload] Kiểm tra dung lượng có sẵn...");
            try
            {
                StorageInfo storageInfo = await GetStorageInfoAsync();
                
                // Nếu file sắp upload + dung lượng đã dùng > giới hạn -> Từ chối
                if (storageInfo == null)
                    throw new Exception("Không thể lấy thông tin dung lượng");

                if (storageInfo.TotalUsed + fi.Length > storageInfo.MaxQuota)
                {
                    long spaceNeeded = (storageInfo.TotalUsed + fi.Length) - storageInfo.MaxQuota;
                    throw new Exception(
                        $"Dung lượng không đủ! File cần: {fi.Length} bytes, còn trống: {storageInfo.TotalRemaining} bytes. " +
                        $"Cần thêm {spaceNeeded} bytes để upload."
                    );
                }
                
                Console.WriteLine($"[Upload] Kiểm tra thành công. Còn trống: {storageInfo.TotalRemaining} bytes, File size: {fi.Length} bytes");
            }
            catch (Exception ex)
            {
                // Nếu không lấy được dung lượng, vẫn cho phép upload (để không block hoàn toàn)
                // Nhưng Server sẽ là layer thứ 2 để xác thực
                Console.WriteLine($"[Upload] Cảnh báo: Không kiểm tra được dung lượng: {ex.Message}");
                throw new Exception($"Lỗi kiểm tra dung lượng: {ex.Message}");
            }

            // --- BƯỚC 1: CHUẨN BỊ METADATA & GỬI JSON ---

            // 1. Tạo đối tượng Metadata đầy đủ 
            var metadata = new FileMetadata
            {
                FileId = Guid.NewGuid().ToString(), // Tạo ID duy nhất cho file ngay tại Client
                FileName = fi.Name,
                Size = fi.Length,
                Path = "/",
                UploadedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                IsDeleted = false
                // OwnerUid: Server sẽ tự điền dựa trên Token đăng nhập
            };

            // 2. Chuyển đối tượng thành chuỗi JSON
            string jsonMeta = Newtonsoft.Json.JsonConvert.SerializeObject(metadata);

            // 3. Gửi lệnh theo cấu trúc mới: UPLOAD_REQ | {JSON}
            await _writer.WriteLineAsync($"{ProtocolCommands.UPLOAD_REQ}|{jsonMeta}");
            await _writer.FlushAsync(); // Đẩy dữ liệu đi ngay

            // B2: Chờ READY (Giữ nguyên)
            string response = await _reader.ReadLineAsync();
            if (response != ProtocolCommands.READY_FOR_UPLOAD)
                throw new Exception($"Server refused: {response}");

            // B3: Gửi Stream (Giữ nguyên - Code này tốt rồi)
            using (var fs = new FileStream(localFilePath, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer = new byte[8192];
                int read;
                while ((read = await fs.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    await _stream.WriteAsync(buffer, 0, read);
                }
                await _stream.FlushAsync();
            }

            // B4: Chờ SUCCESS (Giữ nguyên)
            string result = await _reader.ReadLineAsync();

            // Kiểm tra kỹ hơn: Server có thể trả về thông báo lỗi chi tiết
            if (result == null || !result.StartsWith(ProtocolCommands.UPLOAD_SUCCESS))
            {
                // Nếu Server gửi UPLOAD_FAIL|Lý do -> Lấy lý do ra
                string errorMsg = "Unknown error";
                if (result != null && result.Contains("|"))
                    errorMsg = result.Split('|')[1];
                
                // Nếu là lỗi quota, thông báo cụ thể hơn
                if (result != null && result.Contains(ProtocolCommands.QUOTA_EXCEEDED))
                    throw new Exception($"Dung lượng bộ nhớ không đủ. Vui lòng xóa các file không cần thiết.");

                throw new Exception($"Upload failed: {errorMsg}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Upload Error] {ex.Message}");
            throw;
        }
    }
    //dowload file

    public async Task<bool> DownloadFileAsync(string fileName, string savePath)
    {
        try
        {
            if (!IsConnected)
            {
                Console.WriteLine("Lỗi: Chưa kết nối đến server.");
                return false;
            }

            // 1. Gửi lệnh yêu cầu: DOWNLOAD|TenFile.txt
            string command = $"{ProtocolCommands.DOWNLOAD}|{fileName}";
            await _writer.WriteLineAsync(command);
            await _writer.FlushAsync();

            // 2. Đọc phản hồi Header
            string header = await _reader.ReadLineAsync();

            // Xử lý các trường hợp lỗi từ Server trả về
            if (header == ProtocolCommands.FILE_NOT_FOUND) return false;
            if (header == ProtocolCommands.ACCESS_DENIED) return false;
            if (header == ProtocolCommands.DOWNLOAD_FAIL) return false;

            if (header != null && header.StartsWith(ProtocolCommands.DOWNLOADING))
            {
                // Parse kích thước file
                long fileSize = 0;
                try
                {
                    fileSize = long.Parse(header.Split('|')[1]);
                }
                catch
                {
                    Console.WriteLine("Lỗi: Không đọc được kích thước file từ server.");
                    return false;
                }

                // 3. Gửi tín hiệu 'READY' để Server biết đường bắt đầu gửi
                await _writer.WriteLineAsync("READY");
                await _writer.FlushAsync();

                // 4. Nhận dữ liệu và ghi xuống ổ cứng
                using (FileStream fs = new FileStream(savePath, FileMode.Create, FileAccess.Write))
                {
                    byte[] buffer = new byte[8192];
                    long totalRead = 0;
                    int read;

                    while (totalRead < fileSize)
                    {
                        // Tính toán lượng byte cần đọc (tránh đọc dư thừa sang gói tin sau)
                        int toRead = (int)Math.Min(buffer.Length, fileSize - totalRead);

                        read = await _stream.ReadAsync(buffer, 0, toRead);

                        if (read == 0) throw new IOException("Mất kết nối với Server giữa chừng.");

                        await fs.WriteAsync(buffer, 0, read);
                        totalRead += read;
                    }
                }
                return true; // Tải thành công
            }

            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Client Download Error] {ex.Message}");

            //Nếu tải lỗi, xóa file rác đi để người dùng không mở nhầm
            try
            {
                if (File.Exists(savePath)) File.Delete(savePath);
            }
            catch { }

            return false;
        }
    }

    #endregion
}
