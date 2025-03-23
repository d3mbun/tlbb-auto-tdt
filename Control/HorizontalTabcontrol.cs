using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MicroAuto.Control
{
    class HorizontalTabcontrol : TabControl
    {
        public HorizontalTabcontrol()
        {
            DrawItem += new DrawItemEventHandler(tabControl_DrawItem);
        }

        private void tabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            var tab = sender as TabControl;
            Graphics g = e.Graphics;
            Brush _textBrush;

            // Get the item from the collection.
            TabPage _tabPage = tab.TabPages[e.Index];

            // Get the real bounds for the tab rectangle.
            Rectangle _tabBounds = tab.GetTabRect(e.Index);

            _textBrush = new System.Drawing.SolidBrush(Color.Black);

            // Use our own font.
            Font _tabFont = new Font("Arial", (float)12.0, FontStyle.Bold, GraphicsUnit.Pixel);

            // Draw string. Center the text.
            StringFormat _stringFlags = new StringFormat();
            _stringFlags.Alignment = StringAlignment.Center;
            _stringFlags.LineAlignment = StringAlignment.Center;
            g.DrawString(_tabPage.Text, _tabFont, _textBrush, _tabBounds, new StringFormat(_stringFlags));
        }
    }
}
