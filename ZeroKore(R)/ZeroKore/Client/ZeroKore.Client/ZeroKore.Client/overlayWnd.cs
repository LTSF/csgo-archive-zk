using System;
using System.Windows.Forms;
using ZeroKore.Client.Properties;
using WinUtils;

namespace ZeroKore.Client
{
    public partial class overlayWnd : Form
    {
        public overlayWnd()
        {
            InitializeComponent();

            if (Client.LastMenuPos != default)
                this.StartPosition = FormStartPosition.Manual; this.Location = Client.LastMenuPos;
        }

        private void tsAA_CheckedChanged(object sender, EventArgs e)
        {
            if (tsAA.Checked)
            {
                Settings.Default.AA = true;

                if(Client.IsGameRunning())
                    Client.Modify(Client.Function.AimAssist, true);
            }
            else
            {
                Settings.Default.AA = false;

                if (Client.IsGameRunning())
                    Client.Modify(Client.Function.AimAssist, false);
            }
        }

        private void tsVA_CheckedChanged(object sender, EventArgs e)
        {
            if (tsVA.Checked)
            {
                Settings.Default.VA = true;

                if (Client.IsGameRunning())
                    Client.Modify(Client.Function.VisAssist, true);
            }
            else
            {
                Settings.Default.VA = false;

                if (Client.IsGameRunning())
                    Client.Modify(Client.Function.VisAssist, false);
            }
        }

        private void tsAS_CheckedChanged(object sender, EventArgs e)
        {
            if (tsAS.Checked)
            {
                Settings.Default.AS = true;

                if (Client.IsGameRunning())
                    Client.Modify(Client.Function.AutoShoot, true);
            }
            else
            {
                Settings.Default.AS = false;

                if (Client.IsGameRunning())
                    Client.Modify(Client.Function.AutoShoot, false);
            }
        }

        private void tsCJ_CheckedChanged(object sender, EventArgs e)
        {
            if (tsCJ.Checked)
            {
                Settings.Default.CJ = true;

                if (Client.IsGameRunning())
                    Client.Modify(Client.Function.ConstJump, true);
            }
            else
            {
                Settings.Default.CJ = false;

                if (Client.IsGameRunning())
                    Client.Modify(Client.Function.ConstJump, false);
            }
        }

        private void tsFG_CheckedChanged(object sender, EventArgs e)
        {
            if (tsFG.Checked)
            {
                Settings.Default.FG = true;

                if (Client.IsGameRunning())
                    Client.Modify(Client.Function.FlashGlasses, true);
            }
            else
            {
                Settings.Default.FG = false;

                if (Client.IsGameRunning())
                    Client.Modify(Client.Function.FlashGlasses, false);
            }
        }

        private void tsER_CheckedChanged(object sender, EventArgs e)
        {
            if (tsER.Checked)
            {
                Settings.Default.ER = true;

                if (Client.IsGameRunning())
                    Client.Modify(Client.Function.EnhancedRadar, true);
            }
            else
            {
                Settings.Default.ER = false;

                if (Client.IsGameRunning())
                    Client.Modify(Client.Function.EnhancedRadar, false);
            }
        }

        private void overlayWnd_FormClosing(object sender, FormClosingEventArgs e)
        {
            Client.LastMenuPos = this.Location;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pnlBody_MouseDown(object sender, MouseEventArgs e)
        {
            FormUtils.DragWindow(this.Handle);
        }
    }
}
