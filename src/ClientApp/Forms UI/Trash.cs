using Newtonsoft.Json;
using ServerApp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Formats.Tar;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharedLibrary;

namespace ClientApp.Forms_UI
{
    public partial class trash : UserControl
    {
        private FileTransferClient _client;
        public trash(FileTransferClient client)
        {
            InitializeComponent();
            _client = client;
        }

        private async void trash_Load(object sender, EventArgs e)
        {
            fileList1.SetClient(_client);
            fileList1.SetTrashMode(true);
            string json = await _client.GetFileListAsync("/");
            if (!string.IsNullOrEmpty(json) && json != "[]")
            {
                var allFiles = JsonConvert.DeserializeObject<List<FileMetadata>>(json);

                // LỌC: Chỉ lấy file có IsDeleted == true
                var deletedFiles = allFiles.Where(f => f.IsDeleted == true).ToList();

                // Hiển thị toàn bộ file trong thùng rác
                fileList1.RenderFileList(deletedFiles);
            }
        }
    }
}
