namespace _i
{
    partial class AntiCaptcha
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.picCaptcha = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exitGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tmrRefresh = new System.Windows.Forms.Timer(this.components);
            this.txtAnswer = new _i.NumericTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.picCaptcha)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // picCaptcha
            // 
            this.picCaptcha.ContextMenuStrip = this.contextMenuStrip1;
            this.picCaptcha.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picCaptcha.Dock = System.Windows.Forms.DockStyle.Left;
            //this.picCaptcha.ErrorImage = global::MicroAuto.Properties.Resources.captcha;
            this.picCaptcha.Image = global::_i.Properties.Resources.loading;
            //this.picCaptcha.InitialImage = global::MicroAuto.Properties.Resources.captcha;
            this.picCaptcha.Location = new System.Drawing.Point(0, 0);
            this.picCaptcha.Name = "picCaptcha";
            this.picCaptcha.Size = new System.Drawing.Size(160, 45);
            this.picCaptcha.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picCaptcha.TabIndex = 50;
            this.picCaptcha.TabStop = false;
            this.picCaptcha.MouseClick += new System.Windows.Forms.MouseEventHandler(this.picCaptcha_MouseClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitGameToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(128, 26);
            // 
            // exitGameToolStripMenuItem
            // 
            this.exitGameToolStripMenuItem.Name = "exitGameToolStripMenuItem";
            this.exitGameToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.exitGameToolStripMenuItem.Text = "Exit Game";
            this.exitGameToolStripMenuItem.Click += new System.EventHandler(this.exitGameToolStripMenuItem_Click);
            // 
            // tmrRefresh
            // 
            this.tmrRefresh.Enabled = true;
            this.tmrRefresh.Interval = 1500;
            this.tmrRefresh.Tick += new System.EventHandler(this.tmrRefresh_Tick);
            // 
            // txtAnswer
            // 
            this.txtAnswer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAnswer.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAnswer.Location = new System.Drawing.Point(160, 0);
            this.txtAnswer.Name = "txtAnswer";
            this.txtAnswer.Size = new System.Drawing.Size(100, 45);
            this.txtAnswer.TabIndex = 52;
            this.txtAnswer.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtAnswer.Visible = false;
            this.txtAnswer.WaterMark = "answer...";
            this.txtAnswer.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtAnswer.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAnswer.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtAnswer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtAnswer_KeyDown_1);
            // 
            // AntiCaptcha
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(135)))), ((int)(((byte)(240)))));
            this.Controls.Add(this.txtAnswer);
            this.Controls.Add(this.picCaptcha);
            this.Name = "AntiCaptcha";
            this.Size = new System.Drawing.Size(260, 45);
            this.Load += new System.EventHandler(this.AntiCaptcha_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picCaptcha)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picCaptcha;
        private System.Windows.Forms.Timer tmrRefresh;
        private NumericTextBox txtAnswer;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem exitGameToolStripMenuItem;
    }
}
