using System;
using System.Windows.Forms;

public class LvEx : ListView
{
    public LvEx()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
    }

 
}