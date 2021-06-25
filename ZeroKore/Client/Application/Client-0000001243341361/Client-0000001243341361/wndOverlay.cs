using Client_0000001243341361.Libs;
using System;
using System.Windows.Forms;
using WinUtils;

namespace Client_0000001243341361
{
    public partial class wndOverlay : Form
    {
        public wndOverlay()
        {
            InitializeComponent();

            if (Client.LastPositionOfMSet != default)
                this.StartPosition = FormStartPosition.Manual;  this.Location = Client.LastPositionOfMSet;

            if (Properties.Settings.Default.AimAssist)
                chkAA.Checked = true;
            else
                chkAA.Checked = false;

            if (Properties.Settings.Default.AutoShoot)
                chkAS.Checked = true;
            else
                chkAS.Checked = false;

            if (Properties.Settings.Default.VisualAssistance)
                chkVA.Checked = true;
            else
                chkVA.Checked = false;

            if (Properties.Settings.Default.ConstJump)
                chkCJ.Checked = true;
            else
                chkCJ.Checked = false;

            if (Properties.Settings.Default.EnhancedRadar)
                chkER.Checked = true;
            else
                chkER.Checked = false;

            if (Properties.Settings.Default.FlashGlasses)
                chkFG.Checked = true;
            else
                chkFG.Checked = false;


        }

        private void pnlBody_MouseDown(object sender, MouseEventArgs e)
        {
            FormUtils.DragWindow(this.Handle);
        }

        private void wndOverlay_FormClosing(object sender, FormClosingEventArgs e)
        {
            Client.LastPositionOfMSet = this.Location;
        }

        private void chkAA_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAA.Checked)
                Properties.Settings.Default.AimAssist = true;
            else
                Properties.Settings.Default.AimAssist = false;

            Client.PauseZeroKore();
            Client.StartZeroKore();
        }

        private void chkAS_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAS.Checked)
                Properties.Settings.Default.AutoShoot = true;
            else
                Properties.Settings.Default.AutoShoot = false;

            Client.PauseZeroKore();
            Client.StartZeroKore();
        }

        private void chkVA_CheckedChanged(object sender, EventArgs e)
        {
            if (chkVA.Checked)
                Properties.Settings.Default.VisualAssistance = true;
            else
                Properties.Settings.Default.VisualAssistance = false;

            Client.PauseZeroKore();
            Client.StartZeroKore();
        }

        private void chkCJ_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCJ.Checked)
                Properties.Settings.Default.ConstJump = true;
            else
                Properties.Settings.Default.ConstJump = false;

            Client.PauseZeroKore();
            Client.StartZeroKore();
        }

        private void chkER_CheckedChanged(object sender, EventArgs e)
        {
            if (chkER.Checked)
                Properties.Settings.Default.EnhancedRadar = true;
            else
                Properties.Settings.Default.EnhancedRadar = false;

            Client.PauseZeroKore();
            Client.StartZeroKore();
        }

        private void chkFG_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFG.Checked)
                Properties.Settings.Default.FlashGlasses = true;
            else
                Properties.Settings.Default.FlashGlasses = false;

            Client.PauseZeroKore();
            Client.StartZeroKore();
        }

        private void wndOverlay_Load(object sender, EventArgs e)
        {
            this.Activate();
            this.Focus();
        }
    }
}
