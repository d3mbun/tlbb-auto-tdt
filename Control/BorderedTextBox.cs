using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;

namespace _i
{
    class BorderTextBox : Panel
    {
        private TextBox textbox;
        private bool focusedAlways = false;
        private Color normalBorderColor = Color.Gray;
        private Color focusedBorderColor = Color.Red;

        public BorderTextBox()
        {
            this.DoubleBuffered = true;
            this.Padding = new Padding(2);

            this.textbox = new TextBox();
            this.textbox.AutoSize = false;
            this.textbox.BorderStyle = BorderStyle.None;
            this.textbox.Dock = DockStyle.Fill;
            this.textbox.MaxLength = 4;
            this.textbox.KeyPress += new KeyPressEventHandler(TextBox_KeyPress);
            this.textbox.Enter += new EventHandler(this.TextBox_Refresh);
            this.textbox.Leave += new EventHandler(this.TextBox_Refresh);
            this.textbox.Resize += new EventHandler(this.TextBox_Refresh);
            this.Controls.Add(this.textbox);            
        }

        void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
            (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void TextBox_Refresh(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(SystemColors.Window);
            using (Pen borderPen = new Pen(this.TextBox.Focused || FocusedAlways ?
                focusedBorderColor : normalBorderColor))
            {
                e.Graphics.DrawRectangle(borderPen,
                    new Rectangle(0, 0, this.ClientSize.Width - 1, this.ClientSize.Height - 1));
            }
            base.OnPaint(e);
        }

        public TextBox TextBox
        {
            get { return textbox; }
            set { textbox = value; }
        }

        public bool FocusedAlways
        {
            get { return focusedAlways; }
            set { focusedAlways = value; }
        }
    }
}
