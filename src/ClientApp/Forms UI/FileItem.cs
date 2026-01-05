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
    public partial class FileItem : UserControl
    {
        public event EventHandler<FileMetadata> OnDownloadClicked;
        public event EventHandler<FileMetadata> OnDeleteClicked;
        public event EventHandler<FileMetadata> OnRenameClicked;
        public event EventHandler<FileMetadata> OnStarClicked;
        public FileMetadata FileData { get; private set; }
        public FileItem(FileMetadata metadata)
        {
            InitializeComponent();
            this.FileData = metadata;

            // 2. Gán dữ liệu lên giao diện
            if (lblFileName != null) lblFileName.Text = metadata.FileName;

            // Xử lý icon file 
            if (pbIcon != null) pbIcon.Image = GetIcon(metadata.FileName);

            // 3. GẮN SỰ KIỆN CHO CÁC NÚT BẠN ĐÃ KÉO THẢ

            if (btnDownload != null)
                btnDownload.Click += (s, e) => OnDownloadClicked?.Invoke(this, FileData);

            if (btnDelete != null)
                btnDelete.Click += (s, e) => OnDeleteClicked?.Invoke(this, FileData);

            if (btnRename != null)
                btnRename.Click += (s, e) => OnRenameClicked?.Invoke(this, FileData);

            if (btnStar != null)
                btnStar.Click += (s, e) => OnStarClicked?.Invoke(this, FileData);

            btnDelete.Cursor = Cursors.Hand;
            btnRename.Cursor = Cursors.Hand;
            btnStar.Cursor = Cursors.Hand;
            btnDownload.Cursor = Cursors.Hand;


            // 4. Hiệu ứng Hover cho đẹp (Tùy chọn)
            btnDelete.MouseEnter += (s, e) => btnDelete.BackColor = Color.LightGray;
            btnDelete.MouseLeave += (s, e) => btnDelete.BackColor = Color.Transparent;
            btnStar.MouseEnter += (s, e) => btnStar.BackColor = Color.LightGray;
            btnStar.MouseLeave += (s, e) => btnStar.BackColor = Color.Transparent;
            btnDownload.MouseEnter += (s, e) => btnDownload.BackColor = Color.LightGray;
            btnDownload.MouseLeave += (s, e) => btnDownload.BackColor = Color.Transparent;
            btnRename.MouseEnter += (s, e) => btnRename.BackColor = Color.LightGray;
            btnRename.MouseLeave += (s, e) => btnRename.BackColor = Color.Transparent;

            this.MouseEnter += (s, e) => this.BackColor = Color.FromArgb(245, 245, 245);
            this.MouseLeave += (s, e) => {
                if (!this.ClientRectangle.Contains(this.PointToClient(Control.MousePosition)))
                    this.BackColor = Color.White;
            };
        }
        private Image GetIcon(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return Properties.Resources.icon_default;
            }
            string ext = System.IO.Path.GetExtension(fileName)?.ToLower() ?? "";
            if (ext == ".doc" || ext == ".docx") return Properties.Resources.icon_word;
            if (ext == ".xls" || ext == ".xlsx") return Properties.Resources.icon_excel;
            if (ext == ".pdf") return Properties.Resources.icon_pdf;
            if (ext == ".txt") return Properties.Resources.icon_txt;
            if (ext == ".png" || ext == ".jpg" || ext == ".jpeg") return Properties.Resources.icon_image;
            return Properties.Resources.icon_default;
        }
        public void SetFileName(string newName)
        {
            if (lblFileName != null) lblFileName.Text = newName;
        }
        public void SetStarStatus(bool isStarred)
        {
            // Cập nhật lại dữ liệu nội bộ
            FileData.IsStarred = isStarred;

            // Đổi màu nút (Giả sử nút tên là btnStar)
            if (isStarred)
            {
                btnStar.Text = "★"; // Sao đặc
                btnStar.ForeColor = Color.Gold; // Màu vàng
            }
            else
            {
                btnStar.Text = "☆"; // Sao rỗng
                btnStar.ForeColor = Color.Gray; // Màu xám
            }
        }
    }
}
