namespace ZeroKore.Client
{
    partial class mwndColour
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.pbxResult = new System.Windows.Forms.PictureBox();
            this.lblB = new System.Windows.Forms.Label();
            this.lblG = new System.Windows.Forms.Label();
            this.lblR = new System.Windows.Forms.Label();
            this.hsbB = new System.Windows.Forms.HScrollBar();
            this.hsbG = new System.Windows.Forms.HScrollBar();
            this.hsbR = new System.Windows.Forms.HScrollBar();
            this.pnlBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxResult)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlBody
            // 
            this.pnlBody.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.pnlBody.Controls.Add(this.btnCancel);
            this.pnlBody.Controls.Add(this.btnOK);
            this.pnlBody.Controls.Add(this.pbxResult);
            this.pnlBody.Controls.Add(this.lblB);
            this.pnlBody.Controls.Add(this.lblG);
            this.pnlBody.Controls.Add(this.lblR);
            this.pnlBody.Controls.Add(this.hsbB);
            this.pnlBody.Controls.Add(this.hsbG);
            this.pnlBody.Controls.Add(this.hsbR);
            this.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBody.Location = new System.Drawing.Point(1, 1);
            this.pnlBody.Name = "pnlBody";
            this.pnlBody.Size = new System.Drawing.Size(282, 159);
            this.pnlBody.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(250)))), ((int)(((byte)(155)))));
            this.btnCancel.Location = new System.Drawing.Point(143, 117);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(60, 25);
            this.btnCancel.TabIndex = 27;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(250)))), ((int)(((byte)(155)))));
            this.btnOK.Location = new System.Drawing.Point(209, 117);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(60, 25);
            this.btnOK.TabIndex = 26;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // pbxResult
            // 
            this.pbxResult.BackColor = System.Drawing.Color.Black;
            this.pbxResult.Location = new System.Drawing.Point(17, 94);
            this.pbxResult.Name = "pbxResult";
            this.pbxResult.Size = new System.Drawing.Size(48, 48);
            this.pbxResult.TabIndex = 25;
            this.pbxResult.TabStop = false;
            // 
            // lblB
            // 
            this.lblB.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblB.ForeColor = System.Drawing.Color.Blue;
            this.lblB.Location = new System.Drawing.Point(14, 67);
            this.lblB.Name = "lblB";
            this.lblB.Size = new System.Drawing.Size(45, 15);
            this.lblB.TabIndex = 24;
            this.lblB.Text = "Blue:";
            this.lblB.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblG
            // 
            this.lblG.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblG.ForeColor = System.Drawing.Color.Lime;
            this.lblG.Location = new System.Drawing.Point(14, 42);
            this.lblG.Name = "lblG";
            this.lblG.Size = new System.Drawing.Size(45, 15);
            this.lblG.TabIndex = 23;
            this.lblG.Text = "Green:";
            this.lblG.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblR
            // 
            this.lblR.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblR.ForeColor = System.Drawing.Color.Red;
            this.lblR.Location = new System.Drawing.Point(14, 17);
            this.lblR.Name = "lblR";
            this.lblR.Size = new System.Drawing.Size(45, 15);
            this.lblR.TabIndex = 22;
            this.lblR.Text = "Red:";
            this.lblR.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // hsbB
            // 
            this.hsbB.Location = new System.Drawing.Point(69, 67);
            this.hsbB.Maximum = 255;
            this.hsbB.Name = "hsbB";
            this.hsbB.Size = new System.Drawing.Size(200, 15);
            this.hsbB.TabIndex = 21;
            this.hsbB.ValueChanged += new System.EventHandler(this.hsbB_ValueChanged);
            // 
            // hsbG
            // 
            this.hsbG.Location = new System.Drawing.Point(69, 42);
            this.hsbG.Maximum = 255;
            this.hsbG.Name = "hsbG";
            this.hsbG.Size = new System.Drawing.Size(200, 15);
            this.hsbG.TabIndex = 20;
            this.hsbG.ValueChanged += new System.EventHandler(this.hsbG_ValueChanged);
            // 
            // hsbR
            // 
            this.hsbR.Location = new System.Drawing.Point(69, 17);
            this.hsbR.Maximum = 255;
            this.hsbR.Name = "hsbR";
            this.hsbR.Size = new System.Drawing.Size(200, 15);
            this.hsbR.TabIndex = 19;
            this.hsbR.ValueChanged += new System.EventHandler(this.hsbR_ValueChanged);
            // 
            // mwndColour
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(250)))), ((int)(((byte)(155)))));
            this.ClientSize = new System.Drawing.Size(284, 161);
            this.Controls.Add(this.pnlBody);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(250)))), ((int)(((byte)(225)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "mwndColour";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Colour Picker";
            this.pnlBody.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbxResult)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlBody;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.PictureBox pbxResult;
        private System.Windows.Forms.Label lblB;
        private System.Windows.Forms.Label lblG;
        private System.Windows.Forms.Label lblR;
        private System.Windows.Forms.HScrollBar hsbB;
        private System.Windows.Forms.HScrollBar hsbG;
        private System.Windows.Forms.HScrollBar hsbR;
    }
}