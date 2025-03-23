using ChreneLib.Controls.TextBoxes;

namespace _i
{
    partial class Buff
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Buff));
            this.cboPlayer = new System.Windows.Forms.ComboBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txtLst = new ChreneLib.Controls.TextBoxes.TextBoxEx();
            this.listViewName = new _i.ListViewEx();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.txtSearch = new ChreneLib.Controls.TextBoxes.TextBoxEx();
            this.button1 = new System.Windows.Forms.Button();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cboPlayer
            // 
            this.cboPlayer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.cboPlayer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPlayer.FormattingEnabled = true;
            this.cboPlayer.Items.AddRange(new object[] {
            "Thiết Lập Dành Cho Tất Cả"});
            this.cboPlayer.Location = new System.Drawing.Point(0, 332);
            this.cboPlayer.Name = "cboPlayer";
            this.cboPlayer.Size = new System.Drawing.Size(473, 21);
            this.cboPlayer.TabIndex = 238;
            this.cboPlayer.DropDown += new System.EventHandler(this.cboPlayer_DropDown);
            this.cboPlayer.SelectedIndexChanged += new System.EventHandler(this.cboPlayer_SelectedIndexChanged);
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
            this.splitContainer1.Panel2.Controls.Add(this.listViewName);
            this.splitContainer1.Panel2.Controls.Add(this.txtSearch);
            this.splitContainer1.Size = new System.Drawing.Size(473, 332);
            this.splitContainer1.SplitterDistance = 231;
            this.splitContainer1.TabIndex = 239;
            // 
            // txtLst
            // 
            this.txtLst.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLst.Location = new System.Drawing.Point(0, 0);
            this.txtLst.Multiline = true;
            this.txtLst.Name = "txtLst";
            this.txtLst.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLst.Size = new System.Drawing.Size(231, 332);
            this.txtLst.TabIndex = 6;
            this.txtLst.WaterMark = "";
            this.txtLst.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtLst.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLst.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtLst.TextChanged += new System.EventHandler(this.txtLst_TextChanged);
            this.txtLst.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtLst_KeyDown);
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
            this.listViewName.Size = new System.Drawing.Size(238, 312);
            this.listViewName.TabIndex = 7;
            this.listViewName.UseCompatibleStateImageBehavior = false;
            this.listViewName.View = System.Windows.Forms.View.Details;
            this.listViewName.DoubleClick += new System.EventHandler(this.listViewName_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 185;
            // 
            // txtSearch
            // 
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtSearch.Location = new System.Drawing.Point(0, 0);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(238, 20);
            this.txtSearch.TabIndex = 8;
            this.txtSearch.WaterMark = "Search.";
            this.txtSearch.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtSearch.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearch.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button1.Location = new System.Drawing.Point(0, 353);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(473, 23);
            this.button1.TabIndex = 240;
            this.button1.Text = "Lấy Danh Sách Của Tôi";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // Buff
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.cboPlayer);
            this.Controls.Add(this.button1);
            this.Name = "Buff";
            this.Size = new System.Drawing.Size(473, 376);
            this.Load += new System.EventHandler(this.Buff_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private TextBoxEx txtLst;
        private System.Windows.Forms.ComboBox cboPlayer;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private ListViewEx listViewName;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private ChreneLib.Controls.TextBoxes.TextBoxEx txtSearch;
        private System.Windows.Forms.Button button1;
    }
}