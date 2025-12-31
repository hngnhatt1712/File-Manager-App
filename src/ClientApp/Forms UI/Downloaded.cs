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
        public Downloaded(FileTransferClient client)
        {
            InitializeComponent();
            _client = client;
            pnlDropZone.AllowDrop = true;
            pnlDropZone.DragEnter += PnlDropZone_DragEnter;
            pnlDropZone.DragDrop += PnlDropZone_DragDrop;
        }

        private async void Downloaded_Load(object sender, EventArgs e)
        {
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

            // A. TẠO GIAO DIỆN DÒNG TRẠNG THÁI (Code vẽ Panel động)
            Panel pnlItem = new Panel();
            pnlItem.Size = new Size(flpHistory.Width - 25, 50); // Chiều cao 50px
            pnlItem.BackColor = Color.WhiteSmoke;
            pnlItem.Margin = new Padding(0, 0, 0, 5);
            pnlItem.BorderStyle = BorderStyle.FixedSingle;

            // Icon (hoặc text tên file)
            Label lblName = new Label();
            lblName.Text = "📄 " + fileName; // Thêm icon text cho sinh động
            lblName.Location = new Point(10, 15);
            lblName.AutoSize = true;
            lblName.Font = new Font("Segoe UI", 10, FontStyle.Regular);

            // Thanh Progress Bar
            ProgressBar prog = new ProgressBar();
            prog.Location = new Point(300, 15); // Căn chỉnh tọa độ tùy độ rộng màn hình bạn
            prog.Width = 200;
            prog.Height = 20;
            prog.Style = ProgressBarStyle.Marquee; // Chạy qua chạy lại (Đang xử lý)

            // Label Trạng thái
            Label lblStatus = new Label();
            lblStatus.Text = "⏳ Đang tải...";
            lblStatus.Location = new Point(520, 15);
            lblStatus.AutoSize = true;
            lblStatus.ForeColor = Color.Blue;

            // Thêm các control vào Panel con
            pnlItem.Controls.Add(lblName);
            pnlItem.Controls.Add(prog);
            pnlItem.Controls.Add(lblStatus);

            // Thêm Panel con vào danh sách (Thêm lên đầu để dễ thấy mới nhất)
            flpHistory.Controls.Add(pnlItem);
            flpHistory.Controls.SetChildIndex(pnlItem, 0);

            // B. GỌI LOGIC UPLOAD (NETWORK)
            try
            {
                // Gọi hàm upload của Client
                // Đảm bảo hàm UploadFileAsync của bạn trả về Task (await được)
                await _client.UploadFileAsync(filePath, "/");

                // C. CẬP NHẬT KHI THÀNH CÔNG
                prog.Style = ProgressBarStyle.Blocks;
                prog.Value = 100;
                lblStatus.Text = "✅ Hoàn tất";
                lblStatus.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                // D. CẬP NHẬT KHI CÓ LỖI
                prog.Style = ProgressBarStyle.Blocks;
                prog.Value = 0; // Hoặc màu đỏ nếu set được state
                lblStatus.Text = "❌ Lỗi";
                lblStatus.ForeColor = Color.Red;

                // Tooltip để xem chi tiết lỗi khi di chuột vào chữ Lỗi
                ToolTip tt = new ToolTip();
                tt.SetToolTip(lblStatus, ex.Message);
            }
        }
    }
}
