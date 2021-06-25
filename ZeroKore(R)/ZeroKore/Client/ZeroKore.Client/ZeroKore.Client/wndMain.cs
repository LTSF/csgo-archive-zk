using System;
using System.Drawing;
using System.Windows.Forms;
using ZeroKore.Client.Properties;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Threading;

namespace ZeroKore.Client
{
    public partial class wndMain : Form
    {
        public wndMain()
        {
            InitializeComponent();
            InitApp();

            new Thread(T_StatusUpdater).Start();
        }

        private HotKey hkm;
       
        #region Driver Status
        private enum Status
        {
            Ready,
            NotReady,
            Busy,
            Unknown
        }

        public enum DriverStatus
        {
            Running,
            Stopped,
            Missing,
            Waiting,
            Unknown
        }

        private void UpdateStatus(string status, Status st, int value)
        {
            try
            {
                this.Invoke(new Action(() =>
                {
                    lblActionStatus.Text = status;
                    svcStatus.Value = value;
                    
                    switch(st)
                    {
                        case Status.Ready:
                            pbReady.Image = Resources.checkmark_96px;
                            break;
                        case Status.NotReady:
                            pbReady.Image = Resources.delete_96px;
                            break;             
                        case Status.Busy:
                            pbReady.Image = Resources.accuracy_96px;
                            break;
                        case Status.Unknown:
                            pbReady.Image = Resources.question_mark_96px;
                            break;
                        default:
                            pbReady.Image = Resources.question_mark_96px;
                            break;
                    }
                }));
            }
            catch { }
        }

        public static DriverStatus GetDriverStatus()
        {
            try
            {
                ServiceController dSC = new ServiceController("ZeroKore");

                switch (dSC.Status)
                {
                    case ServiceControllerStatus.Running:
                        return DriverStatus.Running;
                    case ServiceControllerStatus.StopPending:
                        return DriverStatus.Waiting;
                    case ServiceControllerStatus.Stopped:
                        return DriverStatus.Stopped;
                    case ServiceControllerStatus.StartPending:
                        return DriverStatus.Waiting;
                    default:
                        return DriverStatus.Unknown;
                }
            }
            catch
            {
                return DriverStatus.Missing;
            }
        }


        private void T_StatusUpdater()
        {
            while(true)
            {
                try
                { 
                    var dStatus = GetDriverStatus();

                    switch(dStatus)
                    {
                        case DriverStatus.Missing:
                            UpdateStatus("ZeroKore® Service is not installed.", Status.NotReady, 0);
                            this.Invoke(new Action(() =>
                            {
                                btnStartStop.Visible = false;
                                btnStartSvc.Enabled = false;
                                btnStopSvc.Enabled = false;
                                btnRmSvc.Enabled = false;
                                btnInstSvc.Enabled = true;
                            }));
                            break;
                        case DriverStatus.Running:
                            UpdateStatus("ZeroKore® Service is operational.", Status.Ready, 100);
                            this.Invoke(new Action(() =>
                            {
                                btnStartStop.Visible = true; 
                                btnStartSvc.Enabled = false;
                                btnStopSvc.Enabled = true;
                                btnRmSvc.Enabled = false;
                                btnInstSvc.Enabled = false;
                            }));
                            break;
                        case DriverStatus.Stopped:
                            UpdateStatus("ZeroKore® Service is not operational.", Status.NotReady, 50);
                            this.Invoke(new Action(() =>
                            {
                                btnStartStop.Visible = false;
                                btnStartSvc.Enabled = true;
                                btnStopSvc.Enabled = false;
                                btnRmSvc.Enabled = true;
                                btnInstSvc.Enabled = false;
                            }));
                            break;
                        case DriverStatus.Unknown:
                            UpdateStatus("ZeroKore® could not retreive any information about the ZeroKore® Service", Status.NotReady, 0);
                            this.Invoke(new Action(() =>
                            {
                                btnStartStop.Visible = false;
                                btnStartSvc.Enabled = false;
                                btnStopSvc.Enabled = false;
                                btnRmSvc.Enabled = false;
                                btnInstSvc.Enabled = false;
                            }));
                            break;
                        case DriverStatus.Waiting:
                            UpdateStatus("ZeroKore® Service is busy.", Status.Busy, 75);
                            this.Invoke(new Action(() =>
                            {
                                btnStartStop.Visible = false;
                                btnStartSvc.Enabled = false;
                                btnStopSvc.Enabled = false;
                                btnRmSvc.Enabled = false;
                                btnInstSvc.Enabled = false;
                            }));
                            break;
                    }

                    Thread.Sleep(5000);
                }
                catch { }
            }
        }
        #endregion

        #region SideBar
        private void btnDashboard_Click(object sender, EventArgs e)
        {
            pnlPageHome.BringToFront();
            btnDashboard.BackColor = Color.FromArgb(32, 0, 10, 15);
            btnZeroKore.BackColor = Color.Transparent;
            btnControlCentre.BackColor = Color.Transparent;
            btnSysDriver.BackColor = Color.Transparent;
        }

        private void btnZeroKore_Click(object sender, EventArgs e)
        {
            pnlPageDeploy.BringToFront();
            btnDashboard.BackColor = Color.Transparent;
            btnZeroKore.BackColor = Color.FromArgb(32, 0, 10, 15);
            btnControlCentre.BackColor = Color.Transparent;
            btnSysDriver.BackColor = Color.Transparent;
        }

        private void btnControlCentre_Click(object sender, EventArgs e)
        {
            pnlPageCC.BringToFront();
            btnDashboard.BackColor = Color.Transparent;
            btnZeroKore.BackColor = Color.Transparent;
            btnControlCentre.BackColor = Color.FromArgb(32, 0, 10, 15);
            btnSysDriver.BackColor = Color.Transparent;
        }
        private void btnSysDriver_Click(object sender, EventArgs e)
        {
            pnlDriver.BringToFront();
            btnDashboard.BackColor = Color.Transparent;
            btnZeroKore.BackColor = Color.Transparent;
            btnControlCentre.BackColor = Color.Transparent;
            btnSysDriver.BackColor = Color.FromArgb(32, 0, 10, 15);
        }
        #endregion
        

        #region Control Centre
        private void lnkChangeTeam_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var result = new mwndColour().ShowDialog();

            switch (result)
            {
                case DialogResult.Cancel:
                    break;
                case DialogResult.OK:
                    goto SetTeamColor;
                default:
                    break;
            }

            return;

            SetTeamColor:
            {
                pbTeam.BackColor = Color.FromArgb(ColorResult.R, ColorResult.G, ColorResult.B);
                Settings.Default.TeamCol = Color.FromArgb(ColorResult.R, ColorResult.G, ColorResult.B);
            }
        }

        private void lnkChangeEnemy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var result = new mwndColour().ShowDialog();

            switch (result)
            {
                case DialogResult.Cancel:
                    break;
                case DialogResult.OK:
                    goto SetEnemyColor;
                default:
                    break;
            }

            return;

            SetEnemyColor:
            {
                pbEnemy.BackColor = Color.FromArgb(ColorResult.R, ColorResult.G, ColorResult.B);
                Settings.Default.EnemyCol = Color.FromArgb(ColorResult.R, ColorResult.G, ColorResult.B);
            }
        }

        private void tsAA_CheckedChanged(object sender, EventArgs e)
        {
            if (tsAA.Checked)
                Settings.Default.AA = true;
            else
                Settings.Default.AA = false;
        }

        private void tsVA_CheckedChanged(object sender, EventArgs e)
        {
            if (tsVA.Checked)
            {
                Settings.Default.VA = true;
                gbColourOptions.Visible = true;
            }
            else
            {
                Settings.Default.VA = false;
                gbColourOptions.Visible = false;
            }
        }

        private void tsAS_CheckedChanged(object sender, EventArgs e)
        {
            if (tsAS.Checked)
                Settings.Default.AS = true;
            else
                Settings.Default.AS = false;
        }

        private void tsCJ_CheckedChanged(object sender, EventArgs e)
        {
            if (tsCJ.Checked)
                Settings.Default.CJ = true;
            else
                Settings.Default.CJ = false;
        }

        private void tsFG_CheckedChanged(object sender, EventArgs e)
        {
            if (tsFG.Checked)
                Settings.Default.FG = true;
            else
                Settings.Default.FG = false;
        }

        private void tsER_CheckedChanged(object sender, EventArgs e)
        {
            if (tsER.Checked)
                Settings.Default.ER = true;
            else
                Settings.Default.ER = false;
        }
        #endregion

        #region Deployment
        private void tsZPN_CheckedChanged(object sender, EventArgs e)
        {
            if (tsZPN.Checked)
                Settings.Default.ZPN = true;
            else
                Settings.Default.ZPN = false;
        }

        private void tsIGM_CheckedChanged(object sender, EventArgs e)
        {
            if (tsIGM.Checked)
                Settings.Default.IGM = true;
            else
                Settings.Default.IGM = false;
        }

        private bool FirstRun = true;
        private void btnStartStop_Click(object sender, EventArgs e)
        {
            if (!Client.IsRunning())
            {
                if(FirstRun)
                    Settings.Default.TimesPlayed += 1; Settings.Default.Save();

                if (!Settings.Default.ZPN)
                    new Thread(Waiter).Start();
                else if (Settings.Default.ZPN)
                    Client.StartPerf();

                FirstRun = false;

                Client.timePlayed.Reset();
                Client.timePlayed.Start();

                pnlstf.Enabled = false;
                btnStartStop.Text = "UNLOAD";
            }
            else if (Client.IsRunning())
            {
                Client.timePlayed.Stop();
                Settings.Default.MinsPlayed += (int)Client.timePlayed.Elapsed.TotalMinutes;
                statMins.Text = Settings.Default.MinsPlayed.ToString();

                Settings.Default.Save();

                if (!Settings.Default.ZPN)
                    Client.Stop();
                if (Settings.Default.ZPN)
                    Client.Terminate();

                pnlstf.Enabled = true;
                btnStartStop.Text = "REDEPLOY";
            }
        }


        private void Waiter()
        {
            while (!Client.IsGameRunning())
                Thread.Sleep(1000);

            Client.Start();
        }

        #endregion

        #region Service Manager
        private void btnInstSvc_Click(object sender, EventArgs e)
        {
            svcStatus.Value = 25;
            lblActionStatus.Text = "Installing Service . . .";

            try { Process.Start("sc", "create ZeroKore type=kernel binpath=\"" + Directory.GetCurrentDirectory() + "\\ZK.Driver.sys"); }
            catch (Exception ex)
            {
                MessageBox.Show("Could not create the ZeroKore® service.\n\n" + ex.Message, "Install Service", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRmSvc_Click(object sender, EventArgs e)
        {
            svcStatus.Value = 25;
            lblActionStatus.Text = "Removing Service . . .";

            try { Process.Start("sc", "delete ZeroKore"); }
            catch (Exception ex)
            {
                MessageBox.Show("Could not remove the ZeroKore® service.\n\n" + ex.Message, "Remove Service", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnStartSvc_Click(object sender, EventArgs e)
        {
            svcStatus.Value = 25;
            lblActionStatus.Text = "Starting Service . . .";

            try { Process.Start("sc", "start ZeroKore"); }
            catch (Exception ex)
            {
                MessageBox.Show("Could not start the ZeroKore® service.\n\n" + ex.Message, "Start Service", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnStopSvc_Click(object sender, EventArgs e)
        {
            svcStatus.Value = 25;
            lblActionStatus.Text = "Stopping Service . . .";

            try { Process.Start("sc", "stop ZeroKore"); }
            catch (Exception ex)
            {
                MessageBox.Show("Could not stop the ZeroKore® service.\n\n" + ex.Message, "Stop Service", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
    }
}
