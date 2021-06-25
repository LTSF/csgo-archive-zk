using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ZeroKore.Client.UX { 
    class GradientPanel : Panel
    {
        public Color TopColor { get; set; } = Color.FromArgb(0, 200, 175);
        public Color BottomColor { get; set; } = Color.FromArgb(0, 250, 155);

        public int Angle { get; set; } = 90;

        protected override void OnPaint(PaintEventArgs e)
        {
            ColorBlend colorBlend = new ColorBlend();
            colorBlend.Colors = new Color[] { TopColor, BottomColor };
            colorBlend.Positions = new float[] { 0f, 1f };

            LinearGradientBrush brush = new LinearGradientBrush(this.ClientRectangle, TopColor, BottomColor, Angle);
            brush.InterpolationColors = colorBlend;

            Graphics g = e.Graphics;
            g.FillRectangle(brush, this.ClientRectangle);

            base.OnPaint(e);
        }
    }
}
