using System.Net;
using System.Net.Sockets;
using FileApp.Server;

const int PORT = 8888;
const string STORAGE_PATH = "ServerStorage";

// Đảm bảo thư mục lưu trữ tồn tại
Directory.CreateDirectory(STORAGE_PATH);

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
