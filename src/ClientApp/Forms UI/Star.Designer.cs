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
            startFile.Name = "startFile";
            startFile.Size = new Size(787, 381);
            startFile.TabIndex = 0;
            // 
            // Star
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(startFile);
            Margin = new Padding(2);
            Name = "Star";
            Size = new Size(787, 381);
            Load += Star_Load;
            ResumeLayout(false);
        }

        #endregion

        private FileList startFile;
    }
}
