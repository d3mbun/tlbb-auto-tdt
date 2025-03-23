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
    partial class TalkChannel : UserControl
    {

        public TalkChannel()
        {
            InitializeComponent();
            //Icon = Properties.Resources.icon;
        }

        public static bool IsNear { get; set; }
        public static bool IsScene { get; set; }
        public static bool IsIPRegion { get; set; }
        public static bool IsGuildLeague { get; set; }
        public static bool IsGuild { get; set; }
        public static bool IsMenpai { get; set; }
        public static bool IsTeam { get; set; }
        public static bool IsBigWorld { get; set; }
        public static int Time { get; set; }

        public static void LoadSetting()
        {
            IsNear = Game.IniParser.Read("TalkChanel", "Near") == "True";
            IsScene = Game.IniParser.Read("TalkChanel", "Scene") != "False";
            IsIPRegion = Game.IniParser.Read("TalkChanel", "IPRegion") == "True";
            IsGuildLeague = Game.IniParser.Read("TalkChanel", "GuildLeague") == "True";
            IsGuild = Game.IniParser.Read("TalkChanel", "Guild") == "True";
            IsMenpai = Game.IniParser.Read("TalkChanel", "Menpai") == "True";
            IsTeam = Game.IniParser.Read("TalkChanel", "Team") == "True";
            IsBigWorld = Game.IniParser.Read("TalkChanel", "BigWorld") == "True";
            Time = TDT.ParseInt(Game.IniParser.Read("TalkChanel", "Time"));
            IsR = Game.IniParser.Read("TalkChanel", "R") == "True";
            if (Time == 0)
                Time = 180;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Game.IniParser.Write("TalkChanel", "Near", chk_near.Checked.ToString());
            Game.IniParser.Write("TalkChanel", "Scene", chk_scene.Checked.ToString());
            Game.IniParser.Write("TalkChanel", "IPRegion", chk_ipregion.Checked.ToString());
            Game.IniParser.Write("TalkChanel", "GuildLeague", chk_guild_league.Checked.ToString());
            Game.IniParser.Write("TalkChanel", "Guild", chk_guild.Checked.ToString());
            Game.IniParser.Write("TalkChanel", "Menpai", chk_menpai.Checked.ToString());
            Game.IniParser.Write("TalkChanel", "Team", chk_team.Checked.ToString());
            Game.IniParser.Write("TalkChanel", "BigWorld", chk_bigworld.Checked.ToString());
            Game.IniParser.Write("TalkChanel", "Time", numTime.Text);
            Game.IniParser.Write("TalkChanel", "R", chk_r.Checked.ToString());
            IsNear = Game.IniParser.Read("TalkChanel", "Near") == "True";
            IsScene = Game.IniParser.Read("TalkChanel", "Scene") != "False";
            IsIPRegion = Game.IniParser.Read("TalkChanel", "IPRegion") == "True";
            IsGuildLeague = Game.IniParser.Read("TalkChanel", "GuildLeague") == "True";
            IsGuild = Game.IniParser.Read("TalkChanel", "Guild") == "True";
            IsMenpai = Game.IniParser.Read("TalkChanel", "Menpai") == "True";
            IsTeam = Game.IniParser.Read("TalkChanel", "Team") == "True";
            IsBigWorld = Game.IniParser.Read("TalkChanel", "BigWorld") == "True";
            Time = TDT.ParseInt(Game.IniParser.Read("TalkChanel", "Time"));
            IsR = Game.IniParser.Read("TalkChanel", "R") == "True";
            if (Time == 0)
                Time = 180;
            SaveSetting();
            this.Dispose();
        }

        private void SaveSetting()
        {
         
      
        }

        private void Rao_Load(object sender, EventArgs e)
        {
            chk_near.Checked = Game.IniParser.Read("TalkChanel", "Near") == "True";
            chk_scene.Checked = Game.IniParser.Read("TalkChanel", "Scene") != "False";
            chk_ipregion.Checked = Game.IniParser.Read("TalkChanel", "IPRegion") == "True";
            chk_guild_league.Checked = Game.IniParser.Read("TalkChanel", "GuildLeague") == "True";
            chk_guild.Checked = Game.IniParser.Read("TalkChanel", "Guild") == "True";
            chk_menpai.Checked = Game.IniParser.Read("TalkChanel", "Menpai") == "True";
            chk_team.Checked = Game.IniParser.Read("TalkChanel", "Team") == "True";
            chk_bigworld.Checked = Game.IniParser.Read("TalkChanel", "BigWorld") == "True";           
            numTime.Text = Game.IniParser.Read("TalkChanel", "Time");
            chk_r.Checked = Game.IniParser.Read("TalkChanel", "R") == "True";
        }

        public static bool IsR { get; set; }

        private void chk_bigworld_CheckedChanged(object sender, EventArgs e)
        {
            SaveSetting();
        }

        private void chk_r_CheckedChanged(object sender, EventArgs e)
        {
            SaveSetting();
        }

        private void chk_near_CheckedChanged(object sender, EventArgs e)
        {
            SaveSetting();
        }

        private void chk_scene_CheckedChanged(object sender, EventArgs e)
        {
            SaveSetting();
        }

        private void chk_ipregion_CheckedChanged(object sender, EventArgs e)
        {
            SaveSetting();
        }

        private void chk_guild_league_CheckedChanged(object sender, EventArgs e)
        {
            SaveSetting();
        }

        private void chk_guild_CheckedChanged(object sender, EventArgs e)
        {
            SaveSetting();
        }

        private void chk_menpai_CheckedChanged(object sender, EventArgs e)
        {
            SaveSetting();
        }

        private void chk_team_CheckedChanged(object sender, EventArgs e)
        {
            SaveSetting();
        }

        private void numTime_TextChanged(object sender, EventArgs e)
        {
            SaveSetting();
        }
    }
}
