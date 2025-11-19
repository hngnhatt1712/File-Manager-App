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

        private async void btn_login_Click(object sender, EventArgs e)
        {
           
        }
        private async Task EnsureConnectedAsync()
        {
            
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
