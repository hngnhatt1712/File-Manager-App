namespace ClientApp
{
    partial class FileItem
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
            pbIcon = new PictureBox();
            btnDownload = new Button();
            lblFileName = new Label();
            btnDelete = new Button();
            btnStar = new Button();
            btnRename = new Button();
            ((System.ComponentModel.ISupportInitialize)pbIcon).BeginInit();
            SuspendLayout();
            // 
            // pbIcon
            // 
            pbIcon.Location = new Point(3, 3);
            pbIcon.Name = "pbIcon";
            pbIcon.Size = new Size(32, 34);
            pbIcon.TabIndex = 0;
            pbIcon.TabStop = false;
            // 
            // btnDownload
            // 
            btnDownload.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDownload.BackColor = Color.White;
            btnDownload.FlatAppearance.BorderSize = 0;
            btnDownload.FlatStyle = FlatStyle.Flat;
            btnDownload.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDownload.ForeColor = SystemColors.ActiveCaptionText;
            btnDownload.Location = new Point(600, -3);
            btnDownload.Name = "btnDownload";
            btnDownload.Size = new Size(43, 48);
            btnDownload.TabIndex = 1;
            btnDownload.Text = "⤓";
            btnDownload.UseVisualStyleBackColor = false;
            // 
            // lblFileName
            // 
            lblFileName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblFileName.AutoEllipsis = true;
            lblFileName.AutoSize = true;
            lblFileName.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblFileName.Location = new Point(47, 9);
            lblFileName.Name = "lblFileName";
            lblFileName.Size = new Size(48, 20);
            lblFileName.TabIndex = 2;
            lblFileName.Text = "label1";
            // 
            // btnDelete
            // 
            btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDelete.BackColor = Color.White;
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnDelete.ForeColor = Color.Lime;
            btnDelete.Location = new Point(737, 0);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(43, 48);
            btnDelete.TabIndex = 3;
            btnDelete.Text = "🗑️";
            btnDelete.UseVisualStyleBackColor = false;
            // 
            // btnStar
            // 
            btnStar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnStar.BackColor = Color.White;
            btnStar.FlatAppearance.BorderSize = 0;
            btnStar.FlatStyle = FlatStyle.Flat;
            btnStar.Font = new Font("Segoe UI", 27.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnStar.ForeColor = Color.Yellow;
            btnStar.Location = new Point(698, -9);
            btnStar.Name = "btnStar";
            btnStar.Size = new Size(43, 48);
            btnStar.TabIndex = 4;
            btnStar.Text = "⭐";
            btnStar.UseVisualStyleBackColor = false;
            // 
            // btnRename
            // 
            btnRename.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRename.BackColor = Color.White;
            btnRename.FlatAppearance.BorderSize = 0;
            btnRename.FlatStyle = FlatStyle.Flat;
            btnRename.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnRename.ForeColor = Color.DarkGoldenrod;
            btnRename.Location = new Point(649, -1);
            btnRename.Name = "btnRename";
            btnRename.Size = new Size(43, 48);
            btnRename.TabIndex = 5;
            btnRename.Text = "✏️";
            btnRename.UseVisualStyleBackColor = false;
            // 
            // File
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(btnRename);
            Controls.Add(btnStar);
            Controls.Add(btnDelete);
            Controls.Add(lblFileName);
            Controls.Add(btnDownload);
            Controls.Add(pbIcon);
            Name = "File";
            Size = new Size(780, 42);
            ((System.ComponentModel.ISupportInitialize)pbIcon).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pbIcon;
        private Button btnDownload;
        private Label lblFileName;
        private Button btnDelete;
        private Button btnStar;
        private Button btnRename;
    }
}
