using Newtonsoft.Json;
using SharedLibrary;
using System;
using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using SharedLibrary;
using System.Collections.Generic;

public class FileTransferClient
{
    private TcpClient _tcpClient;
    private NetworkStream _stream;
    private StreamReader _reader;
    private StreamWriter _writer;
    private bool _isAuthenticated = false;  // Track xem đã SYNC_USER chưa
    private string _currentToken = null;    // Lưu token hiện tại
    private string _currentEmail = null;    // Lưu email hiện tại
    private string _currentPhone = null;    // Lưu phone hiện tại
    
    // SEMAPHORE: Chống Race Condition - đảm bảo chỉ 1 lệnh được gửi/nhận cùng lúc
    private readonly SemaphoreSlim _networkLock = new SemaphoreSlim(1, 1);

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
        await _networkLock.WaitAsync(); // 🔒 Khóa
        try
        {
            await EnsureConnectedAsync();
            await _writer.WriteLineAsync("DELETE_ACCOUNT");
            await _writer.FlushAsync();

            string response = await _reader.ReadLineAsync();
            return response == "DELETE_SUCCESS";
        }
        catch { return false; }
        finally
        {
            _networkLock.Release();
        }
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
        await _networkLock.WaitAsync();
        try
        {
            await _writer.WriteLineAsync(ProtocolCommands.GET_TRASH_FILES);
            await _writer.FlushAsync();

            string json = await _reader.ReadLineAsync();
            return JsonConvert.DeserializeObject<List<FileMetadata>>(json) ?? new List<FileMetadata>();
        }
        finally { _networkLock.Release(); }
    }


    // để gửi tên file lên Server
    public async Task<List<FileMetadata>> SearchFilesAsync(string query)
    {
        await _networkLock.WaitAsync(); // 🔒 Khóa
        try
        {
            await EnsureConnectedAsync();
            await _writer.WriteLineAsync(ProtocolCommands.SEARCH_REQ);
            await _writer.FlushAsync();
            await _writer.WriteLineAsync(query);
            await _writer.FlushAsync();

            string response = await _reader.ReadLineAsync();
            if (response == ProtocolCommands.SEARCH_SUCCESS)
            {
                string json = await _reader.ReadLineAsync();
                return JsonConvert.DeserializeObject<List<FileMetadata>>(json);
            }
            return new List<FileMetadata>();
        }
        finally
        {
            _networkLock.Release();
        }
    }



    // Lấy thông tin dung lượng bộ nhớ từ Server
    public async Task<StorageInfo> GetStorageInfoAsync(int maxRetries = 1)
    {
        int retry = 0;
        while (true)
        {
            await _networkLock.WaitAsync(); // 🔒 Khóa
            bool lockAcquired = true; // Track if we still hold the lock
            try
            {
                await EnsureConnectedAsync();

                // 1. Gửi lệnh
                await _writer.WriteLineAsync(ProtocolCommands.GET_STORAGE_INFO);
                await _writer.FlushAsync();

                // 2. Đọc phản hồi
                string response = await _reader.ReadLineAsync();
                if (string.IsNullOrEmpty(response)) throw new Exception("Server không phản hồi");

                string[] parts = response.Split(new[] { '|' }, 2);

                // 3. Xử lý thành công
                if (parts[0] == ProtocolCommands.GET_STORAGE_INFO_SUCCESS && parts.Length > 1)
                {
                    if (!_isAuthenticated) _isAuthenticated = true; // Cập nhật trạng thái
                    return JsonConvert.DeserializeObject<StorageInfo>(parts[1]);
                }

                // 4. Xử lý khi chưa đăng nhập (Server báo Access Denied)
                if (parts[0] == ProtocolCommands.ACCESS_DENIED)
                {
                    _isAuthenticated = false;
                    if (retry < maxRetries)
                    {
                        Console.WriteLine($"[Storage] Token hết hạn, đang thử đăng nhập lại (Lần {retry + 1})...");
                        // Giải phóng khóa để gọi hàm SyncUser
                        _networkLock.Release();
                        lockAcquired = false; // Mark that we released it
                        try
                        {
                            await SyncUserAsync(_currentToken, _currentEmail, _currentPhone);
                            retry++;
                            continue; // Quay lại đầu vòng lặp while để thử lại
                        }
                        finally
                        {
                            // SyncUser đã gửi lệnh riêng, vòng lặp sẽ WaitAsync lại
                        }
                    }
                    throw new Exception("Từ chối truy cập: Vui lòng đăng nhập lại.");
                }

                // Các lỗi khác
                throw new Exception($"Lỗi Server: {response}");
            }
            catch
            {
                throw; // Ném lỗi ra ngoài để UploadFileAsync biết mà dừng lại
            }
            finally
            {
                // Chỉ Release nếu ta vẫn giữ khóa
                if (lockAcquired) _networkLock.Release();
            }
        }


    }

    // vẫn chuyển lệnh từ client sang server 
    private async Task<bool> SendCommandAsync(string command)
    {
        await _networkLock.WaitAsync(); // 🔒 Khóa
        try
        {
            await EnsureConnectedAsync();
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
        finally
        {
            _networkLock.Release();
        }
    }

    // Đăng ký user lên Server
    public async Task<bool> SyncUserAsync(string token, string email, string phone)
    {
        await _networkLock.WaitAsync(); // 🔒 Khóa
        try
        {
            await EnsureConnectedAsync();

            string command = $"SYNC_USER|{token}|{email}|{phone}";
            await _writer.WriteLineAsync(command);
            await _writer.FlushAsync();

            string response = await _reader.ReadLineAsync();

            if (response != null && response.StartsWith("SYNC_OK"))
            {
                // Lưu credentials để dùng lại nếu cần
                _isAuthenticated = true;
                _currentToken = token;
                _currentEmail = email;
                _currentPhone = phone;
                Console.WriteLine("[Auth] Đã SYNC_USER thành công");
                return true;
            }

            throw new Exception($"Lỗi đồng bộ Server: {response}");
        }
        finally
        {
            _networkLock.Release();
        }
    }

    // 1. Chuyển vào thùng rác
    // 2. Khôi phục file
    public async Task<bool> RestoreFileAsync(string fileId)
    {
        await _networkLock.WaitAsync();
        try
        {
            await _writer.WriteLineAsync(ProtocolCommands.RESTORE_FILE);
            await _writer.WriteLineAsync(fileId);
            await _writer.FlushAsync();

            string response = await _reader.ReadLineAsync();
            return response == ProtocolCommands.RESTORE_SUCCESS;
        }
        catch { return false; }
        finally { _networkLock.Release(); }
    }

    // 3. Xóa vĩnh viễn (Tái sử dụng lệnh xóa cũ)
    public async Task<bool> DeletePermanentlyAsync(string fileId)
    {
        // Giả sử bạn đã có hàm DeleteFileAsync gửi lệnh DELETE_FILE
        // Nếu chưa có, hãy cho tôi biết code xóa cũ của bạn
        return await DeleteFileAsync(fileId);
    }

    // hàm xóa file

    public async Task<bool> DeleteFileAsync(string fileId)
    {
        await _networkLock.WaitAsync(); // Chờ đến lượt gửi (tránh gửi chồng chéo)
        try
        {
            // 1. Gửi lệnh XÓA
            await _writer.WriteLineAsync(ProtocolCommands.DELETE_FILE);

            // 2. Gửi ID file cần xóa
            await _writer.WriteLineAsync(fileId);
            await _writer.FlushAsync();

            // 3. Đọc phản hồi từ Server
            string response = await _reader.ReadLineAsync();

            return response == ProtocolCommands.DELETE_SUCCESS;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Delete Error] {ex.Message}");
            return false;
        }
        finally
        {
            _networkLock.Release(); // Giải phóng để lệnh khác được chạy
        }
    }

    // Trong file: FileTransferClient.cs

    public async Task<bool> MoveToTrashAsync(string fileId)
    {
        await _networkLock.WaitAsync(); // Dùng lock để tránh xung đột luồng
        try
        {
            await EnsureConnectedAsync();

            // Gửi lệnh theo định dạng: COMMAND|ID
            string command = $"{ProtocolCommands.MOVE_TO_TRASH}|{fileId}";
            await _writer.WriteLineAsync(command);
            await _writer.FlushAsync();

            // Đọc phản hồi từ Server
            string response = await _reader.ReadLineAsync();

            // Kiểm tra xem Server có trả về SUCCESS không
            if (response == ProtocolCommands.MOVE_TO_TRASH_SUCCESS)
            {
                return true;
            }
            else
            {
                Console.WriteLine($"[MoveToTrash] Lỗi: {response}");
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Client Error] {ex.Message}");
            return false;
        }
        finally
        {
            _networkLock.Release();
        }
    }

    // Hàm này đảm bảo đã xác thực trước khi làm gì đó
    private async Task EnsureAuthenticatedAsync()
    {
        if (_isAuthenticated)
        {
            Console.WriteLine("[Auth] ✓ Đã xác thực");
            return;
        }
        
        // Nếu chưa xác thực nhưng có credentials, tự động sync lại
        if (!string.IsNullOrEmpty(_currentToken))
        {
            Console.WriteLine("[Auth] Chưa xác thực, cố gắng sync với credentials đã lưu...");
            try
            {
                await SyncUserAsync(_currentToken, _currentEmail, _currentPhone);
                Console.WriteLine("[Auth] ✓ Sync thành công");
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Auth] ✗ Lỗi xác thực lại: {ex.Message}");
                _isAuthenticated = false;
                // Throw exception để caller biết phải xử lý
                throw new Exception($"Lỗi xác thực: {ex.Message}");
            }
        }
        
        // Nếu không có credentials, throw exception
        Console.WriteLine("[Auth] ✗ Chưa đăng nhập");
        throw new Exception("Chưa đăng nhập! Vui lòng đăng nhập trước.");
    }

    #region API File Metadata (Quản lý thông tin file)

    public async Task<string> GetFileListAsync(string uid, string path = "/")
    {
        await _networkLock.WaitAsync(); // 🔒 Khóa để tránh race condition với GetStorageInfoAsync
        try
        {
            await EnsureConnectedAsync();

            // 1. Gửi lệnh
            await _writer.WriteLineAsync($"{ProtocolCommands.LIST_FILES}|{uid}|{path}");
            await _writer.FlushAsync();

            // 2. Đọc phản hồi
            string response = await _reader.ReadLineAsync();
            if (string.IsNullOrEmpty(response)) return "[]";

            string[] parts = response.Split(new[] { '|' }, 2);
            if (parts[0] == "LIST_FILES_OK" && parts.Length > 1) return parts[1];

            return "[]";
        }
        finally
        {
            _networkLock.Release();
        }
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
        await EnsureConnectedAsync();

        FileInfo fi = new FileInfo(localFilePath);
        if (!fi.Exists) throw new FileNotFoundException("File not found");

        // --- BƯỚC PRE-0: ĐẢM BẢO ĐÃ SYNC_USER (XÁC THỰC) ---
        Console.WriteLine("[Upload] Kiểm tra xác thực...");
        if (!_isAuthenticated)
        {
            Console.WriteLine("[Upload] Chưa xác thực, cố gắng sync...");
            if (string.IsNullOrEmpty(_currentToken))
            {
                throw new Exception("Lỗi: Chưa đăng nhập! Vui lòng đăng nhập trước.");
            }
            
            try
            {
                await SyncUserAsync(_currentToken, _currentEmail, _currentPhone);
                Console.WriteLine("[Upload] ✓ Xác thực thành công");
            }
            catch (Exception syncEx)
            {
                throw new Exception($"Lỗi xác thực: {syncEx.Message}");
            }
        }

        // --- BƯỚC 0: KIỂM TRA QUOTA TRƯỚC KHI UPLOAD ---
        Console.WriteLine("[Upload] Kiểm tra dung lượng có sẵn...");
        
        StorageInfo storageInfo = null;
        bool quotaCheckFailed = false;

        try
        {
            // Gọi hàm kiểm tra (đã sửa ở trên)
            storageInfo = await GetStorageInfoAsync();

            // Logic kiểm tra dung lượng
            if (storageInfo.TotalUsed + fi.Length > storageInfo.MaxQuota)
            {
                throw new Exception($"Không đủ dung lượng! Còn trống {storageInfo.TotalRemaining} bytes.");
            }
        }
        catch (Exception ex)
        {
            // 🛑 QUAN TRỌNG: NẾU LỖI THÌ PHẢI DỪNG NGAY!
            // Không được nuốt lỗi (swallow) vì luồng mạng (Stream) có thể đang bị lệch pha
            Console.WriteLine($"[Upload] Lỗi kiểm tra bộ nhớ: {ex.Message}");
            throw new Exception($"Không thể upload vì lỗi kiểm tra bộ nhớ: {ex.Message}");
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

        // LOCK: BẮT ĐẦU KHÓA TỪ ĐÂY - Toàn bộ quá trình upload (gửi metadata + file data + nhận kết quả)
        // KHÔNG khóa toàn bộ hàm vì bên trong có gọi GetStorageInfoAsync (nó cũng cần khóa -> Deadlock)
        await _networkLock.WaitAsync();
        try
        {
            // 3. Gửi lệnh theo cấu trúc mới: UPLOAD_REQ | {JSON}
            // QUAN TRỌNG: Phải flush _writer TRƯỚC khi gửi binary data
            string uploadCommand = $"{ProtocolCommands.UPLOAD_REQ}|{jsonMeta}";
            await _writer.WriteLineAsync(uploadCommand);
            await _writer.FlushAsync(); // Đẩy dữ liệu đi ngay
            
            Console.WriteLine($"[Upload] Gửi UPLOAD_REQ: {uploadCommand}");

            // B2: Chờ READY (Giữ nguyên)
            string response = await _reader.ReadLineAsync();
            Console.WriteLine($"[Upload] Nhận phản hồi: {response}");
            
            if (response != ProtocolCommands.READY_FOR_UPLOAD)
                throw new Exception($"Server refused: {response}");

            // B3: Gửi Stream (QUAN TRỌNG: Phải gửi file binary đầy đủ)
            Console.WriteLine($"[Upload] Bắt đầu gửi file data: {fi.Length} bytes...");
            Console.WriteLine($"[Upload] File path: {localFilePath}");
            Console.WriteLine($"[Upload] File exists: {File.Exists(localFilePath)}");
            Console.WriteLine($"[Upload] File info - Size: {fi.Length}, Exists: {fi.Exists}, FullPath: {fi.FullName}");
            
            long totalSent = 0;
            
            try
            {
                Console.WriteLine($"[Upload] Attempting to open FileStream...");
                
                // Retry logic nếu file bị lock
                FileStream fs = null;
                int retries = 3;
                while (retries > 0)
                {
                    try
                    {
                        fs = new FileStream(localFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                        break;
                    }
                    catch (IOException ioex) when (retries > 1)
                    {
                        Console.WriteLine($"[Upload] ⚠ File busy, retrying... ({retries - 1} retries left)");
                        await Task.Delay(500);
                        retries--;
                    }
                }
                
                if (fs == null)
                {
                    throw new Exception($"Không thể mở file sau {3} lần thử");
                }
                
                using (fs)
                {
                    Console.WriteLine($"[Upload] ✓ File stream opened, size: {fs.Length} bytes");
                    
                    byte[] buffer = new byte[8192];
                    int read;
                    
                    Console.WriteLine($"[Upload] Entering read loop...");
                    while ((read = await fs.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        Console.WriteLine($"[Upload] Read {read} bytes from file");
                        await _stream.WriteAsync(buffer, 0, read);
                        totalSent += read;
                        
                        // Log tiến độ mỗi 100KB
                        if (totalSent % 102400 == 0 || totalSent == fi.Length)
                        {
                            Console.WriteLine($"[Upload] Đã gửi: {totalSent}/{fi.Length} bytes");
                        }
                    }
                    
                    Console.WriteLine($"[Upload] File read loop done. Total sent: {totalSent} bytes");
                }
                
                // QUAN TRỌNG: Phải flush network stream
                Console.WriteLine($"[Upload] Flushing stream...");
                await _stream.FlushAsync();
                Console.WriteLine($"[Upload] ✓ Gửi xong file data. Tổng: {totalSent} bytes");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Upload] ✗ Lỗi khi gửi file: {ex.Message}");
                Console.WriteLine($"[Upload] Exception type: {ex.GetType().Name}");
                Console.WriteLine($"[Upload] Stack: {ex.StackTrace}");
                throw new Exception($"Lỗi gửi file data: {ex.Message}");
            }

            // B4: Chờ SUCCESS (Giữ nguyên)
            string result = await _reader.ReadLineAsync();
            Console.WriteLine($"[Upload] Nhận kết quả: {result}");

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
        finally
        {
            // UNLOCK: Luôn phải mở khóa, dù có lỗi hay không
            _networkLock.Release();
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
