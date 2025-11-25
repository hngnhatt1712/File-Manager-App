
using FirebaseAdmin.Auth;
using ServerApp;
using SharedLibrary; 
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class ClientHandler
{
    private readonly TcpClient _client;
    private readonly NetworkStream _stream;
    private readonly StreamReader _reader; 
    private readonly StreamWriter _writer; 

    private readonly FirebaseAuthService _authService;
    private readonly FirebaseAdminService _firestoreService;
    private bool _isAuthenticated = false;
    private string _authenticatedUid = null;
    public ClientHandler(TcpClient client, FirebaseAuthService authService, FirebaseAdminService firestoreService)
    {
        _client = client;
        _stream = _client.GetStream();
        _reader = new StreamReader(_stream, Encoding.UTF8);
        _writer = new StreamWriter(_stream, Encoding.UTF8) { AutoFlush = true };
        _authService = authService;
        _firestoreService = firestoreService;
    }

    public async Task HandleClientAsync()
    {
        try
        {
            Console.WriteLine("[TCP] Client ket noi. Dang cho Token...");

            // Đọc dòng đầu tiên Client gửi (Client đang gửi Token ngay lập tức)
            string token = await _reader.ReadLineAsync();

            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("[TCP] Token rong -> Ngat ket noi.");
                return;
            }

            // Gọi hàm verify (Lưu ý: Bạn cần chắc chắn _authService trả về FirebaseToken hợp lệ)
            // Nếu bạn dùng thư viện FirebaseAdmin thì kết quả trả về là FirebaseToken
            FirebaseToken decodedToken = await _authService.VerifyIdTokenAsync(token);

            if (decodedToken != null)
            {
                _isAuthenticated = true;
                _authenticatedUid = decodedToken.Uid;
                Console.WriteLine($"[TCP] Login OK: {_authenticatedUid}");

                // QUAN TRỌNG: Gửi tín hiệu OK về cho Client để Client biết mà chạy tiếp
                // Client của bạn đang chờ dòng chữ "AUTH_OK" (theo code Client bạn gửi lúc trước)
                await _writer.WriteLineAsync("AUTH_OK");
            }
            else
            {
                Console.WriteLine("[TCP] Token sai -> Tu choi.");
                await _writer.WriteLineAsync("AUTH_FAIL");
                return; // Ngắt kết nối luôn
            }
            // VÒNG LẶP LỆNH 
            string command;
            while ((command = await _reader.ReadLineAsync()) != null)
            {
                // Phân luồng lệnh
                switch (command.ToUpper())
                {
                    
                    case ProtocolCommands.QUIT:
                        return; 
                    case ProtocolCommands.PING:
                        await _writer.WriteLineAsync(ProtocolCommands.PONG);
                        break;
                    case ProtocolCommands.UPLOAD:
                        await HandleUploadAsync();
                        break;
                    case ProtocolCommands.DOWNLOAD:
                        await HandleDownloadAsync();
                        break;
                    case ProtocolCommands.DELETE_FILE:
                         await HandleDeleteFileAsync();
                        break;
                    case ProtocolCommands.LIST_FILES:
                         await HandleListFilesAsync();
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

    private async Task<bool> AuthenticateClientAsync()
    {
        try
        {
            string token = await _reader.ReadLineAsync();
            if (string.IsNullOrEmpty(token)) return false;

            FirebaseToken decodedToken = await _authService.VerifyIdTokenAsync(token);
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
    private async Task HandleListFilesAsync()
    {
        string path = await _reader.ReadLineAsync();
        var fileList = await _firestoreService.GetFileListAsync(_authenticatedUid, path);
        string jsonResponse = JsonSerializer.Serialize(fileList);

        await _writer.WriteLineAsync(ProtocolCommands.LIST_FILES_SUCCESS);
        await _writer.WriteLineAsync(jsonResponse);
    }
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

            // 2. TODO:  cần tạo hàm 'DeleteFileAsync' bên trong 'FirebaseAdminService.cs' sau đó gọi vào dưới
            // await _firestoreService.DeleteFileAsync(_authenticatedUid, filePath);

            await _writer.WriteLineAsync(ProtocolCommands.DELETE_SUCCESS);
        }
        catch (Exception ex)
        {
            await _writer.WriteLineAsync(ProtocolCommands.DELETE_FAIL);
        }
    }
    private async Task HandleUploadAsync()
    {
        // BƯỚC 1: Kiểm tra xác thực
        if (!_isAuthenticated)
        {
            await _writer.WriteLineAsync(ProtocolCommands.ACCESS_DENIED);
            return;
        }

        try
        {
            // Đọc metadata (tên file, kích thước)
            string fileName = await _reader.ReadLineAsync();
            long fileSize = long.Parse(await _reader.ReadLineAsync());

            // (Kiểm tra dung lượng, tên file hợp lệ...)

            // Báo Client sẵn sàng
            

            // (Code nhận stream file và lưu vào đĩa...)

            // (Code cập nhật metadata vào Firestore...)

            // Báo thành công
            await _writer.WriteLineAsync(ProtocolCommands.UPLOAD_SUCCESS);
        }
        catch (Exception ex)
        {
            await _writer.WriteLineAsync(ProtocolCommands.UPLOAD_FAIL);
        }
    }

    private async Task HandleDownloadAsync()
    {
        // BƯỚC 1: Kiểm tra xác thực
        if (!_isAuthenticated)
        {
            await _writer.WriteLineAsync(ProtocolCommands.ACCESS_DENIED);
            return;
        }

        try
        {
            // Đọc file client muốn tải
            string filePath = await _reader.ReadLineAsync();

            // (Kiểm tra file tồn tại và quyền sở hữu (dùng _authenticatedUid)...)

            // Báo Client bắt đầu tải
            

            // (Code gửi kích thước file...)
            // (Code gửi stream file...)
        }
        catch (Exception ex)
        {
            await _writer.WriteLineAsync(ProtocolCommands.DOWNLOAD_FAIL);
        }
    }
}
