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
    public partial class GameItemForm : UserControl
    {
        public GameItemForm()
        {
            InitializeComponent();
        }




        private void btnRefresh_Click(object sender, EventArgs e)
        {
            lvName.Items.Clear();
            lvName.hideItems.Clear();
            lvName.dicHides.Clear();
            List<string> list = new List<string>()
            {
                "Điển Tịch","Trang Bị 1,2,3,4 Sao","Trang Bị 5 Sao","Trang Bị 10x,11x",
            };
            lvName.Items.AddRange(new HashSet<string>(list.Concat(PacketItem.AllName).Concat(PacketItem.AllType)).Select(i => new ListViewItem(i)).ToArray());
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            lvName.Search(txtSearch.Text);
        }

        private void menuXemAiDangCam_Click(object sender, EventArgs e)
        {
            if (lvName.SelectedItems.Count > 0)
            {
                string name = lvName.SelectedItems[0].Text;
                List<Game> list = Main.Instance.AllOnelineGame.Where(g => g.PacketItems.All.Concat(g.PacketItems.ThienCo).SelectMany(i => new[] { i.Name, i.TypeName }).Contains(name)).ToList();
                if (list.Count > 0)
                {
                    string listHave = string.Join(",", list.Select(o => o.TLBB.Name).ToArray());
                    if (MessageBox.Show(this, listHave + "\r\n" + "Bạn có muốn hiện những Nhân Vật Này", "MicroAuto", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        list.ForEach(g => g.Active());
                    }
                }
            }
        }
    }
}
