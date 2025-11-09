using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SharedLibrary;

namespace ClientApp
{
    internal class FileTransferClient
    {
        private TcpClient _client;
        private NetworkStream _stream;
        private StreamReader _reader;
        private StreamWriter _writer;

        public bool IsConnected => _client?.Connected ?? false;

        public async Task ConnectAsync(string ip, int port)
        {
            if (IsConnected) return;
            try
            {
                _client = new TcpClient();
                await _client.ConnectAsync(ip, port);
                _stream = _client.GetStream();
                _reader = new StreamReader(_stream, Encoding.UTF8);
                _writer = new StreamWriter(_stream, Encoding.UTF8) { AutoFlush = true };
            }
            catch (Exception ex)
            {
                // Ném lỗi ra để UI hiển thị
                throw new Exception($"Không thể kết nối đến server: {ex.Message}");
            }
        }

        public async Task DisconnectAsync()
        {
            if (!IsConnected) return;
            try
            {
                await _writer.WriteLineAsync(ProtocolCommands.QUIT);
            }
            catch { /* Bỏ qua lỗi nếu không gửi được */ }

            _reader?.Close();
            _writer?.Close();
            _stream?.Close();
            _client?.Close();
        }

        public async Task<bool> UploadFileAsync(string fileId, string localFilePath)
        {
            if (!IsConnected) throw new Exception("Chưa kết nối đến server.");

            FileInfo fileInfo = new FileInfo(localFilePath);
            if (!fileInfo.Exists) throw new FileNotFoundException("File nội bộ không tồn tại!", localFilePath);

            long fileSize = fileInfo.Length;

            // 1. Gửi lệnh UPLOAD
            await _writer.WriteLineAsync($"{ProtocolCommands.UPLOAD}|{fileId}|{fileSize}");

            // 2. Chờ phản hồi READY
            string response = await _reader.ReadLineAsync();
            if (response == null) throw new IOException("Server ngắt kết nối!");

            string[] parts = response.Split('|');
            if (parts[0] != ProtocolCommands.READY_FOR_UPLOAD)
            {
                throw new Exception($"Server không sẵn sàng: {response}");
            }

            // 3. Gửi file 
            using (var fileStream = new FileStream(localFilePath, FileMode.Open, FileAccess.Read))
            {
                await fileStream.CopyToAsync(_stream);
            }

            // 4. Chờ phản hồi UPLOAD_SUCCESS
            string successResponse = await _reader.ReadLineAsync();
            if (successResponse == null) throw new IOException("Server ngắt kết nối sau khi upload!");

            parts = successResponse.Split('|');
            return parts[0] == ProtocolCommands.UPLOAD_SUCCESS;
        }

        public async Task<bool> DownloadFileAsync(string fileId, string savePath)
        {
            if (!IsConnected) throw new Exception("Chưa kết nối đến server!");

            // 1. Gửi lệnh DOWNLOAD
            await _writer.WriteLineAsync($"{ProtocolCommands.DOWNLOAD}|{fileId}");

            // 2. Chờ phản hồi DOWNLOADING
            string response = await _reader.ReadLineAsync();
            if (response == null) throw new IOException("Server ngắt kết nối!");

            string[] parts = response.Split('|');
            if (parts[0] != ProtocolCommands.DOWNLOADING)
            {
                throw new Exception($"Server báo lỗi: {response}");
            }

            // 3. Nhận file 
            long fileSize = long.Parse(parts[1]);
            await ReadFileFromStream(savePath, fileSize);

            return true; // Download thành công nếu không có exception
        }

        public async Task<bool> DeleteFileAsync(string fileId)
        {
            if (!IsConnected) throw new Exception("Chưa kết nối đến server!");

            // 1. Gửi lệnh DELETE_FILE
            await _writer.WriteLineAsync($"{ProtocolCommands.DELETE_FILE}|{fileId}");

            // 2. Chờ phản hồi
            string response = await _reader.ReadLineAsync();
            if (response == null) throw new IOException("Server ngắt kết nối!");

            string[] parts = response.Split('|');
            if (parts[0] == ProtocolCommands.DELETE_SUCCESS)
            }
            
            throw new Exception($"Server báo lỗi xoá file: {response}");
        }
        // Hàm trợ giúp
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
                        throw new IOException("Mất kết nối khi download file!");
                    }
                    await fileStream.WriteAsync(buffer, 0, read);
                    bytesRead += read;
                }
            }
        }
    }
}
