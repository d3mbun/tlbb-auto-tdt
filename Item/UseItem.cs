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
    partial class UseItem : UserControl
    {
        public UseItem()
        {
            InitializeComponent();
        }

        private void UseItem_Load(object sender, EventArgs e)
        {
            txtName.Text = SettingOld.UseName;
            lvName.Items.AddRange(PacketItem.AllName.Concat(PacketItem.AllType).Select(i => new ListViewItem(i)).ToArray());
            lvName.Items.AddRange(PacketItem.AllType.Select(i => new ListViewItem(i)).ToArray());
            List<string> listChanNguyen = new List<string>();
            foreach(var game in Main.Instance.AllOnelineGame)
            {
                foreach(var item in game.PacketItems.TuiChanNguyen)
                {
                    if (!listChanNguyen.Contains(item.Name))
                    {
                        listChanNguyen.Add(item.Name);
                    }
                }
            }

            lvName.Items.AddRange(listChanNguyen.Select(i => new ListViewItem(i)).ToArray());


            //chkAutoGetThienCo.Checked = OptionEx.IsAutoGetThienCoUse;
        }


   

        private void listViewName_DoubleClick(object sender, EventArgs e)
        {
        }

      


        private void btnOk_Click(object sender, EventArgs e)
        {
            SettingOld.UseName = txtName.Text;
            foreach (KeyValuePair<int, Game> kvp in Main.DicGame)
            {
                kvp.Value.LoadDrop();
            }
        }

        private void txtSearchName_TextChanged(object sender, EventArgs e)
        {
            lvName.Search(txtSearchName.Text);
        }

     

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            cboSearchName.Items.Clear();
            foreach (var game in Main.Instance.AllOnelineGame)
            {
                cboSearchName.Items.Add(game.TLBB.Name);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            var combobox = sender as ComboBox;
            // get the keyword to search
            string textToSearch = combobox.Text.VietLien();
            lbSearch.Visible = false; // hide the listbox, see below for why doing that
            if (String.IsNullOrEmpty(textToSearch))
                return; // return with listbox's Visible set to false if the keyword is empty
                        //search
            string[] result = (from i in Main.Instance.AllOnelineGame.Select(o => o.TLBB.Name)
                               where i.VietLien().Contains(textToSearch)
                               select i).ToArray();
            if (result.Length == 0)
                return; // return with listbox's Visible set to false if nothing found

            lbSearch.Items.Clear(); // remember to Clear before Add
            lbSearch.Items.AddRange(result);
            lbSearch.Visible = true; // show the listbox again
        }

        private void lbSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboSearchName.Text = lbSearch.SelectedItem.ToString();
            lbSearch.Visible = false;
        }

        private void txtType_TextChanged(object sender, EventArgs e)
        {

        }

        private void menuAddName_Click_1(object sender, EventArgs e)
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
                        if (cboSearchName.Text.Trim().Length > 0)
                            txtName.Text = txtName.Text.Trim() + " {" + cboSearchName.Text + "}";
                    }
                }
            }
            //txtDropName.SelectionStart = txtDropName.TextLength;
            txtName.ScrollToEnd();
        }

        private void thêmCốĐịnhDoubleClickInLockedToolStripMenuItem_Click(object sender, EventArgs e)
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
                        txtName.Text = txtName.Text.Trim() + "\r\n" + item.Text + " [Locked]";
                        if (cboSearchName.Text.Trim().Length > 0)
                            txtName.Text = txtName.Text.Trim() + " {" + cboSearchName.Text + "}";
                    }
                }
            }
            //txtDropName.SelectionStart = txtDropName.TextLength;
            txtName.ScrollToEnd();
        }

        private void xemAiĐangCầmToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {

        }

        private void lvName_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            var hitInfo = lvName.HitTest(e.Location);

            int subItemIndex = hitInfo.Item.SubItems.IndexOf(hitInfo.SubItem);


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
                        if (subItemIndex == -1)
                            txtName.Text = txtName.Text.Trim() + "\r\n" + item.Text + " [Locked]";
                        else
                            txtName.Text = txtName.Text.Trim() + "\r\n" + item.Text;
                        if (cboSearchName.Text.Trim().Length > 0)
                            txtName.Text = txtName.Text.Trim() + " {" + cboSearchName.Text + "}";
                    }
                }
            }

            txtName.ScrollToEnd();
        }
    }
}
