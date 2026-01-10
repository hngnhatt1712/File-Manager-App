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
            await fileList1.LoadTrashFromServer();
        }

    }
}
