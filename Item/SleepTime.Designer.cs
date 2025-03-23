
namespace _i.Item
{
    partial class SleepTime
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SleepTime));
            this.lv = new _i.ListViewEx();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txtMFrom = new ChreneLib.Controls.TextBoxes.TextBoxEx();
            this.txtHFrom = new ChreneLib.Controls.TextBoxes.TextBoxEx();
            this.label1 = new System.Windows.Forms.Label();
            this.txtMEnd = new ChreneLib.Controls.TextBoxes.TextBoxEx();
            this.txtHEnd = new ChreneLib.Controls.TextBoxes.TextBoxEx();
            this.label2 = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lv
            // 
            this.lv.AllowColumnReorder = true;
            this.lv.AllowDrop = true;
            this.lv.AllowReorder = true;
            this.lv.AllowSort = true;
            this.lv.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lv.DoubleClickActivation = false;
            this.lv.FullRowSelect = true;
            this.lv.GridLines = true;
            this.lv.hideItems = ((System.Collections.Generic.List<System.Windows.Forms.ListViewItem>)(resources.GetObject("lv.hideItems")));
            this.lv.HideSelection = false;
            this.lv.LineColor = System.Drawing.Color.Red;
            this.lv.Location = new System.Drawing.Point(0, 85);
            this.lv.Name = "lv";
            this.lv.Size = new System.Drawing.Size(232, 263);
            this.lv.TabIndex = 1;
            this.lv.UseCompatibleStateImageBehavior = false;
            this.lv.View = System.Windows.Forms.View.Details;
            this.lv.DoubleClick += new System.EventHandler(this.lv_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Thời Gian";
            this.columnHeader1.Width = 166;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.splitContainer1);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(232, 85);
            this.panel1.TabIndex = 2;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.txtMFrom);
            this.splitContainer1.Panel1.Controls.Add(this.txtHFrom);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.txtMEnd);
            this.splitContainer1.Panel2.Controls.Add(this.txtHEnd);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Size = new System.Drawing.Size(232, 62);
            this.splitContainer1.SplitterDistance = 116;
            this.splitContainer1.TabIndex = 1;
            // 
            // txtMFrom
            // 
            this.txtMFrom.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtMFrom.Location = new System.Drawing.Point(0, 43);
            this.txtMFrom.Name = "txtMFrom";
            this.txtMFrom.Size = new System.Drawing.Size(116, 20);
            this.txtMFrom.TabIndex = 2;
            this.txtMFrom.WaterMark = "Phút";
            this.txtMFrom.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtMFrom.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMFrom.WaterMarkForeColor = System.Drawing.Color.LightGray;
            // 
            // txtHFrom
            // 
            this.txtHFrom.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtHFrom.Location = new System.Drawing.Point(0, 23);
            this.txtHFrom.Name = "txtHFrom";
            this.txtHFrom.Size = new System.Drawing.Size(116, 20);
            this.txtHFrom.TabIndex = 1;
            this.txtHFrom.WaterMark = "Giờ";
            this.txtHFrom.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtHFrom.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHFrom.WaterMarkForeColor = System.Drawing.Color.LightGray;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Bắt Đầu";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtMEnd
            // 
            this.txtMEnd.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtMEnd.Location = new System.Drawing.Point(0, 43);
            this.txtMEnd.Name = "txtMEnd";
            this.txtMEnd.Size = new System.Drawing.Size(112, 20);
            this.txtMEnd.TabIndex = 4;
            this.txtMEnd.WaterMark = "Phút";
            this.txtMEnd.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtMEnd.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMEnd.WaterMarkForeColor = System.Drawing.Color.LightGray;
            // 
            // txtHEnd
            // 
            this.txtHEnd.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtHEnd.Location = new System.Drawing.Point(0, 23);
            this.txtHEnd.Name = "txtHEnd";
            this.txtHEnd.Size = new System.Drawing.Size(112, 20);
            this.txtHEnd.TabIndex = 3;
            this.txtHEnd.WaterMark = "Giờ";
            this.txtHEnd.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtHEnd.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHEnd.WaterMarkForeColor = System.Drawing.Color.LightGray;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Kết Thúc";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnAdd
            // 
            this.btnAdd.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnAdd.Location = new System.Drawing.Point(0, 62);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(232, 23);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "Thêm";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button1.Location = new System.Drawing.Point(0, 348);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(232, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Ok";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // SleepTime
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lv);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button1);
            this.Name = "SleepTime";
            this.Size = new System.Drawing.Size(232, 371);
            this.Load += new System.EventHandler(this.SleepTime_Load);
            this.panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ListViewEx lv;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private ChreneLib.Controls.TextBoxes.TextBoxEx txtMFrom;
        private ChreneLib.Controls.TextBoxes.TextBoxEx txtHFrom;
        private ChreneLib.Controls.TextBoxes.TextBoxEx txtMEnd;
        private ChreneLib.Controls.TextBoxes.TextBoxEx txtHEnd;
        private System.Windows.Forms.Button button1;
    }
}