namespace _i
{
    partial class ChatLogs
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChatLogs));
            this.tooltipEx = new System.Windows.Forms.ToolTip(this.components);
            this.listViewChat = new _i.ListViewEx();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.txtContent = new ChreneLib.Controls.TextBoxes.TextBoxEx();
            this.textBoxEx1 = new ChreneLib.Controls.TextBoxes.TextBoxEx();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tooltipEx
            // 
            this.tooltipEx.AutoPopDelay = 5000;
            this.tooltipEx.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tooltipEx.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.tooltipEx.InitialDelay = 100;
            this.tooltipEx.ReshowDelay = 100;
            this.tooltipEx.ShowAlways = true;
            // 
            // listViewChat
            // 
            this.listViewChat.AllowColumnReorder = true;
            this.listViewChat.AllowDrop = true;
            this.listViewChat.AllowReorder = true;
            this.listViewChat.AllowSort = false;
            this.listViewChat.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.listViewChat.ContextMenuStrip = this.contextMenuStrip1;
            this.listViewChat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewChat.DoubleClickActivation = false;
            this.listViewChat.FullRowSelect = true;
            this.listViewChat.GridLines = true;
            this.listViewChat.hideItems = ((System.Collections.Generic.List<System.Windows.Forms.ListViewItem>)(resources.GetObject("listViewChat.hideItems")));
            this.listViewChat.HideSelection = false;
            this.listViewChat.LineColor = System.Drawing.Color.Red;
            this.listViewChat.Location = new System.Drawing.Point(0, 20);
            this.listViewChat.Name = "listViewChat";
            this.listViewChat.ShowItemToolTips = true;
            this.listViewChat.Size = new System.Drawing.Size(677, 394);
            this.listViewChat.TabIndex = 0;
            this.listViewChat.UseCompatibleStateImageBehavior = false;
            this.listViewChat.View = System.Windows.Forms.View.Details;
            this.listViewChat.SelectedIndexChanged += new System.EventHandler(this.listViewChat_SelectedIndexChanged);
            this.listViewChat.DoubleClick += new System.EventHandler(this.listViewChat_DoubleClick);
            this.listViewChat.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listViewChat_KeyDown);
            this.listViewChat.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listViewChat_MouseClick);
            this.listViewChat.MouseMove += new System.Windows.Forms.MouseEventHandler(this.listViewChat_MouseMove);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Time";
            this.columnHeader1.Width = 74;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "From";
            this.columnHeader2.Width = 70;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "To";
            this.columnHeader3.Width = 70;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Message";
            this.columnHeader4.Width = 247;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(108, 26);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // txtContent
            // 
            this.txtContent.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtContent.Location = new System.Drawing.Point(0, 414);
            this.txtContent.Multiline = true;
            this.txtContent.Name = "txtContent";
            this.txtContent.Size = new System.Drawing.Size(677, 63);
            this.txtContent.TabIndex = 4;
            this.txtContent.WaterMark = "Content...";
            this.txtContent.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtContent.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtContent.WaterMarkForeColor = System.Drawing.Color.LightGray;
            // 
            // textBoxEx1
            // 
            this.textBoxEx1.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxEx1.Location = new System.Drawing.Point(0, 0);
            this.textBoxEx1.Name = "textBoxEx1";
            this.textBoxEx1.Size = new System.Drawing.Size(677, 20);
            this.textBoxEx1.TabIndex = 3;
            this.textBoxEx1.WaterMark = "Search...";
            this.textBoxEx1.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.textBoxEx1.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxEx1.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.textBoxEx1.TextChanged += new System.EventHandler(this.textBoxEx1_TextChanged);
            // 
            // ChatLogs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listViewChat);
            this.Controls.Add(this.txtContent);
            this.Controls.Add(this.textBoxEx1);
            this.Name = "ChatLogs";
            this.Size = new System.Drawing.Size(677, 477);
            this.Load += new System.EventHandler(this.ChatLogs_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ListViewEx listViewChat;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ToolTip tooltipEx;
        private ChreneLib.Controls.TextBoxes.TextBoxEx textBoxEx1;
        private ChreneLib.Controls.TextBoxes.TextBoxEx txtContent;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
    }
}