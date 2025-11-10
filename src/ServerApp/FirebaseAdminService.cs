using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Cloud.Firestore;
using Google.Apis.Auth.OAuth2;

namespace ServerApp
{
    internal class FirebaseAdminService
    {
        private static readonly FirestoreDb _firestoreDb;
        static FirebaseAdminService()
        {
            // 1. Lấy đường dẫn tuyệt đối đến file key
            string keyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "service-account-key.json");

            if (!File.Exists(keyPath))
            {
                throw new FileNotFoundException("Không tìm thấy file 'service-account-key.json'!");
            }
            // 2. Khởi tạo Firebase App
            GoogleCredential credential;
            using (var stream = new FileStream(keyPath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream);
            }
            FirebaseApp.Create(new AppOptions()
            {
                Credential = credential
            });
            // 3. Khởi tạo Firestore DB
            string projectId = GetProjectIdFromKeyFile(keyPath);

            // 4️. Tạo instance FirestoreDb
            _firestoreDb = FirestoreDb.Create(projectId);
        }
        private static string GetProjectIdFromKeyFile(string keyPath)
        {
            using var reader = new StreamReader(keyPath);
            var json = System.Text.Json.JsonDocument.Parse(reader.ReadToEnd());
            return json.RootElement.GetProperty("project_id").GetString();
        }
        public static FirestoreDb GetDb() => _firestoreDb;
        public static void Initialize()
        {
            // Hàm này rỗng, nhưng việc gọi nó sẽ kích hoạt hàm 'static FirebaseAdminService()'  chạy
        }

        /// Xác thực token (JWT) mà Client gửi lên
        /// Nếu token hợp lệ, trả về thông tin user
        /// Nếu không, ném ra một Exception
        public static async Task<FirebaseToken> VerifyTokenAsync(string token)
        {
            // Hàm này sẽ tự động kiểm tra token hết hạn, sai chữ ký,..
            return await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);
        }

        /// Tạo Document cho user trên Firestore
        public static async Task CreateUserDocumentAsync(string uid, string email, string phoneNumber)
        {
            // Trỏ đến document của user
            DocumentReference docRef = _firestoreDb.Collection("users").Document(uid);

            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
            if (snapshot.Exists)
            {
                // User đã tồn tại 
                // Chỉ cập nhật SĐT nếu nó đang trống
                if (!snapshot.ContainsField("PhoneNumber"))
                {
                    await docRef.UpdateAsync("PhoneNumber", phoneNumber);
                }
                return;
            }

            // Tạo mới user document
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

        /// Kiểm tra và tạo user 
        /// Đề phòng trường hợp user đã có trong Auth nhưng chưa có trong DB
        public static async Task CheckAndCreateUserAsync(string uid, string email)
        {
            DocumentReference docRef = _firestoreDb.Collection("users").Document(uid);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

            if (!snapshot.Exists)
            {
                // User này không có trong DB -> Tự động tạo
                var newUser = new
                {
                    Email = email,
                    StorageUsed = 0,
                    CreatedAt = Timestamp.FromDateTime(DateTime.UtcNow)
                    // SĐT sẽ bị thiếu, user có thể cập nhật profile sau
                };
                await docRef.SetAsync(newUser);
                Console.WriteLine($"Đã tự động tạo Document Firestore (thiếu SĐT) cho UID: {uid}");
            }
        }
    }
}
