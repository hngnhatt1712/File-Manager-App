using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using System;
using System.Threading.Tasks;

// Lớp dịch vụ chịu trách nhiệm giao tiếp với Firebase Auth
public class FirebaseAuthService
{
    public FirebaseAuthService()
    {
        // Khởi tạo Firebase Admin SDK
        if (FirebaseApp.DefaultInstance == null)
        {
            var credential = GoogleCredential.FromFile("service-account-key.json");
            FirebaseApp.Create(new AppOptions()
            {
                Credential = credential,
            });
        }
    }

    // Giai đoạn 4: Xây dựng API (Phần thực thi logic)
    #region Authentication Methods

    // Quốc Sử: 'Xây dựng API đăng kí' + 'Xây dựng API thêm user mới'
    public async Task<string> RegisterUserAsync(string email, string password, string displayName)
    {
        try
        {
            UserRecordArgs args = new UserRecordArgs()
            {
                Email = email,
                Password = password,
                DisplayName = displayName,
                Disabled = false,
            };

            UserRecord userRecord = await FirebaseAuth.DefaultInstance.CreateUserAsync(args);
            return userRecord.Uid;
        }
        catch (FirebaseAuthException ex)
        {
            // TODO: Xử lý lỗi ( email đã tồn tại)
            return $"Error: {ex.Message}";
        }
    }

    // Client gửi token, Server xác thực
    public async Task<FirebaseToken> VerifyIdTokenAsync(string idToken)
    {
        try
        {
            // idToken này là token client gửi lên sau khi đăng nhập thành công ở phía client
            FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);
            return decodedToken;
        }
        catch (FirebaseAuthException)
        {
            return null;
        }
    }
    public async Task<string> GeneratePasswordResetLinkAsync(string email)
    {
        try
        {
            string link = await FirebaseAuth.DefaultInstance.GeneratePasswordResetLinkAsync(email);
            return link;
        }
        catch (FirebaseAuthException ex)
        {
            return $"Error: {ex.Message}";
        }
    }

    // Đình Quốc: 'Xây dựng API đăng xuất'
    public async Task RevokeRefreshTokensAsync(string uid)
    {
        await FirebaseAuth.DefaultInstance.RevokeRefreshTokensAsync(uid);
    }

    #endregion
}