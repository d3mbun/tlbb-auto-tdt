namespace _i
{
    partial class GomItem
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GomItem));
            this.menuType = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuAddType = new System.Windows.Forms.ToolStripMenuItem();
            this.lvName = new _i.ListViewEx();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.menuName = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuAddName = new System.Windows.Forms.ToolStripMenuItem();
            this.txtName = new ChreneLib.Controls.TextBoxes.TextBoxEx();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txtSearchName = new ChreneLib.Controls.TextBoxes.TextBoxEx();
            this.tabControlEx1 = new _i.TabControlEx();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.menuType.SuspendLayout();
            this.menuName.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControlEx1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuType
            // 
            this.menuType.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuAddType});
            this.menuType.Name = "menuName";
            this.menuType.Size = new System.Drawing.Size(105, 26);
            // 
            // menuAddType
            // 
            this.menuAddType.Name = "menuAddType";
            this.menuAddType.Size = new System.Drawing.Size(104, 22);
            this.menuAddType.Text = "Thêm";
            this.menuAddType.Click += new System.EventHandler(this.menuAddType_Click);
            // 
            // lvName
            // 
            this.lvName.AllowColumnReorder = true;
            this.lvName.AllowDrop = true;
            this.lvName.AllowReorder = true;
            this.lvName.AllowSort = true;
            this.lvName.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lvName.ContextMenuStrip = this.menuName;
            this.lvName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvName.DoubleClickActivation = false;
            this.lvName.FullRowSelect = true;
            this.lvName.GridLines = true;
            this.lvName.hideItems = ((System.Collections.Generic.List<System.Windows.Forms.ListViewItem>)(resources.GetObject("lvName.hideItems")));
            this.lvName.HideSelection = false;
            this.lvName.LineColor = System.Drawing.Color.Red;
            this.lvName.Location = new System.Drawing.Point(0, 20);
            this.lvName.Name = "lvName";
            this.lvName.Size = new System.Drawing.Size(237, 323);
            this.lvName.TabIndex = 24;
            this.lvName.UseCompatibleStateImageBehavior = false;
            this.lvName.View = System.Windows.Forms.View.Details;
            this.lvName.DoubleClick += new System.EventHandler(this.listViewName_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Tên Vật Phẩm";
            this.columnHeader1.Width = 180;
            // 
            // menuName
            // 
            this.menuName.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuAddName});
            this.menuName.Name = "menuName";
            this.menuName.Size = new System.Drawing.Size(105, 26);
            // 
            // menuAddName
            // 
            this.menuAddName.Name = "menuAddName";
            this.menuAddName.Size = new System.Drawing.Size(104, 22);
            this.menuAddName.Text = "Thêm";
            this.menuAddName.Click += new System.EventHandler(this.menuAddName_Click);
            // 
            // txtName
            // 
            this.txtName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtName.Location = new System.Drawing.Point(0, 0);
            this.txtName.Multiline = true;
            this.txtName.Name = "txtName";
            this.txtName.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtName.Size = new System.Drawing.Size(229, 343);
            this.txtName.TabIndex = 22;
            this.txtName.WaterMark = "";
            this.txtName.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtName.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtName.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.txtName);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lvName);
            this.splitContainer1.Panel2.Controls.Add(this.txtSearchName);
            this.splitContainer1.Size = new System.Drawing.Size(470, 343);
            this.splitContainer1.SplitterDistance = 229;
            this.splitContainer1.TabIndex = 0;
            // 
            // txtSearchName
            // 
            this.txtSearchName.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtSearchName.Location = new System.Drawing.Point(0, 0);
            this.txtSearchName.Name = "txtSearchName";
            this.txtSearchName.Size = new System.Drawing.Size(237, 20);
            this.txtSearchName.TabIndex = 8;
            this.txtSearchName.WaterMark = "Search...";
            this.txtSearchName.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtSearchName.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearchName.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtSearchName.TextChanged += new System.EventHandler(this.txtSearchName_TextChanged);
            // 
            // tabControlEx1
            // 
            this.tabControlEx1.Alignment = System.Windows.Forms.TabAlignment.Top;
            this.tabControlEx1.Controls.Add(this.tabPage1);
            this.tabControlEx1.Controls.Add(this.tabPage2);
            this.tabControlEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlEx1.Location = new System.Drawing.Point(0, 0);
            this.tabControlEx1.Name = "tabControlEx1";
            this.tabControlEx1.SelectedIndex = 0;
            this.tabControlEx1.Size = new System.Drawing.Size(484, 375);
            this.tabControlEx1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(476, 349);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Đồ Thường";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(476, 349);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Đồ KNB";
            this.tabPage2.UseVisualStyleBackColor = true;
            this.tabPage2.Click += new System.EventHandler(this.tabPage2_Click);
            // 
            // GomItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControlEx1);
            this.Name = "GomItem";
            this.Size = new System.Drawing.Size(484, 375);
            this.Tag = "Gom Đồ";
            this.Load += new System.EventHandler(this.GomItem_Load);
            this.menuType.ResumeLayout(false);
            this.menuName.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.tabControlEx1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip menuType;
        private System.Windows.Forms.ToolStripMenuItem menuAddType;
        private ListViewEx lvName;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ContextMenuStrip menuName;
        private System.Windows.Forms.ToolStripMenuItem menuAddName;
        private ChreneLib.Controls.TextBoxes.TextBoxEx txtName;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private ChreneLib.Controls.TextBoxes.TextBoxEx txtSearchName;
        private TabControlEx tabControlEx1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
    }
}