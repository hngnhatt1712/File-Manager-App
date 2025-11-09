using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Auth;

namespace ClientApp
{
    internal class FirebaseAuthService
    {
        private readonly FirebaseAuthClient _authClient;

        // Đây là Web API Key lấy từ Firebase Console
        private const string FIREBASE_WEB_API_KEY = "WEB_API_KEY";

        public FirebaseAuthService()
        {
            // Chỉ khởi tạo Auth
            var config = new FirebaseConfig(FIREBASE_WEB_API_KEY);
            _authClient = new FirebaseAuthClient(config);
        }
        /// Xử lý Đăng ký 
        public async Task RegisterAndSubmitProfileAsync(string email, string password, string phoneNumber, FileTransferClient tcpClient)
        {
            try
            {
                // 1. Dùng thư viện Auth để tạo user
                // (Thư viện này tự động đăng nhập user sau khi tạo)
                var authResult = await _authClient.CreateUserWithEmailAndPasswordAsync(email, password);
                string jwtToken = authResult.User.FirebaseToken;

                // 2. Gửi token và SĐT lên Server TCP ngay lập tức
                // Server sẽ lo phần tạo Document trên Firestore
                await tcpClient.SendFirstLoginRegisterAsync(jwtToken, phoneNumber);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi đăng ký: {ex.Message}");
            }
        }
        /// Xử lý Đăng nhập 
        public async Task LoginAsync(string email, string password, FileTransferClient tcpClient)
        {
            try
            {
                // 1. Gọi SignInWithEmailAndPasswordAsync() để đăng nhập
                var authResult = await _authClient.SignInWithEmailAndPasswordAsync(email, password);

                // 2. Lấy jwtToken (FirebaseToken)
                string jwtToken = authResult.User.FirebaseToken;

                // 3. Gửi token này cho Server TCP để xác thực
                await tcpClient.SendLoginAttemptAsync(jwtToken);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi đăng nhập: {ex.Message}");
            }
        }
    }
}
