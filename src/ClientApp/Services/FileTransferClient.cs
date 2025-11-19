using SharedLibrary;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class FileTransferClient
{
    private string authToken;
    private HttpClient apiClient; // Dùng để gọi Web API
    private string jwtToken;
    private TcpClient tcpClient;  // Dùng để truyền file
    private NetworkStream _stream;
    private readonly string _host = "127.0.0.1"; 
    private readonly int _port = 8888;

    public FileTransferClient()
    {
        apiClient = new HttpClient();
        apiClient.BaseAddress = new Uri("http://localhost:5000/");
        tcpClient = new TcpClient();
        // Địa chỉ server API của bạn
    }
    public class UserServerPayload
    {
        public string Uid { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
    public bool IsConnected
    {
        get { return tcpClient != null && tcpClient.Connected; }
    }
    public async Task EnsureConnectedAsync()
    {
        if (IsConnected)
        {
            return;
        }
        // 2. Kiểm tra token (không thể kết nối TCP nếu chưa đăng nhập)
        if (string.IsNullOrEmpty(jwtToken))
        {
            throw new InvalidOperationException("Chưa đăng nhập! Không thể kết nối đến TCP Server");
        }

        try
        {
            // 3. Kết nối đến TCP Server 
            await  tcpClient.ConnectAsync(_host, _port);
            _stream = tcpClient.GetStream();

            // 4. Gửi token cho Server để xác thực (Handshake)
            string authMessage = $"{jwtToken}\n"; 
            byte[] authBuffer = Encoding.UTF8.GetBytes(authMessage);
            await _stream.WriteAsync(authBuffer, 0, authBuffer.Length);

            // 5.Chờ Server phản hồi "OK"
            // Đây là thiết kế tốt để đảm bảo Server chấp nhận token
            byte[] responseBuffer = new byte[1024];
            int bytesRead = await _stream.ReadAsync(responseBuffer, 0, responseBuffer.Length);
            string response = Encoding.UTF8.GetString(responseBuffer, 0, bytesRead).Trim();

            if (response != "AUTH_OK") 
            {
                tcpClient.Close(); // Đóng kết nối nếu xác thực thất bại
                throw new Exception($"TCP Server từ chối xác thực: {response}");
            }
            // Nếu thành công, BẮT ĐẦU CHẠY THREAD LISTENER
            _ = StartListeningAsync(); 
        }
        catch (Exception ex)
        {
            if (tcpClient.Connected)
            {
                tcpClient.Close();
            }
            throw new Exception($"Không thể kết nối với TCP Server: {ex.Message}");
        }
    }

    // Hàm này sẽ chạy ngầm để nghe các lệnh từ Server
    private async Task StartListeningAsync()
    {
        try
        {
            byte[] buffer = new byte[4096];
            while (IsConnected)
            {
                int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
                if (bytesRead == 0)
                {
                    break;
                }
                string serverMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                // TODO: Xử lý các lệnh từ server (ví dụ: "UPLOAD_SUCCESS", "FILE_DELETED"...)
            }
        }
        catch
        {
            // Lỗi (vd: mất kết nối)
        }
        finally
        {
            tcpClient.Close();
        }
    }

    public async Task DisconnectAsync()
    {
        try
        {
            // 1. Xóa token
            jwtToken = null;
            apiClient.DefaultRequestHeaders.Authorization = null;

            // 2. Gửi lệnh QUIT báo cho Server biết (Nếu đang kết nối)
            if (IsConnected)
            {
                string quitCommand = $"{ProtocolCommands.QUIT}\n";
                byte[] buffer = Encoding.UTF8.GetBytes(quitCommand);
                await _stream.WriteAsync(buffer, 0, buffer.Length);
            }
        }
        catch
        {
            // Bỏ qua lỗi nếu server đã ngắt trước
        }
        finally
        {
            // 3. Đóng kết nối vật lý
            if (tcpClient != null)
            {
                tcpClient.Close();
                // _tcpClient = new TcpClient(); // (Tùy chọn: tạo mới để sẵn sàng cho lần sau)
            }
        }
    }


    //  Xây dựng API (Phần gọi từ Client)
            #region API Authentication (Xác thực người dùng)
            // Đăng ký user lên Server
    public async Task RegisterUserOnServerAsync(string uid, string email, string phone, string jwtToken)
    {
        // Sử
    }

    #endregion

    #region API File Metadata (Quản lý thông tin file)

    public async Task<string> GetFileList(string path)
    {

         apiClient.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
         var response = await apiClient.GetAsync($"api/files?path={path}");
         return await response.Content.ReadAsStringAsync();
        return "[]"; // Trả về JSON string rỗng
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
    public void UploadFile(string localFilePath, string remotePath)
    {
        Console.WriteLine($"Dang upload {localFilePath} len {remotePath}");
        // TODO: 
        // 1. Kết nối đến TCP Server (nếu chưa)
        // 2. Gửi lệnh "UPLOAD"
        // 3. Gửi thông tin file (tên, kích thước, token...)
        // 4. Đọc file từ đĩa và gửi từng gói tin (chunk)
        // 5. Chờ xác nhận từ Server
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
