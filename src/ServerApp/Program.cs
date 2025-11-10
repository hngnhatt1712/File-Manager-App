using System.Net;
using System.Net.Sockets;
using ServerApp;

const int PORT = 8888;
const string STORAGE_PATH = "ServerStorage";

// Đảm bảo thư mục lưu trữ tồn tại
Directory.CreateDirectory(STORAGE_PATH);
try
{
    FirebaseAdminService.Initialize();
    Console.WriteLine("Firebase Admin SDK đã khởi tạo thành công!");
}
catch (Exception ex)
{
    Console.WriteLine($"LỖI KHỞI TẠO FIREBASE: {ex.Message}");
    Console.WriteLine("Server sẽ tắt!");
    return; 
}
TcpListener listener = new TcpListener(IPAddress.Any, PORT);
listener.Start();
Console.WriteLine($"Server đang lắng nghe trên cổng {PORT}...");
Console.WriteLine($"Các file sẽ được lưu tại: {Path.GetFullPath(STORAGE_PATH)}");

try
{
    // Main Listener
    while (true)
    {
        // Chờ một client kết nối
        TcpClient client = await listener.AcceptTcpClientAsync();
        Console.WriteLine($"Client đã kết nối từ: {client.Client.RemoteEndPoint}");

        // Khởi chạy một trình xử lý riêng cho client này
        var handler = new ClientHandler(client, STORAGE_PATH);
        _ = Task.Run(handler.HandleClientAsync);
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Lỗi server: {ex.Message}");
}
finally
{
    listener.Stop();
}
