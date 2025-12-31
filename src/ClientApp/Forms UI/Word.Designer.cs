namespace ClientApp.Forms_UI
{
    partial class Word
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
            fileList1 = new FileList();
            SuspendLayout();
            // 
            // fileList1
            // 
            fileList1.Location = new Point(0, 0);
            fileList1.Name = "fileList1";
            fileList1.Size = new Size(787, 381);
            fileList1.TabIndex = 0;
            // 
            // Word
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(fileList1);
            Margin = new Padding(2);
            Name = "Word";
            Size = new Size(787, 381);
            Load += Word_Load;
            ResumeLayout(false);
        }

        #endregion

        private FileList fileList1;
    }
}
