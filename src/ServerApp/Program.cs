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
catch(TypeInitializationException ex) // <-- Bắt lỗi "Type Initializer"
{
    Console.WriteLine("!!!!!!!!!! LỖI KHỞI TẠO FIREBASE (TypeInitializationException) !!!!!!!!!!");

    // ĐÂY LÀ PHẦN QUAN TRỌNG NHẤT: In ra lỗi thật
    if (ex.InnerException != null)
    {
        Console.WriteLine("\n--- LỖI THẬT (INNER EXCEPTION) ---");
        Console.WriteLine(ex.InnerException.Message);
        Console.WriteLine("\n--- STACK TRACE CỦA LỖI THẬT ---");
        Console.WriteLine(ex.InnerException.StackTrace);
    }
    else
    {
        Console.WriteLine("Không tìm thấy InnerException. Lỗi gốc là:");
        Console.WriteLine(ex.Message);
    }

    Console.WriteLine("\nServer sẽ tắt. Vui lòng CHỤP ẢNH MÀN HÌNH lỗi trên và gửi lại.");
    Console.ReadKey(); // Dừng lại để bạn đọc lỗi
    return; // Dừng server
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
