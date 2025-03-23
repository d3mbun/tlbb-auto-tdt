namespace _i
{
    partial class Alan
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
            this.tmrCountDown = new System.Windows.Forms.Timer(this.components);
            this.tmrMute = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // tmrCountDown
            // 
            this.tmrCountDown.Enabled = true;
            this.tmrCountDown.Tick += new System.EventHandler(this.tmrCountDown_Tick);
            // 
            // tmrMute
            // 
            this.tmrMute.Enabled = true;
            this.tmrMute.Interval = 1000;
            this.tmrMute.Tick += new System.EventHandler(this.tmrMute_Tick);
            // 
            // Alan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(360, 90);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximumSize = new System.Drawing.Size(360, 90);
            this.Name = "Alan";
            this.Opacity = 0.75D;
            this.ShowInTaskbar = false;
            this.Text = "AlarmEx";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Alarm_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer tmrCountDown;
        private System.Windows.Forms.Timer tmrMute;
    }
}