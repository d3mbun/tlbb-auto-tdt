using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _i.Item
{
    public partial class SleepTime : UserControl
    {
        public SleepTime()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ListViewItem item = new ListViewItem();
            item.Text = txtHFrom.Text.ToInt() + ":" + txtMFrom.Text.ToInt() + " -> " + txtHEnd.Text.ToInt() + ":" + txtMEnd.Text.ToInt();            
            lv.Items.Add(item);
            SaveTask();
        }

        public void SaveTask()
        {
            string note = "";
            foreach (ListViewItem it in lv.Items)
            {
                note += it.Text + "\r\n";
            }

            Game.IniParser.Write("Config", "SleepTime", note);
        }

        private void SleepTime_Load(object sender, EventArgs e)
        {
            string note = Game.IniParser.Read("Config", "SleepTime");    
            foreach (string n in note.Split('\n'))
            {
                if (n.Trim().Length > 3)
                {
                    ListViewItem item = new ListViewItem(n.Trim());
                    lv.Items.Add(item);
                }
            }
        }

        private void SleepTime_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveTask();
        }

        private void lv_DoubleClick(object sender, EventArgs e)
        {
            lv.SelectedItems.Cast<ListViewItem>().ToList().ForEach(i => i.Remove());
            SaveTask();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}
