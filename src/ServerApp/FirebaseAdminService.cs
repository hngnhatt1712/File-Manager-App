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
    public class FileMetadata
    {
        [FirestoreProperty]
        public string FileName { get; set; }
        [FirestoreProperty]
        public string Path { get; set; }
        [FirestoreProperty]
        public long Size { get; set; }
        [FirestoreProperty]
        public string OwnerUid { get; set; }
        [FirestoreProperty]
        public System.DateTime UploadedDate { get; set; }
    }
    public class FirebaseAdminService
    {
        private static readonly FirestoreDb _firestoreDb;

        private const string KeyFileName = "service-account-key.json";

        static FirebaseAdminService()
        {
            string keyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, KeyFileName);

            if (!File.Exists(keyPath))
            {
                throw new FileNotFoundException($"Không tìm thấy file key: '{KeyFileName}'");
            }

            // Khởi tạo Firebase Admin (dùng xác thực)
            GoogleCredential credential = GoogleCredential.FromFile(keyPath);

            FirebaseApp.Create(new AppOptions()
            {
                Credential = credential
            });

            // Lấy project_id từ file key JSON
            string projectId = GetProjectIdFromKeyFile(keyPath);

            // Khởi tạo Firestore (THƯ VIỆN ĐÚNG)
            _firestoreDb = new FirestoreDbBuilder
            {
                ProjectId = projectId,
                Credential = credential
            }.Build();
        }

        // Hàm này để kích hoạt static constructor
        public static void Initialize() { }

        // --- Lấy project_id từ JSON ---
        private static string GetProjectIdFromKeyFile(string path)
        {
            string json = File.ReadAllText(path);
            dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            return data.project_id;
        }

        // ================= FIREBASE AUTH =================

        public static async Task<FirebaseToken> VerifyTokenAsync(string token)
        {
            return await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);
        }

        // ================= FIRESTORE METHODS =================

        public static async Task CreateUserDocumentAsync(string uid, string email, string phoneNumber)
        {
            DocumentReference docRef = _firestoreDb.Collection("users").Document(uid);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

            if (snapshot.Exists)
            {
                if (!snapshot.ContainsField("PhoneNumber"))
                {
                    await docRef.UpdateAsync("PhoneNumber", phoneNumber);
                }
                return;
            }

            var newUser = new
            {
                Email = email,
                PhoneNumber = phoneNumber,
                StorageUsed = 0,
                CreatedAt = Timestamp.FromDateTime(DateTime.UtcNow)
            };

            await docRef.SetAsync(newUser);
            Console.WriteLine($"Đã tạo Document Firestore cho UID: {uid}");
        }
        // Châu Sử
        public async Task CheckAndCreateUserAsync(string uid, string email, string phone)
        {
            // Đảm bảo biến _firestoreDb đã được khởi tạo trong Constructor của class này
            DocumentReference docRef = _firestoreDb.Collection("users").Document(uid);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

            if (!snapshot.Exists)
            {
                var newUser = new
                {
                    Email = email,
                    Phone = phone,
                    StorageUsed = 0,
                    CreatedAt = Timestamp.FromDateTime(DateTime.UtcNow)
                };

                await docRef.SetAsync(newUser);
                Console.WriteLine($"Đã tạo user mới: UID={uid}, Email={email}, Phone={phone}");
            }
            else
            {
                Console.WriteLine($"User {uid} đã tồn tại");
            }

        }
        public async Task<UserServerPayload> GetUserAsync(string uid)
        {
            var doc = await _firestoreDb
                .Collection("users")
                .Document(uid)
                .GetSnapshotAsync();

            if (!doc.Exists) return null;

            return doc.ConvertTo<UserServerPayload>();
        }
        public async Task<List<FileMetadata>> GetFileListAsync(string uid, string path)
        {
            var fileList = new List<FileMetadata>();

            // Truy vấn các file của user 'uid' tại đường dẫn 'path'
            Query query = _firestoreDb.Collection("files")
                .WhereEqualTo("OwnerUid", uid)
                .WhereEqualTo("Path", path);

            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
            foreach (DocumentSnapshot doc in querySnapshot.Documents)
            {
                fileList.Add(doc.ConvertTo<FileMetadata>());
            }
            return fileList;
        }
    }
}
