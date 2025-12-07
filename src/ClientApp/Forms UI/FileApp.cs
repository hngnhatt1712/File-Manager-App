using ClientApp.Services;
using SharedLibrary;
using System;
using System.Windows.Forms;

namespace ClientApp
{
    // Khởi tạo service
    public partial class FileApp : Form
    {
        private readonly UserAuth _authService;
        private readonly FileTransferClient _fileClient;
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ReleaseCapture();


        public FileApp()
        {
            InitializeComponent();
            _fileClient = new FileTransferClient();
            _authService = new UserAuth(_fileClient);
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


        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (e.Clicks == 1 && e.Y <= this.Height && e.Y >= 0)
                {
                    ReleaseCapture();
                    SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                }
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
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
                AuthResult result = await _authService.LoginAsync(email, password);

                // Truyền Token vào FileTransferClient
                // Kết nối đến Server TCP 
                if (!_fileClient.IsConnected)
                {
                    await _fileClient.ConnectAsync();
                }

                // Gọi hàm đồng bộ mới
                await _fileClient.SyncUserAsync(result.Token, result.Email, "");
                MessageBox.Show("Đăng nhập và kết nối Server thành công!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide();

                // Truyền UID sang Form chính để sau này dùng tải danh sách file
                MainMenu main = new MainMenu(_fileClient, _authService);
                main.ShowDialog();

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                button2.Enabled = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (tb_pass.PasswordChar == '\0')
            {
                button3.BringToFront();
                tb_pass.PasswordChar = '*';
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (tb_pass.PasswordChar == '*')
            {
                button4.BringToFront();
                tb_pass.PasswordChar = '\0';
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ForgotPass forgotPass = new ForgotPass(_authService);
            forgotPass.ShowDialog();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var signup = new SignUp(_authService);
            signup.ShowDialog();
        }
    }
}
