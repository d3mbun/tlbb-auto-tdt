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
    partial class ChatLogs : UserControl
    {
        public ChatLogs()
        {
            InitializeComponent();
            instance = this;
        }


        private static ChatLogs instance;
        public static ListViewEx ListView;
        public static ChatLogs Instance
        {
            get
            {
                if (instance == null || instance.IsDisposed)
                {
                    instance = new ChatLogs();
                    ListView = instance.listViewChat;
                }
                return instance;
            }
        }

        private void ChatLogs_Load(object sender, EventArgs e)
        {
            ReadChat();
            //checkBox2.Checked = IsKhongTheGioi;
        }

        public void ReadChat()
        {
            listViewChat.dicHides.Clear();
            listViewChat.hideItems.Clear();
            listViewChat.Items.Clear();

            try
            {
                List<ListViewItem> list = new List<ListViewItem>();
                string content = Game.IniParser.Read("Item", "Chat");
                foreach (string msg in content.Split('\n'))
                {
                    try
                    {
                        ListViewItem item = new ListViewItem(msg.Split(new char[] { '#' }, 2)[0]);
                        try
                        {

                            item.SubItems.Add(msg.Split(new char[] { '#' }, 2)[1].Split(new char[] { ':' }, 2)[0].Split(new string[] { "->" }, StringSplitOptions.None)[0].Replace("[", "").Replace("]", ""));
                            item.SubItems.Add(msg.Split(new char[] { '#' }, 2)[1].Split(new char[] { ':' }, 2)[0].Split(new string[] { "->" }, StringSplitOptions.None)[1].Replace("[", "").Replace("]", ""));
                            item.Tag = msg;
                        }
                        catch { }
                        item.SubItems.Add(msg.Split(new char[] { '#' }, 2)[1].Split(new char[] { ':' }, 2)[1]);
                        //item.
                        list.Add(item);
                    }
                    catch { }
                }
                list.Reverse();
                listViewChat.Items.AddRange(list.ToArray());
                listViewChat.ScrollToEnd();

            }
            catch { }
        }

        private void listViewChat_DoubleClick(object sender, EventArgs e)
        {
            if (listViewChat.SelectedItems.Count == 0)
                return;
            ListViewItem item = listViewChat.SelectedItems[0];
            foreach(KeyValuePair<int,Game> kvp in Main.DicGame)
            {
                if (item.SubItems[1].Text.Contains(kvp.Value.TLBB.Name))
                    kvp.Value.Active();
            }
        }

    
        public static bool NotShow
        {
            get;
            set;
        }

        private void listViewChat_MouseMove(object sender, MouseEventArgs e)
        {
            //if (LastMousePos == e.Location)
            //    return;

            //ListViewItem item = listViewChat.GetItemAt(e.X, e.Y);
            //ListViewHitTestInfo info = listViewChat.HitTest(e.X, e.Y);

            //if ((item != null) && (info.SubItem != null))
            //{
            //    LastMousePos = e.Location;
            //    tooltipEx.Show(info.SubItem.Text, listViewChat);
            //}
            //else
            //{
            //    tooltipEx.Hide(listViewChat);
            //}
        }

        private Point LastMousePos = new Point(-1, -1);

        private void listViewChat_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                if (ListView.SelectedItems.Count > 0)
                {
                    string txt = ListView.SelectedItems[0].SubItems[1].Text;
                    txt = TDT.StringBetween(txt, "[", "]");
                    Main.DicGame.ToList().ForEach(kvp => kvp.Value.DoStringEx("setmetatable(_G, {__index = ChatFrame_Env}); ChatFrame_SetEditBoxTxt('/" + txt + " ');"));
                }
            }
            //if(ListView.SelectedItems.Count > 0)
            //{
            //    string txt = ListView.SelectedItems[0].SubItems[1].Text;
            //    txt = TDT.StringBetween(txt, "[", "]");
            //    Main.DicGame.ToList().ForEach(kvp => kvp.Value.LuaDoOneLineString("setmetatable(_G, {__index = ChatFrame_Env}); ChatFrame_SetEditBoxTxt('/" + txt + " ');"));
            //}
        }

        private void textBoxEx1_TextChanged(object sender, EventArgs e)
        {
            ListView.Search(textBoxEx1.Text);
            //lvW.Search(textBoxEx1.Text);
        }

        private void listViewChat_SelectedIndexChanged(object sender, EventArgs e)
        {
            var lv = sender as System.Windows.Forms.ListView;
            if (lv.SelectedItems.Count > 0)
            {
                txtContent.Text = lv.SelectedItems[0].SubItems[0].Text + "\t"  + lv.SelectedItems[0].SubItems[1].Text + "\t" + lv.SelectedItems[0].SubItems[2].Text + "\r\n\r\n" + lv.SelectedItems[0].SubItems[3].Text;
            }
        }


        public static bool IsKhongTheGioi { get; set; } = true;

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            while(listViewChat.SelectedItems.Count > 0)
            {
                listViewChat.SelectedItems[0].Remove();
            }

            Game.IniParser.Write("Item", "Chat", string.Join("\n", listViewChat.Items.Cast<ListViewItem>().Select(i => i.Tag.ToString()).ToArray()));
        }

        private void listViewChat_KeyDown(object sender, KeyEventArgs e)
        {
         
            if (e.KeyCode == Keys.A)
            {
                if (e.Control == true)
                {
                    listViewChat.SelectAllItems();
                }
            }
        }
    }
}
