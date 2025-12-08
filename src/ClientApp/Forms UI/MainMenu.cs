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
            txtSearch.Text = "🔍 Tìm kiếm File";
            txtSearch.ForeColor = Color.Gray;

            txtSearch.Enter += (s, e) =>
            {
                if (txtSearch.Text == "🔍 Tìm kiếm File")
                {
                    txtSearch.Text = "";
                    txtSearch.ForeColor = Color.Black;
                }
            };

            txtSearch.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    txtSearch.Text = "🔍 Tìm kiếm File";
                    txtSearch.ForeColor = Color.Gray;
                }
            };

            button1_Click(null, null);


            sidebar.Width = 64;

            // 3. Ẩn chữ ngay lập tức
            btn_Logout.Text = "";
            btn_filetype.Text = "";

            // 4. Gọi hàm chỉnh kích thước để thanh tìm kiếm DÀI RA lấp chỗ trống
            DieuChinhKichThuoc();
            sidebar.BringToFront();
        }

        private void MainMenu_Load(object sender, EventArgs e)
        {

        }

        private async void btnLogout_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel2.Controls.Clear();

            Home tc = new Home();
            tc.Dock = DockStyle.Fill;
            panel2.Controls.Add(tc);

            tc.BringToFront();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            panel2.Controls.Clear();

            Downloaded dt = new Downloaded();
            dt.Dock = DockStyle.Fill;

            panel2.Controls.Add(dt);

            dt.BringToFront();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel2.Controls.Clear();

            RiengTu rt = new RiengTu();
            rt.Dock = DockStyle.Fill;
            panel2.Controls.Add(rt);

            rt.BringToFront();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            panel2.Controls.Clear();

            trash tr = new trash();
            tr.Dock = DockStyle.Fill;
            panel2.Controls.Add(tr);

            tr.BringToFront();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel2.Controls.Clear();

            Setting cd = new Setting();
            cd.Dock = DockStyle.Fill;
            panel2.Controls.Add(cd);

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

        bool sidebarExpand = false;
        private void sideBarTransition_Tick(object sender, EventArgs e)
        {
            if (sidebarExpand)
            {
                // Đang MỞ (488) -> Thu về NHỎ (91)
                sidebar.Width -= 50; // Tăng tốc độ lên 20 cho dứt khoát
                if (sidebar.Width <= 55)
                {
                    sidebarExpand = false;
                    sideBarTransition.Stop();
                    sidebar.Width = 55; // Chốt số

                    // Ẩn chữ
                    btn_Logout.Text = "";
                    btn_filetype.Text = "";
                }
            }
            else
            {
                // Đang NHỎ (91) -> Mở ra TO (488)
                sidebar.Width += 50;
                if (sidebar.Width >= 203)
                {
                    sidebarExpand = true;
                    sideBarTransition.Stop();
                    sidebar.Width = 203; // Chốt số

                    // Hiện chữ
                    btn_Logout.Text = "     Log out";
                    btn_filetype.Text = "      File type";
                }
            }
            DieuChinhKichThuoc();
        }

        private void DieuChinhKichThuoc()
        {
            // --- 1. HEADER (PANEL 4) - Dính sát vào sidebar ---
            panel4.Left = sidebar.Width;
            panel4.Width = this.ClientSize.Width - sidebar.Width;

            // --- 2. XỬ LÝ THANH TÌM KIẾM (roundedPanel1) ---
            if (roundedPanel1 != null)
            {
                // Đảm bảo cha là panel4
                if (roundedPanel1.Parent != panel4) roundedPanel1.Parent = panel4;

                // --- CẤU HÌNH ĐỘ CO DÃN ---
                // Tổng khoảng cách chừa ra 2 bên (cho Logo, Text, Nút chuông...)
                // Số càng LỚN thì thanh tìm kiếm càng NGẮN lại.
                int leHaiBen = 400;

                // Tính toán chiều rộng tự động
                int widthMoi = panel4.Width - leHaiBen;

                // Giới hạn an toàn: Không cho bé quá 250px
                if (widthMoi < 250) widthMoi = 250;

                roundedPanel1.Width = widthMoi;
                roundedPanel1.Height = 45; // Chiều cao cố định

                // CĂN GIỮA CHIỀU NGANG
                roundedPanel1.Left = (panel4.Width - roundedPanel1.Width) / 2;

                // CĂN GIỮA CHIỀU DỌC (Quan trọng)
                roundedPanel1.Top = (panel4.Height - roundedPanel1.Height) / 2;

                roundedPanel1.BringToFront();
                roundedPanel1.Invalidate();

                // --- CẬP NHẬT Ô NHẬP LIỆU BÊN TRONG (txtSearch) ---
                if (txtSearch != null)
                {
                    if (txtSearch.Parent != roundedPanel1) txtSearch.Parent = roundedPanel1;

                    txtSearch.Width = roundedPanel1.Width - 40;
                    txtSearch.Left = (roundedPanel1.Width - txtSearch.Width) / 2;
                    txtSearch.Top = (roundedPanel1.Height - txtSearch.Height) / 2;
                    txtSearch.BringToFront();
                }
            }

            // --- 3. XỬ LÝ PHẦN THÂN (PANEL 2) ---
            if (panel2 != null)
            {
                panel2.Dock = DockStyle.None;
                panel2.Anchor = AnchorStyles.Top | AnchorStyles.Left;

                // Dính sát vào sidebar giống Header
                panel2.Left = sidebar.Width;
                panel2.Width = this.ClientSize.Width - sidebar.Width;

                panel2.Top = panel4.Bottom; // Nằm ngay dưới Header

                // Tính chiều cao (Trừ đi thanh menu đáy khoảng 80px)
                int heightMoi = this.ClientSize.Height - panel2.Top - 80;

                if (heightMoi > 0)
                {
                    panel2.Height = heightMoi;
                }

                // Đưa các thành phần quan trọng lên trên
                panel4.BringToFront();
                sidebar.BringToFront();
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

        }

        private async void btn_Logout_Click_1(object sender, EventArgs e)
        {
            try
            {
                _authService.Logout();
                await _fileClient.DisconnectAsync();

                MessageBox.Show("Đăng xuất thành công!");

                // Lệnh này sẽ tắt hết và mở lại app từ đầu (về lại màn Login)
                Application.Restart();
                Environment.Exit(0); // Đảm bảo tắt hẳn process cũ
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi đăng xuất: {ex.Message}");
            }
        }
        bool fileExpand = false;
        private void fileTransition_Tick(object sender, EventArgs e)
        {
            if (fileExpand == false)
            {
                changefile.Height += 60;
                if (changefile.Height >= 260)
                {
                    fileTransition.Stop();
                    fileExpand = true;
                }
            }
            else
            {
                changefile.Height -= 60;
                if (changefile.Height <= 52)
                {
                    fileTransition.Stop();
                    fileExpand = false;
                }
            }
        }

        private void btn_changefile_Click(object sender, EventArgs e)
        {
            fileTransition.Start();
        }

        private void btn_pdf_Click(object sender, EventArgs e)
        {
            panel2.Controls.Clear();

            PDF pdf = new PDF();
            pdf.Dock = DockStyle.Fill;
            panel2.Controls.Add(pdf);

            pdf.BringToFront();
        }

        private void btn_word_Click(object sender, EventArgs e)
        {
            panel2.Controls.Clear();

            Word word = new Word();
            word.Dock = DockStyle.Fill;
            panel2.Controls.Add(word);

            word.BringToFront();
        }

        private void btn_excel_Click(object sender, EventArgs e)
        {
            panel2.Controls.Clear();

            Excel ex = new Excel();
            ex.Dock = DockStyle.Fill;
            panel2.Controls.Add(ex);

        }

        private void btn_txt_Click(object sender, EventArgs e)
        {
            panel2.Controls.Clear();

            TXT txt = new TXT();
            txt.Dock = DockStyle.Fill;
            panel2.Controls.Add(txt);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            panel2.Controls.Clear();

            Star s = new Star();
            s.Dock = DockStyle.Fill;
            panel2.Controls.Add(s);
        }

        private void btn_ThongBao_Click(object sender, EventArgs e)
        {
            panel2.Controls.Clear();

            Notification tc = new Notification();
            tc.Dock = DockStyle.Fill;
            panel2.Controls.Add(tc);

            tc.BringToFront();
        }

        private void btn_Notification_Click(object sender, EventArgs e)
        {
            btn_Notification.Visible = false;

            // Hiện cái nút tắt lên
            btn_offNotification.Visible = true;

            // Hiện thông báo
            MessageBox.Show("Bạn đã TẮT thông báo!", "Thông báo");
        }

        private void btn_offNotification_Click(object sender, EventArgs e)
        {
            btn_offNotification.Visible = false;

            // Hiện lại cái nút bật
            btn_Notification.Visible = true;

            // Hiện thông báo
            MessageBox.Show("Bạn đã BẬT thông báo!", "Thông báo");
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
