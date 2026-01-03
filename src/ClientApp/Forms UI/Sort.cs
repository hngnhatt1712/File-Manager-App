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
    public partial class Sort : UserControl
    {
        public delegate void SortChangedHandler(string option);
        public event SortChangedHandler OnSortChanged;
        public Sort()
        {
            InitializeComponent();
            foreach (ToolStripItem item in menuSort.Items)
            {
                item.Click += MenuSortItem_Click;
            }
        }

        // Khi chọn một dòng trong menu (ví dụ: Tên A-Z)
        private void MenuSortItem_Click(object sender, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem)sender;
            string luaChon = item.Text;

            // Bắn sự kiện ra bên ngoài cho MainMenu/FileList xử lý
            OnSortChanged?.Invoke(luaChon);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            menuSort.Show(btnSortMenu, new Point(0, btnSortMenu.Height));
        }
    }
}
