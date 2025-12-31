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
    public partial class Home : UserControl
    {
        private FileTransferClient _client;
        public Home(FileTransferClient client)
        {
            InitializeComponent();
            _client = client;

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private async void TrangChu_Load(object sender, EventArgs e)
        {
            homeFileList.SetClient(_client);
            // Logic riêng của Home: Lấy file rồi lọc ra 10 cái mới nhất
            string json = await _client.GetFileListAsync("/");
            var allFiles = JsonConvert.DeserializeObject<List<FileMetadata>>(json);

            var top10Files = allFiles.Take(10).ToList();

            // Gọi hàm Render trực tiếp
            homeFileList.RenderFileList(top10Files);
        }
        
    }
}
