using Newtonsoft.Json;
using ServerApp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
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
        public bool IsStarredMode { get; set; } = false;
        public bool IsLoaded { get; private set; } = false;
        private List<FileMetadata> _allFiles = new List<FileMetadata>();

        /// <summary>
        /// Remove Vietnamese diacritics (dấu) để hỗ trợ tìm kiếm không phân biệt dấu
        /// </summary>
        private string RemoveDiacritics(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;

            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        // Public method to search files by keyword and update UI
        public void SearchFiles(string keyword)
        {
            // Nếu keyword rỗng, hiện tất cả file
            if (string.IsNullOrWhiteSpace(keyword))
            {
                RenderFileList(_allFiles);
                return;
            }

            // Remove diacritics từ keyword để so sánh
            string normalizedKeyword = RemoveDiacritics(keyword.ToLower().Trim());

            var filtered = _allFiles
                .Where(f => RemoveDiacritics(f.FileName.ToLower()).Contains(normalizedKeyword))
                .ToList();
            RenderFileList(filtered);
        }
        public FileList()
        {
            InitializeComponent();
        }
        public void SetClient(FileTransferClient client)
        {
            _fileClient = client;
        }
        public void SetFiles(List<FileMetadata> files)
        {
            _allFiles = files ?? new List<FileMetadata>();
            IsLoaded = true;
            RenderFileList(_allFiles);
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
                    _allFiles = new List<FileMetadata>();
                    IsLoaded = true;
                    RenderFileList(new List<FileMetadata>());
                    return;
                }
              
                var listFiles = JsonConvert.DeserializeObject<List<FileMetadata>>(json);
                _allFiles = listFiles;
                IsLoaded = true;
                SetFiles(listFiles);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải file: " + ex.Message);
                _allFiles = new List<FileMetadata>();
                IsLoaded = true; // Set = true dù có lỗi, để search function vẫn hoạt động
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

                item.OnRenameClicked += async (s, f) => await DoiTenFile(item, f);   // Nút Đổi tên
                item.OnStarClicked += (s, f) => DanhDauSao(s, f);     // Nút Sao

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

        public void ApplySort(string option)
        {
            // 1. Kiểm tra nếu danh sách trống thì không làm gì cả
            if (_allFiles == null || _allFiles.Count == 0) return;

            IEnumerable<FileMetadata> query = _allFiles;

            // 2. Phân loại logic sắp xếp
            switch (option)
            {
                case "Tên (A -> Z)":
                    query = query.OrderBy(f => f.FileName);
                    break;

                case "Tên (Z -> A)":
                    query = query.OrderByDescending(f => f.FileName);
                    break;

                case "Ngày mới nhất":
                    // Sắp xếp Giảm dần (Descending) -> Ngày lớn nhất (mới nhất) lên đầu
                    query = query.OrderByDescending(f => DateTime.Parse(f.UploadedDate));
                    break;

                case "Ngày cũ nhất":
                    // Sắp xếp Tăng dần (Ascending) -> Ngày nhỏ nhất (cũ nhất) lên đầu
                    query = query.OrderBy(f => DateTime.Parse(f.UploadedDate));
                    break;

                default:
                    // Nếu không khớp cái nào thì giữ nguyên
                    break;
            }

            // 3. Gọi hàm vẽ lại giao diện với danh sách đã lọc
            RenderFileList(query.ToList());
        }

        private async void TaiFile(FileMetadata fileData)
        {
            try
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    // 1. Lấy đuôi file để xử lý Filter
                    string originalName = fileData.FileName;
                    string ext = System.IO.Path.GetExtension(originalName).ToLower();

                    // 2. Cấu hình SaveFileDialog
                    sfd.FileName = originalName;
                    sfd.DefaultExt = ext;
                    sfd.AddExtension = true;

                    // 3. Bộ lọc để hiện đúng Icon (Word, PDF...)
                    if (ext.Contains("pdf")) sfd.Filter = "PDF Documents|*.pdf|All Files|*.*";
                    else if (ext.Contains("doc")) sfd.Filter = "Word Documents|*.doc;*.docx|All Files|*.*";
                    else if (ext.Contains("xls")) sfd.Filter = "Excel Files|*.xls;*.xlsx|All Files|*.*";
                    else if (ext.Contains("txt")) sfd.Filter = "Text Files|*.txt|All Files|*.*";
                    else if (ext.Contains("png") || ext.Contains("jpg")) sfd.Filter = "Images|*.png;*.jpg;*.jpeg|All Files|*.*";
                    else sfd.Filter = "All Files|*.*";

                    // 4. Hiện hộp thoại
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        bool success = await _fileClient.DownloadFileAsync(fileData.FileName, sfd.FileName);

                        if (success)
                            MessageBox.Show("Tải xong rồi nha!", "Thành công");
                        else
                            MessageBox.Show("Lỗi tải file (Server hoặc Mạng).", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        // Xử lý khi bấm nút Đổi tên

        private async Task DoiTenFile(FileItem item, FileMetadata fileData)
        {
            // 1. Hiện hộp thoại nhập tên
            string promptValue = Microsoft.VisualBasic.Interaction.InputBox(
                "Nhập tên mới (không cần ghi đuôi file):",
                "Đổi tên file",
                fileData.FileName
            );

            // 2. Kiểm tra nếu user có nhập gì đó và khác tên cũ
            if (!string.IsNullOrWhiteSpace(promptValue) && promptValue != fileData.FileName)
            {
                string newNameInput = promptValue;

                // Gọi Server đổi tên
                bool result = await _fileClient.RenameFileAsync(fileData.FileId, fileData.FileName, newNameInput);

                if (result)
                {
                    MessageBox.Show("Đổi tên thành công!");

                    // --- CẬP NHẬT GIAO DIỆN TẠI CHỖ (Fix lỗi nhảy tab) ---

                    // Tự tính toán lại tên đầy đủ (kèm đuôi) để hiển thị cho đúng
                    string ext = System.IO.Path.GetExtension(fileData.FileName);
                    if (!newNameInput.EndsWith(ext)) newNameInput += ext;

                    // Update chữ trên giao diện ngay lập tức
                    item.SetFileName(newNameInput);

                    // Update lại biến dữ liệu gốc để lần sau đổi tiếp không bị lỗi
                    fileData.FileName = newNameInput;
                }
                else
                {
                    MessageBox.Show("Đổi tên thất bại (Trùng tên hoặc lỗi server).");
                }
            }
        }

        private async void DanhDauSao(object sender, FileMetadata file)
        {
            try
            {
                // 1. Gọi Server đảo ngược trạng thái
                bool success = await _fileClient.ToggleStarAsync(file.FileId, file.IsStarred);

                if (success)
                {
                    // 2. Tính toán trạng thái mới
                    bool trangThaiMoi = !file.IsStarred;

                    // 3. Cập nhật dữ liệu cục bộ
                    file.IsStarred = trangThaiMoi;

                    // 4. Cập nhật giao diện (Đổi màu ngôi sao)
                    if (sender is FileItem item)
                    {
                        item.SetStarStatus(trangThaiMoi);

                        if (IsStarredMode && trangThaiMoi == false)
                        {
                            flowLayoutPanel1.Controls.Remove(item);

                            // Nếu xóa hết thì hiện thông báo trống
                            if (flowLayoutPanel1.Controls.Count == 0)
                            {
                                Label lblEmpty = new Label();
                                lblEmpty.Text = "Không có file nào được đánh dấu.";
                                lblEmpty.AutoSize = false;
                                lblEmpty.Width = flowLayoutPanel1.Width;
                                lblEmpty.TextAlign = ContentAlignment.MiddleCenter;
                                flowLayoutPanel1.Controls.Add(lblEmpty);
                            }
                        }
                    }
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
