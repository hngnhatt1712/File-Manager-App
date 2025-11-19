using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClientApp.Services;

namespace ClientApp
{
    public partial class MainMenu : Form
    {
        private readonly FileTransferClient _fileClient;
        public MainMenu(FileTransferClient fileClient)
        {
            InitializeComponent();
            _fileClient = fileClient;
        }

        private void MainMenu_Load(object sender, EventArgs e)
        {

        }
    }
}
