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
    public partial class Star : UserControl
    {
        private FileTransferClient _client;
        public Star(FileTransferClient client)
        {
            InitializeComponent();
            _client = client;
        }

        private async void Star_Load(object sender, EventArgs e)
        {
            startFile.SetClient(_client);

            startFile.IsStarredMode = true; // Báo cho FileList biết đây là tab Dấu sao
                                          

            string json = await _client.GetFileListAsync("/");
            var allFiles = JsonConvert.DeserializeObject<List<FileMetadata>>(json);

            // Lọc lấy file có dấu sao
            var starredFiles = allFiles.Where(f => f.IsStarred == true).ToList();

            startFile.RenderFileList(starredFiles);
        }

        private void startFile_Load(object sender, EventArgs e)
        {

        }
    }
}
