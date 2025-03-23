using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace _i
{


    partial class Calendar : UserControl
    {
       
        public string Path { get; set; }

        public static bool isFirstTime = false;

        public Calendar()
        {
            InitializeComponent();
            string note = "";
            Text = "Đặt Lịch Phụ Bản - All";
            note = Game.IniParser.Read("Calender", "All");
            //Path = Global.CalenderPath + "\\" + Main.CurGame.TLBB.Id + ".txt";
            foreach (string n in note.Split('\n'))
            {
                if(n.Split('|').Length > 1)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = n.Split('|')[0].Trim();
                    item.SubItems.Add(n.Split('|')[1].Trim());
                    listViewCalendar.Items.Add(item);
                }
            }
            cboMap.SelectedIndex = 0;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {           
            if (cboNote.Text == "")
                return;
            string note = cboNote.Text;
            if (TDT.VietLien(cboNote.Text).Contains("actac"))
            {
                if (cboMap.Text.Contains("Ác Tặc"))
                {
                    note += " [" + Regex.Replace(cboMap.Text, ".* - ", "") + "]";
                }
            }
            if (TDT.VietLien(cboNote.Text).Contains("tangkinhcac"))
            {
                if (cboMap.Text.Contains("Tàng Kinh Các"))
                {
                    note += " [" + Regex.Replace(cboMap.Text, ".* - ", "") + "]";
                }
            }
            ListViewItem item = new ListViewItem();
            item.Text = ToStringEx(nudHour.Value) + ":" + ToStringEx(nudMinute.Value) + " -> " + ToStringEx(nudHourEnd.Value) + ":" + ToStringEx(nudMinuteEnd.Value);
            item.SubItems.Add(note);
            listViewCalendar.Items.Add(item);
            SaveTask();
        }

        public void SaveTask()
        {
            var item = cboPlayer.SelectedItem as ComboboxValue;
            var id = item.Id;
            string note = "";
            foreach (ListViewItem it in listViewCalendar.Items)
            {
                note += it.Text + " | " + it.SubItems[1].Text + "\r\n";
            }

            if (Text == "Đặt Lịch Phụ Bản - All")
            {
                Game.IniParser.Write("Calender", "All", note);
            }
            else
            {
                Game.IniParser.Write("Calender", TDT.GetAllowName(Text.Replace("Đặt Lịch Phụ Bản - ", "")), note);
            }
        }

        public string ToStringEx(decimal value)
        {
            if (value < 10)
                return "0" + value;
            return value.ToString();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            while (listViewCalendar.SelectedItems.Count > 0)
                listViewCalendar.SelectedItems[0].Remove();
            SaveTask();
        }

        private void Calendar_Load(object sender, EventArgs e)
        {
            cboPlayer.Items.Clear();
            cboPlayer.Items.Add(new ComboboxValue("0000FFFF", "Thiết Lập - Toàn Bộ"));
            Main.Instance.AllOnelineGame.ToList().ForEach(game => cboPlayer.Items.Add(new ComboboxValue(game.TLBB.Id, "Thiết Lập - " + game.TLBB.Name)));
            cboPlayer.SelectedIndex = 0;
        }

        public static string Note = string.Empty;

        private void menuCopy_Click(object sender, EventArgs e)
        {
            string note = "";
            foreach (ListViewItem item in listViewCalendar.Items)
            {
                note += item.Text + " | " + item.SubItems[1].Text + "\r\n";
            }
            Note = note;
            try
            {
                Clipboard.SetText(note);
            }
            catch { }
        }

        private void menuDelete_Opening(object sender, CancelEventArgs e)
        {

        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string note = Note;
            try
            {
                if (note == string.Empty)
                    note = Clipboard.GetText();
            }
            catch { }
            foreach (string n in note.Split('\n'))
            {
                if (n.Split('|').Length > 1)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = n.Split('|')[0].Trim();
                    item.SubItems.Add(n.Split('|')[1].Trim());
                    listViewCalendar.Items.Add(item);
                }
            }
            SaveTask();
        }

        private void cboMap_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        string AutoCal = "01:59 -> 03:00 | Ác Tặc [Key]\r\n04:01 -> 05:00 | Ác Bá[Key]\r\n10:01 -> 10:40 | Ác Bá[Key]\r\n10:40 -> 11:15 | Tàng Kinh Các[Key]\r\n11:29 -> 11:50 | Lâu Lan Tầm Bảo[Key]\r\n12:01 -> 13:00 | Ác Bá[Key]\r\n13:00 -> 14:00 | Ác Tặc[Key]\r\n14:00 -> 14:20 | Kỳ Cuộc[Key]\r\n14:59 -> 16:00 | Ác Tặc[Key]\r\n16:01 -> 16:25 | Ác Bá[Key]\r\n16:25 -> 17:00 | Tàng Kinh Các[Key]\r\n17:00 -> 18:00 | Ác Tặc[Key]\r\n18:59 -> 20:00 | Ác Tặc[Key]\r\n20:01 -> 21:00 | Ác Bá[Key]\r\n21:00 -> 21:25 | Ác Tặc[Key]\r\n21:25 -> 22:00 | Tàng Kinh Các[Key]\r\n22:01 -> 23:00 | Ác Bá[Key]\r\n22:55 -> 23:30 | Tàng Kinh Các[Key]\r\n23:30 -> 23:45 | Dã Trư[Key]\r\n00:01 -> 01:00 | Ác Bá[Key]";

        private void btnAutoAd_Click(object sender, EventArgs e)
        {
            string note = AutoCal;
            foreach (string n in note.Split('\n'))
            {
                if (n.Split('|').Length > 1)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = n.Split('|')[0].Trim();
                    item.SubItems.Add(n.Split('|')[1].Trim());
                    listViewCalendar.Items.Add(item);
                }
            }
            SaveTask();
        }

        private void cboPlayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            listViewCalendar.Items.Clear();
            var item = cboPlayer.SelectedItem as ComboboxValue;
            var id = item.Id;


            string note = "";

            if (id == "0000FFFF")
            {
                Text = "Đặt Lịch Phụ Bản - All";
                note = Game.IniParser.Read("Calender", "All");
            }
            else
            {
                foreach (KeyValuePair<int, Game> kvp in Main.DicGame)
                {
                    if (kvp.Value.TLBB.Id == id)
                    {
                        var game = kvp.Value;
                        Text = "Đặt Lịch Phụ Bản - " + game.TLBB.Name;
                        note = Game.IniParser.Read("Calender", game.TLBB.AllowName);
                    }
                }
            }

            foreach (string n in note.Split('\n'))
            {
                if (n.Split('|').Length > 1)
                {
                    ListViewItem it = new ListViewItem
                    {
                        Text = n.Split('|')[0].Trim()
                    };
                    it.SubItems.Add(n.Split('|')[1].Trim());
                    listViewCalendar.Items.Add(it);
                }
            }
            cboMap.SelectedIndex = 0;
        }

        private void listViewCalendar_DragDrop(object sender, DragEventArgs e)
        {
            SaveTask();
        }
    }
}
