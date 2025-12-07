
using FirebaseAdmin.Auth;
using ServerApp;
using ServerApp.Controllers;
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

    private readonly FirebaseAdminService _firestoreService;
    private readonly string _storagePath;
    private readonly FileController _fileController;
    private readonly UserController _userController;
    private bool _isAuthenticated = false;
    private string _authenticatedUid = null;
    public ClientHandler(TcpClient client, string storagePath, FirebaseAdminService firestoreService)
    {
        _client = client;
        _stream = _client.GetStream();
        _reader = new StreamReader(_stream, Encoding.UTF8);
        _writer = new StreamWriter(_stream, Encoding.UTF8) { AutoFlush = true };
        _firestoreService = firestoreService;
        _storagePath = storagePath;
        _fileController = new FileController(firestoreService);
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
                        if (parts.Length < 3) return;

                        string fileName = parts[1];
                        long fileSize = long.Parse(parts[2]);

                        // 1. Tạo đường dẫn lưu file (ServerStorage/UserId/FileName)
                        string userFolder = Path.Combine(_storagePath, _authenticatedUid);
                        Directory.CreateDirectory(userFolder);
                        string savePath = Path.Combine(userFolder, fileName);
                        // 2. Gửi tín hiệu READY cho Client
                        _writer.WriteLine(ProtocolCommands.READY_FOR_UPLOAD);
                        _writer.Flush();
                        Console.WriteLine($"[Upload] Bắt đầu nhận file {fileName} ({fileSize} bytes)...");

                        try
                        {
                            // 3. Gọi hàm nhận luồng dữ liệu 
                            ReceiveFileFromStream(savePath, fileSize);

                            // 4. Lưu Metadata vào Firebase (Sau khi nhận xong file vật lý)
                            _fileController.SaveFileMetadata(_authenticatedUid, fileName, fileSize, savePath);

                            // 5. Báo thành công
                            _writer.WriteLine(ProtocolCommands.UPLOAD_SUCCESS);
                            _writer.Flush();
                            Console.WriteLine("[Upload] Hoàn tất!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[Upload Error] {ex.Message}");
                            _writer.WriteLine(ProtocolCommands.UPLOAD_FAIL + "|" + ex.Message);
                            _writer.Flush();
                            // Xóa file rác nếu lỗi
                            if (File.Exists(savePath)) File.Delete(savePath);
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

    // Hàm nhận dữ liệu
    private void ReceiveFileFromStream(string filePath, long totalBytes)
    {
        // Tạo file mới để ghi
        using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        {
            byte[] buffer = new byte[8192]; // Bộ đệm 8KB
            long bytesReceived = 0;
            int read;

            // Vòng lặp đọc dữ liệu từ mạng -> ghi vào ổ cứng
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
