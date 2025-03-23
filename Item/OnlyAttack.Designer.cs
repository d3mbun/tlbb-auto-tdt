namespace _i
{
    partial class OnlyAttack
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OnlyAttack));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txtBoQua = new ChreneLib.Controls.TextBoxes.TextBoxEx();
            this.listViewName = new _i.ListViewEx();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.txtSearch = new ChreneLib.Controls.TextBoxes.TextBoxEx();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.txtBoQua);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.listViewName);
            this.splitContainer1.Panel2.Controls.Add(this.txtSearch);
            this.splitContainer1.Size = new System.Drawing.Size(482, 379);
            this.splitContainer1.SplitterDistance = 241;
            this.splitContainer1.TabIndex = 11;
            // 
            // txtBoQua
            // 
            this.txtBoQua.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoQua.Location = new System.Drawing.Point(0, 0);
            this.txtBoQua.Multiline = true;
            this.txtBoQua.Name = "txtBoQua";
            this.txtBoQua.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBoQua.Size = new System.Drawing.Size(241, 379);
            this.txtBoQua.TabIndex = 6;
            this.txtBoQua.WaterMark = "";
            this.txtBoQua.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtBoQua.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoQua.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtBoQua.TextChanged += new System.EventHandler(this.txtBoQua_TextChanged);
            // 
            // listViewName
            // 
            this.listViewName.AllowColumnReorder = true;
            this.listViewName.AllowDrop = true;
            this.listViewName.AllowReorder = true;
            this.listViewName.AllowSort = true;
            this.listViewName.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listViewName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewName.DoubleClickActivation = false;
            this.listViewName.FullRowSelect = true;
            this.listViewName.GridLines = true;
            this.listViewName.hideItems = ((System.Collections.Generic.List<System.Windows.Forms.ListViewItem>)(resources.GetObject("listViewName.hideItems")));
            this.listViewName.HideSelection = false;
            this.listViewName.LineColor = System.Drawing.Color.Red;
            this.listViewName.Location = new System.Drawing.Point(0, 20);
            this.listViewName.Name = "listViewName";
            this.listViewName.Size = new System.Drawing.Size(237, 359);
            this.listViewName.TabIndex = 7;
            this.listViewName.UseCompatibleStateImageBehavior = false;
            this.listViewName.View = System.Windows.Forms.View.Details;
            this.listViewName.DoubleClick += new System.EventHandler(this.listViewName_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Tên người";
            this.columnHeader1.Width = 190;
            // 
            // txtSearch
            // 
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtSearch.Location = new System.Drawing.Point(0, 0);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(237, 20);
            this.txtSearch.TabIndex = 8;
            this.txtSearch.WaterMark = "Search...";
            this.txtSearch.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtSearch.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearch.WaterMarkForeColor = System.Drawing.Color.LightGray;
            // 
            // OnlyAttack
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "OnlyAttack";
            this.Size = new System.Drawing.Size(482, 379);
            this.Tag = "Chỉ Đánh - Only Attack";
            this.Load += new System.EventHandler(this.OnlyAttack_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer splitContainer1;
        private ChreneLib.Controls.TextBoxes.TextBoxEx txtBoQua;
        private ListViewEx listViewName;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private ChreneLib.Controls.TextBoxes.TextBoxEx txtSearch;
    }
}