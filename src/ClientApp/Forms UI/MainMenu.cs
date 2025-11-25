using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClientApp.Services;

namespace ClientApp
{
    public partial class MainMenu : Form
    {
        private readonly FileTransferClient _fileClient;
        private readonly UserAuth _authService;
        public MainMenu(FileTransferClient fileClient, UserAuth authService)
        {
            InitializeComponent();
            _fileClient = fileClient;
            _authService = authService;
        }

        private void MainMenu_Load(object sender, EventArgs e)
        {

        }

        private async  void btnLogout_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Gọi Service Auth để đăng xuất Firebase
                _authService.Logout();

                // 2. Gọi Service File để ngắt kết nối TCP
                await _fileClient.DisconnectAsync();

                // 3. Reset giao diện về màn hình đăng nhập
                // Hiện giao diện đăng nhập     
                // Xóa trắng các ô nhập liệu cũ
                
                
                MessageBox.Show("Đăng xuất thành công!");
                FileApp f = new FileApp();
                f.ShowDialog();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi đăng xuất: {ex.Message}");
            }
        }
    }
}
