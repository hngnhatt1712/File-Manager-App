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
            // 1. Gửi lệnh yêu cầu lấy file trong thùng rác
            await _writer.WriteLineAsync("GET_TRASH_FILES");

            // 2. Đợi Server gửi chuỗi JSON chứa danh sách file
            string jsonResponse = await _reader.ReadLineAsync();

            if (string.IsNullOrEmpty(jsonResponse)) return new List<FileMetadata>();

            // 3. Chuyển chuỗi JSON thành List trong C#
            return JsonConvert.DeserializeObject<List<FileMetadata>>(jsonResponse);
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
    public async Task<bool> RenameFile(string oldPath, string newPath)
    {
        // TODO: Code logic gọi API đổi tên
        return false;
    }

    #endregion

    // Upload/Download File (TCP Server)

    #region TCP File Transfer (Truyền tải file)

    // Code chức năng Upload File
    // sẽ tạo nhánh 'feature/chuc-nang-upload'
    public async Task UploadFileAsync(string localFilePath, string currentDirectory = "/")
    {
        await EnsureConnectedAsync();

        FileInfo fi = new FileInfo(localFilePath);
        if (!fi.Exists) throw new FileNotFoundException("File not found");

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

            throw new Exception($"Upload failed: {errorMsg}");
        }
    }

    // Code chức năng Download File
    // sẽ tạo nhánh 'feature/chuc-nang-download'
    public void DownloadFile(string remotePath, string localSavePath)
    {
        Console.WriteLine($"Dang download {remotePath} ve {localSavePath}");
        // TODO:
        // 1. Kết nối đến TCP Server (nếu chưa)
        // 2. Gửi lệnh "DOWNLOAD"
        // 3. Gửi thông tin file (tên, token...)
        // 4. Nhận gói tin từ Server và ghi ra file ở đĩa
        // 5. Gửi xác nhận cho Server khi hoàn tất
    }

    #endregion
}
