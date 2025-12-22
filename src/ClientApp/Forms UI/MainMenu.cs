using ClientApp.Forms_UI;
using ClientApp.Services;
using ServerApp;
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
        private bool _isTrashMode = false; // Mặc định là không phải thùng rác
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
            _isTrashMode = false; // QUAN TRỌNG: Tắt chế độ thùng rác
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

        private async void button5_Click(object sender, EventArgs e)
        {
            panel2.Controls.Clear();
            _isTrashMode = true; // Đánh dấu đang xem Thùng rác

            var danhSachFileRac = await _fileClient.GetTrashFilesAsync();

            // Tận dụng hàm vẽ Card file đã có ở Ảnh 1
            HienThiKetQua(danhSachFileRac);
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

        private FileMetadata _selectedFile = null;
        private Panel _lastSelectedPanel = null;

        // thực hiện tìm kiếm 
        private async void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string query = txtSearch.Text.Trim();
                if (string.IsNullOrEmpty(query)) return;
                var ketQua = await _fileClient.SearchFilesAsync(query);

                // Hiển thị kết quả (Bạn cần viết hàm vẽ các Control file ra panel2)
                HienThiKetQua(ketQua);

                e.SuppressKeyPress = true; 
            }
        }

        // z: Hàm này dùng để vẽ các file tìm được lên màn hình
        private void HienThiKetQua(List<FileMetadata> danhSachFile)
        {
            panel2.Controls.Clear();
            if (danhSachFile == null || danhSachFile.Count == 0)
            {
                Label lblEmpty = new Label();
                lblEmpty.Text = "❌ Không tìm thấy file nào khớp với từ khóa!";
                lblEmpty.AutoSize = true;
                lblEmpty.ForeColor = Color.Red;
                lblEmpty.Font = new Font("Segoe UI", 12, FontStyle.Italic);
                panel2.Controls.Add(lblEmpty);
                return;
            }
            foreach (var file in danhSachFile)
            {
                Panel pnlCard = new Panel();
                pnlCard.Size = new Size(120, 150); 
                pnlCard.Margin = new Padding(15);
                pnlCard.BackColor = Color.White;
                pnlCard.Cursor = Cursors.Hand;

            
                // 2. TẠO PICTUREBOX ĐỂ HIỆN LOGO ẢNH
                PictureBox picIcon = new PictureBox();
                picIcon.Dock = DockStyle.Top;
                picIcon.Height = 90; 
                picIcon.SizeMode = PictureBoxSizeMode.Zoom;
                picIcon.BackColor = Color.Transparent; 
                picIcon.Padding = new Padding(10);
            
                string extension = Path.GetExtension(file.FileName).ToLower();

          
                if (extension == ".docx" || extension == ".doc")
                    picIcon.Image = Properties.Resources.icon_word ?? Properties.Resources.icon_default;
                else if (extension == ".xlsx" || extension == ".xls")
                    picIcon.Image = Properties.Resources.icon_excel ?? Properties.Resources.icon_default;
                else if (extension == ".pdf")
                    picIcon.Image = Properties.Resources.icon_pdf ?? Properties.Resources.icon_default;
                else if (extension == ".txt")
                    picIcon.Image = Properties.Resources.icon_txt ?? Properties.Resources.icon_default;
                else if (extension == ".jpg" || extension == ".png" || extension == ".jpeg")
                    picIcon.Image = Properties.Resources.icon_image ?? Properties.Resources.icon_default;
                else
                    picIcon.Image = Properties.Resources.icon_default;


                Label lblName = new Label();
                lblName.Text = file.FileName;
                lblName.Dock = DockStyle.Fill; 
                lblName.TextAlign = ContentAlignment.TopCenter;
                lblName.AutoEllipsis = true;
                lblName.Font = new Font("Segoe UI", 9, FontStyle.Regular);
                lblName.Padding = new Padding(5, 5, 5, 0); // Đệm thêm chút cho đẹp

                // 4. Hiệu ứng Click cho cả hộp
                // Cập nhật danh sách control: thay lblIcon bằng picIcon
                Control[] controls = { pnlCard, picIcon, lblName };
                foreach (Control c in controls)
                {
                    c.MouseEnter += (s, e) => { pnlCard.BackColor = Color.FromArgb(232, 240, 254); };
                    c.MouseLeave += (s, e) => { pnlCard.BackColor = Color.White; };
                    c.Click += (s, e) => {
                        ThucHienChonFile(file);
                    };
                }

                // Thêm các control con vào thẻ Card
                pnlCard.Controls.Add(lblName);
                pnlCard.Controls.Add(picIcon);

                panel2.Controls.Add(pnlCard);
            }
        }
        // thực hiện chonn file 
        private void ThucHienChonFile(FileMetadata file)
        {
            // 1. Lưu thông tin file vừa chọn
            _selectedFile = file;
            foreach (Control ctr in panel2.Controls)
            {
                if (ctr is Panel pnl)
                {
                    if (pnl.Controls.OfType<Label>().Any(l => l.Text == file.FileName))
                    {
                        // Bỏ chọn ô cũ (nếu có)
                        if (_lastSelectedPanel != null)
                        {
                            _lastSelectedPanel.BorderStyle = BorderStyle.None;
                            _lastSelectedPanel.BackColor = Color.White;
                        }

                        // Tô đậm ô mới
                        pnl.BorderStyle = BorderStyle.FixedSingle;
                        pnl.BackColor = Color.FromArgb(232, 240, 254); // Màu xanh nhạt Google
                        _lastSelectedPanel = pnl;
                        break;
                    }
                }
            }
            ContextMenuStrip menu = new ContextMenuStrip();

            if (_isTrashMode)
            {
                // MENU KHI ĐANG TRONG THÙNG RÁC
                menu.Items.Add("⏪ Khôi phục file", null, (s, e) => KhoiPhucFile(file));
                menu.Items.Add("🔥 Xóa vĩnh viễn", null, (s, e) => XoaVinhVien(file));
            }
            else
            {
                // MENU KHI Ở MÀN HÌNH CHÍNH
                menu.Items.Add("👁 Xem nội dung", null, (s, e) => XemNoiDungFile(file));
                menu.Items.Add("📥 Tải xuống", null, (s, e) => TaiFile(file));
                menu.Items.Add("🗑 Đưa vào thùng rác", null, (s, e) => XoaFile(file));
            }

            menu.Show(Cursor.Position);
        }

        private void XemNoiDungFile(FileMetadata file)
        {
            // Kiểm tra đuôi file, nếu là .txt thì mới cho xem
            if (file.FileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show($"Đang chuẩn bị đọc nội dung file: {file.FileName}");
            }
            else
            {
                MessageBox.Show("Chức năng xem nhanh hiện chỉ hỗ trợ file .txt");
            }
        }

        private async void TaiFile(FileMetadata file)
        {
            MessageBox.Show($"Đang bắt đầu tải: {file.FileName}...");
        }

        private async void XoaFile(FileMetadata file)
        {
            var result = MessageBox.Show($"Bạn có chắc muốn đưa {file.FileName} vào thùng rác?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                bool thanhCong = await _fileClient.MoveToTrashAsync(file.FileId);

                if (thanhCong)
                {
                    MessageBox.Show("Đã chuyển vào thùng rác thành công!");
                    txtSearch_KeyDown(null, new KeyEventArgs(Keys.Enter));
                }
            }
        }

        private async void KhoiPhucFile(FileMetadata file)
        {
            // Gọi Client gửi lệnh RESTORE (Bạn cần thêm hàm RestoreFileAsync vào FileTransferClient)
            bool thanhCong = await _fileClient.RestoreFileAsync(file.FileId);
            if (thanhCong)
            {
                MessageBox.Show("Đã khôi phục file!");
                button5_Click(null, null); // Load lại thùng rác
            }
        }

        // xóa vĩnh viễn file 
        private async void XoaVinhVien(FileMetadata file)
        {
            var confirm = MessageBox.Show("Hành động này không thể hoàn tác. Xóa vĩnh viễn?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm == DialogResult.Yes)
            {
                // Gọi Client gửi lệnh DELETE_PERMANENT 
                bool thanhCong = await _fileClient.DeletePermanentlyAsync(file.FileId);
                if (thanhCong)
                {
                    MessageBox.Show("Đã xóa vĩnh viễn!");
                    button5_Click(null, null); // Load lại thùng rác
                }
            }
        }
    }
}
