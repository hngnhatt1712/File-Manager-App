using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientApp.Forms_UI
{
    public partial class Check_Password : Form
    {
        public string PasswordGiaoDien => txtPasswordConfirm.Text;
        public string PasswordResult { get; private set; }
        public Check_Password()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            PasswordResult = txtPasswordConfirm.Text;
            this.DialogResult = DialogResult.OK; // Đánh dấu là đã bấm xác nhận
            this.Close();
        }
    }
}
