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

namespace ServerApp
{
    [FirestoreData]
    public class FileMetadata
    {
        [FirestoreProperty] public string FileId { get; set; }
        [FirestoreProperty] public string FileName { get; set; }
        [FirestoreProperty] public long Size { get; set; }
        [FirestoreProperty] public string OwnerUid { get; set; }
        [FirestoreProperty] public string StoragePath { get; set; }
        [FirestoreProperty] public string Path { get; set; }
        [FirestoreProperty] public string UploadedDate { get; set; }
    }
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
    }
}
