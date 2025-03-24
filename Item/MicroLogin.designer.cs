namespace _i
{
    partial class MicroLogin
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
            this.chkShowPass = new System.Windows.Forms.CheckBox();
            this.CboTail = new System.Windows.Forms.ComboBox();
            this.btnPublisher = new System.Windows.Forms.Button();
            this.CboServer = new System.Windows.Forms.ComboBox();
            this.btnAddAcc = new System.Windows.Forms.Button();
            this.CboNPH = new System.Windows.Forms.ComboBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.btnEdit = new System.Windows.Forms.Button();
            this.TxtUser = new ChreneLib.Controls.TextBoxes.TextBoxEx();
            this.TxtPass = new ChreneLib.Controls.TextBoxes.TextBoxEx();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tab = new _i.TabControlEx();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.buyClone2 = new _i.BuyClone();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tab.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkShowPass
            // 
            this.chkShowPass.AutoSize = true;
            this.chkShowPass.Dock = System.Windows.Forms.DockStyle.Right;
            this.chkShowPass.Location = new System.Drawing.Point(189, 0);
            this.chkShowPass.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkShowPass.Name = "chkShowPass";
            this.chkShowPass.Size = new System.Drawing.Size(18, 27);
            this.chkShowPass.TabIndex = 43;
            this.chkShowPass.UseVisualStyleBackColor = true;
            this.chkShowPass.CheckedChanged += new System.EventHandler(this.chkShowPass_CheckedChanged);
            // 
            // CboTail
            // 
            this.CboTail.Dock = System.Windows.Forms.DockStyle.Right;
            this.CboTail.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CboTail.FormattingEnabled = true;
            this.CboTail.Location = new System.Drawing.Point(166, 0);
            this.CboTail.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.CboTail.Name = "CboTail";
            this.CboTail.Size = new System.Drawing.Size(101, 24);
            this.CboTail.TabIndex = 40;
            // 
            // btnPublisher
            // 
            this.btnPublisher.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnPublisher.Location = new System.Drawing.Point(236, 0);
            this.btnPublisher.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnPublisher.Name = "btnPublisher";
            this.btnPublisher.Size = new System.Drawing.Size(31, 27);
            this.btnPublisher.TabIndex = 42;
            this.btnPublisher.Text = "+";
            this.btnPublisher.UseVisualStyleBackColor = true;
            this.btnPublisher.Click += new System.EventHandler(this.btnPublisher_Click);
            // 
            // CboServer
            // 
            this.CboServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CboServer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CboServer.FormattingEnabled = true;
            this.CboServer.Location = new System.Drawing.Point(0, 0);
            this.CboServer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.CboServer.Name = "CboServer";
            this.CboServer.Size = new System.Drawing.Size(166, 24);
            this.CboServer.TabIndex = 32;
            this.CboServer.SelectedIndexChanged += new System.EventHandler(this.cboServer_SelectedIndexChanged);
            // 
            // btnAddAcc
            // 
            this.btnAddAcc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAddAcc.Location = new System.Drawing.Point(0, 27);
            this.btnAddAcc.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAddAcc.Name = "btnAddAcc";
            this.btnAddAcc.Size = new System.Drawing.Size(81, 27);
            this.btnAddAcc.TabIndex = 38;
            this.btnAddAcc.Text = "Add";
            this.btnAddAcc.UseVisualStyleBackColor = true;
            this.btnAddAcc.Click += new System.EventHandler(this.btnAddAcc_Click);
            // 
            // CboNPH
            // 
            this.CboNPH.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CboNPH.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CboNPH.FormattingEnabled = true;
            this.CboNPH.Location = new System.Drawing.Point(0, 0);
            this.CboNPH.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.CboNPH.Name = "CboNPH";
            this.CboNPH.Size = new System.Drawing.Size(236, 24);
            this.CboNPH.TabIndex = 30;
            this.CboNPH.SelectedIndexChanged += new System.EventHandler(this.cboNPH_SelectedIndexChanged);
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(0, 0);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(200, 100);
            this.tabPage3.TabIndex = 0;
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 5000;
            this.toolTip.InitialDelay = 100;
            this.toolTip.ReshowDelay = 100;
            this.toolTip.ShowAlways = true;
            // 
            // btnEdit
            // 
            this.btnEdit.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnEdit.Location = new System.Drawing.Point(0, 0);
            this.btnEdit.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(81, 27);
            this.btnEdit.TabIndex = 72;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // TxtUser
            // 
            this.TxtUser.Dock = System.Windows.Forms.DockStyle.Top;
            this.TxtUser.Location = new System.Drawing.Point(0, 0);
            this.TxtUser.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TxtUser.Name = "TxtUser";
            this.TxtUser.Size = new System.Drawing.Size(207, 22);
            this.TxtUser.TabIndex = 24;
            this.TxtUser.WaterMark = "User...";
            this.TxtUser.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.TxtUser.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtUser.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.TxtUser.TextChanged += new System.EventHandler(this.txtUser_TextChanged);
            this.TxtUser.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUser_KeyDown);
            this.TxtUser.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtUser_KeyPress);
            // 
            // TxtPass
            // 
            this.TxtPass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TxtPass.Location = new System.Drawing.Point(0, 0);
            this.TxtPass.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TxtPass.Name = "TxtPass";
            this.TxtPass.PasswordChar = '*';
            this.TxtPass.Size = new System.Drawing.Size(207, 22);
            this.TxtPass.TabIndex = 25;
            this.TxtPass.WaterMark = "Pass...";
            this.TxtPass.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.TxtPass.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtPass.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.TxtPass.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPass_KeyDown);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnAddAcc);
            this.splitContainer1.Panel2.Controls.Add(this.btnEdit);
            this.splitContainer1.Size = new System.Drawing.Size(565, 54);
            this.splitContainer1.SplitterDistance = 479;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 75;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.panel3);
            this.splitContainer3.Panel1.Controls.Add(this.TxtUser);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.panel4);
            this.splitContainer3.Panel2.Controls.Add(this.panel1);
            this.splitContainer3.Size = new System.Drawing.Size(479, 54);
            this.splitContainer3.SplitterDistance = 207;
            this.splitContainer3.SplitterWidth = 5;
            this.splitContainer3.TabIndex = 75;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.chkShowPass);
            this.panel3.Controls.Add(this.TxtPass);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 27);
            this.panel3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(207, 27);
            this.panel3.TabIndex = 26;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.CboServer);
            this.panel4.Controls.Add(this.CboTail);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 27);
            this.panel4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(267, 27);
            this.panel4.TabIndex = 31;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.CboNPH);
            this.panel1.Controls.Add(this.btnPublisher);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(267, 27);
            this.panel1.TabIndex = 32;
            // 
            // tab
            // 
            this.tab.Controls.Add(this.tabPage1);
            this.tab.Controls.Add(this.tabPage5);
            this.tab.Controls.Add(this.tabPage2);
            this.tab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tab.Location = new System.Drawing.Point(0, 54);
            this.tab.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tab.Name = "tab";
            this.tab.SelectedIndex = 0;
            this.tab.Size = new System.Drawing.Size(565, 577);
            this.tab.TabIndex = 45;
            this.tab.SelectedIndexChanged += new System.EventHandler(this.tab_SelectedIndexChanged);
            this.tab.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tab_Selecting);
            this.tab.TabIndexChanged += new System.EventHandler(this.tab_TabIndexChanged);
            this.tab.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tab_MouseClick);
            this.tab.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tab_MouseDown);
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 4);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage1.Size = new System.Drawing.Size(557, 548);
            this.tabPage1.TabIndex = 5;
            this.tabPage1.Tag = "loged";
            this.tabPage1.Text = "Offline";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.buyClone2);
            this.tabPage5.Location = new System.Drawing.Point(4, 4);
            this.tabPage5.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage5.Size = new System.Drawing.Size(557, 548);
            this.tabPage5.TabIndex = 6;
            this.tabPage5.Text = "Clone";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // buyClone2
            // 
            this.buyClone2.BackColor = System.Drawing.Color.White;
            this.buyClone2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buyClone2.Location = new System.Drawing.Point(4, 4);
            this.buyClone2.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.buyClone2.Name = "buyClone2";
            this.buyClone2.Size = new System.Drawing.Size(549, 540);
            this.buyClone2.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 4);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage2.Size = new System.Drawing.Size(557, 548);
            this.tabPage2.TabIndex = 4;
            this.tabPage2.Text = "+";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // MicroLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tab);
            this.Controls.Add(this.splitContainer1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MicroLogin";
            this.Size = new System.Drawing.Size(565, 631);
            this.Load += new System.EventHandler(this.MicroLogin_Load);
            this.Resize += new System.EventHandler(this.MicroLogin_Resize);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tab.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnAddAcc;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button btnEdit;
        public System.Windows.Forms.ComboBox CboTail;
        public System.Windows.Forms.ComboBox CboServer;
        public System.Windows.Forms.ComboBox CboNPH;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage1;
        public System.Windows.Forms.CheckBox chkShowPass;
        public System.Windows.Forms.Button btnPublisher;
        public ChreneLib.Controls.TextBoxes.TextBoxEx TxtUser;
        public ChreneLib.Controls.TextBoxes.TextBoxEx TxtPass;
        public TabControlEx tab;
        private System.Windows.Forms.TabPage tabPage5;
        private BuyClone buyClone2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.SplitContainer splitContainer1;
    }
}