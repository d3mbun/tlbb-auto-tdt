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
    public partial class LayDo : UserControl
    {
        public LayDo()
        {
            InitializeComponent();
        }

        private void LayDo_Load(object sender, EventArgs e)
        {
            txtDropName.Text = SettingOld.LayName;
            lvName.Items.AddRange(PacketItem.AllName.Concat(PacketItem.AllType).Select(i => new ListViewItem(i)).ToArray());            
            checkBox1.Checked = Setting.Is("checkKhongLayCoDinh");
            tabPage2.Controls.Add(new LayDoThienCo() { Dock = DockStyle.Fill });
        }

        private void listViewName_DoubleClick(object sender, EventArgs e)
        {
            AddName();
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


        private void btnOk_Click(object sender, EventArgs e)
        {
    
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
        }

    

        private void txtSearchName_TextChanged(object sender, EventArgs e)
        {
            lvName.Search(txtSearchName.Text);
        }

     

        private void txtDropName_TextChanged(object sender, EventArgs e)
        {
            SettingOld.LayName = txtDropName.Text;
            foreach (KeyValuePair<int, Game> kvp in Main.DicGame)
            {
                kvp.Value.LoadDrop();
            }
        }

   

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            Setting.DicChecked["checkKhongLayCoDinh"] = checkBox1.Checked;
        }
    }
}
