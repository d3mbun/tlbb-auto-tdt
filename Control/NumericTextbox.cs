using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _i
{
    public class NumericTextBox : TextBox
    {
        #region Fields

        #region Protected Fields

        protected string _waterMarkText = "Default Watermark..."; //The watermark text
        protected Color _waterMarkColor; //Color of the watermark when the control does not have focus
        protected Color _waterMarkActiveColor; //Color of the watermark when the control has focus

        #endregion

        #region Private Fields

        private Panel waterMarkContainer; //Container to hold the watermark
        private Font waterMarkFont; //Font of the watermark
        private SolidBrush waterMarkBrush; //Brush for the watermark

        #endregion

        #endregion

        #region Constructors

        public NumericTextBox()
        {
            Initialize();
            KeyDown += TextBoxEx_KeyDown;
        }

        private void TextBoxEx_KeyDown(object sender, KeyEventArgs e)
        {
            var txt = sender as System.Windows.Forms.TextBox;
            if (e.Control && e.KeyCode == Keys.A)
                txt.SelectAll();
        }

        public void ScrollToEnd()
        {
            SelectionStart = TextLength;
            ScrollToCaret();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes watermark properties and adds CtextBox events
        /// </summary>
        private void Initialize()
        {
            //Sets some default values to the watermark properties
            _waterMarkColor = Color.LightGray;
            _waterMarkActiveColor = Color.Gray;
            waterMarkFont = this.Font;
            waterMarkBrush = new SolidBrush(_waterMarkActiveColor);
            waterMarkContainer = null;

            //Draw the watermark, so we can see it in design time
            DrawWaterMark();

            //Eventhandlers which contains function calls. 
            //Either to draw or to remove the watermark
            this.Enter += new EventHandler(ThisHasFocus);
            this.Leave += new EventHandler(ThisWasLeaved);
            this.TextChanged += new EventHandler(ThisTextChanged);
        }

        /// <summary>
        /// Removes the watermark if it should
        /// </summary>
        private void RemoveWaterMark()
        {
            if (waterMarkContainer != null)
            {
                this.Controls.Remove(waterMarkContainer);
                waterMarkContainer = null;
            }
        }

        /// <summary>
        /// Draws the watermark if the text length is 0
        /// </summary>
        private void DrawWaterMark()
        {
            if (this.waterMarkContainer == null && this.TextLength <= 0)
            {
                waterMarkContainer = new Panel(); // Creates the new panel instance
                waterMarkContainer.Paint += new PaintEventHandler(waterMarkContainer_Paint);
                waterMarkContainer.Invalidate();
                waterMarkContainer.Click += new EventHandler(waterMarkContainer_Click);
                this.Controls.Add(waterMarkContainer); // adds the control
            }
        }

        #endregion

        #region Eventhandlers

        #region WaterMark Events

        private void waterMarkContainer_Click(object sender, EventArgs e)
        {
            this.Focus(); //Makes sure you can click wherever you want on the control to gain focus
        }

        private void waterMarkContainer_Paint(object sender, PaintEventArgs e)
        {
            //Setting the watermark container up
            waterMarkContainer.Location = new Point(2, 0); // sets the location
            waterMarkContainer.Height = this.Height; // Height should be the same as its parent
            waterMarkContainer.Width = this.Width; // same goes for width and the parent
            waterMarkContainer.Anchor = AnchorStyles.Left | AnchorStyles.Right; // makes sure that it resizes with the parent control



            if (this.ContainsFocus)
            {
                //if focused use normal color
                waterMarkBrush = new SolidBrush(this._waterMarkActiveColor);
            }

            else
            {
                //if not focused use not active color
                waterMarkBrush = new SolidBrush(this._waterMarkColor);
            }

            //Drawing the string into the panel 
            Graphics g = e.Graphics;
            g.DrawString(this._waterMarkText, waterMarkFont, waterMarkBrush, new PointF(-2f, 1f));//Take a look at that point
            //The reason I'm using the panel at all, is because of this feature, that it has no limits
            //I started out with a label but that looked very very bad because of its paddings 

        }

        #endregion

        #region CTextBox Events

        private void ThisHasFocus(object sender, EventArgs e)
        {
            //if focused use focus color
            waterMarkBrush = new SolidBrush(this._waterMarkActiveColor);

            //The watermark should not be drawn if the user has already written some text
            if (this.TextLength <= 0)
            {
                RemoveWaterMark();
                DrawWaterMark();
            }
        }

        private void ThisWasLeaved(object sender, EventArgs e)
        {
            //if the user has written something and left the control
            if (this.TextLength > 0)
            {
                //Remove the watermark
                RemoveWaterMark();
            }
            else
            {
                //But if the user didn't write anything, Then redraw the control.
                this.Invalidate();
            }
        }

        private void ThisTextChanged(object sender, EventArgs e)
        {
            //If the text of the textbox is not empty
            if (this.TextLength > 0)
            {
                //Remove the watermark
                RemoveWaterMark();
            }
            else
            {
                //But if the text is empty, draw the watermark again.
                DrawWaterMark();
            }
        }

        #region Overrided Events

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            //Draw the watermark even in design time
            DrawWaterMark();
        }

        protected override void OnInvalidated(InvalidateEventArgs e)
        {
            base.OnInvalidated(e);
            //Check if there is a watermark
            if (waterMarkContainer != null)
                //if there is a watermark it should also be invalidated();
                waterMarkContainer.Invalidate();
        }

        #endregion

        #endregion

        #endregion

        #region Properties
        [Category("Watermark attribtues")]
        [Description("Sets the text of the watermark")]
        public string WaterMark
        {
            get { return this._waterMarkText; }
            set
            {
                this._waterMarkText = value;
                this.Invalidate();
            }
        }

        [Category("Watermark attribtues")]
        [Description("When the control gaines focus, this color will be used as the watermark's forecolor")]
        public Color WaterMarkActiveForeColor
        {
            get { return this._waterMarkActiveColor; }

            set
            {
                this._waterMarkActiveColor = value;
                this.Invalidate();
            }
        }

        [Category("Watermark attribtues")]
        [Description("When the control looses focus, this color will be used as the watermark's forecolor")]
        public Color WaterMarkForeColor
        {
            get { return this._waterMarkColor; }

            set
            {
                this._waterMarkColor = value;
                this.Invalidate();
            }
        }

        [Category("Watermark attribtues")]
        [Description("The font used on the watermark. Default is the same as the control")]
        public Font WaterMarkFont
        {
            get
            {
                return this.waterMarkFont;
            }

            set
            {
                this.waterMarkFont = value;
                this.Invalidate();
            }
        }

        #endregion

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            NumberFormatInfo fi = CultureInfo.CurrentCulture.NumberFormat;

            string c = e.KeyChar.ToString();
            if (char.IsDigit(c, 0))
                return;

            if ((SelectionStart == 0) && (c.Equals(fi.NegativeSign)))
                return;

            // copy/paste
            if ((((int)e.KeyChar == 22) || ((int)e.KeyChar == 3))
                && ((ModifierKeys & Keys.Control) == Keys.Control))
                return;

            if (e.KeyChar == '\b')
                return;

            e.Handled = true;
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_PASTE = 0x0302;
            if (m.Msg == WM_PASTE)
            {
                string text = Clipboard.GetText();
                if (string.IsNullOrEmpty(text))
                    return;

                if ((text.IndexOf('+') >= 0) && (SelectionStart != 0))
                    return;

                int i;
                if (!int.TryParse(text, out i)) // change this for other integer types
                    return;

                if ((i < 0) && (SelectionStart != 0))
                    return;
            }
            base.WndProc(ref m);
        }
    }
}