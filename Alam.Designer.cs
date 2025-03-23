namespace _i
{
    partial class Alam
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Alam));
            this.lblClose = new System.Windows.Forms.Label();
            this.lblExit = new System.Windows.Forms.Label();
            this.lblAlarm = new System.Windows.Forms.Label();
            this.lblCharName = new System.Windows.Forms.Label();
            this.tmrCountDown = new System.Windows.Forms.Timer(this.components);
            this.lblMute = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tmrMute = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblClose
            // 
            this.lblClose.BackColor = System.Drawing.Color.Silver;
            this.lblClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblClose.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblClose.Location = new System.Drawing.Point(310, 0);
            this.lblClose.Name = "lblClose";
            this.lblClose.Size = new System.Drawing.Size(20, 30);
            this.lblClose.TabIndex = 10;
            this.lblClose.Text = " ";
            this.lblClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblClose.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lblClose_MouseClick);
            // 
            // lblExit
            // 
            this.lblExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblExit.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExit.ForeColor = System.Drawing.Color.Green;
            this.lblExit.Location = new System.Drawing.Point(270, 0);
            this.lblExit.Name = "lblExit";
            this.lblExit.Size = new System.Drawing.Size(40, 30);
            this.lblExit.TabIndex = 12;
            this.lblExit.Text = "0:00";
            this.lblExit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblExit.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lblExit_MouseClick);
            // 
            // lblAlarm
            // 
            this.lblAlarm.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblAlarm.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblAlarm.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAlarm.ForeColor = System.Drawing.Color.Red;
            this.lblAlarm.Location = new System.Drawing.Point(120, 0);
            this.lblAlarm.Name = "lblAlarm";
            this.lblAlarm.Size = new System.Drawing.Size(150, 30);
            this.lblAlarm.TabIndex = 11;
            this.lblAlarm.Text = "Đang có câu hỏi";
            this.lblAlarm.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblAlarm.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lblAlarm_MouseClick);
            // 
            // lblCharName
            // 
            this.lblCharName.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblCharName.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblCharName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCharName.ForeColor = System.Drawing.Color.Green;
            this.lblCharName.Location = new System.Drawing.Point(0, 0);
            this.lblCharName.Name = "lblCharName";
            this.lblCharName.Size = new System.Drawing.Size(120, 30);
            this.lblCharName.TabIndex = 9;
            this.lblCharName.Text = "LUCDINHPHONG";
            this.lblCharName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblCharName.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lblCharName_MouseClick);
            // 
            // tmrCountDown
            // 
            this.tmrCountDown.Interval = 1000;
            this.tmrCountDown.Tick += new System.EventHandler(this.tmrCountDown_Tick);
            // 
            // lblMute
            // 
            this.lblMute.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMute.BackColor = System.Drawing.Color.Red;
            this.lblMute.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblMute.ForeColor = System.Drawing.Color.Red;
            this.lblMute.Location = new System.Drawing.Point(339, 9);
            this.lblMute.Name = "lblMute";
            this.lblMute.Size = new System.Drawing.Size(21, 10);
            this.lblMute.TabIndex = 14;
            this.lblMute.Text = "label1";
            this.lblMute.Visible = false;
            this.lblMute.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lblMute_MouseClick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(330, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(30, 30);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 13;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseClick);
            // 
            // tmrMute
            // 
            this.tmrMute.Enabled = true;
            this.tmrMute.Interval = 1000;
            this.tmrMute.Tick += new System.EventHandler(this.tmrMute_Tick);
            // 
            // Alam
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblClose);
            this.Controls.Add(this.lblExit);
            this.Controls.Add(this.lblAlarm);
            this.Controls.Add(this.lblCharName);
            this.Controls.Add(this.lblMute);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Alam";
            this.Size = new System.Drawing.Size(360, 30);
            this.Load += new System.EventHandler(this.Alam_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblClose;
        private System.Windows.Forms.Label lblExit;
        private System.Windows.Forms.Label lblAlarm;
        private System.Windows.Forms.Label lblCharName;
        private System.Windows.Forms.Timer tmrCountDown;
        private System.Windows.Forms.Label lblMute;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Timer tmrMute;
    }
}
