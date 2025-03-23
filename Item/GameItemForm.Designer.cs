
namespace _i
{
    partial class GameItemForm
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameItemForm));
            this.btnRefresh = new System.Windows.Forms.Button();
            this.menuName = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuThem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuThemCoDinh = new System.Windows.Forms.ToolStripMenuItem();
            this.menuXemAiDangCam = new System.Windows.Forms.ToolStripMenuItem();
            this.lvName = new _i.ListViewEx();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.txtSearch = new ChreneLib.Controls.TextBoxes.TextBoxEx();
            this.number = new System.Windows.Forms.TextBox();
            this.menuName.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnRefresh
            // 
            this.btnRefresh.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnRefresh.Location = new System.Drawing.Point(0, 471);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(554, 23);
            this.btnRefresh.TabIndex = 12;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // menuName
            // 
            this.menuName.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuThem,
            this.menuThemCoDinh,
            this.menuXemAiDangCam});
            this.menuName.Name = "menuName";
            this.menuName.Size = new System.Drawing.Size(183, 70);
            // 
            // menuThem
            // 
            this.menuThem.Name = "menuThem";
            this.menuThem.Size = new System.Drawing.Size(182, 22);
            this.menuThem.Text = "Thêm [Double Click]";
            // 
            // menuThemCoDinh
            // 
            this.menuThemCoDinh.Name = "menuThemCoDinh";
            this.menuThemCoDinh.Size = new System.Drawing.Size(182, 22);
            this.menuThemCoDinh.Text = "Thêm Cố Định";
            // 
            // menuXemAiDangCam
            // 
            this.menuXemAiDangCam.Name = "menuXemAiDangCam";
            this.menuXemAiDangCam.Size = new System.Drawing.Size(182, 22);
            this.menuXemAiDangCam.Text = "Xem Ai Đang Cầm";
            this.menuXemAiDangCam.Click += new System.EventHandler(this.menuXemAiDangCam_Click);
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
            this.lvName.Size = new System.Drawing.Size(554, 431);
            this.lvName.TabIndex = 10;
            this.lvName.UseCompatibleStateImageBehavior = false;
            this.lvName.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 155;
            // 
            // txtSearch
            // 
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtSearch.Location = new System.Drawing.Point(0, 0);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(554, 20);
            this.txtSearch.TabIndex = 11;
            this.txtSearch.WaterMark = "Search...";
            this.txtSearch.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtSearch.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearch.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // number
            // 
            this.number.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.number.Location = new System.Drawing.Point(0, 451);
            this.number.Name = "number";
            this.number.Size = new System.Drawing.Size(554, 20);
            this.number.TabIndex = 13;
            this.number.Visible = false;
            // 
            // GameItemForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lvName);
            this.Controls.Add(this.number);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.txtSearch);
            this.Name = "GameItemForm";
            this.Size = new System.Drawing.Size(554, 494);
            this.menuName.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Button btnRefresh;
        private ChreneLib.Controls.TextBoxes.TextBoxEx txtSearch;
        private System.Windows.Forms.ContextMenuStrip menuName;
        private System.Windows.Forms.ToolStripMenuItem menuXemAiDangCam;
        internal ListViewEx lvName;
        public System.Windows.Forms.ToolStripMenuItem menuThem;
        public System.Windows.Forms.ToolStripMenuItem menuThemCoDinh;
        public System.Windows.Forms.TextBox number;
    }
}
