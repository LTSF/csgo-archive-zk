using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ZeroKore.Client.UX
{
    class WinOverridePanel : Panel
    {
        public Color BackColour { get; set; } = Color.FromArgb(5, 20, 30);

        protected override void OnPaint(PaintEventArgs e)
        {
            var Angle = 90;

            ColorBlend colorBlend = new ColorBlend();
            colorBlend.Colors = new Color[] { BackColour, BackColour };
            colorBlend.Positions = new float[] { 0f, 1f };

            LinearGradientBrush brush = new LinearGradientBrush(this.ClientRectangle, BackColour, BackColour, Angle);
            brush.InterpolationColors = colorBlend;

            Graphics g = e.Graphics;
            g.FillRectangle(brush, this.ClientRectangle);

            base.OnPaint(e);
        }
    }
}
