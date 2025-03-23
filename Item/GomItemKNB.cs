using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _i
{
    public partial class GomItemKNB : UserControl
    {
        public GomItemKNB()
        {
            InitializeComponent();
        }

        private void GomItem_Load(object sender, EventArgs e)
        {
            txtName.Text = SettingOld.GomKNBName;
            lvName.Items.AddRange(PacketItem.AllName.Concat(PacketItem.AllType).Select(i => new ListViewItem(i)).ToArray());            
        }

        private void AddName()
        {
            foreach (ListViewItem item in lvName.SelectedItems)
            {
                if (item.Text.Trim() != "")
                {
                    bool isHave = false;
                    foreach (string str in txtName.Text.Split('\n'))
                    {
                        if (TDT.VietLien(str).Replace("[locked]", "") == TDT.VietLien(item.Text))
                            isHave = true;
                    }
                    if (!isHave)
                    {
                        txtName.Text = txtName.Text.Trim() + "\r\n" + item.Text;
                    }
                }
            }
            txtName.ScrollToEnd();
        }

     

        private void menuAddName_Click(object sender, EventArgs e)
        {
            AddName();
        }

        private void listViewName_DoubleClick(object sender, EventArgs e)
        {
            AddName();
        }

     

        private void txtSearchName_TextChanged(object sender, EventArgs e)
        {
            lvName.Search(txtSearchName.Text);
        }

     

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            SettingOld.GomKNBName = txtName.Text;
            foreach (KeyValuePair<int, Game> kvp in Main.DicGame)
            {
                kvp.Value.LoadDrop();
            }
        }

     
    }
}
