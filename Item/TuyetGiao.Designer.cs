namespace _i
{
    partial class TuyetGiao
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TuyetGiao));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tuyệtGiaoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.LvFriend = new _i.ListViewEx();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tuyệtGiaoToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(131, 26);
            // 
            // tuyệtGiaoToolStripMenuItem
            // 
            this.tuyệtGiaoToolStripMenuItem.Name = "tuyệtGiaoToolStripMenuItem";
            this.tuyệtGiaoToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.tuyệtGiaoToolStripMenuItem.Text = "Tuyệt Giao";
            this.tuyệtGiaoToolStripMenuItem.Click += new System.EventHandler(this.tuyệtGiaoToolStripMenuItem_Click);
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Top;
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(314, 20);
            this.button1.TabIndex = 1;
            this.button1.Text = "Close";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // LvFriend
            // 
            this.LvFriend.AllowColumnReorder = true;
            this.LvFriend.AllowDrop = true;
            this.LvFriend.AllowReorder = true;
            this.LvFriend.AllowSort = false;
            this.LvFriend.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.LvFriend.ContextMenuStrip = this.contextMenuStrip1;
            this.LvFriend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LvFriend.DoubleClickActivation = false;
            this.LvFriend.FullRowSelect = true;
            this.LvFriend.GridLines = true;
            this.LvFriend.hideItems = ((System.Collections.Generic.List<System.Windows.Forms.ListViewItem>)(resources.GetObject("LvFriend.hideItems")));
            this.LvFriend.HideSelection = false;
            this.LvFriend.LineColor = System.Drawing.Color.Red;
            this.LvFriend.Location = new System.Drawing.Point(0, 20);
            this.LvFriend.Name = "LvFriend";
            this.LvFriend.Size = new System.Drawing.Size(314, 379);
            this.LvFriend.TabIndex = 0;
            this.LvFriend.UseCompatibleStateImageBehavior = false;
            this.LvFriend.View = System.Windows.Forms.View.Details;
            this.LvFriend.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LvFriend_KeyDown);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Tên";
            this.columnHeader1.Width = 184;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Index";
            this.columnHeader2.Width = 74;
            // 
            // TuyetGiao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.LvFriend);
            this.Controls.Add(this.button1);
            this.Name = "TuyetGiao";
            this.Size = new System.Drawing.Size(314, 399);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ListViewEx LvFriend;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tuyệtGiaoToolStripMenuItem;
        private System.Windows.Forms.Button button1;
    }
}