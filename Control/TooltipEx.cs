using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _i
{
    class ToolTipEx : ToolTip
    {
        public ToolTipEx()
        {
            BackColor = Color.Black;
            ForeColor = Color.LimeGreen;
            OwnerDraw = true;
            Draw += ToolTipEx_Draw;
        }

        private void ToolTipEx_Draw(object sender, DrawToolTipEventArgs e)
        {
            e.DrawBackground();
            e.DrawBorder();
            e.DrawText();
        }
    }
}
