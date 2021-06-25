using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace ZeroKore.Client.UX
{
    public partial class SovietskyBar : UserControl
    {
        public SovietskyBar()
        {
            InitializeComponent();

            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.UserPaint |
                          ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.ResizeRedraw |
                          ControlStyles.ContainerControl |
                          ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.SupportsTransparentBackColor
                          , true);
        }

        private bool pGlow = true;
        private int pValue = 0;

        public bool IsGlowing
        {
            get { return pGlow; }
        }
        public int Value
        {
            get { return pValue; }
            set
            {
                if (value > 100 || value < 0)
                    return;
                else
                    pValue = value;

                this.Invalidate();
            }
        }

        public void DisableGlow()
        {
            pGlow = false;
            this.Invalidate();
        }
        public void EnableGlow()
        {
            pGlow = true;
            GlowUpdater.Start();
            this.Invalidate();
        }

        public Color BackColour { get; set; } = Color.FromArgb(230, 230, 230);
        public Color ValueColour { get; set; } = Color.FromArgb(0, 155, 135);
        public Color GlowColour { get; set; } = Color.FromArgb(30, 175, 150);

        private int pGlowPosition = -300;

        private double Round(double Value)
        {
            return (double)Math.Round(Value, 0);
        }


        private void DrawGlowEffect(Graphics g)
        {
            var r = new Rectangle(pGlowPosition, 0, 60, this.Height);
            var lgb = new LinearGradientBrush(r, Color.White, Color.White, LinearGradientMode.Horizontal);
            var cb = new ColorBlend(4);
            cb.Colors = new Color[] { Color.Transparent, GlowColour, GlowColour, Color.Transparent };
            cb.Positions = new float[] { 0.0F, 0.5F, 0.6F, 1.0F };
            lgb.InterpolationColors = cb;
            var clip = new Rectangle(0, 0, this.Width, this.Height)
            {
                Width = (int)Round(Value / 100.0f * this.Width)
            };
            g.SetClip(clip);
            g.FillRectangle(lgb, r);
            g.ResetClip();
        }

        private void DrawBackground(Graphics g)
        {
            Rectangle r = this.ClientRectangle;
            g.FillRectangle(new SolidBrush(BackColour), r);
        }

        private void DrawValue(Graphics g)
        {
            var r = new Rectangle(0, 0, this.Width, this.Height);
            r.Width = (int)Round(pValue / 100.0f * this.Width);
            g.FillRectangle(new SolidBrush(ValueColour), r);
        }

        private void SovietskyBar_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            DrawBackground(e.Graphics);
            DrawValue(e.Graphics);
            if (pGlow)
                DrawGlowEffect(e.Graphics);
        }

        private void GlowUpdater_Tick(object sender, EventArgs e)
        {
            if (pGlow)
            {
                pGlowPosition += 5;

                if (pGlowPosition > this.Width)
                {
                    pGlowPosition = -300;
                    this.Invalidate();
                }

                if (pGlowPosition > -100)
                    this.Invalidate();
            }
            else
            {
                pGlowPosition += 5;
                if (pGlowPosition > this.Width)
                {
                    pGlowPosition = -300;
                    this.Invalidate();

                    GlowUpdater.Stop();
                }
            }
        }
    }
}
