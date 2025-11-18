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
    public partial class Dashboard : Form
    {
        private readonly FileTransferClient _fileClient;
        private readonly FirebaseAuthService _authService;
        public Dashboard(FirebaseAuthService authService, FileTransferClient fileClient)
        {
            InitializeComponent();
            _authService = authService;
            _fileClient = fileClient;
        }



        private void Dashboard_Load(object sender, EventArgs e)
        {

        }

        private async void btnTestLogout_Click(object sender, EventArgs e)
        {
            string message = "Bạn có chắc chắn muốn đăng xuất không?";
            string caption = "Xác nhận Đăng xuất";
            var result = MessageBox.Show(message, caption,
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    _authService.Logout();

                    this.Close(); 
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi đăng xuất: {ex.Message}");
                    this.Close();
                }
            }
        } 
    }
}
