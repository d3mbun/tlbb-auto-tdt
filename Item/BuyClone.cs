using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace _i
{
    public partial class BuyClone : UserControl
    {
        public BuyClone()
        {
            InitializeComponent();
        }

        private void BuyClone_Load(object sender, EventArgs e)
        {
            RefreshL();
        }

        void RefreshL()
        {
            Enabled = false;
            listViewEx1.Items.Clear();

            Task.Run(() =>
            {
               
                string Clone = Poster.CurlGet("http://tieudattai.org/getclone.php?email=" + HttpUtility.UrlEncode(User.Email) + "&pass=" + User.Pass).Trim();
                List<ListViewItem> list = new List<ListViewItem>();
                int cnt = 1;
                foreach (string s in Clone.Split('#'))
                {
                    if (s.Length >= 6)
                    {
                        ListViewItem item = new ListViewItem((cnt++).ToString());
                        item.SubItems.Add(s);
                        list.Add(item);
                    }
                }
                Invoke(new Action(() =>
                {
                    listViewEx1.Items.AddRange(list.ToArray());
                }));
                Enabled = true;

            });
        }

        private void listViewEx1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Control == true && e.KeyCode == Keys.A)
            {
                foreach (ListViewItem item in listViewEx1.Items)
                {
                    item.Selected = true;
                }
            }
        }

        private void copyCloneToolStripMenuItem_Click(object sender, EventArgs e)
        {
           MicroLogin.Clone = "";
            foreach (ListViewItem item in listViewEx1.SelectedItems)
            {
                MicroLogin.Clone += item.SubItems[1].Text + "#";
            }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshL();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                string status = Poster.CurlGet("http://tieudattai.org/buyclone.php?sl=" + textBoxEx1.Text.ToInt() + "&email=" + HttpUtility.UrlEncode(User.Email) + "&pass=" + User.Pass);

                MessageBox.Show(this, status, "MicroAuto", MessageBoxButtons.OK);
                Invoke(new Action(() =>
                {
                    RefreshL();
                }));
            });
        }
    }
}
