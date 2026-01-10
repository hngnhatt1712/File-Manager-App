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
            if (_client != null && fileList1 != null)
            {
                // 1. Cài đặt client và chế độ Thùng rác
                fileList1.SetClient(_client);
                fileList1.SetTrashMode(true);

                // 2. Lấy TOÀN BỘ danh sách file giống như tab Star đã làm
                string json = await _client.GetFileListAsync("/");
                var allFiles = JsonConvert.DeserializeObject<List<FileMetadata>>(json);

                if (allFiles != null)
                {
                    // 3. Lọc lấy những file có IsDeleted == true (đã bị xóa)
                    var trashFiles = allFiles.Where(f => f.IsDeleted == true).ToList();

                    // 4. Hiển thị lên giao diện
                    fileList1.RenderFileList(trashFiles);
                }
            }
        }

        /*fileList1.SetClient(_client);
            fileList1.SetTrashMode(true);
            await fileList1.LoadTrashFromServer();
        */
    }
}
