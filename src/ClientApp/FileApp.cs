using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.CompilerServices;

namespace ClientApp
{
    public partial class FileApp : Form
    {
        private readonly FileTransferClient _fileClient;
        private readonly FirebaseAuthService _authService;

        public FileApp()
        {
            InitializeComponent();
            _fileClient = new FileTransferClient();
            _authService = new FirebaseAuthService();
        }

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
                await EnsureConnectedAsync();

                await _authService.LoginAsync(email, password, _fileClient);

                MessageBox.Show("Đăng nhập và xác thực thành công!");
                this.Hide();

                Dashboard dashboard = new Dashboard(_authService, _fileClient);
                dashboard.ShowDialog(); 

         
                this.Show(); 
                tb_pass.Clear();
                tb_email.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.Show();
            }
        }

        private void btn_signup_Click(object sender, EventArgs e)
        {
            SignUp signUp = new SignUp(_authService, _fileClient);
            signUp.ShowDialog();
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
