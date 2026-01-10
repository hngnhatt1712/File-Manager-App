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
            homeFileList.SetClient(_client);
            await homeFileList.LoadFilesFromServer("/");

            // Logic riêng của Home: Lấy file rồi lọc ra 10 cái mới nhất
            string json = await _client.GetFileListAsync("/");
            if (!string.IsNullOrEmpty(json) && json != "[]")
            {
                _cachedFiles = JsonConvert.DeserializeObject<List<FileMetadata>>(json);
            }
            else
            {
                _cachedFiles = new List<FileMetadata>();
            }

            // Mặc định ban đầu chỉ hiện 10 file mới nhất
            var top10Files = _cachedFiles.Take(10).ToList();
            homeFileList.SetFiles(top10Files);
        }

    }
}
