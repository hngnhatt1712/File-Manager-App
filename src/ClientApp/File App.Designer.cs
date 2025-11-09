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
            tb_name = new TextBox();
            tb_pass = new TextBox();
            label1 = new Label();
            label2 = new Label();
            btn_login = new Button();
            btn_signup = new Button();
            label3 = new Label();
            pictureBox1 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // tb_name
            // 
            tb_name.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tb_name.Location = new Point(56, 115);
            tb_name.Name = "tb_name";
            tb_name.Size = new Size(176, 27);
            tb_name.TabIndex = 0;
            // 
            // tb_pass
            // 
            tb_pass.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tb_pass.Location = new Point(56, 168);
            tb_pass.Name = "tb_pass";
            tb_pass.Size = new Size(176, 27);
            tb_pass.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(56, 95);
            label1.Name = "label1";
            label1.Size = new Size(90, 15);
            label1.TabIndex = 2;
            label1.Text = "Tên người dùng";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(56, 150);
            label2.Name = "label2";
            label2.Size = new Size(57, 15);
            label2.TabIndex = 3;
            label2.Text = "Mật khẩu";
            // 
            // btn_login
            // 
            btn_login.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_login.Location = new Point(33, 239);
            btn_login.Name = "btn_login";
            btn_login.Size = new Size(221, 30);
            btn_login.TabIndex = 4;
            btn_login.Text = "Đăng nhập";
            btn_login.UseVisualStyleBackColor = true;
            // 
            // btn_signup
            // 
            btn_signup.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_signup.Location = new Point(33, 290);
            btn_signup.Name = "btn_signup";
            btn_signup.Size = new Size(221, 30);
            btn_signup.TabIndex = 5;
            btn_signup.Text = "Đăng ký";
            btn_signup.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(129, 272);
            label3.Name = "label3";
            label3.Size = new Size(23, 15);
            label3.TabIndex = 6;
            label3.Text = "OR";
            // 
            // pictureBox1
            // 
            pictureBox1.ErrorImage = null;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.InitialImage = (Image)resources.GetObject("pictureBox1.InitialImage");
            pictureBox1.Location = new Point(350, 59);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(325, 342);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 7;
            pictureBox1.TabStop = false;
            // 
            // File_App
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 464);
            Controls.Add(pictureBox1);
            Controls.Add(label3);
            Controls.Add(btn_signup);
            Controls.Add(btn_login);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(tb_pass);
            Controls.Add(tb_name);
            Name = "File_App";
            Text = "File_App";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox tb_name;
        private TextBox tb_pass;
        private Label label1;
        private Label label2;
        private Button btn_login;
        private Button btn_signup;
        private Label label3;
        private PictureBox pictureBox1;
    }
}