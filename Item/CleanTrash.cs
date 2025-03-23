using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace _i
{
    public partial class CleanTrash : UserControl
    {
        public CleanTrash()
        {
            InitializeComponent();
            Tag = "Dọn Rác";
        }

        private void CleanTras_Load(object sender, EventArgs e)
        {
            RefreshList();
        }

        private void RefreshList()
        {
            lvItems.Items.Clear();
            List<ListViewItem> list = new List<ListViewItem>();
            List<string> deleted = new List<string>();
            foreach (KeyValuePair<int, Game> kvp in Main.DicGame)
            {
                var game = kvp.Value;
                if (game.TLBB.Online)
                {
                    foreach (var i in game.PacketItems.All.Concat(game.PacketItems.ThienCo))
                    {
                        if (i.TypeName == "Đơn Đoản" && i.Lvl == 1)
                            continue;
                        if (deleted.Contains(game.TLBB.Name + "|" + i.Name + "|" + i.TypeName + " => " + "Cấp " + i.Lvl + " | " + i.Star + " Sao " + i.Line + " Dòng"))
                            continue;
                        if (Game.TrangBi.Contains(i.TypeName) || Game.Weapon.Contains(i.TypeName))
                        {
                            if (i.Star <= 4)
                            {
                                ListViewItem item = new ListViewItem(game.TLBB.Name);
                                item.SubItems.Add(i.Name);
                                item.SubItems.Add(i.TypeName + " => " + "Cấp " + i.Lvl + " | " + i.Star + " Sao " + i.Line + " Dòng");
                                deleted.Add(game.TLBB.Name + "|" + i.Name + "|" + i.TypeName + " => " + "Cấp " + i.Lvl + " | " + i.Star + " Sao " + i.Line + " Dòng");
                                item.Tag = game;
                                list.Add(item);

                            }
                        }
                    }
                }
            }
            lvItems.Items.AddRange(list.ToArray());
        }

        private void hủyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Bạn có chắc muốn hủy " + lvItems.SelectedItems.Count + " vật phẩm này chứ", "MicroAuto", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    foreach (ListViewItem item in lvItems.SelectedItems)
                    {
                        string name = item.SubItems[1].Text;
                        Game game = item.Tag as Game;
                        foreach (var i in game.PacketItems.All)
                        {
                            if (i.Name == name && i.TypeName + " => " + "Cấp " + i.Lvl + " | " + i.Star + " Sao " + i.Line + " Dòng" == item.SubItems[2].Text)
                            {
                                game.PostMessage((int)i.Index, 108);
                            }
                        }
                        foreach (PacketItem p in game.PacketItems.ThienCo)
                        {
                            if (p.Name == name && p.TypeName + " => " + "Cấp " + p.Lvl + " | " + p.Star + " Sao " + p.Line + " Dòng" == item.SubItems[2].Text)
                            {
                                game.DropItemThienCo(p.Index);
                            }
                        }
                    }
                    Thread.Sleep(2000);
                    RefreshList();
                }).Start();

            }
        }

        private void copyĐểDánVàoDanhSáchHủyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string copy = "";
            List<string> listName = new List<string>();
            foreach (ListViewItem item in lvItems.SelectedItems)
            {
                string name = item.SubItems[1].Text;
                if (!listName.Contains(name))
                {
                    listName.Add(name);
                    copy += name + "\r\n";
                }
            }
            try
            {
                Clipboard.SetText(copy);
            }
            catch { }
        }

        private void listViewEx1_DoubleClick(object sender, EventArgs e)
        {
            if(lvItems.SelectedItems.Count > 0)
            {
                var game = lvItems.SelectedItems[0].Tag as Game;
                game.Active();
            }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Bạn có chắc muốn hủy " + lvItems.Items.Count + " vật phẩm này chứ", "MicroAuto", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    foreach (ListViewItem item in lvItems.Items)
                    {
                        string name = item.SubItems[1].Text;
                        Game game = item.Tag as Game;
                        foreach (var i in game.PacketItems.All)
                        {
                            if (i.Name == name && i.TypeName + " => " + "Cấp " + i.Lvl + " | " + i.Star + " Sao " + i.Line + " Dòng" == item.SubItems[2].Text)
                            {
                                game.PostMessage((int)i.Index, 108);
                            }
                        }
                        foreach (PacketItem p in game.PacketItems.ThienCo)
                        {
                            if (p.Name == name && p.TypeName + " => " + "Cấp " + p.Lvl + " | " + p.Star + " Sao " + p.Line + " Dòng" == item.SubItems[2].Text)
                            {
                                game.DropItemThienCo(p.Index);
                            }
                        }
                    }
                    Thread.Sleep(2000);
                    RefreshList();
                }).Start();

            }
        }
    }
}
