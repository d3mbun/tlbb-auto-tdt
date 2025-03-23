using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _i
{
    partial class TuyetGiao : UserControl
    {
        Game game;
        public TuyetGiao(Game game)
        {
            InitializeComponent();
            this.game = game;
            LvFriend.Columns[0].Text = "Name [" + game.TLBB.Name + "]";
            LoadFriend();
        }

        void LoadFriend()
        {
            Task.Run(() =>
            {
                LvFriend.Items.Clear();
                game.DoStringEx("local online = ''; for i = 1 , 8 do local j = i; if i == 8 then j = 13; end local relealFriendNumber = DataPool:GetFriendNumber( tonumber(j) ); local index=0; while index < relealFriendNumber  do local guid =  DataPool:GetFriend( tonumber(j), tonumber(index), 'NAME' ); if( DataPool:GetFriend( tonumber(j), tonumber(index), 'NAME' ) ) then online = online  .. guid .. ':' .. (j .. ',' .. index) .. '-'; end index = index + 1; end end return online;");
                string friends = game.LuaToStringMemo("GetFriend");
                Thread.Sleep(600);
                friends = game.LuaToStringMemo("GetFriend");
                LvFriend.Items.AddRange(friends.Split('-').ToList().Where(s => s.Split(':').Length == 2).Select(s => { ListViewItem item = new ListViewItem(s.Split(':')[0]); item.SubItems.Add(s.Split(':')[1]); return item; }).ToArray());
            });
        }

        private void tuyệtGiaoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;               

                foreach(ListViewItem i in LvFriend.SelectedItems.Cast<ListViewItem>().ToArray().Reverse())
                {
                    game.DoStringEx("DataPool:DelFriend(" + i.SubItems[1].Text + ");");
                    i.Remove();
                    Thread.Sleep(500);
                }               
                LoadFriend();
            }).Start();
        }

        private void LvFriend_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.A)
            {
                if (e.Control == true)
                {
                    LvFriend.SelectAllItems();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}
