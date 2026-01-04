using ClientApp;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
 // Dòng này cực kỳ quan trọng để nhận diện EmailAuthProvider
using SharedLibrary;
using Firebase.Auth; // Bắt buộc phải có cái này để dùng Extension Methods
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace ClientApp.Services
{
    // - Điều khiển UI (ẩn/hiện nút, trạng thái đăng nhập)
    // - Gọi Firebase Client SDK để lấy ID Token
    public class UserAuth
    {
        private readonly FileTransferClient _fileClient;
        private FirebaseAuthClient _authClient;
        string _apiKey = "AIzaSyC9rPbS1Ks85CIdHo98WJCLb8n7V6UR8OE"; // API Key của bạn



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
            try
            {
                // 1. GỌI FIREBASE AUTH ĐỂ ĐĂNG NHẬP
                var authResult = await _authClient.SignInWithEmailAndPasswordAsync(email, password);

                // 2. LẤY UID
                string uid = authResult.User.Uid;
                string token = await authResult.User.GetIdTokenAsync();

                // 3. TRẢ VỀ ĐỐI TƯỢNG AuthResult ĐƠN GIẢN
                return new AuthResult
                {
                    IsSuccess = true,
                    Message = "Đăng nhập thành công",
                    Token = token,
                    Uid = authResult.User.Uid,
                    Email = email,

                };
            }
            catch (Exception ex)
            {
                return new AuthResult
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        // ======================= REGISTER =======================

        public async Task<AuthResult> RegisterAsync(string email, string password, string phone)
        {
            string uid = string.Empty;
            string jwtToken = string.Empty;

            // TẠO USER TRÊN FIREBASE AUTHENTICATION
            try
            {
                // Gọi SDK để tạo user
                
                var authLink = await _authClient.CreateUserWithEmailAndPasswordAsync(email, password);

                jwtToken = await authLink.User.GetIdTokenAsync();
                uid = authLink.User.Uid;
            }
            catch (Exception authEx)
            {
                // Trả về object lỗi để UI hiển thị
                return new AuthResult
                {
                    IsSuccess = false,
                    Message = $"Lỗi Firebase Auth: {authEx.Message}"
                };
            }
            try
            {
                // Đảm bảo kết nối
                if (!_fileClient.IsConnected) await _fileClient.ConnectAsync();

                // Gửi lệnh đồng bộ qua TCP Socket
                await _fileClient.SyncUserAsync(jwtToken, email, phone);
            }
            catch (Exception tcpEx)
            {
                return new AuthResult
                {
                    IsSuccess = true,
                    Message = $"Đăng ký thành công (Cảnh báo: Lỗi lưu SĐT lên Server: {tcpEx.Message})",
                    Uid = uid,
                    Token = jwtToken,
                    Email = email
                };
            }
            return new AuthResult
            {
                IsSuccess = true,
                Message = "Đăng ký thành công!",
                Uid = uid,
                Token = jwtToken,
                Email = email
            };
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
                throw new Exception("Không thể gửi mail reset: " + ex.Message);
            }
        }

        // =================== LOG OUT ===================
        public void Logout()
        {
            // Đăng xuất khỏi Firebase Auth Client
            _authClient.SignOut();
        }

        // đổi mail , password
        // Trong UserAuth.cs
        // 1. Thêm dòng này vào ngay dưới dòng: private readonly FileTransferClient _fileClient;
        public Firebase.Auth.User CurrentUser => _authClient.User;
        // 2. Sửa hàm ReAuthenticateAsync (Xác thực lại mật khẩu)
        public async Task<bool> ReAuthenticateAsync(string password)
        {
            try
            {
                if (_authClient.User == null)
                    return false;

                string email = _authClient.User.Info.Email;

                await _authClient.SignInWithEmailAndPasswordAsync(email, password);

                return true;
            }
            catch (Exception ex)
            {
                {
                    MessageBox.Show("ReAuth FAILED: " + ex.Message);
                    return false;
                }
            }
        }

        // Hàm đổi Email
     
        // Hàm đổi Password
        public async Task<bool> UpdatePasswordAsync(string newPassword)
        {
            try
            {
                // Gọi hàm thay đổi mật khẩu
                await _authClient.User.ChangePasswordAsync(newPassword);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}