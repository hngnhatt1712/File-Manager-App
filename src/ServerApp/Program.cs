using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using ServerApp; 

class Program
{
    private const int TCP_PORT = 8888;

    static async Task Main(string[] args)
    {

        // KHỞI TẠO MÔI TRƯỜNG & FIREBASE

        // 1. Tạo thư mục lưu trữ (ServerStorage)
        string storagePath = Path.Combine(Directory.GetCurrentDirectory(), "ServerStorage");
        Directory.CreateDirectory(storagePath);
        Console.WriteLine($"[Storage] Kho lưu trữ tại: {storagePath}");

        // 2. Khởi tạo Firebase Admin Service 
        FirebaseAdminService adminService;
        try
        {
            // Constructor của FirebaseAdminService sẽ tự đọc file json và kết nối
            adminService = new FirebaseAdminService();
            Console.WriteLine("[Firebase] Kết nối Firestore & Auth thành công.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[FATAL ERROR] Không thể khởi tạo Firebase: {ex.Message}");
            Console.WriteLine("Server sẽ dừng hoạt động");
            Console.ReadLine();
            return;
        }
        // KHỞI ĐỘNG TCP SERVER

        TcpListener listener = new TcpListener(IPAddress.Any, TCP_PORT);
        listener.Start();
        Console.WriteLine($"[TCP] Server đang lắng nghe tại cổng {TCP_PORT}...");
        Console.WriteLine("---------------------------------------------");

        // Vòng lặp chính để chấp nhận kết nối
        while (true)
        {
            try
            {
                // Chờ Client kết nối
                TcpClient client = await listener.AcceptTcpClientAsync();
                Console.WriteLine($"[TCP] Client mới kết nối từ: {client.Client.RemoteEndPoint}");

                // Tạo Handler để xử lý riêng cho Client này
                // Lưu ý: Truyền đúng 3 tham số mà ClientHandler cần:
                // 1. Client socket
                // 2. Đường dẫn lưu file
                // 3. Service Firebase
                ClientHandler handler = new ClientHandler(client, storagePath, adminService);

                // Chạy trên luồng riêng (Fire-and-forget)
                _ = Task.Run(() => handler.HandleClientAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[TCP Error] Lỗi khi chấp nhận client: {ex.Message}");
            }
        }
    }
}