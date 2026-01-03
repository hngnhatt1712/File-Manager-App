namespace ClientApp.Forms_UI
{
    partial class Sort
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Sort));
            panel1 = new Panel();
            btnSortMenu = new Button();
            imageList1 = new ImageList(components);
            menuSort = new ContextMenuStrip(components);
            tênAZToolStripMenuItem = new ToolStripMenuItem();
            tênZAToolStripMenuItem = new ToolStripMenuItem();
            ngàyMớiNhấtToolStripMenuItem = new ToolStripMenuItem();
            ngàyCũNhấtToolStripMenuItem = new ToolStripMenuItem();
            panel1.SuspendLayout();
            menuSort.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(btnSortMenu);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(787, 66);
            panel1.TabIndex = 0;
            // 
            // btnSortMenu
            // 
            btnSortMenu.BackgroundImageLayout = ImageLayout.Zoom;
            btnSortMenu.FlatAppearance.BorderSize = 0;
            btnSortMenu.FlatStyle = FlatStyle.Flat;
            btnSortMenu.Image = (Image)resources.GetObject("btnSortMenu.Image");
            btnSortMenu.ImageAlign = ContentAlignment.MiddleLeft;
            btnSortMenu.Location = new Point(3, 0);
            btnSortMenu.Name = "btnSortMenu";
            btnSortMenu.Size = new Size(63, 64);
            btnSortMenu.TabIndex = 1;
            btnSortMenu.UseVisualStyleBackColor = true;
            btnSortMenu.Click += button1_Click;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = Color.Transparent;
            imageList1.Images.SetKeyName(0, "Descending Sorting.png");
            // 
            // menuSort
            // 
            menuSort.Items.AddRange(new ToolStripItem[] { tênAZToolStripMenuItem, tênZAToolStripMenuItem, ngàyMớiNhấtToolStripMenuItem, ngàyCũNhấtToolStripMenuItem });
            menuSort.Name = "contextMenuStrip1";
            menuSort.Size = new Size(154, 92);
            // 
            // tênAZToolStripMenuItem
            // 
            tênAZToolStripMenuItem.Image = (Image)resources.GetObject("tênAZToolStripMenuItem.Image");
            tênAZToolStripMenuItem.Name = "tênAZToolStripMenuItem";
            tênAZToolStripMenuItem.Size = new Size(153, 22);
            tênAZToolStripMenuItem.Text = "Tên (A -> Z)";
            // 
            // tênZAToolStripMenuItem
            // 
            tênZAToolStripMenuItem.Image = (Image)resources.GetObject("tênZAToolStripMenuItem.Image");
            tênZAToolStripMenuItem.Name = "tênZAToolStripMenuItem";
            tênZAToolStripMenuItem.Size = new Size(153, 22);
            tênZAToolStripMenuItem.Text = "Tên (Z -> A)";
            // 
            // ngàyMớiNhấtToolStripMenuItem
            // 
            ngàyMớiNhấtToolStripMenuItem.Image = (Image)resources.GetObject("ngàyMớiNhấtToolStripMenuItem.Image");
            ngàyMớiNhấtToolStripMenuItem.Name = "ngàyMớiNhấtToolStripMenuItem";
            ngàyMớiNhấtToolStripMenuItem.Size = new Size(153, 22);
            ngàyMớiNhấtToolStripMenuItem.Text = "Ngày mới nhất";
            // 
            // ngàyCũNhấtToolStripMenuItem
            // 
            ngàyCũNhấtToolStripMenuItem.Image = (Image)resources.GetObject("ngàyCũNhấtToolStripMenuItem.Image");
            ngàyCũNhấtToolStripMenuItem.Name = "ngàyCũNhấtToolStripMenuItem";
            ngàyCũNhấtToolStripMenuItem.Size = new Size(153, 22);
            ngàyCũNhấtToolStripMenuItem.Text = "Ngày cũ nhất";
            // 
            // Sort
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panel1);
            Margin = new Padding(3, 2, 3, 2);
            Name = "Sort";
            Size = new Size(787, 66);
            Tag = "";
            panel1.ResumeLayout(false);
            menuSort.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Button btnSortMenu;
        private ImageList imageList1;
        private ContextMenuStrip menuSort;
        private ToolStripMenuItem tênAZToolStripMenuItem;
        private ToolStripMenuItem tênZAToolStripMenuItem;
        private ToolStripMenuItem ngàyMớiNhấtToolStripMenuItem;
        private ToolStripMenuItem ngàyCũNhấtToolStripMenuItem;
    }
}
