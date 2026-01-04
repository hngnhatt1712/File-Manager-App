namespace ClientApp.Forms_UI
{
    partial class Downloaded
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
            pnlDropZone = new Panel();
            label1 = new Label();
            btnFile = new Button();
            flpHistory = new FlowLayoutPanel();
            pnlDropZone.SuspendLayout();
            SuspendLayout();
            // 
            // pnlDropZone
            // 
            pnlDropZone.AllowDrop = true;
            pnlDropZone.BackColor = Color.Cornsilk;
            pnlDropZone.Controls.Add(label1);
            pnlDropZone.Dock = DockStyle.Top;
            pnlDropZone.Location = new Point(0, 0);
            pnlDropZone.MaximumSize = new Size(0, 200);
            pnlDropZone.Name = "pnlDropZone";
            pnlDropZone.Size = new Size(787, 173);
            pnlDropZone.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Enabled = false;
            label1.Font = new Font("Comic Sans MS", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.AppWorkspace;
            label1.Location = new Point(345, 60);
            label1.Name = "label1";
            label1.Size = new Size(216, 26);
            label1.TabIndex = 0;
            label1.Text = "☁️ Kéo thả file vào đây";
            // 
            // btnFile
            // 
            btnFile.BackColor = Color.FromArgb(255, 192, 192);
            btnFile.FlatAppearance.BorderSize = 0;
            btnFile.FlatStyle = FlatStyle.Flat;
            btnFile.Font = new Font("Comic Sans MS", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnFile.ForeColor = Color.FromArgb(255, 255, 192);
            btnFile.Location = new Point(382, 214);
            btnFile.Name = "btnFile";
            btnFile.Size = new Size(162, 37);
            btnFile.TabIndex = 1;
            btnFile.Text = "📂 Chọn Tệp";
            btnFile.UseVisualStyleBackColor = false;
            btnFile.Click += btnFile_Click;
            // 
            // flpHistory
            // 
            flpHistory.AutoScroll = true;
            flpHistory.BackColor = Color.White;
            flpHistory.Dock = DockStyle.Bottom;
            flpHistory.FlowDirection = FlowDirection.TopDown;
            flpHistory.Location = new Point(0, 308);
            flpHistory.Name = "flpHistory";
            flpHistory.Size = new Size(787, 73);
            flpHistory.TabIndex = 3;
            flpHistory.WrapContents = false;
            // 
            // Downloaded
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            Controls.Add(flpHistory);
            Controls.Add(btnFile);
            Controls.Add(pnlDropZone);
            Margin = new Padding(2);
            Name = "Downloaded";
            Size = new Size(787, 381);
            Load += Downloaded_Load;
            pnlDropZone.ResumeLayout(false);
            pnlDropZone.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlDropZone;
        private Label label1;
        private Button btnFile;
        private FlowLayoutPanel flpHistory;
    }
}
