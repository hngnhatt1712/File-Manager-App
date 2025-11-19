namespace ClientApp
{
    partial class FileApp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileApp));
            label1 = new Label();
            panel4 = new Panel();
            label5 = new Label();
            tb_email = new TextBox();
            panel3 = new Panel();
            label2 = new Label();
            tb_pass = new TextBox();
            btn_signup = new Button();
            btn_login = new Button();
            panel1 = new Panel();
            label3 = new Label();
            llb_forgotPass = new LinkLabel();
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            pictureBox3 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font(".VnBahamasBH", 36F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.FromArgb(128, 128, 255);
            label1.Location = new Point(73, 32);
            label1.Name = "label1";
            label1.Size = new Size(205, 71);
            label1.TabIndex = 3;
            label1.Text = "File App";
            // 
            // panel4
            // 
            panel4.BackColor = Color.Teal;
            panel4.Location = new Point(33, 166);
            panel4.Name = "panel4";
            panel4.Size = new Size(262, 2);
            panel4.TabIndex = 25;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(32, 120);
            label5.Name = "label5";
            label5.Size = new Size(101, 15);
            label5.TabIndex = 24;
            label5.Text = "Email người dùng";
            // 
            // tb_email
            // 
            tb_email.BorderStyle = BorderStyle.None;
            tb_email.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tb_email.Location = new Point(61, 140);
            tb_email.Name = "tb_email";
            tb_email.Size = new Size(234, 20);
            tb_email.TabIndex = 23;
            // 
            // panel3
            // 
            panel3.BackColor = Color.Teal;
            panel3.Location = new Point(33, 225);
            panel3.Name = "panel3";
            panel3.Size = new Size(259, 2);
            panel3.TabIndex = 29;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(33, 179);
            label2.Name = "label2";
            label2.Size = new Size(57, 15);
            label2.TabIndex = 28;
            label2.Text = "Mật khẩu";
            // 
            // tb_pass
            // 
            tb_pass.BorderStyle = BorderStyle.None;
            tb_pass.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tb_pass.Location = new Point(61, 197);
            tb_pass.Name = "tb_pass";
            tb_pass.PasswordChar = '*';
            tb_pass.Size = new Size(233, 20);
            tb_pass.TabIndex = 27;
            // 
            // btn_signup
            // 
            btn_signup.BackColor = Color.WhiteSmoke;
            btn_signup.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_signup.ForeColor = Color.Blue;
            btn_signup.Location = new Point(49, 348);
            btn_signup.Name = "btn_signup";
            btn_signup.Size = new Size(232, 30);
            btn_signup.TabIndex = 31;
            btn_signup.Text = "Đăng ký";
            btn_signup.UseVisualStyleBackColor = false;
            btn_signup.Click += btn_signup_Click;
            // 
            // btn_login
            // 
            btn_login.BackColor = Color.WhiteSmoke;
            btn_login.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_login.ForeColor = Color.Blue;
            btn_login.Location = new Point(49, 281);
            btn_login.Name = "btn_login";
            btn_login.Size = new Size(232, 30);
            btn_login.TabIndex = 32;
            btn_login.Text = "Đăng nhập";
            btn_login.UseVisualStyleBackColor = false;
            btn_login.Click += btn_login_Click;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(0, 64, 64);
            panel1.Location = new Point(48, 329);
            panel1.Name = "panel1";
            panel1.Size = new Size(230, 2);
            panel1.TabIndex = 30;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(148, 314);
            label3.Name = "label3";
            label3.Size = new Size(29, 20);
            label3.TabIndex = 33;
            label3.Text = "OR";
            // 
            // llb_forgotPass
            // 
            llb_forgotPass.AutoSize = true;
            llb_forgotPass.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            llb_forgotPass.Location = new Point(165, 381);
            llb_forgotPass.Name = "llb_forgotPass";
            llb_forgotPass.Size = new Size(116, 20);
            llb_forgotPass.TabIndex = 34;
            llb_forgotPass.TabStop = true;
            llb_forgotPass.Text = "Quên mật khẩu?";
            llb_forgotPass.LinkClicked += llb_forgotPass_LinkClicked;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.mail;
            pictureBox1.Location = new Point(26, 135);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(36, 30);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 35;
            pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources._lock;
            pictureBox2.Location = new Point(26, 194);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(36, 30);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 36;
            pictureBox2.TabStop = false;
            // 
            // pictureBox3
            // 
            pictureBox3.Image = Properties.Resources.folder;
            pictureBox3.Location = new Point(328, 32);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(382, 422);
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.TabIndex = 37;
            pictureBox3.TabStop = false;
            // 
            // FileApp
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Window;
            ClientSize = new Size(737, 479);
            Controls.Add(pictureBox3);
            Controls.Add(pictureBox2);
            Controls.Add(pictureBox1);
            Controls.Add(llb_forgotPass);
            Controls.Add(label3);
            Controls.Add(btn_login);
            Controls.Add(btn_signup);
            Controls.Add(panel3);
            Controls.Add(label2);
            Controls.Add(tb_pass);
            Controls.Add(panel4);
            Controls.Add(label5);
            Controls.Add(tb_email);
            Controls.Add(label1);
            Controls.Add(panel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FileApp";
            Text = "FileApp";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Panel panel4;
        private Label label5;
        private TextBox tb_email;
        private Panel panel3;
        private Label label2;
        private TextBox tb_pass;
        private Button btn_signup;
        private Button btn_login;
        private Panel panel1;
        private Label label3;
        private LinkLabel llb_forgotPass;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private PictureBox pictureBox3;
    }
}