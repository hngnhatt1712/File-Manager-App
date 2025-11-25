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
    private HttpClient apiClient; // Dùng để gọi Web API
    private string jwtToken;
    private TcpClient tcpClient;  // Dùng để truyền file
    private NetworkStream _stream;
    private StreamReader _reader;
    private StreamWriter _writer;
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
            // Nếu tcpClient cũ đã bị đóng hoặc null, phải tạo mới
            if (tcpClient == null || !tcpClient.Connected)
            {
                tcpClient = new TcpClient();
            }

            // 3. Kết nối đến TCP Server 
            tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(_host, _port);
            _stream = tcpClient.GetStream();

            // Cấu hình bộ đọc/ghi để khớp với Server
            _reader = new StreamReader(_stream, Encoding.UTF8);
            _writer = new StreamWriter(_stream, Encoding.UTF8) { AutoFlush = true };
            // 4. Gửi token cho Server để xác thực (Handshake)
            await _writer.WriteLineAsync(jwtToken);

            // 5.Chờ Server phản hồi "OK"
            // Đây là thiết kế tốt để đảm bảo Server chấp nhận token
            string response = await _reader.ReadLineAsync();

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
            string serverMessage;
            while (IsConnected && (serverMessage = await _reader.ReadLineAsync()) != null)
            {
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
                await _writer.WriteLineAsync(ProtocolCommands.QUIT);
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
        var payload = new UserServerPayload { Uid = uid, Email = email, Phone = phone };
        string jsonBody = JsonSerializer.Serialize(payload);
        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage(HttpMethod.Post, "api/users/register");
        request.Content = content;
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

        var response = await apiClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            string error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Lỗi API Server: {error}");
        }

    }
    public void SetAuthToken(string token)
    {
        this.jwtToken = token;
        // Cập nhật luôn cho HttpClient để dùng cho các API sau này
        apiClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }   
    #endregion

    #region API File Metadata (Quản lý thông tin file)

    public async Task<string> GetFileList(string path)
    {
        // 1. Kiểm tra token
        if (string.IsNullOrEmpty(jwtToken))
            return "[]";

        try
        {
            // 2. Đảm bảo Header luôn mới nhất 
            apiClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", jwtToken);

            // 3. Gọi API
            string encodedPath = System.Net.WebUtility.UrlEncode(path);
            var response = await apiClient.GetAsync($"api/files?path={encodedPath}");

            // 4. Kiểm tra lỗi HTTP (200 OK)
            if (!response.IsSuccessStatusCode)
            {
                // Có thể log lỗi hoặc throw exception tùy bạn
                return "[]";
            }

            // 5. Trả về kết quả JSON
            return await response.Content.ReadAsStringAsync();
        }
        catch
        {
            return "[]";
        }
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
