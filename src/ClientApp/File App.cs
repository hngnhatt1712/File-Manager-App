using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientApp
{
    public partial class File_App : Form
    {
        private readonly FileTransferClient _fileClient;
        private readonly FirebaseAuthService _authService;
        public File_App()
        {
            InitializeComponent();
            _fileClient = new FileTransferClient();
            _authService = new FirebaseAuthService();
        }

        private async void btn_login_Click(object sender, EventArgs e)
        {
            string email = tb_email.Text;
            string password = tb_pass.Text;

            try
            {
                // 1. Đảm bảo đã kết nối đến Server TCP
                await EnsureConnectedAsync();

                // 2. Gọi hàm Login mới
                await _authService.LoginAsync(email, password, _fileClient);

                MessageBox.Show("Đăng nhập và xác thực thành công!");
                this.Hide();
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_signup_Click(object sender, EventArgs e)
        {
            SignUp signUp = new SignUp(_authService, _fileClient);
            signUp.Show();
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

        private void llb_forgotPass_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ForgotPass forgotPass = new ForgotPass(_authService);
            forgotPass.ShowDialog();
        }
    }
}
