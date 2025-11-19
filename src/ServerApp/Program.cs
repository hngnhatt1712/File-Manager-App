using Microsoft.AspNetCore.Builder;     
using Microsoft.Extensions.DependencyInjection; 
using Microsoft.Extensions.Hosting;
using ServerApp;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

class Program
{
    // Port cho API (HTTP) - Dùng để đăng ký, lấy metadata
    private const int API_PORT = 5000;
    // Port cho TCP (File Transfer) - Dùng để upload/download
    private const int TCP_PORT = 8888;

    static async Task Main(string[] args)
    {
        // PHẦN 1: CẤU HÌNH WEB API (ASP.NET CORE)
        var builder = WebApplication.CreateBuilder(args);

        // 1. Thêm Controllers (để nhận diện UserController.cs)
        builder.Services.AddControllers();

        // 2. Đăng ký Service (Dependency Injection)
        // Để UserController và TCP Server dùng chung 1 bản thể (Singleton)
        builder.Services.AddSingleton<FirebaseAuthService>();
        builder.Services.AddSingleton<FirebaseAdminService>();

        var app = builder.Build();

        // 3. Định tuyến (Map) các Controller
        app.MapControllers();
        // PHẦN 2: KHỞI ĐỘNG TCP SERVER (CHẠY NGẦM)

        // Lấy các service đã tạo ra từ "kho" của ứng dụng Web
        // Để truyền cho TCP Handler
        var authService = app.Services.GetRequiredService<FirebaseAuthService>();
        var adminService = app.Services.GetRequiredService<FirebaseAdminService>();

        // Chạy TCP Server trên một luồng riêng biệt (Background Task) để không chặn Web API hoạt động
        _ = Task.Run(() => StartTcpServer(authService, adminService));

        // PHẦN 3: CHẠY WEB API
        Console.WriteLine($"WARNING: Web API running on port {API_PORT}");
        Console.WriteLine($"WARNING: TCP Server running on port {TCP_PORT}");

        // Lệnh này sẽ chặn luồng chính và lắng nghe HTTP request
        app.Run($"http://0.0.0.0:{API_PORT}");
    }

    // Hàm tách riêng logic TCP Server 
    static async Task StartTcpServer(FirebaseAuthService authService, FirebaseAdminService adminService)
    {
        TcpListener listener = new TcpListener(IPAddress.Any, TCP_PORT);
        listener.Start();
        Console.WriteLine($"[TCP] Server started on port {TCP_PORT}. Waiting for file transfers...");

        while (true)
        {
            try
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                Console.WriteLine("[TCP] New Client connected.");

                // Tạo Handler và truyền service vào
                ClientHandler handler = new ClientHandler(client, authService, adminService);

                // Xử lý client trên thread riêng
                _ = Task.Run(() => handler.HandleClientAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[TCP] Error accepting client: {ex.Message}");
            }
        }
    }
}