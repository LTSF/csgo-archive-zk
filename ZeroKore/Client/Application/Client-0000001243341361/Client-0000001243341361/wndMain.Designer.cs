using Client_0000001243341361.Libs;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;
using WinUtils;

namespace Client_0000001243341361
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


        #region User Experience
        private Color TopColor = Color.FromArgb(0, 200, 175);
        private Color BottomColor = Color.FromArgb(0, 250, 155);
        protected override void OnPaint(PaintEventArgs e)
        {      
            var Angle = 90;

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

        [DllImport("dwmapi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);

        [DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        [DllImport("dwmapi.dll")]
        public static extern int DwmIsCompositionEnabled(ref int pfEnabled);

        private bool m_aeroEnabled;
        private const int CS_DROPSHADOW = 0x00020000;
        private const int WM_NCPAINT = 0x0085;

        private const int cGrip = 16;
        private const int cCaption = 32;
        private const int RESIZE_CODE = 0x84;

        public struct MARGINS
        {
            public int leftWidth;
            public int rightWidth;
            public int topHeight;
            public int bottomHeight;
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

        #region TitleBar Event Handling
        private void pnlTitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            FormUtils.DragWindow(this.Handle);
        }
        private void pnlTitleBar_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                FormUtils.ShowSystemMenu(this.Handle);

            TryCloseSideBar();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Client.Terminate();
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
        #endregion

        #region Windows Form Designer Generated Code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(wndMain));
            this.pnlClient = new System.Windows.Forms.Panel();
            this.pnlPageHome = new System.Windows.Forms.Panel();
            this.btnStartStop = new System.Windows.Forms.Button();
            this.pnlSBrd2 = new System.Windows.Forms.Panel();
            this.lblHActivity = new System.Windows.Forms.Label();
            this.pnlSBrd1 = new Client_0000001243341361.Controls.GradientPanel();
            this.pnlStats = new System.Windows.Forms.Panel();
            this.lblmp = new System.Windows.Forms.Label();
            this.lbltp = new System.Windows.Forms.Label();
            this.lblTimePlayed = new System.Windows.Forms.Label();
            this.lblTimesUsed = new System.Windows.Forms.Label();
            this.lblWelcomeCaption = new System.Windows.Forms.Label();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.pnlZKSvc = new System.Windows.Forms.Panel();
            this.pnlSvBrd1 = new Client_0000001243341361.Controls.GradientPanel();
            this.pnlStatusSvc = new System.Windows.Forms.Panel();
            this.lblAdvSvcStat = new System.Windows.Forms.Label();
            this.pnlSvBrd3 = new System.Windows.Forms.Panel();
            this.lblHMgmt = new System.Windows.Forms.Label();
            this.pnlSvBrd2 = new Client_0000001243341361.Controls.GradientPanel();
            this.pnlMgmtBd = new System.Windows.Forms.Panel();
            this.btnSvcHelp = new System.Windows.Forms.Button();
            this.btnRmSvc = new System.Windows.Forms.Button();
            this.btnInsSvc = new System.Windows.Forms.Button();
            this.btnStopSvc = new System.Windows.Forms.Button();
            this.btnStartSvc = new System.Windows.Forms.Button();
            this.lblHSvc = new System.Windows.Forms.Label();
            this.pnlPageZKSet = new System.Windows.Forms.Panel();
            this.pnlCBrd2 = new System.Windows.Forms.Panel();
            this.lblCfgTitle = new System.Windows.Forms.Label();
            this.pnlCBrd1 = new Client_0000001243341361.Controls.GradientPanel();
            this.pnlCfgBdy = new System.Windows.Forms.Panel();
            this.lblIGMKey = new System.Windows.Forms.Label();
            this.hckIGM = new System.Windows.Forms.CheckBox();
            this.lblHypeTxt = new System.Windows.Forms.Label();
            this.lblComingSoon = new System.Windows.Forms.Label();
            this.lblAA = new System.Windows.Forms.Label();
            this.chkFG = new System.Windows.Forms.CheckBox();
            this.chkER = new System.Windows.Forms.CheckBox();
            this.chkCJ = new System.Windows.Forms.CheckBox();
            this.chkVA = new System.Windows.Forms.CheckBox();
            this.chkAS = new System.Windows.Forms.CheckBox();
            this.chkAA = new System.Windows.Forms.CheckBox();
            this.lblZKTitle = new System.Windows.Forms.Label();
            this.pnlTitleBar = new System.Windows.Forms.Panel();
            this.btnMin = new System.Windows.Forms.Button();
            this.btnMax = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.slider = new System.Windows.Forms.Timer(this.components);
            this.pnlSideBar = new Client_0000001243341361.Controls.GradientPanel();
            this.btnCloseMenu = new System.Windows.Forms.Button();
            this.btnDiscord = new System.Windows.Forms.Button();
            this.btnDriver = new System.Windows.Forms.Button();
            this.btnZK = new System.Windows.Forms.Button();
            this.btnHome = new System.Windows.Forms.Button();
            this.pnlClient.SuspendLayout();
            this.pnlPageHome.SuspendLayout();
            this.pnlSBrd2.SuspendLayout();
            this.pnlSBrd1.SuspendLayout();
            this.pnlStats.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            this.pnlZKSvc.SuspendLayout();
            this.pnlSvBrd1.SuspendLayout();
            this.pnlStatusSvc.SuspendLayout();
            this.pnlSvBrd3.SuspendLayout();
            this.pnlSvBrd2.SuspendLayout();
            this.pnlMgmtBd.SuspendLayout();
            this.pnlPageZKSet.SuspendLayout();
            this.pnlCBrd2.SuspendLayout();
            this.pnlCBrd1.SuspendLayout();
            this.pnlCfgBdy.SuspendLayout();
            this.pnlTitleBar.SuspendLayout();
            this.pnlSideBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlClient
            // 
            this.pnlClient.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.pnlClient.Controls.Add(this.pnlPageHome);
            this.pnlClient.Controls.Add(this.pnlPageZKSet);
            this.pnlClient.Controls.Add(this.pnlZKSvc);
            this.pnlClient.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlClient.Location = new System.Drawing.Point(1, 1);
            this.pnlClient.Name = "pnlClient";
            this.pnlClient.Size = new System.Drawing.Size(748, 498);
            this.pnlClient.TabIndex = 0;
            // 
            // pnlPageHome
            // 
            this.pnlPageHome.Controls.Add(this.btnStartStop);
            this.pnlPageHome.Controls.Add(this.pnlSBrd2);
            this.pnlPageHome.Controls.Add(this.pnlSBrd1);
            this.pnlPageHome.Controls.Add(this.lblWelcomeCaption);
            this.pnlPageHome.Controls.Add(this.lblWelcome);
            this.pnlPageHome.Controls.Add(this.pbLogo);
            this.pnlPageHome.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPageHome.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlPageHome.Location = new System.Drawing.Point(0, 0);
            this.pnlPageHome.Name = "pnlPageHome";
            this.pnlPageHome.Size = new System.Drawing.Size(748, 498);
            this.pnlPageHome.TabIndex = 3;
            // 
            // btnStartStop
            // 
            this.btnStartStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartStop.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartStop.Location = new System.Drawing.Point(214, 168);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(100, 30);
            this.btnStartStop.TabIndex = 8;
            this.btnStartStop.Text = "DEPLOY";
            this.btnStartStop.UseVisualStyleBackColor = true;
            this.btnStartStop.Visible = false;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            // 
            // pnlSBrd2
            // 
            this.pnlSBrd2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(200)))), ((int)(((byte)(175)))));
            this.pnlSBrd2.Controls.Add(this.lblHActivity);
            this.pnlSBrd2.Location = new System.Drawing.Point(60, 241);
            this.pnlSBrd2.Name = "pnlSBrd2";
            this.pnlSBrd2.Padding = new System.Windows.Forms.Padding(1, 1, 1, 0);
            this.pnlSBrd2.Size = new System.Drawing.Size(86, 40);
            this.pnlSBrd2.TabIndex = 6;
            // 
            // lblHActivity
            // 
            this.lblHActivity.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(20)))), ((int)(((byte)(30)))));
            this.lblHActivity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblHActivity.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHActivity.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(250)))), ((int)(((byte)(225)))));
            this.lblHActivity.Location = new System.Drawing.Point(1, 1);
            this.lblHActivity.Name = "lblHActivity";
            this.lblHActivity.Padding = new System.Windows.Forms.Padding(10, 8, 0, 0);
            this.lblHActivity.Size = new System.Drawing.Size(84, 39);
            this.lblHActivity.TabIndex = 4;
            this.lblHActivity.Text = "Activity";
            // 
            // pnlSBrd1
            // 
            this.pnlSBrd1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSBrd1.BottomColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(250)))), ((int)(((byte)(155)))));
            this.pnlSBrd1.Controls.Add(this.pnlStats);
            this.pnlSBrd1.Location = new System.Drawing.Point(60, 280);
            this.pnlSBrd1.Name = "pnlSBrd1";
            this.pnlSBrd1.Padding = new System.Windows.Forms.Padding(1);
            this.pnlSBrd1.Size = new System.Drawing.Size(630, 140);
            this.pnlSBrd1.TabIndex = 7;
            this.pnlSBrd1.TopColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(200)))), ((int)(((byte)(175)))));
            // 
            // pnlStats
            // 
            this.pnlStats.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(20)))), ((int)(((byte)(30)))));
            this.pnlStats.Controls.Add(this.lblmp);
            this.pnlStats.Controls.Add(this.lbltp);
            this.pnlStats.Controls.Add(this.lblTimePlayed);
            this.pnlStats.Controls.Add(this.lblTimesUsed);
            this.pnlStats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlStats.Location = new System.Drawing.Point(1, 1);
            this.pnlStats.Name = "pnlStats";
            this.pnlStats.Size = new System.Drawing.Size(628, 138);
            this.pnlStats.TabIndex = 4;
            // 
            // lblmp
            // 
            this.lblmp.AutoSize = true;
            this.lblmp.Font = new System.Drawing.Font("Segoe UI Semilight", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblmp.Location = new System.Drawing.Point(357, 92);
            this.lblmp.Name = "lblmp";
            this.lblmp.Size = new System.Drawing.Size(135, 25);
            this.lblmp.TabIndex = 10;
            this.lblmp.Text = "Minutes Played";
            // 
            // lbltp
            // 
            this.lbltp.AutoSize = true;
            this.lbltp.Font = new System.Drawing.Font("Segoe UI Semilight", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbltp.Location = new System.Drawing.Point(80, 92);
            this.lbltp.Name = "lbltp";
            this.lbltp.Size = new System.Drawing.Size(119, 25);
            this.lbltp.TabIndex = 9;
            this.lbltp.Text = "Times Played";
            // 
            // lblTimePlayed
            // 
            this.lblTimePlayed.AutoEllipsis = true;
            this.lblTimePlayed.Font = new System.Drawing.Font("Segoe UI", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimePlayed.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(250)))), ((int)(((byte)(225)))));
            this.lblTimePlayed.Location = new System.Drawing.Point(295, 7);
            this.lblTimePlayed.Name = "lblTimePlayed";
            this.lblTimePlayed.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.lblTimePlayed.Size = new System.Drawing.Size(250, 85);
            this.lblTimePlayed.TabIndex = 3;
            this.lblTimePlayed.Text = "0";
            this.lblTimePlayed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTimesUsed
            // 
            this.lblTimesUsed.AutoEllipsis = true;
            this.lblTimesUsed.Font = new System.Drawing.Font("Segoe UI", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimesUsed.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(250)))), ((int)(((byte)(225)))));
            this.lblTimesUsed.Location = new System.Drawing.Point(10, 7);
            this.lblTimesUsed.Name = "lblTimesUsed";
            this.lblTimesUsed.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.lblTimesUsed.Size = new System.Drawing.Size(250, 85);
            this.lblTimesUsed.TabIndex = 2;
            this.lblTimesUsed.Text = "0";
            this.lblTimesUsed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblWelcomeCaption
            // 
            this.lblWelcomeCaption.AutoSize = true;
            this.lblWelcomeCaption.Font = new System.Drawing.Font("Segoe UI Semilight", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWelcomeCaption.Location = new System.Drawing.Point(210, 120);
            this.lblWelcomeCaption.Name = "lblWelcomeCaption";
            this.lblWelcomeCaption.Size = new System.Drawing.Size(110, 25);
            this.lblWelcomeCaption.TabIndex = 2;
            this.lblWelcomeCaption.Text = "ZeroKore® ";
            // 
            // lblWelcome
            // 
            this.lblWelcome.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblWelcome.AutoEllipsis = true;
            this.lblWelcome.Font = new System.Drawing.Font("Segoe UI Light", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWelcome.Location = new System.Drawing.Point(205, 70);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(527, 50);
            this.lblWelcome.TabIndex = 1;
            this.lblWelcome.Text = "Welcome back!";
            // 
            // pbLogo
            // 
            this.pbLogo.Image = ((System.Drawing.Image)(resources.GetObject("pbLogo.Image")));
            this.pbLogo.Location = new System.Drawing.Point(60, 70);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(128, 128);
            this.pbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbLogo.TabIndex = 0;
            this.pbLogo.TabStop = false;
            // 
            // pnlZKSvc
            // 
            this.pnlZKSvc.Controls.Add(this.pnlSvBrd1);
            this.pnlZKSvc.Controls.Add(this.pnlSvBrd3);
            this.pnlZKSvc.Controls.Add(this.pnlSvBrd2);
            this.pnlZKSvc.Controls.Add(this.lblHSvc);
            this.pnlZKSvc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlZKSvc.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlZKSvc.Location = new System.Drawing.Point(0, 0);
            this.pnlZKSvc.Name = "pnlZKSvc";
            this.pnlZKSvc.Size = new System.Drawing.Size(748, 498);
            this.pnlZKSvc.TabIndex = 10;
            // 
            // pnlSvBrd1
            // 
            this.pnlSvBrd1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSvBrd1.BottomColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(250)))), ((int)(((byte)(155)))));
            this.pnlSvBrd1.Controls.Add(this.pnlStatusSvc);
            this.pnlSvBrd1.Location = new System.Drawing.Point(383, 68);
            this.pnlSvBrd1.Name = "pnlSvBrd1";
            this.pnlSvBrd1.Padding = new System.Windows.Forms.Padding(1);
            this.pnlSvBrd1.Size = new System.Drawing.Size(307, 50);
            this.pnlSvBrd1.TabIndex = 8;
            this.pnlSvBrd1.TopColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(200)))), ((int)(((byte)(175)))));
            // 
            // pnlStatusSvc
            // 
            this.pnlStatusSvc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(20)))), ((int)(((byte)(30)))));
            this.pnlStatusSvc.Controls.Add(this.lblAdvSvcStat);
            this.pnlStatusSvc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlStatusSvc.Location = new System.Drawing.Point(1, 1);
            this.pnlStatusSvc.Name = "pnlStatusSvc";
            this.pnlStatusSvc.Size = new System.Drawing.Size(305, 48);
            this.pnlStatusSvc.TabIndex = 4;
            // 
            // lblAdvSvcStat
            // 
            this.lblAdvSvcStat.AutoEllipsis = true;
            this.lblAdvSvcStat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAdvSvcStat.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAdvSvcStat.Location = new System.Drawing.Point(0, 0);
            this.lblAdvSvcStat.Name = "lblAdvSvcStat";
            this.lblAdvSvcStat.Size = new System.Drawing.Size(305, 48);
            this.lblAdvSvcStat.TabIndex = 0;
            this.lblAdvSvcStat.Text = ". . .";
            this.lblAdvSvcStat.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlSvBrd3
            // 
            this.pnlSvBrd3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(200)))), ((int)(((byte)(175)))));
            this.pnlSvBrd3.Controls.Add(this.lblHMgmt);
            this.pnlSvBrd3.Location = new System.Drawing.Point(59, 142);
            this.pnlSvBrd3.Name = "pnlSvBrd3";
            this.pnlSvBrd3.Padding = new System.Windows.Forms.Padding(1, 1, 1, 0);
            this.pnlSvBrd3.Size = new System.Drawing.Size(129, 40);
            this.pnlSvBrd3.TabIndex = 6;
            // 
            // lblHMgmt
            // 
            this.lblHMgmt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(20)))), ((int)(((byte)(30)))));
            this.lblHMgmt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblHMgmt.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHMgmt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(250)))), ((int)(((byte)(225)))));
            this.lblHMgmt.Location = new System.Drawing.Point(1, 1);
            this.lblHMgmt.Name = "lblHMgmt";
            this.lblHMgmt.Padding = new System.Windows.Forms.Padding(10, 8, 0, 0);
            this.lblHMgmt.Size = new System.Drawing.Size(127, 39);
            this.lblHMgmt.TabIndex = 4;
            this.lblHMgmt.Text = "Management";
            // 
            // pnlSvBrd2
            // 
            this.pnlSvBrd2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSvBrd2.BottomColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(250)))), ((int)(((byte)(155)))));
            this.pnlSvBrd2.Controls.Add(this.pnlMgmtBd);
            this.pnlSvBrd2.Location = new System.Drawing.Point(59, 181);
            this.pnlSvBrd2.Name = "pnlSvBrd2";
            this.pnlSvBrd2.Padding = new System.Windows.Forms.Padding(1);
            this.pnlSvBrd2.Size = new System.Drawing.Size(630, 102);
            this.pnlSvBrd2.TabIndex = 7;
            this.pnlSvBrd2.TopColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(200)))), ((int)(((byte)(175)))));
            // 
            // pnlMgmtBd
            // 
            this.pnlMgmtBd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(20)))), ((int)(((byte)(30)))));
            this.pnlMgmtBd.Controls.Add(this.btnSvcHelp);
            this.pnlMgmtBd.Controls.Add(this.btnRmSvc);
            this.pnlMgmtBd.Controls.Add(this.btnInsSvc);
            this.pnlMgmtBd.Controls.Add(this.btnStopSvc);
            this.pnlMgmtBd.Controls.Add(this.btnStartSvc);
            this.pnlMgmtBd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMgmtBd.Location = new System.Drawing.Point(1, 1);
            this.pnlMgmtBd.Name = "pnlMgmtBd";
            this.pnlMgmtBd.Size = new System.Drawing.Size(628, 100);
            this.pnlMgmtBd.TabIndex = 4;
            // 
            // btnSvcHelp
            // 
            this.btnSvcHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSvcHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSvcHelp.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSvcHelp.Location = new System.Drawing.Point(531, 22);
            this.btnSvcHelp.Name = "btnSvcHelp";
            this.btnSvcHelp.Size = new System.Drawing.Size(79, 45);
            this.btnSvcHelp.TabIndex = 13;
            this.btnSvcHelp.Text = "HELP";
            this.btnSvcHelp.UseVisualStyleBackColor = true;
            // 
            // btnRmSvc
            // 
            this.btnRmSvc.Enabled = false;
            this.btnRmSvc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRmSvc.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRmSvc.Location = new System.Drawing.Point(354, 22);
            this.btnRmSvc.Name = "btnRmSvc";
            this.btnRmSvc.Size = new System.Drawing.Size(100, 45);
            this.btnRmSvc.TabIndex = 12;
            this.btnRmSvc.Text = "REMOVE SERVICE";
            this.btnRmSvc.UseVisualStyleBackColor = true;
            this.btnRmSvc.Click += new System.EventHandler(this.btnRmSvc_Click);
            // 
            // btnInsSvc
            // 
            this.btnInsSvc.Enabled = false;
            this.btnInsSvc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInsSvc.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInsSvc.Location = new System.Drawing.Point(248, 22);
            this.btnInsSvc.Name = "btnInsSvc";
            this.btnInsSvc.Size = new System.Drawing.Size(100, 45);
            this.btnInsSvc.TabIndex = 11;
            this.btnInsSvc.Text = "INSTALL SERVICE";
            this.btnInsSvc.UseVisualStyleBackColor = true;
            this.btnInsSvc.Click += new System.EventHandler(this.btnInsSvc_Click);
            // 
            // btnStopSvc
            // 
            this.btnStopSvc.Enabled = false;
            this.btnStopSvc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStopSvc.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStopSvc.Location = new System.Drawing.Point(123, 22);
            this.btnStopSvc.Name = "btnStopSvc";
            this.btnStopSvc.Size = new System.Drawing.Size(100, 45);
            this.btnStopSvc.TabIndex = 10;
            this.btnStopSvc.Text = "STOP SERVICE";
            this.btnStopSvc.UseVisualStyleBackColor = true;
            this.btnStopSvc.Click += new System.EventHandler(this.btnStopSvc_Click);
            // 
            // btnStartSvc
            // 
            this.btnStartSvc.Enabled = false;
            this.btnStartSvc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartSvc.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartSvc.Location = new System.Drawing.Point(17, 22);
            this.btnStartSvc.Name = "btnStartSvc";
            this.btnStartSvc.Size = new System.Drawing.Size(100, 45);
            this.btnStartSvc.TabIndex = 9;
            this.btnStartSvc.Text = "START SERVICE";
            this.btnStartSvc.UseVisualStyleBackColor = true;
            this.btnStartSvc.Click += new System.EventHandler(this.btnStartSvc_Click);
            // 
            // lblHSvc
            // 
            this.lblHSvc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblHSvc.AutoEllipsis = true;
            this.lblHSvc.Font = new System.Drawing.Font("Segoe UI Light", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHSvc.Location = new System.Drawing.Point(50, 60);
            this.lblHSvc.Name = "lblHSvc";
            this.lblHSvc.Size = new System.Drawing.Size(327, 50);
            this.lblHSvc.TabIndex = 1;
            this.lblHSvc.Text = "ZK® Service";
            // 
            // pnlPageZKSet
            // 
            this.pnlPageZKSet.Controls.Add(this.pnlCBrd2);
            this.pnlPageZKSet.Controls.Add(this.pnlCBrd1);
            this.pnlPageZKSet.Controls.Add(this.lblZKTitle);
            this.pnlPageZKSet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPageZKSet.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlPageZKSet.Location = new System.Drawing.Point(0, 0);
            this.pnlPageZKSet.Name = "pnlPageZKSet";
            this.pnlPageZKSet.Size = new System.Drawing.Size(748, 498);
            this.pnlPageZKSet.TabIndex = 8;
            // 
            // pnlCBrd2
            // 
            this.pnlCBrd2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(200)))), ((int)(((byte)(175)))));
            this.pnlCBrd2.Controls.Add(this.lblCfgTitle);
            this.pnlCBrd2.Location = new System.Drawing.Point(59, 142);
            this.pnlCBrd2.Name = "pnlCBrd2";
            this.pnlCBrd2.Padding = new System.Windows.Forms.Padding(1, 1, 1, 0);
            this.pnlCBrd2.Size = new System.Drawing.Size(100, 40);
            this.pnlCBrd2.TabIndex = 6;
            // 
            // lblCfgTitle
            // 
            this.lblCfgTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(20)))), ((int)(((byte)(30)))));
            this.lblCfgTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCfgTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCfgTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(250)))), ((int)(((byte)(225)))));
            this.lblCfgTitle.Location = new System.Drawing.Point(1, 1);
            this.lblCfgTitle.Name = "lblCfgTitle";
            this.lblCfgTitle.Padding = new System.Windows.Forms.Padding(10, 8, 0, 0);
            this.lblCfgTitle.Size = new System.Drawing.Size(98, 39);
            this.lblCfgTitle.TabIndex = 4;
            this.lblCfgTitle.Text = "Features";
            // 
            // pnlCBrd1
            // 
            this.pnlCBrd1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlCBrd1.BottomColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(250)))), ((int)(((byte)(155)))));
            this.pnlCBrd1.Controls.Add(this.pnlCfgBdy);
            this.pnlCBrd1.Location = new System.Drawing.Point(59, 181);
            this.pnlCBrd1.Name = "pnlCBrd1";
            this.pnlCBrd1.Padding = new System.Windows.Forms.Padding(1);
            this.pnlCBrd1.Size = new System.Drawing.Size(630, 273);
            this.pnlCBrd1.TabIndex = 7;
            this.pnlCBrd1.TopColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(200)))), ((int)(((byte)(175)))));
            // 
            // pnlCfgBdy
            // 
            this.pnlCfgBdy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(20)))), ((int)(((byte)(30)))));
            this.pnlCfgBdy.Controls.Add(this.lblIGMKey);
            this.pnlCfgBdy.Controls.Add(this.hckIGM);
            this.pnlCfgBdy.Controls.Add(this.lblHypeTxt);
            this.pnlCfgBdy.Controls.Add(this.lblComingSoon);
            this.pnlCfgBdy.Controls.Add(this.lblAA);
            this.pnlCfgBdy.Controls.Add(this.chkFG);
            this.pnlCfgBdy.Controls.Add(this.chkER);
            this.pnlCfgBdy.Controls.Add(this.chkCJ);
            this.pnlCfgBdy.Controls.Add(this.chkVA);
            this.pnlCfgBdy.Controls.Add(this.chkAS);
            this.pnlCfgBdy.Controls.Add(this.chkAA);
            this.pnlCfgBdy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCfgBdy.Location = new System.Drawing.Point(1, 1);
            this.pnlCfgBdy.Name = "pnlCfgBdy";
            this.pnlCfgBdy.Size = new System.Drawing.Size(628, 271);
            this.pnlCfgBdy.TabIndex = 4;
            // 
            // lblIGMKey
            // 
            this.lblIGMKey.AutoSize = true;
            this.lblIGMKey.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIGMKey.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(250)))), ((int)(((byte)(225)))));
            this.lblIGMKey.Location = new System.Drawing.Point(515, 104);
            this.lblIGMKey.Name = "lblIGMKey";
            this.lblIGMKey.Size = new System.Drawing.Size(64, 18);
            this.lblIGMKey.TabIndex = 10;
            this.lblIGMKey.Text = "Key: F3";
            // 
            // hckIGM
            // 
            this.hckIGM.AutoSize = true;
            this.hckIGM.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hckIGM.Location = new System.Drawing.Point(310, 102);
            this.hckIGM.Name = "hckIGM";
            this.hckIGM.Size = new System.Drawing.Size(199, 23);
            this.hckIGM.TabIndex = 9;
            this.hckIGM.Text = "Enable In-Game Menu";
            this.hckIGM.UseVisualStyleBackColor = true;
            this.hckIGM.CheckedChanged += new System.EventHandler(this.hckIGM_CheckedChanged);
            // 
            // lblHypeTxt
            // 
            this.lblHypeTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblHypeTxt.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHypeTxt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(250)))), ((int)(((byte)(225)))));
            this.lblHypeTxt.Location = new System.Drawing.Point(307, 53);
            this.lblHypeTxt.Name = "lblHypeTxt";
            this.lblHypeTxt.Size = new System.Drawing.Size(291, 42);
            this.lblHypeTxt.TabIndex = 8;
            this.lblHypeTxt.Text = "Stay tuned for the future updates. They are coming!";
            // 
            // lblComingSoon
            // 
            this.lblComingSoon.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblComingSoon.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblComingSoon.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(250)))), ((int)(((byte)(225)))));
            this.lblComingSoon.Location = new System.Drawing.Point(307, 24);
            this.lblComingSoon.Name = "lblComingSoon";
            this.lblComingSoon.Size = new System.Drawing.Size(291, 29);
            this.lblComingSoon.TabIndex = 7;
            this.lblComingSoon.Text = "Customisation Coming Soon!";
            // 
            // lblAA
            // 
            this.lblAA.AutoSize = true;
            this.lblAA.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAA.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(250)))), ((int)(((byte)(225)))));
            this.lblAA.Location = new System.Drawing.Point(151, 32);
            this.lblAA.Name = "lblAA";
            this.lblAA.Size = new System.Drawing.Size(72, 18);
            this.lblAA.TabIndex = 6;
            this.lblAA.Text = "Key: ALT";
            // 
            // chkFG
            // 
            this.chkFG.AutoSize = true;
            this.chkFG.Checked = true;
            this.chkFG.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFG.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkFG.Location = new System.Drawing.Point(31, 176);
            this.chkFG.Name = "chkFG";
            this.chkFG.Size = new System.Drawing.Size(145, 23);
            this.chkFG.TabIndex = 5;
            this.chkFG.Text = "Flash Glasses";
            this.chkFG.UseVisualStyleBackColor = true;
            this.chkFG.CheckedChanged += new System.EventHandler(this.chkFG_CheckedChanged);
            // 
            // chkER
            // 
            this.chkER.AutoSize = true;
            this.chkER.Checked = true;
            this.chkER.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkER.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkER.Location = new System.Drawing.Point(31, 147);
            this.chkER.Name = "chkER";
            this.chkER.Size = new System.Drawing.Size(154, 23);
            this.chkER.TabIndex = 4;
            this.chkER.Text = "Enhanced Radar";
            this.chkER.UseVisualStyleBackColor = true;
            this.chkER.CheckedChanged += new System.EventHandler(this.chkER_CheckedChanged);
            // 
            // chkCJ
            // 
            this.chkCJ.AutoSize = true;
            this.chkCJ.Checked = true;
            this.chkCJ.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCJ.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCJ.Location = new System.Drawing.Point(31, 117);
            this.chkCJ.Name = "chkCJ";
            this.chkCJ.Size = new System.Drawing.Size(145, 23);
            this.chkCJ.TabIndex = 3;
            this.chkCJ.Text = "Constant Jump";
            this.chkCJ.UseVisualStyleBackColor = true;
            this.chkCJ.CheckedChanged += new System.EventHandler(this.chkCJ_CheckedChanged);
            // 
            // chkVA
            // 
            this.chkVA.AutoSize = true;
            this.chkVA.Checked = true;
            this.chkVA.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkVA.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkVA.Location = new System.Drawing.Point(31, 88);
            this.chkVA.Name = "chkVA";
            this.chkVA.Size = new System.Drawing.Size(181, 23);
            this.chkVA.TabIndex = 2;
            this.chkVA.Text = "Visual Assistance";
            this.chkVA.UseVisualStyleBackColor = true;
            this.chkVA.CheckedChanged += new System.EventHandler(this.chkVA_CheckedChanged);
            // 
            // chkAS
            // 
            this.chkAS.AutoSize = true;
            this.chkAS.Checked = true;
            this.chkAS.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAS.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAS.Location = new System.Drawing.Point(31, 59);
            this.chkAS.Name = "chkAS";
            this.chkAS.Size = new System.Drawing.Size(118, 23);
            this.chkAS.TabIndex = 1;
            this.chkAS.Text = "Auto Shoot";
            this.chkAS.UseVisualStyleBackColor = true;
            this.chkAS.CheckedChanged += new System.EventHandler(this.chkAS_CheckedChanged);
            // 
            // chkAA
            // 
            this.chkAA.AutoSize = true;
            this.chkAA.Checked = true;
            this.chkAA.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAA.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAA.Location = new System.Drawing.Point(31, 30);
            this.chkAA.Name = "chkAA";
            this.chkAA.Size = new System.Drawing.Size(118, 23);
            this.chkAA.TabIndex = 0;
            this.chkAA.Text = "Aim Assist";
            this.chkAA.UseVisualStyleBackColor = true;
            this.chkAA.CheckedChanged += new System.EventHandler(this.chkAA_CheckedChanged);
            // 
            // lblZKTitle
            // 
            this.lblZKTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblZKTitle.AutoEllipsis = true;
            this.lblZKTitle.Font = new System.Drawing.Font("Segoe UI Light", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblZKTitle.Location = new System.Drawing.Point(50, 60);
            this.lblZKTitle.Name = "lblZKTitle";
            this.lblZKTitle.Size = new System.Drawing.Size(527, 50);
            this.lblZKTitle.TabIndex = 1;
            this.lblZKTitle.Text = "Settings";
            // 
            // pnlTitleBar
            // 
            this.pnlTitleBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.pnlTitleBar.Controls.Add(this.btnMin);
            this.pnlTitleBar.Controls.Add(this.btnMax);
            this.pnlTitleBar.Controls.Add(this.btnClose);
            this.pnlTitleBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTitleBar.Location = new System.Drawing.Point(31, 1);
            this.pnlTitleBar.Name = "pnlTitleBar";
            this.pnlTitleBar.Size = new System.Drawing.Size(718, 30);
            this.pnlTitleBar.TabIndex = 1;
            this.pnlTitleBar.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pnlTitleBar_MouseClick);
            this.pnlTitleBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlTitleBar_MouseDown);
            // 
            // btnMin
            // 
            this.btnMin.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnMin.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.btnMin.FlatAppearance.BorderSize = 0;
            this.btnMin.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(75)))), ((int)(((byte)(100)))));
            this.btnMin.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))));
            this.btnMin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMin.Image = global::Client_0000001243341361.Properties.Resources.minimise_transparent;
            this.btnMin.Location = new System.Drawing.Point(583, 0);
            this.btnMin.Name = "btnMin";
            this.btnMin.Size = new System.Drawing.Size(45, 30);
            this.btnMin.TabIndex = 999;
            this.btnMin.TabStop = false;
            this.btnMin.UseVisualStyleBackColor = true;
            this.btnMin.Click += new System.EventHandler(this.btnMin_Click);
            // 
            // btnMax
            // 
            this.btnMax.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnMax.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.btnMax.FlatAppearance.BorderSize = 0;
            this.btnMax.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(75)))), ((int)(((byte)(100)))));
            this.btnMax.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))));
            this.btnMax.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMax.Image = global::Client_0000001243341361.Properties.Resources.maximise_transparent;
            this.btnMax.Location = new System.Drawing.Point(628, 0);
            this.btnMax.Name = "btnMax";
            this.btnMax.Size = new System.Drawing.Size(45, 30);
            this.btnMax.TabIndex = 998;
            this.btnMax.TabStop = false;
            this.btnMax.UseVisualStyleBackColor = true;
            this.btnMax.Click += new System.EventHandler(this.btnMax_Click);
            // 
            // btnClose
            // 
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Image = global::Client_0000001243341361.Properties.Resources.close_transparent;
            this.btnClose.Location = new System.Drawing.Point(673, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(45, 30);
            this.btnClose.TabIndex = 997;
            this.btnClose.TabStop = false;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // slider
            // 
            this.slider.Interval = 5;
            this.slider.Tick += new System.EventHandler(this.slider_Tick);
            // 
            // pnlSideBar
            // 
            this.pnlSideBar.BottomColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(250)))), ((int)(((byte)(155)))));
            this.pnlSideBar.Controls.Add(this.btnCloseMenu);
            this.pnlSideBar.Controls.Add(this.btnDiscord);
            this.pnlSideBar.Controls.Add(this.btnDriver);
            this.pnlSideBar.Controls.Add(this.btnZK);
            this.pnlSideBar.Controls.Add(this.btnHome);
            this.pnlSideBar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSideBar.Location = new System.Drawing.Point(1, 1);
            this.pnlSideBar.Name = "pnlSideBar";
            this.pnlSideBar.Padding = new System.Windows.Forms.Padding(0, 30, 1, 10);
            this.pnlSideBar.Size = new System.Drawing.Size(30, 498);
            this.pnlSideBar.TabIndex = 2;
            this.pnlSideBar.TopColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(200)))), ((int)(((byte)(175)))));
            this.pnlSideBar.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pnlSideBar_MouseClick);
            this.pnlSideBar.MouseHover += new System.EventHandler(this.pnlSideBar_MouseHover);
            // 
            // btnCloseMenu
            // 
            this.btnCloseMenu.BackColor = System.Drawing.Color.Transparent;
            this.btnCloseMenu.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnCloseMenu.FlatAppearance.BorderSize = 0;
            this.btnCloseMenu.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.btnCloseMenu.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.btnCloseMenu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseMenu.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCloseMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.btnCloseMenu.Image = global::Client_0000001243341361.Properties.Resources.cls;
            this.btnCloseMenu.Location = new System.Drawing.Point(0, 458);
            this.btnCloseMenu.Name = "btnCloseMenu";
            this.btnCloseMenu.Size = new System.Drawing.Size(29, 30);
            this.btnCloseMenu.TabIndex = 7;
            this.btnCloseMenu.UseVisualStyleBackColor = false;
            this.btnCloseMenu.Visible = false;
            this.btnCloseMenu.Click += new System.EventHandler(this.btnCloseMenu_Click);
            // 
            // btnDiscord
            // 
            this.btnDiscord.BackColor = System.Drawing.Color.Transparent;
            this.btnDiscord.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnDiscord.FlatAppearance.BorderSize = 0;
            this.btnDiscord.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.btnDiscord.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.btnDiscord.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDiscord.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDiscord.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.btnDiscord.Image = global::Client_0000001243341361.Properties.Resources.dis;
            this.btnDiscord.Location = new System.Drawing.Point(0, 162);
            this.btnDiscord.Name = "btnDiscord";
            this.btnDiscord.Size = new System.Drawing.Size(29, 44);
            this.btnDiscord.TabIndex = 5;
            this.btnDiscord.TabStop = false;
            this.btnDiscord.UseVisualStyleBackColor = false;
            this.btnDiscord.Click += new System.EventHandler(this.btnDiscord_Click);
            // 
            // btnDriver
            // 
            this.btnDriver.BackColor = System.Drawing.Color.Transparent;
            this.btnDriver.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnDriver.FlatAppearance.BorderSize = 0;
            this.btnDriver.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.btnDriver.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.btnDriver.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDriver.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDriver.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.btnDriver.Image = global::Client_0000001243341361.Properties.Resources.sys;
            this.btnDriver.Location = new System.Drawing.Point(0, 118);
            this.btnDriver.Name = "btnDriver";
            this.btnDriver.Size = new System.Drawing.Size(29, 44);
            this.btnDriver.TabIndex = 3;
            this.btnDriver.TabStop = false;
            this.btnDriver.UseVisualStyleBackColor = false;
            this.btnDriver.Click += new System.EventHandler(this.btnDriver_Click);
            // 
            // btnZK
            // 
            this.btnZK.BackColor = System.Drawing.Color.Transparent;
            this.btnZK.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnZK.FlatAppearance.BorderSize = 0;
            this.btnZK.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.btnZK.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.btnZK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnZK.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnZK.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.btnZK.Image = global::Client_0000001243341361.Properties.Resources.set;
            this.btnZK.Location = new System.Drawing.Point(0, 74);
            this.btnZK.Name = "btnZK";
            this.btnZK.Size = new System.Drawing.Size(29, 44);
            this.btnZK.TabIndex = 1;
            this.btnZK.TabStop = false;
            this.btnZK.UseVisualStyleBackColor = false;
            this.btnZK.Click += new System.EventHandler(this.btnZK_Click);
            // 
            // btnHome
            // 
            this.btnHome.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.btnHome.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnHome.FlatAppearance.BorderSize = 0;
            this.btnHome.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.btnHome.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.btnHome.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHome.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.btnHome.Image = global::Client_0000001243341361.Properties.Resources.dsh;
            this.btnHome.Location = new System.Drawing.Point(0, 30);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(29, 44);
            this.btnHome.TabIndex = 0;
            this.btnHome.TabStop = false;
            this.btnHome.UseVisualStyleBackColor = false;
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // wndMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(200)))), ((int)(((byte)(175)))));
            this.ClientSize = new System.Drawing.Size(750, 500);
            this.Controls.Add(this.pnlTitleBar);
            this.Controls.Add(this.pnlSideBar);
            this.Controls.Add(this.pnlClient);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(200)))), ((int)(((byte)(175)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(750, 450);
            this.Name = "wndMain";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ZeroKore";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.wndMain_FormClosing);
            this.Load += new System.EventHandler(this.wndMain_Load);
            this.SizeChanged += new System.EventHandler(this.wndMain_SizeChanged);
            this.Resize += new System.EventHandler(this.wndMain_Resize);
            this.pnlClient.ResumeLayout(false);
            this.pnlPageHome.ResumeLayout(false);
            this.pnlPageHome.PerformLayout();
            this.pnlSBrd2.ResumeLayout(false);
            this.pnlSBrd1.ResumeLayout(false);
            this.pnlStats.ResumeLayout(false);
            this.pnlStats.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            this.pnlZKSvc.ResumeLayout(false);
            this.pnlSvBrd1.ResumeLayout(false);
            this.pnlStatusSvc.ResumeLayout(false);
            this.pnlSvBrd3.ResumeLayout(false);
            this.pnlSvBrd2.ResumeLayout(false);
            this.pnlMgmtBd.ResumeLayout(false);
            this.pnlPageZKSet.ResumeLayout(false);
            this.pnlCBrd2.ResumeLayout(false);
            this.pnlCBrd1.ResumeLayout(false);
            this.pnlCfgBdy.ResumeLayout(false);
            this.pnlCfgBdy.PerformLayout();
            this.pnlTitleBar.ResumeLayout(false);
            this.pnlSideBar.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        #region User Init Code
        private void InitUX()
        {
            Screen screen = Screen.FromControl(this);
            int x = screen.WorkingArea.X - screen.Bounds.X;
            int y = screen.WorkingArea.Y - screen.Bounds.Y;
            this.MaximizedBounds = new Rectangle(x, y, screen.WorkingArea.Width, screen.WorkingArea.Height);
            this.MaximumSize = screen.WorkingArea.Size;

            lblWelcome.Text = "Welcome back " + Environment.UserName + "!";

            if (Properties.Settings.Default.IGM)
                hckIGM.Checked = true;
            else
                hckIGM.Checked = false;


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
        private void wndMain_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                btnMax.Image = Properties.Resources.maximised_transparent;
                TopColor = Color.FromArgb(0, 10, 15);
                BottomColor = Color.FromArgb(0, 10, 15);
            }
            else
            {
                btnMax.Image = Properties.Resources.maximise_transparent;
                TopColor = Color.FromArgb(0, 200, 175);
                BottomColor = Color.FromArgb(0, 250, 155);
            }

            if (this.WindowState != FormWindowState.Minimized)
            {
                this.Invalidate();
                pnlSideBar.Invalidate();
            }
        }

        private KeyboardHook hook = new KeyboardHook();
        private bool IsOverlayOpen = false;
        private wndOverlay Overlay;
        private void wndMain_Load(object sender, EventArgs e)
        {
            var LaunchedX = Properties.Settings.Default.TimesPlayed;
            if (LaunchedX > 0)
                lblTimesUsed.Text = LaunchedX.ToString();

            var TimeSpent = Properties.Settings.Default.MinsPlayed;
            if (TimeSpent > 0)
                lblTimePlayed.Text = TimeSpent.ToString();

            hook.KeyPressed += new EventHandler<KeyPressedEventArgs>(hook_KeyPressed);
            hook.RegisterHotKey(Libs.ModifierKeys.None, Keys.F3);
        }
        private void wndMain_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
        private void hook_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            if (Properties.Settings.Default.IGM)
            {
                if (IsOverlayOpen)
                {
                    Overlay.Close();
                    Overlay.Dispose();
                    IsOverlayOpen = false;

                    InitUX();

                    return;
                }
                if (!IsOverlayOpen)
                {
                    Overlay = new wndOverlay();
                    Overlay.Show();
                    IsOverlayOpen = true;

                    return;
                }
            }
        }
        #endregion

        #region Side Bar
        private bool SideBarIsOpen = false;
        private bool SideBarProcessing = false;
        private void pnlSideBar_MouseHover(object sender, EventArgs e)
        {
            if (!SideBarProcessing)
            {
                slider.Stop();
                SideBarIsOpen = false;
                slider.Start();
            }
        }
        private void ChangeControlStateSideBar(bool Vis)
        {
            if (Vis)
            {
                btnHome.Text = "Dashboard";
                btnHome.Padding = new Padding(15, 0, 15, 0);
                btnHome.TextAlign = ContentAlignment.MiddleRight;
                btnHome.ImageAlign = ContentAlignment.MiddleLeft;

                btnZK.Text = "Settings";
                btnZK.Padding = new Padding(22, 0, 25, 0);
                btnZK.TextAlign = ContentAlignment.MiddleRight;
                btnZK.ImageAlign = ContentAlignment.MiddleLeft;

                btnDiscord.Text = "Discord";
                btnDiscord.Padding = new Padding(23, 0, 30, 0);
                btnDiscord.TextAlign = ContentAlignment.MiddleRight;
                btnDiscord.ImageAlign = ContentAlignment.MiddleLeft;

                btnDriver.Text = "Service";
                btnDriver.Padding = new Padding(23, 0, 32, 0);
                btnDriver.TextAlign = ContentAlignment.MiddleRight;
                btnDriver.ImageAlign = ContentAlignment.MiddleLeft;

                btnCloseMenu.Visible = true;
            }
            else if (!Vis)
            {
                btnHome.Text = "";
                btnHome.Padding = new Padding(0);
                btnHome.TextAlign = ContentAlignment.MiddleCenter;
                btnHome.ImageAlign = ContentAlignment.MiddleCenter;

                btnZK.Text = "";
                btnZK.Padding = new Padding(0);
                btnZK.TextAlign = ContentAlignment.MiddleCenter;
                btnZK.ImageAlign = ContentAlignment.MiddleCenter;

                btnDiscord.Text = "";
                btnDiscord.Padding = new Padding(0);
                btnDiscord.TextAlign = ContentAlignment.MiddleCenter;
                btnDiscord.ImageAlign = ContentAlignment.MiddleCenter;

                btnDriver.Text = "";
                btnDriver.Padding = new Padding(0);
                btnDriver.TextAlign = ContentAlignment.MiddleCenter;
                btnDriver.ImageAlign = ContentAlignment.MiddleCenter;

                btnCloseMenu.Visible = false;
            }
        }
        private void TryCloseSideBar()
        {
            if (pnlSideBar.Width > 20)
            {
                slider.Stop();
                SideBarIsOpen = true;
                slider.Start();
            }
        }
        private void btnCloseMenu_Click(object sender, EventArgs e)
        {
            TryCloseSideBar();
        }
        private void slider_Tick(object sender, EventArgs e)
        {
            SideBarProcessing = true;

            if (!SideBarIsOpen)
            {
                if (pnlSideBar.Width != 150)
                    pnlSideBar.Width += 30;
                else
                {
                    SideBarIsOpen = true;
                    SideBarProcessing = false;
                    slider.Stop();
                }

                ChangeControlStateSideBar(true);
            }
            else if (SideBarIsOpen)
            {
                ChangeControlStateSideBar(false);

                if (pnlSideBar.Width != 30)
                    pnlSideBar.Width -= 30;
                else
                {
                    SideBarProcessing = false;
                    SideBarIsOpen = false;
                    slider.Stop();
                }
            }
        }
        private void btnHome_Click(object sender, EventArgs e)
        {
            pnlPageHome.BringToFront();
            btnHome.BackColor = Color.FromArgb(32, 0, 10, 15);
            btnZK.BackColor = Color.Transparent;
            btnDriver.BackColor = Color.Transparent;
            TryCloseSideBar();
        }
        private void btnZK_Click(object sender, EventArgs e)
        {
            pnlPageZKSet.BringToFront();
            btnHome.BackColor = Color.Transparent;
            btnZK.BackColor = Color.FromArgb(32, 0, 10, 15);
            btnDriver.BackColor = Color.Transparent;
            TryCloseSideBar();
        }
        private void btnDriver_Click(object sender, EventArgs e)
        {
            pnlZKSvc.BringToFront();
            btnHome.BackColor = Color.Transparent;
            btnZK.BackColor = Color.Transparent;
            btnDriver.BackColor = Color.FromArgb(32, 0, 10, 15);
            TryCloseSideBar();
        }
        private void btnDiscord_Click(object sender, EventArgs e)
        {
            Process.Start("https://discord.gg/EZ3u77z");
        }
        private void pnlSideBar_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                TryCloseSideBar();
        }
        #endregion

        #region Settings
        private void chkAA_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAA.Checked)
                Properties.Settings.Default.AimAssist = true;
            else
                Properties.Settings.Default.AimAssist = false;
        }

        private void chkAS_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAS.Checked)
                Properties.Settings.Default.AutoShoot = true;
            else
                Properties.Settings.Default.AutoShoot = false;
        }

        private void chkVA_CheckedChanged(object sender, EventArgs e)
        {
            if (chkVA.Checked)
                Properties.Settings.Default.VisualAssistance = true;
            else
                Properties.Settings.Default.VisualAssistance = false;
        }

        private void chkCJ_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCJ.Checked)
                Properties.Settings.Default.ConstJump = true;
            else
                Properties.Settings.Default.ConstJump = false;
        }

        private void chkER_CheckedChanged(object sender, EventArgs e)
        {
            if (chkER.Checked)
                Properties.Settings.Default.EnhancedRadar = true;
            else
                Properties.Settings.Default.EnhancedRadar = false;
        }

        private void chkFG_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFG.Checked)
                Properties.Settings.Default.FlashGlasses = true;
            else
                Properties.Settings.Default.FlashGlasses = false;
        }
        #endregion

        private System.Windows.Forms.Panel pnlClient;
        private System.Windows.Forms.Panel pnlTitleBar;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnMin;
        private System.Windows.Forms.Button btnMax;
        private Panel pnlPageHome;
        private PictureBox pbLogo;
        private Label lblWelcome;
        private Label lblWelcomeCaption;
        private Timer slider;
        private Label lblHActivity;
        private Panel pnlSBrd2;
        private Controls.GradientPanel pnlSBrd1;
        private Panel pnlStats;
        private Label lblTimePlayed;
        private Label lblTimesUsed;
        private Panel pnlPageZKSet;
        private Panel pnlCBrd2;
        private Label lblCfgTitle;
        private Controls.GradientPanel pnlCBrd1;
        private Panel pnlCfgBdy;
        private Label lblZKTitle;
        private CheckBox chkCJ;
        private CheckBox chkVA;
        private CheckBox chkAS;
        private CheckBox chkAA;
        private CheckBox chkFG;
        private CheckBox chkER;
        private Label lblAA;
        private Label lblComingSoon;
        private Label lblHypeTxt;
        private CheckBox hckIGM;
        private Label lblIGMKey;
        private Button btnHome;
        private Button btnZK;
        private Button btnDriver;
        private Button btnDiscord;
        private Button btnCloseMenu;
        private Controls.GradientPanel pnlSideBar;
        private Panel pnlZKSvc;
        private Panel pnlSvBrd3;
        private Label lblHMgmt;
        private Controls.GradientPanel pnlSvBrd2;
        private Panel pnlMgmtBd;
        private Label lblHSvc;
        private Controls.GradientPanel pnlSvBrd1;
        private Panel pnlStatusSvc;
        private Label lblAdvSvcStat;
        private Button btnStartStop;
        private Button btnStopSvc;
        private Button btnStartSvc;
        private Button btnInsSvc;
        private Button btnRmSvc;
        private Button btnSvcHelp;
        private Label lblmp;
        private Label lbltp;
    }
}

