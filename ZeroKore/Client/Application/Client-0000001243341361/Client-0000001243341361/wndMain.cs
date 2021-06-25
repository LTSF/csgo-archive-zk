using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ServiceProcess;
using System.Windows.Forms;
using Client_0000001243341361.Libs;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;

namespace Client_0000001243341361
{
    public partial class wndMain : Form
    {
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        enum KeyModifier
        {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4,
            WinKey = 8
        }

        private void UpdateMessage(string simple, string adv = null)
        {
            try
            {
                this.Invoke(new Action(() =>
                {
                    lblWelcomeCaption.Text = simple;
                    lblAdvSvcStat.Text = adv;
                }));
            }
            catch { }
        }

        private void T_StatusUpdater()
        {
            while (true)
            {
                try
                {
                    var dStatus = Client.GetDriverStatus();
                    switch (dStatus)
                    {
                        case Client.DriverStatus.Missing:
                            UpdateMessage("ZeroKore® Service isn't installed on this PC.", "ZK_SERVICE_MISSING");
                            this.Invoke(new Action(() =>
                            {
                                btnStartStop.Visible = false;
                                btnStartSvc.Enabled = false;
                                btnStopSvc.Enabled = false;
                                btnRmSvc.Enabled = false;

                                btnInsSvc.Enabled = true;
                            }));
                            break;
                        case Client.DriverStatus.Running:
                            UpdateMessage("ZeroKore® is ready at your command.", "ZK_RUNNING");
                            this.Invoke(new Action(() =>
                            {
                                btnInsSvc.Enabled = false;
                                btnStartSvc.Enabled = false;
                                btnStopSvc.Enabled = false;
                                btnRmSvc.Enabled = false;

                                btnStartStop.Visible = true;
                                btnStopSvc.Enabled = true;
                            }));
                            break;
                        case Client.DriverStatus.Stopped:
                            UpdateMessage("ZeroKore® Service isn't running.", "ZK_INACTIVE");
                            this.Invoke(new Action(() =>
                            {
                                btnInsSvc.Enabled = false;
                                btnStopSvc.Enabled = false;
                                btnStartStop.Visible = false;
                                btnStopSvc.Enabled = false;

                                btnRmSvc.Enabled = true;
                                btnStartSvc.Enabled = true;
                            }));
                            break;
                        case Client.DriverStatus.Unknown:
                            UpdateMessage("ZeroKore® isn't responding.", "ZK_UNKNOWN_STATUS");
                            this.Invoke(new Action(() =>
                            {
                                btnInsSvc.Enabled = false;
                                btnStopSvc.Enabled = false;
                                btnStartStop.Visible = false;
                                btnStopSvc.Enabled = false;
                                btnRmSvc.Enabled = false;
                                btnStartSvc.Enabled = false;
                            }));
                            break;
                        case Client.DriverStatus.Waiting:
                            UpdateMessage("ZeroKore® is busy.", "ZK_WAITING_STATUS");
                            this.Invoke(new Action(() =>
                            {
                                btnInsSvc.Enabled = false;
                                btnStopSvc.Enabled = false;
                                btnStartStop.Visible = false;
                                btnStopSvc.Enabled = false;
                                btnRmSvc.Enabled = false;
                                btnStartSvc.Enabled = false;
                            }));
                            break;
                    }

                    Thread.Sleep(5000);
                }
                catch { }
            }
        }

        public wndMain()
        {
            InitializeComponent();
            InitUX();

            new Thread(T_StatusUpdater).Start();
        }

        private void wndMain_Resize(object sender, EventArgs e)
        {
           
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            if (!Client.IsRunning && Client.FirstRun)
                Properties.Settings.Default.TimesPlayed += 1; Properties.Settings.Default.Save();

            if (!Client.IsRunning)
            {
                Client.FirstRun = false;
                Client.IsRunning = true;

                Client.StartZeroKore();

                btnStartStop.Text = "STOP";

                lblWelcomeCaption.Text = "ZeroKore® is enhancing your experience.";
            }
            else
            {
                Client.IsRunning = false;

                int Mins = Client.StopZeroKore();
                Properties.Settings.Default.MinsPlayed += Mins;

                lblTimePlayed.Text = Properties.Settings.Default.MinsPlayed.ToString();

                btnStartStop.Text = "REDEPLOY";

                lblWelcomeCaption.Text = "ZeroKore® has served you well.";
            }
        }

        private void hckIGM_CheckedChanged(object sender, EventArgs e)
        {
            if (hckIGM.Checked)
                Properties.Settings.Default.IGM = true;
            else
                Properties.Settings.Default.IGM = false;
        }

        private void btnInsSvc_Click(object sender, EventArgs e)
        {
            try { Process.Start("sc", "create ZeroKore type=kernel binpath=\"" + Directory.GetCurrentDirectory() + "\\0000002662474757.sys"); }
            catch { }
        }

        private void btnRmSvc_Click(object sender, EventArgs e)
        {
            try { Process.Start("sc", "delete ZeroKore"); }
            catch { }
        }

        private void btnStartSvc_Click(object sender, EventArgs e)
        {
            try { Process.Start("sc", "start ZeroKore"); }
            catch { }
        }

        private void btnStopSvc_Click(object sender, EventArgs e)
        {
            try { Process.Start("sc", "stop ZeroKore"); }
            catch { }
        }
    }
}
