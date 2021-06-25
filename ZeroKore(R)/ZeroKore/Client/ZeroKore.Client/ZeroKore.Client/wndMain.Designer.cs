using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ZeroKore.Client.Properties;
using WinUtils;

namespace ZeroKore.Client
{
    partial class wndMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region System Tray 
        private bool AlternateIcon = false;
        private void sysTrayAnimator_Tick(object sender, EventArgs e)
        {
            if (AlternateIcon)
                sysTray.Icon = Resources.Logo;
            else if (!AlternateIcon)
                sysTray.Icon = Resources.Logo_Alt;

            sysTray.Visible = true;
            AlternateIcon = !AlternateIcon;
        }

        private bool LastCallBack = false;
        private EventHandler LCB = null;
        private void sysTrayPushNotification(string title, string text = " ", ToolTipIcon icon = ToolTipIcon.None, EventHandler callback = null)
        {
            sysTray.BalloonTipTitle = title;
            sysTray.BalloonTipText = text;
            sysTray.BalloonTipIcon = icon;

            if (LastCallBack)
            {
                sysTray.BalloonTipClicked -= LCB;
                LastCallBack = false;
            }

            if (callback != null)
            {
                sysTray.BalloonTipClicked += callback;
                LastCallBack = true;
                LCB = callback;
            }

            sysTray.ShowBalloonTip(10000);
        }

        private void sysTray_Click(object sender, EventArgs e)
        {
            this.Show();
        }
        #endregion

        #region Window UX
        private Color TopColor = Color.FromArgb(0, 30, 45);
        private Color BottomColor = Color.FromArgb(0, 10, 15);
        protected override void OnPaint(PaintEventArgs e)
        {
            var Angle = 0;

            ColorBlend colorBlend = new ColorBlend();
            colorBlend.Colors = new Color[] { TopColor, BottomColor };
            colorBlend.Positions = new float[] { 0f, 1f };

            LinearGradientBrush brush = new LinearGradientBrush(this.ClientRectangle, TopColor, BottomColor, Angle);
            brush.InterpolationColors = colorBlend;

            Graphics g = e.Graphics;
            g.FillRectangle(brush, this.ClientRectangle);

            base.OnPaint(e);
        }

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
             int nLeftRect,
             int nTopRect,
             int nRightRect,
             int nBottomRect,
             int nWidthEllipse,
             int nHeightEllipse
        );
        public struct MARGINS
        {
            public int leftWidth;
            public int rightWidth;
            public int topHeight;
            public int bottomHeight;
        }

        [DllImport("dwmapi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);
        [DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);
        [DllImport("dwmapi.dll")]
        public static extern int DwmIsCompositionEnabled(ref int pfEnabled);

        private bool m_aeroEnabled;

        private const int CS_DROPSHADOW = 0x00020000;

        private const int cGrip = 16;
        private const int cCaption = 32;

        private const int WM_NCPAINT = 0x0085;
        private const int RESIZE_CODE = 0x84;

        private bool CheckAeroEnabled()
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                int enabled = 0;
                DwmIsCompositionEnabled(ref enabled);
                return (enabled == 1) ? true : false;
            }
            return false;
        }
        protected override CreateParams CreateParams
        {
            get
            {
                m_aeroEnabled = CheckAeroEnabled();

                CreateParams cp = base.CreateParams;
                cp.Style |= Constants.WS_SYSMENU;

                if (!m_aeroEnabled)
                    cp.ClassStyle |= CS_DROPSHADOW;

                return cp;
            }
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCPAINT:
                    if (m_aeroEnabled)
                    {
                        var v = 2;
                        DwmSetWindowAttribute(this.Handle, 2, ref v, 4);
                        MARGINS margins = new MARGINS()
                        {
                            bottomHeight = 1,
                            leftWidth = 0,
                            rightWidth = 0,
                            topHeight = 0
                        };
                        DwmExtendFrameIntoClientArea(this.Handle, ref margins);

                    }
                    break;
                case RESIZE_CODE:
                    Point pos = new Point(m.LParam.ToInt32());
                    pos = this.PointToClient(pos);
                    if (pos.Y < cCaption)
                    {
                        m.Result = (IntPtr)2;
                        return;
                    }
                    if (pos.X >= this.ClientSize.Width - cGrip && pos.Y >= this.ClientSize.Height - cGrip)
                    {
                        m.Result = (IntPtr)17;
                        return;
                    }
                    break;
                default:
                    break;
            }
            base.WndProc(ref m);
        }
        #endregion

        #region Crucial Event Handlers
        private void pnlTitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            FormUtils.DragWindow(this.Handle);
        }

        private void pnlTitleBar_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                FormUtils.ShowSystemMenu(this.Handle);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();

            if (!Client.IsRunning())
            {
                sysTray.Visible = false;
                Client.Terminate();
            }
            else
            {
                this.Hide();
                sysTrayPushNotification("ZeroKore® Client", "is running in the background. Click this notification to terminate.", ToolTipIcon.None, (s, ev) => { sysTray.Visible = false; Client.Terminate(); } );
            }
        }

        private void btnMax_Click(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Maximized)
                this.WindowState = FormWindowState.Maximized;
            else
                this.WindowState = FormWindowState.Normal;
        }

        private void btnMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void wndMain_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                btnMax.Image = Resources.maximised_transparent;
            }
            else
            {
                btnMax.Image = Resources.maximise_transparent;
            }

            if (this.WindowState != FormWindowState.Minimized)
            {
                this.Invalidate();
                pnlSideBar.Invalidate();
                pnlTitleBar.Invalidate();
            }
        }
        private void wndMain_Load(object sender, EventArgs e)
        {
            sysTrayPushNotification("Welcome back " + Environment.UserName + "!");

            hkm = new HotKey();
            hkm.RegisterHotKey(ZeroKore.Client.ModifierKeys.None, Settings.Default.HOTKEY);
            hkm.KeyPressed += hkm_ToggleMenu;
        }
        #endregion

        #region User Form Code
        private void InitApp()
        {
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.UserPaint |
                          ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.ResizeRedraw |
                          ControlStyles.ContainerControl |
                          ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.SupportsTransparentBackColor
                          , true);

            Screen screen = Screen.FromControl(this);

            int x = screen.WorkingArea.X - screen.Bounds.X;
            int y = screen.WorkingArea.Y - screen.Bounds.Y;
            this.MaximizedBounds = new Rectangle(x, y, screen.WorkingArea.Width, screen.WorkingArea.Height);
            this.MaximumSize = screen.WorkingArea.Size;
                
            if (Settings.Default.IGM)
                tsIGM.Checked = true;
            else
                tsIGM.Checked = false;

            if (Settings.Default.ZPN)
                tsZPN.Checked = true;
            else
                tsZPN.Checked = false;


            if (Settings.Default.AA)
                tsAA.Checked = true;
            else
                tsAA.Checked = false;

            if (Settings.Default.VA)
                tsVA.Checked = true;
            else
                tsVA.Checked = false;

            if (Settings.Default.AS)
                tsAS.Checked = true;
            else
                tsAS.Checked = false;

            if (Settings.Default.CJ)
                tsCJ.Checked = true;
            else
                tsCJ.Checked = false;

            if (Settings.Default.FG)
                tsFG.Checked = true;
            else
                tsFG.Checked = false;

            if (Settings.Default.ER)
                tsER.Checked = true;
            else
                tsER.Checked = false;

            pbTeam.BackColor = Settings.Default.TeamCol;
            pbEnemy.BackColor = Settings.Default.EnemyCol;
        }

        private bool IsOverlayOpen = false;
        private overlayWnd Overlay;
        private void hkm_ToggleMenu(object sender, KeyPressedEventArgs e)
        {
            if (Settings.Default.IGM)
            {
                if (IsOverlayOpen)
                {
                    Overlay.Close();
                    Overlay.Dispose();
                    IsOverlayOpen = false;

                    InitApp();

                    return;
                }
                if (!IsOverlayOpen)
                {
                    Overlay = new overlayWnd();
                    Overlay.Show();
                    IsOverlayOpen = true;

                    return;
                }
            }
        }
        #endregion

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(wndMain));
            this.sysTray = new System.Windows.Forms.NotifyIcon(this.components);
            this.sysTrayAnimator = new System.Windows.Forms.Timer(this.components);
            this.pnlPageHome = new System.Windows.Forms.Panel();
            this.lblProductManufacturer = new System.Windows.Forms.Label();
            this.lblProductName = new System.Windows.Forms.Label();
            this.pbxLogo = new System.Windows.Forms.PictureBox();
            this.pbReady = new System.Windows.Forms.PictureBox();
            this.lblDashTitle = new System.Windows.Forms.Label();
            this.brd_stath = new System.Windows.Forms.Panel();
            this.lblUsageStatsH = new System.Windows.Forms.Label();
            this.pnlPageDeploy = new System.Windows.Forms.Panel();
            this.pnlHParams = new System.Windows.Forms.Panel();
            this.lblParams = new System.Windows.Forms.Label();
            this.btnStartStop = new System.Windows.Forms.Button();
            this.lblDeployH = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.pnlPageCC = new System.Windows.Forms.Panel();
            this.brd_ft = new System.Windows.Forms.Panel();
            this.lblCC = new System.Windows.Forms.Label();
            this.lblCCH = new System.Windows.Forms.Label();
            this.pnlDriver = new System.Windows.Forms.Panel();
            this.brd_hsvcctrl = new System.Windows.Forms.Panel();
            this.lblSvcControl = new System.Windows.Forms.Label();
            this.lblDriver = new System.Windows.Forms.Label();
            this.brd_params = new ZeroKore.Client.UX.GradientPanel();
            this.pnlParams = new System.Windows.Forms.Panel();
            this.lblIGM = new System.Windows.Forms.Label();
            this.tsZPN = new ZeroKore.Client.UX.ToggleSwitch();
            this.lblZPN = new System.Windows.Forms.Label();
            this.tsIGM = new ZeroKore.Client.UX.ToggleSwitch();
            this.brd_stat = new ZeroKore.Client.UX.GradientPanel();
            this.pnlStat = new System.Windows.Forms.Panel();
            this.lblMp = new System.Windows.Forms.Label();
            this.lblTp = new System.Windows.Forms.Label();
            this.statMins = new System.Windows.Forms.Label();
            this.statLaunches = new System.Windows.Forms.Label();
            this.brd_stf = new ZeroKore.Client.UX.GradientPanel();
            this.pnlstf = new System.Windows.Forms.Panel();
            this.gbColourOptions = new System.Windows.Forms.GroupBox();
            this.lnkChangeEnemy = new System.Windows.Forms.LinkLabel();
            this.lnkChangeTeam = new System.Windows.Forms.LinkLabel();
            this.lblEC = new System.Windows.Forms.Label();
            this.lblTC = new System.Windows.Forms.Label();
            this.pbEnemy = new System.Windows.Forms.PictureBox();
            this.pbTeam = new System.Windows.Forms.PictureBox();
            this.lblFG = new System.Windows.Forms.Label();
            this.tsFG = new ZeroKore.Client.UX.ToggleSwitch();
            this.lblER = new System.Windows.Forms.Label();
            this.tsER = new ZeroKore.Client.UX.ToggleSwitch();
            this.lblCJ = new System.Windows.Forms.Label();
            this.tsCJ = new ZeroKore.Client.UX.ToggleSwitch();
            this.lblAS = new System.Windows.Forms.Label();
            this.tsAS = new ZeroKore.Client.UX.ToggleSwitch();
            this.lblVA = new System.Windows.Forms.Label();
            this.tsVA = new ZeroKore.Client.UX.ToggleSwitch();
            this.lblAA = new System.Windows.Forms.Label();
            this.tsAA = new ZeroKore.Client.UX.ToggleSwitch();
            this.pnlSideBar = new ZeroKore.Client.UX.GradientPanel();
            this.btnHelp = new System.Windows.Forms.Button();
            this.btnSysDriver = new System.Windows.Forms.Button();
            this.btnControlCentre = new System.Windows.Forms.Button();
            this.btnZeroKore = new System.Windows.Forms.Button();
            this.btnDashboard = new System.Windows.Forms.Button();
            this.pnlTitleBar = new ZeroKore.Client.UX.GradientPanel();
            this.pnlSpacer = new ZeroKore.Client.UX.WinOverridePanel();
            this.btnMin = new System.Windows.Forms.Button();
            this.btnMax = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.brd_svcctrl = new ZeroKore.Client.UX.GradientPanel();
            this.pnlSvcCtrl = new System.Windows.Forms.Panel();
            this.lblActionStatus = new System.Windows.Forms.Label();
            this.svcStatus = new ZeroKore.Client.UX.SovietskyBar();
            this.btnStopSvc = new System.Windows.Forms.Button();
            this.btnStartSvc = new System.Windows.Forms.Button();
            this.btnRmSvc = new System.Windows.Forms.Button();
            this.btnInstSvc = new System.Windows.Forms.Button();
            this.pnlPageHome.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbReady)).BeginInit();
            this.brd_stath.SuspendLayout();
            this.pnlPageDeploy.SuspendLayout();
            this.pnlHParams.SuspendLayout();
            this.pnlPageCC.SuspendLayout();
            this.brd_ft.SuspendLayout();
            this.pnlDriver.SuspendLayout();
            this.brd_hsvcctrl.SuspendLayout();
            this.brd_params.SuspendLayout();
            this.pnlParams.SuspendLayout();
            this.brd_stat.SuspendLayout();
            this.pnlStat.SuspendLayout();
            this.brd_stf.SuspendLayout();
            this.pnlstf.SuspendLayout();
            this.gbColourOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbEnemy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTeam)).BeginInit();
            this.pnlSideBar.SuspendLayout();
            this.pnlTitleBar.SuspendLayout();
            this.brd_svcctrl.SuspendLayout();
            this.pnlSvcCtrl.SuspendLayout();
            this.SuspendLayout();
            // 
            // sysTray
            // 
            this.sysTray.Icon = ((System.Drawing.Icon)(resources.GetObject("sysTray.Icon")));
            this.sysTray.Text = "ZeroKore®";
            this.sysTray.Visible = true;
            this.sysTray.Click += new System.EventHandler(this.sysTray_Click);
            // 
            // sysTrayAnimator
            // 
            this.sysTrayAnimator.Enabled = true;
            this.sysTrayAnimator.Interval = 500;
            this.sysTrayAnimator.Tick += new System.EventHandler(this.sysTrayAnimator_Tick);
            // 
            // pnlPageHome
            // 
            this.pnlPageHome.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlPageHome.BackColor = System.Drawing.Color.Transparent;
            this.pnlPageHome.Controls.Add(this.lblProductManufacturer);
            this.pnlPageHome.Controls.Add(this.lblProductName);
            this.pnlPageHome.Controls.Add(this.pbxLogo);
            this.pnlPageHome.Controls.Add(this.pbReady);
            this.pnlPageHome.Controls.Add(this.lblDashTitle);
            this.pnlPageHome.Controls.Add(this.brd_stath);
            this.pnlPageHome.Controls.Add(this.brd_stat);
            this.pnlPageHome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(250)))), ((int)(((byte)(155)))));
            this.pnlPageHome.Location = new System.Drawing.Point(63, 36);
            this.pnlPageHome.Name = "pnlPageHome";
            this.pnlPageHome.Size = new System.Drawing.Size(627, 402);
            this.pnlPageHome.TabIndex = 5;
            // 
            // lblProductManufacturer
            // 
            this.lblProductManufacturer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProductManufacturer.AutoEllipsis = true;
            this.lblProductManufacturer.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProductManufacturer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.lblProductManufacturer.Location = new System.Drawing.Point(106, 350);
            this.lblProductManufacturer.Name = "lblProductManufacturer";
            this.lblProductManufacturer.Size = new System.Drawing.Size(474, 29);
            this.lblProductManufacturer.TabIndex = 15;
            this.lblProductManufacturer.Text = "© VodCode Lab 2020";
            // 
            // lblProductName
            // 
            this.lblProductName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProductName.AutoEllipsis = true;
            this.lblProductName.Font = new System.Drawing.Font("Segoe UI Semilight", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProductName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(200)))), ((int)(((byte)(175)))));
            this.lblProductName.Location = new System.Drawing.Point(103, 315);
            this.lblProductName.Name = "lblProductName";
            this.lblProductName.Size = new System.Drawing.Size(474, 35);
            this.lblProductName.TabIndex = 14;
            this.lblProductName.Text = "ZeroKore®";
            // 
            // pbxLogo
            // 
            this.pbxLogo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pbxLogo.Image = global::ZeroKore.Client.Properties.Resources.LogoPNG;
            this.pbxLogo.Location = new System.Drawing.Point(23, 315);
            this.pbxLogo.Name = "pbxLogo";
            this.pbxLogo.Size = new System.Drawing.Size(64, 64);
            this.pbxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbxLogo.TabIndex = 13;
            this.pbxLogo.TabStop = false;
            // 
            // pbReady
            // 
            this.pbReady.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbReady.Image = global::ZeroKore.Client.Properties.Resources.question_mark_96px;
            this.pbReady.Location = new System.Drawing.Point(481, 18);
            this.pbReady.Name = "pbReady";
            this.pbReady.Size = new System.Drawing.Size(96, 96);
            this.pbReady.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbReady.TabIndex = 12;
            this.pbReady.TabStop = false;
            // 
            // lblDashTitle
            // 
            this.lblDashTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDashTitle.AutoEllipsis = true;
            this.lblDashTitle.Font = new System.Drawing.Font("Segoe UI Light", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDashTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(200)))), ((int)(((byte)(175)))));
            this.lblDashTitle.Location = new System.Drawing.Point(14, 18);
            this.lblDashTitle.Name = "lblDashTitle";
            this.lblDashTitle.Size = new System.Drawing.Size(448, 52);
            this.lblDashTitle.TabIndex = 11;
            this.lblDashTitle.Text = "Dashboard";
            // 
            // brd_stath
            // 
            this.brd_stath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(200)))), ((int)(((byte)(175)))));
            this.brd_stath.Controls.Add(this.lblUsageStatsH);
            this.brd_stath.Location = new System.Drawing.Point(23, 102);
            this.brd_stath.Name = "brd_stath";
            this.brd_stath.Padding = new System.Windows.Forms.Padding(1, 1, 1, 0);
            this.brd_stath.Size = new System.Drawing.Size(155, 45);
            this.brd_stath.TabIndex = 10;
            // 
            // lblUsageStatsH
            // 
            this.lblUsageStatsH.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.lblUsageStatsH.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblUsageStatsH.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsageStatsH.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(250)))), ((int)(((byte)(155)))));
            this.lblUsageStatsH.Location = new System.Drawing.Point(1, 1);
            this.lblUsageStatsH.Name = "lblUsageStatsH";
            this.lblUsageStatsH.Padding = new System.Windows.Forms.Padding(15, 10, 0, 0);
            this.lblUsageStatsH.Size = new System.Drawing.Size(153, 44);
            this.lblUsageStatsH.TabIndex = 0;
            this.lblUsageStatsH.Text = "Usage Statistics";
            // 
            // pnlPageDeploy
            // 
            this.pnlPageDeploy.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlPageDeploy.BackColor = System.Drawing.Color.Transparent;
            this.pnlPageDeploy.Controls.Add(this.pnlHParams);
            this.pnlPageDeploy.Controls.Add(this.brd_params);
            this.pnlPageDeploy.Controls.Add(this.btnStartStop);
            this.pnlPageDeploy.Controls.Add(this.lblDeployH);
            this.pnlPageDeploy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(250)))), ((int)(((byte)(155)))));
            this.pnlPageDeploy.Location = new System.Drawing.Point(63, 36);
            this.pnlPageDeploy.Name = "pnlPageDeploy";
            this.pnlPageDeploy.Size = new System.Drawing.Size(627, 402);
            this.pnlPageDeploy.TabIndex = 16;
            // 
            // pnlHParams
            // 
            this.pnlHParams.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(200)))), ((int)(((byte)(175)))));
            this.pnlHParams.Controls.Add(this.lblParams);
            this.pnlHParams.Location = new System.Drawing.Point(23, 102);
            this.pnlHParams.Name = "pnlHParams";
            this.pnlHParams.Padding = new System.Windows.Forms.Padding(1, 1, 1, 0);
            this.pnlHParams.Size = new System.Drawing.Size(135, 45);
            this.pnlHParams.TabIndex = 18;
            // 
            // lblParams
            // 
            this.lblParams.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.lblParams.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblParams.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblParams.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(250)))), ((int)(((byte)(155)))));
            this.lblParams.Location = new System.Drawing.Point(1, 1);
            this.lblParams.Name = "lblParams";
            this.lblParams.Padding = new System.Windows.Forms.Padding(15, 10, 0, 0);
            this.lblParams.Size = new System.Drawing.Size(133, 44);
            this.lblParams.TabIndex = 0;
            this.lblParams.Text = "Parameters";
            // 
            // btnStartStop
            // 
            this.btnStartStop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.btnStartStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartStop.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartStop.Location = new System.Drawing.Point(23, 309);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(113, 38);
            this.btnStartStop.TabIndex = 16;
            this.btnStartStop.Text = "DEPLOY";
            this.btnStartStop.UseVisualStyleBackColor = false;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            // 
            // lblDeployH
            // 
            this.lblDeployH.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDeployH.AutoEllipsis = true;
            this.lblDeployH.Font = new System.Drawing.Font("Segoe UI Light", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDeployH.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(200)))), ((int)(((byte)(175)))));
            this.lblDeployH.Location = new System.Drawing.Point(14, 18);
            this.lblDeployH.Name = "lblDeployH";
            this.lblDeployH.Size = new System.Drawing.Size(578, 52);
            this.lblDeployH.TabIndex = 11;
            this.lblDeployH.Text = "Deployment";
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 10000;
            this.toolTip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(20)))), ((int)(((byte)(30)))));
            this.toolTip.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(200)))), ((int)(((byte)(175)))));
            this.toolTip.InitialDelay = 500;
            this.toolTip.ReshowDelay = 100;
            // 
            // pnlPageCC
            // 
            this.pnlPageCC.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlPageCC.BackColor = System.Drawing.Color.Transparent;
            this.pnlPageCC.Controls.Add(this.brd_ft);
            this.pnlPageCC.Controls.Add(this.brd_stf);
            this.pnlPageCC.Controls.Add(this.lblCCH);
            this.pnlPageCC.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(250)))), ((int)(((byte)(155)))));
            this.pnlPageCC.Location = new System.Drawing.Point(63, 36);
            this.pnlPageCC.Name = "pnlPageCC";
            this.pnlPageCC.Size = new System.Drawing.Size(627, 402);
            this.pnlPageCC.TabIndex = 19;
            // 
            // brd_ft
            // 
            this.brd_ft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(200)))), ((int)(((byte)(175)))));
            this.brd_ft.Controls.Add(this.lblCC);
            this.brd_ft.Location = new System.Drawing.Point(23, 102);
            this.brd_ft.Name = "brd_ft";
            this.brd_ft.Padding = new System.Windows.Forms.Padding(1, 1, 1, 0);
            this.brd_ft.Size = new System.Drawing.Size(113, 45);
            this.brd_ft.TabIndex = 18;
            // 
            // lblCC
            // 
            this.lblCC.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.lblCC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCC.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCC.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(250)))), ((int)(((byte)(155)))));
            this.lblCC.Location = new System.Drawing.Point(1, 1);
            this.lblCC.Name = "lblCC";
            this.lblCC.Padding = new System.Windows.Forms.Padding(15, 10, 0, 0);
            this.lblCC.Size = new System.Drawing.Size(111, 44);
            this.lblCC.TabIndex = 0;
            this.lblCC.Text = "Features";
            // 
            // lblCCH
            // 
            this.lblCCH.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCCH.AutoEllipsis = true;
            this.lblCCH.Font = new System.Drawing.Font("Segoe UI Light", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCCH.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(200)))), ((int)(((byte)(175)))));
            this.lblCCH.Location = new System.Drawing.Point(14, 18);
            this.lblCCH.Name = "lblCCH";
            this.lblCCH.Size = new System.Drawing.Size(578, 52);
            this.lblCCH.TabIndex = 11;
            this.lblCCH.Text = "Control Centre";
            // 
            // pnlDriver
            // 
            this.pnlDriver.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlDriver.BackColor = System.Drawing.Color.Transparent;
            this.pnlDriver.Controls.Add(this.brd_hsvcctrl);
            this.pnlDriver.Controls.Add(this.brd_svcctrl);
            this.pnlDriver.Controls.Add(this.lblDriver);
            this.pnlDriver.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(250)))), ((int)(((byte)(155)))));
            this.pnlDriver.Location = new System.Drawing.Point(63, 36);
            this.pnlDriver.Name = "pnlDriver";
            this.pnlDriver.Size = new System.Drawing.Size(627, 402);
            this.pnlDriver.TabIndex = 20;
            // 
            // brd_hsvcctrl
            // 
            this.brd_hsvcctrl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(200)))), ((int)(((byte)(175)))));
            this.brd_hsvcctrl.Controls.Add(this.lblSvcControl);
            this.brd_hsvcctrl.Location = new System.Drawing.Point(23, 102);
            this.brd_hsvcctrl.Name = "brd_hsvcctrl";
            this.brd_hsvcctrl.Padding = new System.Windows.Forms.Padding(1, 1, 1, 0);
            this.brd_hsvcctrl.Size = new System.Drawing.Size(147, 45);
            this.brd_hsvcctrl.TabIndex = 18;
            // 
            // lblSvcControl
            // 
            this.lblSvcControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.lblSvcControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSvcControl.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSvcControl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(250)))), ((int)(((byte)(155)))));
            this.lblSvcControl.Location = new System.Drawing.Point(1, 1);
            this.lblSvcControl.Name = "lblSvcControl";
            this.lblSvcControl.Padding = new System.Windows.Forms.Padding(15, 10, 0, 0);
            this.lblSvcControl.Size = new System.Drawing.Size(145, 44);
            this.lblSvcControl.TabIndex = 0;
            this.lblSvcControl.Text = "Service Control";
            // 
            // lblDriver
            // 
            this.lblDriver.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDriver.AutoEllipsis = true;
            this.lblDriver.Font = new System.Drawing.Font("Segoe UI Light", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDriver.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(200)))), ((int)(((byte)(175)))));
            this.lblDriver.Location = new System.Drawing.Point(14, 18);
            this.lblDriver.Name = "lblDriver";
            this.lblDriver.Size = new System.Drawing.Size(578, 52);
            this.lblDriver.TabIndex = 11;
            this.lblDriver.Text = "ZeroKore® Service";
            // 
            // brd_params
            // 
            this.brd_params.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.brd_params.Angle = 90;
            this.brd_params.BottomColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(250)))), ((int)(((byte)(155)))));
            this.brd_params.Controls.Add(this.pnlParams);
            this.brd_params.Location = new System.Drawing.Point(23, 146);
            this.brd_params.Name = "brd_params";
            this.brd_params.Padding = new System.Windows.Forms.Padding(1);
            this.brd_params.Size = new System.Drawing.Size(480, 125);
            this.brd_params.TabIndex = 17;
            this.brd_params.TopColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(200)))), ((int)(((byte)(175)))));
            // 
            // pnlParams
            // 
            this.pnlParams.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.pnlParams.Controls.Add(this.lblIGM);
            this.pnlParams.Controls.Add(this.tsZPN);
            this.pnlParams.Controls.Add(this.lblZPN);
            this.pnlParams.Controls.Add(this.tsIGM);
            this.pnlParams.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlParams.Location = new System.Drawing.Point(1, 1);
            this.pnlParams.Name = "pnlParams";
            this.pnlParams.Size = new System.Drawing.Size(478, 123);
            this.pnlParams.TabIndex = 0;
            // 
            // lblIGM
            // 
            this.lblIGM.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIGM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(250)))), ((int)(((byte)(225)))));
            this.lblIGM.Location = new System.Drawing.Point(15, 20);
            this.lblIGM.Name = "lblIGM";
            this.lblIGM.Size = new System.Drawing.Size(144, 28);
            this.lblIGM.TabIndex = 14;
            this.lblIGM.Text = "In-Game Menu (F3)";
            this.lblIGM.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.lblIGM, "Allows you to press F3 (or any other predefined hotkey) to open a menu \r\nthat all" +
        "ows you to configure features of ZeroKore while playing.");
            // 
            // tsZPN
            // 
            this.tsZPN.CheckedBack = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(60)))), ((int)(((byte)(90)))));
            this.tsZPN.CheckedFore = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(250)))), ((int)(((byte)(155)))));
            this.tsZPN.Location = new System.Drawing.Point(165, 58);
            this.tsZPN.Name = "tsZPN";
            this.tsZPN.Padding = new System.Windows.Forms.Padding(6);
            this.tsZPN.Size = new System.Drawing.Size(50, 25);
            this.tsZPN.TabIndex = 12;
            this.tsZPN.UnCheckedBack = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(60)))), ((int)(((byte)(90)))));
            this.tsZPN.UnCheckedFore = System.Drawing.Color.LightGray;
            this.tsZPN.UseVisualStyleBackColor = true;
            this.tsZPN.CheckedChanged += new System.EventHandler(this.tsZPN_CheckedChanged);
            // 
            // lblZPN
            // 
            this.lblZPN.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblZPN.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(250)))), ((int)(((byte)(225)))));
            this.lblZPN.Location = new System.Drawing.Point(15, 55);
            this.lblZPN.Name = "lblZPN";
            this.lblZPN.Size = new System.Drawing.Size(117, 28);
            this.lblZPN.TabIndex = 15;
            this.lblZPN.Text = "ZPN Mode";
            this.lblZPN.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.lblZPN, resources.GetString("lblZPN.ToolTip"));
            // 
            // tsIGM
            // 
            this.tsIGM.Checked = true;
            this.tsIGM.CheckedBack = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(60)))), ((int)(((byte)(90)))));
            this.tsIGM.CheckedFore = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(250)))), ((int)(((byte)(155)))));
            this.tsIGM.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsIGM.Location = new System.Drawing.Point(165, 23);
            this.tsIGM.Name = "tsIGM";
            this.tsIGM.Padding = new System.Windows.Forms.Padding(6);
            this.tsIGM.Size = new System.Drawing.Size(50, 25);
            this.tsIGM.TabIndex = 13;
            this.tsIGM.UnCheckedBack = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(60)))), ((int)(((byte)(90)))));
            this.tsIGM.UnCheckedFore = System.Drawing.Color.LightGray;
            this.tsIGM.UseVisualStyleBackColor = true;
            this.tsIGM.CheckedChanged += new System.EventHandler(this.tsIGM_CheckedChanged);
            // 
            // brd_stat
            // 
            this.brd_stat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.brd_stat.Angle = 90;
            this.brd_stat.BottomColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(250)))), ((int)(((byte)(155)))));
            this.brd_stat.Controls.Add(this.pnlStat);
            this.brd_stat.Location = new System.Drawing.Point(23, 145);
            this.brd_stat.Name = "brd_stat";
            this.brd_stat.Padding = new System.Windows.Forms.Padding(1);
            this.brd_stat.Size = new System.Drawing.Size(554, 145);
            this.brd_stat.TabIndex = 1;
            this.brd_stat.TopColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(200)))), ((int)(((byte)(175)))));
            // 
            // pnlStat
            // 
            this.pnlStat.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.pnlStat.Controls.Add(this.lblMp);
            this.pnlStat.Controls.Add(this.lblTp);
            this.pnlStat.Controls.Add(this.statMins);
            this.pnlStat.Controls.Add(this.statLaunches);
            this.pnlStat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlStat.Location = new System.Drawing.Point(1, 1);
            this.pnlStat.Name = "pnlStat";
            this.pnlStat.Size = new System.Drawing.Size(552, 143);
            this.pnlStat.TabIndex = 0;
            // 
            // lblMp
            // 
            this.lblMp.AutoSize = true;
            this.lblMp.Font = new System.Drawing.Font("Segoe UI Semilight", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(200)))), ((int)(((byte)(175)))));
            this.lblMp.Location = new System.Drawing.Point(301, 92);
            this.lblMp.Name = "lblMp";
            this.lblMp.Size = new System.Drawing.Size(135, 25);
            this.lblMp.TabIndex = 11;
            this.lblMp.Text = "Minutes Played";
            // 
            // lblTp
            // 
            this.lblTp.AutoSize = true;
            this.lblTp.Font = new System.Drawing.Font("Segoe UI Semilight", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(200)))), ((int)(((byte)(175)))));
            this.lblTp.Location = new System.Drawing.Point(80, 92);
            this.lblTp.Name = "lblTp";
            this.lblTp.Size = new System.Drawing.Size(119, 25);
            this.lblTp.TabIndex = 10;
            this.lblTp.Text = "Times Played";
            // 
            // statMins
            // 
            this.statMins.AutoEllipsis = true;
            this.statMins.Font = new System.Drawing.Font("Consolas", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statMins.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(250)))), ((int)(((byte)(225)))));
            this.statMins.Location = new System.Drawing.Point(262, 12);
            this.statMins.Name = "statMins";
            this.statMins.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.statMins.Size = new System.Drawing.Size(200, 80);
            this.statMins.TabIndex = 5;
            this.statMins.Text = "0";
            this.statMins.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // statLaunches
            // 
            this.statLaunches.AutoEllipsis = true;
            this.statLaunches.Font = new System.Drawing.Font("Consolas", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statLaunches.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(250)))), ((int)(((byte)(225)))));
            this.statLaunches.Location = new System.Drawing.Point(32, 12);
            this.statLaunches.Name = "statLaunches";
            this.statLaunches.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.statLaunches.Size = new System.Drawing.Size(200, 80);
            this.statLaunches.TabIndex = 4;
            this.statLaunches.Text = "0";
            this.statLaunches.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // brd_stf
            // 
            this.brd_stf.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.brd_stf.Angle = 90;
            this.brd_stf.BottomColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(250)))), ((int)(((byte)(155)))));
            this.brd_stf.Controls.Add(this.pnlstf);
            this.brd_stf.Location = new System.Drawing.Point(23, 146);
            this.brd_stf.Name = "brd_stf";
            this.brd_stf.Padding = new System.Windows.Forms.Padding(1);
            this.brd_stf.Size = new System.Drawing.Size(569, 223);
            this.brd_stf.TabIndex = 17;
            this.brd_stf.TopColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(200)))), ((int)(((byte)(175)))));
            // 
            // pnlstf
            // 
            this.pnlstf.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.pnlstf.Controls.Add(this.gbColourOptions);
            this.pnlstf.Controls.Add(this.lblFG);
            this.pnlstf.Controls.Add(this.tsFG);
            this.pnlstf.Controls.Add(this.lblER);
            this.pnlstf.Controls.Add(this.tsER);
            this.pnlstf.Controls.Add(this.lblCJ);
            this.pnlstf.Controls.Add(this.tsCJ);
            this.pnlstf.Controls.Add(this.lblAS);
            this.pnlstf.Controls.Add(this.tsAS);
            this.pnlstf.Controls.Add(this.lblVA);
            this.pnlstf.Controls.Add(this.tsVA);
            this.pnlstf.Controls.Add(this.lblAA);
            this.pnlstf.Controls.Add(this.tsAA);
            this.pnlstf.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlstf.Location = new System.Drawing.Point(1, 1);
            this.pnlstf.Name = "pnlstf";
            this.pnlstf.Size = new System.Drawing.Size(567, 221);
            this.pnlstf.TabIndex = 0;
            // 
            // gbColourOptions
            // 
            this.gbColourOptions.BackColor = System.Drawing.Color.Transparent;
            this.gbColourOptions.Controls.Add(this.lnkChangeEnemy);
            this.gbColourOptions.Controls.Add(this.lnkChangeTeam);
            this.gbColourOptions.Controls.Add(this.lblEC);
            this.gbColourOptions.Controls.Add(this.lblTC);
            this.gbColourOptions.Controls.Add(this.pbEnemy);
            this.gbColourOptions.Controls.Add(this.pbTeam);
            this.gbColourOptions.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(250)))), ((int)(((byte)(155)))));
            this.gbColourOptions.Location = new System.Drawing.Point(266, 105);
            this.gbColourOptions.Name = "gbColourOptions";
            this.gbColourOptions.Size = new System.Drawing.Size(274, 85);
            this.gbColourOptions.TabIndex = 25;
            this.gbColourOptions.TabStop = false;
            this.gbColourOptions.Text = "Visual Assistance";
            // 
            // lnkChangeEnemy
            // 
            this.lnkChangeEnemy.ActiveLinkColor = System.Drawing.Color.Red;
            this.lnkChangeEnemy.AutoSize = true;
            this.lnkChangeEnemy.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkChangeEnemy.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(250)))), ((int)(((byte)(155)))));
            this.lnkChangeEnemy.Location = new System.Drawing.Point(127, 48);
            this.lnkChangeEnemy.Name = "lnkChangeEnemy";
            this.lnkChangeEnemy.Size = new System.Drawing.Size(43, 13);
            this.lnkChangeEnemy.TabIndex = 5;
            this.lnkChangeEnemy.TabStop = true;
            this.lnkChangeEnemy.Text = "Change";
            this.lnkChangeEnemy.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(250)))), ((int)(((byte)(155)))));
            this.lnkChangeEnemy.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkChangeEnemy_LinkClicked);
            // 
            // lnkChangeTeam
            // 
            this.lnkChangeTeam.ActiveLinkColor = System.Drawing.Color.Lime;
            this.lnkChangeTeam.AutoSize = true;
            this.lnkChangeTeam.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkChangeTeam.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(250)))), ((int)(((byte)(155)))));
            this.lnkChangeTeam.Location = new System.Drawing.Point(120, 26);
            this.lnkChangeTeam.Name = "lnkChangeTeam";
            this.lnkChangeTeam.Size = new System.Drawing.Size(43, 13);
            this.lnkChangeTeam.TabIndex = 4;
            this.lnkChangeTeam.TabStop = true;
            this.lnkChangeTeam.Text = "Change";
            this.lnkChangeTeam.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(250)))), ((int)(((byte)(155)))));
            this.lnkChangeTeam.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkChangeTeam_LinkClicked);
            // 
            // lblEC
            // 
            this.lblEC.AutoSize = true;
            this.lblEC.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEC.ForeColor = System.Drawing.Color.White;
            this.lblEC.Location = new System.Drawing.Point(41, 48);
            this.lblEC.Name = "lblEC";
            this.lblEC.Size = new System.Drawing.Size(79, 13);
            this.lblEC.TabIndex = 3;
            this.lblEC.Text = "Enemy Colour";
            // 
            // lblTC
            // 
            this.lblTC.AutoSize = true;
            this.lblTC.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTC.ForeColor = System.Drawing.Color.White;
            this.lblTC.Location = new System.Drawing.Point(41, 26);
            this.lblTC.Name = "lblTC";
            this.lblTC.Size = new System.Drawing.Size(73, 13);
            this.lblTC.TabIndex = 2;
            this.lblTC.Text = "Team Colour";
            // 
            // pbEnemy
            // 
            this.pbEnemy.BackColor = System.Drawing.Color.Red;
            this.pbEnemy.Location = new System.Drawing.Point(19, 46);
            this.pbEnemy.Name = "pbEnemy";
            this.pbEnemy.Size = new System.Drawing.Size(16, 16);
            this.pbEnemy.TabIndex = 1;
            this.pbEnemy.TabStop = false;
            // 
            // pbTeam
            // 
            this.pbTeam.BackColor = System.Drawing.Color.Lime;
            this.pbTeam.Location = new System.Drawing.Point(19, 24);
            this.pbTeam.Name = "pbTeam";
            this.pbTeam.Size = new System.Drawing.Size(16, 16);
            this.pbTeam.TabIndex = 0;
            this.pbTeam.TabStop = false;
            // 
            // lblFG
            // 
            this.lblFG.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFG.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(250)))), ((int)(((byte)(225)))));
            this.lblFG.Location = new System.Drawing.Point(262, 20);
            this.lblFG.Name = "lblFG";
            this.lblFG.Size = new System.Drawing.Size(135, 28);
            this.lblFG.TabIndex = 24;
            this.lblFG.Text = "Flash Glasses";
            this.lblFG.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.lblFG, "Grants invulnerability to flash-bang grenades. The player will\r\nnot be affected b" +
        "y the \'white\' screen.");
            // 
            // tsFG
            // 
            this.tsFG.Checked = true;
            this.tsFG.CheckedBack = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(60)))), ((int)(((byte)(90)))));
            this.tsFG.CheckedFore = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(250)))), ((int)(((byte)(155)))));
            this.tsFG.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsFG.Location = new System.Drawing.Point(411, 22);
            this.tsFG.Name = "tsFG";
            this.tsFG.Padding = new System.Windows.Forms.Padding(6);
            this.tsFG.Size = new System.Drawing.Size(50, 25);
            this.tsFG.TabIndex = 23;
            this.tsFG.UnCheckedBack = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(60)))), ((int)(((byte)(90)))));
            this.tsFG.UnCheckedFore = System.Drawing.Color.LightGray;
            this.tsFG.UseVisualStyleBackColor = true;
            this.tsFG.CheckedChanged += new System.EventHandler(this.tsFG_CheckedChanged);
            // 
            // lblER
            // 
            this.lblER.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblER.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(250)))), ((int)(((byte)(225)))));
            this.lblER.Location = new System.Drawing.Point(262, 55);
            this.lblER.Name = "lblER";
            this.lblER.Size = new System.Drawing.Size(135, 28);
            this.lblER.TabIndex = 22;
            this.lblER.Text = "Enhanced Radar";
            this.lblER.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.lblER, "Shows enemy players on the radar.");
            // 
            // tsER
            // 
            this.tsER.Checked = true;
            this.tsER.CheckedBack = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(60)))), ((int)(((byte)(90)))));
            this.tsER.CheckedFore = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(250)))), ((int)(((byte)(155)))));
            this.tsER.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsER.Location = new System.Drawing.Point(411, 57);
            this.tsER.Name = "tsER";
            this.tsER.Padding = new System.Windows.Forms.Padding(6);
            this.tsER.Size = new System.Drawing.Size(50, 25);
            this.tsER.TabIndex = 21;
            this.tsER.UnCheckedBack = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(60)))), ((int)(((byte)(90)))));
            this.tsER.UnCheckedFore = System.Drawing.Color.LightGray;
            this.tsER.UseVisualStyleBackColor = true;
            this.tsER.CheckedChanged += new System.EventHandler(this.tsER_CheckedChanged);
            // 
            // lblCJ
            // 
            this.lblCJ.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCJ.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(250)))), ((int)(((byte)(225)))));
            this.lblCJ.Location = new System.Drawing.Point(15, 125);
            this.lblCJ.Name = "lblCJ";
            this.lblCJ.Size = new System.Drawing.Size(117, 28);
            this.lblCJ.TabIndex = 20;
            this.lblCJ.Text = "Constant Jump";
            this.lblCJ.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.lblCJ, "Allows the player to jump constantly (bunnyhop)\r\nby simply holding the spacebar.");
            // 
            // tsCJ
            // 
            this.tsCJ.Checked = true;
            this.tsCJ.CheckedBack = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(60)))), ((int)(((byte)(90)))));
            this.tsCJ.CheckedFore = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(250)))), ((int)(((byte)(155)))));
            this.tsCJ.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsCJ.Location = new System.Drawing.Point(165, 127);
            this.tsCJ.Name = "tsCJ";
            this.tsCJ.Padding = new System.Windows.Forms.Padding(6);
            this.tsCJ.Size = new System.Drawing.Size(50, 25);
            this.tsCJ.TabIndex = 19;
            this.tsCJ.UnCheckedBack = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(60)))), ((int)(((byte)(90)))));
            this.tsCJ.UnCheckedFore = System.Drawing.Color.LightGray;
            this.tsCJ.UseVisualStyleBackColor = true;
            this.tsCJ.CheckedChanged += new System.EventHandler(this.tsCJ_CheckedChanged);
            // 
            // lblAS
            // 
            this.lblAS.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAS.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(250)))), ((int)(((byte)(225)))));
            this.lblAS.Location = new System.Drawing.Point(15, 90);
            this.lblAS.Name = "lblAS";
            this.lblAS.Size = new System.Drawing.Size(94, 28);
            this.lblAS.TabIndex = 18;
            this.lblAS.Text = "Auto Shoot";
            this.lblAS.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.lblAS, "Detects when a player\'s crosshair is on an enemy\'s\r\nhead and automatically fires " +
        "the selected weapon.");
            // 
            // tsAS
            // 
            this.tsAS.Checked = true;
            this.tsAS.CheckedBack = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(60)))), ((int)(((byte)(90)))));
            this.tsAS.CheckedFore = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(250)))), ((int)(((byte)(155)))));
            this.tsAS.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsAS.Location = new System.Drawing.Point(165, 93);
            this.tsAS.Name = "tsAS";
            this.tsAS.Padding = new System.Windows.Forms.Padding(6);
            this.tsAS.Size = new System.Drawing.Size(50, 25);
            this.tsAS.TabIndex = 17;
            this.tsAS.UnCheckedBack = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(60)))), ((int)(((byte)(90)))));
            this.tsAS.UnCheckedFore = System.Drawing.Color.LightGray;
            this.tsAS.UseVisualStyleBackColor = true;
            this.tsAS.CheckedChanged += new System.EventHandler(this.tsAS_CheckedChanged);
            // 
            // lblVA
            // 
            this.lblVA.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVA.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(250)))), ((int)(((byte)(225)))));
            this.lblVA.Location = new System.Drawing.Point(15, 55);
            this.lblVA.Name = "lblVA";
            this.lblVA.Size = new System.Drawing.Size(136, 28);
            this.lblVA.TabIndex = 16;
            this.lblVA.Text = "Visual Assistance";
            this.lblVA.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.lblVA, "Enhances a player\'s awareness of enemies and threats by\r\nmaking all players \'glow" +
        "\' in-game. This option is customisable.");
            // 
            // tsVA
            // 
            this.tsVA.Checked = true;
            this.tsVA.CheckedBack = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(60)))), ((int)(((byte)(90)))));
            this.tsVA.CheckedFore = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(250)))), ((int)(((byte)(155)))));
            this.tsVA.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsVA.Location = new System.Drawing.Point(165, 58);
            this.tsVA.Name = "tsVA";
            this.tsVA.Padding = new System.Windows.Forms.Padding(6);
            this.tsVA.Size = new System.Drawing.Size(50, 25);
            this.tsVA.TabIndex = 15;
            this.tsVA.UnCheckedBack = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(60)))), ((int)(((byte)(90)))));
            this.tsVA.UnCheckedFore = System.Drawing.Color.LightGray;
            this.tsVA.UseVisualStyleBackColor = true;
            this.tsVA.CheckedChanged += new System.EventHandler(this.tsVA_CheckedChanged);
            // 
            // lblAA
            // 
            this.lblAA.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAA.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(250)))), ((int)(((byte)(225)))));
            this.lblAA.Location = new System.Drawing.Point(15, 20);
            this.lblAA.Name = "lblAA";
            this.lblAA.Size = new System.Drawing.Size(117, 28);
            this.lblAA.TabIndex = 14;
            this.lblAA.Text = "Aim Assistance";
            this.lblAA.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.lblAA, resources.GetString("lblAA.ToolTip"));
            // 
            // tsAA
            // 
            this.tsAA.Checked = true;
            this.tsAA.CheckedBack = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(60)))), ((int)(((byte)(90)))));
            this.tsAA.CheckedFore = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(250)))), ((int)(((byte)(155)))));
            this.tsAA.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsAA.Location = new System.Drawing.Point(165, 23);
            this.tsAA.Name = "tsAA";
            this.tsAA.Padding = new System.Windows.Forms.Padding(6);
            this.tsAA.Size = new System.Drawing.Size(50, 25);
            this.tsAA.TabIndex = 13;
            this.tsAA.UnCheckedBack = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(60)))), ((int)(((byte)(90)))));
            this.tsAA.UnCheckedFore = System.Drawing.Color.LightGray;
            this.tsAA.UseVisualStyleBackColor = true;
            this.tsAA.CheckedChanged += new System.EventHandler(this.tsAA_CheckedChanged);
            // 
            // pnlSideBar
            // 
            this.pnlSideBar.Angle = 90;
            this.pnlSideBar.BottomColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(250)))), ((int)(((byte)(155)))));
            this.pnlSideBar.Controls.Add(this.btnHelp);
            this.pnlSideBar.Controls.Add(this.btnSysDriver);
            this.pnlSideBar.Controls.Add(this.btnControlCentre);
            this.pnlSideBar.Controls.Add(this.btnZeroKore);
            this.pnlSideBar.Controls.Add(this.btnDashboard);
            this.pnlSideBar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSideBar.Location = new System.Drawing.Point(0, 30);
            this.pnlSideBar.Name = "pnlSideBar";
            this.pnlSideBar.Padding = new System.Windows.Forms.Padding(0, 0, 0, 30);
            this.pnlSideBar.Size = new System.Drawing.Size(48, 420);
            this.pnlSideBar.TabIndex = 21;
            this.pnlSideBar.TopColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(200)))), ((int)(((byte)(175)))));
            // 
            // btnHelp
            // 
            this.btnHelp.BackColor = System.Drawing.Color.Transparent;
            this.btnHelp.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnHelp.FlatAppearance.BorderSize = 0;
            this.btnHelp.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.btnHelp.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.btnHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHelp.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHelp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.btnHelp.Image = ((System.Drawing.Image)(resources.GetObject("btnHelp.Image")));
            this.btnHelp.Location = new System.Drawing.Point(0, 346);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(48, 44);
            this.btnHelp.TabIndex = 10;
            this.btnHelp.TabStop = false;
            this.btnHelp.UseVisualStyleBackColor = false;
            // 
            // btnSysDriver
            // 
            this.btnSysDriver.BackColor = System.Drawing.Color.Transparent;
            this.btnSysDriver.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnSysDriver.FlatAppearance.BorderSize = 0;
            this.btnSysDriver.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.btnSysDriver.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.btnSysDriver.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSysDriver.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSysDriver.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.btnSysDriver.Image = ((System.Drawing.Image)(resources.GetObject("btnSysDriver.Image")));
            this.btnSysDriver.Location = new System.Drawing.Point(0, 132);
            this.btnSysDriver.Name = "btnSysDriver";
            this.btnSysDriver.Size = new System.Drawing.Size(48, 44);
            this.btnSysDriver.TabIndex = 9;
            this.btnSysDriver.TabStop = false;
            this.btnSysDriver.UseVisualStyleBackColor = false;
            this.btnSysDriver.Click += new System.EventHandler(this.btnSysDriver_Click);
            // 
            // btnControlCentre
            // 
            this.btnControlCentre.BackColor = System.Drawing.Color.Transparent;
            this.btnControlCentre.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnControlCentre.FlatAppearance.BorderSize = 0;
            this.btnControlCentre.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.btnControlCentre.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.btnControlCentre.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnControlCentre.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnControlCentre.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.btnControlCentre.Image = ((System.Drawing.Image)(resources.GetObject("btnControlCentre.Image")));
            this.btnControlCentre.Location = new System.Drawing.Point(0, 88);
            this.btnControlCentre.Name = "btnControlCentre";
            this.btnControlCentre.Size = new System.Drawing.Size(48, 44);
            this.btnControlCentre.TabIndex = 8;
            this.btnControlCentre.TabStop = false;
            this.btnControlCentre.UseVisualStyleBackColor = false;
            this.btnControlCentre.Click += new System.EventHandler(this.btnControlCentre_Click);
            // 
            // btnZeroKore
            // 
            this.btnZeroKore.BackColor = System.Drawing.Color.Transparent;
            this.btnZeroKore.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnZeroKore.FlatAppearance.BorderSize = 0;
            this.btnZeroKore.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.btnZeroKore.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.btnZeroKore.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnZeroKore.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnZeroKore.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.btnZeroKore.Image = ((System.Drawing.Image)(resources.GetObject("btnZeroKore.Image")));
            this.btnZeroKore.Location = new System.Drawing.Point(0, 44);
            this.btnZeroKore.Name = "btnZeroKore";
            this.btnZeroKore.Size = new System.Drawing.Size(48, 44);
            this.btnZeroKore.TabIndex = 7;
            this.btnZeroKore.TabStop = false;
            this.btnZeroKore.UseVisualStyleBackColor = false;
            this.btnZeroKore.Click += new System.EventHandler(this.btnZeroKore_Click);
            // 
            // btnDashboard
            // 
            this.btnDashboard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.btnDashboard.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnDashboard.FlatAppearance.BorderSize = 0;
            this.btnDashboard.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.btnDashboard.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.btnDashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDashboard.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDashboard.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.btnDashboard.Image = ((System.Drawing.Image)(resources.GetObject("btnDashboard.Image")));
            this.btnDashboard.Location = new System.Drawing.Point(0, 0);
            this.btnDashboard.Name = "btnDashboard";
            this.btnDashboard.Size = new System.Drawing.Size(48, 44);
            this.btnDashboard.TabIndex = 6;
            this.btnDashboard.TabStop = false;
            this.btnDashboard.UseVisualStyleBackColor = false;
            this.btnDashboard.Click += new System.EventHandler(this.btnDashboard_Click);
            // 
            // pnlTitleBar
            // 
            this.pnlTitleBar.Angle = 0;
            this.pnlTitleBar.BottomColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.pnlTitleBar.Controls.Add(this.pnlSpacer);
            this.pnlTitleBar.Controls.Add(this.btnMin);
            this.pnlTitleBar.Controls.Add(this.btnMax);
            this.pnlTitleBar.Controls.Add(this.btnClose);
            this.pnlTitleBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTitleBar.Location = new System.Drawing.Point(0, 0);
            this.pnlTitleBar.Name = "pnlTitleBar";
            this.pnlTitleBar.Size = new System.Drawing.Size(700, 30);
            this.pnlTitleBar.TabIndex = 22;
            this.pnlTitleBar.TopColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(30)))), ((int)(((byte)(45)))));
            this.pnlTitleBar.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pnlTitleBar_MouseClick);
            this.pnlTitleBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlTitleBar_MouseDown);
            // 
            // pnlSpacer
            // 
            this.pnlSpacer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(200)))), ((int)(((byte)(175)))));
            this.pnlSpacer.BackColour = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(200)))), ((int)(((byte)(175)))));
            this.pnlSpacer.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSpacer.Location = new System.Drawing.Point(0, 0);
            this.pnlSpacer.Name = "pnlSpacer";
            this.pnlSpacer.Size = new System.Drawing.Size(48, 30);
            this.pnlSpacer.TabIndex = 1004;
            // 
            // btnMin
            // 
            this.btnMin.BackColor = System.Drawing.Color.Transparent;
            this.btnMin.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnMin.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.btnMin.FlatAppearance.BorderSize = 0;
            this.btnMin.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(75)))), ((int)(((byte)(100)))));
            this.btnMin.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(20)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))));
            this.btnMin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMin.Image = global::ZeroKore.Client.Properties.Resources.minimise_transparent;
            this.btnMin.Location = new System.Drawing.Point(565, 0);
            this.btnMin.Name = "btnMin";
            this.btnMin.Size = new System.Drawing.Size(45, 30);
            this.btnMin.TabIndex = 1003;
            this.btnMin.TabStop = false;
            this.btnMin.UseVisualStyleBackColor = false;
            this.btnMin.Click += new System.EventHandler(this.btnMin_Click);
            // 
            // btnMax
            // 
            this.btnMax.BackColor = System.Drawing.Color.Transparent;
            this.btnMax.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnMax.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.btnMax.FlatAppearance.BorderSize = 0;
            this.btnMax.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(75)))), ((int)(((byte)(100)))));
            this.btnMax.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(20)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))));
            this.btnMax.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMax.Image = global::ZeroKore.Client.Properties.Resources.maximise_transparent;
            this.btnMax.Location = new System.Drawing.Point(610, 0);
            this.btnMax.Name = "btnMax";
            this.btnMax.Size = new System.Drawing.Size(45, 30);
            this.btnMax.TabIndex = 1002;
            this.btnMax.TabStop = false;
            this.btnMax.UseVisualStyleBackColor = false;
            this.btnMax.Click += new System.EventHandler(this.btnMax_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(20)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Image = global::ZeroKore.Client.Properties.Resources.close_transparent;
            this.btnClose.Location = new System.Drawing.Point(655, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(45, 30);
            this.btnClose.TabIndex = 1001;
            this.btnClose.TabStop = false;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // brd_svcctrl
            // 
            this.brd_svcctrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.brd_svcctrl.Angle = 90;
            this.brd_svcctrl.BottomColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(250)))), ((int)(((byte)(155)))));
            this.brd_svcctrl.Controls.Add(this.pnlSvcCtrl);
            this.brd_svcctrl.Location = new System.Drawing.Point(23, 146);
            this.brd_svcctrl.Name = "brd_svcctrl";
            this.brd_svcctrl.Padding = new System.Windows.Forms.Padding(1);
            this.brd_svcctrl.Size = new System.Drawing.Size(505, 170);
            this.brd_svcctrl.TabIndex = 17;
            this.brd_svcctrl.TopColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(200)))), ((int)(((byte)(175)))));
            // 
            // pnlSvcCtrl
            // 
            this.pnlSvcCtrl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.pnlSvcCtrl.Controls.Add(this.lblActionStatus);
            this.pnlSvcCtrl.Controls.Add(this.svcStatus);
            this.pnlSvcCtrl.Controls.Add(this.btnStopSvc);
            this.pnlSvcCtrl.Controls.Add(this.btnStartSvc);
            this.pnlSvcCtrl.Controls.Add(this.btnRmSvc);
            this.pnlSvcCtrl.Controls.Add(this.btnInstSvc);
            this.pnlSvcCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSvcCtrl.Location = new System.Drawing.Point(1, 1);
            this.pnlSvcCtrl.Name = "pnlSvcCtrl";
            this.pnlSvcCtrl.Size = new System.Drawing.Size(503, 168);
            this.pnlSvcCtrl.TabIndex = 0;
            // 
            // lblActionStatus
            // 
            this.lblActionStatus.AutoSize = true;
            this.lblActionStatus.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblActionStatus.Location = new System.Drawing.Point(18, 93);
            this.lblActionStatus.Name = "lblActionStatus";
            this.lblActionStatus.Size = new System.Drawing.Size(37, 13);
            this.lblActionStatus.TabIndex = 20;
            this.lblActionStatus.Text = ". . .";
            // 
            // svcStatus
            // 
            this.svcStatus.BackColour = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(20)))), ((int)(((byte)(30)))));
            this.svcStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(220)))), ((int)(((byte)(200)))));
            this.svcStatus.GlowColour = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(220)))), ((int)(((byte)(200)))));
            this.svcStatus.Location = new System.Drawing.Point(21, 114);
            this.svcStatus.Name = "svcStatus";
            this.svcStatus.Size = new System.Drawing.Size(300, 20);
            this.svcStatus.TabIndex = 19;
            this.svcStatus.Value = 100;
            this.svcStatus.ValueColour = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(200)))), ((int)(((byte)(175)))));
            // 
            // btnStopSvc
            // 
            this.btnStopSvc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStopSvc.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStopSvc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(250)))), ((int)(((byte)(155)))));
            this.btnStopSvc.Location = new System.Drawing.Point(372, 26);
            this.btnStopSvc.Name = "btnStopSvc";
            this.btnStopSvc.Size = new System.Drawing.Size(100, 50);
            this.btnStopSvc.TabIndex = 30;
            this.btnStopSvc.Text = "STOP SERVICE";
            this.btnStopSvc.UseVisualStyleBackColor = true;
            this.btnStopSvc.Click += new System.EventHandler(this.btnStopSvc_Click);
            // 
            // btnStartSvc
            // 
            this.btnStartSvc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartSvc.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartSvc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(250)))), ((int)(((byte)(155)))));
            this.btnStartSvc.Location = new System.Drawing.Point(266, 26);
            this.btnStartSvc.Name = "btnStartSvc";
            this.btnStartSvc.Size = new System.Drawing.Size(100, 50);
            this.btnStartSvc.TabIndex = 29;
            this.btnStartSvc.Text = "START SERVICE";
            this.btnStartSvc.UseVisualStyleBackColor = true;
            this.btnStartSvc.Click += new System.EventHandler(this.btnStartSvc_Click);
            // 
            // btnRmSvc
            // 
            this.btnRmSvc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRmSvc.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRmSvc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(250)))), ((int)(((byte)(155)))));
            this.btnRmSvc.Location = new System.Drawing.Point(127, 26);
            this.btnRmSvc.Name = "btnRmSvc";
            this.btnRmSvc.Size = new System.Drawing.Size(100, 50);
            this.btnRmSvc.TabIndex = 28;
            this.btnRmSvc.Text = "REMOVE SERVICE";
            this.btnRmSvc.UseVisualStyleBackColor = true;
            this.btnRmSvc.Click += new System.EventHandler(this.btnRmSvc_Click);
            // 
            // btnInstSvc
            // 
            this.btnInstSvc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInstSvc.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInstSvc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(250)))), ((int)(((byte)(155)))));
            this.btnInstSvc.Location = new System.Drawing.Point(21, 26);
            this.btnInstSvc.Name = "btnInstSvc";
            this.btnInstSvc.Size = new System.Drawing.Size(100, 50);
            this.btnInstSvc.TabIndex = 27;
            this.btnInstSvc.Text = "INSTALL SERVICE";
            this.btnInstSvc.UseVisualStyleBackColor = true;
            this.btnInstSvc.Click += new System.EventHandler(this.btnInstSvc_Click);
            // 
            // wndMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.ClientSize = new System.Drawing.Size(700, 450);
            this.Controls.Add(this.pnlPageHome);
            this.Controls.Add(this.pnlPageDeploy);
            this.Controls.Add(this.pnlPageCC);
            this.Controls.Add(this.pnlSideBar);
            this.Controls.Add(this.pnlTitleBar);
            this.Controls.Add(this.pnlDriver);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "wndMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ZeroKore®";
            this.Load += new System.EventHandler(this.wndMain_Load);
            this.SizeChanged += new System.EventHandler(this.wndMain_SizeChanged);
            this.pnlPageHome.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbxLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbReady)).EndInit();
            this.brd_stath.ResumeLayout(false);
            this.pnlPageDeploy.ResumeLayout(false);
            this.pnlHParams.ResumeLayout(false);
            this.pnlPageCC.ResumeLayout(false);
            this.brd_ft.ResumeLayout(false);
            this.pnlDriver.ResumeLayout(false);
            this.brd_hsvcctrl.ResumeLayout(false);
            this.brd_params.ResumeLayout(false);
            this.pnlParams.ResumeLayout(false);
            this.brd_stat.ResumeLayout(false);
            this.pnlStat.ResumeLayout(false);
            this.pnlStat.PerformLayout();
            this.brd_stf.ResumeLayout(false);
            this.pnlstf.ResumeLayout(false);
            this.gbColourOptions.ResumeLayout(false);
            this.gbColourOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbEnemy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTeam)).EndInit();
            this.pnlSideBar.ResumeLayout(false);
            this.pnlTitleBar.ResumeLayout(false);
            this.brd_svcctrl.ResumeLayout(false);
            this.pnlSvcCtrl.ResumeLayout(false);
            this.pnlSvcCtrl.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon sysTray;
        private System.Windows.Forms.Timer sysTrayAnimator;
        private Panel pnlPageHome;
        private Label lblUsageStatsH;
        private UX.GradientPanel brd_stat;
        private Panel pnlStat;
        private Label statMins;
        private Label statLaunches;
        private Label lblTp;
        private Label lblMp;
        private Panel brd_stath;
        private Label lblDashTitle;
        private PictureBox pbReady;
        private PictureBox pbxLogo;
        private Label lblProductManufacturer;
        private Label lblProductName;
        private Panel pnlPageDeploy;
        private Label lblDeployH;
        private UX.ToggleSwitch tsZPN;
        private UX.ToggleSwitch tsIGM;
        private Label lblIGM;
        private Label lblZPN;
        private Button btnStartStop;
        private UX.GradientPanel brd_params;
        private Panel pnlParams;
        private Panel pnlHParams;
        private Label lblParams;
        private ToolTip toolTip;
        private Panel pnlPageCC;
        private Panel brd_ft;
        private Label lblCC;
        private UX.GradientPanel brd_stf;
        private Panel pnlstf;
        private Label lblAA;
        private UX.ToggleSwitch tsAA;
        private Label lblCCH;
        private Label lblVA;
        private UX.ToggleSwitch tsVA;
        private Label lblAS;
        private UX.ToggleSwitch tsAS;
        private Label lblCJ;
        private UX.ToggleSwitch tsCJ;
        private Label lblER;
        private UX.ToggleSwitch tsER;
        private Label lblFG;
        private UX.ToggleSwitch tsFG;
        private GroupBox gbColourOptions;
        private PictureBox pbEnemy;
        private PictureBox pbTeam;
        private Label lblEC;
        private Label lblTC;
        private LinkLabel lnkChangeEnemy;
        private LinkLabel lnkChangeTeam;
        private Panel pnlDriver;
        private Panel brd_hsvcctrl;
        private Label lblSvcControl;
        private UX.GradientPanel brd_svcctrl;
        private Panel pnlSvcCtrl;
        private Label lblDriver;
        private UX.SovietskyBar svcStatus;
        private Label lblActionStatus;
        private Button btnStopSvc;
        private Button btnStartSvc;
        private Button btnRmSvc;
        private Button btnInstSvc;
        private UX.GradientPanel pnlSideBar;
        private Button btnHelp;
        private Button btnSysDriver;
        private Button btnControlCentre;
        private Button btnZeroKore;
        private Button btnDashboard;
        private UX.GradientPanel pnlTitleBar;
        private Button btnMin;
        private Button btnMax;
        private Button btnClose;
        private UX.WinOverridePanel pnlSpacer;
    }
}

