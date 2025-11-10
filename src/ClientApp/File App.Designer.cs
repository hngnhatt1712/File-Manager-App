namespace ClientApp
{
    partial class File_App
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(File_App));
            tb_email = new TextBox();
            tb_pass = new TextBox();
            label1 = new Label();
            label2 = new Label();
            btn_login = new Button();
            btn_signup = new Button();
            pictureBox1 = new PictureBox();
            panel1 = new Panel();
            label3 = new Label();
            pictureBox2 = new PictureBox();
            panel2 = new Panel();
            panel3 = new Panel();
            pictureBox3 = new PictureBox();
            label4 = new Label();
            llb_forgotPass = new LinkLabel();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            SuspendLayout();
            // 
            // tb_email
            // 
            tb_email.BorderStyle = BorderStyle.None;
            tb_email.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tb_email.Location = new Point(56, 140);
            tb_email.Name = "tb_email";
            tb_email.Size = new Size(209, 22);
            tb_email.TabIndex = 0;
            // 
            // tb_pass
            // 
            tb_pass.BorderStyle = BorderStyle.None;
            tb_pass.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tb_pass.Location = new Point(56, 199);
            tb_pass.Name = "tb_pass";
            tb_pass.PasswordChar = '*';
            tb_pass.Size = new Size(210, 22);
            tb_pass.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(28, 121);
            label1.Name = "label1";
            label1.Size = new Size(101, 15);
            label1.TabIndex = 2;
            label1.Text = "Email người dùng";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(28, 181);
            label2.Name = "label2";
            label2.Size = new Size(57, 15);
            label2.TabIndex = 3;
            label2.Text = "Mật khẩu";
            // 
            // btn_login
            // 
            btn_login.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_login.Location = new Point(35, 263);
            btn_login.Name = "btn_login";
            btn_login.Size = new Size(221, 30);
            btn_login.TabIndex = 4;
            btn_login.Text = "Đăng nhập";
            btn_login.UseVisualStyleBackColor = true;
            btn_login.Click += btn_login_Click;
            // 
            // btn_signup
            // 
            btn_signup.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_signup.Location = new Point(35, 314);
            btn_signup.Name = "btn_signup";
            btn_signup.Size = new Size(221, 30);
            btn_signup.TabIndex = 5;
            btn_signup.Text = "Đăng ký";
            btn_signup.UseVisualStyleBackColor = true;
            btn_signup.Click += btn_signup_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.ErrorImage = null;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.InitialImage = (Image)resources.GetObject("pictureBox1.InitialImage");
            pictureBox1.Location = new Point(293, 59);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(288, 317);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 7;
            pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            panel1.Location = new Point(37, 305);
            panel1.Name = "panel1";
            panel1.Size = new Size(219, 2);
            panel1.TabIndex = 8;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(136, 296);
            label3.Name = "label3";
            label3.Size = new Size(23, 15);
            label3.TabIndex = 9;
            label3.Text = "OR";
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.image_from_rawpixel_id_23103344_png;
            pictureBox2.Location = new Point(28, 139);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(22, 23);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 10;
            pictureBox2.TabStop = false;
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(192, 255, 255);
            panel2.Location = new Point(28, 168);
            panel2.Name = "panel2";
            panel2.Size = new Size(238, 2);
            panel2.TabIndex = 9;
            // 
            // panel3
            // 
            panel3.BackColor = Color.FromArgb(192, 255, 255);
            panel3.Location = new Point(28, 227);
            panel3.Name = "panel3";
            panel3.Size = new Size(239, 2);
            panel3.TabIndex = 10;
            // 
            // pictureBox3
            // 
            pictureBox3.Image = Properties.Resources.image_from_rawpixel_id_23103341_png;
            pictureBox3.Location = new Point(28, 199);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(22, 23);
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.TabIndex = 11;
            pictureBox3.TabStop = false;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font(".VnBahamasBH", 27.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.ForeColor = Color.FromArgb(128, 128, 255);
            label4.Location = new Point(75, 46);
            label4.Name = "label4";
            label4.Size = new Size(159, 54);
            label4.TabIndex = 12;
            label4.Text = "File App";
            // 
            // llb_forgotPass
            // 
            llb_forgotPass.AutoSize = true;
            llb_forgotPass.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            llb_forgotPass.Location = new Point(147, 356);
            llb_forgotPass.Name = "llb_forgotPass";
            llb_forgotPass.Size = new Size(109, 20);
            llb_forgotPass.TabIndex = 14;
            llb_forgotPass.TabStop = true;
            llb_forgotPass.Text = "Quên mật khẩu";
            llb_forgotPass.LinkClicked += llb_forgotPass_LinkClicked;
            // 
            // File_App
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Window;
            ClientSize = new Size(633, 427);
            Controls.Add(llb_forgotPass);
            Controls.Add(label4);
            Controls.Add(pictureBox3);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(pictureBox2);
            Controls.Add(label3);
            Controls.Add(pictureBox1);
            Controls.Add(btn_signup);
            Controls.Add(btn_login);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(tb_pass);
            Controls.Add(tb_email);
            Controls.Add(panel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "File_App";
            Text = "File_App";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox tb_email;
        private TextBox tb_pass;
        private Label label1;
        private Label label2;
        private Button btn_login;
        private Button btn_signup;
        private PictureBox pictureBox1;
        private Panel panel1;
        private Label label3;
        private PictureBox pictureBox2;
        private Panel panel2;
        private Panel panel3;
        private PictureBox pictureBox3;
        private Label label4;
        private LinkLabel llb_forgotPass;
    }
}