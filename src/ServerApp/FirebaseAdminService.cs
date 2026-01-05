using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedLibrary;

namespace ServerApp
{
    
    public class FirebaseAdminService
    {
        private readonly FirestoreDb _firestoreDb;

        private const string KeyFileName = "service-account-key.json";


        public FirebaseAdminService()
        {
            string keyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, KeyFileName);

            if (!File.Exists(keyPath))
            {
                throw new FileNotFoundException($"Không tìm thấy file key: '{KeyFileName}'");
            }

            // Khởi tạo Firebase Admin (dùng xác thực)
            GoogleCredential credential = GoogleCredential.FromFile(keyPath);

            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions() { Credential = credential });
            }

            // Lấy project_id từ file key JSON
            string projectId = GetProjectIdFromKeyFile(keyPath);

            // Khởi tạo Firestore 
            _firestoreDb = new FirestoreDbBuilder
            {
                ProjectId = projectId,
                Credential = credential
            }.Build();
        }

        // Hàm này để kích hoạt static constructor
        public static void Initialize() { }

        // --- Lấy project_id từ JSON ---
        private string GetProjectIdFromKeyFile(string path)
        {
            string json = File.ReadAllText(path);
            dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            return data.project_id;
        }

        // ================= FIREBASE AUTH =================

        public async Task<FirebaseToken> VerifyTokenAsync(string token)
        {
            try
            {
                // Gọi Auth của Admin SDK
                return await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);
            }
            catch
            {
                return null; // Token lỗi hoặc hết hạn
            }
        }

        // ================= FIRESTORE METHODS =================

        public async Task CheckAndCreateUserAsync(string uid, string email, string phone)
        {
            try
            {
                DocumentReference docRef = _firestoreDb.Collection("users").Document(uid);
                DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

                if (!snapshot.Exists)
                {
                    var newUser = new Dictionary<string, object>
                    {
                        { "email", email },
                        { "phoneNumber", phone },
                        { "storageUsed", 0 },
                        { "storageCap", 1073741824 }, // 1GB
                        { "createdAt", DateTime.UtcNow }
                    };

                    await docRef.SetAsync(newUser);
                    Console.WriteLine($"[Firestore] Created new user: {email}");
                }
                else
                {
                    // Nếu user đã tồn tại nhưng chưa có SĐT 
                    if (!snapshot.ContainsField("PhoneNumber") && !string.IsNullOrEmpty(phone))
                    {
                        await docRef.UpdateAsync("PhoneNumber", phone);
                        Console.WriteLine($"[Firestore] Updated phone for user: {email}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Firestore Error] CheckAndCreateUser: {ex.Message}");
                throw;
            }
        }
        public async Task<UserServerPayload> GetUserAsync(string uid)
        {
            try
            {
                var doc = await _firestoreDb.Collection("users").Document(uid).GetSnapshotAsync();
                if (!doc.Exists) return null;
                var data = doc.ToDictionary();
                return new UserServerPayload
                {
                    Uid = uid,
                    Email = data.ContainsKey("Email") ? data["Email"].ToString() : "",
                    Phone = data.ContainsKey("PhoneNumber") ? data["PhoneNumber"].ToString() : ""
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Firestore Error] GetUser: {ex.Message}");
                return null;
            }
        }

        // ================= FILE MANAGEMENT =================

        // 1. [UPLOAD] Hàm lưu metadata file sau khi Server nhận xong file vật lý
        public async Task AddFileMetadataAsync(FileMetadata fileData)
        {
            try
            {
                CollectionReference col = _firestoreDb.Collection("files");

                var data = new Dictionary<string, object>
            {
                { "fileName", fileData.FileName },
                { "size", fileData.Size },
                { "ownerUid", fileData.OwnerUid },
                { "storagePath", fileData.StoragePath }, 
                { "path", fileData.Path ?? "/" },
                { "uploadedDate", DateTime.UtcNow.ToString("o") }
            };

                await col.AddAsync(data);
                Console.WriteLine($"[Firestore] Saved metadata for {fileData.FileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] AddFileMetadataAsync: {ex.Message}");
                throw;
            }
        }

        // 2. [DOWNLOAD] Hàm lấy thông tin file theo ID (để biết đường dẫn StoragePath)

        // 3. [LIST] Hàm lấy danh sách file 
        public async Task<List<FileMetadata>> GetFileListAsync(string uid, string path)
        {
            try
            {
                // Query theo cả UID và Path
                Query query = _firestoreDb.Collection("files")
                    .WhereEqualTo("ownerUid", uid)
                    .WhereEqualTo("path", path); // Lọc file trong thư mục cụ thể

                QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

                var fileList = new List<FileMetadata>();
                foreach (DocumentSnapshot doc in querySnapshot.Documents)
                {
                    if (doc.Exists)
                    {
                        var fileData = doc.ConvertTo<FileMetadata>();
                        fileData.FileId = doc.Id;
                        fileList.Add(fileData);
                    }
                }
                return fileList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Firestore Error] GetFileList: {ex.Message}");
                return new List<FileMetadata>();
            }
        }
        public class UserServerPayload
        {
            public string Uid { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
        }


        // hàm tìm file theo user
        public async Task<List<FileMetadata>> SearchFilesAsync(string uid, string searchTerm)
        {
            try
            {
                // z: Lấy tất cả file thuộc sở hữu của User này
                Query query = _firestoreDb.Collection("files").WhereEqualTo("ownerUid", uid);
                QuerySnapshot snapshot = await query.GetSnapshotAsync();

                var ketQua = new List<FileMetadata>();
                foreach (DocumentSnapshot doc in snapshot.Documents)
                {
                    var file = doc.ConvertTo<FileMetadata>();
                    file.FileId = doc.Id;
                    // z: Lọc những file có tên chứa từ khóa (không phân biệt hoa thường)
                    if (file.FileName.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        ketQua.Add(file);
                    }
                }
                return ketQua;
            }
            catch { return new List<FileMetadata>(); }
        }

        // thay đổi trạng thái hiển thị
        public async Task<bool> UpdateDeletedStatusAsync(string fileId, bool isDeleted)
        {
            try
            {
                DocumentReference docRef = _firestoreDb.Collection("files").Document(fileId);
                Dictionary<string, object> updates = new Dictionary<string, object>
        {
            { "IsDeleted", isDeleted }
        };
                await docRef.UpdateAsync(updates);
                return true;
            }
            catch { return false; }
        }

        // 2. Hàm lấy riêng danh sách trong Thùng rác
        public async Task<List<FileMetadata>> GetTrashFilesAsync(string uid)
        {
            try
            {
                // 1. Chỉ lấy những file của User đó VÀ đã bị đánh dấu IsDeleted = true
                Query query = _firestoreDb.Collection("files")
                    .WhereEqualTo("ownerUid", uid)
                    .WhereEqualTo("IsDeleted", true);

                QuerySnapshot snapshot = await query.GetSnapshotAsync();

                var ketQua = new List<FileMetadata>();
                foreach (DocumentSnapshot doc in snapshot.Documents)
                {
                    var file = doc.ConvertTo<FileMetadata>();
                    file.FileId = doc.Id;
                    ketQua.Add(file);
                }
                return ketQua;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi lấy thùng rác: " + ex.Message);
                return new List<FileMetadata>();
            }
        }

        //Rename

        public async Task<bool> RenameFileDBAsync(string fileId, string newName)
        {
            try
            {
                Console.WriteLine($"[DB Debug] Đang tìm ID: {fileId} để đổi tên thành: {newName}");

                // Kiểm tra biến DB
                if (_firestoreDb == null)
                {
                    Console.WriteLine("[DB Error] Biến _firestoreDb bị NULL!");
                    return false;
                }

                DocumentReference docRef = _firestoreDb.Collection("Files").Document(fileId);

                // Kiểm tra xem file có tồn tại không
                DocumentSnapshot snap = await docRef.GetSnapshotAsync();
                if (!snap.Exists)
                {
                    Console.WriteLine($"[DB Error] Không tìm thấy ID {fileId} trong Collection 'Files'.");
                    return false;
                }

                await docRef.UpdateAsync("FileName", newName);

                Console.WriteLine("[DB Success] Update thành công!");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DB EXCEPTION] Lỗi chi tiết: {ex.Message}");
                return false;
            }
        }

        //Đánh dấu
        public async Task<bool> ToggleStarDBAsync(string fileId, bool currentStatus)
        {
            try
            {
                DocumentReference docRef = _firestoreDb.Collection("Files").Document(fileId); 

                await docRef.UpdateAsync("IsStarred", !currentStatus);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[Star DB Error] " + ex.Message);
                return false;
            }
        }
    }
}
