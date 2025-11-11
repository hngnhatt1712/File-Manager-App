using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientApp
{
    public partial class Dashboard : Form
    {
        public Dashboard(FileTransferClient fileClient)
        {
            InitializeComponent();
            _fileClient = _fileClient;
        }

        private readonly FileTransferClient _fileClient;


        private void Dashboard_Load(object sender, EventArgs e)
        {
            
        }
    }
}
