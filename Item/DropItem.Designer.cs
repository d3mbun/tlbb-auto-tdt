namespace _i
{
    partial class DropItem
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
            this.btnSave = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.textEdit1 = new _i.TextEdit();
            this.comboPlayer1 = new _i.ComboPlayer();
            this.gameItemForm1 = new _i.GameItemForm();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnSave.Location = new System.Drawing.Point(0, 383);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(442, 23);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.textEdit1);
            this.splitContainer1.Panel1.Controls.Add(this.comboPlayer1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gameItemForm1);
            this.splitContainer1.Size = new System.Drawing.Size(442, 383);
            this.splitContainer1.SplitterDistance = 217;
            this.splitContainer1.TabIndex = 0;
            // 
            // textEdit1
            // 
            this.textEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textEdit1.Location = new System.Drawing.Point(0, 0);
            this.textEdit1.Name = "textEdit1";
            this.textEdit1.Size = new System.Drawing.Size(217, 362);
            this.textEdit1.TabIndex = 1;
            // 
            // comboPlayer1
            // 
            this.comboPlayer1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.comboPlayer1.Location = new System.Drawing.Point(0, 362);
            this.comboPlayer1.Name = "comboPlayer1";
            this.comboPlayer1.Size = new System.Drawing.Size(217, 21);
            this.comboPlayer1.TabIndex = 0;
            // 
            // gameItemForm1
            // 
            this.gameItemForm1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gameItemForm1.Location = new System.Drawing.Point(0, 0);
            this.gameItemForm1.Name = "gameItemForm1";
            this.gameItemForm1.Size = new System.Drawing.Size(221, 383);
            this.gameItemForm1.TabIndex = 0;
            // 
            // DropItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.btnSave);
            this.Name = "DropItem";
            this.Size = new System.Drawing.Size(442, 406);
            this.Tag = "Hủy Vật Phẩm - Drop Item";
            this.Load += new System.EventHandler(this.DropItem_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private GameItemForm gameItemForm1;
        private ComboPlayer comboPlayer1;
        private TextEdit textEdit1;
    }
}