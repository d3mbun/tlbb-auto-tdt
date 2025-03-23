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
    partial class SellItem : UserControl
    {
        public SellItem()
        {
            InitializeComponent();
        }

        private void SellItem_Load(object sender, EventArgs e)
        {
            txtSellName.Text = SettingOld.SellName;
            lvName.Items.AddRange(PacketItem.AllName.Concat(PacketItem.AllType).Select(i => new ListViewItem(i)).ToArray());
            foreach (ListViewItem i in lvDinhSan.Items)
            {
                if (SettingOld.SellDinhSan.Contains(i.Text))
                    i.Checked = true;
            }
            tabPage2.Controls.Add(new SellItemPS() { Dock = DockStyle.Fill });
        }

        private void AddName()
        {
            foreach (ListViewItem item in lvName.SelectedItems)
            {
                if (item.Text.Trim() != "")
                {
                    bool isHave = false;
                    foreach (string str in txtSellName.Text.Split('\n'))
                    {
                        if (TDT.VietLien(str) == TDT.VietLien(item.Text))
                            isHave = true;
                    }
                    if (!isHave)
                        txtSellName.Text = txtSellName.Text.Trim() + "\r\n" + item.Text;
                }
            }
            txtSellName.ScrollToEnd();
        }

 

        private void menuAddName_Click(object sender, EventArgs e)
        {
            AddName();
        }

        private void menuAddType_Click(object sender, EventArgs e)
        {
            //AddType();
        }

        private void listViewName_DoubleClick(object sender, EventArgs e)
        {
            AddName();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            SettingOld.SellName = txtSellName.Text;
            string note = "";
            foreach(ListViewItem i in lvDinhSan.Items)
            {
                if(i.Checked)
                    note += "[" + i.Text.Trim() + "]";
            }
            SettingOld.SellDinhSan = note;
            foreach (KeyValuePair<int, Game> kvp in Main.DicGame)
            {
                kvp.Value.LoadDrop();
            }
        }

        private void txtSearchName_TextChanged(object sender, EventArgs e)
        {
            lvName.Search(txtSearchName.Text);
        }



        private void lvDinhSan_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBoxEx1_TextChanged(object sender, EventArgs e)
        {
            txtSellName.Search(textBoxEx1.Text);
        }

        private void txtSellName_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
