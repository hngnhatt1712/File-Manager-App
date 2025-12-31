namespace ClientApp.Forms_UI
{
    partial class Home
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
            homeFileList = new FileList();
            SuspendLayout();
            // 
            // homeFileList
            // 
            homeFileList.Location = new Point(0, 0);
            homeFileList.Name = "homeFileList";
            homeFileList.Size = new Size(787, 381);
            homeFileList.TabIndex = 0;
            // 
            // Home
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(homeFileList);
            Margin = new Padding(2);
            Name = "Home";
            Size = new Size(787, 381);
            Load += TrangChu_Load;
            ResumeLayout(false);
        }

        #endregion

        private FileList homeFileList;
    }
}
