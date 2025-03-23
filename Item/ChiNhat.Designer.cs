namespace _i
{
    partial class FrmLocdoOpt
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmLocdoOpt));
            this.lvName = new _i.ListViewEx();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.menuName = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuAddName = new System.Windows.Forms.ToolStripMenuItem();
            this.txtLst = new ChreneLib.Controls.TextBoxes.TextBoxEx();
            this.txtSearch = new ChreneLib.Controls.TextBoxes.TextBoxEx();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.menuName.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
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
            this.lvName.Size = new System.Drawing.Size(246, 319);
            this.lvName.TabIndex = 11;
            this.lvName.UseCompatibleStateImageBehavior = false;
            this.lvName.View = System.Windows.Forms.View.Details;
            this.lvName.SelectedIndexChanged += new System.EventHandler(this.listViewName_SelectedIndexChanged);
            this.lvName.DoubleClick += new System.EventHandler(this.listViewName_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 198;
            // 
            // menuName
            // 
            this.menuName.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuAddName});
            this.menuName.Name = "menuName";
            this.menuName.Size = new System.Drawing.Size(105, 26);
            this.menuName.Opening += new System.ComponentModel.CancelEventHandler(this.menuName_Opening);
            // 
            // menuAddName
            // 
            this.menuAddName.Name = "menuAddName";
            this.menuAddName.Size = new System.Drawing.Size(104, 22);
            this.menuAddName.Text = "Thêm";
            this.menuAddName.Click += new System.EventHandler(this.menuAddName_Click);
            // 
            // txtLst
            // 
            this.txtLst.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLst.Location = new System.Drawing.Point(0, 0);
            this.txtLst.Multiline = true;
            this.txtLst.Name = "txtLst";
            this.txtLst.Size = new System.Drawing.Size(229, 339);
            this.txtLst.TabIndex = 12;
            this.txtLst.WaterMark = "Chỉ Nhặt Những Vật Phẩm Sau";
            this.txtLst.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtLst.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLst.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtLst.TextChanged += new System.EventHandler(this.txtLst_TextChanged);
            // 
            // txtSearch
            // 
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtSearch.Location = new System.Drawing.Point(0, 0);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(246, 20);
            this.txtSearch.TabIndex = 13;
            this.txtSearch.WaterMark = "Search";
            this.txtSearch.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtSearch.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearch.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.txtLst);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lvName);
            this.splitContainer1.Panel2.Controls.Add(this.txtSearch);
            this.splitContainer1.Size = new System.Drawing.Size(479, 339);
            this.splitContainer1.SplitterDistance = 229;
            this.splitContainer1.TabIndex = 14;
            // 
            // FrmLocdoOpt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "FrmLocdoOpt";
            this.Size = new System.Drawing.Size(479, 339);
            this.Load += new System.EventHandler(this.FrmLocdoOpt_Load);
            this.menuName.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private ListViewEx lvName;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ContextMenuStrip menuName;
        private System.Windows.Forms.ToolStripMenuItem menuAddName;
        private ChreneLib.Controls.TextBoxes.TextBoxEx txtLst;
        private ChreneLib.Controls.TextBoxes.TextBoxEx txtSearch;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}