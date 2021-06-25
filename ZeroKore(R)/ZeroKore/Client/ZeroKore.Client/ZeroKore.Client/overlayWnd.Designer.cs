namespace ZeroKore.Client
{
    partial class overlayWnd
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlBody = new System.Windows.Forms.Panel();
            this.pbxLogo = new System.Windows.Forms.PictureBox();
            this.lblCopyright = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
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
            this.btnClose = new System.Windows.Forms.Button();
            this.pnlBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlBody
            // 
            this.pnlBody.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.pnlBody.Controls.Add(this.btnClose);
            this.pnlBody.Controls.Add(this.lblFG);
            this.pnlBody.Controls.Add(this.tsFG);
            this.pnlBody.Controls.Add(this.lblER);
            this.pnlBody.Controls.Add(this.tsER);
            this.pnlBody.Controls.Add(this.lblCJ);
            this.pnlBody.Controls.Add(this.tsCJ);
            this.pnlBody.Controls.Add(this.lblAS);
            this.pnlBody.Controls.Add(this.tsAS);
            this.pnlBody.Controls.Add(this.lblVA);
            this.pnlBody.Controls.Add(this.tsVA);
            this.pnlBody.Controls.Add(this.lblAA);
            this.pnlBody.Controls.Add(this.tsAA);
            this.pnlBody.Controls.Add(this.lblTitle);
            this.pnlBody.Controls.Add(this.lblCopyright);
            this.pnlBody.Controls.Add(this.pbxLogo);
            this.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBody.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(200)))), ((int)(((byte)(175)))));
            this.pnlBody.Location = new System.Drawing.Point(1, 1);
            this.pnlBody.Name = "pnlBody";
            this.pnlBody.Size = new System.Drawing.Size(498, 298);
            this.pnlBody.TabIndex = 0;
            this.pnlBody.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlBody_MouseDown);
            // 
            // pbxLogo
            // 
            this.pbxLogo.Image = global::ZeroKore.Client.Properties.Resources.LogoPNG;
            this.pbxLogo.Location = new System.Drawing.Point(420, 215);
            this.pbxLogo.Name = "pbxLogo";
            this.pbxLogo.Size = new System.Drawing.Size(64, 64);
            this.pbxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbxLogo.TabIndex = 0;
            this.pbxLogo.TabStop = false;
            // 
            // lblCopyright
            // 
            this.lblCopyright.AutoSize = true;
            this.lblCopyright.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCopyright.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(75)))), ((int)(((byte)(100)))));
            this.lblCopyright.Location = new System.Drawing.Point(15, 265);
            this.lblCopyright.Name = "lblCopyright";
            this.lblCopyright.Size = new System.Drawing.Size(133, 15);
            this.lblCopyright.TabIndex = 1;
            this.lblCopyright.Text = "© VodCode Lab 2020";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semilight", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(12, 22);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(293, 32);
            this.lblTitle.TabIndex = 2;
            this.lblTitle.Text = "ZeroKore® In-Game Menu";
            // 
            // lblFG
            // 
            this.lblFG.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFG.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(250)))), ((int)(((byte)(225)))));
            this.lblFG.Location = new System.Drawing.Point(261, 74);
            this.lblFG.Name = "lblFG";
            this.lblFG.Size = new System.Drawing.Size(135, 28);
            this.lblFG.TabIndex = 37;
            this.lblFG.Text = "Flash Glasses";
            this.lblFG.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tsFG
            // 
            this.tsFG.Checked = true;
            this.tsFG.CheckedBack = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(60)))), ((int)(((byte)(90)))));
            this.tsFG.CheckedFore = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(250)))), ((int)(((byte)(155)))));
            this.tsFG.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsFG.Location = new System.Drawing.Point(410, 76);
            this.tsFG.Name = "tsFG";
            this.tsFG.Padding = new System.Windows.Forms.Padding(6);
            this.tsFG.Size = new System.Drawing.Size(50, 25);
            this.tsFG.TabIndex = 36;
            this.tsFG.UnCheckedBack = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(60)))), ((int)(((byte)(90)))));
            this.tsFG.UnCheckedFore = System.Drawing.Color.LightGray;
            this.tsFG.UseVisualStyleBackColor = true;
            this.tsFG.CheckedChanged += new System.EventHandler(this.tsFG_CheckedChanged);
            // 
            // lblER
            // 
            this.lblER.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblER.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(250)))), ((int)(((byte)(225)))));
            this.lblER.Location = new System.Drawing.Point(261, 109);
            this.lblER.Name = "lblER";
            this.lblER.Size = new System.Drawing.Size(135, 28);
            this.lblER.TabIndex = 35;
            this.lblER.Text = "Enhanced Radar";
            this.lblER.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tsER
            // 
            this.tsER.Checked = true;
            this.tsER.CheckedBack = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(60)))), ((int)(((byte)(90)))));
            this.tsER.CheckedFore = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(250)))), ((int)(((byte)(155)))));
            this.tsER.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsER.Location = new System.Drawing.Point(410, 111);
            this.tsER.Name = "tsER";
            this.tsER.Padding = new System.Windows.Forms.Padding(6);
            this.tsER.Size = new System.Drawing.Size(50, 25);
            this.tsER.TabIndex = 34;
            this.tsER.UnCheckedBack = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(60)))), ((int)(((byte)(90)))));
            this.tsER.UnCheckedFore = System.Drawing.Color.LightGray;
            this.tsER.UseVisualStyleBackColor = true;
            this.tsER.CheckedChanged += new System.EventHandler(this.tsER_CheckedChanged);
            // 
            // lblCJ
            // 
            this.lblCJ.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCJ.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(250)))), ((int)(((byte)(225)))));
            this.lblCJ.Location = new System.Drawing.Point(14, 179);
            this.lblCJ.Name = "lblCJ";
            this.lblCJ.Size = new System.Drawing.Size(117, 28);
            this.lblCJ.TabIndex = 33;
            this.lblCJ.Text = "Constant Jump";
            this.lblCJ.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tsCJ
            // 
            this.tsCJ.Checked = true;
            this.tsCJ.CheckedBack = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(60)))), ((int)(((byte)(90)))));
            this.tsCJ.CheckedFore = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(250)))), ((int)(((byte)(155)))));
            this.tsCJ.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsCJ.Location = new System.Drawing.Point(164, 181);
            this.tsCJ.Name = "tsCJ";
            this.tsCJ.Padding = new System.Windows.Forms.Padding(6);
            this.tsCJ.Size = new System.Drawing.Size(50, 25);
            this.tsCJ.TabIndex = 32;
            this.tsCJ.UnCheckedBack = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(60)))), ((int)(((byte)(90)))));
            this.tsCJ.UnCheckedFore = System.Drawing.Color.LightGray;
            this.tsCJ.UseVisualStyleBackColor = true;
            this.tsCJ.CheckedChanged += new System.EventHandler(this.tsAS_CheckedChanged);
            // 
            // lblAS
            // 
            this.lblAS.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAS.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(250)))), ((int)(((byte)(225)))));
            this.lblAS.Location = new System.Drawing.Point(14, 144);
            this.lblAS.Name = "lblAS";
            this.lblAS.Size = new System.Drawing.Size(94, 28);
            this.lblAS.TabIndex = 31;
            this.lblAS.Text = "Auto Shoot";
            this.lblAS.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tsAS
            // 
            this.tsAS.Checked = true;
            this.tsAS.CheckedBack = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(60)))), ((int)(((byte)(90)))));
            this.tsAS.CheckedFore = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(250)))), ((int)(((byte)(155)))));
            this.tsAS.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsAS.Location = new System.Drawing.Point(164, 147);
            this.tsAS.Name = "tsAS";
            this.tsAS.Padding = new System.Windows.Forms.Padding(6);
            this.tsAS.Size = new System.Drawing.Size(50, 25);
            this.tsAS.TabIndex = 30;
            this.tsAS.UnCheckedBack = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(60)))), ((int)(((byte)(90)))));
            this.tsAS.UnCheckedFore = System.Drawing.Color.LightGray;
            this.tsAS.UseVisualStyleBackColor = true;
            this.tsAS.CheckedChanged += new System.EventHandler(this.tsAS_CheckedChanged);
            // 
            // lblVA
            // 
            this.lblVA.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVA.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(250)))), ((int)(((byte)(225)))));
            this.lblVA.Location = new System.Drawing.Point(14, 109);
            this.lblVA.Name = "lblVA";
            this.lblVA.Size = new System.Drawing.Size(136, 28);
            this.lblVA.TabIndex = 29;
            this.lblVA.Text = "Visual Assistance";
            this.lblVA.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tsVA
            // 
            this.tsVA.Checked = true;
            this.tsVA.CheckedBack = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(60)))), ((int)(((byte)(90)))));
            this.tsVA.CheckedFore = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(250)))), ((int)(((byte)(155)))));
            this.tsVA.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsVA.Location = new System.Drawing.Point(164, 112);
            this.tsVA.Name = "tsVA";
            this.tsVA.Padding = new System.Windows.Forms.Padding(6);
            this.tsVA.Size = new System.Drawing.Size(50, 25);
            this.tsVA.TabIndex = 28;
            this.tsVA.UnCheckedBack = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(60)))), ((int)(((byte)(90)))));
            this.tsVA.UnCheckedFore = System.Drawing.Color.LightGray;
            this.tsVA.UseVisualStyleBackColor = true;
            this.tsVA.CheckedChanged += new System.EventHandler(this.tsVA_CheckedChanged);
            // 
            // lblAA
            // 
            this.lblAA.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAA.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(250)))), ((int)(((byte)(225)))));
            this.lblAA.Location = new System.Drawing.Point(14, 74);
            this.lblAA.Name = "lblAA";
            this.lblAA.Size = new System.Drawing.Size(117, 28);
            this.lblAA.TabIndex = 27;
            this.lblAA.Text = "Aim Assistance";
            this.lblAA.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tsAA
            // 
            this.tsAA.Checked = true;
            this.tsAA.CheckedBack = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(60)))), ((int)(((byte)(90)))));
            this.tsAA.CheckedFore = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(250)))), ((int)(((byte)(155)))));
            this.tsAA.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsAA.Location = new System.Drawing.Point(164, 77);
            this.tsAA.Name = "tsAA";
            this.tsAA.Padding = new System.Windows.Forms.Padding(6);
            this.tsAA.Size = new System.Drawing.Size(50, 25);
            this.tsAA.TabIndex = 26;
            this.tsAA.UnCheckedBack = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(60)))), ((int)(((byte)(90)))));
            this.tsAA.UnCheckedFore = System.Drawing.Color.LightGray;
            this.tsAA.UseVisualStyleBackColor = true;
            this.tsAA.CheckedChanged += new System.EventHandler(this.tsAA_CheckedChanged);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(20)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Image = global::ZeroKore.Client.Properties.Resources.close_transparent;
            this.btnClose.Location = new System.Drawing.Point(465, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(30, 25);
            this.btnClose.TabIndex = 1003;
            this.btnClose.TabStop = false;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // overlayWnd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(200)))), ((int)(((byte)(175)))));
            this.ClientSize = new System.Drawing.Size(500, 300);
            this.Controls.Add(this.pnlBody);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "overlayWnd";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.overlayWnd_FormClosing);
            this.pnlBody.ResumeLayout(false);
            this.pnlBody.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlBody;
        private System.Windows.Forms.PictureBox pbxLogo;
        private System.Windows.Forms.Label lblCopyright;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblFG;
        private UX.ToggleSwitch tsFG;
        private System.Windows.Forms.Label lblER;
        private UX.ToggleSwitch tsER;
        private System.Windows.Forms.Label lblCJ;
        private UX.ToggleSwitch tsCJ;
        private System.Windows.Forms.Label lblAS;
        private UX.ToggleSwitch tsAS;
        private System.Windows.Forms.Label lblVA;
        private UX.ToggleSwitch tsVA;
        private System.Windows.Forms.Label lblAA;
        private UX.ToggleSwitch tsAA;
        private System.Windows.Forms.Button btnClose;
    }
}