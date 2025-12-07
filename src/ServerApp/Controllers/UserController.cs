using FirebaseAdmin.Auth;
using ServerApp;
using System;
using System.Collections.Generic;

public class UserController
{
    private readonly FirebaseAdminService _firebaseAdmin;

    public UserController(FirebaseAdminService firebaseAdmin)
    {
        _firebaseAdmin = firebaseAdmin;
    }

    // Client gửi: SYNC_USER|token|email|name
    public async Task<string> SyncUser(string token, string email, string phone)
    {
        try
        {
            // 1. XÁC THỰC TOKEN (Dùng await thay vì .Result để không treo luồng)
            FirebaseToken decodedToken = await _firebaseAdmin.VerifyTokenAsync(token);

            if (decodedToken == null)
            {
                return "SYNC_FAIL|Token không hợp lệ hoặc đã hết hạn.";
            }

            string uid = decodedToken.Uid;
            Console.WriteLine($"[User] Token hợp lệ. User: {email} ({uid})");

            // 2. TẠO DỮ LIỆU USER (Dùng await)
            await _firebaseAdmin.CheckAndCreateUserAsync(uid, email, phone);

            // 3. QUAN TRỌNG: Trả về kèm UID để ClientHandler lưu lại
            return $"SYNC_OK|{uid}";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Error] Sync thất bại: {ex.Message}");
            return "SYNC_FAIL|" + ex.Message;
        }
    }
}