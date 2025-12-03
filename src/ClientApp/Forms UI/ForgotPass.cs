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

namespace ClientApp
{
    public partial class ForgotPass : Form
    {
        private readonly UserAuth _authService;
        public ForgotPass(UserAuth authService)
        {
            InitializeComponent();
            _authService = authService;
        }

        private void ForgotPass_Load(object sender, EventArgs e)
        {

        }


        private async void btn_send_Click_1(object sender, EventArgs e)
        {
            string email = tb_email.Text.Trim();
            if (string.IsNullOrEmpty(email) || !email.Contains("@"))
            {
                MessageBox.Show("Vui lòng nhập một địa chỉ email hợp lệ!", "Lỗi",
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 3. Gọi dịch vụ
            try
            {
                this.Text = "Đang gửi email...";
                btn_send.Enabled = false;

                await _authService.ResetPasswordAsync(email);

                MessageBox.Show("Đã gửi email thành công!\n\n" +
                                "Vui lòng kiểm tra hộp thư và nhấp vào " +
                                "đường link để đặt lại mật khẩu!",
                                "Gửi thành công",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Gửi thất bại",
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Text = "Quên mật khẩu ?";
                btn_send.Enabled = true;
            }
        }
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Check if the click was on the title bar
                if (e.Clicks == 1 && e.Y <= this.Height && e.Y >= 0)
                {
                    ReleaseCapture();
                    SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                }
            }
        }
    }
}