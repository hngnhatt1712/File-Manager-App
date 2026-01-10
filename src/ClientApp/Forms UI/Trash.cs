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
                // 1. Gán client cho fileList1 để nó có thể kết nối server
                fileList1.SetClient(_client);

                // 2. Bật chế độ TrashMode = true 
                // (Để nó biết là đang ở thùng rác, hiện nút 'Khôi phục' thay vì 'Xóa vào thùng rác')
                fileList1.SetTrashMode(true);

                // 3. Gọi hàm tải dữ liệu từ Server (hàm này bạn đã viết trong FileList.cs)
                await fileList1.LoadTrashFromServer();
            }
        }

        /*fileList1.SetClient(_client);
            fileList1.SetTrashMode(true);
            await fileList1.LoadTrashFromServer();
        */
    }
}
