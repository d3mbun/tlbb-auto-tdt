namespace _i
{
    partial class Publisher
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Publisher));
            this.listViewNPH = new _i.ListViewEx();
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.menuNPH = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuAddNPH = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEditNPH = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDeleteNPH = new System.Windows.Forms.ToolStripMenuItem();
            this.menuServer = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuAddServer = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEditServer = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDeleteServer = new System.Windows.Forms.ToolStripMenuItem();
            this.btnPath = new System.Windows.Forms.Button();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnReset = new System.Windows.Forms.Button();
            this.menuTail = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuAddTail = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEditTail = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDeleteTail = new System.Windows.Forms.ToolStripMenuItem();
            this.listViewServer = new _i.ListViewEx();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listViewTail = new _i.ListViewEx();
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.menuNPH.SuspendLayout();
            this.menuServer.SuspendLayout();
            this.menuTail.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listViewNPH
            // 
            this.listViewNPH.AllowColumnReorder = true;
            this.listViewNPH.AllowDrop = true;
            this.listViewNPH.AllowReorder = true;
            this.listViewNPH.AllowSort = true;
            this.listViewNPH.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5});
            this.listViewNPH.ContextMenuStrip = this.menuNPH;
            this.listViewNPH.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewNPH.DoubleClickActivation = false;
            this.listViewNPH.FullRowSelect = true;
            this.listViewNPH.GridLines = true;
            this.listViewNPH.hideItems = ((System.Collections.Generic.List<System.Windows.Forms.ListViewItem>)(resources.GetObject("listViewNPH.hideItems")));
            this.listViewNPH.HideSelection = false;
            this.listViewNPH.LineColor = System.Drawing.Color.Red;
            this.listViewNPH.Location = new System.Drawing.Point(0, 0);
            this.listViewNPH.Name = "listViewNPH";
            this.listViewNPH.Size = new System.Drawing.Size(144, 348);
            this.listViewNPH.TabIndex = 0;
            this.listViewNPH.UseCompatibleStateImageBehavior = false;
            this.listViewNPH.View = System.Windows.Forms.View.Details;
            this.listViewNPH.SelectedIndexChanged += new System.EventHandler(this.listViewNPH_SelectedIndexChanged);
            this.listViewNPH.DragDrop += new System.Windows.Forms.DragEventHandler(this.listViewNPH_DragDrop);
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Name";
            this.columnHeader5.Width = 119;
            // 
            // menuNPH
            // 
            this.menuNPH.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuAddNPH,
            this.menuEditNPH,
            this.menuDeleteNPH});
            this.menuNPH.Name = "menuNPH";
            this.menuNPH.Size = new System.Drawing.Size(108, 70);
            // 
            // menuAddNPH
            // 
            this.menuAddNPH.Name = "menuAddNPH";
            this.menuAddNPH.Size = new System.Drawing.Size(107, 22);
            this.menuAddNPH.Text = "Add";
            this.menuAddNPH.Click += new System.EventHandler(this.menuAddNPH_Click);
            // 
            // menuEditNPH
            // 
            this.menuEditNPH.Name = "menuEditNPH";
            this.menuEditNPH.Size = new System.Drawing.Size(107, 22);
            this.menuEditNPH.Text = "Edit";
            this.menuEditNPH.Click += new System.EventHandler(this.menuEditNPH_Click);
            // 
            // menuDeleteNPH
            // 
            this.menuDeleteNPH.Name = "menuDeleteNPH";
            this.menuDeleteNPH.Size = new System.Drawing.Size(107, 22);
            this.menuDeleteNPH.Text = "Delete";
            this.menuDeleteNPH.Click += new System.EventHandler(this.menuDeleteNPH_Click);
            // 
            // menuServer
            // 
            this.menuServer.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuAddServer,
            this.menuEditServer,
            this.menuDeleteServer});
            this.menuServer.Name = "menu";
            this.menuServer.Size = new System.Drawing.Size(108, 70);
            // 
            // menuAddServer
            // 
            this.menuAddServer.Name = "menuAddServer";
            this.menuAddServer.Size = new System.Drawing.Size(107, 22);
            this.menuAddServer.Text = "Add";
            this.menuAddServer.Click += new System.EventHandler(this.menuAddServer_Click);
            // 
            // menuEditServer
            // 
            this.menuEditServer.Name = "menuEditServer";
            this.menuEditServer.Size = new System.Drawing.Size(107, 22);
            this.menuEditServer.Text = "Edit";
            this.menuEditServer.Click += new System.EventHandler(this.menuEditServer_Click);
            // 
            // menuDeleteServer
            // 
            this.menuDeleteServer.Name = "menuDeleteServer";
            this.menuDeleteServer.Size = new System.Drawing.Size(107, 22);
            this.menuDeleteServer.Text = "Delete";
            this.menuDeleteServer.Click += new System.EventHandler(this.menuDeleteServer_Click);
            // 
            // btnPath
            // 
            this.btnPath.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnPath.Location = new System.Drawing.Point(247, 0);
            this.btnPath.Name = "btnPath";
            this.btnPath.Size = new System.Drawing.Size(33, 21);
            this.btnPath.TabIndex = 17;
            this.btnPath.Text = "...";
            this.btnPath.UseVisualStyleBackColor = true;
            this.btnPath.Click += new System.EventHandler(this.btnPath_Click);
            // 
            // txtPath
            // 
            this.txtPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPath.Location = new System.Drawing.Point(0, 0);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(247, 20);
            this.txtPath.TabIndex = 15;
            // 
            // label6
            // 
            this.label6.Dock = System.Windows.Forms.DockStyle.Top;
            this.label6.Location = new System.Drawing.Point(0, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(434, 20);
            this.label6.TabIndex = 16;
            this.label6.Text = "Cài Đặt Server";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnReset
            // 
            this.btnReset.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnReset.Location = new System.Drawing.Point(0, 325);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(286, 23);
            this.btnReset.TabIndex = 21;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // menuTail
            // 
            this.menuTail.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuAddTail,
            this.menuEditTail,
            this.menuDeleteTail});
            this.menuTail.Name = "menuNPH";
            this.menuTail.Size = new System.Drawing.Size(108, 70);
            // 
            // menuAddTail
            // 
            this.menuAddTail.Name = "menuAddTail";
            this.menuAddTail.Size = new System.Drawing.Size(107, 22);
            this.menuAddTail.Text = "Add";
            this.menuAddTail.Click += new System.EventHandler(this.menuAddTail_Click);
            // 
            // menuEditTail
            // 
            this.menuEditTail.Name = "menuEditTail";
            this.menuEditTail.Size = new System.Drawing.Size(107, 22);
            this.menuEditTail.Text = "Edit";
            this.menuEditTail.Click += new System.EventHandler(this.menuEditTail_Click);
            // 
            // menuDeleteTail
            // 
            this.menuDeleteTail.Name = "menuDeleteTail";
            this.menuDeleteTail.Size = new System.Drawing.Size(107, 22);
            this.menuDeleteTail.Text = "Delete";
            this.menuDeleteTail.Click += new System.EventHandler(this.menuDeleteTail_Click);
            // 
            // listViewServer
            // 
            this.listViewServer.AllowColumnReorder = true;
            this.listViewServer.AllowDrop = true;
            this.listViewServer.AllowReorder = true;
            this.listViewServer.AllowSort = true;
            this.listViewServer.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader4});
            this.listViewServer.ContextMenuStrip = this.menuServer;
            this.listViewServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewServer.DoubleClickActivation = false;
            this.listViewServer.FullRowSelect = true;
            this.listViewServer.GridLines = true;
            this.listViewServer.hideItems = ((System.Collections.Generic.List<System.Windows.Forms.ListViewItem>)(resources.GetObject("listViewServer.hideItems")));
            this.listViewServer.HideSelection = false;
            this.listViewServer.LineColor = System.Drawing.Color.Red;
            this.listViewServer.Location = new System.Drawing.Point(0, 40);
            this.listViewServer.Name = "listViewServer";
            this.listViewServer.Size = new System.Drawing.Size(286, 144);
            this.listViewServer.TabIndex = 18;
            this.listViewServer.UseCompatibleStateImageBehavior = false;
            this.listViewServer.View = System.Windows.Forms.View.Details;
            this.listViewServer.DragDrop += new System.Windows.Forms.DragEventHandler(this.listViewServer_DragDrop);
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Index";
            this.columnHeader2.Width = 40;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Server";
            this.columnHeader4.Width = 159;
            // 
            // listViewTail
            // 
            this.listViewTail.AllowColumnReorder = true;
            this.listViewTail.AllowDrop = true;
            this.listViewTail.AllowReorder = true;
            this.listViewTail.AllowSort = true;
            this.listViewTail.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader6,
            this.columnHeader7});
            this.listViewTail.ContextMenuStrip = this.menuTail;
            this.listViewTail.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.listViewTail.DoubleClickActivation = false;
            this.listViewTail.FullRowSelect = true;
            this.listViewTail.GridLines = true;
            this.listViewTail.hideItems = ((System.Collections.Generic.List<System.Windows.Forms.ListViewItem>)(resources.GetObject("listViewTail.hideItems")));
            this.listViewTail.HideSelection = false;
            this.listViewTail.LineColor = System.Drawing.Color.Red;
            this.listViewTail.Location = new System.Drawing.Point(0, 184);
            this.listViewTail.Name = "listViewTail";
            this.listViewTail.Size = new System.Drawing.Size(286, 141);
            this.listViewTail.TabIndex = 19;
            this.listViewTail.UseCompatibleStateImageBehavior = false;
            this.listViewTail.View = System.Windows.Forms.View.Details;
            this.listViewTail.DragDrop += new System.Windows.Forms.DragEventHandler(this.listViewTail_DragDrop);
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Index";
            this.columnHeader6.Width = 40;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Tail";
            this.columnHeader7.Width = 161;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 20);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listViewNPH);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.listViewServer);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel2.Controls.Add(this.listViewTail);
            this.splitContainer1.Panel2.Controls.Add(this.btnReset);
            this.splitContainer1.Size = new System.Drawing.Size(434, 348);
            this.splitContainer1.SplitterDistance = 144;
            this.splitContainer1.TabIndex = 22;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(286, 40);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Path";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtPath);
            this.panel1.Controls.Add(this.btnPath);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 16);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(280, 21);
            this.panel1.TabIndex = 23;
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button1.Location = new System.Drawing.Point(0, 368);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(434, 23);
            this.button1.TabIndex = 23;
            this.button1.Text = "Ok";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Publisher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label6);
            this.Name = "Publisher";
            this.Size = new System.Drawing.Size(434, 391);
            this.Load += new System.EventHandler(this.Publisher_Load);
            this.menuNPH.ResumeLayout(false);
            this.menuServer.ResumeLayout(false);
            this.menuTail.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ListViewEx listViewNPH;
        private System.Windows.Forms.Button btnPath;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Label label6;
        private ListViewEx listViewServer;
        private ListViewEx listViewTail;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ContextMenuStrip menuServer;
        private System.Windows.Forms.ToolStripMenuItem menuAddServer;
        private System.Windows.Forms.ToolStripMenuItem menuEditServer;
        private System.Windows.Forms.ToolStripMenuItem menuDeleteServer;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.ContextMenuStrip menuNPH;
        private System.Windows.Forms.ToolStripMenuItem menuAddNPH;
        private System.Windows.Forms.ToolStripMenuItem menuEditNPH;
        private System.Windows.Forms.ToolStripMenuItem menuDeleteNPH;
        private System.Windows.Forms.ContextMenuStrip menuTail;
        private System.Windows.Forms.ToolStripMenuItem menuAddTail;
        private System.Windows.Forms.ToolStripMenuItem menuEditTail;
        private System.Windows.Forms.ToolStripMenuItem menuDeleteTail;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
    }
}