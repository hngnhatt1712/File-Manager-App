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
using SharedLibrary;

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
        private void ShowPage(UserControl page)
        {
            _isTrashMode = false; // Mặc định tắt trash mode khi chuyển trang

            // 1. Dọn dẹp
            flowLayoutPanel1.Controls.Clear();

            // 2. Thiết lập kích thước cho trang con
            // Vì FlowLayoutPanel không hỗ trợ Dock.Fill tốt, ta ép kích thước bằng tay
            page.Width = flowLayoutPanel1.Width - 10; // Trừ hao khoảng cách thanh cuộn
            page.Height = flowLayoutPanel1.Height - 10;

            // 3. Thêm vào khung hiển thị
            flowLayoutPanel1.Controls.Add(page);
            page.BringToFront();
        }
        private void MainMenu_Load(object sender, EventArgs e)
        {

        }

        private async void btnLogout_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            _isTrashMode = false; // QUAN TRỌNG: Tắt chế độ thùng rác
            ShowPage(new Home(_fileClient));
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();

            ShowPage(new Downloaded(_fileClient));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            ShowPage(new RiengTu(_fileClient));
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            _isTrashMode = true; // Đánh dấu đang xem Thùng rác

            var danhSachFileRac = await _fileClient.GetTrashFilesAsync();

            // Tận dụng hàm vẽ Card file đã có ở Ảnh 1
            RenderFileList(danhSachFileRac);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();

            ShowPage(new Setting());
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
            if (flowLayoutPanel1 != null)
            {
                flowLayoutPanel1.Dock = DockStyle.None;
                flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Left;

                // Dính sát vào sidebar giống Header
                flowLayoutPanel1.Left = sidebar.Width;
                flowLayoutPanel1.Width = this.ClientSize.Width - sidebar.Width;

                flowLayoutPanel1.Top = panel4.Bottom; // Nằm ngay dưới Header

                // Tính chiều cao (Trừ đi thanh menu đáy khoảng 80px)
                int heightMoi = this.ClientSize.Height - flowLayoutPanel1.Top - 80;

                if (heightMoi > 0)
                {
                    flowLayoutPanel1.Height = heightMoi;
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
            flowLayoutPanel1.Controls.Clear();

            ShowPage(new PDF(_fileClient));
        }

        private void btn_word_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();

            ShowPage(new Word(_fileClient));
        }

        private void btn_excel_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();

            ShowPage(new Excel(_fileClient));

        }

        private void btn_txt_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();

            ShowPage(new TXT(_fileClient));
        }

        private void button8_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();

            ShowPage(new Star(_fileClient));
        }

        private void btn_ThongBao_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();

            ShowPage(new Notification());
        }

        private void btn_Notification_Click(object sender, EventArgs e)
        {
            
        }

        private void btn_offNotification_Click(object sender, EventArgs e)
        {
        
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

                // Hiển thị kết quả 
                RenderFileList(ketQua);

                e.SuppressKeyPress = true; 
            }
        }

        // z: Hàm này dùng để vẽ các file tìm được lên màn hình
        private FileItem _currentSelectedItem = null;
        private void RenderFileList(List<FileMetadata> danhSachFile)
        {
            // 1. Đảm bảo chạy trên luồng giao diện (tránh lỗi Cross-thread)
            if (flowLayoutPanel1.InvokeRequired)
            {
                flowLayoutPanel1.Invoke(new Action(() => RenderFileList(danhSachFile)));
                return;
            }

            // 2. Xóa danh sách cũ
            flowLayoutPanel1.Controls.Clear();

            // 3. Xử lý trường hợp danh sách rỗng
            if (danhSachFile == null || danhSachFile.Count == 0)
            {
                Label lblEmpty = new Label();
                lblEmpty.Text = "📂 Thư mục trống";
                lblEmpty.AutoSize = false;
                lblEmpty.Width = flowLayoutPanel1.Width - 10;
                lblEmpty.TextAlign = ContentAlignment.MiddleCenter;
                lblEmpty.Font = new Font("Segoe UI", 12, FontStyle.Italic);
                lblEmpty.ForeColor = Color.Gray;
                lblEmpty.Padding = new Padding(0, 20, 0, 0); 
                flowLayoutPanel1.Controls.Add(lblEmpty);
                return;
            }

            // 4. Tạo UserControl cho từng file
            foreach (var file in danhSachFile)
            {
                // Khởi tạo FileItem với dữ liệu file
                FileItem item = new FileItem(file);

                // --- CẤU HÌNH GIAO DIỆN ---
                item.Width = flowLayoutPanel1.Width - 25;
                item.Margin = new Padding(0, 0, 0, 2);

                // --- BẮT SỰ KIỆN TỪ CÁC NÚT BẤM (GỌI THẲNG HÀM LOGIC) ---
                item.OnDeleteClicked += (s, f) => XoaFile(f);      // Nút Xóa
                item.OnDownloadClicked += (s, f) => TaiFile(f);    // Nút Tải

                item.OnRenameClicked += (s, f) => DoiTenFile(s, f);   // Nút Đổi tên
                item.OnStarClicked += (s, f) => DanhDauSao(s, f);      // Nút Sao

                // --- BẮT SỰ KIỆN CLICK VÀO NỀN (HIGHLIGHT & MENU) ---
                item.MouseClick += (s, e) =>
                {
                    // Luôn Highlight item này dù bấm chuột trái hay phải
                    HighlightItem(item);

                    // Nếu là chuột phải -> Hiện Menu Context
                    if (e.Button == MouseButtons.Right)
                    {
                        ShowContextMenu(item, Cursor.Position);
                    }
                };

                // QUAN TRỌNG: Thêm Item vào Panel 
                flowLayoutPanel1.Controls.Add(item);
            }
        }
        // Xử lý khi bấm nút Tải xuống
        private async void TaiFile(FileMetadata file)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = file.FileName; // Gợi ý tên file gốc
            sfd.Filter = "All files (*.*)|*.*";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    bool ok = await _fileClient.DownloadFileAsync(file.FileName, sfd.FileName);

                    if (ok) MessageBox.Show("Tải thành công!");
                    else MessageBox.Show("Lỗi: Không tìm thấy file trên Server hoặc mất kết nối!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi tải file: " + ex.Message);
                }
            }
        }
        
        // Xử lý khi bấm nút Đổi tên
        private async void DoiTenFile(object sender, FileMetadata file)
        {
            string newName = Microsoft.VisualBasic.Interaction.InputBox("Nhập tên mới:", "Đổi tên", file.FileName);

            if (!string.IsNullOrWhiteSpace(newName) && newName != file.FileName)
            {
                try
                {
                    // Gọi Server
                    bool ok = await _fileClient.RenameFileAsync(file.FileId, file.FileName, newName);
                    if (ok)
                    {
                        file.FileName = newName; 

                        if (sender is FileItem item)
                        {
                            item.SetFileName(newName);
                        }
                    }
                    else MessageBox.Show("Đổi tên thất bại!");
                }
                catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
            }
        }

        private async void DanhDauSao(object sender, FileMetadata file)
        {
            try
            {
                await _fileClient.ToggleStarAsync(file.FileId, file.IsStarred);

                bool trangThaiMoi = !file.IsStarred;
                file.IsStarred = trangThaiMoi;

                // 3. Đổi màu ngôi sao ngay lập tức cho đẹp
                if (sender is FileItem item)
                {
                    item.SetStarStatus(trangThaiMoi); 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
        // thực hiện chonn file 
        private void HighlightItem(FileItem clickedItem)
        {
            if (_currentSelectedItem != null)
            {
                _currentSelectedItem.BackColor = Color.White;
                _currentSelectedItem.BorderStyle = BorderStyle.None;
            }

            // 2. Chọn cái mới
            clickedItem.BackColor = Color.FromArgb(232, 240, 254); 
            clickedItem.BorderStyle = BorderStyle.FixedSingle;

            // 3. Lưu vết
            _currentSelectedItem = clickedItem;
            _selectedFile = clickedItem.FileData; // Lưu Metadata để dùng cho việc khác
        }
        private void ShowContextMenu(FileItem item, Point screenPosition)
        {
            ContextMenuStrip menu = new ContextMenuStrip();

            if (_isTrashMode) // Nếu đang ở chế độ xem thùng rác
            {
                menu.Items.Add("⏪ Khôi phục file", null, (s, e) => KhoiPhucFile(item.FileData));
                menu.Items.Add("🔥 Xóa vĩnh viễn", null, (s, e) => XoaVinhVien(item.FileData));
            }
            else // Chế độ bình thường
            {
                menu.Items.Add("👁 Xem nội dung", null, (s, e) => XemNoiDungFile(item.FileData));
                menu.Items.Add("📥 Tải xuống", null, (s, e) => TaiFile(item.FileData));
                menu.Items.Add("🗑 Đưa vào thùng rác", null, (s, e) => XoaFile(item.FileData));
            }

            menu.Show(screenPosition);
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
