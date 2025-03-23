using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _i
{
    partial class DebugEx : Form
    {
        public Game Game { get; set; }

        public DebugEx()
        {
            InitializeComponent();
        }

        private void allToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lvObject.Items.Clear();
            foreach (GameObject i in Game.Objects.All)
            {
                ListViewItem item = new ListViewItem(i.Id.ToString());
                item.SubItems.Add(i.Name);
                item.SubItems.Add(i.Distance.ToString());
                item.Tag = i;
                lvObject.Items.Add(item);
            }
        }

        private void btnDoString_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                Game.DoStringEx(txtDoString.Text);
                txtToString.Text = Game.LuaToStringMemo("DebugEx");
                Thread.Sleep(500);
                txtToString.Text = Game.LuaToStringMemo("DebugEx");
            });
      
        }

        private void txtPushDebugMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                Game.DoStringEx("PushDebugMessage('" + txtPushDebugMessage.Text + "')");
        }

        private void questFrameItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lvObject.Items.Clear();
            foreach(QuestFrameItem i in Game.QuestFrame.Items)
            {
                ListViewItem item = new ListViewItem(i.Id.ToString());
                item.SubItems.Add(i.Name);
                item.SubItems.Add(i.StrOptionExtra1.ToString());
                item.Tag = i;
                lvObject.Items.Add(item);
            }
        }

        private void lvObject_SelectedIndexChanged(object sender, EventArgs e)
        {
            pgGameObject.SelectedObject = lvObject.SelectedItems[0].Tag;
        }

        private void DebugEx_Load(object sender, EventArgs e)
        {
            Icon = Properties.Resources.icon;
            if (Game != null)
            {
                pgTLBB.SelectedObject = Game.TLBB;
                pgGame.SelectedObject = Game;
                Text = Game.TLBB.Name;
            }
        }

        private void txtQuestFrameClick_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                Game.QuestFrame.Click(txtQuestFrameClick.Text);
            }
        }

        private void gameTaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lvObject.Items.Clear();
            foreach (GameTask i in GameTask.Enum(Game))
            {
                ListViewItem item = new ListViewItem(i.Id.ToString());
                item.SubItems.Add(i.Name);
                item.SubItems.Add(i.Complete.ToString());
                item.Tag = i;
                lvObject.Items.Add(item);
            }
        }

        private void btnPush_Click(object sender, EventArgs e)
        {
            Game.DoStringEx("PushDebugMessage('" + txtPushDebugMessage.Text + "')");
        }

        private void useSkill3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(lvObject.SelectedItems.Count > 0)
            {
                if(lvObject.Tag is GameObject)
                {
                    Game.UseSkill(3, (lvObject.Tag as GameObject).Id);
                }
            }
        }

        private void bankToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lvObject.Items.Clear();
            foreach (var i in Game.PacketItems.Bank)
            {
                ListViewItem item = new ListViewItem(i.PacketId.ToString());
                item.SubItems.Add(i.Name);
                item.SubItems.Add(i.TypeName);
                item.Tag = i;
                lvObject.Items.Add(item);
            }
        }

        private void packetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lvObject.Items.Clear();
            foreach (var i in Game.PacketItems.All)
            {
                ListViewItem item = new ListViewItem(i.PacketId.ToString());
                item.SubItems.Add(i.Name);
                item.SubItems.Add(i.TypeName);
                item.Tag = i;
                lvObject.Items.Add(item);
            }
        }

        private void trangBiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lvObject.Items.Clear();
            foreach (var i in Game.PacketItems.TrangBi)
            {
                ListViewItem item = new ListViewItem(i.PacketId.ToString());
                item.SubItems.Add(i.Name);
                item.SubItems.Add(i.TypeName);
                item.Tag = i;
                lvObject.Items.Add(item);
            }
        }

        private void shopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lvObject.Items.Clear();
            foreach (var i in Game.PacketItems.Shop)
            {
                ListViewItem item = new ListViewItem(i.PacketId.ToString());
                item.SubItems.Add(i.Name);
                item.SubItems.Add(i.TypeName);
                item.Tag = i;
                lvObject.Items.Add(item);
            }
        }

        private void rideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lvObject.Items.Clear();
            foreach (var i in Game.PacketItems.Ride)
            {
                ListViewItem item = new ListViewItem(i.Address.ToString());
                item.SubItems.Add(i.Name);
                item.SubItems.Add(i.TypeName);
                item.Tag = i;
                lvObject.Items.Add(item);
            }
        }

        private void doActionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var i = lvObject.SelectedItems[0].Tag as PacketItem;
            i.DoAction();
        }

        private void doSubActionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var i = lvObject.SelectedItems[0].Tag as PacketItem;
            i.DoSubAction();
        }

        private void useToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var i = lvObject.SelectedItems[0].Tag as PacketItem;
            i.Use();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var j = (JObject)JsonConvert.DeserializeObject("{ \"a\": 2 }");

            MessageBox.Show(j["a"].EmptyIfNull());

            try
            {
                MediaPlayer.Play(File.ReadAllBytes("alarm.mp3"));
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }

            return;

            Dictionary<string, string> dic = new Dictionary<string, string>();

            Stopwatch sw = Stopwatch.StartNew();

            string note = "";

            note += Controls.Find("button1", true).FirstOrDefault().EmptyIfNull();
            note += Controls.Find("butt2on1", true).FirstOrDefault().EmptyIfNull();
            note += Controls.Find("butto4n1", true).FirstOrDefault().EmptyIfNull();
            note += Controls.Find("butt2on1", true).FirstOrDefault().EmptyIfNull();
            note += Controls.Find("button1", true).FirstOrDefault().EmptyIfNull();
            note += Controls.Find("butto4n1", true).FirstOrDefault().EmptyIfNull();

            Text = note + "-" + sw.Elapsed.TotalMilliseconds.ToString() + "=>" + sw.Elapsed.TotalSeconds.ToString() + ";;;" +  dic["abc"].EmptyIfNull();

            //SharpConfig.Configuration config = SharpConfig.Configuration.LoadFromFile("nier.ini");

            //config["vkl"]. = "dcm";

            //config.SaveToFile("enir.ini");




            //Game.SendPacket(Game.HexToString(Game.AddressGameExe + 0x62FE50) + " 00 00 00 00 00 00 00 00 FF FF FF FF 24 06");
            //try
            //{
            //    Dictionary<string, string> dic = new Dictionary<string, string>();
            //    dic["dcm"] = "vl;";
            //    dic.Remove("dcm");
            //    MessageBox.Show("success" + dic["dcm"]);
            //}
            //catch(Exception ex)
            //{
            //    MessageBox.Show(this, ex.Message);
            //}
        }

        private void lootToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lvObject.Items.Clear();
            foreach (var i in Game.PacketItems.LootPacket)
            {
                ListViewItem item = new ListViewItem(i.Address.ToString());
                item.SubItems.Add(i.Name);
                item.SubItems.Add(i.TypeName);
                item.Tag = i;
                lvObject.Items.Add(item);
            }
        }

        private void thienCoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lvObject.Items.Clear();
            foreach (var i in Game.PacketItems.ThienCo)
            {
                ListViewItem item = new ListViewItem(i.PacketId.ToString());
                item.SubItems.Add(i.Name);
                item.SubItems.Add(i.TypeName);
                item.Tag = i;
                lvObject.Items.Add(item);
            }
        }

        private void teamToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lvObject.Items.Clear();
            Game.Team.Read();
            Game.Team.Count = 6;
            foreach (var i in Game.Team.All)
            {
                ListViewItem item = new ListViewItem(i.Name);
                item.SubItems.Add(i.Name);
                item.Tag = i;
                lvObject.Items.Add(item);
            }
        }

        private void instanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvObject.SelectedItems.Count > 0)
            {
                if (lvObject.SelectedItems[0].Tag is GameObject)
                {
                    var _object = lvObject.SelectedItems[0].Tag as GameObject;
                    string npc = "public static NPC " + TDT.ClearSign(_object.Name).Replace(" ", "") + "  = new NPC()\r\n{\r\n\tId = " + _object.Id + ",\r\n\tX = " + _object.RoundX + ",\r\n\tY = " + _object.RoundY + ",\r\n\tMap = Id,\r\n\t" + "INFOAIM = \"#G" + Game.TLBB.MapName + "#R" + _object.Name + "#{_INFOAIM" + _object.RoundX + "," + _object.RoundY + "," + Game.TLBB.MapId + "," + _object.Name + "}\"\r\n" + "};";
                    MessageBox.Show(this, npc, "MicroAuto", MessageBoxButtons.OK);
                    Clipboard.SetText(npc);
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            btnClearSniff.Text = "Clear (" + Game.ListRecv.Count + ")";
        }

        private void btnShowSniff_Click(object sender, EventArgs e)
        {
            while (Game.ListRecv.Count > 0)
            {
                ByteViewer bv = new ByteViewer();
                bv.SetBytes(Game.ListRecv[0]);
                bv.Dock = DockStyle.Top;
                bv.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                bv.Height = 100;
                bv.MouseClick += (ss, ee) =>
                {
                    ByteViewer bv = ss as ByteViewer;
                    txtHex.Text = ConverterEx.LogPacket(bv.GetBytes());
                };
                panSniff.Controls.Add(bv);
                Game.ListRecv.RemoveAt(0);
            }
        }

        private void btnClearSniff_Click(object sender, EventArgs e)
        {
            Game.ListRecv.Clear();
            panSniff.Controls.Clear();
        }

        private void btnSendPacket_Click(object sender, EventArgs e)
        {
            Game.SendPacket(txtPacket.Text);
        }

        private void tuiChanNguyenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lvObject.Items.Clear();
            foreach (var i in Game.PacketItems.TuiChanNguyen)
            {
                ListViewItem item = new ListViewItem(i.Address.ToString());
                item.SubItems.Add(i.Name);
                item.SubItems.Add(i.TypeName);
                item.Tag = i;
                lvObject.Items.Add(item);
            }
        }

        private void uskill3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvObject.SelectedItems.Count > 0)
            {
                if (lvObject.SelectedItems[0].Tag is GameObject)
                {
                    var _object = lvObject.SelectedItems[0].Tag as GameObject;
                    Game.UseSkill(3, _object.Id);
                }
            }
        }

        private void pickToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvObject.SelectedItems.Count > 0)
            {
                if (lvObject.SelectedItems[0].Tag is GameObject)
                {
                    var _object = lvObject.SelectedItems[0].Tag as GameObject;
                    Game.PickItem((int)_object.Id);
                }
            }
        }

        private void trapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lvObject.Items.Clear();
            foreach (GameObject i in Game.Objects.All.Where(o => o.IsTrap))
            {
                ListViewItem item = new ListViewItem(i.Id.ToString());
                item.SubItems.Add(i.Name);
                item.SubItems.Add(i.Distance.ToString());
                item.Tag = i;
                lvObject.Items.Add(item);
            }
        }

        private void gameControlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lvObject.Items.Clear();
            foreach (var i in GameControl.Enum(Game))
            {
                ListViewItem item = new ListViewItem(i.Id.ToString());
                item.SubItems.Add(i.Name);
                item.SubItems.Add(i.PacketId.ToString());
                item.Tag = i;
                lvObject.Items.Add(item);
            }
        }

        private void petToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lvObject.Items.Clear();
            foreach (var i in Game.TLBB.Pets)
            {
                ListViewItem item = new ListViewItem(i.idx.ToString());
                item.SubItems.Add(i.PetName);
                item.Tag = i;
                lvObject.Items.Add(item);
            }
        }
    }
}
