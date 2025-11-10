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
            label1 = new Label();
            panel2 = new Panel();
            pictureBox2 = new PictureBox();
            label2 = new Label();
            tb_email = new TextBox();
            panel5 = new Panel();
            btn_send = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            panel5.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Comic Sans MS", 21.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.FromArgb(128, 128, 255);
            label1.Location = new Point(38, 56);
            label1.Name = "label1";
            label1.Size = new Size(266, 40);
            label1.TabIndex = 1;
            label1.Text = "Quên mật khẩu ?";
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(192, 255, 255);
            panel2.Location = new Point(15, 47);
            panel2.Name = "panel2";
            panel2.Size = new Size(246, 2);
            panel2.TabIndex = 13;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.image_from_rawpixel_id_23103344_png;
            pictureBox2.Location = new Point(15, 18);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(22, 23);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 14;
            pictureBox2.TabStop = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(33, 0);
            label2.Name = "label2";
            label2.Size = new Size(104, 15);
            label2.TabIndex = 12;
            label2.Text = " Email người dùng";
            // 
            // tb_email
            // 
            tb_email.BorderStyle = BorderStyle.None;
            tb_email.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tb_email.Location = new Point(43, 18);
            tb_email.Name = "tb_email";
            tb_email.Size = new Size(220, 22);
            tb_email.TabIndex = 11;
            // 
            // panel5
            // 
            panel5.Controls.Add(label2);
            panel5.Controls.Add(tb_email);
            panel5.Controls.Add(panel2);
            panel5.Controls.Add(pictureBox2);
            panel5.Location = new Point(22, 114);
            panel5.Name = "panel5";
            panel5.Size = new Size(282, 60);
            panel5.TabIndex = 37;
            // 
            // btn_send
            // 
            btn_send.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_send.Location = new Point(114, 206);
            btn_send.Name = "btn_send";
            btn_send.Size = new Size(93, 37);
            btn_send.TabIndex = 39;
            btn_send.Text = "Xác nhận";
            btn_send.UseVisualStyleBackColor = true;
            btn_send.Click += btn_send_Click;
            // 
            // ForgotPass
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(350, 281);
            Controls.Add(btn_send);
            Controls.Add(label1);
            Controls.Add(panel5);
            Name = "ForgotPass";
            Text = "ForgotPass";
            Load += ForgotPass_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Panel panel2;
        private PictureBox pictureBox2;
        private Label label2;
        private TextBox tb_email;
        private Panel panel5;
        private Button btn_send;
    }
}