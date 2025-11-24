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
    public async Task<IActionResult> GetProfile()
    {
        try
        {
            // 1. Lấy token từ Header
            string authHeader = HttpContext.Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                return Unauthorized("Missing or invalid Authorization header");

            string idToken = authHeader.Replace("Bearer ", "");

            // 2. Xác thực Token bằng FirebaseAuthService
            var decoded = await _authService.VerifyIdTokenAsync(idToken);
            if (decoded == null)
                return Unauthorized("Invalid or expired token");

            string uid = decoded.Uid;

            // 3. Lấy dữ liệu user từ Firestore (bạn cần có hàm GetUserAsync trong FirebaseAdminService)
            var userData = await _firestoreService.GetUserAsync(uid);
            if (userData == null)
                return NotFound($"User with uid {uid} not found in Firestore");

            // 4. Trả JSON về UI
            return Ok(new
            {
                Uid = uid,
                email = userData.Email,
                phone = userData.Phone,
                userData = userData
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Server error: {ex.Message}");
        }
    }
}

// Class để hứng dữ liệu gửi lên (Giống hệt class bên Client)
public class UserServerPayload
{
    public string Uid { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
}
