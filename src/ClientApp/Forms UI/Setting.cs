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
        private UserAuth _authService; // 1. Thêm biến này
        public Setting()
        {
            InitializeComponent();
        }

        private void Setting_Load(object sender, EventArgs e)
        {

        }
        public void SetServices(FileTransferClient client, UserAuth auth)
        {
            _fileClient = client;
            _authService = auth;
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
    }
}
