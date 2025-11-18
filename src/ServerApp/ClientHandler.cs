using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SharedLibrary;
using FirebaseAdmin.Auth;

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

       
    
        public async Task HandleClientAsync()
        {
            try
            {
                _stream = _client.GetStream();
                _reader = new StreamReader(_stream, Encoding.UTF8);
                _writer = new StreamWriter(_stream, Encoding.UTF8) { AutoFlush = true };

                string line;
                while ((line = await _reader.ReadLineAsync()) != null)
                {
                    Console.WriteLine($"Nhận từ {_client.Client.RemoteEndPoint}: {line}");
                    string[] parts = line.Split('|');
                    string command = parts[0].ToUpper();

                    switch (command)
                    {
                        case ProtocolCommands.FIRST_LOGIN_REGISTER:
                            await HandleFirstLoginRegisterAsync(parts);
                            break;

                        case ProtocolCommands.LOGIN_ATTEMPT:
                            await HandleLoginAttemptAsync(parts);
                            break;

                        case ProtocolCommands.PING:
                            await _writer.WriteLineAsync(ProtocolCommands.PONG);
                            break;

                        case ProtocolCommands.UPLOAD:
                            await HandleUploadAsync(parts);
                            break;

                        case ProtocolCommands.DOWNLOAD:
                            await HandleDownloadAsync(parts);
                            break;

                        case ProtocolCommands.DELETE_FILE:
                            await HandleDeleteAsync(parts);
                            break;

                        case ProtocolCommands.QUIT:
                            Console.WriteLine($"Client {_client.Client.RemoteEndPoint} requested QUIT.");
                            return;

                        // KHỐI LOGIC LOGOUT ĐÃ ĐƯỢC XÓA THEO YÊU CẦU MỚI:
                        // case ProtocolCommands.LOGOUT:
                        // Console.WriteLine($"Client {_client.Client.RemoteEndPoint} requested LOGOUT.");
                        // return; 
                        // KẾT THÚC KHỐI LOGOUT ĐÃ XÓA

                        default:
                            await _writer.WriteLineAsync(ProtocolCommands.UNKNOWN_COMMAND);
                            break;
                    }
                }
            }
            catch (IOException)
            {
                Console.WriteLine($"Client {_client.Client.RemoteEndPoint} đã ngắt kết nối.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi client: {ex}");
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

                var decodedToken = await FirebaseAdminService.VerifyTokenAsync(token);

           
                var userRecord = await FirebaseAuth.DefaultInstance.GetUserAsync(decodedToken.Uid);

                await FirebaseAdminService.CreateUserDocumentAsync(userRecord.Uid, userRecord.Email, phone);

                _authenticatedUserId = userRecord.Uid;
                await _writer.WriteLineAsync(ProtocolCommands.LOGIN_SUCCESS);
                Console.WriteLine($"Client {userRecord.Uid} đã đăng ký và đăng nhập.");
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

                var decodedToken = await FirebaseAdminService.VerifyTokenAsync(token);

                var userRecord = await FirebaseAuth.DefaultInstance.GetUserAsync(decodedToken.Uid);

                await FirebaseAdminService.CheckAndCreateUserAsync(userRecord.Uid, userRecord.Email);

                _authenticatedUserId = userRecord.Uid;
                await _writer.WriteLineAsync(ProtocolCommands.LOGIN_SUCCESS);
                Console.WriteLine($"Client {userRecord.Uid} đã đăng nhập.");
            }
            catch (Exception ex)
            {
                await _writer.WriteLineAsync($"{ProtocolCommands.LOGIN_FAIL}|{ex.Message}");
            }
        }

        private void CheckAuthentication()
        {
            if (_authenticatedUserId == null)
                throw new Exception("Chưa đăng nhập!");
        }

        private string GetSafeFilePath(string fileId)
        {
            string userPath = Path.Combine(_storagePath, _authenticatedUserId);
            Directory.CreateDirectory(userPath);
            return Path.Combine(userPath, fileId);
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

                await _writer.WriteLineAsync($"{ProtocolCommands.READY_FOR_UPLOAD}|{fileId}");
                await ReadFileFromStream(filePath, fileSize);

                await _writer.WriteLineAsync($"{ProtocolCommands.UPLOAD_SUCCESS}|{fileId}");
            }
            catch (Exception ex)
            {
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
                    throw new FileNotFoundException("Không tìm thấy file!", fileId);

                long fileSize = new FileInfo(filePath).Length;

                await _writer.WriteLineAsync($"{ProtocolCommands.DOWNLOADING}|{fileSize}");

                using var file = new FileStream(filePath, FileMode.Open);
                await file.CopyToAsync(_stream);
            }
            catch (Exception ex)
            {
                await _writer.WriteLineAsync($"{ProtocolCommands.DOWNLOAD_FAIL}|{ex.Message}");
            }
        }

        private async Task HandleDeleteAsync(string[] parts)
        {
            try
            {
                CheckAuthentication();
                if (parts.Length < 2) throw new Exception("Thiếu tham số DELETE");

                string fileId = parts[1];
                string filePath = GetSafeFilePath(fileId);

                if (!File.Exists(filePath))
                    throw new FileNotFoundException("Không tìm thấy file!", fileId);

                File.Delete(filePath);
                await _writer.WriteLineAsync($"{ProtocolCommands.DELETE_SUCCESS}|{fileId}");
            }
            catch (Exception ex)
            {
                await _writer.WriteLineAsync($"{ProtocolCommands.DELETE_FAIL}|{ex.Message}");
            }
        }

        private async Task ReadFileFromStream(string savePath, long fileSize)
        {
            byte[] buffer = new byte[8192];
            long readTotal = 0;

            using var file = new FileStream(savePath, FileMode.Create);
            while (readTotal < fileSize)
            {
                int read = await _stream.ReadAsync(buffer, 0, buffer.Length);
                if (read == 0) throw new IOException("Client ngắt kết nối!");

                await file.WriteAsync(buffer, 0, read);
                readTotal += read;
            }
        }

        private void CloseConnection()
        {
            _reader?.Close();
            _writer?.Close();
            _stream?.Close();
            _client?.Close();

            Console.WriteLine($"Đóng kết nối {_client.Client.RemoteEndPoint}");
        }
    }
}
