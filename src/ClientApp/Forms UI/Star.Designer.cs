namespace ClientApp.Forms_UI
{
    partial class Star
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
            startFile = new FileList();
            SuspendLayout();
            // 
            // startFile
            // 
            startFile.Location = new Point(-1, 0);
            startFile.Margin = new Padding(3, 5, 3, 5);
            startFile.Name = "startFile";
            startFile.Size = new Size(899, 508);
            startFile.TabIndex = 0;
            startFile.Load += startFile_Load;
            // 
            // Star
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(startFile);
            Margin = new Padding(2, 3, 2, 3);
            Name = "Star";
            Size = new Size(899, 508);
            Load += Star_Load;
            ResumeLayout(false);
        }

        #endregion

        private FileList startFile;
    }
}
