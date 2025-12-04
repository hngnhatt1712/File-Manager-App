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
            button1 = new Button();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(644, 219);
            button1.Name = "button1";
            button1.Size = new Size(205, 656);
            button1.TabIndex = 0;
            button1.Text = "file gắn sao";
            button1.UseVisualStyleBackColor = true;
            // 
            // Star
            // 
            AutoScaleDimensions = new SizeF(15F, 37F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(button1);
            Name = "Star";
            Size = new Size(1103, 1010);
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
    }
}
