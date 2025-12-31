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
    }
}
