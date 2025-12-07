using SharedLibrary;
using System;
using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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

    //Code chức năng Delete File
    // sẽ tạo nhánh 'feature/chuc-nang-delete'
    public async Task<bool> DeleteFile(string path)
    {
        // TODO: Code logic gọi API xóa file
        return false;
    }

    #endregion

    // Upload/Download File (TCP Server)

    #region TCP File Transfer (Truyền tải file)

    // Code chức năng Upload File
    // sẽ tạo nhánh 'feature/chuc-nang-upload'
    public async Task UploadFileAsync(string localFilePath)
    {
        await EnsureConnectedAsync();

        FileInfo fi = new FileInfo(localFilePath);
        if (!fi.Exists) throw new FileNotFoundException("File not found");

        // B1: Gửi Request
        await _writer.WriteLineAsync($"{ProtocolCommands.UPLOAD_REQ}|{fi.Name}|{fi.Length}");

        // B2: Chờ READY
        string response = await _reader.ReadLineAsync();
        if (response != ProtocolCommands.READY_FOR_UPLOAD)
            throw new Exception($"Server refused: {response}");

        // B3: Gửi Stream
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

        // B4: Chờ SUCCESS
        string result = await _reader.ReadLineAsync();
        if (result != ProtocolCommands.UPLOAD_SUCCESS)
            throw new Exception("Upload failed on Server side.");
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
