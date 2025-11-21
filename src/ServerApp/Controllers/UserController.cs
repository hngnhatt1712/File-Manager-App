using Firebase.Auth;
using Microsoft.AspNetCore.Mvc;
using ServerApp;
using ServerApp;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// 1. Định nghĩa địa chỉ cấp 1: "api/users"
[Route("api/users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly FirebaseAdminService _firestoreService;
    private readonly FirebaseAuthService _authService;

    public UserController(FirebaseAdminService firestoreService, FirebaseAuthService authService)
    {
        _firestoreService = firestoreService;
        _authService = authService;
    }

    // 2. Định nghĩa địa chỉ cấp 2 (Hành động): "register"
    // => Địa chỉ đầy đủ: POST api/users/register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserServerPayload payload)
    {
        // [FromBody] nghĩa là lấy dữ liệu JSON client gửi lên

        // Xác thực token 
        string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var decoded = await _authService.VerifyIdTokenAsync(token);
        if (decoded == null) return Unauthorized("Token không hợp lệ");

        try
        {
            // BƯỚC 2: Gọi Service để lưu vào Firestore
            await _firestoreService.CheckAndCreateUserAsync(payload.Uid, payload.Email, payload.Phone);

            // Trả về 200 OK
            return Ok(new { message = "Lưu database thành công" });
        }
        catch (System.Exception ex)
        {
            // Trả về lỗi 500 hoặc 400
            return BadRequest(ex.Message);
        }

    }
    // Lấy thông tin User hiện lên UI
    [HttpGet("profile")]
    public async Task<IAsyncResult> GetProfile()
    {
        // Lấy User ID từ Token
        
        // Gọi Service lấy từ Firestore
        
    }
}

// Class để hứng dữ liệu gửi lên (Giống hệt class bên Client)
public class UserServerPayload
{
    public string Uid { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
}
