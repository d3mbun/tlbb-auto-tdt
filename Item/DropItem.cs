using FastColoredTextBoxNS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace _i
{
    partial class DropItem : UserControl
    {
        public DropItem()
        {
            InitializeComponent();
        }
        private void DropItem_Load(object sender, EventArgs e)
        {
            if (Tag != null)
            {
                if (Tag.ToString() == "DropItem")
                {
                    gameItemForm1.lvName.Columns[0].Text = "Hủy Đồ";
                }
                if (Tag.ToString() == "UseItem")
                {
                    gameItemForm1.lvName.Columns[0].Text = "Dùng Đồ";
                }
                if (Tag.ToString() == "SellItem")
                {
                    gameItemForm1.lvName.Columns[0].Text = "Bán Đồ";
                }
                if (Tag.ToString() == "BankItem")
                {
                    gameItemForm1.lvName.Columns[0].Text = "Thương Khố";
                }
                if (Tag.ToString() == "ThienCoItem")
                {
                    gameItemForm1.lvName.Columns[0].Text = "Thiên Cơ";
                }
                if (Tag.ToString() == "GomItem")
                {
                    gameItemForm1.lvName.Columns[0].Text = "Gom Đồ";
                }
            }
        }

    }
}
