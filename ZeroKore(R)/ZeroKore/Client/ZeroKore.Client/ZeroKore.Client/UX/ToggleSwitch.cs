using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ZeroKore.Client.UX
{
    class ToggleSwitch : CheckBox
    {
        public ToggleSwitch()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            Padding = new Padding(6);
        }

        public Color CheckedBack { get; set; } = Color.FromArgb(0, 60, 90);
        public Color UnCheckedBack { get; set; } = Color.FromArgb(0, 60, 90);

        public Color CheckedFore { get; set; } = Color.FromArgb(0, 250, 155);
        public Color UnCheckedFore { get; set; } = Color.LightGray;

        protected override void OnPaint(PaintEventArgs e)
        {
            this.OnPaintBackground(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (var path = new GraphicsPath())
            {
                var d = Padding.All;
                var r = this.Height - 2 * d;
                path.AddArc(d, d, r, r, 90, 180);
                path.AddArc(this.Width - r - d, d, r, r, -90, 180);
                path.CloseFigure();
                e.Graphics.FillPath(Checked ? new SolidBrush(CheckedBack) : new SolidBrush(UnCheckedBack), path);
                r = Height - 1;
                var rect = Checked ? new Rectangle(Width - r - 1, 0, r, r)
                                   : new Rectangle(0, 0, r, r);
                e.Graphics.FillEllipse(Checked ? new SolidBrush(CheckedFore) : new SolidBrush(UnCheckedFore), rect);
            }
        }
    }
}
