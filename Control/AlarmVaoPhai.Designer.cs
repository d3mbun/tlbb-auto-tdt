namespace _i
{
    partial class AlarmVaoPhai
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
            this.lblName = new System.Windows.Forms.Label();
            this.lblAlarm = new System.Windows.Forms.Label();
            this.cboMenpai = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.lblClose = new System.Windows.Forms.Label();
            this.chkCoBan = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblName.Location = new System.Drawing.Point(0, 0);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(320, 23);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "tieudattai";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblName.Click += new System.EventHandler(this.lblName_Click);
            this.lblName.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lblName_MouseClick);
            // 
            // lblAlarm
            // 
            this.lblAlarm.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblAlarm.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblAlarm.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAlarm.ForeColor = System.Drawing.Color.Red;
            this.lblAlarm.Location = new System.Drawing.Point(0, 23);
            this.lblAlarm.Name = "lblAlarm";
            this.lblAlarm.Size = new System.Drawing.Size(320, 48);
            this.lblAlarm.TabIndex = 1;
            this.lblAlarm.Text = "chưa vào phái\r\nbạn có muốn auto tự vào phái không\r\n( tự vào phái khi đủ cấp )";
            this.lblAlarm.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblAlarm.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lblAlarm_MouseClick);
            // 
            // cboMenpai
            // 
            this.cboMenpai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMenpai.FormattingEnabled = true;
            this.cboMenpai.Items.AddRange(new object[] {
            "Thiếu Lâm",
            "Minh Giáo",
            "Cái Bang",
            "Võ Đang (Đánh Xa)",
            "Nga My (Đánh Xa)",
            "Tinh Túc (Đánh Xa)",
            "Thiên Long (Đánh Xa)",
            "Thiên Sơn",
            "Tiêu Dao (Đánh Xa)",
            "Mộ Dung",
            "Đường Môn (Đánh Xa)",
            "Quỷ Cốc (Đánh Xa)",
            "Đào Hoa (Đánh Xa)",
            "Để Tôi Tự Vào"});
            this.cboMenpai.Location = new System.Drawing.Point(59, 77);
            this.cboMenpai.Name = "cboMenpai";
            this.cboMenpai.Size = new System.Drawing.Size(170, 21);
            this.cboMenpai.TabIndex = 2;
            this.cboMenpai.SelectedIndexChanged += new System.EventHandler(this.cboMenpai_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Vào Phái";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(235, 76);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "Đồng Ý";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // lblClose
            // 
            this.lblClose.BackColor = System.Drawing.Color.Silver;
            this.lblClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblClose.Location = new System.Drawing.Point(300, 0);
            this.lblClose.Name = "lblClose";
            this.lblClose.Size = new System.Drawing.Size(20, 23);
            this.lblClose.TabIndex = 5;
            this.lblClose.Text = "X";
            this.lblClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblClose.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lblClose_MouseClick);
            // 
            // chkCoBan
            // 
            this.chkCoBan.AutoSize = true;
            this.chkCoBan.Location = new System.Drawing.Point(8, 104);
            this.chkCoBan.Name = "chkCoBan";
            this.chkCoBan.Size = new System.Drawing.Size(240, 17);
            this.chkCoBan.TabIndex = 6;
            this.chkCoBan.Text = "Làm nhiệm vụ cơ bản để lên cấp và lấy vàng";
            this.chkCoBan.UseVisualStyleBackColor = true;
            this.chkCoBan.CheckedChanged += new System.EventHandler(this.chkCoBan_CheckedChanged);
            // 
            // AlarmVaoPhai
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.chkCoBan);
            this.Controls.Add(this.lblClose);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboMenpai);
            this.Controls.Add(this.lblAlarm);
            this.Controls.Add(this.lblName);
            this.Name = "AlarmVaoPhai";
            this.Size = new System.Drawing.Size(320, 130);
            this.Load += new System.EventHandler(this.AlarmVaoPhai_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblAlarm;
        private System.Windows.Forms.ComboBox cboMenpai;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label lblClose;
        private System.Windows.Forms.CheckBox chkCoBan;
    }
}
