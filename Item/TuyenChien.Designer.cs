namespace _i
{
    partial class TuyenChien
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TuyenChien));
            this.listViewName = new _i.ListViewEx();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.txtName = new ChreneLib.Controls.TextBoxes.TextBoxEx();
            this.listViewGuildName = new _i.ListViewEx();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.txtGuildName = new ChreneLib.Controls.TextBoxes.TextBoxEx();
            this.listViewIgnore = new _i.ListViewEx();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.txtNameIgnore = new ChreneLib.Controls.TextBoxes.TextBoxEx();
            this.btnOk = new System.Windows.Forms.Button();
            this.tabControlEx1 = new _i.TabControlEx();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
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
            this.SuspendLayout();
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
            this.listViewName.Location = new System.Drawing.Point(0, 0);
            this.listViewName.Name = "listViewName";
            this.listViewName.Size = new System.Drawing.Size(247, 271);
            this.listViewName.TabIndex = 7;
            this.listViewName.UseCompatibleStateImageBehavior = false;
            this.listViewName.View = System.Windows.Forms.View.Details;
            this.listViewName.DoubleClick += new System.EventHandler(this.listViewName_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Tên người chơi";
            this.columnHeader1.Width = 181;
            // 
            // txtName
            // 
            this.txtName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtName.Location = new System.Drawing.Point(0, 0);
            this.txtName.Multiline = true;
            this.txtName.Name = "txtName";
            this.txtName.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtName.Size = new System.Drawing.Size(185, 271);
            this.txtName.TabIndex = 6;
            this.txtName.WaterMark = "";
            this.txtName.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtName.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtName.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // listViewGuildName
            // 
            this.listViewGuildName.AllowColumnReorder = true;
            this.listViewGuildName.AllowDrop = true;
            this.listViewGuildName.AllowReorder = true;
            this.listViewGuildName.AllowSort = true;
            this.listViewGuildName.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
            this.listViewGuildName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewGuildName.DoubleClickActivation = false;
            this.listViewGuildName.FullRowSelect = true;
            this.listViewGuildName.GridLines = true;
            this.listViewGuildName.hideItems = ((System.Collections.Generic.List<System.Windows.Forms.ListViewItem>)(resources.GetObject("listViewGuildName.hideItems")));
            this.listViewGuildName.HideSelection = false;
            this.listViewGuildName.LineColor = System.Drawing.Color.Red;
            this.listViewGuildName.Location = new System.Drawing.Point(0, 0);
            this.listViewGuildName.Name = "listViewGuildName";
            this.listViewGuildName.Size = new System.Drawing.Size(247, 271);
            this.listViewGuildName.TabIndex = 9;
            this.listViewGuildName.UseCompatibleStateImageBehavior = false;
            this.listViewGuildName.View = System.Windows.Forms.View.Details;
            this.listViewGuildName.DoubleClick += new System.EventHandler(this.listViewGuildName_DoubleClick);
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Tên bang";
            this.columnHeader2.Width = 181;
            // 
            // txtGuildName
            // 
            this.txtGuildName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtGuildName.Location = new System.Drawing.Point(0, 0);
            this.txtGuildName.Multiline = true;
            this.txtGuildName.Name = "txtGuildName";
            this.txtGuildName.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtGuildName.Size = new System.Drawing.Size(185, 271);
            this.txtGuildName.TabIndex = 8;
            this.txtGuildName.WaterMark = "";
            this.txtGuildName.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtGuildName.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGuildName.WaterMarkForeColor = System.Drawing.Color.LightGray;
            // 
            // listViewIgnore
            // 
            this.listViewIgnore.AllowColumnReorder = true;
            this.listViewIgnore.AllowDrop = true;
            this.listViewIgnore.AllowReorder = true;
            this.listViewIgnore.AllowSort = true;
            this.listViewIgnore.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3});
            this.listViewIgnore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewIgnore.DoubleClickActivation = false;
            this.listViewIgnore.FullRowSelect = true;
            this.listViewIgnore.GridLines = true;
            this.listViewIgnore.hideItems = ((System.Collections.Generic.List<System.Windows.Forms.ListViewItem>)(resources.GetObject("listViewIgnore.hideItems")));
            this.listViewIgnore.HideSelection = false;
            this.listViewIgnore.LineColor = System.Drawing.Color.Red;
            this.listViewIgnore.Location = new System.Drawing.Point(0, 0);
            this.listViewIgnore.Name = "listViewIgnore";
            this.listViewIgnore.Size = new System.Drawing.Size(247, 271);
            this.listViewIgnore.TabIndex = 11;
            this.listViewIgnore.UseCompatibleStateImageBehavior = false;
            this.listViewIgnore.View = System.Windows.Forms.View.Details;
            this.listViewIgnore.DoubleClick += new System.EventHandler(this.listViewIgnore_DoubleClick);
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Tên người chơi không đánh";
            this.columnHeader3.Width = 181;
            // 
            // txtNameIgnore
            // 
            this.txtNameIgnore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNameIgnore.Location = new System.Drawing.Point(0, 0);
            this.txtNameIgnore.Multiline = true;
            this.txtNameIgnore.Name = "txtNameIgnore";
            this.txtNameIgnore.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtNameIgnore.Size = new System.Drawing.Size(185, 271);
            this.txtNameIgnore.TabIndex = 10;
            this.txtNameIgnore.WaterMark = "";
            this.txtNameIgnore.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtNameIgnore.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNameIgnore.WaterMarkForeColor = System.Drawing.Color.LightGray;
            // 
            // btnOk
            // 
            this.btnOk.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnOk.Location = new System.Drawing.Point(0, 303);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(450, 23);
            this.btnOk.TabIndex = 15;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // tabControlEx1
            // 
            this.tabControlEx1.Alignment = System.Windows.Forms.TabAlignment.Top;
            this.tabControlEx1.Controls.Add(this.tabPage1);
            this.tabControlEx1.Controls.Add(this.tabPage2);
            this.tabControlEx1.Controls.Add(this.tabPage3);
            this.tabControlEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlEx1.Location = new System.Drawing.Point(0, 0);
            this.tabControlEx1.Name = "tabControlEx1";
            this.tabControlEx1.SelectedIndex = 0;
            this.tabControlEx1.Size = new System.Drawing.Size(450, 303);
            this.tabControlEx1.TabIndex = 19;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(442, 277);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Đánh Người";
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
            this.splitContainer1.Panel1.Controls.Add(this.txtName);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.listViewName);
            this.splitContainer1.Size = new System.Drawing.Size(436, 271);
            this.splitContainer1.SplitterDistance = 185;
            this.splitContainer1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.splitContainer2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(442, 277);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Đánh Bang";
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
            this.splitContainer2.Panel1.Controls.Add(this.txtGuildName);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.listViewGuildName);
            this.splitContainer2.Size = new System.Drawing.Size(436, 271);
            this.splitContainer2.SplitterDistance = 185;
            this.splitContainer2.TabIndex = 1;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.splitContainer3);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(442, 277);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Không Đánh";
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
            this.splitContainer3.Panel1.Controls.Add(this.txtNameIgnore);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.listViewIgnore);
            this.splitContainer3.Size = new System.Drawing.Size(436, 271);
            this.splitContainer3.SplitterDistance = 185;
            this.splitContainer3.TabIndex = 1;
            // 
            // TuyenChien
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControlEx1);
            this.Controls.Add(this.btnOk);
            this.Name = "TuyenChien";
            this.Size = new System.Drawing.Size(450, 326);
            this.Tag = "Tuyên Chiến - PK";
            this.Load += new System.EventHandler(this.AskPK_Load);
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
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ListViewEx listViewName;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private ChreneLib.Controls.TextBoxes.TextBoxEx txtName;
        private ListViewEx listViewGuildName;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private ChreneLib.Controls.TextBoxes.TextBoxEx txtGuildName;
        private ListViewEx listViewIgnore;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private ChreneLib.Controls.TextBoxes.TextBoxEx txtNameIgnore;
        private System.Windows.Forms.Button btnOk;
        private TabControlEx tabControlEx1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.SplitContainer splitContainer3;
    }
}