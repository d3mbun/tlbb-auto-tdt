namespace _i
{
    partial class BoQuaEx
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
            this.txtBoQua = new ChreneLib.Controls.TextBoxes.TextBoxEx();
            this.SuspendLayout();
            // 
            // txtBoQua
            // 
            this.txtBoQua.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoQua.Location = new System.Drawing.Point(0, 0);
            this.txtBoQua.Multiline = true;
            this.txtBoQua.Name = "txtBoQua";
            this.txtBoQua.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBoQua.Size = new System.Drawing.Size(381, 421);
            this.txtBoQua.TabIndex = 6;
            this.txtBoQua.WaterMark = "";
            this.txtBoQua.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtBoQua.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoQua.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtBoQua.TextChanged += new System.EventHandler(this.txtBoQua_TextChanged);
            // 
            // BoQuaEx
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(381, 421);
            this.Controls.Add(this.txtBoQua);
            this.Name = "BoQuaEx";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tự Động Chặn Khi Có Nội Dung";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BoQuaEx_FormClosing);
            this.Load += new System.EventHandler(this.BoQua_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ChreneLib.Controls.TextBoxes.TextBoxEx txtBoQua;
    }
}