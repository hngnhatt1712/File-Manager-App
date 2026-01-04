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
            btn_logout = new Button();
            panel1 = new Panel();
            label2 = new Label();
            label4 = new Label();
            label5 = new Label();
            button3 = new Button();
            progressBar1 = new ProgressBar();
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
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 326);
            panel1.Name = "panel1";
            panel1.Size = new Size(780, 0);
            panel1.TabIndex = 5;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 12.25F, FontStyle.Bold);
            label2.Location = new Point(3, 271);
            label2.Name = "label2";
            label2.Size = new Size(148, 23);
            label2.TabIndex = 6;
            label2.Text = "Thông tin bộ nhớ:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(347, 164);
            label4.Name = "label4";
            label4.Size = new Size(0, 15);
            label4.TabIndex = 8;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI Semibold", 12.25F, FontStyle.Bold);
            label5.Location = new Point(145, 270);
            label5.Name = "label5";
            label5.Size = new Size(27, 23);
            label5.TabIndex = 9;
            label5.Text = "số";
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
            // progressBar1
            // 
            progressBar1.BackColor = SystemColors.ActiveBorder;
            progressBar1.Dock = DockStyle.Bottom;
            progressBar1.Location = new Point(0, 296);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(780, 30);
            progressBar1.TabIndex = 10;
            // 
            // Setting
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.PowderBlue;
            Controls.Add(progressBar1);
            Controls.Add(label5);
            Controls.Add(label4);
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
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Button button1;
        private Button btnXoaTaiKhoan;
        private Button btn_logout;
        private ImageList imageList1;
        private Panel panel1;
        private Label label2;
        private Label label4;
        private Label label5;
        private Button button3;
        private ProgressBar progressBar1;
    }
}
