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
    public partial class SignUp : Form
    {
        private readonly FileTransferClient _fileClient;
        private readonly FirebaseAuthService _authService;
        public SignUp(FirebaseAuthService authService, FileTransferClient fileClient)
        {
            InitializeComponent();
            _authService = authService;
            _fileClient = fileClient;
        }

        private void SignUp_Load(object sender, EventArgs e)
        {

        }

        private async void btn_signup_Click(object sender, EventArgs e)
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

            if (password != confirmPass)
            {
                MessageBox.Show("Mật khẩu và xác nhận mật khẩu không khớp!");
                return;
            }

            try
            {
                // 3. Đảm bảo đã kết nối đến Server TCP
                await EnsureConnectedAsync();

                // 4. Gọi hàm đăng ký (đã bao gồm gửi token lên Server)
                await _authService.RegisterAndSubmitProfileAsync(email, password, phone, _fileClient);

                // 5. Thông báo và đóng form
                MessageBox.Show("Đăng ký thành công! Vui lòng quay lại đăng nhập!");
                this.Close();
            }
            catch (Exception ex)
            {
                // Hiển thị bất kỳ lỗi nào (từ Firebase hoặc Server TCP)
                MessageBox.Show($"Đăng ký thất bại: {ex.Message}");
            }
        }
        private async Task EnsureConnectedAsync()
        {
            if (_fileClient.IsConnected) return;
            try
            {
                string ip = "127.0.0.1";
                int port = 8888;
                await _fileClient.ConnectAsync(ip, port);
            }
            catch (Exception ex)
            {
                throw new Exception($"Kết nối TCP thất bại: {ex.Message}");
            }
        }
        
        //kiểm tra định dạnh sdt
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
