using Google.Cloud.Firestore;
using Newtonsoft.Json;
using SharedLibrary;

namespace ServerApp.Controllers
{
    public class FileController
    {
        // 1. Sửa lại biến thành FirestoreDb (Gọi trực tiếp Database)
        private readonly FirestoreDb _firestoreDb;

        // Constructor nhận vào FirestoreDb (được tiêm từ Program.cs)
        // Hoặc nếu bạn muốn giữ FirebaseAdminService thì xem lưu ý bên dưới
        public FileController(FirestoreDb firestoreDb)
        {
            _firestoreDb = firestoreDb;
        }

        // --- LƯU METADATA ---
        public async Task SaveFileMetadata(FileMetadata metadata)
        {
            try
            {
                // 2. Sửa _firebaseService thành _firestoreDb
                var docRef = _firestoreDb.Collection("Files").Document(metadata.FileId);

                await docRef.SetAsync(metadata);
                Console.WriteLine("Đã lưu metadata vào Firestore thành công.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi lưu Firestore: " + ex.Message);
                throw;
            }
        }

        // --- LẤY DANH SÁCH FILE ---
        public async Task<string> HandleListFilesAsync(string uid, string path)
        {
            try
            {
                // 3. Logic lấy file từ Firestore
                // Query: Lấy tất cả file trong Collection "Files" mà có OwnerUid == uid
                Query query = _firestoreDb.Collection("Files").WhereEqualTo("OwnerUid", uid);

                // Nếu muốn lọc theo path (thư mục) nữa thì mở comment dòng này:
                // if (!string.IsNullOrEmpty(path)) query = query.WhereEqualTo("path", path);

                QuerySnapshot snapshot = await query.GetSnapshotAsync();

                List<FileMetadata> files = new List<FileMetadata>();
                foreach (DocumentSnapshot document in snapshot.Documents)
                {
                    if (document.Exists)
                    {
                        // Convert về Object FileMetadata
                        FileMetadata file = document.ConvertTo<FileMetadata>();
                        files.Add(file);
                    }
                }

                // Chuyển sang JSON
                string json = JsonConvert.SerializeObject(files);
                return $"LIST_FILES_OK|{json}";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi GetFileList: " + ex.Message);
                return $"LIST_FILES_FAIL|{ex.Message}";
            }
        }

        // --- TÍNH TỔNG DUNG LƯỢNG BỘ NHỚ ---
        public async Task<string> HandleGetStorageInfoAsync(string uid, long maxQuota = 5368709120) // Default 5GB
        {
            try
            {
                // 1. Query tất cả file của user từ Firestore
                Query query = _firestoreDb.Collection("Files").WhereEqualTo("OwnerUid", uid).Select("Size");
                QuerySnapshot snapshot = await query.GetSnapshotAsync();

                // 2. Cộng tổng cột Size của tất cả file
                long totalUsed = 0;
                foreach (DocumentSnapshot document in snapshot.Documents)
                {
                    if (document.Exists && document.ContainsField("Size"))
                    {
                        var sizeValue = document.GetValue<long?>("Size");
                        if (sizeValue.HasValue)
                        {
                            totalUsed += sizeValue.Value;
                        }
                    }
                }

                // 3. Tính dung lượng còn trống
                long totalRemaining = maxQuota - totalUsed;

                // 4. Tạo object JSON để gửi về Client
                var storageInfo = new
                {
                    TotalUsed = totalUsed,
                    MaxQuota = maxQuota,
                    TotalRemaining = totalRemaining,
                    UsagePercent = (int)((totalUsed * 100) / maxQuota)
                };

                string json = JsonConvert.SerializeObject(storageInfo);
                Console.WriteLine($"[Storage] User {uid} đã dùng {totalUsed} bytes / {maxQuota} bytes");
                return $"GET_STORAGE_INFO_SUCCESS|{json}";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi GetStorageInfo: " + ex.Message);
                return $"GET_STORAGE_INFO_FAIL|{ex.Message}";
            }
        }

        // --- KIỂM TRA QUOTA TRƯỚC UPLOAD ---
        public async Task<bool> CheckStorageQuotaAsync(string uid, long fileSize, long maxQuota = 5368709120)
        {
            try
            {
                // 1. Query tất cả file của user từ Firestore
                Query query = _firestoreDb.Collection("Files").WhereEqualTo("OwnerUid", uid).Select("Size");
                QuerySnapshot snapshot = await query.GetSnapshotAsync();

                // 2. Cộng tổng cột Size của tất cả file
                long totalUsed = 0;
                foreach (DocumentSnapshot document in snapshot.Documents)
                {
                    if (document.Exists && document.ContainsField("Size"))
                    {
                        var sizeValue = document.GetValue<long?>("Size");
                        if (sizeValue.HasValue)
                        {
                            totalUsed += sizeValue.Value;
                        }
                    }
                }

                // 3. Kiểm tra: dung lượng hiện tại + file mới có vượt quá không
                long totalAfterUpload = totalUsed + fileSize;
                
                if (totalAfterUpload > maxQuota)
                {
                    Console.WriteLine($"[Quota Check] User {uid}: FAIL - {totalAfterUpload} > {maxQuota}");
                    return false; // Không đủ dung lượng
                }

                Console.WriteLine($"[Quota Check] User {uid}: OK - {totalAfterUpload} <= {maxQuota}");
                return true; // Đủ dung lượng
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi CheckStorageQuota: " + ex.Message);
                return false; // Nếu lỗi, từ chối upload để safety
            }
        }
    }
}
