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

        private async void btn_signup_Click(object sender, EventArgs e)
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
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(phone) ||
                     string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPass))
                {
                    MessageBox.Show("Vui lòng điền đầy đủ thông tin!");
                    return;
                }
                if (!ValidateUsingEmailAddressAttribute(email))
                {
                    MessageBox.Show("Email không hợp lệ!");
                    return;
                }
                if (password != confirmPass)
                {
                    MessageBox.Show("Mật khẩu và xác nhận mật khẩu không khớp!");
                    return;
                }

                // Gọi hàm từ service
                var authResult = await authService.RegisterAsync(email, password, phone);

                // Xử lý kết quả
                MessageBox.Show($"Đăng ký thành công!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi đăng ký: {ex.Message}");
            }
            finally
            {
                btn_signup.Enabled = true;
                lb_status.Text = "";
            }

        }
        // Hàm kiểm tra định dạng (có thể thêm vào hoặc tự làm)
        // Xử lý Email lỗi
        public bool ValidateUsingEmailAddressAttribute(string emailAddress)
        {
            var emailVal = new EmailAddressAttribute();
            return emailVal.IsValid(emailAddress);
        }

        private void tb_sdt_TextChanged(object sender, EventArgs e)
        {
            if (!(int.TryParse(tb_sdt.Text, out int value)) && (tb_sdt.Text != ""))
            {
                MessageBox.Show("Sai Định Dạng!");
                tb_sdt.Clear();
                tb_sdt.Focus();
            }
        }
        


    }
}
