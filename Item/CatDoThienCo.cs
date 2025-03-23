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
    public partial class CatDoThienCo : UserControl
    {
        public CatDoThienCo()
        {
            InitializeComponent();
        }

        private void CatDo_Load(object sender, EventArgs e)
        {
            txtDropName.Text = SettingOld.CatThienCoName;
            lvName.Items.AddRange(PacketItem.AllName.Concat(PacketItem.AllType).Concat(PacketItem.AllType).Select(i => new ListViewItem(i)).ToArray());            
        }

        private void AddName()
        {
            foreach (ListViewItem item in lvName.SelectedItems)
            {
                if (item.Text.Trim() != "")
                {
                    bool isHave = false;
                    foreach (string str in txtDropName.Text.Split('\n'))
                    {
                        if (TDT.VietLien(str) == TDT.VietLien(item.Text))
                            isHave = true;
                    }
                    if (!isHave)
                    {
                        txtDropName.Text = txtDropName.Text.Trim() + "\r\n" + item.Text;
                    }
                }
            }
            txtDropName.ScrollToEnd();
        }

    

        private void listViewName_DoubleClick(object sender, EventArgs e)
        {
            AddName();
        }

     
        private void menuAddName_Click(object sender, EventArgs e)
        {
            AddName();
        }

        private void menuAddType_Click(object sender, EventArgs e)
        {
        }


       

        private void txtSearchName_TextChanged(object sender, EventArgs e)
        {
            lvName.Search(txtSearchName.Text);
        }

  

        private void txtDropName_TextChanged(object sender, EventArgs e)
        {
            SettingOld.CatThienCoName = txtDropName.Text;
            foreach (KeyValuePair<int, Game> kvp in Main.DicGame)
            {
                kvp.Value.LoadDrop();
            }
        }

    }
}
