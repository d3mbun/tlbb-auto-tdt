namespace _i
{
    partial class ConfigBossMap
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigBossMap));
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.menuDelete = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.xóaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.txtOnlyBossMap = new ChreneLib.Controls.TextBoxes.TextBoxEx();
            this.listviewmonter = new _i.ListViewEx();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lstv = new _i.ListViewEx();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel3 = new System.Windows.Forms.Panel();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.textBoxEx2 = new ChreneLib.Controls.TextBoxes.TextBoxEx();
            this.chkVeLacDuong = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.menuDelete.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.checkBox1.Location = new System.Drawing.Point(0, 5);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.checkBox1.Size = new System.Drawing.Size(227, 17);
            this.checkBox1.TabIndex = 10;
            this.checkBox1.Text = "Chạy Thử";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // btnSave
            // 
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnSave.Location = new System.Drawing.Point(0, 45);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(227, 23);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnAdd.Location = new System.Drawing.Point(0, 22);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(227, 23);
            this.btnAdd.TabIndex = 7;
            this.btnAdd.Text = "Thêm Tọa Độ Hiện Tại - Add point";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // menuDelete
            // 
            this.menuDelete.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.xóaToolStripMenuItem});
            this.menuDelete.Name = "mnuDelete";
            this.menuDelete.Size = new System.Drawing.Size(95, 26);
            // 
            // xóaToolStripMenuItem
            // 
            this.xóaToolStripMenuItem.Name = "xóaToolStripMenuItem";
            this.xóaToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.xóaToolStripMenuItem.Text = "Xóa";
            this.xóaToolStripMenuItem.Click += new System.EventHandler(this.xóaToolStripMenuItem_Click);
            // 
            // txtOnlyBossMap
            // 
            this.txtOnlyBossMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtOnlyBossMap.Location = new System.Drawing.Point(0, 0);
            this.txtOnlyBossMap.Multiline = true;
            this.txtOnlyBossMap.Name = "txtOnlyBossMap";
            this.txtOnlyBossMap.Size = new System.Drawing.Size(276, 197);
            this.txtOnlyBossMap.TabIndex = 13;
            this.txtOnlyBossMap.WaterMark = "Danh Sách Quái...";
            this.txtOnlyBossMap.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtOnlyBossMap.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOnlyBossMap.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtOnlyBossMap.TextChanged += new System.EventHandler(this.txtOnlyBossMap_TextChanged);
            // 
            // listviewmonter
            // 
            this.listviewmonter.AllowColumnReorder = true;
            this.listviewmonter.AllowDrop = true;
            this.listviewmonter.AllowReorder = true;
            this.listviewmonter.AllowSort = false;
            this.listviewmonter.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
            this.listviewmonter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listviewmonter.DoubleClickActivation = false;
            this.listviewmonter.FullRowSelect = true;
            this.listviewmonter.GridLines = true;
            this.listviewmonter.hideItems = ((System.Collections.Generic.List<System.Windows.Forms.ListViewItem>)(resources.GetObject("listviewmonter.hideItems")));
            this.listviewmonter.HideSelection = false;
            this.listviewmonter.LineColor = System.Drawing.Color.Red;
            this.listviewmonter.Location = new System.Drawing.Point(0, 20);
            this.listviewmonter.Name = "listviewmonter";
            this.listviewmonter.Size = new System.Drawing.Size(276, 195);
            this.listviewmonter.TabIndex = 11;
            this.listviewmonter.UseCompatibleStateImageBehavior = false;
            this.listviewmonter.View = System.Windows.Forms.View.Details;
            this.listviewmonter.SelectedIndexChanged += new System.EventHandler(this.listviewmonter_SelectedIndexChanged);
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Name";
            this.columnHeader2.Width = 218;
            // 
            // lstv
            // 
            this.lstv.AllowColumnReorder = true;
            this.lstv.AllowDrop = true;
            this.lstv.AllowReorder = true;
            this.lstv.AllowSort = false;
            this.lstv.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lstv.ContextMenuStrip = this.menuDelete;
            this.lstv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstv.DoubleClickActivation = false;
            this.lstv.FullRowSelect = true;
            this.lstv.GridLines = true;
            this.lstv.hideItems = ((System.Collections.Generic.List<System.Windows.Forms.ListViewItem>)(resources.GetObject("lstv.hideItems")));
            this.lstv.HideSelection = false;
            this.lstv.LineColor = System.Drawing.Color.Red;
            this.lstv.Location = new System.Drawing.Point(0, 20);
            this.lstv.Name = "lstv";
            this.lstv.Size = new System.Drawing.Size(227, 328);
            this.lstv.TabIndex = 6;
            this.lstv.Tag = "Cài Đặt Boss Map";
            this.lstv.UseCompatibleStateImageBehavior = false;
            this.lstv.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Tọa độ";
            this.columnHeader1.Width = 179;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lstv);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.panel3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(507, 416);
            this.splitContainer1.SplitterDistance = 227;
            this.splitContainer1.TabIndex = 15;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.checkBox1);
            this.panel3.Controls.Add(this.btnAdd);
            this.panel3.Controls.Add(this.btnSave);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 348);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(227, 68);
            this.panel3.TabIndex = 9;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.chkVeLacDuong);
            this.splitContainer2.Panel1.Controls.Add(this.label2);
            this.splitContainer2.Panel1.Controls.Add(this.txtOnlyBossMap);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.listviewmonter);
            this.splitContainer2.Panel2.Controls.Add(this.textBoxEx2);
            this.splitContainer2.Size = new System.Drawing.Size(276, 416);
            this.splitContainer2.SplitterDistance = 197;
            this.splitContainer2.TabIndex = 2;
            // 
            // textBoxEx2
            // 
            this.textBoxEx2.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxEx2.Location = new System.Drawing.Point(0, 0);
            this.textBoxEx2.Name = "textBoxEx2";
            this.textBoxEx2.Size = new System.Drawing.Size(276, 20);
            this.textBoxEx2.TabIndex = 12;
            this.textBoxEx2.WaterMark = "Search..";
            this.textBoxEx2.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.textBoxEx2.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxEx2.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.textBoxEx2.TextChanged += new System.EventHandler(this.textBoxEx2_TextChanged);
            // 
            // chkVeLacDuong
            // 
            this.chkVeLacDuong.AutoSize = true;
            this.chkVeLacDuong.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkVeLacDuong.Location = new System.Drawing.Point(0, 20);
            this.chkVeLacDuong.Name = "chkVeLacDuong";
            this.chkVeLacDuong.Size = new System.Drawing.Size(276, 17);
            this.chkVeLacDuong.TabIndex = 13;
            this.chkVeLacDuong.Text = "Hết Boss Tự Về Lạc Dương";
            this.chkVeLacDuong.UseVisualStyleBackColor = true;
            this.chkVeLacDuong.CheckedChanged += new System.EventHandler(this.chkVeLacDuong_CheckedChanged);
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(227, 20);
            this.label3.TabIndex = 13;
            this.label3.Text = "Bản Đồ";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(276, 20);
            this.label2.TabIndex = 14;
            this.label2.Text = "Đánh Quái Tại Bản Đồ Này";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ConfigBossMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.splitContainer1);
            this.Name = "ConfigBossMap";
            this.Size = new System.Drawing.Size(507, 416);
            this.Load += new System.EventHandler(this.ConfigBossMap_Load);
            this.menuDelete.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnAdd;
        private ListViewEx lstv;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private ListViewEx listviewmonter;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private ChreneLib.Controls.TextBoxes.TextBoxEx txtOnlyBossMap;
        private System.Windows.Forms.ContextMenuStrip menuDelete;
        private System.Windows.Forms.ToolStripMenuItem xóaToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private ChreneLib.Controls.TextBoxes.TextBoxEx textBoxEx2;
        private System.Windows.Forms.CheckBox chkVeLacDuong;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
    }
}