using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp
{
    public class RoundedPanel : Panel  
    {
        // Properties
        public int BorderRadius { get; set; } = 30;
        public Color BorderColor { get; set; } = Color.Transparent;

        // Constructor để chỉnh cho mượt hơn
        public RoundedPanel()
        {
            this.DoubleBuffered = true; // Chống nháy
            this.BackColor = Color.Transparent; // Để bo tròn không bị viền trắng
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Fix lỗi viền bị răng cưa: Vẽ hình chữ nhật nhỏ hơn 1 xíu
            RectangleF rect = new RectangleF(0, 0, this.Width - 1, this.Height - 1);

            using (GraphicsPath path = GetRoundedPath(rect, BorderRadius))
            using (Pen pen = new Pen(BorderColor, 1.75f))
            {
                this.Region = new Region(path); // Cắt hình thành bo tròn
                e.Graphics.DrawPath(pen, path); // Vẽ viền
            }
        }

        private GraphicsPath GetRoundedPath(RectangleF rect, float radius)
        {
            GraphicsPath path = new GraphicsPath();
            float d = radius * 2; // Đường kính góc bo

            // Vẽ 4 góc bo tròn
            path.AddArc(rect.X, rect.Y, d, d, 180, 90); // Góc trên trái
            path.AddArc(rect.Width - d, rect.Y, d, d, 270, 90); // Góc trên phải
            path.AddArc(rect.Width - d, rect.Height - d, d, d, 0, 90); // Góc dưới phải
            path.AddArc(rect.X, rect.Height - d, d, d, 90, 90); // Góc dưới trái

            path.CloseFigure();
            return path;
        }
    }  
}
