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
    public partial class Home : UserControl, ISearchable, ISortable
    {
        private FileTransferClient _client;
        private List<FileMetadata> _cachedFiles = new List<FileMetadata>();
        public FileList FileListControl => homeFileList;
        public Home(FileTransferClient client)
        {
            InitializeComponent();
            _client = client;

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            
        }

        public void SearchFiles(string keyword)
        {
            homeFileList.SearchFiles(keyword);
        }

        public void ApplySort(string option)
        {
            homeFileList.ApplySort(option);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private async void TrangChu_Load(object sender, EventArgs e)
        {
            Console.WriteLine("[Home] TrangChu_Load được gọi");
            
            // Đảm bảo HomeFileList được khởi tạo
            if (homeFileList != null)
            {
                Console.WriteLine("[Home] homeFileList không null, tiến hành setup");
                homeFileList.SetClient(_client);
                homeFileList.SetTrashMode(false);
                
                Console.WriteLine("[Home] Bắt đầu LoadFilesFromServer");
                await homeFileList.LoadFilesFromServer("/");
                Console.WriteLine("[Home] LoadFilesFromServer kết thúc");
            }
            else
            {
                MessageBox.Show("[DEBUG] homeFileList là NULL! Không thể load.");
                Console.WriteLine("[Home] ERROR: homeFileList là NULL");
            }
        }

    }
}
