namespace _i
{
    partial class SellItemPS
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SellItemPS));
            this.txtSearchName = new ChreneLib.Controls.TextBoxes.TextBoxEx();
            this.lvName = new _i.ListViewEx();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lvPrice = new _i.ListViewEx();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.txtPrice = new ChreneLib.Controls.TextBoxes.TextBoxEx();
            this.button1 = new System.Windows.Forms.Button();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSearchName
            // 
            this.txtSearchName.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtSearchName.Location = new System.Drawing.Point(0, 0);
            this.txtSearchName.Name = "txtSearchName";
            this.txtSearchName.Size = new System.Drawing.Size(255, 20);
            this.txtSearchName.TabIndex = 8;
            this.txtSearchName.WaterMark = "Search...";
            this.txtSearchName.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtSearchName.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearchName.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtSearchName.TextChanged += new System.EventHandler(this.txtSearchName_TextChanged);
            // 
            // lvName
            // 
            this.lvName.AllowColumnReorder = true;
            this.lvName.AllowDrop = true;
            this.lvName.AllowReorder = true;
            this.lvName.AllowSort = true;
            this.lvName.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lvName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvName.DoubleClickActivation = false;
            this.lvName.FullRowSelect = true;
            this.lvName.GridLines = true;
            this.lvName.hideItems = ((System.Collections.Generic.List<System.Windows.Forms.ListViewItem>)(resources.GetObject("lvName.hideItems")));
            this.lvName.HideSelection = false;
            this.lvName.LineColor = System.Drawing.Color.Red;
            this.lvName.Location = new System.Drawing.Point(0, 20);
            this.lvName.Name = "lvName";
            this.lvName.Size = new System.Drawing.Size(255, 305);
            this.lvName.TabIndex = 11;
            this.lvName.UseCompatibleStateImageBehavior = false;
            this.lvName.View = System.Windows.Forms.View.Details;
            this.lvName.SelectedIndexChanged += new System.EventHandler(this.lvPrice_DoubleClick);
            this.lvName.DoubleClick += new System.EventHandler(this.listViewName_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Tên Vật Phẩm";
            this.columnHeader1.Width = 200;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lvPrice);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lvName);
            this.splitContainer1.Panel2.Controls.Add(this.txtPrice);
            this.splitContainer1.Panel2.Controls.Add(this.txtSearchName);
            this.splitContainer1.Panel2.Controls.Add(this.button1);
            this.splitContainer1.Size = new System.Drawing.Size(505, 368);
            this.splitContainer1.SplitterDistance = 246;
            this.splitContainer1.TabIndex = 0;
            // 
            // lvPrice
            // 
            this.lvPrice.AllowColumnReorder = true;
            this.lvPrice.AllowDrop = true;
            this.lvPrice.AllowReorder = true;
            this.lvPrice.AllowSort = true;
            this.lvPrice.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader3});
            this.lvPrice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvPrice.DoubleClickActivation = false;
            this.lvPrice.FullRowSelect = true;
            this.lvPrice.GridLines = true;
            this.lvPrice.hideItems = ((System.Collections.Generic.List<System.Windows.Forms.ListViewItem>)(resources.GetObject("lvPrice.hideItems")));
            this.lvPrice.HideSelection = false;
            this.lvPrice.LineColor = System.Drawing.Color.Red;
            this.lvPrice.Location = new System.Drawing.Point(0, 0);
            this.lvPrice.Name = "lvPrice";
            this.lvPrice.Size = new System.Drawing.Size(246, 368);
            this.lvPrice.TabIndex = 12;
            this.lvPrice.UseCompatibleStateImageBehavior = false;
            this.lvPrice.View = System.Windows.Forms.View.Details;
            this.lvPrice.DoubleClick += new System.EventHandler(this.lvPrice_DoubleClick);
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Tên Vật Phẩm";
            this.columnHeader2.Width = 151;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Giá";
            // 
            // txtPrice
            // 
            this.txtPrice.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtPrice.Location = new System.Drawing.Point(0, 325);
            this.txtPrice.Name = "txtPrice";
            this.txtPrice.Size = new System.Drawing.Size(255, 20);
            this.txtPrice.TabIndex = 12;
            this.txtPrice.WaterMark = "Price... 300";
            this.txtPrice.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtPrice.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPrice.WaterMarkForeColor = System.Drawing.Color.LightGray;
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button1.Location = new System.Drawing.Point(0, 345);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(255, 23);
            this.button1.TabIndex = 13;
            this.button1.Text = "Add";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // SellItemPS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "SellItemPS";
            this.Size = new System.Drawing.Size(505, 368);
            this.Load += new System.EventHandler(this.SellItemPS_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private ChreneLib.Controls.TextBoxes.TextBoxEx txtSearchName;
        private ListViewEx lvName;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private ChreneLib.Controls.TextBoxes.TextBoxEx txtPrice;
        private ListViewEx lvPrice;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Button button1;
    }
}