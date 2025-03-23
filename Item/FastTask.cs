using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace _i
{
    partial class FastTask : UserControl
    {
        Game game;
        public FastTask(Game game)
        {
            this.game = game;
            InitializeComponent();
            Disposed += FastTask_Disposed;
            Tag = game.TLBB.Name;
        }

        private void FastTask_Disposed(object sender, EventArgs e)
        {
            foreach (ListViewItem it in lvGame.Items)
            {
                var g = it.Tag as Game;
                g.IsWaitFast = false;
            }
        }

   

        Game Leader
        {
            get
            {
                foreach(ListViewItem i in lvGame.Items)
                {
                    var game = i.Tag as Game;
                    if (game.TLBB.NiceName == lename)
                    {                        
                        return game;
                    }
                }
                return null;
            }
        }


        string lename = "";

  
        private void FastTask_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            foreach (Game game in this.game.Party)
            {
                if (!game.TLBB.IsLeader)
                {
                    ListViewItem item = new ListViewItem(game.TLBB.Name);
                    item.SubItems.Add("Thành Viên");
                    item.Tag = game;
                    lvGame.Items.Add(item);
                }
                else
                {
                    ListViewItem item = new ListViewItem(game.TLBB.Name) { UseItemStyleForSubItems = false };
                    item.ForeColor = Color.Blue;
                    item.SubItems.Add("Đội Trưởng");
                    item.Tag = game;
                    lvGame.Items.Add(item);
                    lename = game.TLBB.NiceName;
                }
            }
            lvGame.Columns[0].Text = "Tổ Đội [" + lvGame.Items.Count + "]";
            string note = Game.IniParser.Read("CanQuet", lename);
            if (string.IsNullOrEmpty(note))
                note = Game.IniParser.Read("Item", "FastTask");
            List<ListViewItem> listItems = new List<ListViewItem>();
            foreach (string t in note.Split('\n'))
            {
                if (t.Trim().Length > 2 && t.Split('|').Length == 2)
                {
                    ListViewItem item = new ListViewItem(t.Split('|')[0].Trim());
                    if (t.Split('|')[1].Trim() == "1")
                        item.Checked = true;                    
                    item.SubItems.Add("Đang Chờ");
                    item.SubItems.Add("00:00");
                    listItems.Add(item);                    
                }
            }
            lvTask.Items.AddRange(listItems.ToArray());
            label1.Text = lvGame.Items.Count.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ListViewItem item = new ListViewItem(comboBox1.Text);
            item.SubItems.Add("Đang Chờ");
            item.SubItems.Add("00:00");
            item.Checked = true;
            lvTask.Items.Add(item);
            SaveTask();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            lvTask.Items.Clear();
            foreach (var i in comboBox1.Items)
            {
                if (i.ToString() == ("Luyện Kim") || i.ToString().Contains("Sửa Trang Bị"))
                    continue;
                ListViewItem item = new ListViewItem(i.ToString());
                item.Checked = true;
                item.SubItems.Add("Đang Chờ");
                item.SubItems.Add("00:00");
                lvTask.Items.Add(item);          
            }
            SaveTask();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(button2.Text == "Start")
            {
                tmrStart.Enabled = true;
                button2.Text = "Stop";
            }
            else
            {
                tmrStart.Enabled = false;
                button2.Text = "Start";
            }
        }

        private void xóaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            while(lvTask.SelectedItems.Count > 0)
            {
                lvTask.SelectedItems[0].Remove();
            }
            SaveTask();
        }
        public int TotalSec { get; set; } = 0;
        private void tmrStart_Tick(object sender, EventArgs e)
        {
            try
            {
                if (lvTask.Items.Cast<ListViewItem>().Where(i => i.SubItems[1].Text.Contains("Đang")).Count() == 0)
                {
                    return;
                }


             
                foreach (ListViewItem i in lvGame.Items)
                {
                    foreach (var game in Main.Instance.AllOnelineGame)
                    {
                        if (game.TLBB.Name == i.Text)
                        {
                            i.Tag = game;
                            break;
                        }
                    }
                }
                TotalSec = 0;
                foreach (ListViewItem i in lvTask.Items)
                {
                    string time = i.SubItems[2].Text;
                    if (time.Split(':').Length == 2)
                    {
                        int min = TDT.ParseAllInt(time.Split(':')[0]);
                        int sec = TDT.ParseAllInt(time.Split(':')[1]);
                        TotalSec += min * 60 + sec;
                    }
                }

                int tsec = TotalSec % 60;
                int tmin = (int)Math.Floor((decimal)TotalSec / 60);
             
                lvTask.Columns[2].Text = string.Format("{0:00}", tmin) + ":" + string.Format("{0:00}", tsec);
                if (Leader == null || Leader.TLBB.OnlineTimeSec < 10)
                    return;
                if (Leader.TLBB.PartyId == 0xFFFFFFFF)
                {
                    bool isAskTeam = false;
                    foreach (ListViewItem item in lvGame.Items)
                    {
                        var game = item.Tag as Game;
                        if (game == Leader)
                            continue;
                        if (game.TLBB.PartyId != 0xFFFFFFFF && !game.IsKhacTheGioi(Leader.TLBB))
                        {
                            Leader.AskTeam(game.TLBB.VISCIIName);
                            isAskTeam = true;
                            break;
                        }
                    }
                    if (!isAskTeam)
                        Leader.DoStringEx("Player:CreateTeamSelf();");
                    return;
                }
            
                if (!Leader.TLBB.IsLeader)
                {
                    foreach (var game in Leader.Party)
                    {
                        if (game.TLBB.IsLeader)
                        {
                            game.AppointLeader(Leader.TLBB.Name);
                            break;
                        }
                    }
                    return;
                }

                foreach (ListViewItem i in lvGame.Items)
                {
                    var game = i.Tag as Game;
                    game.IsWaitFast = false;
                }

             

                foreach (ListViewItem i in lvGame.Items)
                {
                    var game = i.Tag as Game;

                    if (game.Process.HasExited)
                    {
                        Leader.IsWaitFast = true;
                        foreach (var g in Main.Instance.AllOnelineGame)
                        {
                            if (g.TLBB.Name == i.Text)
                            {
                                i.Tag = g;
                                break;
                            }
                        }
                        continue;
                    }

                    if (game == Leader)
                        continue;
                    //if(game.TLBB.PartyId == 0xFFFFFFFF)
                    //{
                    //    //if(Leader )
                    //}
                    if (game.IsKhacTheGioi(Leader.TLBB))
                    {
                        game.GoToEx(Leader.CharX, Leader.CharY, (int)Leader.TLBB.MapId);
                        Leader.IsWaitFast = true;
                        foreach (ListViewItem it in lvGame.Items)
                        {
                            var g = it.Tag as Game;
                            g.IsWaitFast = true;
                        }
                    }
                    if (game.TLBB.PartyId == 0xFFFFFFFF)
                    {
                        game.AskTeam(Leader.TLBB.VISCIIName);
                        foreach (ListViewItem it in lvGame.Items)
                        {
                            var g = it.Tag as Game;
                            g.IsWaitFast = true;
                            Leader.IsWaitFast = true;
                        }
                    }
                }
                foreach (ListViewItem item in lvTask.Items.Cast<ListViewItem>().Where(i => i.SubItems[1].Text == "Đang Làm"))
                {
                    if (item.SubItems[1].Text == "Đang Làm")
                    {
                        string time = item.SubItems[2].Text;
                        if (time.Split(':').Length == 2)
                        {
                            int min = TDT.ParseAllInt(time.Split(':')[0]);
                            int sec = TDT.ParseAllInt(time.Split(':')[1]);
                            int truesec = min * 60 + sec;
                            truesec++;
                            TimeSpan tim = TimeSpan.FromSeconds(truesec);
                            item.SubItems[2].Text = (tim.Hours * 60 + tim.Minutes) + ":" + tim.Seconds;
                        }
                        if (item.Text == "Nhận Phỉ Thúy")
                        {
                            foreach (ListViewItem i in lvGame.Items)
                            {
                                var game = i.Tag as Game;
                                if (game.Missions.Contains(MissionsType.NhanPhiThuy))
                                {
                                    return;
                                }
                            }
                            item.SubItems[1].Text = "Hoàn Thành";
                        }
                        if (item.Text == "Nhận x2")
                        {
                            foreach (ListViewItem i in lvGame.Items)
                            {
                                var game = i.Tag as Game;
                                if (game.Missions.Contains(MissionsType.NhanX2))
                                {
                                    return;
                                }
                            }
                            item.SubItems[1].Text = "Hoàn Thành";
                        }
                        if (item.Text == "Đông x2")
                        {
                            foreach (ListViewItem i in lvGame.Items)
                            {
                                var game = i.Tag as Game;
                                if (game.Missions.Contains(MissionsType.DongX2))
                                {
                                    return;
                                }
                            }
                            item.SubItems[1].Text = "Hoàn Thành";
                        }
                        if (item.Text == "Nhận Chiến Công + Kiếm Chỉ")
                        {
                            foreach (ListViewItem i in lvGame.Items)
                            {
                                var game = i.Tag as Game;
                                if (game.Missions.Contains(MissionsType.NhanChienCong) || game.Missions.Contains(MissionsType.NhanKiemChi))
                                {
                                    return;
                                }
                            }
                            item.SubItems[1].Text = "Hoàn Thành";
                        }
                        if (item.Text == "Hàng Ngày")
                        {
                            foreach (ListViewItem i in lvGame.Items)
                            {
                                var game = i.Tag as Game;
                                if (game.Missions.Contains(MissionsType.ThuTaiVanMay) || game.Missions.Contains(MissionsType.LoLyHoa) || game.Missions.Contains(MissionsType.NguyenVongThienLinh))
                                {
                                    return;
                                }
                            }
                            item.SubItems[1].Text = "Hoàn Thành";
                        }
                        if (item.Text == "Bán Đồ - Cất Đồ - Trị Liệu")
                        {
                            foreach (ListViewItem i in lvGame.Items)
                            {
                                var game = i.Tag as Game;
                                if (game.Missions.Contains(MissionsType.TriLieu) || game.Missions.Contains(MissionsType.BanRac) || game.Missions.Contains(MissionsType.CatVang) || game.Missions.Contains(MissionsType.CatDo))
                                {
                                    return;
                                }
                            }
                            item.SubItems[1].Text = "Hoàn Thành";
                        }
                        if (item.Text == "Luyện Kim")
                        {
                            foreach (ListViewItem i in lvGame.Items)
                            {
                                var game = i.Tag as Game;
                                if (game.Missions.Contains(MissionsType.LuyenKim))
                                {
                                    return;
                                }
                            }
                            item.SubItems[1].Text = "Hoàn Thành";
                        }
                        if (item.Text == "Luyện Kim Nhanh")
                        {
                            foreach (ListViewItem i in lvGame.Items)
                            {
                                var game = i.Tag as Game;
                                if (game.Missions.Contains(MissionsType.LuyenKimNhanh))
                                {
                                    return;
                                }
                            }
                            item.SubItems[1].Text = "Hoàn Thành";
                        }
                        if (item.Text == "Sửa Trang Bị")
                        {
                            foreach (ListViewItem i in lvGame.Items)
                            {
                                var game = i.Tag as Game;
                                if (game.Missions.Contains(MissionsType.SuaTrangBi))
                                {
                                    return;
                                }
                            }
                            item.SubItems[1].Text = "Hoàn Thành";
                        }
                        if (item.Text == "Phiêu Miễu Phong")
                        {
                            if (Leader.Missions.Contains(MissionsType.DatDoiPhieuMieuPhong))
                                return;
                            item.SubItems[1].Text = "Hoàn Thành";
                        }
                        if (item.Text == "Q Tô Châu")
                        {
                            if (Leader.Missions.Contains(MissionsType.DatDoiQ123ToChau))
                                return;
                            item.SubItems[1].Text = "Hoàn Thành";
                        }
                        if (item.Text == "Q Lâu Lan")
                        {
                            if (Leader.Missions.Contains(MissionsType.DatDoiQ123LauLan))
                                return;
                            item.SubItems[1].Text = "Hoàn Thành";
                        }
                        if (item.Text == "Phân Giải Trang Bị Pet")
                        {
                            foreach (ListViewItem i in lvGame.Items)
                            {
                                var game = i.Tag as Game;
                                if (game.Missions.Contains(MissionsType.PhanGiaiTrangBiPet))
                                    return;
                            }
                            item.SubItems[1].Text = "Hoàn Thành";
                        }
                        if (item.Text == "Phiêu Miễu Phong Huyết Chiến")
                        {
                            if (Leader.Missions.Contains(MissionsType.DatDoiKhieuChienPhieuMieuPhong))
                                return;
                            item.SubItems[1].Text = "Hoàn Thành";
                        }
                        if (item.Text == "Yến Tử Ô")
                        {
                            if (Leader.Missions.Contains(MissionsType.DatDoiYenTuO))
                                return;
                            item.SubItems[1].Text = "Hoàn Thành";
                        }
                        if (item.Text == "Sát Tinh")
                        {
                            if (Leader.Missions.Contains(MissionsType.DatDoiSatTinh))
                                return;
                            item.SubItems[1].Text = "Hoàn Thành";
                        }
                        if (item.Text == "Tứ Tuyệt Trang")
                        {
                            if (Leader.Missions.Contains(MissionsType.DatDoiTuTuyetTrang))
                                return;
                            item.SubItems[1].Text = "Hoàn Thành";
                        }
                        if (item.Text == "Thiếu Thất Sơn")
                        {
                            if (Leader.Missions.Contains(MissionsType.DatDoiThieuThatSon))
                                return;
                            item.SubItems[1].Text = "Hoàn Thành";
                        }
                        if (item.Text == "Vương Lăng")
                        {
                            if (Leader.Missions.Contains(MissionsType.DatDoiVuongLang))
                                return;
                            item.SubItems[1].Text = "Hoàn Thành";
                        }
                        if (item.Text == "Tam Thần Huyễn Cảnh")
                        {
                            if (Leader.Missions.Contains(MissionsType.DatDoiTamThan))
                                return;
                            item.SubItems[1].Text = "Hoàn Thành";
                        }
                        if (item.Text == "Lan Hoàn Phúc Địa")
                        {
                            if (Leader.Missions.Contains(MissionsType.DatDoiPhucDia))
                                return;
                            item.SubItems[1].Text = "Hoàn Thành";
                        }

                        return;

                    }
                }
                foreach (ListViewItem item in lvTask.Items)
                {
                    if (!item.Checked)
                        continue;
                    if (item.SubItems[1].Text == "Đang Chờ")
                    {
                        if (item.Text == "Nhận Phỉ Thúy")
                        {
                            foreach (ListViewItem i in lvGame.Items)
                            {
                                var game = i.Tag as Game;
                                game.PushMissions(MissionsType.NhanPhiThuy);
                            }
                            item.SubItems[1].Text = "Đang Làm";
                        }
                        if (item.Text == "Nhận x2")
                        {
                            foreach (ListViewItem i in lvGame.Items)
                            {
                                var game = i.Tag as Game;
                                game.PushMissions(MissionsType.NhanX2);
                            }
                            item.SubItems[1].Text = "Đang Làm";
                        }
                        if (item.Text == "Đông x2")
                        {
                            foreach (ListViewItem i in lvGame.Items)
                            {
                                var game = i.Tag as Game;
                                game.PushMissions(MissionsType.DongX2);
                            }
                            item.SubItems[1].Text = "Đang Làm";
                        }
                        if (item.Text == "Nhận Chiến Công + Kiếm Chỉ")
                        {
                            foreach (ListViewItem i in lvGame.Items)
                            {
                                var game = i.Tag as Game;
                                game.PushMissions(MissionsType.NhanChienCong);
                                game.PushMissions(MissionsType.NhanKiemChi);
                            }
                            item.SubItems[1].Text = "Đang Làm";
                        }
                        if (item.Text == "Hàng Ngày")
                        {
                            foreach (ListViewItem i in lvGame.Items)
                            {
                                var game = i.Tag as Game;
                                game.PushMissions(MissionsType.ThuTaiVanMay);
                                game.PushMissions(MissionsType.LoLyHoa);
                                foreach (var packetItem in game.PacketItems.All)
                                {
                                    if (packetItem.ClearName == "nguyenlinhtuyen" && packetItem.Count >= 5)
                                    {
                                        game.PushMissions(MissionsType.NguyenVongThienLinh);
                                        break;
                                    }
                                }
                            }
                            item.SubItems[1].Text = "Đang Làm";
                        }
                        if (item.Text == "Bán Đồ - Cất Đồ - Trị Liệu")
                        {
                            foreach (ListViewItem i in lvGame.Items)
                            {
                                var game = i.Tag as Game;
                                game.PushMissions(new[] { MissionsType.TriLieu, MissionsType.BanRac, MissionsType.CatVang, MissionsType.CatDo });
                            }
                            item.SubItems[1].Text = "Đang Làm";
                        }
                        if (item.Text == "Luyện Kim")
                        {
                            foreach (ListViewItem i in lvGame.Items)
                            {
                                var game = i.Tag as Game;
                                game.PushMissions(MissionsType.LuyenKim);
                            }
                            item.SubItems[1].Text = "Đang Làm";
                        }
                        if (item.Text == "Luyện Kim Nhanh")
                        {
                            foreach (ListViewItem i in lvGame.Items)
                            {
                                var game = i.Tag as Game;
                                game.PushMissions(MissionsType.LuyenKimNhanh);
                            }
                            item.SubItems[1].Text = "Đang Làm";
                        }
                        if (item.Text == "Sửa Trang Bị")
                        {
                            foreach (ListViewItem i in lvGame.Items)
                            {
                                var game = i.Tag as Game;
                                game.PushMissions(MissionsType.SuaTrangBi);
                            }
                            item.SubItems[1].Text = "Đang Làm";
                        }
                        if (item.Text == "Q Lâu Lan")
                        {
                            Leader.PushMissions(MissionsType.DatDoiQ123LauLan);
                            item.SubItems[1].Text = "Đang Làm";
                        }
                        if (item.Text == "Q Tô Châu")
                        {
                            Leader.PushMissions(MissionsType.DatDoiQ123ToChau);
                            item.SubItems[1].Text = "Đang Làm";
                        }
                        if (item.Text == "Phiêu Miễu Phong")
                        {
                            Leader.PushMissions(MissionsType.DatDoiPhieuMieuPhong);
                            item.SubItems[1].Text = "Đang Làm";
                        }
                        if (item.Text == "Phân Giải Trang Bị Pet")
                        {
                            foreach (ListViewItem i in lvGame.Items)
                            {
                                var game = i.Tag as Game;
                                game.PushMissions(MissionsType.PhanGiaiTrangBiPet);
                            }
                            item.SubItems[1].Text = "Đang Làm";
                        }
                        if (item.Text == "Phiêu Miễu Phong Huyết Chiến")
                        {
                            Leader.PushMissions(MissionsType.DatDoiKhieuChienPhieuMieuPhong);
                            item.SubItems[1].Text = "Đang Làm";
                        }
                        if (item.Text == "Yến Tử Ô")
                        {
                            Leader.PushMissions(MissionsType.DatDoiYenTuO);
                            item.SubItems[1].Text = "Đang Làm";
                        }
                        if (item.Text == "Sát Tinh")
                        {
                            Leader.PushMissions(MissionsType.DatDoiSatTinh);
                            item.SubItems[1].Text = "Đang Làm";
                        }
                        if (item.Text == "Tứ Tuyệt Trang")
                        {
                            Leader.PushMissions(MissionsType.DatDoiTuTuyetTrang);
                            item.SubItems[1].Text = "Đang Làm";
                        }
                        if (item.Text == "Thiếu Thất Sơn")
                        {
                            Leader.PushMissions(MissionsType.DatDoiThieuThatSon);
                            item.SubItems[1].Text = "Đang Làm";
                        }
                        if (item.Text == "Vương Lăng")
                        {
                            Leader.PushMissions(MissionsType.DatDoiVuongLang);
                            item.SubItems[1].Text = "Đang Làm";
                        }
                        if (item.Text == "Tam Thần Huyễn Cảnh")
                        {
                            Leader.PushMissions(MissionsType.DatDoiTamThan);
                            item.SubItems[1].Text = "Đang Làm";
                        }
                        if (item.Text == "Lan Hoàn Phúc Địa")
                        {
                            Leader.PushMissions(MissionsType.DatDoiPhucDia);
                            item.SubItems[1].Text = "Đang Làm";
                        }
                        return;

                    }
                }
            }
            catch { }
        }

        private void FastTask_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveTask();
            foreach (ListViewItem it in lvGame.Items)
            {
                var g = it.Tag as Game;
                g.IsWaitFast = false;
            }
        }

        void SaveTask()
        {
            string note = "";
            foreach (ListViewItem item in lvTask.Items)
            {
                note += item.Text + "|" + item.Checked.ToInt() + "\r\n";
            }
            Game.IniParser.Write("Item", "FastTask", note);
            Game.IniParser.Write("CanQuet", lename, note);
        }

        private void listViewEx2_DoubleClick(object sender, EventArgs e)
        {
            if(lvGame.SelectedItems.Count > 0)
            {
                (lvGame.SelectedItems[0].Tag as Game).Active();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                if(Leader != null)
                {
                    Leader.DoStringEx(@"QUEST = '';
ActiveCount = GetActivePointCount();
for i = 0,ActiveCount - 1 do
	local id,Name,missionIndex,Level,Number,Time,Pattern,Content,activepoint,activeIsConst = EnumActivePoint(i);
	local level =Player:GetData('LEVEL');
	if(( id==11 or id==15 or id==7) and level < 70) then
		activepoint=activepoint/2;
	end
	local Micount = DataPool:GetPlayerMission_DataCountByte(missionIndex/4+580,math.mod(missionIndex,4));
	
	local npoint = 0;
	if 2 == id and level < 60 then
		npoint = 0;
	else
		_, npoint = EnumActivePointRatio(level);
		if activeIsConst == 0 then
			npoint = math.floor(activepoint);
		else
			npoint = math.floor(npoint*activepoint);
		end
		if npoint == 0 then
			npoint = 1;
		end
	end
	QUEST = QUEST .. '[' .. Name..'/'..Micount..'/'..Number..'/'..npoint..']';
end
return QUEST;");
                    string q = Leader.LuaToStringMemo("AllQuest");
                    Thread.Sleep(300);
                    q = Leader.LuaToStringMemo("AllQuest");
                    string msg = "Mèo kiểm tra thấy nhân vật này đã làm xong các nhiệm vụ sau";
                    if(q.Contains("Thử Tài Vận May/1/1"))
                    {
                        msg += "\r\n" + "Hàng Ngày";
                    }                   
                    if(q.Contains("Yến Tử Ổ/1/1"))
                    {
                        msg += "\r\n" + "Yến Tử Ô";
                    }
                    if (q.Contains("Sát Tinh/2/3"))
                    {
                        msg += "\r\n" + "Sát Tinh";
                    }
                    if (q.Contains("Tứ Tuyệt Trang/2/3"))
                    {
                        msg += "\r\n" + "Tứ Tuyệt Trang";
                    }
                    if (q.Contains("Sơ Chiến Phiêu Miễu Phong/1/1"))
                    {
                        msg += "\r\n" + "Phiêu Miễu Phong";
                    }
                    if (q.Contains("Thiếu Thất Sơn/1/1"))
                    {
                        msg += "\r\n" + "Thiếu Thất Sơn";
                    }
                    if (q.Contains("Vương Lăng/1/1"))
                    {
                        msg += "\r\n" + "Vương Lăng";
                    }
                    if (q.Contains("Khiêu Chiến Phiêu Miễu Phong/1/1"))
                    {
                        msg += "\r\n" + "Phiêu Miễu Phong Huyết Chiến";
                    }
                    if (q.Contains("Kiếm Trừ Yêu Ma/1/1"))
                    {
                        msg += "\r\n" + "Q Lâu Lan";
                    }
                    if (q.Contains("Trừ Phỉ Phá Tam Quan/1/1"))
                    {
                        msg += "\r\n" + "Q Tô Châu";
                    }
                    if (q.Contains("Tam Thần Huyễn Cảnh/1/1"))
                    {
                        msg += "\r\n" + "Tam Thần Huyễn Cảnh";
                    }
                    if (q.Contains("Lang Hoàn Phúc Địa/1/1"))
                    {
                        msg += "\r\n" + "Lan Hoàn Phúc Địa";
                    }
                    msg += "\r\n";
                    foreach (ListViewItem i in lvTask.Items)
                    {
                        if (i.Text == "Đang Chờ")
                        {
                            if (msg.Contains("\r\n" + i.Text + "\r\n"))
                            {
                                i.SubItems[1].Text = "Hoàn Thành";
                            }
                        }
                    }
                }
            }).Start();
        }

        private void checkAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem i in lvTask.Items)
                i.Checked = true;
        }

        private void uncheckAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem i in lvTask.Items)
                i.Checked = false;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem i in lvGame.Items)
            {
                var game = i.Tag as Game;
                game.Exit();
            }
        }

        private void lvTask_DragDrop(object sender, DragEventArgs e)
        {
            SaveTask();
        }

        private void lvTask_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            
        }

        private void FastTask_Resize(object sender, EventArgs e)
        {
            
        }
        public static bool IsAutoReset { get; set; }

        private void tmrReset_Tick(object sender, EventArgs e)
        {
            if (IsAutoReset)
            {
                if (DateTime.Now.Hour == 0 && DateTime.Now.Minute == 0 && DateTime.Now.Second <= 5)
                {
                    if (button2.Text == "Start")
                    {
                        tmrStart.Enabled = true;
                        button2.Text = "Stop";
                    }
                    lvTask.Items.Cast<ListViewItem>().Where(i => i.SubItems[1].Text.Contains("Hoàn Thành")).ForEach(i => i.SubItems[1].Text = "Đang Chờ");
                }
            }
        }

        private void lvTask_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            SaveTask();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            Dispose();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (button2.Text == "Start")
            {
                tmrStart.Enabled = true;
                button2.Text = "Stop";
            }
            else
            {
                tmrStart.Enabled = false;
                button2.Text = "Start";
            }
        }
    }
}
