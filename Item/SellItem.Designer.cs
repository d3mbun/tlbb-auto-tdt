namespace _i
{
    partial class SellItem
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SellItem));
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("Ngọc Thời Trang Cấp 1 [Cố Định]");
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("Huyền Binh Thạch [Nếu Đã Có Thần Khí 8 Sao]");
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem("Long Văn, Chú Văn Cố Định [Nếu Long Văn 100]");
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem("Võ Hồn Cố Định [Nếu Võ Hồn Cấp 7]");
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem(new string[] {
            "Vũ Khí Đả Tạo Đồ",
            "Khuyên Dùng"}, -1);
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem(new string[] {
            "Đả Tạo Đồ 1,2,3,4",
            "Khuyên Dùng"}, -1);
            System.Windows.Forms.ListViewItem listViewItem7 = new System.Windows.Forms.ListViewItem("Đục Lỗ 1,2,3,4,5,6,7");
            System.Windows.Forms.ListViewItem listViewItem8 = new System.Windows.Forms.ListViewItem(new string[] {
            "Thức Ăn, Đạo Cụ Rác",
            "Khuyên Dùng"}, -1);
            System.Windows.Forms.ListViewItem listViewItem9 = new System.Windows.Forms.ListViewItem("Trang Bị 1,2,3,4 Sao");
            System.Windows.Forms.ListViewItem listViewItem10 = new System.Windows.Forms.ListViewItem("Trang Bị 5 Sao");
            System.Windows.Forms.ListViewItem listViewItem11 = new System.Windows.Forms.ListViewItem("Trang Bị Chưa Giám Định");
            System.Windows.Forms.ListViewItem listViewItem12 = new System.Windows.Forms.ListViewItem(new string[] {
            "Nguyên Liệu Đúc, May, Công Nghệ, Dược",
            "Khuyên Dùng"}, -1);
            this.menuName = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuAddName = new System.Windows.Forms.ToolStripMenuItem();
            this.menuType = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuAddType = new System.Windows.Forms.ToolStripMenuItem();
            this.btnOk = new System.Windows.Forms.Button();
            this.tabControlEx1 = new _i.TabControlEx();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txtSellName = new ChreneLib.Controls.TextBoxes.TextBoxEx();
            this.textBoxEx1 = new ChreneLib.Controls.TextBoxes.TextBoxEx();
            this.lvName = new _i.ListViewEx();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.txtSearchName = new ChreneLib.Controls.TextBoxes.TextBoxEx();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.lvDinhSan = new _i.ListViewEx();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.menuName.SuspendLayout();
            this.menuType.SuspendLayout();
            this.tabControlEx1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
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
            // btnOk
            // 
            this.btnOk.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnOk.Location = new System.Drawing.Point(0, 457);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(568, 23);
            this.btnOk.TabIndex = 13;
            this.btnOk.Text = "Save";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // tabControlEx1
            // 
            this.tabControlEx1.Alignment = System.Windows.Forms.TabAlignment.Top;
            this.tabControlEx1.Controls.Add(this.tabPage1);
            this.tabControlEx1.Controls.Add(this.tabPage3);
            this.tabControlEx1.Controls.Add(this.tabPage2);
            this.tabControlEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlEx1.Location = new System.Drawing.Point(0, 0);
            this.tabControlEx1.Name = "tabControlEx1";
            this.tabControlEx1.SelectedIndex = 0;
            this.tabControlEx1.Size = new System.Drawing.Size(568, 457);
            this.tabControlEx1.TabIndex = 15;
            this.tabControlEx1.Tag = "Bán Đồ - Sell Item";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(560, 431);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Theo Tên";
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
            this.splitContainer1.Panel1.Controls.Add(this.txtSellName);
            this.splitContainer1.Panel1.Controls.Add(this.textBoxEx1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lvName);
            this.splitContainer1.Panel2.Controls.Add(this.txtSearchName);
            this.splitContainer1.Size = new System.Drawing.Size(554, 425);
            this.splitContainer1.SplitterDistance = 270;
            this.splitContainer1.TabIndex = 0;
            // 
            // txtSellName
            // 
            this.txtSellName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSellName.Location = new System.Drawing.Point(0, 20);
            this.txtSellName.Multiline = true;
            this.txtSellName.Name = "txtSellName";
            this.txtSellName.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSellName.Size = new System.Drawing.Size(270, 405);
            this.txtSellName.TabIndex = 9;
            this.txtSellName.WaterMark = "";
            this.txtSellName.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtSellName.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSellName.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtSellName.TextChanged += new System.EventHandler(this.txtSellName_TextChanged);
            // 
            // textBoxEx1
            // 
            this.textBoxEx1.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxEx1.Location = new System.Drawing.Point(0, 0);
            this.textBoxEx1.Name = "textBoxEx1";
            this.textBoxEx1.Size = new System.Drawing.Size(270, 20);
            this.textBoxEx1.TabIndex = 10;
            this.textBoxEx1.WaterMark = "Search";
            this.textBoxEx1.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.textBoxEx1.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxEx1.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.textBoxEx1.TextChanged += new System.EventHandler(this.textBoxEx1_TextChanged);
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
            this.lvName.Size = new System.Drawing.Size(280, 405);
            this.lvName.TabIndex = 11;
            this.lvName.UseCompatibleStateImageBehavior = false;
            this.lvName.View = System.Windows.Forms.View.Details;
            this.lvName.DoubleClick += new System.EventHandler(this.listViewName_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Tên Vật Phẩm";
            this.columnHeader1.Width = 200;
            // 
            // txtSearchName
            // 
            this.txtSearchName.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtSearchName.Location = new System.Drawing.Point(0, 0);
            this.txtSearchName.Name = "txtSearchName";
            this.txtSearchName.Size = new System.Drawing.Size(280, 20);
            this.txtSearchName.TabIndex = 8;
            this.txtSearchName.WaterMark = "Search...";
            this.txtSearchName.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtSearchName.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearchName.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtSearchName.TextChanged += new System.EventHandler(this.txtSearchName_TextChanged);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.lvDinhSan);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(560, 431);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Định Sẵn";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // lvDinhSan
            // 
            this.lvDinhSan.AllowColumnReorder = true;
            this.lvDinhSan.AllowDrop = true;
            this.lvDinhSan.AllowReorder = true;
            this.lvDinhSan.AllowSort = false;
            this.lvDinhSan.CheckBoxes = true;
            this.lvDinhSan.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4});
            this.lvDinhSan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvDinhSan.DoubleClickActivation = false;
            this.lvDinhSan.FullRowSelect = true;
            this.lvDinhSan.GridLines = true;
            this.lvDinhSan.hideItems = ((System.Collections.Generic.List<System.Windows.Forms.ListViewItem>)(resources.GetObject("lvDinhSan.hideItems")));
            this.lvDinhSan.HideSelection = false;
            listViewItem1.StateImageIndex = 0;
            listViewItem2.StateImageIndex = 0;
            listViewItem3.StateImageIndex = 0;
            listViewItem4.StateImageIndex = 0;
            listViewItem5.StateImageIndex = 0;
            listViewItem6.StateImageIndex = 0;
            listViewItem7.StateImageIndex = 0;
            listViewItem8.StateImageIndex = 0;
            listViewItem9.StateImageIndex = 0;
            listViewItem10.StateImageIndex = 0;
            listViewItem11.StateImageIndex = 0;
            listViewItem12.StateImageIndex = 0;
            this.lvDinhSan.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4,
            listViewItem5,
            listViewItem6,
            listViewItem7,
            listViewItem8,
            listViewItem9,
            listViewItem10,
            listViewItem11,
            listViewItem12});
            this.lvDinhSan.LineColor = System.Drawing.Color.Red;
            this.lvDinhSan.Location = new System.Drawing.Point(3, 3);
            this.lvDinhSan.Name = "lvDinhSan";
            this.lvDinhSan.Size = new System.Drawing.Size(554, 425);
            this.lvDinhSan.TabIndex = 0;
            this.lvDinhSan.UseCompatibleStateImageBehavior = false;
            this.lvDinhSan.View = System.Windows.Forms.View.Details;
            this.lvDinhSan.SelectedIndexChanged += new System.EventHandler(this.lvDinhSan_SelectedIndexChanged);
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Name";
            this.columnHeader3.Width = 294;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Note";
            this.columnHeader4.Width = 122;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(560, 431);
            this.tabPage2.TabIndex = 3;
            this.tabPage2.Text = "Treo Shop";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // SellItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControlEx1);
            this.Controls.Add(this.btnOk);
            this.Name = "SellItem";
            this.Size = new System.Drawing.Size(568, 480);
            this.Tag = "Bán Đồ - Sell Item";
            this.Load += new System.EventHandler(this.SellItem_Load);
            this.menuName.ResumeLayout(false);
            this.menuType.ResumeLayout(false);
            this.tabControlEx1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip menuName;
        private System.Windows.Forms.ToolStripMenuItem menuAddName;
        private System.Windows.Forms.ContextMenuStrip menuType;
        private System.Windows.Forms.ToolStripMenuItem menuAddType;
        private System.Windows.Forms.Button btnOk;
        private ListViewEx lvName;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private ChreneLib.Controls.TextBoxes.TextBoxEx txtSellName;
        private TabControlEx tabControlEx1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private ChreneLib.Controls.TextBoxes.TextBoxEx txtSearchName;
        private System.Windows.Forms.TabPage tabPage3;
        private ListViewEx lvDinhSan;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private ChreneLib.Controls.TextBoxes.TextBoxEx textBoxEx1;
        private System.Windows.Forms.TabPage tabPage2;
    }
}