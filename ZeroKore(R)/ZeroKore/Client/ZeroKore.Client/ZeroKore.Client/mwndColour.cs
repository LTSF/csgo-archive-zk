using System;
using System.Drawing;
using System.Windows.Forms;

namespace ZeroKore.Client
{
    public partial class mwndColour : Form
    {
        public mwndColour()
        {
            InitializeComponent();
        }



        private void hsbR_ValueChanged(object sender, EventArgs e)
        {
            pbxResult.BackColor = Color.FromArgb(hsbR.Value, hsbG.Value, hsbB.Value);
        }

        private void hsbG_ValueChanged(object sender, EventArgs e)
        {
            pbxResult.BackColor = Color.FromArgb(hsbR.Value, hsbG.Value, hsbB.Value);
        }

        private void hsbB_ValueChanged(object sender, EventArgs e)
        {
            pbxResult.BackColor = Color.FromArgb(hsbR.Value, hsbG.Value, hsbB.Value);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            ColorResult.R = hsbR.Value;
            ColorResult.G = hsbG.Value;
            ColorResult.B = hsbB.Value;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }

    public class ColorResult
    {
        public static int R;
        public static int G;
        public static int B;
    }
}
