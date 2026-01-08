using Newtonsoft.Json;
using ServerApp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharedLibrary;

namespace ClientApp.Forms_UI
{
    public partial class Downloaded : UserControl
    {
        private FileTransferClient _client;
        public event EventHandler DataChanged;
        public Downloaded()
        {
            InitializeComponent();
        }

        public Downloaded(FileTransferClient client) : this()
        {
            _client = client;
        }


        private async void Downloaded_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;

            pnlDropZone.AllowDrop = true;
            pnlDropZone.DragEnter += PnlDropZone_DragEnter;
            pnlDropZone.DragDrop += PnlDropZone_DragDrop;
        }
        private void PnlDropZone_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
        // Khi thả chuột ra -> Lấy danh sách đường dẫn
        private async void PnlDropZone_DragDrop(object sender, DragEventArgs e)
        {
            string[] paths = (string[])e.Data.GetData(DataFormats.FileDrop);

            // Xử lý từng đường dẫn (vì người dùng có thể kéo 1 lúc nhiều file)
            foreach (string path in paths)
            {
                await AnalyzeAndUpload(path);
            }
        }

        private async void btnFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true; // Cho phép chọn nhiều file
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in ofd.FileNames)
                {
                    await AnalyzeAndUpload(file);
                }
            }
            DataChanged?.Invoke(this, EventArgs.Empty);
        }
        private async void btnFolder_Click(object sender, EventArgs e)
        {
            
        }
        private async Task AnalyzeAndUpload(string path)
        {
            // Kiểm tra xem là File hay Folder
            if (File.Exists(path))
            {
                // Là File -> Upload File
                await ProcessUploadFile(path);
            }
            else if (Directory.Exists(path))
            {
                // Là Folder -> Upload Folder 
                // Nếu chưa có hàm UploadFolder, bạn có thể duyệt đệ quy để lấy hết file bên trong
                string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    await ProcessUploadFile(file);
                }
            }
        }
        private async Task ProcessUploadFile(string filePath)
        {
            string fileName = Path.GetFileName(filePath);

            // 1. KHAI BÁO BIẾN Ở NGOÀI (Để tí nữa dùng lại được)
            Panel pnlItem = null;
            Label lblStatus = null;
            ProgressBar prog = null;
            string errorDetails = ""; // Lưu lỗi chi tiết

            // 2. TẠO GIAO DIỆN (Invoke lần 1)
            flpHistory.Invoke(new Action(() => {
                pnlItem = new Panel();
                pnlItem.Width = flpHistory.ClientSize.Width - 5;
                pnlItem.Height = 65;
                pnlItem.BackColor = Color.White;
                pnlItem.Margin = new Padding(0, 0, 0, 2);
                pnlItem.BorderStyle = BorderStyle.None;

                Label lblName = new Label
                {
                    Text = "📄 " + fileName,
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    Location = new Point(15, 22),
                    Size = new Size((int)(pnlItem.Width * 0.4), 20),
                    AutoEllipsis = true
                };

                // Gán vào biến đã khai báo ở trên
                prog = new ProgressBar
                {
                    Location = new Point((int)(pnlItem.Width * 0.45), 25),
                    Size = new Size((int)(pnlItem.Width * 0.3), 15),
                    Style = ProgressBarStyle.Marquee, // Đang chạy
                    MarqueeAnimationSpeed = 30
                };

                // Gán vào biến đã khai báo ở trên
                lblStatus = new Label
                {
                    Text = "⏳ Đang tải...",
                    ForeColor = Color.DimGray,
                    Font = new Font("Segoe UI", 8, FontStyle.Italic),
                    Location = new Point((int)(pnlItem.Width * 0.8), 23),
                    AutoSize = true
                };

                pnlItem.Controls.Add(lblName);
                pnlItem.Controls.Add(prog);
                pnlItem.Controls.Add(lblStatus);

                flpHistory.Controls.Add(pnlItem);
                flpHistory.Controls.SetChildIndex(pnlItem, 0);
                flpHistory.ScrollControlIntoView(pnlItem);
            }));

            // 3. XỬ LÝ UPLOAD
            bool uploadSuccess = false;
            try
            {
                // Kiểm tra file có tồn tại không
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException($"File không tồn tại: {filePath}");
                }

                // Kiểm tra client đã kết nối chưa
                if (_client == null || !_client.IsConnected)
                {
                    throw new Exception("Không kết nối được đến Server. Vui lòng đăng nhập lại.");
                }

                await _client.UploadFileAsync(filePath, "/");
                uploadSuccess = true;
            }
            catch (Exception ex)
            {
                uploadSuccess = false;
                errorDetails = ex.Message; // Lưu lỗi chi tiết
                
                // In log để debug
                Console.WriteLine($"[Upload Error] {fileName}: {ex.Message}");
                Console.WriteLine($"[Stack Trace] {ex.StackTrace}");
            }

            flpHistory.Invoke(new Action(() => {
                if (lblStatus != null && prog != null)
                {
                    if (uploadSuccess)
                    {
                        lblStatus.Text = "✅ Hoàn tất";
                        lblStatus.ForeColor = Color.Green;
                        prog.Style = ProgressBarStyle.Blocks;
                        prog.Value = 100;
                    }
                    else
                    {
                        // Hiển thị lỗi chi tiết (rút ngắn nếu quá dài)
                        string displayError = errorDetails.Length > 40 
                            ? errorDetails.Substring(0, 37) + "..." 
                            : errorDetails;

                        lblStatus.Text = $"❌ {displayError}";
                        lblStatus.ForeColor = Color.Red;
                        prog.Style = ProgressBarStyle.Blocks;
                        prog.Value = 0;
                    }
                }
            }));

            if (uploadSuccess)
            {
                DataChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
