namespace _i
{
    partial class Calendar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Calendar));
            this.menuDelete = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cboNote = new System.Windows.Forms.ComboBox();
            this.Từ = new System.Windows.Forms.Label();
            this.nudMinute = new System.Windows.Forms.NumericUpDown();
            this.nudHour = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.nudMinuteEnd = new System.Windows.Forms.NumericUpDown();
            this.nudHourEnd = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.cboMap = new System.Windows.Forms.ComboBox();
            this.btnAutoAd = new System.Windows.Forms.Button();
            this.listViewCalendar = new _i.ListViewEx();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cboPlayer = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.menuDelete.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinute)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHour)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinuteEnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHourEnd)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuDelete
            // 
            this.menuDelete.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuCopy,
            this.pasteToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.menuDelete.Name = "menuDelete";
            this.menuDelete.Size = new System.Drawing.Size(128, 70);
            this.menuDelete.Opening += new System.ComponentModel.CancelEventHandler(this.menuDelete_Opening);
            // 
            // menuCopy
            // 
            this.menuCopy.Name = "menuCopy";
            this.menuCopy.Size = new System.Drawing.Size(127, 22);
            this.menuCopy.Text = "Copy [All]";
            this.menuCopy.Click += new System.EventHandler(this.menuCopy_Click);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.pasteToolStripMenuItem.Text = "Paste";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // cboNote
            // 
            this.cboNote.Dock = System.Windows.Forms.DockStyle.Top;
            this.cboNote.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNote.FormattingEnabled = true;
            this.cboNote.Items.AddRange(new object[] {
            "Ác Tặc [Key]",
            "Ác Bá [Key]",
            "Tàng Kinh Các [Key]",
            "Dã Trư [Key]",
            "Phụng Hoàng Lăng Mộ [Key]",
            "Kỳ Cuộc [Key]",
            "Lâu Lan Tầm Bảo [Key]",
            "Phiêu Miễu Phong [Key]",
            "Huyết Chiến Phiêu Miễu Phong [Key]",
            "Thiên Giáng Kỳ Thú [Key]",
            "Q123 Tô Châu [Key]",
            "Q123 Lâu Lan [Key]",
            "Yến Tử Ô [Key]",
            "Tứ Tuyệt Trang [Key]",
            "Tụ Bảo Bồn",
            "Luyện Kim",
            "Luyện Kim Nhanh",
            "Phù Về Thành",
            "Nhiệm Vụ Hàng Ngày",
            "Nhiệm Vụ Sư Môn",
            "Thủ Bị [Chiến Minh]",
            "Bán Đồ + Cất Đồ + Trị Liệu",
            "Thiếu Thất Sơn",
            "Sát Tinh",
            "Nhận Bóng",
            "Móng Heo",
            "Tam Thần Huyễn Cảnh",
            "Vương Lăng",
            "Lên Bãi Train",
            "Dùng Thổ Linh Châu",
            "Reset Time",
            "Nhận Kinh Nghiệm Lưu Trữ",
            "Phúc Địa",
            "Phúc Địa Khó",
            "Phân Giải Trang Bị Pet",
            "Reset Giờ Toàn Bộ Người Chơi",
            "Thoát Game",
            "Mã Tặc",
            "Trừng Ác",
            "Thần Khí 9 Sao",
            "Đổi Huyễn Sắc Câu Thiên Thái",
            "Trang Sức Cửu Lê",
            "Đến Kim Lăng",
            "Binh Thánh",
            "Binh Thánh Khó",
            "Nhận Phỉ Thúy"});
            this.cboNote.Location = new System.Drawing.Point(0, 0);
            this.cboNote.Name = "cboNote";
            this.cboNote.Size = new System.Drawing.Size(358, 21);
            this.cboNote.TabIndex = 1;
            // 
            // Từ
            // 
            this.Từ.AutoSize = true;
            this.Từ.Location = new System.Drawing.Point(11, 8);
            this.Từ.Name = "Từ";
            this.Từ.Size = new System.Drawing.Size(20, 13);
            this.Từ.TabIndex = 2;
            this.Từ.Text = "Từ";
            // 
            // nudMinute
            // 
            this.nudMinute.Location = new System.Drawing.Point(108, 6);
            this.nudMinute.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.nudMinute.Name = "nudMinute";
            this.nudMinute.Size = new System.Drawing.Size(38, 20);
            this.nudMinute.TabIndex = 213;
            // 
            // nudHour
            // 
            this.nudHour.Location = new System.Drawing.Point(37, 6);
            this.nudHour.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
            this.nudHour.Name = "nudHour";
            this.nudHour.Size = new System.Drawing.Size(38, 20);
            this.nudHour.TabIndex = 211;
            this.nudHour.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(81, 8);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(21, 13);
            this.label9.TabIndex = 212;
            this.label9.Text = "giờ";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(152, 8);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(40, 13);
            this.label7.TabIndex = 214;
            this.label7.Text = "phút ->";
            // 
            // nudMinuteEnd
            // 
            this.nudMinuteEnd.Location = new System.Drawing.Point(269, 6);
            this.nudMinuteEnd.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.nudMinuteEnd.Name = "nudMinuteEnd";
            this.nudMinuteEnd.Size = new System.Drawing.Size(38, 20);
            this.nudMinuteEnd.TabIndex = 217;
            // 
            // nudHourEnd
            // 
            this.nudHourEnd.Location = new System.Drawing.Point(198, 6);
            this.nudHourEnd.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
            this.nudHourEnd.Name = "nudHourEnd";
            this.nudHourEnd.Size = new System.Drawing.Size(38, 20);
            this.nudHourEnd.TabIndex = 215;
            this.nudHourEnd.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(242, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 13);
            this.label1.TabIndex = 216;
            this.label1.Text = "giờ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(313, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 13);
            this.label2.TabIndex = 218;
            this.label2.Text = "phút";
            // 
            // btnAdd
            // 
            this.btnAdd.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAdd.Location = new System.Drawing.Point(0, 74);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(358, 23);
            this.btnAdd.TabIndex = 219;
            this.btnAdd.Text = "Thêm";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // cboMap
            // 
            this.cboMap.Dock = System.Windows.Forms.DockStyle.Top;
            this.cboMap.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMap.FormattingEnabled = true;
            this.cboMap.Items.AddRange(new object[] {
            "Bản đồ [Tự động]",
            "[Ác Tặc] - Vô Lượng Sơn",
            "[Ác Tặc] - Kính Hồ",
            "[Ác Tặc] - Kiếm Các",
            "[Ác Tặc] - Thái Hồ",
            "[Ác Tặc] - Tung Sơn",
            "[Ác Tặc] | - Đôn Hoàng",
            "[Tàng Kinh Các] - Tây Hồ",
            "[Tàng Kinh Các] - Nhĩ Hải",
            "[Tàng Kinh Các] - Nhạn Nam"});
            this.cboMap.Location = new System.Drawing.Point(0, 53);
            this.cboMap.Name = "cboMap";
            this.cboMap.Size = new System.Drawing.Size(358, 21);
            this.cboMap.TabIndex = 220;
            this.cboMap.SelectedIndexChanged += new System.EventHandler(this.cboMap_SelectedIndexChanged);
            // 
            // btnAutoAd
            // 
            this.btnAutoAd.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnAutoAd.Location = new System.Drawing.Point(0, 483);
            this.btnAutoAd.Name = "btnAutoAd";
            this.btnAutoAd.Size = new System.Drawing.Size(358, 23);
            this.btnAutoAd.TabIndex = 221;
            this.btnAutoAd.Text = "Thêm tự động";
            this.btnAutoAd.UseVisualStyleBackColor = true;
            this.btnAutoAd.Click += new System.EventHandler(this.btnAutoAd_Click);
            // 
            // listViewCalendar
            // 
            this.listViewCalendar.AllowColumnReorder = true;
            this.listViewCalendar.AllowDrop = true;
            this.listViewCalendar.AllowReorder = true;
            this.listViewCalendar.AllowSort = true;
            this.listViewCalendar.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listViewCalendar.ContextMenuStrip = this.menuDelete;
            this.listViewCalendar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewCalendar.DoubleClickActivation = false;
            this.listViewCalendar.FullRowSelect = true;
            this.listViewCalendar.GridLines = true;
            this.listViewCalendar.hideItems = ((System.Collections.Generic.List<System.Windows.Forms.ListViewItem>)(resources.GetObject("listViewCalendar.hideItems")));
            this.listViewCalendar.HideSelection = false;
            this.listViewCalendar.LineColor = System.Drawing.Color.Red;
            this.listViewCalendar.Location = new System.Drawing.Point(0, 97);
            this.listViewCalendar.Name = "listViewCalendar";
            this.listViewCalendar.Size = new System.Drawing.Size(358, 365);
            this.listViewCalendar.TabIndex = 0;
            this.listViewCalendar.UseCompatibleStateImageBehavior = false;
            this.listViewCalendar.View = System.Windows.Forms.View.Details;
            this.listViewCalendar.DragDrop += new System.Windows.Forms.DragEventHandler(this.listViewCalendar_DragDrop);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Thời Gian";
            this.columnHeader1.Width = 106;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Mô Tả";
            this.columnHeader2.Width = 203;
            // 
            // cboPlayer
            // 
            this.cboPlayer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.cboPlayer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPlayer.FormattingEnabled = true;
            this.cboPlayer.Items.AddRange(new object[] {
            "Thiết Lập Dành Cho Tất Cả"});
            this.cboPlayer.Location = new System.Drawing.Point(0, 462);
            this.cboPlayer.Name = "cboPlayer";
            this.cboPlayer.Size = new System.Drawing.Size(358, 21);
            this.cboPlayer.TabIndex = 237;
            this.cboPlayer.SelectedIndexChanged += new System.EventHandler(this.cboPlayer_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.Từ);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.nudHour);
            this.panel1.Controls.Add(this.nudMinute);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.nudMinuteEnd);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.nudHourEnd);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 21);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(358, 32);
            this.panel1.TabIndex = 238;
            // 
            // Calendar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(358, 506);
            this.Controls.Add(this.listViewCalendar);
            this.Controls.Add(this.cboPlayer);
            this.Controls.Add(this.btnAutoAd);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.cboMap);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.cboNote);
            this.Name = "Calendar";
            this.Tag = "Đặt Lịch";
            this.Text = "Đặt Lịch Phụ Bản";
            this.Load += new System.EventHandler(this.Calendar_Load);
            this.menuDelete.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudMinute)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHour)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinuteEnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHourEnd)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ListViewEx listViewCalendar;
        private System.Windows.Forms.ComboBox cboNote;
        private System.Windows.Forms.Label Từ;
        private System.Windows.Forms.NumericUpDown nudMinute;
        private System.Windows.Forms.NumericUpDown nudHour;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown nudMinuteEnd;
        private System.Windows.Forms.NumericUpDown nudHourEnd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ContextMenuStrip menuDelete;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuCopy;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ComboBox cboMap;
        private System.Windows.Forms.Button btnAutoAd;
        private System.Windows.Forms.ComboBox cboPlayer;
        private System.Windows.Forms.Panel panel1;
    }
}