using ClientApp;
using Firebase.Auth;
using Firebase.Auth.Providers;
using SharedLibrary;
using System;
using System.Threading.Tasks;

namespace ClientApp.Services
{
    // - Điều khiển UI (ẩn/hiện nút, trạng thái đăng nhập)
    // - Gọi Firebase Client SDK để lấy ID Token
    public class UserAuth
    {   
        private FirebaseAuthClient _authClient;
        private readonly FileTransferClient _fileClient;
        public UserAuth(FileTransferClient fileClient)
        {
            var config = new FirebaseAuthConfig
            {
                ApiKey = "AIzaSyC9rPbS1Ks85CIdHo98WJCLb8n7V6UR8OE",
                AuthDomain = "fileapp-9fce3.firebaseapp.com",
                Providers = new FirebaseAuthProvider[]
                {
                    new EmailProvider()
                }
            };

            _authClient = new FirebaseAuthClient(config);
            _fileClient = fileClient;
        }

        // ======================= LOGIN =======================

        public async Task<AuthResult> LoginAsync(string email, string password)
        {
            
        }

        // ======================= REGISTER =======================

        public async Task<AuthResult> RegisterAsync(string email, string password, string phone)
        {
            
        }

        // =================== RESET PASSWORD ===================
        public async Task ResetPasswordAsync(string email)
        {
            try
            {
                await _authClient.ResetEmailPasswordAsync(email);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // =================== LOG OUT ===================
        public void Logout()
        {
            
        }
    }
}