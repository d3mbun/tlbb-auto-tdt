using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace _i
{
    partial class ScriptManager : Form
    {
        Script script;
        Game game;

        public ScriptManager(Script script, Game game)
        {
            this.game = game;
            this.script = script;
            InitializeComponent();
        }

        private void ScriptManager_Load(object sender, EventArgs e)
        {
            txtID.Text = script.ID;
            txtMD.Text = script.MD;
            txtInfo.Text = script.Info;
            if(script.Recv.Length == 12)
            {
                recvID.Value = Memory.Hex2Int(script.Recv.Substring(0, 3));
                try
                {
                    recvX.Value = Memory.Hex2Int(script.Recv.Substring(3, 3));
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                recvY.Value = Memory.Hex2Int(script.Recv.Substring(6, 3));
                recvMAP.Value = Memory.Hex2Int(script.Recv.Substring(9, 3));
            }
            if (script.Send.Length == 12)
            {
                sendID.Value = Memory.Hex2Int(script.Send.Substring(0, 3));
                sendX.Value = Memory.Hex2Int(script.Send.Substring(3, 3));
                sendY.Value = Memory.Hex2Int(script.Send.Substring(6, 3));
                sendMAP.Value = Memory.Hex2Int(script.Send.Substring(9, 3));
            }
            txdDo.Text = script.Do;
            atk1MD.Text = script.AtkNPC1.MD;
            atk1X.Value = script.AtkNPC1.X;
            atk1Y.Value = script.AtkNPC1.Y;
            atk1Map.Value = script.AtkNPC1.Map;
            atk2MD.Text = script.AtkNPC2.MD;
            atk2X.Value = script.AtkNPC2.X;
            atk2Y.Value = script.AtkNPC2.Y;
            atk2Map.Value = script.AtkNPC2.Map;
            atk3MD.Text = script.AtkNPC3.MD;
            atk3X.Value = script.AtkNPC3.X;
            atk3Y.Value = script.AtkNPC3.Y;
            atk3Map.Value = script.AtkNPC3.Map;
            chkPick.Checked = script.IsPick;
            chkIsThuThap.Checked = script.IsThuThap;
            chkCompleted.Checked = script.Completed;
            chkUseItem.Checked = script.IsUseItem;
            txtName.Text = script.Name;
            nudColloect.Value = TDT.ParseInt(script.IsCollect);
            chkComplete.Checked = script.IsComplete;
            txtMonter.Text = script.Monter;
            chkBienThan.Checked = script.IsBienThan;
        }

        public event System.EventHandler Edited;

        public void btnEdit_Click(object sender, EventArgs e)
        {
            script.ID = txtID.Text;
            script.MD = txtMD.Text;
            script.Recv = Int2Hex(recvID.Value) + Int2Hex(recvX.Value) + Int2Hex(recvY.Value) + Int2Hex(recvMAP.Value);
            script.Send = Int2Hex(sendID.Value) + Int2Hex(sendX.Value) + Int2Hex(sendY.Value) + Int2Hex(sendMAP.Value);
            script.Do = txdDo.Text;
            script.Name = txtName.Text;
            script.Info = txtInfo.Text;
            script.Monter = txtMonter.Text;
            Scripts.Save();
            if (Edited != null)
                Edited(this, null);            
        }

        public static string Int2Hex(decimal val)
        {
            int va = (int)val;
            string output = va.ToString("X8");
            return output.Substring(5, 3);
        }

        private void recvClick1_ValueChanged(object sender, EventArgs e)
        {
            string send = "000";
            if(sendClick1.Value + sendClick2.Value != 0)
            {
                send = TDT.Hasher.MD5(sendClick1.ToString() + sendClick2.ToString()).Substring(0, 3);
            }
            string recv = "000";
            if(recvClick1.Value + recvClick2.Value != 0)
            {
                recv = TDT.Hasher.MD5(recvClick1.ToString() + recvClick2.ToString()).Substring(0, 3);
            }
            if (send == "000")
                send = recv;
            if (recv == "000")
                recv = send;
            script.Do = recv + send;
        }

        private void atk1MD_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ChangeDo();
        }

        private void ChangeDo()
        {
            string ex = "";
            if (script.Do.Length >= 58)
            {
                ex = script.Do.Substring(42, 16);
            }
            else
            {
                ex = "0000000000000000";
            }
            if (script.Do.Length > 6)
                script.Do = script.Do.Substring(0, 6);
            else
            {
                script.Do = "000000";
            }

            txdDo.Text = script.Do + atk1MD.Text + Int2Hex(atk1X.Value) + Int2Hex(atk1Y.Value) + Int2Hex(atk1Map.Value);
            txdDo.Text = txdDo.Text + atk2MD.Text + Int2Hex(atk2X.Value) + Int2Hex(atk2Y.Value) + Int2Hex(atk2Map.Value);
            txdDo.Text = txdDo.Text + atk3MD.Text + Int2Hex(atk3X.Value) + Int2Hex(atk3Y.Value) + Int2Hex(atk3Map.Value);
            if (ex != "")
            {
                txdDo.Text = txdDo.Text + ex;
            }
            else
            {
                txdDo.Text = txdDo.Text + "1000000000000000";
            }
            script.Do = txdDo.Text;
        }

        //private void listViewInfo_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if(listViewInfo.SelectedItems.Count > 0)
        //    {
        //        if(listViewInfo.SelectedItems[0].SubItems[1].Text != "")
        //        {
        //            Clipboard.SetText(listViewInfo.SelectedItems[0].SubItems[1].Text);
        //        }
        //    }
        //}

        //private void button2_Click(object sender, EventArgs e)
        //{
        //    game.LuaDoString("MISSIONINFO = ''; cnt = 0; while true do local name, content = DataPool:GetPlayerMission_Memo(cnt); if string.len(name) > 0 then MISSIONINFO = MISSIONINFO .. name .. '=' .. content .. ';'; end cnt = cnt + 1; if cnt == 20 then return end end return MISSIONINFO;");
        //    game.LuaToString();
        //    string info = game.LuaStringEx();
        //    MessageBox.Show(info);
        //    if (MessageBox.Show(this, "Bạn có muốn thêm info", "MicroAuto", MessageBoxButtons.YesNo) == DialogResult.Yes)
        //    {
        //        info = info.Replace("=", ":");
        //        info = info.Replace("   ", "");
        //        info = info.Replace("  ", "");
        //        info.Trim(';');
        //        foreach (string s in info.Split(';'))
        //        {
        //            ListViewItem item = new ListViewItem(s.Split('=')[0]);
        //            item.SubItems.Add(s.Split('=')[1]);
        //            listViewInfo.Items.Add(item);
        //        }
        //    }
        //}

        private void button3_Click(object sender, EventArgs e)
        {
            button1_Click(null, null);
            script.Completed = !script.Completed;
            txdDo.Text = script.Do;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button1_Click(null, null);
            script.IsUseItem = !script.IsUseItem;
            txdDo.Text = script.Do;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            button1_Click(null, null);
            script.CompleteNoi = (int)numericUpDown1.Value;
            txdDo.Text = script.Do;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            button1_Click(null, null);
            script.CompleteNgoai = (int)numericUpDown2.Value;
            txdDo.Text = script.Do;
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            atk1X.Value = (int)game.CharX;
            atk1Y.Value = (int)game.CharY;
            atk1Map.Value = game.TLBB.MapId;
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            button1_Click(null, null);
            script.IsCollect = nudColloect.Value.ToString();
            txdDo.Text = script.Do;
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            button1_Click(null, null);
            script.IsComplete = !script.IsComplete;
            txdDo.Text = script.Do;
        }

        private void chkPick_CheckedChanged(object sender, EventArgs e)
        {
            button1_Click(null, null);
            script.IsPick = chkPick.Checked;
            txdDo.Text = script.Do;
        }

        private void chkCompleted_CheckedChanged(object sender, EventArgs e)
        {
            button1_Click(null, null);
            script.Completed = chkCompleted.Checked;
            txdDo.Text = script.Do;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            button1_Click(null, null);
            script.IsUseItem = chkUseItem.Checked;
            txdDo.Text = script.Do;
        }

        private void chkIsThuThap_CheckedChanged(object sender, EventArgs e)
        {
            button1_Click(null, null);
            script.IsThuThap = chkIsThuThap.Checked;
            txdDo.Text = script.Do;
        }

        private void txtInfoEx_TextChanged(object sender, EventArgs e)
        {

        }

        private void chkComplete_CheckedChanged(object sender, EventArgs e)
        {
            button1_Click(null, null);
            script.IsComplete = chkComplete.Checked;
            txdDo.Text = script.Do;
        }

        private void chkBienThan_CheckedChanged(object sender, EventArgs e)
        {
            button1_Click(null, null);
            script.IsBienThan = chkBienThan.Checked;
            txdDo.Text = script.Do;
        }

        private void atk1X_ValueChanged(object sender, EventArgs e)
        {
            //ChangeDo();
        }

        private void atk1Y_ValueChanged(object sender, EventArgs e)
        {
            //ChangeDo();
        }

        private void atk1Map_ValueChanged(object sender, EventArgs e)
        {
            //ChangeDo();
        }

        private void atk2X_ValueChanged(object sender, EventArgs e)
        {
            //ChangeDo();
        }

        private void atk2Y_ValueChanged(object sender, EventArgs e)
        {
            //ChangeDo();
        }

        private void atk2Map_ValueChanged(object sender, EventArgs e)
        {
            //ChangeDo();
        }

        private void atk3X_ValueChanged(object sender, EventArgs e)
        {
            //ChangeDo();
        }

        private void atk3Y_ValueChanged(object sender, EventArgs e)
        {
            //ChangeDo();
        }

        private void atk3Map_ValueChanged(object sender, EventArgs e)
        {
            //ChangeDo();
        }

        private void button2_Click(object sender, EventArgs e)
        {
                game.DoStringEx("MISSIONINFO = ''; cnt = 0; while true do local name, content = DataPool:GetPlayerMission_Memo(cnt); if string.len(name) > 0 then MISSIONINFO = MISSIONINFO .. name .. '=' .. content .. ';'; end cnt = cnt + 1; if cnt == 20 then return end end return MISSIONINFO;");
                game.LuaToString();
                string info = game.LuaToString();
                MessageBox.Show(info);
                if (MessageBox.Show(this, "Bạn có muốn thêm info", "MicroAuto", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    info = info.Replace("=", ":");
                    info = info.Replace("   ", "");
                    info = info.Replace("  ", "");
                    info.Trim(';');
                    foreach (string s in info.Split(';'))
                    {
                        ListViewItem item = new ListViewItem(s.Split('=')[0]);
                        item.SubItems.Add(s.Split('=')[1]);
                        //listViewInfo.Items.Add(item);
                    }
                }
        }
    }
}
