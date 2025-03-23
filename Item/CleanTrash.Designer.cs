namespace _i
{
    partial class CleanTrash
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CleanTrash));
            this.lvItems = new _i.ListViewEx();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.hủyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyĐểDánVàoDanhSáchHủyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listViewEx1
            // 
            this.lvItems.AllowColumnReorder = true;
            this.lvItems.AllowDrop = true;
            this.lvItems.AllowReorder = true;
            this.lvItems.AllowSort = false;
            this.lvItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lvItems.ContextMenuStrip = this.contextMenuStrip1;
            this.lvItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvItems.DoubleClickActivation = false;
            this.lvItems.FullRowSelect = true;
            this.lvItems.GridLines = true;
            this.lvItems.hideItems = ((System.Collections.Generic.List<System.Windows.Forms.ListViewItem>)(resources.GetObject("listViewEx1.hideItems")));
            this.lvItems.HideSelection = false;
            this.lvItems.LineColor = System.Drawing.Color.Red;
            this.lvItems.Location = new System.Drawing.Point(0, 0);
            this.lvItems.Name = "listViewEx1";
            this.lvItems.Size = new System.Drawing.Size(498, 457);
            this.lvItems.TabIndex = 2;
            this.lvItems.UseCompatibleStateImageBehavior = false;
            this.lvItems.View = System.Windows.Forms.View.Details;
            this.lvItems.DoubleClick += new System.EventHandler(this.listViewEx1_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Player";
            this.columnHeader1.Width = 108;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Name";
            this.columnHeader2.Width = 168;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Lý Do";
            this.columnHeader3.Width = 178;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hủyToolStripMenuItem,
            this.copyĐểDánVàoDanhSáchHủyToolStripMenuItem,
            this.refreshToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(253, 70);
            // 
            // hủyToolStripMenuItem
            // 
            this.hủyToolStripMenuItem.Name = "hủyToolStripMenuItem";
            this.hủyToolStripMenuItem.Size = new System.Drawing.Size(252, 22);
            this.hủyToolStripMenuItem.Text = "Hủy";
            this.hủyToolStripMenuItem.Click += new System.EventHandler(this.hủyToolStripMenuItem_Click);
            // 
            // copyĐểDánVàoDanhSáchHủyToolStripMenuItem
            // 
            this.copyĐểDánVàoDanhSáchHủyToolStripMenuItem.Name = "copyĐểDánVàoDanhSáchHủyToolStripMenuItem";
            this.copyĐểDánVàoDanhSáchHủyToolStripMenuItem.Size = new System.Drawing.Size(252, 22);
            this.copyĐểDánVàoDanhSáchHủyToolStripMenuItem.Text = "Copy (Để dán vào danh sách hủy)";
            this.copyĐểDánVàoDanhSáchHủyToolStripMenuItem.Click += new System.EventHandler(this.copyĐểDánVàoDanhSáchHủyToolStripMenuItem_Click);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(252, 22);
            this.refreshToolStripMenuItem.Text = "Refresh";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button1.Location = new System.Drawing.Point(0, 457);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(498, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Dọn";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // CleanTras
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 480);
            this.Controls.Add(this.lvItems);
            this.Controls.Add(this.button1);
            this.Name = "CleanTras";
            this.Text = "Dọn Rác";
            this.Load += new System.EventHandler(this.CleanTras_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private ListViewEx lvItems;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem hủyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyĐểDánVàoDanhSáchHủyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.Button button1;
    }
}