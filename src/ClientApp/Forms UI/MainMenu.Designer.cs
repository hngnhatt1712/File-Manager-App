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
            button7 = new Button();
            label6 = new Label();
            button6 = new Button();
            panel4 = new Panel();
            sidebar = new FlowLayoutPanel();
            changefile = new FlowLayoutPanel();
            btn_filetype = new Button();
            imageList1 = new ImageList(components);
            btn_word = new Button();
            btn_excel = new Button();
            btn_pdf = new Button();
            btn_txt = new Button();
            btn_ThongBao = new Button();
            btn_star = new Button();
            btn_Logout = new Button();
            sideBarTransition = new System.Windows.Forms.Timer(components);
            fileTransition = new System.Windows.Forms.Timer(components);
            flowLayoutPanel1 = new FlowLayoutPanel();
            setting1 = new ClientApp.Forms_UI.Setting();
            panel1.SuspendLayout();
            roundedPanel1.SuspendLayout();
            panel3.SuspendLayout();
            sidebar.SuspendLayout();
            changefile.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
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
            panel1.Location = new Point(0, 497);
            panel1.Margin = new Padding(0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1017, 44);
            panel1.TabIndex = 1;
            panel1.Paint += panel1_Paint;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 11F);
            label2.Location = new Point(909, 28);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(56, 20);
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
            button1.Font = new Font("Segoe UI", 11F);
            button1.Location = new Point(38, 0);
            button1.Margin = new Padding(2);
            button1.Name = "button1";
            button1.Size = new Size(33, 30);
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
            button2.Font = new Font("Segoe UI", 11F);
            button2.Location = new Point(916, 3);
            button2.Margin = new Padding(2);
            button2.Name = "button2";
            button2.Size = new Size(47, 23);
            button2.TabIndex = 1;
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 11F);
            label5.Location = new Point(712, 27);
            label5.Margin = new Padding(2, 0, 2, 0);
            label5.Name = "label5";
            label5.Size = new Size(43, 20);
            label5.TabIndex = 1;
            label5.Text = "Trash";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 11F);
            label1.Location = new Point(33, 25);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(50, 20);
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
            button4.Font = new Font("Segoe UI", 11F);
            button4.Location = new Point(243, 2);
            button4.Margin = new Padding(2);
            button4.Name = "button4";
            button4.Size = new Size(54, 27);
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
            button5.Font = new Font("Segoe UI", 11F);
            button5.Location = new Point(702, 3);
            button5.Margin = new Padding(2);
            button5.Name = "button5";
            button5.Size = new Size(67, 27);
            button5.TabIndex = 1;
            button5.UseVisualStyleBackColor = false;
            button5.Click += button5_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 11F);
            label4.Location = new Point(243, 25);
            label4.Margin = new Padding(2, 0, 2, 0);
            label4.Name = "label4";
            label4.Size = new Size(54, 20);
            label4.TabIndex = 1;
            label4.Text = "My file";
            // 
            // button3
            // 
            button3.BackColor = Color.Transparent;
            button3.BackgroundImage = (Image)resources.GetObject("button3.BackgroundImage");
            button3.BackgroundImageLayout = ImageLayout.Zoom;
            button3.FlatAppearance.BorderSize = 0;
            button3.FlatStyle = FlatStyle.Flat;
            button3.Font = new Font("Segoe UI", 11F);
            button3.Location = new Point(466, 3);
            button3.Margin = new Padding(2);
            button3.Name = "button3";
            button3.Size = new Size(52, 27);
            button3.TabIndex = 1;
            button3.UseVisualStyleBackColor = false;
            button3.Click += button3_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 11F);
            label3.Location = new Point(466, 28);
            label3.Margin = new Padding(2, 0, 2, 0);
            label3.Name = "label3";
            label3.Size = new Size(58, 20);
            label3.TabIndex = 1;
            label3.Text = "Upload";
            // 
            // roundedPanel1
            // 
            roundedPanel1.Anchor = AnchorStyles.None;
            roundedPanel1.BackColor = Color.White;
            roundedPanel1.BorderColor = Color.Transparent;
            roundedPanel1.BorderRadius = 30;
            roundedPanel1.Controls.Add(txtSearch);
            roundedPanel1.Location = new Point(252, 65);
            roundedPanel1.Margin = new Padding(2);
            roundedPanel1.Name = "roundedPanel1";
            roundedPanel1.Size = new Size(733, 30);
            roundedPanel1.TabIndex = 1;
            // 
            // txtSearch
            // 
            txtSearch.BackColor = Color.White;
            txtSearch.BorderStyle = BorderStyle.None;
            txtSearch.Location = new Point(25, 12);
            txtSearch.Margin = new Padding(2);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(648, 16);
            txtSearch.TabIndex = 1;
            txtSearch.TextChanged += textBox1_TextChanged;
            txtSearch.KeyDown += txtSearch_KeyDown;
            // 
            // panel3
            // 
            panel3.BackColor = Color.Blue;
            panel3.Controls.Add(button7);
            panel3.Controls.Add(label6);
            panel3.Controls.Add(button6);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(0, 0);
            panel3.Margin = new Padding(0);
            panel3.Name = "panel3";
            panel3.Size = new Size(1017, 43);
            panel3.TabIndex = 3;
            panel3.Paint += panel3_Paint;
            panel3.MouseDown += panel3_MouseDown;
            // 
            // button7
            // 
            button7.BackColor = Color.Transparent;
            button7.BackgroundImage = (Image)resources.GetObject("button7.BackgroundImage");
            button7.BackgroundImageLayout = ImageLayout.Zoom;
            button7.FlatAppearance.BorderSize = 0;
            button7.FlatStyle = FlatStyle.Flat;
            button7.Location = new Point(2, 0);
            button7.Margin = new Padding(2);
            button7.Name = "button7";
            button7.Size = new Size(53, 32);
            button7.TabIndex = 1;
            button7.UseVisualStyleBackColor = false;
            button7.Click += button7_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = Color.Transparent;
            label6.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.ForeColor = Color.Aqua;
            label6.Location = new Point(59, 7);
            label6.Margin = new Padding(2, 0, 2, 0);
            label6.Name = "label6";
            label6.Size = new Size(144, 21);
            label6.TabIndex = 0;
            label6.Text = "File App Manager";
            // 
            // button6
            // 
            button6.BackColor = Color.Transparent;
            button6.BackgroundImage = (Image)resources.GetObject("button6.BackgroundImage");
            button6.BackgroundImageLayout = ImageLayout.Zoom;
            button6.Dock = DockStyle.Right;
            button6.FlatAppearance.BorderSize = 0;
            button6.FlatStyle = FlatStyle.Flat;
            button6.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button6.Location = new Point(972, 0);
            button6.Margin = new Padding(2);
            button6.Name = "button6";
            button6.Size = new Size(45, 43);
            button6.TabIndex = 0;
            button6.UseVisualStyleBackColor = false;
            button6.Click += button6_Click;
            // 
            // panel4
            // 
            panel4.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel4.BackColor = SystemColors.ControlLight;
            panel4.Location = new Point(230, 47);
            panel4.Margin = new Padding(2);
            panel4.Name = "panel4";
            panel4.Size = new Size(787, 63);
            panel4.TabIndex = 4;
            // 
            // sidebar
            // 
            sidebar.BackColor = Color.Blue;
            sidebar.Controls.Add(changefile);
            sidebar.Controls.Add(btn_ThongBao);
            sidebar.Controls.Add(btn_star);
            sidebar.Controls.Add(btn_Logout);
            sidebar.Dock = DockStyle.Left;
            sidebar.Location = new Point(0, 43);
            sidebar.Margin = new Padding(0);
            sidebar.Name = "sidebar";
            sidebar.Size = new Size(203, 454);
            sidebar.TabIndex = 0;
            // 
            // changefile
            // 
            changefile.Controls.Add(btn_filetype);
            changefile.Controls.Add(btn_word);
            changefile.Controls.Add(btn_excel);
            changefile.Controls.Add(btn_pdf);
            changefile.Controls.Add(btn_txt);
            changefile.ForeColor = Color.Blue;
            changefile.Location = new Point(0, 0);
            changefile.Margin = new Padding(0);
            changefile.Name = "changefile";
            changefile.Size = new Size(225, 52);
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
            btn_filetype.Size = new Size(225, 52);
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
            imageList1.Images.SetKeyName(6, "TXT.png");
            imageList1.Images.SetKeyName(7, "Upload Document.png");
            imageList1.Images.SetKeyName(8, "Star.png");
            imageList1.Images.SetKeyName(9, "Notification.png");
            imageList1.Images.SetKeyName(10, "Sorting Arrows.png");
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
            btn_word.Location = new Point(0, 52);
            btn_word.Margin = new Padding(0);
            btn_word.Name = "btn_word";
            btn_word.Size = new Size(225, 52);
            btn_word.TabIndex = 3;
            btn_word.Text = "Word";
            btn_word.UseVisualStyleBackColor = false;
            btn_word.UseWaitCursor = true;
            btn_word.Click += btn_word_Click;
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
            btn_excel.Location = new Point(0, 104);
            btn_excel.Margin = new Padding(0);
            btn_excel.Name = "btn_excel";
            btn_excel.Size = new Size(225, 52);
            btn_excel.TabIndex = 4;
            btn_excel.Text = "Excel";
            btn_excel.UseVisualStyleBackColor = false;
            btn_excel.UseWaitCursor = true;
            btn_excel.Click += btn_excel_Click;
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
            btn_pdf.Location = new Point(0, 156);
            btn_pdf.Margin = new Padding(0);
            btn_pdf.Name = "btn_pdf";
            btn_pdf.Size = new Size(225, 52);
            btn_pdf.TabIndex = 2;
            btn_pdf.Text = "PDF";
            btn_pdf.UseVisualStyleBackColor = false;
            btn_pdf.UseWaitCursor = true;
            btn_pdf.Click += btn_pdf_Click;
            // 
            // btn_txt
            // 
            btn_txt.BackColor = Color.Blue;
            btn_txt.FlatAppearance.BorderSize = 0;
            btn_txt.FlatStyle = FlatStyle.Flat;
            btn_txt.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btn_txt.ForeColor = Color.Cyan;
            btn_txt.ImageAlign = ContentAlignment.MiddleLeft;
            btn_txt.ImageKey = "TXT.png";
            btn_txt.ImageList = imageList1;
            btn_txt.Location = new Point(0, 208);
            btn_txt.Margin = new Padding(0);
            btn_txt.Name = "btn_txt";
            btn_txt.Size = new Size(225, 52);
            btn_txt.TabIndex = 5;
            btn_txt.Text = "TXT";
            btn_txt.UseVisualStyleBackColor = false;
            btn_txt.UseWaitCursor = true;
            btn_txt.Click += btn_txt_Click;
            // 
            // btn_ThongBao
            // 
            btn_ThongBao.BackColor = Color.FromArgb(0, 0, 192);
            btn_ThongBao.FlatAppearance.BorderSize = 0;
            btn_ThongBao.FlatStyle = FlatStyle.Flat;
            btn_ThongBao.Font = new Font("Segoe UI", 11F);
            btn_ThongBao.ForeColor = Color.Cyan;
            btn_ThongBao.ImageAlign = ContentAlignment.MiddleLeft;
            btn_ThongBao.ImageKey = "Sorting Arrows.png";
            btn_ThongBao.ImageList = imageList1;
            btn_ThongBao.Location = new Point(0, 52);
            btn_ThongBao.Margin = new Padding(0);
            btn_ThongBao.Name = "btn_ThongBao";
            btn_ThongBao.Size = new Size(225, 52);
            btn_ThongBao.TabIndex = 0;
            btn_ThongBao.Text = "Sort";
            btn_ThongBao.UseVisualStyleBackColor = false;
            btn_ThongBao.Click += btn_ThongBao_Click;
            // 
            // btn_star
            // 
            btn_star.BackColor = Color.FromArgb(0, 0, 192);
            btn_star.BackgroundImageLayout = ImageLayout.Zoom;
            btn_star.FlatAppearance.BorderSize = 0;
            btn_star.FlatStyle = FlatStyle.Flat;
            btn_star.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btn_star.ForeColor = Color.Cyan;
            btn_star.ImageAlign = ContentAlignment.MiddleLeft;
            btn_star.ImageKey = "Star.png";
            btn_star.ImageList = imageList1;
            btn_star.Location = new Point(0, 104);
            btn_star.Margin = new Padding(0);
            btn_star.Name = "btn_star";
            btn_star.Size = new Size(225, 52);
            btn_star.TabIndex = 4;
            btn_star.Text = "Starred file";
            btn_star.UseVisualStyleBackColor = false;
            btn_star.Click += button8_Click;
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
            btn_Logout.Location = new Point(0, 156);
            btn_Logout.Margin = new Padding(0);
            btn_Logout.Name = "btn_Logout";
            btn_Logout.Size = new Size(225, 52);
            btn_Logout.TabIndex = 1;
            btn_Logout.Text = "Log out";
            btn_Logout.UseVisualStyleBackColor = false;
            btn_Logout.Click += btn_Logout_Click_1;
            // 
            // sideBarTransition
            // 
            sideBarTransition.Interval = 9;
            sideBarTransition.Tick += sideBarTransition_Tick;
            // 
            // fileTransition
            // 
            fileTransition.Interval = 9;
            fileTransition.Tick += fileTransition_Tick;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(setting1);
            flowLayoutPanel1.Location = new Point(230, 114);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(787, 381);
            flowLayoutPanel1.TabIndex = 5;
            // 
            // setting1
            // 
            setting1.BackColor = Color.PowderBlue;
            setting1.Location = new Point(2, 2);
            setting1.Margin = new Padding(2);
            setting1.Name = "setting1";
            setting1.Size = new Size(780, 326);
            setting1.TabIndex = 0;
            // 
            // MainMenu
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1017, 541);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(roundedPanel1);
            Controls.Add(panel4);
            Controls.Add(sidebar);
            Controls.Add(panel3);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
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
            sidebar.ResumeLayout(false);
            changefile.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
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
        private Button btn_txt;
        private Button btn_star;
        private Button btn_ThongBao;
        private FlowLayoutPanel flowLayoutPanel1;
        private Forms_UI.Setting setting1;
    }
}