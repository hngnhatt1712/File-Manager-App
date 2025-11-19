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

            try
            {
                // 1. Đảm bảo đã kết nối đến Server TCP
                await EnsureConnectedAsync();

                // 2. Gọi hàm Login mới
                await _authService.LoginAsync(email, password);

                MessageBox.Show("Đăng nhập và xác thực thành công!");
                this.Hide();
                MainMenu main = new MainMenu(_fileClient);
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
                string ip = "127.0.0.1";
                int port = 8888;
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
