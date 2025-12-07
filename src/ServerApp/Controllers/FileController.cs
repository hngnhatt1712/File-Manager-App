using Newtonsoft.Json;
namespace ServerApp.Controllers
{
    public class FileController
    {
        private readonly FirebaseAdminService _firebaseService;

        public FileController(FirebaseAdminService firebaseService)
        {
            _firebaseService = firebaseService;
        }

        // --- LƯU METADATA SAU KHI UPLOAD XONG ---
        public void SaveFileMetadata(string uid, string fileName, long fileSize, string savePath)
        {
            try
            {
                // 1. Tạo object dữ liệu
                var metadata = new FileMetadata
                {
                    FileName = fileName,
                    Size = fileSize,
                    OwnerUid = uid,
                    StoragePath = savePath,
                    Path = "/", // Mặc định thư mục gốc
                    UploadedDate = DateTime.UtcNow.ToString("o")
                };

                // 2. Gọi Service để đẩy lên Firestore
                _firebaseService.AddFileMetadataAsync(metadata).Wait();

                Console.WriteLine($"[Firestore] Đã lưu metadata cho file: {fileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Firestore Error] Không thể lưu metadata: {ex.Message}");
                throw;
            }
        }

        // --- LẤY DANH SÁCH FILE ---
        public async Task<string> HandleListFilesAsync(string uid, string path)
        {
            try
            {
                // 1. Lấy danh sách từ Firestore 
                List<FileMetadata> files = await _firebaseService.GetFileListAsync(uid, path);

                // 2. Chuyển sang JSON
                string json = JsonConvert.SerializeObject(files);

                // 3. Đóng gói theo protocol
                return $"LIST_FILES_OK|{json}";
            }
            catch (Exception ex)
            {
                return $"LIST_FILES_FAIL|{ex.Message}";
            }
        }

        // --- LẤY THÔNG TIN ĐỂ DOWNLOAD ---
        // Viết hàm trả về FileModel để ClientHandler biết đường dẫn file nằm ở đâu trên ổ cứng
    }
}
