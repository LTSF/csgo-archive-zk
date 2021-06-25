namespace ZeroKore.Client
{
    partial class mwndError
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
            this.btnClose = new System.Windows.Forms.Button();
            this.lblSubH = new System.Windows.Forms.Label();
            this.lblHeading = new System.Windows.Forms.Label();
            this.pbxCross = new System.Windows.Forms.PictureBox();
            this.lblError = new System.Windows.Forms.RichTextBox();
            this.pnlBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxCross)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlBody
            // 
            this.pnlBody.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(15)))));
            this.pnlBody.Controls.Add(this.lblError);
            this.pnlBody.Controls.Add(this.btnClose);
            this.pnlBody.Controls.Add(this.lblSubH);
            this.pnlBody.Controls.Add(this.lblHeading);
            this.pnlBody.Controls.Add(this.pbxCross);
            this.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBody.ForeColor = System.Drawing.Color.White;
            this.pnlBody.Location = new System.Drawing.Point(1, 1);
            this.pnlBody.Name = "pnlBody";
            this.pnlBody.Size = new System.Drawing.Size(548, 448);
            this.pnlBody.TabIndex = 0;
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
            this.btnClose.Location = new System.Drawing.Point(513, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(30, 25);
            this.btnClose.TabIndex = 1002;
            this.btnClose.TabStop = false;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblSubH
            // 
            this.lblSubH.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubH.Location = new System.Drawing.Point(135, 61);
            this.lblSubH.Name = "lblSubH";
            this.lblSubH.Size = new System.Drawing.Size(393, 37);
            this.lblSubH.TabIndex = 2;
            this.lblSubH.Text = "This might be due to insufficient permissions, software bug or an issue with the " +
    ".NET Framework.";
            // 
            // lblHeading
            // 
            this.lblHeading.Font = new System.Drawing.Font("Segoe UI Semilight", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeading.Location = new System.Drawing.Point(135, 30);
            this.lblHeading.Name = "lblHeading";
            this.lblHeading.Size = new System.Drawing.Size(393, 40);
            this.lblHeading.TabIndex = 1;
            this.lblHeading.Text = "ZeroKore® has encountered a problem.";
            // 
            // pbxCross
            // 
            this.pbxCross.Image = global::ZeroKore.Client.Properties.Resources.delete_96px;
            this.pbxCross.Location = new System.Drawing.Point(22, 21);
            this.pbxCross.Name = "pbxCross";
            this.pbxCross.Size = new System.Drawing.Size(96, 96);
            this.pbxCross.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbxCross.TabIndex = 0;
            this.pbxCross.TabStop = false;
            // 
            // lblError
            // 
            this.lblError.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(20)))), ((int)(((byte)(30)))));
            this.lblError.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblError.ForeColor = System.Drawing.Color.White;
            this.lblError.Location = new System.Drawing.Point(22, 136);
            this.lblError.Name = "lblError";
            this.lblError.ReadOnly = true;
            this.lblError.Size = new System.Drawing.Size(506, 292);
            this.lblError.TabIndex = 1003;
            this.lblError.Text = "ZeroKore.UnknownError";
            // 
            // mwndError
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Red;
            this.ClientSize = new System.Drawing.Size(550, 450);
            this.Controls.Add(this.pnlBody);
            this.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "mwndError";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Problem Reporter";
            this.TopMost = true;
            this.pnlBody.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbxCross)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlBody;
        private System.Windows.Forms.PictureBox pbxCross;
        private System.Windows.Forms.Label lblHeading;
        private System.Windows.Forms.Label lblSubH;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.RichTextBox lblError;
    }
}