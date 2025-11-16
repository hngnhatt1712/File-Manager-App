using System;
using System.IO;
using System.Threading.Tasks;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;

namespace ServerApp
{
    internal static class FirebaseAdminService
    {
        private static readonly FirestoreDb _firestoreDb;

        private const string KeyFileName = "service-account-key.json";

        static FirebaseAdminService()
        {
            string keyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, KeyFileName);

            if (!File.Exists(keyPath))
            {
                throw new FileNotFoundException($"Không tìm thấy file key: '{KeyFileName}'. " +
                    "Hãy đảm bảo bạn đã:\n" +
                    "1. Đặt file JSON đúng vào project.\n" +
                    "2. Chọn 'Copy to Output Directory' = Copy if newer.");
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

        public static async Task CheckAndCreateUserAsync(string uid, string email)
        {
            DocumentReference docRef = _firestoreDb.Collection("users").Document(uid);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

            if (!snapshot.Exists)
            {
                var newUser = new
                {
                    Email = email,
                    StorageUsed = 0,
                    CreatedAt = Timestamp.FromDateTime(DateTime.UtcNow)
                };

                await docRef.SetAsync(newUser);
                Console.WriteLine($"Đã tự động tạo Document Firestore (thiếu SĐT) cho UID: {uid}");
            }
        }
    }
}