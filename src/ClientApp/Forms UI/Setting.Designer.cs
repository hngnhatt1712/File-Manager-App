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
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold);
            label1.ForeColor = Color.Red;
            label1.Location = new Point(413, 11);
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
            button1.Location = new Point(324, 69);
            button1.Name = "button1";
            button1.Size = new Size(279, 50);
            button1.TabIndex = 1;
            button1.Text = "       Đổi email và mật khẩu";
            button1.UseVisualStyleBackColor = false;
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
            // button2
            // 
            button2.BackColor = Color.DeepSkyBlue;
            button2.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            button2.ForeColor = SystemColors.ControlLightLight;
            button2.ImageAlign = ContentAlignment.MiddleLeft;
            button2.ImageKey = "Trash.png";
            button2.ImageList = imageList1;
            button2.Location = new Point(324, 135);
            button2.Name = "button2";
            button2.Size = new Size(279, 45);
            button2.TabIndex = 2;
            button2.Text = "Xóa tài khoản";
            button2.UseVisualStyleBackColor = false;
            // 
            // button3
            // 
            button3.BackColor = Color.DeepSkyBlue;
            button3.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            button3.ForeColor = SystemColors.ControlLightLight;
            button3.ImageAlign = ContentAlignment.MiddleLeft;
            button3.ImageKey = "Lecturer.png";
            button3.ImageList = imageList1;
            button3.Location = new Point(324, 200);
            button3.Name = "button3";
            button3.Size = new Size(279, 45);
            button3.TabIndex = 3;
            button3.Text = "Giới thiệu chung";
            button3.UseVisualStyleBackColor = false;
            // 
            // button4
            // 
            button4.BackColor = Color.DeepSkyBlue;
            button4.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            button4.ForeColor = SystemColors.ControlLightLight;
            button4.ImageAlign = ContentAlignment.MiddleLeft;
            button4.ImageKey = "Imac Exit.png";
            button4.ImageList = imageList1;
            button4.Location = new Point(324, 263);
            button4.Name = "button4";
            button4.Size = new Size(279, 45);
            button4.TabIndex = 4;
            button4.Text = "Đăng xuất";
            button4.UseVisualStyleBackColor = false;
            // 
            // Setting
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.PowderBlue;
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button2);
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
        private Button button2;
        private Button button3;
        private Button button4;
        private ImageList imageList1;
    }
}
