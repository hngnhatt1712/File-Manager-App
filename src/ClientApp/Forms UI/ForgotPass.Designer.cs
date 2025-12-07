namespace ClientApp
{
    partial class ForgotPass
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ForgotPass));
            pictureBox1 = new PictureBox();
            label1 = new Label();
            label2 = new Label();
            btn_send = new Button();
            tb_email = new TextBox();
            panel2 = new Panel();
            button1 = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(0, 42);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(640, 580);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.Highlight;
            label1.Location = new Point(725, 163);
            label1.Name = "label1";
            label1.Size = new Size(249, 37);
            label1.TabIndex = 1;
            label1.Text = "Recover Password";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 13.2000008F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(646, 287);
            label2.Name = "label2";
            label2.Size = new Size(143, 31);
            label2.TabIndex = 2;
            label2.Text = "User's Email";
            // 
            // btn_send
            // 
            btn_send.BackColor = SystemColors.Highlight;
            btn_send.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_send.ForeColor = Color.White;
            btn_send.Location = new Point(795, 379);
            btn_send.Name = "btn_send";
            btn_send.Size = new Size(147, 58);
            btn_send.TabIndex = 4;
            btn_send.Text = "Send";
            btn_send.UseVisualStyleBackColor = false;
            btn_send.Click += btn_send_Click_1;
            // 
            // tb_email
            // 
            tb_email.Location = new Point(795, 291);
            tb_email.Name = "tb_email";
            tb_email.Size = new Size(263, 27);
            tb_email.TabIndex = 3;
            // 
            // panel2
            // 
            panel2.BackColor = SystemColors.Highlight;
            panel2.Controls.Add(button1);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(1084, 45);
            panel2.TabIndex = 4;
            panel2.MouseDown += panel2_MouseDown;
            // 
            // button1
            // 
            button1.BackgroundImage = (Image)resources.GetObject("button1.BackgroundImage");
            button1.BackgroundImageLayout = ImageLayout.Stretch;
            button1.Dock = DockStyle.Right;
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Location = new Point(1033, 0);
            button1.Name = "button1";
            button1.Size = new Size(51, 45);
            button1.TabIndex = 0;
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // ForgotPass
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Window;
            ClientSize = new Size(1084, 617);
            Controls.Add(btn_send);
            Controls.Add(panel2);
            Controls.Add(tb_email);
            Controls.Add(label1);
            Controls.Add(label2);
            Controls.Add(pictureBox1);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 4, 3, 4);
            Name = "ForgotPass";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ForgotPass";
            Load += ForgotPass_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel2.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private Label label1;
        private Label label2;
        private Button btn_send;
        private TextBox tb_email;
        private Panel panel2;
        private Button button1;
    }
}