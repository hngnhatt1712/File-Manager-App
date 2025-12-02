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
            panel2 = new Panel();
            btn_Logout = new Button();
            sideBarTransition = new System.Windows.Forms.Timer(components);
            flowLayoutPanel1 = new FlowLayoutPanel();
            panel5 = new Panel();
            button8 = new Button();
            panel1.SuspendLayout();
            roundedPanel1.SuspendLayout();
            panel3.SuspendLayout();
            panel4.SuspendLayout();
            sidebar.SuspendLayout();
            panel2.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(128, 128, 255);
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
            label2.Text = "Cài đặt";
            // 
            // button1
            // 
            button1.BackColor = Color.Transparent;
            button1.BackgroundImage = (Image)resources.GetObject("button1.BackgroundImage");
            button1.BackgroundImageLayout = ImageLayout.Zoom;
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Location = new Point(65, 0);
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
            label5.Location = new Point(865, 63);
            label5.Name = "label5";
            label5.Size = new Size(134, 37);
            label5.TabIndex = 1;
            label5.Text = "Thùng rác";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(42, 63);
            label1.Name = "label1";
            label1.Size = new Size(139, 37);
            label1.TabIndex = 2;
            label1.Text = "Trang chủ ";
            label1.Click += label1_Click;
            // 
            // button4
            // 
            button4.BackgroundImage = (Image)resources.GetObject("button4.BackgroundImage");
            button4.BackgroundImageLayout = ImageLayout.Zoom;
            button4.FlatAppearance.BorderSize = 0;
            button4.FlatStyle = FlatStyle.Flat;
            button4.Location = new Point(353, 8);
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
            label4.Location = new Point(364, 64);
            label4.Name = "label4";
            label4.Size = new Size(117, 37);
            label4.TabIndex = 1;
            label4.Text = "Riêng tư";
            // 
            // button3
            // 
            button3.BackColor = Color.Transparent;
            button3.BackgroundImage = (Image)resources.GetObject("button3.BackgroundImage");
            button3.BackgroundImageLayout = ImageLayout.Zoom;
            button3.FlatAppearance.BorderSize = 0;
            button3.FlatStyle = FlatStyle.Flat;
            button3.Location = new Point(645, -1);
            button3.Name = "button3";
            button3.Size = new Size(113, 71);
            button3.TabIndex = 1;
            button3.UseVisualStyleBackColor = false;
            button3.Click += button3_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(654, 62);
            label3.Name = "label3";
            label3.Size = new Size(92, 37);
            label3.TabIndex = 1;
            label3.Text = "Đã Tải";
            // 
            // roundedPanel1
            // 
            roundedPanel1.BackColor = Color.IndianRed;
            roundedPanel1.BorderColor = Color.Transparent;
            roundedPanel1.BorderRadius = 30;
            roundedPanel1.Controls.Add(txtSearch);
            roundedPanel1.Location = new Point(6, 19);
            roundedPanel1.Name = "roundedPanel1";
            roundedPanel1.Size = new Size(778, 75);
            roundedPanel1.TabIndex = 1;
            // 
            // txtSearch
            // 
            txtSearch.BackColor = Color.IndianRed;
            txtSearch.BorderStyle = BorderStyle.None;
            txtSearch.Location = new Point(29, 3);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(727, 36);
            txtSearch.TabIndex = 1;
            txtSearch.TextChanged += textBox1_TextChanged;
            // 
            // panel3
            // 
            panel3.BackColor = Color.FromArgb(0, 0, 192);
            panel3.Controls.Add(label6);
            panel3.Controls.Add(button7);
            panel3.Controls.Add(button6);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(0, 0);
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
            panel4.BackColor = SystemColors.Control;
            panel4.Controls.Add(roundedPanel1);
            panel4.Dock = DockStyle.Top;
            panel4.Location = new Point(488, 62);
            panel4.Name = "panel4";
            panel4.Size = new Size(770, 100);
            panel4.TabIndex = 4;
            // 
            // sidebar
            // 
            sidebar.BackColor = Color.Black;
            sidebar.Controls.Add(panel2);
            sidebar.Dock = DockStyle.Left;
            sidebar.Location = new Point(0, 62);
            sidebar.Name = "sidebar";
            sidebar.Size = new Size(488, 1123);
            sidebar.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.Controls.Add(btn_Logout);
            panel2.Location = new Point(3, 3);
            panel2.Name = "panel2";
            panel2.Size = new Size(488, 146);
            panel2.TabIndex = 2;
            // 
            // btn_Logout
            // 
            btn_Logout.BackColor = Color.Transparent;
            btn_Logout.BackgroundImage = (Image)resources.GetObject("btn_Logout.BackgroundImage");
            btn_Logout.BackgroundImageLayout = ImageLayout.Zoom;
            btn_Logout.FlatAppearance.BorderSize = 0;
            btn_Logout.FlatStyle = FlatStyle.Flat;
            btn_Logout.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btn_Logout.ForeColor = SystemColors.ButtonHighlight;
            btn_Logout.Location = new Point(-274, -5);
            btn_Logout.Name = "btn_Logout";
            btn_Logout.Size = new Size(752, 151);
            btn_Logout.TabIndex = 1;
            btn_Logout.Text = "           Đăng Xuất";
            btn_Logout.TextAlign = ContentAlignment.MiddleRight;
            btn_Logout.UseVisualStyleBackColor = false;
            // 
            // sideBarTransition
            // 
            sideBarTransition.Interval = 10;
            sideBarTransition.Tick += sideBarTransition_Tick;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(panel5);
            flowLayoutPanel1.Controls.Add(button8);
            flowLayoutPanel1.Location = new Point(494, 162);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(761, 1016);
            flowLayoutPanel1.TabIndex = 5;
            // 
            // panel5
            // 
            panel5.Location = new Point(3, 3);
            panel5.Name = "panel5";
            panel5.Size = new Size(572, 150);
            panel5.TabIndex = 2;
            // 
            // button8
            // 
            button8.Location = new Point(3, 159);
            button8.Name = "button8";
            button8.Size = new Size(698, 64);
            button8.TabIndex = 0;
            button8.Text = "button8";
            button8.UseVisualStyleBackColor = true;
            // 
            // MainMenu
            // 
            AutoScaleDimensions = new SizeF(15F, 37F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1258, 1294);
            Controls.Add(flowLayoutPanel1);
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
            panel2.ResumeLayout(false);
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
        private FlowLayoutPanel flowLayoutPanel1;
        private Button button8;
        private Panel panel2;
        private Button btn_Logout;
        private Panel panel5;
    }
}