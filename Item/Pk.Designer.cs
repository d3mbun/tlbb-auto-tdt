namespace _i
{
    partial class Pk
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txtBoQua = new ChreneLib.Controls.TextBoxes.TextBoxEx();
            this.listViewName = new _i.ListViewEx();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.menuName = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuAddName = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.menuName.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 45);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.txtBoQua);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.listViewName);
            this.splitContainer1.Size = new System.Drawing.Size(493, 280);
            this.splitContainer1.SplitterDistance = 239;
            this.splitContainer1.TabIndex = 9;
            // 
            // txtBoQua
            // 
            this.txtBoQua.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoQua.Location = new System.Drawing.Point(0, 0);
            this.txtBoQua.Multiline = true;
            this.txtBoQua.Name = "txtBoQua";
            this.txtBoQua.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBoQua.Size = new System.Drawing.Size(239, 280);
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
            this.listViewName.ContextMenuStrip = this.menuName;
            this.listViewName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewName.FullRowSelect = true;
            this.listViewName.GridLines = true;
            this.listViewName.HideSelection = false;
            this.listViewName.LineColor = System.Drawing.Color.Red;
            this.listViewName.Location = new System.Drawing.Point(0, 0);
            this.listViewName.Name = "listViewName";
            this.listViewName.Size = new System.Drawing.Size(250, 280);
            this.listViewName.TabIndex = 7;
            this.listViewName.UseCompatibleStateImageBehavior = false;
            this.listViewName.View = System.Windows.Forms.View.Details;
            this.listViewName.DoubleClick += new System.EventHandler(this.listViewName_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Tên người";
            this.columnHeader1.Width = 219;
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(295, 26);
            this.label1.TabIndex = 10;
            this.label1.Text = "Chọn đánh theo và theo người chơi bất kỳ thay cho key thực\r\nAuto sẽ coi người đó " +
    "là key, thứ tự ưu tiên từ trên xuống";
            // 
            // Pk
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(491, 322);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Pk";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hỗ trợ Pk";
            this.Load += new System.EventHandler(this.Pk_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.menuName.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private ChreneLib.Controls.TextBoxes.TextBoxEx txtBoQua;
        private ListViewEx listViewName;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ContextMenuStrip menuName;
        private System.Windows.Forms.ToolStripMenuItem menuAddName;
    }
}