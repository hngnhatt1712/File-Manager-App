namespace ClientApp
{
    partial class SignUp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SignUp));
            panel1 = new Panel();
            button1 = new Button();
            pictureBox1 = new PictureBox();
            label1 = new Label();
            label2 = new Label();
            tb_email = new TextBox();
            tb_sdt = new TextBox();
            label3 = new Label();
            tb_pass = new TextBox();
            label4 = new Label();
            tb_cfpass = new TextBox();
            label5 = new Label();
            pictureBox2 = new PictureBox();
            pictureBox3 = new PictureBox();
            pictureBox4 = new PictureBox();
            pictureBox5 = new PictureBox();
            btn_signup = new Button();
            lb_status = new Label();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.Highlight;
            panel1.Controls.Add(button1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1104, 49);
            panel1.TabIndex = 0;
            panel1.MouseDown += panel1_MouseDown;
            // 
            // button1
            // 
            button1.BackgroundImage = (Image)resources.GetObject("button1.BackgroundImage");
            button1.BackgroundImageLayout = ImageLayout.Stretch;
            button1.Dock = DockStyle.Right;
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Location = new Point(1050, 0);
            button1.Name = "button1";
            button1.Size = new Size(54, 49);
            button1.TabIndex = 1;
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(0, 49);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(597, 577);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.FlatStyle = FlatStyle.Flat;
            label1.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            label1.ForeColor = SystemColors.Highlight;
            label1.Location = new Point(778, 91);
            label1.Name = "label1";
            label1.Size = new Size(142, 46);
            label1.TabIndex = 2;
            label1.Text = "Sign up";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            label2.Location = new Point(706, 145);
            label2.Name = "label2";
            label2.Size = new Size(59, 25);
            label2.TabIndex = 3;
            label2.Text = "Email";
            // 
            // tb_email
            // 
            tb_email.Location = new Point(706, 182);
            tb_email.Name = "tb_email";
            tb_email.Size = new Size(315, 27);
            tb_email.TabIndex = 4;
            // 
            // tb_sdt
            // 
            tb_sdt.Location = new Point(706, 265);
            tb_sdt.Name = "tb_sdt";
            tb_sdt.Size = new Size(315, 27);
            tb_sdt.TabIndex = 6;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            label3.Location = new Point(706, 237);
            label3.Name = "label3";
            label3.Size = new Size(69, 25);
            label3.TabIndex = 5;
            label3.Text = "Phone";
            // 
            // tb_pass
            // 
            tb_pass.Location = new Point(706, 347);
            tb_pass.Name = "tb_pass";
            tb_pass.PasswordChar = '*';
            tb_pass.Size = new Size(315, 27);
            tb_pass.TabIndex = 8;
            tb_pass.UseSystemPasswordChar = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            label4.Location = new Point(706, 319);
            label4.Name = "label4";
            label4.Size = new Size(97, 25);
            label4.TabIndex = 7;
            label4.Text = "Password";
            // 
            // tb_cfpass
            // 
            tb_cfpass.Location = new Point(706, 436);
            tb_cfpass.Name = "tb_cfpass";
            tb_cfpass.PasswordChar = '*';
            tb_cfpass.Size = new Size(315, 27);
            tb_cfpass.TabIndex = 10;
            tb_cfpass.UseSystemPasswordChar = true;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            label5.Location = new Point(706, 408);
            label5.Name = "label5";
            label5.Size = new Size(175, 25);
            label5.TabIndex = 9;
            label5.Text = "Confirm Password";
            // 
            // pictureBox2
            // 
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.Location = new Point(675, 182);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(25, 27);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 11;
            pictureBox2.TabStop = false;
            // 
            // pictureBox3
            // 
            pictureBox3.Image = (Image)resources.GetObject("pictureBox3.Image");
            pictureBox3.Location = new Point(675, 265);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(25, 27);
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox3.TabIndex = 12;
            pictureBox3.TabStop = false;
            // 
            // pictureBox4
            // 
            pictureBox4.Image = (Image)resources.GetObject("pictureBox4.Image");
            pictureBox4.Location = new Point(675, 347);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(25, 27);
            pictureBox4.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox4.TabIndex = 13;
            pictureBox4.TabStop = false;
            // 
            // pictureBox5
            // 
            pictureBox5.Image = (Image)resources.GetObject("pictureBox5.Image");
            pictureBox5.Location = new Point(675, 436);
            pictureBox5.Name = "pictureBox5";
            pictureBox5.Size = new Size(25, 27);
            pictureBox5.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox5.TabIndex = 14;
            pictureBox5.TabStop = false;
            // 
            // btn_signup
            // 
            btn_signup.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            btn_signup.ForeColor = SystemColors.Highlight;
            btn_signup.Location = new Point(777, 485);
            btn_signup.Name = "btn_signup";
            btn_signup.Size = new Size(143, 62);
            btn_signup.TabIndex = 15;
            btn_signup.Text = "Oke";
            btn_signup.UseVisualStyleBackColor = true;
            btn_signup.Click += btn_signup_Click_1;
            // 
            // lb_status
            // 
            lb_status.AutoSize = true;
            lb_status.Font = new Font("Segoe UI", 14F);
            lb_status.Location = new Point(788, 550);
            lb_status.Name = "lb_status";
            lb_status.Size = new Size(0, 32);
            lb_status.TabIndex = 16;
            // 
            // SignUp
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Window;
            ClientSize = new Size(1104, 623);
            Controls.Add(lb_status);
            Controls.Add(btn_signup);
            Controls.Add(pictureBox5);
            Controls.Add(pictureBox4);
            Controls.Add(pictureBox3);
            Controls.Add(pictureBox2);
            Controls.Add(tb_cfpass);
            Controls.Add(label5);
            Controls.Add(tb_pass);
            Controls.Add(label4);
            Controls.Add(tb_sdt);
            Controls.Add(label3);
            Controls.Add(tb_email);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(pictureBox1);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 4, 3, 4);
            Name = "SignUp";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SignUp";
            Load += SignUp_Load;
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private PictureBox pictureBox3;
        private Panel panel1;
        private Button button1;
        private Label label1;
        private PictureBox pictureBox1;
        private Label label2;
        private TextBox tb_email;
        private TextBox tb_sdt;
        private Label label3;
        private TextBox tb_pass;
        private Label label4;
        private TextBox tb_cfpass;
        private Label label5;
        private PictureBox pictureBox2;
        private PictureBox pictureBox4;
        private PictureBox pictureBox5;
        private Button btn_signup;
        private Label lb_status;
    }
}