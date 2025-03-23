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
    public partial class ConfigBossMap : UserControl
    {
        public ConfigBossMap()
        {
            InitializeComponent();
        }

        int curMapId = -1;
        Game game = null;

        private void ConfigBossMap_Load(object sender, EventArgs e)
        {            
            game = Main.CurGame;
            label3.Text = game.TLBB.MapName;
            checkBox1.Checked = game.IsRunAutoBossMap;
            chkVeLacDuong.Checked = Global.IsHetBossVeLacDuong;
            if (game == null)
            {
                Dispose();
                return;
            }
            HashSet<string> DropName = new HashSet<string>();
            txtOnlyBossMap.Text = SettingOld.OnlyBossMap;
            foreach (KeyValuePair<int, Game> kvp in Main.DicGame)
            {
                var game = kvp.Value;
                foreach (GameObject _object in game.Objects.All)
                {
                    if (_object.Name.Trim() == "")
                        continue;
                    if (!DropName.Contains(_object.Name))
                    {
                        DropName.Add(_object.Name);
                        listviewmonter.Items.Add(_object.Name);
                    }
                }
            }


            Text = "[" + game.TLBB.Name + "] Cài Đặt BossMap trên => " + Main.CurGame.TLBB.MapName;
            curMapId = (int)Main.CurGame.TLBB.MapId;
            string txt = SettingOld.LoadBossMAP(game.TLBB.MapId.ToString());        
            foreach (string s in txt.Split('-'))
            {
                int x = 0;
                int y = 0;
                try
                {
                    x = TDT.ParseInt(s.Split(',')[0]);
                    y = TDT.ParseInt(s.Split(',')[1]);
                }
                catch { }
                if (x != 0 && y != 0)
                {
                    lstv.Items.Add(new ListViewItem(x + "," + y));
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string txt = ((int)game.CharX).ToString() + "," + ((int)game.CharY).ToString();
            if (lstv.Items.Count > 0 && lstv.Items[lstv.Items.Count - 1].Text == txt)
                return;
            ListViewItem newItem = new ListViewItem(txt);
            lstv.Items.Add(newItem);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string txt = string.Empty;
            foreach (ListViewItem item in lstv.Items)
            {
                txt += item.Text + "-";
            }
            txt = txt.Trim('-');
            SettingOld.SaveBossMAP(game.TLBB.MapId.ToString(), txt);
        }

        private void xóaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            while (lstv.SelectedItems.Count > 0)
            {
                lstv.Items.Remove(lstv.SelectedItems[0]);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            game.IsRunAutoBossMap = checkBox1.Checked;
        }

        private void textBoxEx2_TextChanged(object sender, EventArgs e)
        {
            listviewmonter.Search(textBoxEx2.Text);
        }

        private void listviewmonter_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listviewmonter.SelectedItems)
            {
                if (item.Text.Trim() != "")
                {
                    bool isHave = false;
                    foreach (string str in txtOnlyBossMap.Text.Split('\n'))
                    {
                        if (TDT.VietLien(str) == TDT.VietLien(item.Text))
                            isHave = true;
                    }
                    if (!isHave)
                        txtOnlyBossMap.Text = txtOnlyBossMap.Text.Trim() + "\r\n" + item.Text;
                }
            }
            txtOnlyBossMap.ScrollToEnd();
        }

        private void txtOnlyBossMap_TextChanged(object sender, EventArgs e)
        {
            SettingOld.OnlyBossMap = txtOnlyBossMap.Text;
        }

        private void chkVeLacDuong_CheckedChanged(object sender, EventArgs e)
        {
            Global.IsHetBossVeLacDuong = chkVeLacDuong.Checked;
        }
    }
}
