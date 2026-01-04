namespace ClientApp.Forms_UI
{
    partial class Setting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Setting));
            label1 = new Label();
            button1 = new Button();
            imageList1 = new ImageList(components);
            btnXoaTaiKhoan = new Button();
            button3 = new Button();
            btn_logout = new Button();
            panel1 = new Panel();
            panel2 = new Panel();
            label2 = new Label();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold);
            label1.ForeColor = Color.Red;
            label1.Location = new Point(412, 0);
            label1.Name = "label1";
            label1.Size = new Size(105, 37);
            label1.TabIndex = 0;
            label1.Text = "Cài Đặt";
            // 
            // button1
            // 
            button1.BackColor = Color.DeepSkyBlue;
            button1.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            button1.ForeColor = SystemColors.ControlLightLight;
            button1.ImageAlign = ContentAlignment.MiddleLeft;
            button1.ImageKey = "Change.png";
            button1.ImageList = imageList1;
            button1.Location = new Point(324, 51);
            button1.Name = "button1";
            button1.Size = new Size(279, 50);
            button1.TabIndex = 1;
            button1.Text = "       Thay đổi mật khẩu";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = Color.Transparent;
            imageList1.Images.SetKeyName(0, "Change.png");
            imageList1.Images.SetKeyName(1, "Imac Exit.png");
            imageList1.Images.SetKeyName(2, "Lecturer.png");
            imageList1.Images.SetKeyName(3, "Trash.png");
            // 
            // btnXoaTaiKhoan
            // 
            btnXoaTaiKhoan.BackColor = Color.DeepSkyBlue;
            btnXoaTaiKhoan.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            btnXoaTaiKhoan.ForeColor = SystemColors.ControlLightLight;
            btnXoaTaiKhoan.ImageAlign = ContentAlignment.MiddleLeft;
            btnXoaTaiKhoan.ImageKey = "Trash.png";
            btnXoaTaiKhoan.ImageList = imageList1;
            btnXoaTaiKhoan.Location = new Point(324, 107);
            btnXoaTaiKhoan.Name = "btnXoaTaiKhoan";
            btnXoaTaiKhoan.Size = new Size(279, 45);
            btnXoaTaiKhoan.TabIndex = 2;
            btnXoaTaiKhoan.Text = "Xóa tài khoản";
            btnXoaTaiKhoan.UseVisualStyleBackColor = false;
            btnXoaTaiKhoan.Click += btnXoaTaiKhoan_Click;
            // 
            // button3
            // 
            button3.BackColor = Color.DeepSkyBlue;
            button3.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            button3.ForeColor = SystemColors.ControlLightLight;
            button3.ImageAlign = ContentAlignment.MiddleLeft;
            button3.ImageKey = "Lecturer.png";
            button3.ImageList = imageList1;
            button3.Location = new Point(324, 158);
            button3.Name = "button3";
            button3.Size = new Size(279, 45);
            button3.TabIndex = 3;
            button3.Text = "Giới thiệu chung";
            button3.UseVisualStyleBackColor = false;
            button3.Click += button3_Click;
            // 
            // btn_logout
            // 
            btn_logout.BackColor = Color.DeepSkyBlue;
            btn_logout.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            btn_logout.ForeColor = SystemColors.ControlLightLight;
            btn_logout.ImageAlign = ContentAlignment.MiddleLeft;
            btn_logout.ImageKey = "Imac Exit.png";
            btn_logout.ImageList = imageList1;
            btn_logout.Location = new Point(324, 219);
            btn_logout.Name = "btn_logout";
            btn_logout.Size = new Size(279, 45);
            btn_logout.TabIndex = 4;
            btn_logout.Text = "Đăng xuất";
            btn_logout.UseVisualStyleBackColor = false;
            btn_logout.Click += button4_Click;
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ButtonFace;
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 296);
            panel1.Name = "panel1";
            panel1.Size = new Size(780, 30);
            panel1.TabIndex = 5;
            // 
            // panel2
            // 
            panel2.BackColor = Color.Lime;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(10, 30);
            panel2.TabIndex = 6;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(3, 278);
            label2.Name = "label2";
            label2.Size = new Size(100, 15);
            label2.TabIndex = 6;
            label2.Text = "Thông tin bộ nhớ";
            // 
            // Setting
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.PowderBlue;
            Controls.Add(label2);
            Controls.Add(panel1);
            Controls.Add(btn_logout);
            Controls.Add(button3);
            Controls.Add(btnXoaTaiKhoan);
            Controls.Add(button1);
            Controls.Add(label1);
            Margin = new Padding(2);
            Name = "Setting";
            Size = new Size(780, 326);
            Load += Setting_Load;
            panel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Button button1;
        private Button btnXoaTaiKhoan;
        private Button button3;
        private Button btn_logout;
        private ImageList imageList1;
        private Panel panel1;
        private Panel panel2;
        private Label label2;
    }
}
