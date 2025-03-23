namespace _i
{
    partial class EnterCaptchaContainer
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
            this.components = new System.ComponentModel.Container();
            this.lblPin = new System.Windows.Forms.Label();
            this.tmrMonitor = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // lblPin
            // 
            this.lblPin.BackColor = System.Drawing.Color.Red;
            this.lblPin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblPin.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPin.ForeColor = System.Drawing.Color.Green;
            this.lblPin.Location = new System.Drawing.Point(0, 0);
            this.lblPin.Name = "lblPin";
            this.lblPin.Size = new System.Drawing.Size(188, 10);
            this.lblPin.TabIndex = 62;
            this.lblPin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblPin.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lblPin_MouseClick);
            // 
            // tmrMonitor
            // 
            this.tmrMonitor.Enabled = true;
            this.tmrMonitor.Tick += new System.EventHandler(this.tmrMonitor_Tick);
            // 
            // EnterCaptchaContainer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(135)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(188, 231);
            this.ControlBox = false;
            this.Controls.Add(this.lblPin);
            this.MaximizeBox = false;
            this.Name = "EnterCaptchaContainer";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Captcha";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.EnterCaptchaContainer_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblPin;
        private System.Windows.Forms.Timer tmrMonitor;
    }
}