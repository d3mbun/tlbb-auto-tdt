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
    public partial class Profile : UserControl
    {
        public Profile()
        {
            InitializeComponent();
        }

        private void sửaTênToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listViewEx1.SelectedItems.Count > 0)
            {
                var profile = listViewEx1.SelectedItems[0].Text;
                string input = Microsoft.VisualBasic.Interaction.InputBox("Điền Tên cho Profile", "MicroAuto", "", -1, -1);
                Game.IniParser.Write("profile", profile, input);
                listViewEx1.Items.Clear();
                ListViewItem i = new ListViewItem("#");
                i.SubItems.Add(Game.IniParser.Read("profile", "#"));
                listViewEx1.Items.Add(i);
                for (int j = 1; j < 16; j++)
                {
                    ListViewItem it = new ListViewItem("#" + j);
                    it.SubItems.Add(Game.IniParser.Read("profile", "#" + j));
                    listViewEx1.Items.Add(it);
                }
            }
        }

        private void Profile_Load(object sender, EventArgs e)
        {
            listViewEx1.Items.Clear();
            ListViewItem i = new ListViewItem("#");
            i.SubItems.Add(Game.IniParser.Read("profile", "#"));
            listViewEx1.Items.Add(i);
            for(int j = 1; j < 16; j++)
            {
                ListViewItem it = new ListViewItem("#" + j);
                it.SubItems.Add(Game.IniParser.Read("profile", "#" + j));
                listViewEx1.Items.Add(it);
            }
        }

        private void xóaThiếtLậpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listViewEx1.SelectedItems.Count > 0)
            {
                var profile = listViewEx1.SelectedItems[0].Text;
                if (profile == "#")
                {
                    MessageBox.Show(this, "Bạn không thể xóa thiết lập gốc\r\nMuốn xóa thì thoát auto. xóa file nier.ini nhé" + profile, "MicroAuto", MessageBoxButtons.OK);
                }
                else
                {
                    if (MessageBox.Show(this, "Bạn có chắc chắn muốn xóa thiết lập của " + profile, "MicroAuto", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Game.IniParser.DeleteSectionContain(profile);
                    }
                }
            }
        }
    }
}
