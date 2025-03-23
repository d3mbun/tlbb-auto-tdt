using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace _i
{
    partial class Trader : Form
    {
        Game game;
        public Trader(Game game)
        {
            this.game = game;
            InitializeComponent();
            Icon = Properties.Resources.icon;
            Text = game.TLBB.GuildName + " - " + game.TLBB.GuildId;
        }

        private void control_Changed(object sender, EventArgs e)
        {
            txtCodeTN.Text = "@way," + cboCity.Text + "," + cboRefCity.Text + ","
                + cboIdxCity.Text + "," + cboIdxRefCity.Text + ","
                + nudCityId.Value + "," + nudRefCityId.Value + ","
                + nudCityClick1.Value + "," + nudCityClick2.Value + ","
                + nudRefCityClick1.Value + "," + nudRefCityClick2.Value;
        }

        private void btnGetThanh_Click(object sender, EventArgs e)
        {
            string mapName = TDT.MapToString(game.TLBB.MapId);
            cboCity.SelectedIndex = cboCity.FindString(mapName);
        }

        private void btnGetThanhTT_Click(object sender, EventArgs e)
        {
            string mapName = TDT.MapToString(game.TLBB.MapId);
            cboRefCity.SelectedIndex = cboRefCity.FindString(mapName);
        }

        private void btnGetMyCityID_Click(object sender, EventArgs e)
        {
            if (game.TLBB.MapId >= 500)
            {
                nudCityId.Value = game.TLBB.MapId;
            }
        }

        private void btnSaveWay_Click(object sender, EventArgs e)
        {
            SettingOld.SaveWAY(game.TLBB.GuildId.ToString(), txtCodeTN.Text);
            game.SetWay(txtCodeTN.Text);
        }

        private void Trader_Load(object sender, EventArgs e)
        {
            txtCodeTN.Text = SettingOld.LoadWAY(game.TLBB.GuildId.ToString());
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            game.DoStringEx("AutoTN = '@go';");
        }

        private void btnGetTTCityID_Click(object sender, EventArgs e)
        {
            if (game.TLBB.MapId >= 500)
            {
                nudRefCityId.Value = game.TLBB.MapId;
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            game.DoStringEx("AutoTN = '@bk';");
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            game.DoStringEx("AutoTN = '@st';");
        }
    }
}
