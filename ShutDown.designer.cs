namespace _i
{
    partial class ShutDown
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
            this.lblExitTime = new System.Windows.Forms.Label();
            this.lblClose = new System.Windows.Forms.Label();
            this.tmrCountDown = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // lblExitTime
            // 
            this.lblExitTime.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblExitTime.BackColor = System.Drawing.Color.White;
            this.lblExitTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExitTime.ForeColor = System.Drawing.Color.Green;
            this.lblExitTime.Location = new System.Drawing.Point(-1, 0);
            this.lblExitTime.Name = "lblExitTime";
            this.lblExitTime.Size = new System.Drawing.Size(130, 36);
            this.lblExitTime.TabIndex = 1;
            this.lblExitTime.Text = "Tắt máy sau 5:00";
            this.lblExitTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblClose
            // 
            this.lblClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblClose.BackColor = System.Drawing.Color.Silver;
            this.lblClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblClose.Location = new System.Drawing.Point(117, 36);
            this.lblClose.Name = "lblClose";
            this.lblClose.Size = new System.Drawing.Size(12, 12);
            this.lblClose.TabIndex = 1;
            this.lblClose.Text = "  ";
            this.lblClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblClose.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lblClose_MouseClick);
            // 
            // tmrCountDown
            // 
            this.tmrCountDown.Enabled = true;
            this.tmrCountDown.Interval = 1000;
            this.tmrCountDown.Tick += new System.EventHandler(this.tmrCountDown_Tick);
            // 
            // ShutDown
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Red;
            this.ClientSize = new System.Drawing.Size(129, 48);
            this.Controls.Add(this.lblExitTime);
            this.Controls.Add(this.lblClose);
            this.Name = "ShutDown";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ShutDown";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.ShutDown_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblExitTime;
        private System.Windows.Forms.Label lblClose;
        private System.Windows.Forms.Timer tmrCountDown;
    }
}