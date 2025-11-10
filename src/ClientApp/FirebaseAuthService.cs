using Firebase.Auth;
using Firebase.Auth.Providers;

namespace ClientApp
{
    internal class FirebaseAuthService
    {
        private const string FIREBASE_WEB_API_KEY = "WEB_API_KEY";
        private readonly FirebaseAuthClient _authClient;

        public FirebaseAuthService()
        {
            // 1. Cấu hình Firebase Auth
            var config = new FirebaseAuthConfig
            {
                ApiKey = "AIzaSyC9rPbS1Ks85CIdHo98WJCLb8n7V6UR8OE",
                AuthDomain = "fileapp-9fce3.firebaseapp.com",
                Providers = new Firebase.Auth.Providers.FirebaseAuthProvider[]
                {
                new EmailProvider()
                }
            };

            // 2. Khởi tạo Auth Client
            _authClient = new FirebaseAuthClient(config);
        }

        /// Xử lý Đăng ký 
        public async Task RegisterAndSubmitProfileAsync(string email, string password, string phoneNumber, FileTransferClient tcpClient)
        {
            try
            {
                // 1. Dùng Client để tạo user
                var authResult = await _authClient.CreateUserWithEmailAndPasswordAsync(email, password);

                // 2. Lấy token 
                string jwtToken = await authResult.User.GetIdTokenAsync();

                // 3. Gửi token và SĐT lên Server TCP
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
                // 1. Dùng Client để đăng nhập
                var authResult = await _authClient.SignInWithEmailAndPasswordAsync(email, password);

                // 2. Lấy token 
                string jwtToken = await authResult.User.GetIdTokenAsync();

                // 3. Gửi token này cho Server TCP để xác thực
                await tcpClient.SendLoginAttemptAsync(jwtToken);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi đăng nhập: {ex.Message}");
            }
        }
        // Xử lý quên mật khẩu
        public async Task SendPasswordResetEmailAsync(string email)
        {
            try
            {
                await _authClient.ResetEmailPasswordAsync(email);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi gửi email reset: {ex.Message}");
            }
        }
    }
}