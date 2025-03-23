namespace _i
{
    partial class LayDo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LayDo));
            this.lvName = new _i.ListViewEx();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.txtDropName = new ChreneLib.Controls.TextBoxes.TextBoxEx();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txtSearchName = new ChreneLib.Controls.TextBoxes.TextBoxEx();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tabControlEx1 = new _i.TabControlEx();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControlEx1.SuspendLayout();
            this.tabPage1.SuspendLayout();
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
            this.lvName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvName.DoubleClickActivation = false;
            this.lvName.FullRowSelect = true;
            this.lvName.GridLines = true;
            this.lvName.hideItems = ((System.Collections.Generic.List<System.Windows.Forms.ListViewItem>)(resources.GetObject("lvName.hideItems")));
            this.lvName.HideSelection = false;
            this.lvName.LineColor = System.Drawing.Color.Red;
            this.lvName.Location = new System.Drawing.Point(0, 20);
            this.lvName.Name = "lvName";
            this.lvName.Size = new System.Drawing.Size(227, 318);
            this.lvName.TabIndex = 20;
            this.lvName.UseCompatibleStateImageBehavior = false;
            this.lvName.View = System.Windows.Forms.View.Details;
            this.lvName.DoubleClick += new System.EventHandler(this.listViewName_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Tên Vật Phẩm";
            this.columnHeader1.Width = 180;
            // 
            // txtDropName
            // 
            this.txtDropName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDropName.Location = new System.Drawing.Point(0, 0);
            this.txtDropName.Multiline = true;
            this.txtDropName.Name = "txtDropName";
            this.txtDropName.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDropName.Size = new System.Drawing.Size(215, 355);
            this.txtDropName.TabIndex = 18;
            this.txtDropName.WaterMark = "";
            this.txtDropName.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtDropName.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDropName.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtDropName.TextChanged += new System.EventHandler(this.txtDropName_TextChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.txtDropName);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lvName);
            this.splitContainer1.Panel2.Controls.Add(this.checkBox1);
            this.splitContainer1.Panel2.Controls.Add(this.txtSearchName);
            this.splitContainer1.Size = new System.Drawing.Size(446, 355);
            this.splitContainer1.SplitterDistance = 215;
            this.splitContainer1.TabIndex = 0;
            // 
            // txtSearchName
            // 
            this.txtSearchName.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtSearchName.Location = new System.Drawing.Point(0, 0);
            this.txtSearchName.Name = "txtSearchName";
            this.txtSearchName.Size = new System.Drawing.Size(227, 20);
            this.txtSearchName.TabIndex = 8;
            this.txtSearchName.WaterMark = "Search...";
            this.txtSearchName.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtSearchName.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearchName.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtSearchName.TextChanged += new System.EventHandler(this.txtSearchName_TextChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.checkBox1.Location = new System.Drawing.Point(0, 338);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(227, 17);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "Không Lấy Đồ Cố Định";
            this.toolTip.SetToolTip(this.checkBox1, "Không Lấy Đồ Cố Định");
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged_1);
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 5000;
            this.toolTip.InitialDelay = 100;
            this.toolTip.IsBalloon = true;
            this.toolTip.ReshowDelay = 100;
            this.toolTip.ShowAlways = true;
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
            this.tabControlEx1.Size = new System.Drawing.Size(460, 387);
            this.tabControlEx1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(452, 361);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Thương Khố";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(452, 361);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Thiên Cơ";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // LayDo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControlEx1);
            this.Name = "LayDo";
            this.Size = new System.Drawing.Size(460, 387);
            this.Tag = "Lấy Đồ Từ Thương Khố";
            this.Load += new System.EventHandler(this.LayDo_Load);
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

        private ListViewEx lvName;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private ChreneLib.Controls.TextBoxes.TextBoxEx txtDropName;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private ChreneLib.Controls.TextBoxes.TextBoxEx txtSearchName;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.CheckBox checkBox1;
        private TabControlEx tabControlEx1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
    }
}