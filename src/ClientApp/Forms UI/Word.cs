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
    public partial class Word : UserControl, ISearchable
    {
        private FileTransferClient _client;
        private List<FileMetadata> _cachedFiles = new List<FileMetadata>();
        public Word(FileTransferClient client)
        {
            InitializeComponent();
            _client = client;
        }

        private async void Word_Load(object sender, EventArgs e)
        {
            fileList1.SetClient(_client);
            string json = await _client.GetFileListAsync("/");
            var allFiles = JsonConvert.DeserializeObject<List<FileMetadata>>(json);
            _cachedFiles = allFiles;

            var top10Files = allFiles.Take(10).ToList();

            // Gọi hàm Render trực tiếp
            fileList1.RenderFileList(top10Files);
        }

        public void SearchFiles(string keyword)
        {
            fileList1.SearchFiles(keyword);
        }
    }
}
