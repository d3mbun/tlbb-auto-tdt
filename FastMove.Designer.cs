namespace _i
{
    partial class FastMove
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
            this.textBoxEx1 = new ChreneLib.Controls.TextBoxes.TextBoxEx();
            this.listViewEx1 = new _i.ListViewEx();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.mnu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.diChuyểnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.mnu.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxEx1
            // 
            this.textBoxEx1.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxEx1.Location = new System.Drawing.Point(0, 0);
            this.textBoxEx1.Name = "textBoxEx1";
            this.textBoxEx1.Size = new System.Drawing.Size(328, 20);
            this.textBoxEx1.TabIndex = 0;
            this.textBoxEx1.WaterMark = "Search...";
            this.textBoxEx1.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.textBoxEx1.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxEx1.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.textBoxEx1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxEx1_KeyDown);
            // 
            // listViewEx1
            // 
            this.listViewEx1.AllowColumnReorder = true;
            this.listViewEx1.AllowDrop = true;
            this.listViewEx1.AllowReorder = true;
            this.listViewEx1.AllowSort = false;
            this.listViewEx1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listViewEx1.ContextMenuStrip = this.mnu;
            this.listViewEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewEx1.FullRowSelect = true;
            this.listViewEx1.GridLines = true;
            this.listViewEx1.HideSelection = false;
            this.listViewEx1.LineColor = System.Drawing.Color.Red;
            this.listViewEx1.Location = new System.Drawing.Point(0, 20);
            this.listViewEx1.Name = "listViewEx1";
            this.listViewEx1.Size = new System.Drawing.Size(328, 325);
            this.listViewEx1.TabIndex = 1;
            this.listViewEx1.UseCompatibleStateImageBehavior = false;
            this.listViewEx1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Map";
            this.columnHeader1.Width = 245;
            // 
            // mnu
            // 
            this.mnu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.diChuyểnToolStripMenuItem});
            this.mnu.Name = "mnu";
            this.mnu.Size = new System.Drawing.Size(130, 26);
            // 
            // diChuyểnToolStripMenuItem
            // 
            this.diChuyểnToolStripMenuItem.Name = "diChuyểnToolStripMenuItem";
            this.diChuyểnToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.diChuyểnToolStripMenuItem.Text = "Di Chuyển";
            this.diChuyểnToolStripMenuItem.Click += new System.EventHandler(this.diChuyểnToolStripMenuItem_Click);
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "ID";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // FastMove
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(328, 345);
            this.Controls.Add(this.listViewEx1);
            this.Controls.Add(this.textBoxEx1);
            this.Name = "FastMove";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Đại Thế Giới";
            this.Load += new System.EventHandler(this.FastMove_Load);
            this.mnu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ChreneLib.Controls.TextBoxes.TextBoxEx textBoxEx1;
        private ListViewEx listViewEx1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ContextMenuStrip mnu;
        private System.Windows.Forms.ToolStripMenuItem diChuyểnToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Timer timer1;
    }
}