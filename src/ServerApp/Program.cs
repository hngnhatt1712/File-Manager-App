using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using ServerApp; 
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

class Program
{
    private const int TCP_PORT = 8888;
    private const string PROJECT_ID = "fileapp-9fce3";

    static async Task Main(string[] args)
    {

        // KHỞI TẠO MÔI TRƯỜNG & FIREBASE

        // 1. Tạo thư mục lưu trữ (ServerStorage)
        string storagePath = Path.Combine(Directory.GetCurrentDirectory(), "ServerStorage");
        Directory.CreateDirectory(storagePath);
        Console.WriteLine($"[Storage] Kho lưu trữ tại: {storagePath}");
        string pathToKey = Path.Combine(Directory.GetCurrentDirectory(), "service-account-key.json");

        // Kiểm tra xem file có tồn tại không cho chắc ăn
        if (!File.Exists(pathToKey))
        {
            Console.WriteLine($"[LỖI] Không tìm thấy file key tại: {pathToKey}");
            Console.ReadLine();
            return;
        }

        // Đặt biến môi trường để Firestore tự tìm thấy chìa khóa
        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", pathToKey);

        // 2. Khởi tạo Firebase Admin Service 
        FirestoreDb firestoreDb;
        FirebaseAdminService adminService;
        try
        {
            // Constructor của FirebaseAdminService sẽ tự đọc file json và kết nối
            adminService = new FirebaseAdminService();
            firestoreDb = FirestoreDb.Create(PROJECT_ID);
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
                ClientHandler handler = new ClientHandler(client, storagePath, adminService, firestoreDb);

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