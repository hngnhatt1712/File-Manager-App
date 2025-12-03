namespace ClientApp
{
    partial class MainMenu
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainMenu));
            panel1 = new Panel();
            label2 = new Label();
            button1 = new Button();
            button2 = new Button();
            label5 = new Label();
            label1 = new Label();
            button4 = new Button();
            button5 = new Button();
            label4 = new Label();
            button3 = new Button();
            label3 = new Label();
            roundedPanel1 = new RoundedPanel();
            txtSearch = new TextBox();
            panel3 = new Panel();
            label6 = new Label();
            button7 = new Button();
            button6 = new Button();
            panel4 = new Panel();
            sidebar = new FlowLayoutPanel();
            changefile = new FlowLayoutPanel();
            btn_filetype = new Button();
            imageList1 = new ImageList(components);
            btn_pdf = new Button();
            btn_word = new Button();
            btn_excel = new Button();
            btn_Logout = new Button();
            sideBarTransition = new System.Windows.Forms.Timer(components);
            fileTransition = new System.Windows.Forms.Timer(components);
            panel2 = new Panel();
            panel1.SuspendLayout();
            roundedPanel1.SuspendLayout();
            panel3.SuspendLayout();
            panel4.SuspendLayout();
            sidebar.SuspendLayout();
            changefile.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.Aqua;
            panel1.Controls.Add(label2);
            panel1.Controls.Add(button1);
            panel1.Controls.Add(button2);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(button4);
            panel1.Controls.Add(button5);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(button3);
            panel1.Controls.Add(label3);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 1185);
            panel1.Margin = new Padding(0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1258, 109);
            panel1.TabIndex = 1;
            panel1.Paint += panel1_Paint;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(1091, 63);
            label2.Name = "label2";
            label2.Size = new Size(101, 37);
            label2.TabIndex = 1;
            label2.Text = "Setting";
            // 
            // button1
            // 
            button1.BackColor = Color.Transparent;
            button1.BackgroundImage = (Image)resources.GetObject("button1.BackgroundImage");
            button1.BackgroundImageLayout = ImageLayout.Zoom;
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Location = new Point(102, 2);
            button1.Name = "button1";
            button1.Size = new Size(71, 74);
            button1.TabIndex = 2;
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.BackColor = Color.Transparent;
            button2.BackgroundImage = (Image)resources.GetObject("button2.BackgroundImage");
            button2.BackgroundImageLayout = ImageLayout.Zoom;
            button2.FlatAppearance.BorderSize = 0;
            button2.FlatStyle = FlatStyle.Flat;
            button2.Location = new Point(1091, 6);
            button2.Name = "button2";
            button2.Size = new Size(101, 57);
            button2.TabIndex = 1;
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(892, 62);
            label5.Name = "label5";
            label5.Size = new Size(78, 37);
            label5.TabIndex = 1;
            label5.Text = "Trash";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(93, 62);
            label1.Name = "label1";
            label1.Size = new Size(89, 37);
            label1.TabIndex = 2;
            label1.Text = "Home";
            label1.Click += label1_Click;
            // 
            // button4
            // 
            button4.BackgroundImage = (Image)resources.GetObject("button4.BackgroundImage");
            button4.BackgroundImageLayout = ImageLayout.Zoom;
            button4.FlatAppearance.BorderSize = 0;
            button4.FlatStyle = FlatStyle.Flat;
            button4.Location = new Point(352, 6);
            button4.Name = "button4";
            button4.Size = new Size(116, 66);
            button4.TabIndex = 1;
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button5
            // 
            button5.BackColor = Color.Transparent;
            button5.BackgroundImage = (Image)resources.GetObject("button5.BackgroundImage");
            button5.BackgroundImageLayout = ImageLayout.Zoom;
            button5.FlatAppearance.BorderSize = 0;
            button5.FlatStyle = FlatStyle.Flat;
            button5.Location = new Point(855, 4);
            button5.Name = "button5";
            button5.Size = new Size(144, 66);
            button5.TabIndex = 1;
            button5.UseVisualStyleBackColor = false;
            button5.Click += button5_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(363, 62);
            label4.Name = "label4";
            label4.Size = new Size(98, 37);
            label4.TabIndex = 1;
            label4.Text = "Private";
            // 
            // button3
            // 
            button3.BackColor = Color.Transparent;
            button3.BackgroundImage = (Image)resources.GetObject("button3.BackgroundImage");
            button3.BackgroundImageLayout = ImageLayout.Zoom;
            button3.FlatAppearance.BorderSize = 0;
            button3.FlatStyle = FlatStyle.Flat;
            button3.Location = new Point(641, -1);
            button3.Name = "button3";
            button3.Size = new Size(113, 71);
            button3.TabIndex = 1;
            button3.UseVisualStyleBackColor = false;
            button3.Click += button3_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(593, 62);
            label3.Name = "label3";
            label3.Size = new Size(213, 37);
            label3.TabIndex = 1;
            label3.Text = "Downloaded file";
            // 
            // roundedPanel1
            // 
            roundedPanel1.Anchor = AnchorStyles.None;
            roundedPanel1.BackColor = Color.White;
            roundedPanel1.BorderColor = Color.Transparent;
            roundedPanel1.BorderRadius = 30;
            roundedPanel1.Controls.Add(txtSearch);
            roundedPanel1.Location = new Point(9, 6);
            roundedPanel1.Name = "roundedPanel1";
            roundedPanel1.Size = new Size(749, 71);
            roundedPanel1.TabIndex = 1;
            // 
            // txtSearch
            // 
            txtSearch.BackColor = Color.White;
            txtSearch.BorderStyle = BorderStyle.None;
            txtSearch.Location = new Point(19, 13);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(711, 36);
            txtSearch.TabIndex = 1;
            txtSearch.TextChanged += textBox1_TextChanged;
            // 
            // panel3
            // 
            panel3.BackColor = Color.Blue;
            panel3.Controls.Add(label6);
            panel3.Controls.Add(button7);
            panel3.Controls.Add(button6);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(0, 0);
            panel3.Margin = new Padding(0);
            panel3.Name = "panel3";
            panel3.Size = new Size(1258, 62);
            panel3.TabIndex = 3;
            panel3.Paint += panel3_Paint;
            panel3.MouseDown += panel3_MouseDown;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = Color.Transparent;
            label6.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.ForeColor = Color.Aqua;
            label6.Location = new Point(126, 9);
            label6.Name = "label6";
            label6.Size = new Size(316, 48);
            label6.TabIndex = 0;
            label6.Text = "File App Manager";
            // 
            // button7
            // 
            button7.BackColor = Color.Transparent;
            button7.BackgroundImage = (Image)resources.GetObject("button7.BackgroundImage");
            button7.BackgroundImageLayout = ImageLayout.Zoom;
            button7.FlatAppearance.BorderSize = 0;
            button7.FlatStyle = FlatStyle.Flat;
            button7.Location = new Point(25, -6);
            button7.Name = "button7";
            button7.Size = new Size(95, 78);
            button7.TabIndex = 1;
            button7.UseVisualStyleBackColor = false;
            button7.Click += button7_Click;
            // 
            // button6
            // 
            button6.BackColor = Color.Transparent;
            button6.BackgroundImageLayout = ImageLayout.Zoom;
            button6.Dock = DockStyle.Right;
            button6.FlatAppearance.BorderSize = 0;
            button6.FlatStyle = FlatStyle.Flat;
            button6.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button6.Image = (Image)resources.GetObject("button6.Image");
            button6.Location = new Point(1178, 0);
            button6.Name = "button6";
            button6.Size = new Size(80, 62);
            button6.TabIndex = 0;
            button6.UseVisualStyleBackColor = false;
            button6.Click += button6_Click;
            // 
            // panel4
            // 
            panel4.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel4.BackColor = SystemColors.Control;
            panel4.Controls.Add(roundedPanel1);
            panel4.Location = new Point(491, 62);
            panel4.Name = "panel4";
            panel4.Size = new Size(761, 100);
            panel4.TabIndex = 4;
            // 
            // sidebar
            // 
            sidebar.BackColor = Color.Blue;
            sidebar.Controls.Add(changefile);
            sidebar.Controls.Add(btn_Logout);
            sidebar.Dock = DockStyle.Left;
            sidebar.Location = new Point(0, 62);
            sidebar.Margin = new Padding(0);
            sidebar.Name = "sidebar";
            sidebar.Size = new Size(488, 1123);
            sidebar.TabIndex = 0;
            // 
            // changefile
            // 
            changefile.Controls.Add(btn_filetype);
            changefile.Controls.Add(btn_pdf);
            changefile.Controls.Add(btn_word);
            changefile.Controls.Add(btn_excel);
            changefile.ForeColor = Color.Blue;
            changefile.Location = new Point(3, 3);
            changefile.Name = "changefile";
            changefile.Size = new Size(487, 128);
            changefile.TabIndex = 3;
            changefile.UseWaitCursor = true;
            // 
            // btn_filetype
            // 
            btn_filetype.BackColor = Color.FromArgb(0, 0, 192);
            btn_filetype.FlatAppearance.BorderSize = 0;
            btn_filetype.FlatStyle = FlatStyle.Flat;
            btn_filetype.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btn_filetype.ForeColor = Color.Cyan;
            btn_filetype.ImageAlign = ContentAlignment.MiddleLeft;
            btn_filetype.ImageKey = "Selling Strategy Document.png";
            btn_filetype.ImageList = imageList1;
            btn_filetype.Location = new Point(0, 0);
            btn_filetype.Margin = new Padding(0);
            btn_filetype.Name = "btn_filetype";
            btn_filetype.Size = new Size(486, 128);
            btn_filetype.TabIndex = 0;
            btn_filetype.Text = "File type";
            btn_filetype.UseVisualStyleBackColor = false;
            btn_filetype.UseWaitCursor = true;
            btn_filetype.Click += btn_changefile_Click;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = Color.Transparent;
            imageList1.Images.SetKeyName(0, "logout.png");
            imageList1.Images.SetKeyName(1, "chuyển  đổi.png");
            imageList1.Images.SetKeyName(2, "word file.png");
            imageList1.Images.SetKeyName(3, "PDF.png");
            imageList1.Images.SetKeyName(4, "Selling Strategy Document.png");
            imageList1.Images.SetKeyName(5, "Microsoft Excel 2019.png");
            // 
            // btn_pdf
            // 
            btn_pdf.BackColor = Color.Blue;
            btn_pdf.FlatAppearance.BorderSize = 0;
            btn_pdf.FlatStyle = FlatStyle.Flat;
            btn_pdf.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btn_pdf.ForeColor = Color.Cyan;
            btn_pdf.ImageAlign = ContentAlignment.MiddleLeft;
            btn_pdf.ImageKey = "PDF.png";
            btn_pdf.ImageList = imageList1;
            btn_pdf.Location = new Point(0, 128);
            btn_pdf.Margin = new Padding(0);
            btn_pdf.Name = "btn_pdf";
            btn_pdf.Size = new Size(485, 128);
            btn_pdf.TabIndex = 2;
            btn_pdf.Text = "PDF";
            btn_pdf.UseVisualStyleBackColor = false;
            btn_pdf.UseWaitCursor = true;
            // 
            // btn_word
            // 
            btn_word.BackColor = Color.Blue;
            btn_word.FlatAppearance.BorderSize = 0;
            btn_word.FlatStyle = FlatStyle.Flat;
            btn_word.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btn_word.ForeColor = Color.Cyan;
            btn_word.ImageAlign = ContentAlignment.MiddleLeft;
            btn_word.ImageKey = "word file.png";
            btn_word.ImageList = imageList1;
            btn_word.Location = new Point(0, 256);
            btn_word.Margin = new Padding(0);
            btn_word.Name = "btn_word";
            btn_word.Size = new Size(485, 128);
            btn_word.TabIndex = 3;
            btn_word.Text = "Word";
            btn_word.UseVisualStyleBackColor = false;
            btn_word.UseWaitCursor = true;
            // 
            // btn_excel
            // 
            btn_excel.BackColor = Color.Blue;
            btn_excel.FlatAppearance.BorderSize = 0;
            btn_excel.FlatStyle = FlatStyle.Flat;
            btn_excel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btn_excel.ForeColor = Color.Cyan;
            btn_excel.ImageAlign = ContentAlignment.MiddleLeft;
            btn_excel.ImageKey = "Microsoft Excel 2019.png";
            btn_excel.ImageList = imageList1;
            btn_excel.Location = new Point(0, 384);
            btn_excel.Margin = new Padding(0);
            btn_excel.Name = "btn_excel";
            btn_excel.Size = new Size(485, 128);
            btn_excel.TabIndex = 4;
            btn_excel.Text = "Excel";
            btn_excel.UseVisualStyleBackColor = false;
            btn_excel.UseWaitCursor = true;
            // 
            // btn_Logout
            // 
            btn_Logout.BackColor = Color.FromArgb(0, 0, 192);
            btn_Logout.BackgroundImageLayout = ImageLayout.Zoom;
            btn_Logout.FlatAppearance.BorderSize = 0;
            btn_Logout.FlatStyle = FlatStyle.Flat;
            btn_Logout.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btn_Logout.ForeColor = Color.Cyan;
            btn_Logout.ImageAlign = ContentAlignment.MiddleLeft;
            btn_Logout.ImageKey = "logout.png";
            btn_Logout.ImageList = imageList1;
            btn_Logout.Location = new Point(0, 134);
            btn_Logout.Margin = new Padding(0);
            btn_Logout.Name = "btn_Logout";
            btn_Logout.Size = new Size(485, 138);
            btn_Logout.TabIndex = 1;
            btn_Logout.Text = "Log out";
            btn_Logout.UseVisualStyleBackColor = false;
            btn_Logout.Click += btn_Logout_Click_1;
            // 
            // sideBarTransition
            // 
            sideBarTransition.Interval = 10;
            sideBarTransition.Tick += sideBarTransition_Tick;
            // 
            // fileTransition
            // 
            fileTransition.Interval = 10;
            fileTransition.Tick += fileTransition_Tick;
            // 
            // panel2
            // 
            panel2.Location = new Point(491, 168);
            panel2.Name = "panel2";
            panel2.Size = new Size(761, 1010);
            panel2.TabIndex = 5;
            // 
            // MainMenu
            // 
            AutoScaleDimensions = new SizeF(15F, 37F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1258, 1294);
            Controls.Add(panel2);
            Controls.Add(panel4);
            Controls.Add(sidebar);
            Controls.Add(panel3);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(6, 7, 6, 7);
            Name = "MainMenu";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "MainMenu";
            Load += MainMenu_Load;
            MouseDown += MainMenu_MouseDown;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            roundedPanel1.ResumeLayout(false);
            roundedPanel1.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel4.ResumeLayout(false);
            sidebar.ResumeLayout(false);
            changefile.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private Panel panel1;
        private Button button1;
        private Label label1;
        private Button button2;
        private Label label2;
        private Button button3;
        private Label label3;
        private Label label4;
        private Button button4;
        private Button button5;
        private Label label5;
        private Panel panel3;
        private Button button6;
        private RoundedPanel roundedPanel1;
        private TextBox txtSearch;
        private Panel panel4;
        private Button button7;
        private Label label6;
        private FlowLayoutPanel sidebar;
        private System.Windows.Forms.Timer sideBarTransition;
        private Button btn_filetype;
        private Button btn_Logout;
        private ImageList imageList1;
        private Button btn_pdf;
        private FlowLayoutPanel changefile;
        private Button btn_word;
        private System.Windows.Forms.Timer fileTransition;
        private Button btn_excel;
        private Panel panel2;
    }
}