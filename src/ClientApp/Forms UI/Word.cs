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
    public partial class Word : UserControl, ISearchable, ISortable
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

            // Khai báo: Tab này chỉ chấp nhận file Word
            fileList1.AllowedExtensions = new[] { ".docx"};

            // Gọi hàm load như bình thường, FileList sẽ tự lo phần lọc
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
