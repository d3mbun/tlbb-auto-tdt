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
    partial class BinhThanh : UserControl
    {
        QuanDoan quandoan;
        public BinhThanh(QuanDoan quandoan)
        {
            this.quandoan = quandoan;
            InitializeComponent();
        }

        private void BinhThanh_Load(object sender, EventArgs e)
        {
            if (!Global.IsBinhThanh)
                chkTuDong.Enabled = false;

            int x = 51;
            int y = 54;

            List<string> points = new List<string>();

            points.Add((x) + "," + (y + 3));
            points.Add((x) + "," + (y + 6));
            points.Add((x) + "," + (y - 3));
            points.Add((x) + "," + (y - 6));
            points.Add((x - 3) + "," + (y + 3));
            points.Add((x - 3) + "," + (y + 6));
            points.Add((x - 3) + "," + (y - 3));
            points.Add((x - 3) + "," + (y - 6));
            points.Add((x + 3) + "," + (y + 3));
            points.Add((x + 3) + "," + (y + 6));
            points.Add((x + 3) + "," + (y - 3));
            points.Add((x + 3) + "," + (y - 6));
            int cnt = 0;
            foreach (Game game in quandoan.Party1)
            {                
                ListViewItem item = new ListViewItem(game.TLBB.Name);
                item.Tag = game;
                item.SubItems.Add(points[cnt]);
                listViewEx1.Items.Add(item);
                cnt++;
            }
            listViewEx1.Columns[0].Text = "Name [" + listViewEx1.Items.Count + "]";
            
            foreach (Game game in quandoan.Party2)
            {
                ListViewItem item = new ListViewItem(game.TLBB.Name);
                item.Tag = game;
                item.SubItems.Add(points[cnt]);
                listViewEx2.Items.Add(item);
                cnt++;
            }

            listViewEx2.Columns[0].Text = "Name [" + listViewEx2.Items.Count + "]";

            button7_Click(null, null);
        }

      

        private void button2_Click(object sender, EventArgs e)
        {
            step = 3;
            foreach (ListViewItem i in listViewEx1.Items)
            {
                var game = i.Tag as Game;
                i.SubItems[1].Text = "134,110";
            }
            foreach (ListViewItem i in listViewEx2.Items)
            {
                var game = i.Tag as Game;
                i.SubItems[1].Text = "142,64";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem i in listViewEx1.Items)
            {
                var game = i.Tag as Game;
                i.SubItems[1].Text = "173,100";
            }
            foreach (ListViewItem i in listViewEx2.Items)
            {
                var game = i.Tag as Game;
                i.SubItems[1].Text = "174,36";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            step = 4;
            listViewEx1.Items.Clear();
            listViewEx2.Items.Clear();
     

            List<string> points = new List<string>();
            points.Add("178,103");
            points.Add("172,94");
            points.Add("167,97");
            points.Add("167,103");
            points.Add("179,95");
            points.Add("173,105");
            points.Add("174,43");
            points.Add("180,37");
            points.Add("179,32");
            points.Add("168,34");
            points.Add("172,30");
            points.Add("170,41");
   
            int cnt = 0;
            foreach (Game game in quandoan.Party1)
            {
                ListViewItem item = new ListViewItem(game.TLBB.Name);
                item.Tag = game;
                item.SubItems.Add(points[cnt]);
                listViewEx1.Items.Add(item);
                cnt++;
            }

            foreach (Game game in quandoan.Party2)
            {
                ListViewItem item = new ListViewItem(game.TLBB.Name);
                item.Tag = game;
                item.SubItems.Add(points[cnt]);
                listViewEx2.Items.Add(item);
                cnt++;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            step = 5;
            foreach (ListViewItem i in listViewEx1.Items)
            {
                var game = i.Tag as Game;
                i.SubItems[1].Text = "203,134";
            }
            foreach (ListViewItem i in listViewEx2.Items)
            {
                var game = i.Tag as Game;
                i.SubItems[1].Text = "203,134";
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            step = 6;
            foreach (ListViewItem i in listViewEx1.Items)
            {
                var game = i.Tag as Game;
                i.SubItems[1].Text = "191,195";
            }
            foreach (ListViewItem i in listViewEx2.Items)
            {
                var game = i.Tag as Game;
                i.SubItems[1].Text = "191,195";
            }
        }
        int step = 0;
        private void button7_Click(object sender, EventArgs e)
        {
            step = 1;
            foreach (ListViewItem i in listViewEx1.Items)
            {
                var game = i.Tag as Game;
                i.SubItems[1].Text = "65,90";
            }
            foreach (ListViewItem i in listViewEx2.Items)
            {
                var game = i.Tag as Game;
                i.SubItems[1].Text = "65,90";
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            step = 2;
            listViewEx1.Items.Clear();
            listViewEx2.Items.Clear();
            List<string> points = new List<string>();
            int x = 51;
            int y = 54;
            points.Add((x) + "," + (y + 2));
            points.Add((x) + "," + (y + 6));
            points.Add((x) + "," + (y - 2));
            points.Add((x) + "," + (y - 6));
            points.Add((x - 3) + "," + (y + 2));
            points.Add((x - 3) + "," + (y + 6));
            points.Add((x - 3) + "," + (y - 2));
            points.Add((x - 3) + "," + (y - 6));
            points.Add((x + 3) + "," + (y + 2));
            points.Add((x + 3) + "," + (y + 6));
            points.Add((x + 3) + "," + (y - 2));
            points.Add((x + 3) + "," + (y - 6));
            int cnt = 0;
            foreach (Game game in quandoan.Party1)
            {
                ListViewItem item = new ListViewItem(game.TLBB.Name);
                item.Tag = game;
                item.SubItems.Add(points[cnt]);
                listViewEx1.Items.Add(item);
                cnt++;
            }

            foreach (Game game in quandoan.Party2)
            {
                ListViewItem item = new ListViewItem(game.TLBB.Name);
                item.Tag = game;
                item.SubItems.Add(points[cnt]);
                listViewEx2.Items.Add(item);
                cnt++;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            listViewEx1.Items.Clear();
            listViewEx2.Items.Clear();

            List<string> points = new List<string>();
            // 193 195
            points.Add("190,195");
            points.Add("194,195");
            points.Add("186,195");
            points.Add("198,195");
            points.Add("190,190");
            points.Add("194,190");
            points.Add("186,190");
            points.Add("198,190");
            points.Add("190,200");
            points.Add("194,200");
            points.Add("186,200");
            points.Add("198,200");

            int cnt = 0;
            foreach (Game game in quandoan.Party1)
            {
                ListViewItem item = new ListViewItem(game.TLBB.Name);
                item.Tag = game;
                item.SubItems.Add(points[cnt]);
                listViewEx1.Items.Add(item);
                cnt++;
            }

            foreach (Game game in quandoan.Party2)
            {
                ListViewItem item = new ListViewItem(game.TLBB.Name);
                item.Tag = game;
                item.SubItems.Add(points[cnt]);
                listViewEx2.Items.Add(item);
                cnt++;
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
          
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            step = 7;
            foreach (ListViewItem i in listViewEx1.Items)
            {
                var game = i.Tag as Game;
                i.SubItems[1].Text = "142,178";
            }
            foreach (ListViewItem i in listViewEx2.Items)
            {
                var game = i.Tag as Game;
                i.SubItems[1].Text = "142,178";
            }
        }

        int cnt = 0;

      

        private void button10_Click(object sender, EventArgs e)
        {
            step = 8;
            foreach (ListViewItem i in listViewEx1.Items)
            {
                var game = i.Tag as Game;
                i.SubItems[1].Text = "53,203";
            }
            foreach (ListViewItem i in listViewEx2.Items)
            {
                var game = i.Tag as Game;
                i.SubItems[1].Text = "53,203";
            }
        }

        private void listViewEx1_DoubleClick(object sender, EventArgs e)
        {
            if (listViewEx1.SelectedItems.Count == 0)
                return;
            Game game = (Game)listViewEx1.SelectedItems[0].Tag;
            game.Active();
        }

        private void listViewEx2_DoubleClick(object sender, EventArgs e)
        {
            if (listViewEx2.SelectedItems.Count == 0)
                return;
            Game game = (Game)listViewEx2.SelectedItems[0].Tag;
            game.Active();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem i in listViewEx1.Items)
            {
                var game = i.Tag as Game;
                game.DownRide();
            }
            foreach (ListViewItem i in listViewEx2.Items)
            {
                var game = i.Tag as Game;
                game.DownRide();
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem i in listViewEx1.Items)
            {
                var game = i.Tag as Game;
                game.UpRide();
            }
            foreach (ListViewItem i in listViewEx2.Items)
            {
                var game = i.Tag as Game;
                game.UpRide();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //tmrMove.Enabled = checkBox1.Checked;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {


            foreach (ListViewItem i in listViewEx1.Items)
            {
                var game = i.Tag as Game;
                if (cnt == 0)
                {
                    game.ListMoveEx.Clear();
                }
                if (i.SubItems[1].Text == "Cao Dương")
                {
                    game.Move(LAULAN.CaoDuong.X, LAULAN.CaoDuong.Y, LAULAN.CaoDuong.Map);
                }
                else if (i.SubItems[1].Text == "142,178")
                {
                    game.Move(i.SubItems[1].Text);
                }
                else
                {
                    game.Move(i.SubItems[1].Text);
                }
            }
            foreach (ListViewItem i in listViewEx2.Items)
            {
                var game = i.Tag as Game;
                if (cnt == 0)
                {
                    game.ListMoveEx.Clear();
                }
                if (i.SubItems[1].Text == "Cao Dương")
                {
                    game.Move(LAULAN.CaoDuong.X, LAULAN.CaoDuong.Y, LAULAN.CaoDuong.Map);
                }
                else if (i.SubItems[1].Text == "142,178")
                {
                    game.Move(i.SubItems[1].Text);
                }
                else
                {
                    game.Move(i.SubItems[1].Text);
                }
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {

            foreach (ListViewItem i in listViewEx1.Items)
            {
                var game = i.Tag as Game;
                if (cnt == 0)
                {
                    game.ListMoveEx.Clear();
                }
                if (i.SubItems[1].Text == "Cao Dương")
                {
                    game.GoToEx(LAULAN.CaoDuong.X, LAULAN.CaoDuong.Y, LAULAN.CaoDuong.Map);
                }
                else if (i.SubItems[1].Text == "142,178")
                {
                    game.GoToEx(i.SubItems[1].Text);
                }
                else
                {
                    game.GoToEx(i.SubItems[1].Text);
                }
            }
            foreach (ListViewItem i in listViewEx2.Items)
            {
                var game = i.Tag as Game;
                if (cnt == 0)
                {
                    game.ListMoveEx.Clear();
                }
                if (i.SubItems[1].Text == "Cao Dương")
                {
                    game.GoToEx(LAULAN.CaoDuong.X, LAULAN.CaoDuong.Y, LAULAN.CaoDuong.Map);
                }
                else if (i.SubItems[1].Text == "142,178")
                {
                    game.GoToEx(i.SubItems[1].Text);
                }
                else
                {
                    game.GoToEx(i.SubItems[1].Text);
                }
            }

        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
        }
        CheckBox chkTuDong { get; set; } = new CheckBox();
        private void timer1_Tick(object sender, EventArgs e)
        {
            bool isHaveMonter = false;
            bool is109 = true;
            foreach (ListViewItem i in listViewEx1.Items)
            {
                var game = i.Tag as Game;
                if (game.GetDistance(109, 202) >= 8)
                    is109 = false;
            }
            foreach (ListViewItem i in listViewEx2.Items)
            {
                var game = i.Tag as Game;
                if (game.GetDistance(109, 202) >= 8)
                    is109 = false;
            }
            if (is109 &&  (chkTuDong.Checked))
            {
                step = 8;
                if (step == 8)
                {
                    foreach (ListViewItem i in listViewEx1.Items)
                    {
                        var game = i.Tag as Game;
                        i.SubItems[1].Text = "53,203";
                    }
                    foreach (ListViewItem i in listViewEx2.Items)
                    {
                        var game = i.Tag as Game;
                        i.SubItems[1].Text = "53,203";
                    }                    
                    checkBox1.Checked = true;
                }
            }
         

           
            foreach (ListViewItem i in listViewEx1.Items)
            {
                var game = i.Tag as Game;
                if (game.Objects.NearMonter(20).Count > 0)
                    isHaveMonter = true;
            }
            foreach (ListViewItem i in listViewEx2.Items)
            {
                var game = i.Tag as Game;
                if (game.Objects.NearMonter(20).Count > 0)
                    isHaveMonter = true;
            }
            if (checkBox1.Checked)
            {
                foreach (ListViewItem i in listViewEx1.Items)
                {
                    var game = i.Tag as Game;
                    if (i.SubItems[1].Text == "Cao Dương")
                    {

                        if (!game.GoToEx(LAULAN.CaoDuong.X, LAULAN.CaoDuong.Y, LAULAN.CaoDuong.Map))
                        {
                         
                        }
                        else
                        {
                            if (isHaveMonter)
                                game.DownRide();
                        }
                    }
                    else
                    {
                        if (!game.GoToEx(i.SubItems[1].Text))
                        {
                        }
                    }
                }
                foreach (ListViewItem i in listViewEx2.Items)
                {
                    var game = i.Tag as Game;
                    if (i.SubItems[1].Text == "Cao Dương")
                    {
                        if (!game.GoToEx(LAULAN.CaoDuong.X, LAULAN.CaoDuong.Y, LAULAN.CaoDuong.Map))
                        {
                         
                        }
                    }
                    else
                    {
                        if (!game.GoToEx(i.SubItems[1].Text))
                        {
                         
                        }
                        else
                        {
                            if (isHaveMonter)
                                game.DownRide();
                        }
                    }
                }
            }
            else
            {
                if (chkTuDong.Checked)
                {
                    foreach (ListViewItem i in listViewEx1.Items)
                    {
                        var game = i.Tag as Game;
                        if (isHaveMonter)
                            game.DownRide();
                    }
                    foreach (ListViewItem i in listViewEx2.Items)
                    {
                        var game = i.Tag as Game;
                        if (isHaveMonter)
                            game.DownRide();
                    }
                }
            }
           
        
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (!Global.IsBinhThanh)
            {
                
                MessageBox.Show(this, "Chức năng này phải mua", "MicroAuto", MessageBoxButtons.YesNo);

                chkTuDong.Checked = false;
            }
        }

        private void listViewEx2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click_2(object sender, EventArgs e)
        {
            foreach (ListViewItem i in listViewEx1.Items)
            {
                i.SubItems[1].Text = "Cao Dương";
            }
            foreach (ListViewItem i in listViewEx2.Items)
            {
                i.SubItems[1].Text = "Cao Dương";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}
