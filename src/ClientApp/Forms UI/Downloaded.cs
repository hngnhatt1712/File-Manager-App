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
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                await AnalyzeAndUpload(fbd.SelectedPath);
            }
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

            // Dùng Invoke để tránh lỗi khi cập nhật từ thread khác
            flpHistory.Invoke(new Action(() => {
                Panel pnlItem = new Panel();

                // 1. Tự động lấy chiều rộng thực tế của flpHistory
                // Trừ đi 20px để chừa chỗ cho thanh cuộn (scrollbar) không bị che mất
                pnlItem.Width = flpHistory.ClientSize.Width - 5;
                pnlItem.Height = 65;
                pnlItem.BackColor = Color.White;
                pnlItem.Margin = new Padding(0, 0, 0, 2); // Khít lề, chỉ hở dưới 2px để phân dòng
                pnlItem.BorderStyle = BorderStyle.None;

                // 2. Tên File (Chiếm 40% chiều rộng)
                Label lblName = new Label
                {
                    Text = "📄 " + fileName,
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    Location = new Point(15, 22),
                    Size = new Size((int)(pnlItem.Width * 0.4), 20),
                    AutoEllipsis = true
                };

                // 3. Progress Bar (Nằm ở giữa, bắt đầu từ 45% chiều rộng)
                ProgressBar prog = new ProgressBar
                {
                    Location = new Point((int)(pnlItem.Width * 0.45), 25),
                    Size = new Size((int)(pnlItem.Width * 0.3), 15), // Rộng 30% panel
                    Style = ProgressBarStyle.Marquee,
                    MarqueeAnimationSpeed = 30
                };

                // 4. Trạng thái (Nằm cuối cùng, căn lề phải)
                Label lblStatus = new Label
                {
                    Text = "⏳ Đang tải...",
                    ForeColor = Color.DimGray,
                    Font = new Font("Segoe UI", 8, FontStyle.Italic),
                    Location = new Point((int)(pnlItem.Width * 0.8), 23),
                    AutoSize = true
                };

                // Thêm vào Panel con
                pnlItem.Controls.Add(lblName);
                pnlItem.Controls.Add(prog);
                pnlItem.Controls.Add(lblStatus);

                // Thêm vào FlowLayoutPanel và đưa lên đầu danh sách
                flpHistory.Controls.Add(pnlItem);
                flpHistory.Controls.SetChildIndex(pnlItem, 0);

                // Tự động cuộn lên đầu để xem file mới nhất
                flpHistory.ScrollControlIntoView(pnlItem);
            }));

            // PHẦN LOGIC NETWORK (Giữ nguyên của bạn)
            try
            {
                await _client.UploadFileAsync(filePath, "/");

                // Cập nhật khi xong (Cần tìm lại các control bên trong pnlItem)
                flpHistory.Invoke(new Action(() => {
                    // Bạn có thể dùng Tag hoặc tìm Control theo Type để cập nhật ✅ Hoàn tất
                }));
            }
            catch (Exception ex)
            {
                // Cập nhật khi lỗi ❌
            }
        }
    }
}
