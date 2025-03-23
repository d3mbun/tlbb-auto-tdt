namespace _i
{
    partial class BuyItem
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BuyItem));
            this.button1 = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.txtNum = new ChreneLib.Controls.TextBoxes.TextBoxEx();
            this.txtName = new ChreneLib.Controls.TextBoxes.TextBoxEx();
            this.lvName = new _i.ListViewEx();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvBuyName = new _i.ListViewEx();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.txtSearch = new ChreneLib.Controls.TextBoxes.TextBoxEx();
            this.cboPlayer = new System.Windows.Forms.ComboBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControlEx1 = new _i.TabControlEx();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControlEx1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button1.Location = new System.Drawing.Point(0, 328);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(224, 20);
            this.button1.TabIndex = 11;
            this.button1.Text = "Thêm";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 5000;
            this.toolTip.InitialDelay = 100;
            this.toolTip.ReshowDelay = 100;
            this.toolTip.ShowAlways = true;
            // 
            // txtNum
            // 
            this.txtNum.Dock = System.Windows.Forms.DockStyle.Right;
            this.txtNum.Location = new System.Drawing.Point(179, 0);
            this.txtNum.Name = "txtNum";
            this.txtNum.Size = new System.Drawing.Size(45, 20);
            this.txtNum.TabIndex = 10;
            this.txtNum.Text = "20";
            this.txtNum.WaterMark = "SL";
            this.txtNum.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtNum.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNum.WaterMarkForeColor = System.Drawing.Color.LightGray;
            // 
            // txtName
            // 
            this.txtName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtName.Location = new System.Drawing.Point(0, 0);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(179, 20);
            this.txtName.TabIndex = 9;
            this.txtName.WaterMark = "Tên Vật Phẩm Cần Mua";
            this.txtName.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtName.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtName.WaterMarkForeColor = System.Drawing.Color.LightGray;
            // 
            // lvName
            // 
            this.lvName.AllowColumnReorder = true;
            this.lvName.AllowDrop = true;
            this.lvName.AllowReorder = true;
            this.lvName.AllowSort = true;
            this.lvName.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
            this.lvName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvName.DoubleClickActivation = false;
            this.lvName.FullRowSelect = true;
            this.lvName.GridLines = true;
            this.lvName.hideItems = ((System.Collections.Generic.List<System.Windows.Forms.ListViewItem>)(resources.GetObject("lvName.hideItems")));
            this.lvName.HideSelection = false;
            this.lvName.LineColor = System.Drawing.Color.Red;
            this.lvName.Location = new System.Drawing.Point(0, 20);
            this.lvName.Name = "lvName";
            this.lvName.Size = new System.Drawing.Size(224, 288);
            this.lvName.TabIndex = 8;
            this.lvName.UseCompatibleStateImageBehavior = false;
            this.lvName.View = System.Windows.Forms.View.Details;
            this.lvName.SelectedIndexChanged += new System.EventHandler(this.listViewType_SelectedIndexChanged);
            this.lvName.DoubleClick += new System.EventHandler(this.lvName_DoubleClick);
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Name";
            this.columnHeader2.Width = 197;
            // 
            // lvBuyName
            // 
            this.lvBuyName.AllowColumnReorder = true;
            this.lvBuyName.AllowDrop = true;
            this.lvBuyName.AllowReorder = true;
            this.lvBuyName.AllowSort = true;
            this.lvBuyName.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader3});
            this.lvBuyName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvBuyName.DoubleClickActivation = false;
            this.lvBuyName.FullRowSelect = true;
            this.lvBuyName.GridLines = true;
            this.lvBuyName.hideItems = ((System.Collections.Generic.List<System.Windows.Forms.ListViewItem>)(resources.GetObject("lvBuyName.hideItems")));
            this.lvBuyName.HideSelection = false;
            this.lvBuyName.LineColor = System.Drawing.Color.Red;
            this.lvBuyName.Location = new System.Drawing.Point(0, 0);
            this.lvBuyName.Name = "lvBuyName";
            this.lvBuyName.Size = new System.Drawing.Size(224, 327);
            this.lvBuyName.TabIndex = 7;
            this.lvBuyName.UseCompatibleStateImageBehavior = false;
            this.lvBuyName.View = System.Windows.Forms.View.Details;
            this.lvBuyName.SelectedIndexChanged += new System.EventHandler(this.lvBuyName_SelectedIndexChanged);
            this.lvBuyName.DoubleClick += new System.EventHandler(this.listViewName_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 147;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "SL";
            this.columnHeader3.Width = 38;
            // 
            // txtSearch
            // 
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtSearch.Location = new System.Drawing.Point(0, 0);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(224, 20);
            this.txtSearch.TabIndex = 15;
            this.txtSearch.WaterMark = "Search";
            this.txtSearch.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtSearch.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearch.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // cboPlayer
            // 
            this.cboPlayer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.cboPlayer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPlayer.FormattingEnabled = true;
            this.cboPlayer.Location = new System.Drawing.Point(0, 327);
            this.cboPlayer.Name = "cboPlayer";
            this.cboPlayer.Size = new System.Drawing.Size(224, 21);
            this.cboPlayer.TabIndex = 16;
            this.cboPlayer.DropDown += new System.EventHandler(this.cboPlayer_DropDown);
            this.cboPlayer.SelectedIndexChanged += new System.EventHandler(this.cboPlayer_SelectedIndexChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lvBuyName);
            this.splitContainer1.Panel1.Controls.Add(this.cboPlayer);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lvName);
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Panel2.Controls.Add(this.button1);
            this.splitContainer1.Panel2.Controls.Add(this.txtSearch);
            this.splitContainer1.Size = new System.Drawing.Size(452, 348);
            this.splitContainer1.SplitterDistance = 224;
            this.splitContainer1.TabIndex = 17;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtName);
            this.panel1.Controls.Add(this.txtNum);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 308);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(224, 20);
            this.panel1.TabIndex = 10;
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
            this.tabControlEx1.Size = new System.Drawing.Size(466, 380);
            this.tabControlEx1.TabIndex = 18;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(458, 354);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Shop Thường";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(458, 354);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Shop Đặc Biệt";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // BuyItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControlEx1);
            this.Name = "BuyItem";
            this.Size = new System.Drawing.Size(466, 380);
            this.Tag = "Mua Vật Phẩm - Buy Item";
            this.Load += new System.EventHandler(this.BuyItem_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControlEx1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ListViewEx lvName;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private ListViewEx lvBuyName;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private ChreneLib.Controls.TextBoxes.TextBoxEx txtName;
        private ChreneLib.Controls.TextBoxes.TextBoxEx txtNum;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ToolTip toolTip;
        private ChreneLib.Controls.TextBoxes.TextBoxEx txtSearch;
        private System.Windows.Forms.ComboBox cboPlayer;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel1;
        private TabControlEx tabControlEx1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
    }
}