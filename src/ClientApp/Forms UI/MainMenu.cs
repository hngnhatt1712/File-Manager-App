using ClientApp.Forms_UI;
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

namespace ClientApp
{
    public partial class MainMenu : Form
    {

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        private readonly FileTransferClient _fileClient;
        private readonly UserAuth _authService;
        public MainMenu(FileTransferClient fileClient, UserAuth authService)
        {
            InitializeComponent();
            _fileClient = fileClient;
            _authService = authService;
            // Đặt sự kiện cho ô tìm kiếm (giả sử tên là txtSearch)
            txtSearch.Text = "Tìm kiếm File";
            txtSearch.ForeColor = Color.Gray;

            txtSearch.Enter += (s, e) =>
            {
                if (txtSearch.Text == "Tìm kiếm File")
                {
                    txtSearch.Text = "";
                    txtSearch.ForeColor = Color.Black;
                }
            };

            txtSearch.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    txtSearch.Text = "Tìm trong Drive";
                    txtSearch.ForeColor = Color.Gray;
                }
            };

            button1_Click(null, null);

        }

        private void MainMenu_Load(object sender, EventArgs e)
        {

        }

        private async void btnLogout_Click(object sender, EventArgs e)
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

        private void button1_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();

            TrangChu tc = new TrangChu();
            tc.Dock = DockStyle.Fill;
            flowLayoutPanel1.Controls.Add(tc);

            tc.BringToFront();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();

            DaTai dt = new DaTai();
            dt.Dock = DockStyle.Fill;
            flowLayoutPanel1.Controls.Add(dt);

            dt.BringToFront();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();

            RiengTu rt = new RiengTu();
            rt.Dock = DockStyle.Fill;
            flowLayoutPanel1.Controls.Add(rt);

            rt.BringToFront();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();

            ThungRac tr = new ThungRac();
            tr.Dock = DockStyle.Fill;
            flowLayoutPanel1.Controls.Add(tr);

            tr.BringToFront();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();

            CaiDat cd = new CaiDat();
            cd.Dock = DockStyle.Fill;
            flowLayoutPanel1.Controls.Add(cd);

            cd.BringToFront();
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            sideBarTransition.Start();
        }

        bool sidebarExpand = true;
        private void sideBarTransition_Tick(object sender, EventArgs e)
        {
            if (sidebarExpand)
            {
                sidebar.Width -= 5;
                if (sidebar.Width <= 91)
                {
                    sidebarExpand = false;
                    sideBarTransition.Stop();
                    button8.Width = sidebar.Width;
                    btn_Logout.Width = sidebar.Width;
                }
            }
            else
            {
                sidebar.Width += 5;
                if (sidebar.Width >= 306)
                {
                    sidebarExpand = true;
                    sideBarTransition.Stop();

                    button8.Width = sidebar.Width;
                    btn_Logout.Width = sidebar.Width;
                }
            }
        }

        private void MainMenu_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void panel3_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                // Gửi lệnh di chuyển đến Handle của chính Form này (this.Handle)
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private async void btn_Logout_Click(object sender, EventArgs e)
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
