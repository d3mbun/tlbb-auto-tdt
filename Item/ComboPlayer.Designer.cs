
namespace _i
{
    partial class ComboPlayer
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
            this.lbSearch = new System.Windows.Forms.ListBox();
            this.cboPlayer = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // lbSearch
            // 
            this.lbSearch.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lbSearch.FormattingEnabled = true;
            this.lbSearch.Location = new System.Drawing.Point(0, 139);
            this.lbSearch.Name = "lbSearch";
            this.lbSearch.Size = new System.Drawing.Size(227, 56);
            this.lbSearch.TabIndex = 15;
            this.lbSearch.Visible = false;
            this.lbSearch.SelectedIndexChanged += new System.EventHandler(this.lbSearch_SelectedIndexChanged);
            // 
            // cboPlayer
            // 
            this.cboPlayer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.cboPlayer.FormattingEnabled = true;
            this.cboPlayer.Location = new System.Drawing.Point(0, 195);
            this.cboPlayer.Name = "cboPlayer";
            this.cboPlayer.Size = new System.Drawing.Size(227, 21);
            this.cboPlayer.TabIndex = 16;
            this.cboPlayer.DropDown += new System.EventHandler(this.cboPlayer_DropDown);
            this.cboPlayer.SelectedIndexChanged += new System.EventHandler(this.cboPlayer_SelectedIndexChanged);
            this.cboPlayer.TextChanged += new System.EventHandler(this.cboPlayer_TextChanged);
            // 
            // ComboPlayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbSearch);
            this.Controls.Add(this.cboPlayer);
            this.Name = "ComboPlayer";
            this.Size = new System.Drawing.Size(227, 216);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lbSearch;
        private System.Windows.Forms.ComboBox cboPlayer;
    }
}
