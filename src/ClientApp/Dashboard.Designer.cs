namespace ClientApp
{
    partial class Dashboard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Dashboard));
            btnTestLogout = new Button();
            SuspendLayout();
            // 
            // btnTestLogout
            // 
            btnTestLogout.Location = new Point(1110, 971);
            btnTestLogout.Name = "btnTestLogout";
            btnTestLogout.Size = new Size(410, 99);
            btnTestLogout.TabIndex = 38;
            btnTestLogout.Text = "Đăng Xuất";
            btnTestLogout.UseVisualStyleBackColor = true;
            btnTestLogout.Click += btnTestLogout_Click;
            // 
            // Dashboard
            // 
            AutoScaleDimensions = new SizeF(15F, 37F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(2164, 1423);
            Controls.Add(btnTestLogout);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(6, 7, 6, 7);
            Name = "Dashboard";
            Text = "Dashboard";
            Load += Dashboard_Load;
            ResumeLayout(false);
        }

        #endregion
        private Button btnTestLogout;
    }
}