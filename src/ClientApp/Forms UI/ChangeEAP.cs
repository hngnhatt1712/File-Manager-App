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
    public partial class ChangeEAP : Form
    {
        private UserAuth _authService;
        public ChangeEAP(UserAuth authService)
        {
            InitializeComponent();
            _authService = authService; // Gán giá trị được truyền vào

            // Hiển thị email hiện tại lên ô txtEmailCu khi vừa mở form
            if (_authService.CurrentUser != null)
            {
                txtEmailCu.Text = _authService.CurrentUser.Info.Email;
            }
        }

        private async Task<bool> ReAuthOnceAsync()
        {
            using (var checkPass = new Check_Password())
            {
                if (checkPass.ShowDialog() != DialogResult.OK)
                    return false;

                return await _authService.ReAuthenticateAsync(checkPass.PasswordGiaoDien);
            }
        }

        

        private async void button2_Click(object sender, EventArgs e)
        {
            string newPassword = txtNewPassword.Text.Trim();

            // 1. Kiểm tra đầu vào
            if (string.IsNullOrEmpty(newPassword) || newPassword.Length < 6)
            {
                MessageBox.Show("Mật khẩu mới phải có ít nhất 6 ký tự!");
                return;
            }

            // 2. Yêu cầu nhập mật khẩu HIỆN TẠI để xác thực (Security)
            // Bạn có thể dùng Form Check_Password của bạn ở đây
            string currentPassword = PromptForCurrentPassword();
            if (string.IsNullOrEmpty(currentPassword)) return;

            // 3. Tiến hành xác thực lại và đổi
            bool isAuth = await _authService.ReAuthenticateAsync(currentPassword);
            if (isAuth)
            {
                bool success = await _authService.UpdatePasswordAsync(newPassword);
                if (success)
                {
                    MessageBox.Show("Đổi mật khẩu thành công!");
                    txtNewPassword.Clear();
                }
                else MessageBox.Show("Lỗi: Không thể đổi mật khẩu.");
            }
            else MessageBox.Show("Mật khẩu hiện tại không đúng!");
        }

        private string PromptForCurrentPassword()
        {
            // Đây là cách đơn giản dùng InputBox của thư viện VisualBasic
            // Hoặc bạn có thể khởi tạo Form "Check_Password" của bạn tại đây
            return Microsoft.VisualBasic.Interaction.InputBox("Vui lòng nhập MẬT KHẨU HIỆN TẠI để xác nhận thay đổi:", "Xác thực bảo mật", "");
        }

        private void btnUpdateEmail_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void ChangeEAP_Load(object sender, EventArgs e)
        {
            if (_authService.CurrentUser != null)
            {
                txtEmailCu.Text = _authService.CurrentUser.Info.Email;
            }
        }
    }
}
