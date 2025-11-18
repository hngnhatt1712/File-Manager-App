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
            pictureBox1 = new PictureBox();
            label1 = new Label();
            panel4 = new Panel();
            pictureBox5 = new PictureBox();
            label5 = new Label();
            tb_email = new TextBox();
            pictureBox3 = new PictureBox();
            panel3 = new Panel();
            label2 = new Label();
            tb_pass = new TextBox();
            btn_signup = new Button();
            btn_login = new Button();
            panel1 = new Panel();
            label3 = new Label();
            llb_forgotPass = new LinkLabel();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.folder;
            pictureBox1.Location = new Point(763, 133);
            pictureBox1.Margin = new Padding(6, 7, 6, 7);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(759, 957);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 2;
            pictureBox1.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft Sans Serif", 36F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.FromArgb(128, 128, 255);
            label1.Location = new Point(156, 79);
            label1.Margin = new Padding(6, 0, 6, 0);
            label1.Name = "label1";
            label1.Size = new Size(455, 122);
            label1.TabIndex = 3;
            label1.Text = "File App";
            // 
            // panel4
            // 
            panel4.BackColor = Color.Teal;
            panel4.Location = new Point(71, 409);
            panel4.Margin = new Padding(6, 7, 6, 7);
            panel4.Name = "panel4";
            panel4.Size = new Size(561, 5);
            panel4.TabIndex = 25;
            // 
            // pictureBox5
            // 
            pictureBox5.Image = Properties.Resources.mail;
            pictureBox5.Location = new Point(69, 340);
            pictureBox5.Margin = new Padding(6, 7, 6, 7);
            pictureBox5.Name = "pictureBox5";
            pictureBox5.Size = new Size(49, 74);
            pictureBox5.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox5.TabIndex = 26;
            pictureBox5.TabStop = false;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(69, 296);
            label5.Margin = new Padding(6, 0, 6, 0);
            label5.Name = "label5";
            label5.Size = new Size(228, 37);
            label5.TabIndex = 24;
            label5.Text = "Email người dùng";
            // 
            // tb_email
            // 
            tb_email.BorderStyle = BorderStyle.None;
            tb_email.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tb_email.Location = new Point(131, 340);
            tb_email.Margin = new Padding(6, 7, 6, 7);
            tb_email.Name = "tb_email";
            tb_email.Size = new Size(499, 45);
            tb_email.TabIndex = 23;
            // 
            // pictureBox3
            // 
            pictureBox3.Image = Properties.Resources.image_from_rawpixel_id_23103341_png;
            pictureBox3.Location = new Point(71, 486);
            pictureBox3.Margin = new Padding(6, 7, 6, 7);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(47, 57);
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.TabIndex = 30;
            pictureBox3.TabStop = false;
            // 
            // panel3
            // 
            panel3.BackColor = Color.Teal;
            panel3.Location = new Point(71, 555);
            panel3.Margin = new Padding(6, 7, 6, 7);
            panel3.Name = "panel3";
            panel3.Size = new Size(555, 5);
            panel3.TabIndex = 29;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(71, 442);
            label2.Margin = new Padding(6, 0, 6, 0);
            label2.Name = "label2";
            label2.Size = new Size(128, 37);
            label2.TabIndex = 28;
            label2.Text = "Mật khẩu";
            // 
            // tb_pass
            // 
            tb_pass.BorderStyle = BorderStyle.None;
            tb_pass.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tb_pass.Location = new Point(131, 486);
            tb_pass.Margin = new Padding(6, 7, 6, 7);
            tb_pass.Name = "tb_pass";
            tb_pass.PasswordChar = '*';
            tb_pass.Size = new Size(499, 45);
            tb_pass.TabIndex = 27;
            // 
            // btn_signup
            // 
            btn_signup.BackColor = Color.WhiteSmoke;
            btn_signup.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_signup.ForeColor = Color.Blue;
            btn_signup.Location = new Point(105, 858);
            btn_signup.Margin = new Padding(6, 7, 6, 7);
            btn_signup.Name = "btn_signup";
            btn_signup.Size = new Size(497, 74);
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
            btn_login.Location = new Point(105, 693);
            btn_login.Margin = new Padding(6, 7, 6, 7);
            btn_login.Name = "btn_login";
            btn_login.Size = new Size(497, 74);
            btn_login.TabIndex = 32;
            btn_login.Text = "Đăng nhập";
            btn_login.UseVisualStyleBackColor = false;
            btn_login.Click += btn_login_Click;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(0, 64, 64);
            panel1.Location = new Point(103, 812);
            panel1.Margin = new Padding(6, 7, 6, 7);
            panel1.Name = "panel1";
            panel1.Size = new Size(493, 5);
            panel1.TabIndex = 30;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(317, 775);
            label3.Margin = new Padding(6, 0, 6, 0);
            label3.Name = "label3";
            label3.Size = new Size(66, 46);
            label3.TabIndex = 33;
            label3.Text = "OR";
            // 
            // llb_forgotPass
            // 
            llb_forgotPass.AutoSize = true;
            llb_forgotPass.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            llb_forgotPass.Location = new Point(354, 940);
            llb_forgotPass.Margin = new Padding(6, 0, 6, 0);
            llb_forgotPass.Name = "llb_forgotPass";
            llb_forgotPass.Size = new Size(265, 46);
            llb_forgotPass.TabIndex = 34;
            llb_forgotPass.TabStop = true;
            llb_forgotPass.Text = "Quên mật khẩu?";
            llb_forgotPass.LinkClicked += llb_forgotPass_LinkClicked;
            // 
            // FileApp
            // 
            AutoScaleDimensions = new SizeF(15F, 37F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Window;
            ClientSize = new Size(1579, 1182);
            Controls.Add(llb_forgotPass);
            Controls.Add(label3);
            Controls.Add(btn_login);
            Controls.Add(btn_signup);
            Controls.Add(pictureBox3);
            Controls.Add(panel3);
            Controls.Add(label2);
            Controls.Add(tb_pass);
            Controls.Add(panel4);
            Controls.Add(pictureBox5);
            Controls.Add(label5);
            Controls.Add(tb_email);
            Controls.Add(label1);
            Controls.Add(pictureBox1);
            Controls.Add(panel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(6, 7, 6, 7);
            Name = "FileApp";
            Text = "FileApp";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private Label label1;
        private Panel panel4;
        private PictureBox pictureBox5;
        private Label label5;
        private TextBox tb_email;
        private PictureBox pictureBox3;
        private Panel panel3;
        private Label label2;
        private TextBox tb_pass;
        private Button btn_signup;
        private Button btn_login;
        private Panel panel1;
        private Label label3;
        private LinkLabel llb_forgotPass;
    }
}