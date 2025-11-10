using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SharedLibrary;
using ServerApp;

namespace ServerApp
{
    internal class ClientHandler
    {
        private readonly TcpClient _client;
        private readonly string _storagePath;
        private NetworkStream _stream;
        private StreamReader _reader;
        private StreamWriter _writer;
        private string _authenticatedUserId = null;

        public ClientHandler(TcpClient client, string storagePath)
        {
            _client = client;
            _storagePath = storagePath;
        }

        // Hàm xử lý chính cho mỗi client
        public async Task HandleClientAsync()
        {
            try
            {
                _stream = _client.GetStream();
                // Hỗ trợ tên file có dấu
                _reader = new StreamReader(_stream, Encoding.UTF8);
                // AutoFlush = true để đảm bảo message được gửi đi ngay
                _writer = new StreamWriter(_stream, Encoding.UTF8) { AutoFlush = true };

                string line;
                // Vòng lặp đọc lệnh từ client
                while ((line = await _reader.ReadLineAsync()) != null)
                {
                    Console.WriteLine($"Nhận từ {_client.Client.RemoteEndPoint}: {line}");
                    string[] parts = line.Split('|');
                    string command = parts[0].ToUpper();

                    switch (command)
                    {
                        case ProtocolCommands.FIRST_LOGIN_REGISTER:
                            // FIRST_LOGIN_REGISTER|jwt_token|phone_number
                            await HandleFirstLoginRegisterAsync(parts);
                            break;

                        case ProtocolCommands.LOGIN_ATTEMPT:
                            // LOGIN_ATTEMPT|jwt_token
                            await HandleLoginAttemptAsync(parts);
                            break;
                        case ProtocolCommands.PING:
                            await _writer.WriteLineAsync(ProtocolCommands.PONG);
                            break;

                        case ProtocolCommands.UPLOAD:
                            // UPLOAD|file_id|file_size
                            await HandleUploadAsync(parts);
                            break;

                        case ProtocolCommands.DOWNLOAD:
                            // DOWNLOAD|file_id
                            await HandleDownloadAsync(parts);
                            break;

                        case ProtocolCommands.DELETE_FILE:
                            // DELETE_FILE|file_id
                            await HandleDeleteAsync(parts);
                            break;

                        case ProtocolCommands.QUIT:
                            return; // Thoát vòng lặp và đóng kết nối

                        default:
                            await _writer.WriteLineAsync(ProtocolCommands.UNKNOWN_COMMAND);
                            break;
                    }
                }
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"Client {(_client.Client.RemoteEndPoint)} đã ngắt kết nối: {ioEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi xử lý client: {ex.Message}");
            }
            finally
            {
                CloseConnection();
            }
        }
        private async Task HandleFirstLoginRegisterAsync(string[] parts)
        {
            try
            {
                if (parts.Length < 3) throw new Exception("Thiếu token hoặc SĐT");
                string token = parts[1];
                string phone = parts[2];

                // Xác thực token
                var firebaseUser = await FirebaseAdminService.VerifyTokenAsync(token);

                // Tạo Document trên Firestore
                await FirebaseAdminService.CreateUserDocumentAsync(firebaseUser.Uid, firebaseUser.Email, phone);

                _authenticatedUserId = firebaseUser.Uid;
                await _writer.WriteLineAsync(ProtocolCommands.LOGIN_SUCCESS);
                Console.WriteLine($"Client {firebaseUser.Uid} đã đăng ký và đăng nhập.");
            }
            catch (Exception ex)
            {
                await _writer.WriteLineAsync($"{ProtocolCommands.LOGIN_FAIL}|{ex.Message}");
            }
        }

        private async Task HandleLoginAttemptAsync(string[] parts)
        {
            try
            {
                if (parts.Length < 2) throw new Exception("Thiếu token");
                string token = parts[1];

                // Xác thực token
                var firebaseUser = await FirebaseAdminService.VerifyTokenAsync(token);

                // Kiểm tra/Tạo Document (phòng trường hợp DB bị lỗi)
                await FirebaseAdminService.CheckAndCreateUserAsync(firebaseUser.Uid, firebaseUser.Email);

                _authenticatedUserId = firebaseUser.Uid;
                await _writer.WriteLineAsync(ProtocolCommands.LOGIN_SUCCESS);
                Console.WriteLine($"Client {firebaseUser.Uid} đã đăng nhập.");
            }
            catch (Exception ex)
            {
                await _writer.WriteLineAsync($"{ProtocolCommands.LOGIN_FAIL}|{ex.Message}");
            }
        }
        private void CheckAuthentication()
        {
            if (string.IsNullOrEmpty(_authenticatedUserId))
            {
                throw new Exception("Lỗi bảo mật: Người dùng chưa đăng nhập!");
            }
        }
        // Tạo đường dẫn file an toàn (mỗi user 1 thư mục)
        private string GetSafeFilePath(string fileId)
        {
            string userStoragePath = Path.Combine(_storagePath, _authenticatedUserId);
            Directory.CreateDirectory(userStoragePath); 
            return Path.Combine(userStoragePath, fileId);
        }
        private async Task HandleUploadAsync(string[] parts)
        {
            try
            {
                CheckAuthentication();
                if (parts.Length < 3) throw new Exception("Thiếu tham số UPLOAD");
                string fileId = parts[1];
                long fileSize = long.Parse(parts[2]);
                string filePath = GetSafeFilePath(fileId);

                // 1. Báo client "Sẵn sàng"
                await _writer.WriteLineAsync($"{ProtocolCommands.READY_FOR_UPLOAD}|{fileId}");

                // 2. Nhận dữ liệu file 
                // Phải đọc trực tiếp từ _stream, không qua _reader
                await ReadFileFromStream(filePath, fileSize);

                // 3. Báo client "Thành công"
                Console.WriteLine($"Nhận file {fileId} thành công");
                await _writer.WriteLineAsync($"{ProtocolCommands.UPLOAD_SUCCESS}|{fileId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Upload lỗi: {ex.Message}");
                await _writer.WriteLineAsync($"{ProtocolCommands.UPLOAD_FAIL}|{ex.Message}");
            }
        }

        private async Task HandleDownloadAsync(string[] parts)
        {
            try
            {
                CheckAuthentication();
                if (parts.Length < 2) throw new Exception("Thiếu tham số DOWNLOAD");
                string fileId = parts[1];
                string filePath = GetSafeFilePath(fileId);

                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException("File không tồn tại trên server!", fileId);
                }

                FileInfo fileInfo = new FileInfo(filePath);
                long fileSize = fileInfo.Length;

                // 1. Báo client "Đang gửi" 
                await _writer.WriteLineAsync($"{ProtocolCommands.DOWNLOADING}|{fileSize}");

                // 2. Gửi dữ liệu file 
                // Phải ghi trực tiếp vào _stream, không qua _writer
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    await fileStream.CopyToAsync(_stream);
                }
                Console.WriteLine($"Gửi file {fileId} thành công");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Download lỗi: {ex.Message}");
                await _writer.WriteLineAsync($"{ProtocolCommands.DOWNLOAD_FAIL}|{ex.Message}");
            }
        }

        private async Task HandleDeleteAsync(string[] parts)
        {
            try
            {
                CheckAuthentication();
                if (parts.Length < 2) throw new Exception("Thiếu tham số DELETE_FILE");
                string fileId = parts[1];
                string filePath = GetSafeFilePath(fileId);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    Console.WriteLine($"Đã xoá file {fileId}.");
                    await _writer.WriteLineAsync($"{ProtocolCommands.DELETE_SUCCESS}|{fileId}");
                }
                else
                {
                    throw new FileNotFoundException("File không tồn tại!", fileId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Xoá file lỗi: {ex.Message}");
                await _writer.WriteLineAsync($"{ProtocolCommands.DELETE_FAIL}|{ex.Message}");
            }
        }

        // Hàm trợ giúp đọc file từ stream
        private async Task ReadFileFromStream(string savePath, long fileSize)
        {
            const int bufferSize = 8192;
            byte[] buffer = new byte[bufferSize];
            long bytesRead = 0;

            using (var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write))
            {
                while (bytesRead < fileSize)
                {
                    int read = await _stream.ReadAsync(buffer, 0, (int)Math.Min(buffer.Length, fileSize - bytesRead));
                    if (read == 0)
                    {
                        // Client ngắt kết nối đột ngột
                        throw new IOException("Mất kết nối khi upload file!");
                    }
                    await fileStream.WriteAsync(buffer, 0, read);
                    bytesRead += read;
                }
            }
        }

        private void CloseConnection()
        {
            _reader?.Close();
            _writer?.Close();
            _stream?.Close();
            _client?.Close();
            Console.WriteLine($"Đã đóng kết nối với {(_client.Client.RemoteEndPoint)}");
        }
    }
}
