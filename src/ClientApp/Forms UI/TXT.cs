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
    public partial class TXT : UserControl, ISearchable, ISortable
    {
        private FileTransferClient _client;
        private List<FileMetadata> _cachedFiles = new List<FileMetadata>();
        public TXT(FileTransferClient client)
        {
            InitializeComponent();
            _client = client;
        }

        private async void TXT_Load(object sender, EventArgs e)
        {
            fileList1.SetClient(_client);
            fileList1.AllowedExtensions = new[] { ".txt" };
            await fileList1.LoadFilesFromServer("/");
        }

        public void SearchFiles(string keyword)
        {
            fileList1.SearchFiles(keyword);
        }

        public void ApplySort(string option)
        {
            fileList1.ApplySort(option);
        }
    }
}
