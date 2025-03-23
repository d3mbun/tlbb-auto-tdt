namespace _i
{
    partial class AutoPK
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoPK));
            this.listViewName = new _i.ListViewEx();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button2 = new System.Windows.Forms.Button();
            this.txtSearch = new ChreneLib.Controls.TextBoxes.TextBoxEx();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
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
            this.listViewName.Location = new System.Drawing.Point(0, 20);
            this.listViewName.Name = "listViewName";
            this.listViewName.Size = new System.Drawing.Size(273, 269);
            this.listViewName.TabIndex = 8;
            this.listViewName.UseCompatibleStateImageBehavior = false;
            this.listViewName.View = System.Windows.Forms.View.Details;
            this.listViewName.SelectedIndexChanged += new System.EventHandler(this.listViewName_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Tên người chơi";
            this.columnHeader1.Width = 189;
            // 
            // button2
            // 
            this.button2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button2.Location = new System.Drawing.Point(0, 289);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(273, 23);
            this.button2.TabIndex = 10;
            this.button2.Text = "Refresh";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtSearch.Location = new System.Drawing.Point(0, 0);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(273, 20);
            this.txtSearch.TabIndex = 11;
            this.txtSearch.WaterMark = "Search";
            this.txtSearch.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtSearch.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearch.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.checkBox1.Location = new System.Drawing.Point(0, 312);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(273, 17);
            this.checkBox1.TabIndex = 12;
            this.checkBox1.Text = "Tuyên Chiến Và Tấn Công";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(193, 314);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Chọn Nhân Vật";
            // 
            // AutoPK
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(273, 329);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listViewName);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.checkBox1);
            this.MaximizeBox = false;
            this.Name = "AutoPK";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tuyên Chiến";
            this.Load += new System.EventHandler(this.AutoPK_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ListViewEx listViewName;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Button button2;
        private ChreneLib.Controls.TextBoxes.TextBoxEx txtSearch;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label1;
    }
}