namespace _i
{
    partial class CalendarEx
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CalendarEx));
            this.menuDelete = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listViewTeamMem = new _i.ListViewEx();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnAddTeam = new System.Windows.Forms.Button();
            this.listViewBallMem = new _i.ListViewEx();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.menuDeleteBallMem = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteBallMem = new System.Windows.Forms.ToolStripMenuItem();
            this.listViewBallID = new _i.ListViewEx();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.menuDeleteBall = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteBall = new System.Windows.Forms.ToolStripMenuItem();
            this.btnAllBallName = new System.Windows.Forms.Button();
            this.txtTeam = new ChreneLib.Controls.TextBoxes.TextBoxEx();
            this.txtBallName = new ChreneLib.Controls.TextBoxes.TextBoxEx();
            this.tabControlEx1 = new _i.TabControlEx();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lvTeamName = new _i.ListViewEx();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cboPlayer = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.lvTyVoId = new _i.ListViewEx();
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.menuDeleteTyVo = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.lvTyVoMem = new _i.ListViewEx();
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.lvQuanDoan = new _i.ListViewEx();
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.menuDeleteQuanDoan = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.lvQuanDoanMem = new _i.ListViewEx();
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.menuDelete.SuspendLayout();
            this.menuDeleteBallMem.SuspendLayout();
            this.menuDeleteBall.SuspendLayout();
            this.tabControlEx1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.menuDeleteTyVo.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.menuDeleteQuanDoan.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuDelete
            // 
            this.menuDelete.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem});
            this.menuDelete.Name = "menuDelete";
            this.menuDelete.Size = new System.Drawing.Size(108, 26);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // listViewTeamMem
            // 
            this.listViewTeamMem.AllowColumnReorder = true;
            this.listViewTeamMem.AllowDrop = true;
            this.listViewTeamMem.AllowReorder = true;
            this.listViewTeamMem.AllowSort = false;
            this.listViewTeamMem.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
            this.listViewTeamMem.ContextMenuStrip = this.menuDelete;
            this.listViewTeamMem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewTeamMem.DoubleClickActivation = false;
            this.listViewTeamMem.FullRowSelect = true;
            this.listViewTeamMem.GridLines = true;
            this.listViewTeamMem.hideItems = ((System.Collections.Generic.List<System.Windows.Forms.ListViewItem>)(resources.GetObject("listViewTeamMem.hideItems")));
            this.listViewTeamMem.HideSelection = false;
            this.listViewTeamMem.LineColor = System.Drawing.Color.Red;
            this.listViewTeamMem.Location = new System.Drawing.Point(0, 0);
            this.listViewTeamMem.Name = "listViewTeamMem";
            this.listViewTeamMem.Size = new System.Drawing.Size(264, 297);
            this.listViewTeamMem.TabIndex = 1;
            this.listViewTeamMem.UseCompatibleStateImageBehavior = false;
            this.listViewTeamMem.View = System.Windows.Forms.View.Details;
            
            this.listViewTeamMem.DoubleClick += new System.EventHandler(this.listViewTeamMem_DoubleClick);
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Member";
            this.columnHeader2.Width = 160;
            // 
            // btnAddTeam
            // 
            this.btnAddTeam.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnAddTeam.Location = new System.Drawing.Point(0, 318);
            this.btnAddTeam.Name = "btnAddTeam";
            this.btnAddTeam.Size = new System.Drawing.Size(257, 23);
            this.btnAddTeam.TabIndex = 4;
            this.btnAddTeam.Text = "Add";
            this.btnAddTeam.UseVisualStyleBackColor = true;
            this.btnAddTeam.Click += new System.EventHandler(this.btnAddTeam_Click);
            // 
            // listViewBallMem
            // 
            this.listViewBallMem.AllowColumnReorder = true;
            this.listViewBallMem.AllowDrop = true;
            this.listViewBallMem.AllowReorder = true;
            this.listViewBallMem.AllowSort = false;
            this.listViewBallMem.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3});
            this.listViewBallMem.ContextMenuStrip = this.menuDeleteBallMem;
            this.listViewBallMem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewBallMem.DoubleClickActivation = false;
            this.listViewBallMem.FullRowSelect = true;
            this.listViewBallMem.GridLines = true;
            this.listViewBallMem.hideItems = ((System.Collections.Generic.List<System.Windows.Forms.ListViewItem>)(resources.GetObject("listViewBallMem.hideItems")));
            this.listViewBallMem.HideSelection = false;
            this.listViewBallMem.LineColor = System.Drawing.Color.Red;
            this.listViewBallMem.Location = new System.Drawing.Point(0, 0);
            this.listViewBallMem.Name = "listViewBallMem";
            this.listViewBallMem.Size = new System.Drawing.Size(263, 341);
            this.listViewBallMem.TabIndex = 9;
            this.listViewBallMem.UseCompatibleStateImageBehavior = false;
            this.listViewBallMem.View = System.Windows.Forms.View.Details;
            
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Ball Mem [2]";
            this.columnHeader3.Width = 160;
            // 
            // menuDeleteBallMem
            // 
            this.menuDeleteBallMem.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteBallMem});
            this.menuDeleteBallMem.Name = "menuDeleteTeam";
            this.menuDeleteBallMem.Size = new System.Drawing.Size(108, 26);
            // 
            // deleteBallMem
            // 
            this.deleteBallMem.Name = "deleteBallMem";
            this.deleteBallMem.Size = new System.Drawing.Size(107, 22);
            this.deleteBallMem.Text = "Delete";
            this.deleteBallMem.Click += new System.EventHandler(this.deleteBallMem_Click);
            // 
            // listViewBallID
            // 
            this.listViewBallID.AllowColumnReorder = true;
            this.listViewBallID.AllowDrop = true;
            this.listViewBallID.AllowReorder = true;
            this.listViewBallID.AllowSort = false;
            this.listViewBallID.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4});
            this.listViewBallID.ContextMenuStrip = this.menuDeleteBall;
            this.listViewBallID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewBallID.DoubleClickActivation = false;
            this.listViewBallID.FullRowSelect = true;
            this.listViewBallID.GridLines = true;
            this.listViewBallID.hideItems = ((System.Collections.Generic.List<System.Windows.Forms.ListViewItem>)(resources.GetObject("listViewBallID.hideItems")));
            this.listViewBallID.HideSelection = false;
            this.listViewBallID.LineColor = System.Drawing.Color.Red;
            this.listViewBallID.Location = new System.Drawing.Point(0, 0);
            this.listViewBallID.Name = "listViewBallID";
            this.listViewBallID.Size = new System.Drawing.Size(258, 298);
            this.listViewBallID.TabIndex = 8;
            this.listViewBallID.UseCompatibleStateImageBehavior = false;
            this.listViewBallID.View = System.Windows.Forms.View.Details;
            
            this.listViewBallID.SelectedIndexChanged += new System.EventHandler(this.listViewBallID_SelectedIndexChanged);
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Ball ID";
            this.columnHeader4.Width = 160;
            // 
            // menuDeleteBall
            // 
            this.menuDeleteBall.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteBall});
            this.menuDeleteBall.Name = "menuDeleteTeam";
            this.menuDeleteBall.Size = new System.Drawing.Size(108, 26);
            // 
            // deleteBall
            // 
            this.deleteBall.Name = "deleteBall";
            this.deleteBall.Size = new System.Drawing.Size(107, 22);
            this.deleteBall.Text = "Delete";
            this.deleteBall.Click += new System.EventHandler(this.deleteBall_Click);
            // 
            // btnAllBallName
            // 
            this.btnAllBallName.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnAllBallName.Location = new System.Drawing.Point(0, 318);
            this.btnAllBallName.Name = "btnAllBallName";
            this.btnAllBallName.Size = new System.Drawing.Size(258, 23);
            this.btnAllBallName.TabIndex = 11;
            this.btnAllBallName.Text = "Add";
            this.btnAllBallName.UseVisualStyleBackColor = true;
            this.btnAllBallName.Click += new System.EventHandler(this.btnAllBallName_Click);
            // 
            // txtTeam
            // 
            this.txtTeam.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtTeam.Location = new System.Drawing.Point(0, 298);
            this.txtTeam.Name = "txtTeam";
            this.txtTeam.Size = new System.Drawing.Size(257, 20);
            this.txtTeam.TabIndex = 13;
            this.txtTeam.WaterMark = "Tên Nhóm";
            this.txtTeam.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtTeam.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTeam.WaterMarkForeColor = System.Drawing.Color.LightGray;
            // 
            // txtBallName
            // 
            this.txtBallName.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtBallName.Location = new System.Drawing.Point(0, 298);
            this.txtBallName.Name = "txtBallName";
            this.txtBallName.Size = new System.Drawing.Size(258, 20);
            this.txtBallName.TabIndex = 14;
            this.txtBallName.WaterMark = "Tên Nhóm Kết Bái Nhận Bóng";
            this.txtBallName.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtBallName.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBallName.WaterMarkForeColor = System.Drawing.Color.LightGray;
            // 
            // tabControlEx1
            // 
            this.tabControlEx1.Alignment = System.Windows.Forms.TabAlignment.Top;
            this.tabControlEx1.Controls.Add(this.tabPage1);
            this.tabControlEx1.Controls.Add(this.tabPage2);
            this.tabControlEx1.Controls.Add(this.tabPage3);
            this.tabControlEx1.Controls.Add(this.tabPage4);
            this.tabControlEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlEx1.Location = new System.Drawing.Point(0, 0);
            this.tabControlEx1.Name = "tabControlEx1";
            this.tabControlEx1.SelectedIndex = 0;
            this.tabControlEx1.Size = new System.Drawing.Size(539, 373);
            this.tabControlEx1.TabIndex = 16;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(531, 347);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Đội Ngũ";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lvTeamName);
            this.splitContainer1.Panel1.Controls.Add(this.txtTeam);
            this.splitContainer1.Panel1.Controls.Add(this.btnAddTeam);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.listViewTeamMem);
            this.splitContainer1.Panel2.Controls.Add(this.cboPlayer);
            this.splitContainer1.Panel2.Controls.Add(this.button1);
            this.splitContainer1.Size = new System.Drawing.Size(525, 341);
            this.splitContainer1.SplitterDistance = 257;
            this.splitContainer1.TabIndex = 0;
            // 
            // lvTeamName
            // 
            this.lvTeamName.AllowColumnReorder = true;
            this.lvTeamName.AllowDrop = true;
            this.lvTeamName.AllowReorder = true;
            this.lvTeamName.AllowSort = false;
            this.lvTeamName.CheckBoxes = true;
            this.lvTeamName.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lvTeamName.ContextMenuStrip = this.menuDelete;
            this.lvTeamName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvTeamName.DoubleClickActivation = false;
            this.lvTeamName.FullRowSelect = true;
            this.lvTeamName.GridLines = true;
            this.lvTeamName.hideItems = ((System.Collections.Generic.List<System.Windows.Forms.ListViewItem>)(resources.GetObject("lvTeamName.hideItems")));
            this.lvTeamName.HideSelection = false;
            this.lvTeamName.LineColor = System.Drawing.Color.Red;
            this.lvTeamName.Location = new System.Drawing.Point(0, 0);
            this.lvTeamName.Name = "lvTeamName";
            this.lvTeamName.Size = new System.Drawing.Size(257, 298);
            this.lvTeamName.TabIndex = 14;
            this.lvTeamName.UseCompatibleStateImageBehavior = false;
            this.lvTeamName.View = System.Windows.Forms.View.Details;
            
            this.lvTeamName.VirtualMode = true;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Member";
            this.columnHeader1.Width = 160;
            // 
            // cboPlayer
            // 
            this.cboPlayer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.cboPlayer.FormattingEnabled = true;
            this.cboPlayer.Location = new System.Drawing.Point(0, 297);
            this.cboPlayer.Name = "cboPlayer";
            this.cboPlayer.Size = new System.Drawing.Size(264, 21);
            this.cboPlayer.TabIndex = 15;
            this.cboPlayer.DropDown += new System.EventHandler(this.cboSearchName_DropDown);
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button1.Location = new System.Drawing.Point(0, 318);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(264, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Add";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.splitContainer2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(531, 347);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Nhận Bóng";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(3, 3);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.listViewBallID);
            this.splitContainer2.Panel1.Controls.Add(this.txtBallName);
            this.splitContainer2.Panel1.Controls.Add(this.btnAllBallName);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.listViewBallMem);
            this.splitContainer2.Size = new System.Drawing.Size(525, 341);
            this.splitContainer2.SplitterDistance = 258;
            this.splitContainer2.TabIndex = 1;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.splitContainer3);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(531, 347);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Tỷ Võ";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(3, 3);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.lvTyVoId);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.lvTyVoMem);
            this.splitContainer3.Size = new System.Drawing.Size(525, 341);
            this.splitContainer3.SplitterDistance = 258;
            this.splitContainer3.TabIndex = 2;
            // 
            // lvTyVoId
            // 
            this.lvTyVoId.AllowColumnReorder = true;
            this.lvTyVoId.AllowDrop = true;
            this.lvTyVoId.AllowReorder = true;
            this.lvTyVoId.AllowSort = false;
            this.lvTyVoId.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5});
            this.lvTyVoId.ContextMenuStrip = this.menuDeleteTyVo;
            this.lvTyVoId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvTyVoId.DoubleClickActivation = false;
            this.lvTyVoId.FullRowSelect = true;
            this.lvTyVoId.GridLines = true;
            this.lvTyVoId.hideItems = ((System.Collections.Generic.List<System.Windows.Forms.ListViewItem>)(resources.GetObject("lvTyVoId.hideItems")));
            this.lvTyVoId.HideSelection = false;
            this.lvTyVoId.LineColor = System.Drawing.Color.Red;
            this.lvTyVoId.Location = new System.Drawing.Point(0, 0);
            this.lvTyVoId.Name = "lvTyVoId";
            this.lvTyVoId.Size = new System.Drawing.Size(258, 341);
            this.lvTyVoId.TabIndex = 8;
            this.lvTyVoId.UseCompatibleStateImageBehavior = false;
            this.lvTyVoId.View = System.Windows.Forms.View.Details;
            
            this.lvTyVoId.SelectedIndexChanged += new System.EventHandler(this.lvTyVoId_SelectedIndexChanged);
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Tỷ Võ ID";
            this.columnHeader5.Width = 160;
            // 
            // menuDeleteTyVo
            // 
            this.menuDeleteTyVo.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.menuDeleteTyVo.Name = "menuDeleteTeam";
            this.menuDeleteTyVo.Size = new System.Drawing.Size(108, 26);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(107, 22);
            this.toolStripMenuItem1.Text = "Delete";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // lvTyVoMem
            // 
            this.lvTyVoMem.AllowColumnReorder = true;
            this.lvTyVoMem.AllowDrop = true;
            this.lvTyVoMem.AllowReorder = true;
            this.lvTyVoMem.AllowSort = false;
            this.lvTyVoMem.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader6});
            this.lvTyVoMem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvTyVoMem.DoubleClickActivation = false;
            this.lvTyVoMem.FullRowSelect = true;
            this.lvTyVoMem.GridLines = true;
            this.lvTyVoMem.hideItems = ((System.Collections.Generic.List<System.Windows.Forms.ListViewItem>)(resources.GetObject("lvTyVoMem.hideItems")));
            this.lvTyVoMem.HideSelection = false;
            this.lvTyVoMem.LineColor = System.Drawing.Color.Red;
            this.lvTyVoMem.Location = new System.Drawing.Point(0, 0);
            this.lvTyVoMem.Name = "lvTyVoMem";
            this.lvTyVoMem.Size = new System.Drawing.Size(263, 341);
            this.lvTyVoMem.TabIndex = 9;
            this.lvTyVoMem.UseCompatibleStateImageBehavior = false;
            this.lvTyVoMem.View = System.Windows.Forms.View.Details;
            
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Tỷ Võ Mem [3]";
            this.columnHeader6.Width = 160;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.splitContainer4);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(531, 347);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Quân Đoàn";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(3, 3);
            this.splitContainer4.Name = "splitContainer4";
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.lvQuanDoan);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.lvQuanDoanMem);
            this.splitContainer4.Size = new System.Drawing.Size(525, 341);
            this.splitContainer4.SplitterDistance = 258;
            this.splitContainer4.TabIndex = 3;
            // 
            // lvQuanDoan
            // 
            this.lvQuanDoan.AllowColumnReorder = true;
            this.lvQuanDoan.AllowDrop = true;
            this.lvQuanDoan.AllowReorder = true;
            this.lvQuanDoan.AllowSort = false;
            this.lvQuanDoan.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader7});
            this.lvQuanDoan.ContextMenuStrip = this.menuDeleteQuanDoan;
            this.lvQuanDoan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvQuanDoan.DoubleClickActivation = false;
            this.lvQuanDoan.FullRowSelect = true;
            this.lvQuanDoan.GridLines = true;
            this.lvQuanDoan.hideItems = ((System.Collections.Generic.List<System.Windows.Forms.ListViewItem>)(resources.GetObject("lvQuanDoan.hideItems")));
            this.lvQuanDoan.HideSelection = false;
            this.lvQuanDoan.LineColor = System.Drawing.Color.Red;
            this.lvQuanDoan.Location = new System.Drawing.Point(0, 0);
            this.lvQuanDoan.Name = "lvQuanDoan";
            this.lvQuanDoan.Size = new System.Drawing.Size(258, 341);
            this.lvQuanDoan.TabIndex = 8;
            this.lvQuanDoan.UseCompatibleStateImageBehavior = false;
            this.lvQuanDoan.View = System.Windows.Forms.View.Details;
            
            this.lvQuanDoan.SelectedIndexChanged += new System.EventHandler(this.lvQuanDoan_SelectedIndexChanged);
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Quân Đoàn ID";
            this.columnHeader7.Width = 160;
            // 
            // menuDeleteQuanDoan
            // 
            this.menuDeleteQuanDoan.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2});
            this.menuDeleteQuanDoan.Name = "menuDeleteTeam";
            this.menuDeleteQuanDoan.Size = new System.Drawing.Size(108, 26);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(107, 22);
            this.toolStripMenuItem2.Text = "Delete";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // lvQuanDoanMem
            // 
            this.lvQuanDoanMem.AllowColumnReorder = true;
            this.lvQuanDoanMem.AllowDrop = true;
            this.lvQuanDoanMem.AllowReorder = true;
            this.lvQuanDoanMem.AllowSort = false;
            this.lvQuanDoanMem.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader8});
            this.lvQuanDoanMem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvQuanDoanMem.DoubleClickActivation = false;
            this.lvQuanDoanMem.FullRowSelect = true;
            this.lvQuanDoanMem.GridLines = true;
            this.lvQuanDoanMem.hideItems = ((System.Collections.Generic.List<System.Windows.Forms.ListViewItem>)(resources.GetObject("lvQuanDoanMem.hideItems")));
            this.lvQuanDoanMem.HideSelection = false;
            this.lvQuanDoanMem.LineColor = System.Drawing.Color.Red;
            this.lvQuanDoanMem.Location = new System.Drawing.Point(0, 0);
            this.lvQuanDoanMem.Name = "lvQuanDoanMem";
            this.lvQuanDoanMem.Size = new System.Drawing.Size(263, 341);
            this.lvQuanDoanMem.TabIndex = 9;
            this.lvQuanDoanMem.UseCompatibleStateImageBehavior = false;
            this.lvQuanDoanMem.View = System.Windows.Forms.View.Details;
            
            this.lvQuanDoanMem.DragDrop += new System.Windows.Forms.DragEventHandler(this.lvQuanDoanMem_DragDrop);
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Quân Đoàn Mem";
            this.columnHeader8.Width = 160;
            // 
            // CalendarEx
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControlEx1);
            this.Name = "CalendarEx";
            this.Size = new System.Drawing.Size(539, 373);
            this.Load += new System.EventHandler(this.CalenderEx_Load);
            this.menuDelete.ResumeLayout(false);
            this.menuDeleteBallMem.ResumeLayout(false);
            this.menuDeleteBall.ResumeLayout(false);
            this.tabControlEx1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.ResumeLayout(false);
            this.menuDeleteTyVo.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            this.splitContainer4.ResumeLayout(false);
            this.menuDeleteQuanDoan.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private ListViewEx listViewTeamMem;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button btnAddTeam;
        private ListViewEx listViewBallMem;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private ListViewEx listViewBallID;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Button btnAllBallName;
        private System.Windows.Forms.ContextMenuStrip menuDeleteBall;
        private System.Windows.Forms.ToolStripMenuItem deleteBall;
        private System.Windows.Forms.ContextMenuStrip menuDeleteBallMem;
        private System.Windows.Forms.ToolStripMenuItem deleteBallMem;
        private ChreneLib.Controls.TextBoxes.TextBoxEx txtTeam;
        private ChreneLib.Controls.TextBoxes.TextBoxEx txtBallName;
        private TabControlEx tabControlEx1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private ListViewEx lvTyVoId;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private ListViewEx lvTyVoMem;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ContextMenuStrip menuDeleteTyVo;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private ListViewEx lvQuanDoan;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private ListViewEx lvQuanDoanMem;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ContextMenuStrip menuDeleteQuanDoan;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox cboPlayer;
        private System.Windows.Forms.ContextMenuStrip menuDelete;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private ListViewEx lvTeamName;
        private System.Windows.Forms.ColumnHeader columnHeader1;
    }
}