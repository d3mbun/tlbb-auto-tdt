using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace _i
{ 

    partial class AlarmVaoPhai : UserControl
    {
        private Game game;
        public AlarmVaoPhai(Game game)
        {
            this.game = game;            
            InitializeComponent();
            lblName.Text = game.TLBB.Name;
        }

        private void AlarmVaoPhai_Load(object sender, EventArgs e)
        {
            chkCoBan.Checked = game.Missions.Contains(MissionsType.NhiemVuThangCap);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            int index = cboMenpai.SelectedIndex + 1;
            if (index == 10)
                index = 32;
            if (index == 11)
                index = 37;
            if (index == 12)
                index = MENPAI.QuyCoc;
            if (index == 13)
                index = MENPAI.DaoHoa;
            if (index == 14)
                index = 0;
            if (index != 0)
            {
                game.IsSetMenPai = true;
                game.SetMenPai = index;
            }
            this.Dispose();
        }

        private void lblAlarm_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                game.Active();
            }
        }

        private void lblName_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                game.Active();
            }
        }

        private void lblClose_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Parent.Controls.Remove(this);
                if (AlarmEx.Instance.Controls.Count == 0 && AlarmEx.Instance.Visible)
                {
                    AlarmEx.Instance.Hide();
                }
            }
        }

        private void chkCoBan_CheckedChanged(object sender, EventArgs e)
        {
            if(Global.IsVIP == 0)
            {
                MessageBox.Show(this, "Chức năng này cần kích hoạt VIP", "MicroAuto", MessageBoxButtons.OK);
                chkCoBan.Checked = false;
            }
            game.PushMissions(MissionsType.NhiemVuThangCap, chkCoBan.Checked);
            game.SaveSetting();
        }

        private void lblName_Click(object sender, EventArgs e)
        {

        }

        private void cboMenpai_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
