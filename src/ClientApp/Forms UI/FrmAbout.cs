using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientApp.Forms_UI
{
    public partial class FrmAbout : Form
    {
        public FrmAbout()
        {
            InitializeComponent();
            this.Load += FrmAbout_Load;
        }
        private async void FrmAbout_Load(object sender, EventArgs e)
        {
            // Bước quan trọng: Đợi WebView2 sẵn sàng
            await webView21.EnsureCoreWebView2Async(null);

            // Chuỗi HTML tui đã soạn cho bạn
            string htmlContent = @"
        <!DOCTYPE html>
<html>
<head>
    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
    <meta charset=""utf-8"">
    <style>
        :root {
            --main-grad: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            --sub-grad: linear-gradient(135deg, #00dbde 0%, #fc00ff 100%);
            --card-bg: rgba(255, 255, 255, 0.95);
        }
        body {
            font-family: 'Segoe UI', system-ui, -apple-system, sans-serif;
            background: #f0f2f5; margin: 0; padding: 25px;
            display: flex; justify-content: center;
        }
        .container {
            background: white; width: 100%; max-width: 650px;
            border-radius: 30px; box-shadow: 0 20px 60px rgba(0,0,0,0.12);
            overflow: hidden; animation: fadeIn 0.8s ease-out;
        }
        @keyframes fadeIn { from { opacity: 0; transform: translateY(20px); } to { opacity: 1; transform: translateY(0); } }
        
        .hero {
            background: var(--main-grad); padding: 50px 30px;
            text-align: center; color: white; position: relative;
        }
        .hero h1 { margin: 0; font-size: 28px; text-transform: uppercase; letter-spacing: 2px; }
        .group-badge {
            display: inline-block; margin-top: 15px; padding: 6px 20px;
            background: rgba(255,255,255,0.2); border-radius: 50px;
            font-weight: 600; font-size: 14px; backdrop-filter: blur(10px);
        }

        .main-text { padding: 35px; line-height: 1.8; color: #444; font-size: 15.5px; text-align: justify; }
        .main-text b { color: #764ba2; }

        .feature-grid {
            display: grid; grid-template-columns: repeat(3, 1fr);
            gap: 15px; padding: 0 35px 35px;
        }
        .f-item {
            padding: 15px 10px; border-radius: 18px; text-align: center;
            background: #fff; border: 1px solid #f0f0f0;
            transition: all 0.3s;
        }
        .f-item:hover { transform: translateY(-5px); box-shadow: 0 10px 20px rgba(0,0,0,0.05); border-color: #764ba2; }
        .f-icon { font-size: 24px; margin-bottom: 8px; display: block; }
        .f-name { font-size: 13px; font-weight: 700; color: #333; }

        .footer {
            background: #fdfdfd; padding: 20px; text-align: center;
            border-top: 1px solid #eee; font-size: 12px; color: #999;
        }
        /* Colors for icons */
        .c1 { color: #ff5252; } .c2 { color: #2979ff; } .c3 { color: #ffc107; }
        .c4 { color: #4caf50; } .c5 { color: #9c27b0; } .c6 { color: #00bcd4; }
    </style>
</head>
<body>
    <div class=""container"">
        <div class=""hero"">
            <h1>File App Manager</h1>
            <div class=""group-badge"">🚀 ĐỒ ÁN CƠ SỞ - NHÓM 11</div>
        </div>
        
        <div class=""main-text"">
            Ứng dụng <b>Quản lý File</b> là một giải pháp phần mềm được xây dựng trên kiến trúc <b>Client-Server</b>, 
            nhằm cung cấp cho người dùng môi trường lưu trữ và tổ chức dữ liệu cá nhân hiệu quả, an toàn. 
            Hệ thống tích hợp đầy đủ các công cụ thiết yếu để thao tác với tập tin như: <i>tải lên, phân loại, 
            tìm kiếm, đổi tên, di chuyển và chia sẻ dữ liệu.</i> Bên cạnh khả năng kiểm soát dung lượng và 
            ghi nhật ký hoạt động (log), ứng dụng còn chú trọng đến trải nghiệm người dùng qua các tính năng 
            quản lý thùng rác và khôi phục dữ liệu, giúp việc quản lý tài nguyên số trở nên trực quan hơn.
        </div>

        <div class=""feature-grid"">
            <div class=""f-item""><span class=""f-icon"">📤</span><span class=""f-name"">Tải lên</span></div>
            <div class=""f-item""><span class=""f-icon"">🔍</span><span class=""f-name"">Tìm kiếm</span></div>
            <div class=""f-item""><span class=""f-icon"">📂</span><span class=""f-name"">Phân loại</span></div>
            <div class=""f-item""><span class=""f-icon"">🔄</span><span class=""f-name"">Di chuyển</span></div>
            <div class=""f-item""><span class=""f-icon"">🔗</span><span class=""f-name"">Chia sẻ</span></div>
            <div class=""f-item""><span class=""f-icon"">🔒</span><span class=""f-name"">Bảo mật</span></div>
            <div class=""f-item""><span class=""f-icon"">📊</span><span class=""f-name"">Nhật ký</span></div>
            <div class=""f-item""><span class=""f-icon"">🗑️</span><span class=""f-name"">Thùng rác</span></div>
            <div class=""f-item""><span class=""f-icon"">💎</span><span class=""f-name"">Khôi phục</span></div>
        </div>

        <div class=""footer"">
            Thành viên thực hiện: <b>Nhóm 11</b><br>
            Công nghệ sử dụng: .NET WinForms, Firebase & TCP/IP Protocol
        </div>
    </div>
</body>
</html>";
            // Lệnh để hiện HTML lên WebView2
            webView21.NavigateToString(htmlContent);
        }
    }
}
