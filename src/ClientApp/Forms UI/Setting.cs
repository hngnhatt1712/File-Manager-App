using ClientApp.Services;
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
    public partial class Setting : UserControl
    {
        private FileTransferClient _fileClient;
        private UserAuth _authService; 
        public Setting()
        {
            InitializeComponent();
        }

        private void Setting_Load(object sender, EventArgs e)
        {
            // Không gọi UpdateStorageUI() ở đây vì _fileClient chưa được khởi tạo
            // Sẽ được gọi từ MainMenu.cs sau khi SetServices()
            Console.WriteLine("[Setting] Loaded - waiting for SetServices()");
        }
        public void SetServices(FileTransferClient client, UserAuth auth)
        {
            _fileClient = client;
            _authService = auth;
            
            // Gọi UpdateStorageUI() TẠI ĐÂY sau khi đã gán services
            if (_fileClient != null)
            {
                UpdateStorageUI();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (_fileClient == null)
            {
                MessageBox.Show("Lỗi: Chưa kết nối được với Server!");
                return;
            }
            else
            {
                DialogResult result = MessageBox.Show(
            "Bạn có chắc chắn muốn đăng xuất không?",
            "Xác nhận",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question
        );

                if (result == DialogResult.Yes)
                {
                    // Lệnh này sẽ khởi động lại toàn bộ App
                    Application.Restart();
                    Environment.Exit(0); // Đảm bảo tiến trình cũ bị đóng hoàn toàn
                }
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            FrmAbout fa = new FrmAbout();
            fa.ShowDialog();
        }

        private async void btnXoaTaiKhoan_Click(object sender, EventArgs e)
        {
            if (_fileClient == null)
            {
                MessageBox.Show("Lỗi: Chưa kết nối được với Server! (Client bị Null)");
                return;
            }

            // 1. Cảnh báo cực kỳ nghiêm trọng (Xoá chứ không phải đăng xuất)
            DialogResult result = MessageBox.Show(
                "HÀNH ĐỘNG NÀY SẼ XOÁ VĨNH VIỄN TÀI KHOẢN VÀ TOÀN BỘ FILE.\nBạn có chắc chắn muốn tiếp tục?",
                "XÁC NHẬN XOÁ TÀI KHOẢN",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                // 2. Gửi lệnh lên Server để thực hiện xoá trên Firebase
                bool isDeleted = await _fileClient.DeleteAccountAsync();

                if (isDeleted)
                {
                    MessageBox.Show("Tài khoản đã được xoá vĩnh viễn khỏi hệ thống.");

                    // 3. Xoá xong mới cho Restart App để về màn hình Login
                    Application.Restart();
                    Environment.Exit(0);
                }
                else
                {
                    MessageBox.Show("Lỗi: Server không thể xoá tài khoản lúc này. Vui lòng thử lại!");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_authService == null)
            {
                MessageBox.Show("Lỗi: Dịch vụ xác thực chưa sẵn sàng!");
                return;
            }

            // 3. TRUYỀN _authService VÀO ĐÂY (Đúng kiểu UserAuth mà ChangeEAP cần)
            ChangeEAP frm = new ChangeEAP(_authService);
            frm.ShowDialog();
        }
        // Hàm đổi Bytes sang KB, MB, GB
        private string FormatBytes(long bytes)
        {
            string[] suffixes = { "B", "KB", "MB", "GB", "TB" };
            int counter = 0;
            decimal number = (decimal)bytes;

            while (Math.Round(number / 1024) >= 1)
            {
                number = number / 1024;
                counter++;
            }
            // Hiển thị 2 chữ số thập phân (ví dụ: 1.25 GB)
            return string.Format("{0:n2} {1}", number, suffixes[counter]);
        }
        public async void UpdateStorageUI()
        {
            try
            {
                // Kiểm tra null trước
                if (_fileClient == null)
                {
                    Console.WriteLine("[Storage UI] Error: _fileClient is null");
                    return;
                }

                // 1. Gọi Client để lấy thông tin (Hàm bạn đã viết ở bước trước)
                // Giả sử hàm này trả về model chứa TotalUsed và MaxQuota
                var info = await _fileClient.GetStorageInfoAsync();

                if (info == null)
                {
                    Console.WriteLine("[Storage UI] Error: GetStorageInfoAsync returned null");
                    return;
                }

                // 2. Tính toán phần trăm để hiển thị lên thanh ProgressBar
                int percentage = 0;
                if (info.MaxQuota > 0)
                {
                    // Ép kiểu double để chia có số thập phân
                    percentage = (int)((double)info.TotalUsed / info.MaxQuota * 100);
                }

                // Chặn lỗi: Nếu vượt quá 100% thì chỉ hiện 100 thôi (để không crash app)
                if (percentage > 100) percentage = 100;
                if (percentage < 0) percentage = 0;

                // 3. Cập nhật lên UI 
                this.Invoke((MethodInvoker)delegate
                {
                    // Kiểm tra các control có tồn tại không
                    if (pbStorage != null && lblStorageInfo != null)
                    {
                        // Cập nhật thanh ProgressBar
                        pbStorage.Value = percentage;

                        // Cập nhật dòng chữ: "1.25 GB / 5.00 GB (25%)"
                        string usedStr = FormatBytes(info.TotalUsed);
                        string maxStr = FormatBytes(info.MaxQuota);
                        lblStorageInfo.Text = $"Bộ nhớ: {usedStr} / {maxStr} ({percentage}%)";

                        // (Tùy chọn) Đổi màu chữ thành Đỏ nếu sắp đầy (> 90%)
                        if (percentage >= 90)
                        {
                            lblStorageInfo.ForeColor = Color.Red;
                        }
                        else
                        {
                            lblStorageInfo.ForeColor = Color.Green;
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] UpdateStorageUI: {ex.Message}");
            }
        }

    }
}
