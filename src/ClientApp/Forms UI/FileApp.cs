using System;
using System.Windows.Forms;
using ClientApp.Services;

namespace ClientApp
{
    // Khởi tạo service
    public partial class FileApp : Form
    {
        private readonly UserAuth _authService;
        private readonly FileTransferClient _fileClient;

        public FileApp()
        {
            InitializeComponent();
            _fileClient = new FileTransferClient();
            _authService = new UserAuth(_fileClient);
        }
        // Trần Chính 
        private async void btn_login_Click(object sender, EventArgs e)
        {
            string email = tb_email.Text;
            string password = tb_pass.Text;

            if (email == "" || password == "")
            {
                MessageBox.Show("Email và mật khẩu không được để trống!",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var result = await _authService.LoginAsync(email, password);

                // BƯỚC 2: Truyền Token vào FileTransferClient
                _fileClient.SetAuthToken(result.Token);
                // 1. Đảm bảo đã kết nối đến Server TCP
                await EnsureConnectedAsync();
                MessageBox.Show("Đăng nhập và xác thực thành công!");
                this.Hide();
                MainMenu main = new MainMenu(_fileClient, _authService);
                main.ShowDialog();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private async Task EnsureConnectedAsync()
        {
            if (_fileClient.IsConnected) return;
            try
            {
                await _fileClient.EnsureConnectedAsync(); 
            }
            catch (Exception ex)
            {
                throw new Exception($"Kết nối TCP thất bại: {ex.Message}");
            }
        }
        private void btn_signup_Click(object sender, EventArgs e)
        {
            var signup = new SignUp(_authService);
            signup.ShowDialog();
        }

        private void llb_forgotPass_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ForgotPass forgotPass = new ForgotPass(_authService);
            forgotPass.ShowDialog();
        }
    }
}
