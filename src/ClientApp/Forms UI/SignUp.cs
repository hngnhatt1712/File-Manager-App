using ClientApp.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Firebase.Auth;
using System.Windows.Forms;

namespace ClientApp
{
    public partial class SignUp : Form
    {
        private readonly UserAuth authService;
        public SignUp(UserAuth _authService)
        {
            InitializeComponent();
            authService = _authService;
        }

        private void SignUp_Load(object sender, EventArgs e)
        {

        }

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ReleaseCapture();
        private async void btn_signup_Click_1(object sender, EventArgs e)
        {
            btn_signup.Enabled = false;
            lb_status.Text = "Đang đăng ký...";
            try
            {
                string email = tb_email.Text.Trim();
                string phone = tb_sdt.Text.Trim();
                string password = tb_pass.Text.Trim();
                string confirmPass = tb_cfpass.Text.Trim();

                // 2. Kiểm tra dữ liệu
                // if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(phone) ||
                //      string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPass))
                // {
                //     MessageBox.Show("Vui lòng điền đầy đủ thông tin!");
                //     return;
                // }


                if (!CheckEmail(email))
                {
                    MessageBox.Show("Vui lòng nhập đúng định dạng email", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!CheckPhone(phone))
                {
                    MessageBox.Show("Vui lòng nhập đúng định dạng số điện thoại", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!CheckAccount(password))
                {
                    MessageBox.Show("Vui lòng nhập mật khẩu dài 6-24 ký tự, với các chữ và số, chữ hoa và chữ thường"
                        , "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (password != confirmPass)
                {
                    MessageBox.Show("Mật khẩu và xác nhận mật khẩu không khớp!", 
                        "Lỗi!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // Gọi hàm từ service
                var authResult = await authService.RegisterAsync(email, password, phone);

                // Xử lý kết quả
                MessageBox.Show("Đăng ký thành công!", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("EMAIL_EXISTS") ||
                    ex.Message.Contains("email exists") ||
                    ex.Message.Contains("already"))
                {
                    MessageBox.Show("Email này đã được đăng ký trước đó!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show($"Lỗi đăng ký: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btn_signup.Enabled = true;
                lb_status.Text = "";
            }
        }
        public bool CheckAccount(string ac) //check mat khau va ten tai khoan
        {
            return Regex.IsMatch(ac, "^[a-zA-Z0-9]{6,24}$");
        }

        public bool CheckEmail(string em) //check email
        {
            return Regex.IsMatch(em, @"^[a-zA-Z0-9._]{3,20}@gmail.com(.vn)?$");
        }
        public bool CheckPhone(string ph) //check so dien thoai
        {
            return Regex.IsMatch(ph, @"^[0-9]{10,11}$");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
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
