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
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            button1 = new Button();
            button2 = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold);
            label1.ForeColor = Color.Red;
            label1.Location = new Point(234, 17);
            label1.Name = "label1";
            label1.Size = new Size(363, 37);
            label1.TabIndex = 0;
            label1.Text = "Thông Tin File App Manager";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 14.25F);
            label2.Location = new Point(96, 99);
            label2.Name = "label2";
            label2.Size = new Size(62, 25);
            label2.TabIndex = 1;
            label2.Text = "Email:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 14.25F);
            label3.Location = new Point(63, 169);
            label3.Name = "label3";
            label3.Size = new Size(95, 25);
            label3.TabIndex = 2;
            label3.Text = "Password:";
            // 
            // textBox1
            // 
            textBox1.BackColor = Color.Gainsboro;
            textBox1.Enabled = false;
            textBox1.Location = new Point(234, 99);
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(393, 23);
            textBox1.TabIndex = 3;
            // 
            // textBox2
            // 
            textBox2.BackColor = Color.Gainsboro;
            textBox2.Enabled = false;
            textBox2.Location = new Point(234, 171);
            textBox2.Name = "textBox2";
            textBox2.ReadOnly = true;
            textBox2.Size = new Size(393, 23);
            textBox2.TabIndex = 4;
            // 
            // button1
            // 
            button1.BackColor = Color.PeachPuff;
            button1.Font = new Font("Segoe UI", 12F);
            button1.ForeColor = Color.Red;
            button1.Location = new Point(466, 254);
            button1.Name = "button1";
            button1.Size = new Size(161, 44);
            button1.TabIndex = 5;
            button1.Text = "Đổi mật khẩu";
            button1.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            button2.BackColor = Color.PeachPuff;
            button2.Font = new Font("Segoe UI", 12F);
            button2.ForeColor = Color.Red;
            button2.Location = new Point(234, 254);
            button2.Name = "button2";
            button2.Size = new Size(161, 44);
            button2.TabIndex = 6;
            button2.Text = "Thông tin App";
            button2.UseVisualStyleBackColor = false;
            // 
            // Setting
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Margin = new Padding(2);
            Name = "Setting";
            Size = new Size(780, 326);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox textBox1;
        private TextBox textBox2;
        private Button button1;
        private Button button2;
    }
}
