using Newtonsoft.Json;
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

namespace ClientApp.Forms_UI
{
    public partial class FileList : UserControl
    {
        private FileTransferClient _fileClient;
        private string _currentPath = "/";
        private bool _isTrashMode = false;
        private FileMetadata _selectedFile = null;
        public FileList()
        {
            InitializeComponent();
        }
        public void SetClient(FileTransferClient client)
        {
            _fileClient = client;
        }

        // 2. Hàm Tải dữ liệu từ Server (Dùng cho My File)
        public async Task LoadFilesFromServer(string path = "/")
        {
            if (_fileClient == null) return;
            _currentPath = path;

            try
            {
                // Gọi Client lấy JSON
                string json = await _fileClient.GetFileListAsync(path);
                if (string.IsNullOrEmpty(json) || json == "[]")
                {
                    RenderFileList(new List<FileMetadata>());
                    return;
                }
              
                var listFiles = JsonConvert.DeserializeObject<List<FileMetadata>>(json);
                RenderFileList(listFiles);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải file: " + ex.Message);
            }
        }
        // z: Hàm này dùng để vẽ các file tìm được lên màn hình
        private FileItem _currentSelectedItem = null;
        public void RenderFileList(List<FileMetadata> danhSachFile)
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

                item.OnRenameClicked += (s, f) => DoiTenFile(f);   // Nút Đổi tên
                item.OnStarClicked += (s, f) => DanhDauSao(f);     // Nút Sao

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

        }

        // Xử lý khi bấm nút Đổi tên
        private void DoiTenFile(FileMetadata file)
        {

        }

        // Xử lý khi bấm nút Sao (Yêu thích)
        private void DanhDauSao(FileMetadata file)
        {

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
                    await LoadFilesFromServer(_currentPath);
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
                await LoadFilesFromServer(_currentPath);
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
                    await LoadFilesFromServer(_currentPath);
                }
            }
        }
    }
}
