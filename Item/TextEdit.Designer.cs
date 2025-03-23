
namespace _i
{
    partial class TextEdit
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
            this.lbSearch = new System.Windows.Forms.ListBox();
            this.text = new FastColoredTextBoxNS.FastColoredTextBox();
            this.txtSearch = new ChreneLib.Controls.TextBoxes.TextBoxEx();
            ((System.ComponentModel.ISupportInitialize)(this.text)).BeginInit();
            this.SuspendLayout();
            // 
            // lbSearch
            // 
            this.lbSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbSearch.FormattingEnabled = true;
            this.lbSearch.Location = new System.Drawing.Point(0, 20);
            this.lbSearch.Name = "lbSearch";
            this.lbSearch.Size = new System.Drawing.Size(536, 56);
            this.lbSearch.TabIndex = 18;
            this.lbSearch.Visible = false;
            this.lbSearch.SelectedIndexChanged += new System.EventHandler(this.lbSearch_SelectedIndexChanged);
            // 
            // text
            // 
            this.text.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.text.AutoIndentCharsPatterns = "^\\s*[\\w\\.]+(\\s\\w+)?\\s*(?<range>=)\\s*(?<range>[^;=]+);\n^\\s*(case|default)\\s*[^:]*(" +
    "?<range>:)\\s*(?<range>[^;]+);";
            this.text.AutoScrollMinSize = new System.Drawing.Size(2, 13);
            this.text.BackBrush = null;
            this.text.CharHeight = 13;
            this.text.CharWidth = 7;
            this.text.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.text.DefaultMarkerSize = 8;
            this.text.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.text.Dock = System.Windows.Forms.DockStyle.Fill;
            this.text.Font = new System.Drawing.Font("Courier New", 9F);
            this.text.IsReplaceMode = false;
            this.text.Location = new System.Drawing.Point(0, 20);
            this.text.Name = "text";
            this.text.Paddings = new System.Windows.Forms.Padding(0);
            this.text.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.text.ServiceColors = null;
            this.text.ShowLineNumbers = false;
            this.text.Size = new System.Drawing.Size(536, 472);
            this.text.TabIndex = 17;
            this.text.Zoom = 100;
            // 
            // txtSearch
            // 
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtSearch.Location = new System.Drawing.Point(0, 0);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(536, 20);
            this.txtSearch.TabIndex = 16;
            this.txtSearch.WaterMark = "Search...";
            this.txtSearch.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtSearch.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearch.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // TextEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbSearch);
            this.Controls.Add(this.text);
            this.Controls.Add(this.txtSearch);
            this.Name = "TextEdit";
            this.Size = new System.Drawing.Size(536, 492);
            ((System.ComponentModel.ISupportInitialize)(this.text)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbSearch;
        private FastColoredTextBoxNS.FastColoredTextBox text;
        private ChreneLib.Controls.TextBoxes.TextBoxEx txtSearch;
    }
}
