namespace ClientApp.Forms_UI
{
    partial class ChangeEAP
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChangeEAP));
            label1 = new Label();
            label2 = new Label();
            label4 = new Label();
            txtEmailCu = new TextBox();
            txtNewPassword = new TextBox();
            btnUpdatePassword = new Button();
            imageList1 = new ImageList(components);
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.OrangeRed;
            label1.Location = new Point(206, 9);
            label1.Name = "label1";
            label1.Size = new Size(340, 32);
            label1.TabIndex = 0;
            label1.Text = "Thay đổi Email hoặc Password";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold);
            label2.ForeColor = Color.Red;
            label2.Location = new Point(31, 65);
            label2.Name = "label2";
            label2.Size = new Size(89, 25);
            label2.TabIndex = 1;
            label2.Text = "Email cũ:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold);
            label4.ForeColor = Color.Red;
            label4.Location = new Point(31, 126);
            label4.Name = "label4";
            label4.Size = new Size(135, 25);
            label4.TabIndex = 3;
            label4.Text = "Password mới:";
            // 
            // txtEmailCu
            // 
            txtEmailCu.Enabled = false;
            txtEmailCu.Location = new Point(206, 65);
            txtEmailCu.Name = "txtEmailCu";
            txtEmailCu.ReadOnly = true;
            txtEmailCu.Size = new Size(375, 23);
            txtEmailCu.TabIndex = 4;
            // 
            // txtNewPassword
            // 
            txtNewPassword.Location = new Point(206, 126);
            txtNewPassword.Name = "txtNewPassword";
            txtNewPassword.Size = new Size(375, 23);
            txtNewPassword.TabIndex = 6;
            txtNewPassword.TextChanged += textBox3_TextChanged;
            // 
            // btnUpdatePassword
            // 
            btnUpdatePassword.BackgroundImage = (Image)resources.GetObject("btnUpdatePassword.BackgroundImage");
            btnUpdatePassword.BackgroundImageLayout = ImageLayout.Zoom;
            btnUpdatePassword.FlatAppearance.BorderSize = 0;
            btnUpdatePassword.FlatStyle = FlatStyle.Flat;
            btnUpdatePassword.Location = new Point(604, 117);
            btnUpdatePassword.Name = "btnUpdatePassword";
            btnUpdatePassword.Size = new Size(63, 38);
            btnUpdatePassword.TabIndex = 8;
            btnUpdatePassword.UseVisualStyleBackColor = true;
            btnUpdatePassword.Click += button2_Click;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = Color.Transparent;
            imageList1.Images.SetKeyName(0, "Available Updates.png");
            // 
            // ChangeEAP
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.LightBlue;
            ClientSize = new Size(800, 183);
            Controls.Add(btnUpdatePassword);
            Controls.Add(txtNewPassword);
            Controls.Add(txtEmailCu);
            Controls.Add(label4);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "ChangeEAP";
            Text = "ChangeEAP";
            Load += ChangeEAP_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label4;
        private TextBox txtEmailCu;
        private TextBox txtNewPassword;
        private Button btnUpdatePassword;
        private ImageList imageList1;
    }
}