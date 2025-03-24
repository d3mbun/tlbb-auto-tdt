using ChreneLib.Controls.TextBoxes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Authentication;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using System.Xml;

namespace _i
{
    partial class Main : Form
    {
        static string TXT = string.Empty;


        //public static List<IntPtr[]> CaptureMess = new List<IntPtr[]>();

        //[DllImport("user32")]
        //public static extern bool ChangeWindowMessageFilter(uint msg, int flags);
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Global.HookMessage)
            {
                int procId = (int)m.WParam;
                int index = (int)m.LParam;
                if (DicGame.ContainsKey(procId))
                {
                    var game = DicGame[procId];
                    game.ListReceives.Add(index);
                }
                //int[] var = new int[2] { 1, 2 };
                //CaptureMess.Add(new IntPtr[]{ m.WParam,m.LParam});
                //QueueMess.Add(m.WParam.ToString("X8"));
                //COPYDATASTRUCT dataCopy = (COPYDATASTRUCT)Marshal.PtrToStructure(m.LParam, typeof(COPYDATASTRUCT));
                //buffs = new byte[dataCopy.cbData];
                //Marshal.Copy(dataCopy.lpData, buffs, 0, dataCopy.cbData);
                ////string mess = Encoding.ASCII.GetString(buffs);
                ////if(textLogs.Lines.Length > 15)
                ////{
                ////    textLogs.Lines = txtLog.Lines.Skip(txtLog.Lines.Length - 10).ToArray();
                ////}
                ////textLogs.Text = mess + "\r\n" + textLogs.Text;
                ////PushLogEx(dataCopy.dwData.ToString("X8"));
                //QueueMess.Add(dataCopy.dwData.ToString("X8"));

                //if (cds.cbData > 20)
                //{
                //    buffs = new byte[cds.cbData];
                //    Marshal.Copy(cds.lpData, buffs, 0, cds.cbData);
                //    //byte[] buffer = new byte[buffs.Length];
                //    //buffer = buffs.ToArray();
                //    //for(int i = 0; i < buffer.Length; i++)
                //    //{
                //    //    buffer[i] = buff[i];
                //    //}
                //    ListReceices.Add(buffs);
                //}
                //Marshal.Copy(cds.lpData, buff, 0, cds.cbData);
                //int isMsg = -1;
                ////string hex = "";
                //int isAcBa = -1;
                //bool IsVuKho = false;
                //for (int i = 0; i < buff.Length - 10; i++)
                //{
                //    //hex += buff[i].ToString("X2");
                //    if (buff[i] == 0xDA && buff[i + 1] == 0x03 && buff[i + 6] == 0x03)
                //    {
                //        isMsg = i;
                //        break;
                //    }
                //    if (buff[i] == 0x1E && buff[i + 1] == 0x02 && buff[i + 6] == 0x03)
                //    {
                //        isMsg = i;
                //        break;
                //    }
                //    if(buff[i] == 0xB0 && buff[i + 1] == 0x04 && buff[i + 6] == 0x03)
                //    {
                //        IsVuKho = true;
                //        break;
                //    }
                //    if (buff.Length > 25)
                //    {
                //        if (buff[i] == 0xDA && buff[i + 1] == 0x03 && buff[i + 6] == 0x04)
                //        {
                //            isAcBa = i;
                //            break;
                //        }
                //    }
                //}
                //int procId = BitConverter.ToInt32(buff, 0);
                //Game game = null;
                //foreach (KeyValuePair<int, Game> kvp in DicGame)
                //{
                //    if (kvp.Value.ProcessId == procId)
                //    {
                //        game = kvp.Value;
                //    }
                //}
                //string msg = "";               
                //if (buff.Length > 25)
                //{
                //    byte[] buffer = new byte[buff.Length - 15];
                //    for (int i = 0; i < buffer.Length; i++)
                //    {
                //        buffer[i] = buff[i + 15];
                //    }
                //    msg = ConverterEx.VISCII2UnicodeEx(buffer);
                //    //if (User.Email == "tieudattai@yahoo.com")
                //    //    TDT.AppendFile("D:\\Logs.txt", msg);
                //}
                //if (IsVuKho)
                //{
                //    if(game != null)
                //    {
                //        if (msg.Contains("Khương"))
                //            game.VuKhoString = msg;
                //        if (msg.Contains("Người"))
                //            game.VuKhoString = msg;
                //        if (msg.Contains("Thu"))
                //            game.VuKhoString = msg;
                //        if (msg.Contains("Tây"))
                //            game.VuKhoString = msg;
                //        if (msg.Contains("Lam"))
                //            game.VuKhoString = msg;
                //        if (msg.Contains("Dung"))
                //            game.VuKhoString = msg;
                //        if (msg.Contains("Nhu"))
                //            game.VuKhoString = msg;
                //    }
                //}
                //if (isAcBa != -1)
                //{
                //    if (game != null)
                //    {
                //        if (TDT.VietLien(msg).Contains("duongmon") && (TDT.VietLien(msg).Contains("gianghotieutieu") || TDT.VietLien(msg).Contains("#{qyxt_15}")))
                //        {
                //            game.AcBa = 37;
                //            game.IsAlarmAcBa = false;
                //        }
                //        if (TDT.VietLien(msg).Contains("modung") && (TDT.VietLien(msg).Contains("gianghotieutieu") || TDT.VietLien(msg).Contains("#{qyxt_15}")))
                //        {
                //            game.AcBa = 32;
                //            game.IsAlarmAcBa = false;
                //        }
                //        if (TDT.VietLien(msg).Contains("tinhtuc") && (TDT.VietLien(msg).Contains("gianghotieutieu") || TDT.VietLien(msg).Contains("#{qyxt_15}")))
                //        {
                //            game.AcBa = 6;
                //            game.IsAlarmAcBa = false;
                //        }
                //        if (TDT.VietLien(msg).Contains("tieudao") && (TDT.VietLien(msg).Contains("gianghotieutieu") || TDT.VietLien(msg).Contains("#{qyxt_15}")))
                //        {
                //            game.AcBa = 9;
                //            game.IsAlarmAcBa = false;
                //        }
                //        if (TDT.VietLien(msg).Contains("thieulam") && (TDT.VietLien(msg).Contains("gianghotieutieu") || TDT.VietLien(msg).Contains("#{qyxt_15}")))
                //        {
                //            game.AcBa = 1;
                //            game.IsAlarmAcBa = false;
                //        }
                //        if (TDT.VietLien(msg).Contains("thienson") && (TDT.VietLien(msg).Contains("gianghotieutieu") || TDT.VietLien(msg).Contains("#{qyxt_15}")))
                //        {
                //            game.AcBa = 8;
                //            game.IsAlarmAcBa = false;
                //        }
                //        if (TDT.VietLien(msg).Contains("thienlong") && (TDT.VietLien(msg).Contains("gianghotieutieu") || TDT.VietLien(msg).Contains("#{qyxt_15}")))
                //        {
                //            game.AcBa = 7;
                //            game.IsAlarmAcBa = false;
                //        }
                //        if (TDT.VietLien(msg).Contains("ngamy") && (TDT.VietLien(msg).Contains("gianghotieutieu") || TDT.VietLien(msg).Contains("#{qyxt_15}")))
                //        {
                //            game.AcBa = 5;
                //            game.IsAlarmAcBa = false;
                //        }
                //        if (TDT.VietLien(msg).Contains("vodang") && (TDT.VietLien(msg).Contains("gianghotieutieu") || TDT.VietLien(msg).Contains("#{qyxt_15}")))
                //        {
                //            game.AcBa = 4;
                //            game.IsAlarmAcBa = false;
                //        }
                //        if (TDT.VietLien(msg).Contains("minhgiao") && (TDT.VietLien(msg).Contains("gianghotieutieu") || TDT.VietLien(msg).Contains("#{qyxt_15}")))
                //        {
                //            game.AcBa = 2;
                //            game.IsAlarmAcBa = false;
                //        }
                //        if (TDT.VietLien(msg).Contains("caibang") && (TDT.VietLien(msg).Contains("gianghotieutieu") || TDT.VietLien(msg).Contains("#{qyxt_15}")))
                //        {
                //            game.AcBa = 3;
                //            game.IsAlarmAcBa = false;
                //        }
                //    }
                //    //}
                //    //catch(Exception ex)
                //    //{
                //    //    Main.AddLog(ex.Message);
                //    //}
                //}
                ////if ((buff[4] == 0xDA && buff[5] == 0x03 && buff[10] == 0x03) || (buff[4] == 0x53 && buff[5] == 0x02 && buff[10] == 0x01 && buff[17] == 0xDA && buff[18] == 0x03 && buff[23] == 0x03))
                //if (isMsg != -1)
                //{
                //    try
                //    {
                //        foreach (KeyValuePair<int, Game> kvp in DicGame)
                //        {
                //            if (kvp.Value.ProcessId == procId)
                //            {
                //                Byte[] buffer;
                //                if (kvp.Value.Address.GameType == 1)
                //                {
                //                    buffer = new byte[buff.Length - isMsg - 11];
                //                    for (int i = (isMsg + 11); i < buff.Length - 1; i++)
                //                    {
                //                        buffer[i - (isMsg + 11)] = buff[i];
                //                        if (buff[i] < 0x20)
                //                        {
                //                            buffer[i - (isMsg + 11)] = 0x23;
                //                        }
                //                    }
                //                }
                //                else
                //                {
                //                    buffer = new byte[buff.Length - isMsg - 8];
                //                    for (int i = (isMsg + 8); i < buff.Length - 1; i++)
                //                    {
                //                        buffer[i - (isMsg + 8)] = buff[i];
                //                        if (buff[i] < 0x20)
                //                        {
                //                            buffer[i - (isMsg + 8)] = 0x23;
                //                        }
                //                    }
                //                }
                //                string content = DateTime.Now.ToString("HH:mm dd-MM") + "#" + "[" + kvp.Value.TLBB.Name + "]:" + ConverterEx.VISCII2Unicode(buffer);
                //                if (content.Split('#').Length > 2)
                //                {
                //                    string fromName = "";
                //                    for (int i = 2; i < content.Split('#').Length; i++)
                //                    {
                //                        if (i > 3)
                //                            break;
                //                        fromName += content.Split('#')[i];                                        
                //                    }
                //                    content = content.Split('#')[0] + "#" + "[" + fromName + "] -> " + content.Split('#')[1];
                //                    if (fromName.Length < 3)
                //                        content = "";
                //                }
                //                //else if (content.Split('#').Length > 1)
                //                //{
                //                //    content = content.Split('#')[0] + "#" + "[" + content.Split('#')[2] + "] -> " + content.Split('#')[1];
                //                //}
                //                if (content == "")
                //                    return;
                //                TDT.AppendFile(Global.ConfigPath + "\\Chat.txt", content);
                //                System.Media.SoundPlayer player = new System.Media.SoundPlayer();
                //                player.Stream = Properties.Resources.chat;
                //                player.Play();
                //                if (!ChatLogs.NotShow)
                //                {
                //                    ChatLogs.Instance.Show();
                //                    ChatLogs.Instance.ReadChat();
                //                    Win.Active(ChatLogs.Instance.Handle);
                //                }
                //            }
                //        }
                //    }
                //    catch
                //    {
                //    }
                //}
                //string msg = Encoding.ASCII.GetString(buff, 0, cds.cbData);
                //MessageBox.Show(msg);
            }

            if (m.Msg == 0x0312)
            {
                /* Note that the three lines below are not needed if you only want to register one hotkey.
                 * The below lines are useful in case you want to register multiple keys, which you can use a switch with the id as argument, or if you want to know which key/modifier was pressed for some particular reason. */
                Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);                  // The key of the hotkey that was pressed.
                KeyModifier modifier = (KeyModifier)((int)m.LParam & 0xFFFF);       // The modifier of the hotkey that was pressed.
                int id = m.WParam.ToInt32();                                        // The id of the hotkey that was pressed.
                // 1 - pause
                if (id == 1)
                {
                    Paused();
                }

                if (id == 50)
                {
                    PausedSkill = menuPausedSkill.Checked = !menuPausedSkill.Checked;
                    if (PausedSkill)
                    {
                        new Loader("Tắt Skill").Show();
                    }
                    else
                    {
                        new Loader("Bật Skill").Show();
                    }

                }

                // 2 - refresh
                if (id == 2)
                {
                    foreach (var game in AllOnelineGame)
                    {
                        if (game.Address.GameType != 1)
                        {
                            game.OkPhu();
                        }
                        else
                        {
                            if (Win.GetForegroundWindow() == game.Handle)
                            {
                                Refresh(game);
                                new Loader(game.TLBB.Name + ": Refresh").Show();
                            }
                        }
                    }
                }


                if (id == 40)
                {
                    AllOnelineGame.ForEach(g => g.Refresh());
                    new Loader("Refresh").Show();
                }

                // 3 - stop current user
                if (id == 37)
                {
                    Global.IsByPass = !Global.IsByPass;
                    if (Global.IsByPass)
                        new Loader("bật").Show();
                    else
                        new Loader("tắt").Show();
                }

                if (id == 3)
                {
                    foreach (var game in AllGame)
                    {
                        if (Win.GetForegroundWindow() == game.Handle)
                        {
                            game.Item.Checked = !game.Item.Checked;
                            if (game.Item.Checked)
                                new Loader(game.TLBB.Name + ": bật auto").Show();
                            else
                                new Loader(game.TLBB.Name + ": tắt auto").Show();
                        }
                    }
                }
                // 4 - show auto
                if (id == 4)
                {
                    Activate();
                    Win.Active(this.Handle);
                }
                // 5 - an game
                if (id == 5)
                {
                    foreach (var game in AllGame)
                    {
                        if (Win.GetForegroundWindow() == game.Handle)
                        {
                            game.Hide();
                        }
                    }
                }
                // 6 - nhat do
                if (id == 6)
                {
                    Global.IsPickItem = menuPickItem.Checked = !menuPickItem.Checked;
                    if (Global.IsPickItem)
                    {
                        new Loader("MicroAuto - bật nhặt đồ").Show();
                    }
                    else
                    {
                        new Loader("MicroAuto - tắt nhặt đồ").Show();
                    }
                    return;
                }

                // 8 - chế đồ
                if (id == 8)
                {
                    Game game = ForeGame;
                    if (game != null && game.TLBB.Online && game.Address.GameType == 2)
                    {
                        game.IsCheDo = !game.IsCheDo;
                        if (game.IsCheDo)
                            ShowMsg(game.TLBB.Name + " - bật chế đồ");
                        else
                            ShowMsg(game.TLBB.Name + " - tắt chế đồ");
                    }
                }

                // 9 - chuc phuc
                if (id == 9)
                {
                    xuốngNgựaToolStripMenuItem_Click(null, null);
                    new Loader("Xuống Ngựa").Show();
                }

                //12 - phu to doi
                if (id == 12)
                {
                    foreach (var game in AllOnelineGame)
                    {
                        if (Win.GetForegroundWindow() == game.Handle)
                        {
                            if (game.SafeTime > 0)
                            {
                                game.DoStringEx("PushEvent('TOGLE_BIGBANK'); PushEvent('UPDATE_BANK')");
                                new Loader(game.TLBB.Name + " - mở rương").Show();
                            }
                            else
                            {
                                new Loader(game.TLBB.Name + " - time an toàn").Show();
                            }
                            break;
                        }
                    }
                }
                // 13 - danh theo key
                if (id == 13)
                {
                    Global.AtkFollowKey = menuAtkFollowKey.Checked = !menuAtkFollowKey.Checked;
                    if (Global.AtkFollowKey)
                    {
                        new Loader("MicroAuto - bật đánh theo key").Show();
                    }
                    else
                    {
                        new Loader("MicroAuto - tắt đánh theo key").Show();
                    }
                }
                // 14 - theo doi
                if (id == 14)
                {
                    foreach (var game in AllOnelineGame)
                    {
                        if (Win.GetForegroundWindow() == game.Handle)
                        {
                            game.AskTeamFollow();
                            break;
                        }
                    }
                }
                // 15 - bo theo doi
                if (id == 15)
                {
                    foreach (var game in AllOnelineGame)
                    {
                        if (Win.GetForegroundWindow() == game.Handle)
                        {
                            game.StopFollow();
                            break;
                        }
                    }
                }
                // 16 - an toan bo game
                if (id == 16)
                {
                    AllGame.ForEach(g => g.Hide());
                }
                // 17 - trieu tap nhom
                if (id == 17)
                {
                    TrieuTapNhom();
                }
                // 18 - moi doi
                if (id == 18)
                {
                    foreach (var game in AllOnelineGame)
                    {
                        if (Win.GetForegroundWindow() == game.Handle)
                        {
                            MoiDoi(game);
                            break;
                        }
                    }
                }
                // 20 - luyen kim
                if (id == 20)
                {
                    foreach (var game in AllOnelineGame)
                    {
                        if (Win.GetForegroundWindow() == game.Handle)
                        {
                            if (game.Missions.Contains(MissionsType.LuyenKim))
                            {
                                game.RemoveMission(MissionsType.LuyenKim);
                                new Loader(game.TLBB.Name + " - tắt luyện kim").Show();
                            }
                            else
                            {
                                game.PushMissions(MissionsType.LuyenKim);
                                new Loader(game.TLBB.Name + " - bật luyện kim").Show();
                            }
                            break;
                        }
                    }
                }
                // 21 - bach hoa duyen
                if (id == 21)
                {
                    foreach (var game in AllOnelineGame)
                    {
                        if (Win.GetForegroundWindow() == game.Handle)
                        {
                            if (game.Missions.Contains(MissionsType.BachHoaDuyen))
                            {
                                game.RemoveMission(MissionsType.BachHoaDuyen);
                                new Loader(game.TLBB.Name + " - tắt bách hoa duyên").Show();
                            }
                            else
                            {
                                game.PushMissions(MissionsType.BachHoaDuyen);
                                new Loader(game.TLBB.Name + " - bật bách hoa duyên").Show();
                            }
                            break;
                        }
                    }
                }
                // 22 - trung ac
                if (id == 22)
                {
                    foreach (var game in AllOnelineGame)
                    {
                        if (Win.GetForegroundWindow() == game.Handle)
                        {
                            if (game.Missions.Contains(MissionsType.TrungAc))
                            {
                                game.RemoveMission(MissionsType.TrungAc);
                                new Loader(game.TLBB.Name + " - tắt trừng ác").Show();
                            }
                            else
                            {
                                game.PushMissions(MissionsType.TrungAc);
                                new Loader(game.TLBB.Name + " - bật trừng ác").Show();
                            }
                            game.SaveSetting();
                            break;
                        }
                    }
                }

                // 24 - su mon
                if (id == 24)
                {
                    foreach (var game in AllOnelineGame)
                    {
                        if (Win.GetForegroundWindow() == game.Handle)
                        {
                            if (game.Missions.Contains(MissionsType.NhiemVuSuMon))
                            {
                                game.RemoveMission(MissionsType.NhiemVuSuMon);
                                new Loader(game.TLBB.Name + " - tắt Q sư môn").Show();
                            }
                            else
                            {
                                game.PushMissions(MissionsType.NhiemVuSuMon);
                                new Loader(game.TLBB.Name + " - bật Q sư môn").Show();
                            }
                            break;
                        }
                    }
                }
                // 25 - tbb
                if (id == 25)
                {
                    foreach (var game in AllOnelineGame)
                    {
                        if (Win.GetForegroundWindow() == game.Handle)
                        {
                            if (game.Missions.Contains(MissionsType.TuBaoBon))
                            {
                                game.RemoveMission(MissionsType.TuBaoBon);
                                new Loader(game.TLBB.Name + " - tắt tụ bảo bồn").Show();
                            }
                            else
                            {
                                game.PushMissions(MissionsType.TuBaoBon);
                                new Loader(game.TLBB.Name + " - bật tụ bảo bồn").Show();
                            }
                            break;
                        }
                    }
                }
                // 26 - tu duong
                if (id == 26)
                {
                    lênNgựaToolStripMenuItem_Click(null, null);
                    new Loader("Lên Ngựa").Show();
                }
                // 27 - exit
                if (id == 27)
                {
                    foreach (var game in AllOnelineGame)
                    {
                        if (Win.GetForegroundWindow() == game.Handle)
                        {
                            game.Exit();
                            break;
                        }
                    }
                }
                // 28 - trieu tap
                if (id == 28)
                {
                    foreach (var game in AllOnelineGame)
                    {
                        if (Win.GetForegroundWindow() == game.Handle)
                        {
                            TrieuTap(game);
                        }
                    }
                }
                // 29 - nhiem vu co ban
                if (id == 29)
                {
                    foreach (var game in AllOnelineGame)
                    {
                        if (Win.GetForegroundWindow() == game.Handle)
                        {
                            if (game.Missions.Contains(MissionsType.NhiemVuThangCap))
                            {
                                game.RemoveMission(MissionsType.NhiemVuThangCap);
                                new Loader(game.TLBB.Name + " - tắt nhiệm vụ cơ bản").Show();
                            }
                            else
                            {
                                game.PushMissions(MissionsType.NhiemVuThangCap);
                                new Loader(game.TLBB.Name + " - bật nhiệm vụ cơ bản").Show();
                            }
                            game.SaveSetting();
                        }
                    }
                }
                //// 30 - ks knb
                //if(id == 30)
                //{
                //    var game = GetForeGame();
                //    if(game != null)
                //    {
                //        menuAutoMuaKNB_Click(null, null);
                //    }
                //}
                if (id == 31)
                {
                    DiemDanh();
                    foreach (ListViewItem item in Lv.Items)
                    {
                        Game game = (Game)item.Tag;
                        game.IsP = true;
                        game.TraQ = game.NhanQ = game.IsClick = game.IsContinute = false;
                    }
                }
                if (id == 32)
                {
                    mởGameToolStripMenuItem_Click(null, null);
                }
                if (id == 33)
                {
                    Game game = ForeGame;
                    if (game != null)
                    {
                        game.IsAskPk = !game.IsAskPk;
                        new Loader(game.TLBB.Name + " - Tuyên chiến " + (game.IsAskPk ? "Bật" : "Tắt")).Show();
                    }

                }
                if (id == 34)
                {
                    Game game = ForeGame;
                    if (game != null)
                    {
                        if (game.Missions.Contains(MissionsType.LongPhuMau))
                        {
                            game.RemoveMission(MissionsType.LongPhuMau);
                            new Loader(game.TLBB.Name + " - tắt lòng phụ mẫu").Show();
                        }
                        else
                        {
                            game.PushMissions(MissionsType.LongPhuMau);
                            new Loader(game.TLBB.Name + " - bật lòng phụ mẫu").Show();
                        }
                    }
                }
                if (id == 36)
                {
                    if (AutoPK == null || AutoPK.IsDisposed)
                    {
                        AutoPK = new AutoPK();
                        AutoPK.Show();
                        Win.Active(AutoPK);
                    }
                }
                if (id == 51)
                {
                    AllOnelineGame.ForEach(g => g.LUA.PackUp());
                }
                if (id == 52)
                {
                    AllOnelineGame.ForEach(g => g.MoveVongTron());
                }
            }
            base.WndProc(ref m);
        }

        public AutoPK AutoPK;

        private PictureBox title = new PictureBox(); // create a PictureBox
        private Label minimise = new Label(); // this doesn't even have to be a label!
        private Label maximise = new Label(); // this will simulate our this.maximise box
        private Label close = new Label(); // simulates the this.close box

        private bool drag = false; // determine if we should be moving the form
        private Point startPoint = new Point(0, 0); // also for the moving

        public static User User = new User()
        {
            Dock = DockStyle.Fill
        };
        //public static GlobalKeyboardHook gkh = new GlobalKeyboardHook();
        public static Game CurGame;
        //public static TextBox TxtLog;
        private bool isRefresh = false;
        public delegate void CallBack(Game game);
        public delegate void LogBack(string log);
        public delegate void PipeBack(Game game);
        public delegate void CallBacks(List<Game> games);


        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        enum KeyModifier
        {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4,
            WinKey = 8
        }

        public const uint WM_COPYDATA = 0x4A;
        [StructLayout(LayoutKind.Sequential)]
        struct COPYDATASTRUCT
        {
            public int dwData;
            public int cbData;
            public IntPtr lpData;
        }


        //public static List<byte[]> ListReceice = new List<byte[]>();



        private void Paused()
        {
            Global.Paused = !Global.Paused;
            if (Global.Paused)
            {
                menuPause.Checked = true;
                Icon = Properties.Resources.paused;
                icon.Icon = Properties.Resources.paused;
                foreach (KeyValuePair<int, Game> kvp in DicGame)
                {
                    var game = kvp.Value;
                    if (game.TLBB.Online)
                    {
                        game.Move(game.RoundX + 1, game.RoundY - 1);
                    }
                }
                new Loader("MicroAuto: tạm dừng").Show();
            }
            else
            {
                menuPause.Checked = false;
                Icon = Properties.Resources.icon;
                icon.Icon = Properties.Resources.icon;
                new Loader("MicroAuto: tiếp tục").Show();
            }
        }

        void UnSetHoKey()
        {
            for (int i = 0; i < 100; i++)
            {
                UnregisterHotKey(Handle, i);
            }
        }

        private void SetHotKey()
        {
            // 1 - pause
            RegisterHotKey(this.Handle, 1, (int)(KeyModifier.Control | KeyModifier.Shift), Keys.Z.GetHashCode());
            RegisterHotKey(this.Handle, 1, 0, Keys.Pause.GetHashCode());
            // 2 - refresh
            RegisterHotKey(this.Handle, 2, (int)(KeyModifier.Control), Keys.W.GetHashCode());
            // 3 - stop current user
            RegisterHotKey(this.Handle, 3, 0, Keys.Insert.GetHashCode());
            RegisterHotKey(this.Handle, 3, (int)(KeyModifier.Control | KeyModifier.Shift), Keys.X.GetHashCode());
            // 4 - show auto
            RegisterHotKey(this.Handle, 4, 0, Keys.PageUp.GetHashCode());
            // 5 - an game
            RegisterHotKey(this.Handle, 5, 0, Keys.PageDown.GetHashCode());
            // 12 - phu to doi
            //RegisterHotKey(this.Handle, 12, (int)KeyModifier.Control, Keys.W.GetHashCode());
            // 13 - danh theo key
            RegisterHotKey(this.Handle, 13, (int)KeyModifier.Control, Keys.Q.GetHashCode());
            // 14 - theo doi
            RegisterHotKey(this.Handle, 14, (int)KeyModifier.Alt, Keys.I.GetHashCode());
            // 15 - bo theo doi
            RegisterHotKey(this.Handle, 15, (int)KeyModifier.Alt, Keys.K.GetHashCode());
            // 17 - trieu tap nhom
            RegisterHotKey(this.Handle, 17, (int)KeyModifier.Alt, Keys.F3.GetHashCode());
            // 18 - moi doi
            RegisterHotKey(this.Handle, 18, (int)KeyModifier.Control, Keys.End.GetHashCode());
            // 28 - trieutap
            RegisterHotKey(this.Handle, 28, (int)KeyModifier.Alt, Keys.F2.GetHashCode());
            // 31 - P
            RegisterHotKey(this.Handle, 31, (int)KeyModifier.Control, Keys.P.GetHashCode());
            // 32 - open game
            RegisterHotKey(this.Handle, 32, (int)KeyModifier.Control, Keys.O.GetHashCode());
            // 36 - askpk
            RegisterHotKey(this.Handle, 36, (int)KeyModifier.Control, Keys.I.GetHashCode());
            // 37 - vượt captcha
            RegisterHotKey(this.Handle, 37, (int)(KeyModifier.Control | KeyModifier.Shift), Keys.G.GetHashCode());

            // 20 - luyen kim
            RegisterHotKey(this.Handle, 20, (int)KeyModifier.Control, Keys.L.GetHashCode());
            // 21 - bach hoa duyen
            RegisterHotKey(this.Handle, 21, (int)KeyModifier.Control, Keys.H.GetHashCode());
            // 22 - trung ac
            RegisterHotKey(this.Handle, 22, (int)KeyModifier.Control, Keys.T.GetHashCode());
            // 27 - thoat game
            RegisterHotKey(this.Handle, 27, (int)KeyModifier.Control, Keys.Delete.GetHashCode());
            // 29 - nhiem vu co ban
            RegisterHotKey(this.Handle, 29, (int)KeyModifier.Control, Keys.Y.GetHashCode());
            // 24 - su mon
            RegisterHotKey(this.Handle, 24, (int)KeyModifier.Control, Keys.M.GetHashCode());
            // 9 - lên ngựa
            RegisterHotKey(this.Handle, 26, (int)KeyModifier.Control, Keys.U.GetHashCode());
            // 26 - xuống ngựa
            RegisterHotKey(this.Handle, 9, (int)KeyModifier.Control, Keys.D.GetHashCode());

            // 40 - refresh all
            RegisterHotKey(this.Handle, 40, (int)(KeyModifier.Alt), Keys.W.GetHashCode());
            // 16 - an toan bo game
            RegisterHotKey(this.Handle, 16, (int)KeyModifier.Control, Keys.PageDown.GetHashCode());
            //RegisterHotKey(this.Handle, 4, (int)(KeyModifier.Control | KeyModifier.Shift), Keys.C.GetHashCode());
            //RegisterHotKey(this.Handle, 5, (int)(KeyModifier.Control | KeyModifier.Shift), Keys.A.GetHashCode());
            //RegisterHotKey(this.Handle, 6, (int)(KeyModifier.Control | KeyModifier.Shift), Keys.Q.GetHashCode());
            //RegisterHotKey(this.Handle, 7, (int)KeyModifier.Control, Keys.W.GetHashCode());
            //RegisterHotKey(this.Handle, 8, (int)(KeyModifier.Alt), Keys.F1.GetHashCode());
            //RegisterHotKey(this.Handle, 9, (int)(KeyModifier.Alt), Keys.F11.GetHashCode());
            //RegisterHotKey(this.Handle, 10, 0, Keys.PageUp.GetHashCode());

            RegisterHotKey(this.Handle, 12, (int)(KeyModifier.Control), Keys.B.GetHashCode());
            RegisterHotKey(this.Handle, 50, (int)(KeyModifier.Control), Keys.K.GetHashCode());
            RegisterHotKey(this.Handle, 51, (int)(KeyModifier.Alt), Keys.Z.GetHashCode());
            RegisterHotKey(this.Handle, 52, (int)(KeyModifier.Alt), Keys.G.GetHashCode());
            //RegisterHotKey(this.Handle, 13, (int)(KeyModifier.Control | KeyModifier.Shift), Keys.I.GetHashCode());
        }



        void Title_MouseUp(object sender, MouseEventArgs e)
        {
            this.drag = false;
        }

        void Title_MouseDown(object sender, MouseEventArgs e)
        {
            this.startPoint = e.Location;
            this.drag = true;
        }

        void Title_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.drag)
            { // if we should be dragging it, we need to figure out some movement
                Point p1 = new Point(e.X, e.Y);
                Point p2 = this.PointToScreen(p1);
                Point p3 = new Point(p2.X - this.startPoint.X,
                                     p2.Y - this.startPoint.Y);
                this.Location = p3;
            }
        }
        private bool go;
        private int dx = 1;
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (go)
            {
                this.Location = new Point(this.Location.X + dx, this.Location.Y);
                if (Location.X < 10 || Location.X > 1200)
                {
                    go = false;
                    dx = -dx;
                }
                else
                {
                    this.Invalidate();
                }
            }
        }
        private void btnGo_Click(object sender, EventArgs e)
        {
            go = true;
            this.Invalidate();
        }



        public Main()
        {
            try
            {
                Console.WriteLine("main");
                InitializeComponent();
                Instance = this;
                LoadGeneralSetting();
                try
                {
                    Poster.DisableValidate();
                }
                catch
                {

                }

                Form.CheckForIllegalCrossThreadCalls = false;







                this.textBoxComment = new System.Windows.Forms.TextBox();
                this.textBoxComment.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                this.textBoxComment.Location = new Point(32, 104);
                this.textBoxComment.Multiline = true;
                this.textBoxComment.Name = "textBoxComment";
                this.textBoxComment.Size = new System.Drawing.Size(80, 16);
                this.textBoxComment.TabIndex = 3;
                this.textBoxComment.Text = "";
                this.textBoxComment.Visible = false;
                this.Controls.Add(this.textBoxComment);

                Lv.DrawColumnHeader += (ss, ee) =>
                {
                    if (ee.ColumnIndex == 0)
                    {
                        ee.DrawBackground();
                        bool value = false;
                        try
                        {
                            value = Convert.ToBoolean(ee.Header.Tag);
                        }
                        catch (Exception)
                        {
                        }
                        CheckBoxRenderer.DrawCheckBox(ee.Graphics,
                            new Point(ee.Bounds.Left + 4, ee.Bounds.Top + 4),
                            value ? System.Windows.Forms.VisualStyles.CheckBoxState.CheckedNormal :
                            System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal);

                        // Create string to draw.
                        String drawString = Lv.Columns[0].Text;

                        // Create font and brush.
                        // Font drawFont = new Font("Arial", 8);
                        Font drawFont = Lv.Font;
                        SolidBrush drawBrush = new SolidBrush(Color.Black);

                        // Create point for upper-left corner of drawing.
                        float x = ee.Bounds.Left + 18;
                        float y = ee.Bounds.Top + 5;

                        // Set format of string.
                        StringFormat drawFormat = new StringFormat();
                        drawFormat.FormatFlags = StringFormatFlags.DisplayFormatControl;

                        // Draw string to screen.
                        ee.Graphics.DrawString(drawString, drawFont, drawBrush, x, y, drawFormat);
                    }
                    else
                    {
                        ee.DrawDefault = true;
                    }
                };

                Lv.DrawItem += (ss, ee) =>
                {
                    ee.DrawDefault = true;
                };

                Lv.DrawSubItem += (ss, ee) =>
                {
                    ee.DrawDefault = true;
                };

                Lv.ColumnClick += (ss, ee) =>
                {
                    if (ee.Column == 0)
                    {
                        Lv.BeginUpdate();
                        bool value = false;
                        try
                        {
                            value = Convert.ToBoolean(Lv.Columns[ee.Column].Tag);
                        }
                        catch (Exception)
                        {
                        }
                        Lv.Columns[ee.Column].Tag = !value;
                        foreach (ListViewItem item in Lv.Items)
                            item.Checked = !value;
                        Lv.EndUpdate();

                        Lv.Invalidate();
                        //Lv.Items.Cast<ListViewItem>().Reverse();
                    }
                };

                //Lv.Columns[14].DisplayIndex = 3;

                Icon = Properties.Resources.icon;
                icon.Icon = Properties.Resources.icon;


                //Controls.Add(User);
                //statusStrip1.Visible = false;
                //User.Disposed += new EventHandler(User_Disposed);
                //User.BringToFront();
                //if (Win.GetHandle("WindowsForms", "MicroAuto") != IntPtr.Zero)
                //{
                //    Win.Active(Win.GetHandle("WindowsForms", "MicroAuto"));
                //    IsDispose = true;
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        void User_Update(object sender, EventArgs e)
        {
            this.Dispose();
        }

        //private void Hook()
        //{
        //    gkh.HookedKeys.Add(Keys.Insert);
        //    gkh.HookedKeys.Add(Keys.PageDown);
        //    gkh.HookedKeys.Add(Keys.Pause);
        //    gkh.HookedKeys.Add(Keys.F1);
        //    gkh.HookedKeys.Add(Keys.L);
        //    gkh.HookedKeys.Add(Keys.N);
        //    gkh.HookedKeys.Add(Keys.S);
        //    gkh.HookedKeys.Add(Keys.Home);
        //    gkh.HookedKeys.Add(Keys.D);
        //    gkh.HookedKeys.Add(Keys.M);
        //    gkh.HookedKeys.Add(Keys.B);
        //    gkh.HookedKeys.Add(Keys.F2);
        //    gkh.HookedKeys.Add(Keys.T);
        //    gkh.HookedKeys.Add(Keys.End);
        //    gkh.HookedKeys.Add(Keys.H);
        //    gkh.HookedKeys.Add(Keys.I);
        //    gkh.HookedKeys.Add(Keys.K);
        //    gkh.HookedKeys.Add(Keys.Q);
        //    gkh.HookedKeys.Add(Keys.W);
        //    gkh.HookedKeys.Add(Keys.J);
        //    gkh.HookedKeys.Add(Keys.E);
        //    gkh.HookedKeys.Add(Keys.I);
        //    gkh.HookedKeys.Add(Keys.U);
        //    gkh.HookedKeys.Add(Keys.Left);
        //    gkh.HookedKeys.Add(Keys.Right);
        //    gkh.HookedKeys.Add(Keys.Down);
        //    gkh.HookedKeys.Add(Keys.Z);
        //    gkh.HookedKeys.Add(Keys.C);
        //    gkh.HookedKeys.Add(Keys.F8);
        //    gkh.HookedKeys.Add(Keys.F9);
        //    gkh.HookedKeys.Add(Keys.Delete);            
        //    gkh.KeyDown += new KeyEventHandler(gkh_KeyDown);
        //}

        bool IsGame
        {
            get;
            set;
        }


        //public void DeleteItem(List<Game> games)
        //{
        //    foreach (Game game in games)
        //    {
        //        DeleteItem(game);
        //    }
        //    if (Global.AutoShutDown)
        //    {
        //        if (listViewMain.Items.Count == 0)
        //        {
        //            if (IsGame)
        //            {
        //                IsGame = false;
        //                new ShutDown();
        //            }                    
        //        }
        //        else
        //        {
        //            IsGame = true;
        //        }
        //    }
        //}



        //public static int MaxGame = 0;





        private void TrieuTap(Game foreGame)
        {
            if (foreGame == null)
                return;
            if (foreGame.TLBB.Name == "ĐăngNhập")
                return;


            AllOnelineGame.ToList().ForEach(game =>
            {
                if (game.TLBB.Online)
                    game.Move((int)foreGame.CharX, (int)foreGame.CharY, (int)foreGame.TLBB.MapId);
            });




            //if (Lv.SelectedItems.Count < 2)
            //{
            //    foreach (ListViewItem item in Lv.Items)
            //    {
            //        Game game = (Game)item.Tag;
            //        if (game.TLBB.Name == "ĐăngNhập")
            //            continue;
            //        if (game == foreGame)
            //            continue;
            //        game.Move((int)foreGame.CharX, (int)foreGame.CharY, (int)foreGame.TLBB.MapId);
            //    }
            //}
            //else
            //{
            //    foreach (ListViewItem item in Lv.SelectedItems)
            //    {
            //        Game game = (Game)item.Tag;
            //        if (game.TLBB.Name == "ĐăngNhập")
            //            continue;
            //        if (game == foreGame)
            //            continue;
            //        game.Move(foreGame.CharX, foreGame.CharY, (int)foreGame.TLBB.MapId);
            //    }
            //}
        }


        //Stopwatch timeAuto = Stopwatch.StartNew();
        Stopwatch delaytime = Stopwatch.StartNew();




        public static bool IsWait = false;
        public static int TrueGameCount = -1;
        Dictionary<int, Stopwatch> FakeGame = new Dictionary<int, Stopwatch>();
        static string LastMD5
        {
            get;
            set;
        }
        public void RenameFile(string originalName, string newName)
        {
            File.Move(originalName, newName);
        }

        bool isReName = false;
        Dictionary<string, string> DicMd5 { get; set; } = new Dictionary<string, string>();
        bool IsMonitor { get; set; }

        public static Dictionary<string, Address> DicAddress { get; set; } = new Dictionary<string, Address>();


        static Dictionary<string, uint> dicAddr = new Dictionary<string, uint>();

        public bool IsBinAcc { get; set; }

        public bool IsRunNemBienThan { get; set; }

        public void NemBienThan()
        {
            //if (!IsRunNemBienThan)
            //    IsRunNemBienThan = true;
            //else
            //    return;
            //while (true)
            //{
            //    if (Global.IsAdminEx)
            //        break;
            //    try
            //    {
            //        foreach (KeyValuePair<int, Game> kvp in DicGame.ToList())
            //        {
            //            //LECAOTRI2020LOITAMTHOI DONG LAI
            //            if (kvp.Value.Process.HasExited)
            //            {
            //                //deleteGames.Add(kvp.Value);
            //                if (CurGame == kvp.Value)
            //                    CurGame = null;
            //                this.Invoke(new CallBack(DeleteItem), kvp.Value);
            //            }
            //            if (kvp.Value.TLBB.Online)
            //                kvp.Value.NemBienThan();
            //        }
            //        Thread.Sleep(500);
            //    }
            //    catch { }
            //}
        }
        public bool IsRunNemBienThanEx { get; set; }
        public void NemBienThanEx()
        {
            if (!IsRunNemBienThanEx)
                IsRunNemBienThanEx = true;
            else
                return;
            while (true)
            {
                Thread.Sleep(500);
                //continue;
                ////if (!checkNemBienThan.Checked)
                ////    continue;
                //try
                //{

                //    foreach (GameObject p in Game.AllKSPlayer)
                //    {
                //        if (dicnem.ContainsKey(p.Id))
                //        {
                //            if (dicnem[p.Id].Elapsed.TotalSeconds < DelayBienThan)
                //                continue;
                //            dicnem[p.Id] = Stopwatch.StartNew();
                //        }
                //        else
                //        {

                //            dicnem.Add(p.Id, Stopwatch.StartNew());
                //        }



                //        Task.Run(() =>
                //        {
                //            p.Game.DownRide();
                //            p.Game.SelectTarget(p.Id);
                //            Thread.Sleep(500);
                //            p.Game.DoAction("Charm4_16");
                //        });
                //    }
                //}
                //catch { }
            }
        }

        Dictionary<uint, Stopwatch> dicnem = new Dictionary<uint, Stopwatch>();

        public void Monitor()
        {
            Console.WriteLine($"Monitor: {IsMonitor}");
            Process[] allProcesses = Process.GetProcesses();
            foreach (Process process in allProcesses)
            {
                try
                {
                    if (process.ProcessName == "Game")
                    {
                        var currentSessionID = Process.GetCurrentProcess().SessionId;
                        Console.WriteLine("currentSessionID", currentSessionID);
                        Console.WriteLine($"1, {Process.GetProcessesByName("Game").Where(p => p.SessionId == currentSessionID && !p.HasExited && p.HandleCount > 0 && !DicGame.ContainsKey(p.Id))}");
                    }
                    //Console.WriteLine($"Process: {process.ProcessName} | ID: {process.Id} | Title: {process.MainWindowTitle}");
                }
                catch { }
            }
            if (IsMonitor)
                return;
            IsMonitor = true;
            while (true)
            {
                //Console.WriteLine("Monitor");
                try
                {
                    if (Setting.Is("checkAnGameTime"))
                    {
                        var games = AllOnelineGame.OrderByDescending(g => g.HideTime.Elapsed.TotalSeconds).FirstOrDefault();
                        if (games != null)
                        {
                            if (games.HideTime.Elapsed.TotalMinutes > Setting.Value("numberAnGameTime") && Setting.Value("numberAnGameTime") > 0)
                            {
                                Win.ShowInactive(games);
                                games.IsHideAgain = true;
                            }
                        }
                    }
                    try
                    {
                        IntPtr closeHandle = Win.FindWindow("#32770", "Microsoft Visual C++ Runtime Library");
                        if (closeHandle != IntPtr.Zero)
                            Win.Close(closeHandle);
                        closeHandle = Win.FindWindow("#32770", "TTL3D");
                        if (closeHandle != IntPtr.Zero)
                            Win.Close(closeHandle);



                        foreach (var kvp in DicGame.Where(kvp => kvp.Value.Process.HasExited).ToList())
                        {
                            kvp.Value.SaveSetting();
                            if (CurGame == kvp.Value)
                                CurGame = null;
                            DicGame.Remove(kvp.Key);
                            this.Invoke(() =>
                            {
                                Lv.dicHides.Remove(kvp.Value.Item);
                                Lv.hideItems.Remove(kvp.Value.Item);
                                kvp.Value.Item.Remove();
                                if (Global.AutoShutDown)
                                    if (Lv.Items.Count == 0)
                                        new ShutDown();
                            });
                        };

                        // add new game
                        //if(DicGame.Count >= Global.MaxLogin)

                        //Console.WriteLine($"Process.GetProcessesByName: {Process.GetProcessesByName("Game")}");


                        var currentSessionID = Process.GetCurrentProcess().SessionId;
                        foreach (Process process in Process.GetProcessesByName("Game").Where(p => p.SessionId == currentSessionID && !p.HasExited && p.HandleCount > 0 && !DicGame.ContainsKey(p.Id)))
                        {
                            Console.WriteLine($"Gameeeeeeeeee: {process.ProcessName}");
                            string md5 = "";
                            Stopwatch sw = Stopwatch.StartNew();
                            if (Global.IsFull == 0)
                            {
                                if (Lv.Items.Count >= 2)
                                    continue;
                            }
                            else
                            {
                                if (Global.IsFull == 3)
                                {
                                    if (Lv.Items.Count >= 3)
                                        continue;
                                }
                                if (Global.IsFull == 7)
                                {
                                    if (Lv.Items.Count >= 7)
                                        continue;
                                }
                            }
                            try
                            {
                                if (!isReName)
                                {
                                    string link = process.MainModule.FileName;
                                    Global.linknew = link.Substring(0, link.Length - 8);
                                    try
                                    {
                                        File.Delete(Global.linknew + "/WebClient/webclient.exe");

                                    }
                                    catch { }
                                    try
                                    {
                                        File.Delete(Global.linknew + "/CrashReport.exe");
                                    }
                                    catch { }
                                    isReName = true;
                                }

                            }
                            catch
                            {

                            }
                            try
                            {

                                if (DicMd5.ContainsKey(process.MainModule.FileName))
                                {
                                    md5 = DicMd5[process.MainModule.FileName];
                                }
                                else
                                {
                                    DicMd5[process.MainModule.FileName] = TDT.Hasher.MD5(process.MainModule.FileName);
                                }
                                if ((Win.GetHandle(process.Id, "TianLongBaBu WndClass") == IntPtr.Zero && Win.GetHandle(process.Id, "knqtiabnclpogbh") == IntPtr.Zero) && Win.GetHandle(process.Id, "#32770") == IntPtr.Zero)
                                    continue;
                                if (dicAddr.ContainsKey(md5))
                                {
                                    Global.addmaytinh = dicAddr[md5];
                                }
                                else
                                {
                                    Global.addmaytinh = (uint)Memory.BaseGameAddreadnew(process.Id);//lecaotri2020 
                                    dicAddr.Add(md5, Global.addmaytinh);
                                }
                            }
                            catch
                            {
                                continue;
                            }

                            Console.WriteLine($"MD5: {md5}");
                            Console.WriteLine($"addmaytinh: {Global.addmaytinh}");

                            bool isFake = false;
                            if (FakeGame.ContainsKey(process.Id))
                            {
                                foreach (KeyValuePair<int, Stopwatch> kvp in FakeGame)
                                {
                                    if (kvp.Key == process.Id && (kvp.Value.Elapsed.TotalSeconds > 240))
                                    {
                                        isFake = true;
                                        try
                                        {
                                            Process.GetProcessById(process.Id).Kill();
                                        }
                                        catch { }
                                    }
                                }
                            }





                            if (isFake)
                                continue;
                            try
                            {


                                Address address = null;
                                if (DicAddress.ContainsKey(md5))
                                {
                                    address = DicAddress[md5];
                                }
                                else
                                {
                                    Global.OFFSET = "1234";
                                    if (Global.OFFSET.IndexOf(md5.ToUpper()) != -1)
                                    {
                                        if (!Global.addmaytinh.ToString("X8").Contains("00400000"))
                                        {

                                            address = Address.GetInstance(md5, Global.OFFSET, Global.addmaytinh);
                                        }
                                        else


                                        {
                                            address = Address.GetInstance(md5, Global.OFFSET, 0);
                                        }
                                    }
                                    else
                                    {
                                        // 4543299BF0394624684BF53AC97BC25B
                                        int charState = Memory.Scan("A1 ???????? 3BC1 56 57 0F85 ???????? 8B4D", -1, -1, 0, process.Id);
                                        charState = Memory.ReadAddressId(charState + 1, process.Id);
                                        //Main.PushLog(charState.ToString("X8"));
                                        if (!Global.addmaytinh.ToString("X8").Contains("00400000"))
                                        {
                                            address = Address.GetInstance(charState.ToString("X7"), Global.OFFSET, Global.addmaytinh);
                                        }
                                        else
                                        {
                                            address = Address.GetInstance(charState.ToString("X7"), Global.OFFSET, 0);
                                        }
                                        //Main.PushLog(address.CharState[0].ToString("X8"));
                                    }
                                    DicAddress.Add(md5, address);
                                }
                                Console.WriteLine($"address: {address}");
                                Game game = new Game(process, address);
                                DicGame.Add(process.Id, game);
                                game.MD5 = md5;
                                game.AddressGameExe = Global.addmaytinh;


                                game.SkillLoaded += new EventHandler(Game_SkillLoaded);


                                this.Invoke(() =>
                                {
                                    ListViewItem item = new ListViewItem(new string[] { "ĐăngNhập", "", "", "", "", "", "", "", "", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", "", "", "", "", "" })
                                    {
                                        UseItemStyleForSubItems = false,
                                    };
                                    item.Tag = game;
                                    game.Item = item;
                                    item.Checked = true;
                                    Lv.Items.Add(item);
                                });



                            }
                            catch
                            {
                                //Main.PushLog(ex.Message + ":" + ex.StackTrace);
                                //Main.PushLogEx(ex.Message);
                                if (!FakeGame.ContainsKey(process.Id))
                                {
                                    //PushLog("fake2");
                                    FakeGame.Add(process.Id, Stopwatch.StartNew());
                                }
                            }

                            //while (Global.IsFull == 0)
                            //    Thread.Sleep(1000);
                        }


                    }
                    catch
                    {
                    }
                }
                catch { }
                Thread.Sleep(2000);
            }

        }

        public Game ForeGame
        {
            get
            {
                foreach (var game in AllOnelineGame)
                {
                    if (Win.GetForegroundWindow() == game.Handle)
                    {
                        return game;
                    }
                }
                return null;
            }
        }


        void ShowMsg(string msg)
        {
            new Loader(msg).Show();
        }


        //public static int SuspendTime = 0;

        Thread ThreadMonitor;

        public static bool IsLoged = false;
        void User_Disposed(object sender, EventArgs e)
        {
            foreach (ToolStripItem m in menuIcon.Items)
            {
                m.Visible = true;
            }
            Task.Run(() => { SettingOld.MapIni = new IniParser(Poster.CurlGet("http://tieudattai.org/map.php", null, true)); });
            //statusStrip1.Visible = true;
            //linkLabel3.Visible = linkLabel4.Visible = true;
            try
            {
                try
                {
                    //ricInbox.Text = Game.IniParser.Read("Item", "ChatLogs");
                    //StringExtensions.RichTextBoxChangeWordColor(ref Main.Instance.ricInbox, "->", "<-", Color.BlueViolet);
                    //ricInbox.ScroolToEnd();
                    ricExit.Text = Game.IniParser.Read("Log", "Exit");
                    ricXong.Text = Game.IniParser.Read("Log", "Xong");
                    ricMaTac.Text = Game.IniParser.Read("Log", "MaTac");
                    ricAcBa.Text = Game.IniParser.Read("Log", "AcBa");


                    ricExit.ScroolToEnd();
                    ricXong.ScroolToEnd();
                    ricMaTac.ScroolToEnd();
                    ricAcBa.ScroolToEnd();

                    LoadSetting();
                }
                catch { }
                mainMenu.Visible = true;
                if (!User.IsLogged)
                    return;
                if (Global.IsFull > 0)
                {
                    Text = Global.Version + " - " + "Pro";
                    if (Global.IsFull == 3)
                    {
                        Text = Global.Version + " - " + "Lite";
                    }
                    if (Global.IsFull == 7)
                    {
                        Text = Global.Version + " - " + "Medi";
                    }
                }
                else
                {
                    Text = Global.Version + " - " + "Free";
                }




                try
                {
                    LoadGeneralSetting();
                }
                catch (Exception ex)
                {
                    Main.PushLogEx(ex.Message);
                }
                LoadUserInfo();
                LoadAccountInfo();

                //wb.Navigate(Global.HomePage + "/Help.php?email=" + HttpUtility.UrlEncode(User.Email) + "&pass=" + HttpUtility.UrlEncode(User.Pass) + "&version=" + HttpUtility.UrlEncode(Global.Version));
                //new Thread(() =>
                //{
                //    Thread.CurrentThread.IsBackground = true;
                //    adtext = Poster.CurlGet("http://tieudattai.org/ad.php");
                //    webBrowser1.DocumentText = adtext;
                //    webBrowser1.DocumentCompleted += WebBrowser1_DocumentCompleted;
                //}).Start();
            }
            catch { }
        }

        private void WebBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
        }

        private void Body_MouseDown1(object sender, HtmlElementEventArgs e)
        {
        }


        void conn()
        {
            //connThread();
            new Thread(new ThreadStart(connThread))
            {
                IsBackground = true
            }.Start();
        }


        void connThread()
        {

        }

        public static string WsState { get; set; } = string.Empty;




        private void PosterSec_Completed(object sender, EventArgs e)
        {
            var poster = sender as Poster;
            int time = TDT.ParseInt(poster.Response);
            //ThuHoachHoa.Sec = time;
        }

        public static Dictionary<int, Game> DicGame = new Dictionary<int, Game>();

        //kiem tra
        public static int LoginGameCount => DicGame.ToList().Where(kvp => kvp.Value.AutoTime.Elapsed.TotalSeconds < 120 && kvp.Value.IsSelectLogin == false && !kvp.Value.TLBB.Online).ToList().Count;
        
        void SkillLoaded(Game game)
        {
            try
            {
                Invoke(new Action(() =>
               {
                   game.Item.Checked = game.IsAuto;
               }));

            }
            catch { }
            if (CurGame == game)
            {
                LoadSkill();
            }
        }

        void SettingLoaded(Game game)
        {
            game.Item.Checked = game.IsAuto;
        }

        void Game_SkillLoaded(object sender, EventArgs e)
        {
            this.Invoke(new CallBack(SkillLoaded), sender as Game);
        }

        void game_SettingLoaded(object sender, EventArgs e)
        {
            this.Invoke(new CallBack(SettingLoaded), sender as Game);
        }

        void ValidSeri()
        {
            string serial = SettingOld.Read("User", "HWID");
            if (serial.Length == 32)
            {
                if (serial != FingerPrint.TrueSerial)
                {
                    Game.IniParser.Save("nier.ini");
                    Environment.Exit(0);
                }
            }
        }

        bool IsDispose
        {
            get;
            set;
        }

        private delegate int ChangeWindowMessageFilterDelegate(uint msg, int flag);
        static private ChangeWindowMessageFilterDelegate ChangeWindowMessageFilter;

        //public delegate int SetHookDelegate(IntPtr hanlde);
        //static public SetHookDelegate SetHook;
        //public delegate int GetMSGDelegate();
        //static public GetMSGDelegate GetMSG;

        string method_10(string string_5)
        {
            if (string_5 != "")
            {
                Match match = Regex.Match(string_5, "://(.+?)/");
                if (match != null)
                {
                    return match.ToString().Replace("://", "").Replace("/", "");
                }
            }
            return "";
        }

        private const int BACKLOG_SIZE = 10; //<<< Change this to 10, 20 ... 100 and see what happens!!!!
        private const int PORT = 0;
        private const int maxClients = 100;




        private byte[] byteData = new byte[4096];





        //static Socket serverSocket6002;
        //Socket serverSocket;
        private System.Windows.Forms.DateTimePicker dateTimePicker1 = new DateTimePicker();
        private System.Windows.Forms.ComboBox comboBox1 = new ComboBox();
        private System.Windows.Forms.CheckBox checkBoxDoubleClickActivation = new CheckBox();
        private System.Windows.Forms.TextBox textBoxComment = new TextBox();
        private System.Windows.Forms.TextBox textBoxPassword = new TextBox();
        private System.Windows.Forms.NumericUpDown numericUpDown1 = new NumericUpDown();
        private Control[] Editors;

        public static Setting SettingForm = new Setting() { Dock = DockStyle.Fill };

        private readonly IDictionary<string, Assembly> additional = new Dictionary<string, Assembly>();

        private Assembly ResolveAssembly(Object sender, ResolveEventArgs e)
        {
            Assembly res;
            additional.TryGetValue(e.Name.Split(',')[0], out res);

            ricExit.Text = e.Name;

            return res;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            //ClassLibrary2.Class1.Show();

            additional.Add("Newtonsoft.Json", Assembly.Load(File.ReadAllBytes("..\\Debug\\Newtonsoft.Json.dll")));
            

            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += ResolveAssembly;
            AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;

            tabSetting.Controls.Add(SettingForm);


            foreach (ColumnHeader col in Lv.Columns)
            {
                ToolStripMenuItem i = new ToolStripMenuItem(col.Text.ToString());
                menuListView.Items.Add(i);
                i.Tag = col;
                i.Click += (ss, ee) =>
                {
                    i.Checked = !i.Checked;
                    if (!i.Checked)
                    {
                        col.Tag = col.Width;
                        col.Width = 0;
                        hidecol.Add(col.Index);
                    }
                    else
                    {
                        listAutoHide.Remove(col.Index);
                        hidecol.Remove(col.Index);
                        col.Width = col.Tag.ToString().ToInt();
                        col.Tag = col.Width;
                    }
                };

            }

            foreach (var idx in Game.IniParser.Read("Config", "HideCol").Split(','))
            {
                if (idx.Length <= 0)
                    continue;
                foreach (ColumnHeader col in Lv.Columns)
                {
                    if (col.Index == idx.ToInt())
                    {
                        col.Tag = col.Width;
                        col.Width = 0;
                        hidecol.Add(col.Index);
                    }
                }
            }

            Form.CheckForIllegalCrossThreadCalls = false;
            mainMenu.Visible = false;
            Editors = new Control[] {
                                    dateTimePicker1,	// for column 0
									comboBox1,			// for column 1
									textBoxComment,		// for column 2
									textBoxPassword,	// for column 3
									numericUpDown1		// for column 4
									};



            lvConfig.SubItemClicked += (ss, ee) =>
            {
                if (ee.SubItem == 1)
                    lvConfig.StartEditing(Editors[2], ee.Item, ee.SubItem);
            };
            lvConfig.Scroll += (ss, ee) =>
            {
                lvConfig.EndEditing(false);
            };

            lvConfig.SubItemEndEditing += (ss, ee) =>
            {
                if (!IsHoldForDownSettingEx)
                {
                    SelectedOnlinedGames.ForEach(game =>
                    {
                        for (int i = 0; i < 12; i++)
                        {
                            game.KeyDelay[i] = TDT.ParseInt(lvConfig.Items[i + 6].SubItems[1].Text);
                            if (game.KeyDelay[i] == 0)
                                game.KeyDelay[i] = 1;
                        }
                        for (int i = 0; i < 9; i++)
                        {
                            game.KeyDelay[i + 13] = TDT.ParseInt(lvConfig.Items[i + 6 + 12].SubItems[1].Text);
                            if (game.KeyDelay[i + 13] == 0)
                                game.KeyDelay[i + 13] = 1;
                        }
                        game.KeyDelay[21] = lvConfig.Items[27].SubItems[1].Text.ToNumber();
                    });
                }
            };

            lvConfig.AllowReorder = false;
            lvConfig.AllowDrop = false;

            woker.RunWorkerAsync();
            Text = Global.Version;

            Config_Disposed(null, null);


            string fixboss = SettingOld.OnlyBossMap;
            try
            {
                bool isdis = true;
                do
                {
                    isdis = false;
                    foreach (ToolStripItem item in diChuyểnToolStripMenuItem.DropDownItems)
                    {
                        if (item.Tag != null && !string.IsNullOrEmpty(item.Tag.ToString().Trim()))
                        {
                            item.Dispose();
                            isdis = true;
                            break;
                        }
                    }
                }
                while (isdis);
                List<ToolStripMenuItem> lst = new List<ToolStripMenuItem>();
                string map = Game.IniParser.Read("Item", "DiChuyen");
                foreach (string line in map.Split('\n'))
                {
                    string l = line.Trim();
                    if (l.Split('|').Length == 3)
                    {
                        string xy = l.Split('|')[0];
                        string nicename = l.Split('|')[2];
                        ToolStripMenuItem item = new ToolStripMenuItem();
                        item.Text = nicename;
                        item.Tag = xy;
                        item.Click += Item_Click;
                        lst.Add(item);
                    }
                }
                if (lst.Count > 0)
                    diChuyểnToolStripMenuItem.DropDownItems.Add(new ToolStripSeparator() { Tag = "user" });
                diChuyểnToolStripMenuItem.DropDownItems.AddRange(lst.ToArray());
            }
            catch
            {
            }
            string captchahash = Game.IniParser.Read("Logs", "CaptchaHash");
            foreach (string s in captchahash.Split('\n'))
            {
                if (s.Trim() == string.Empty)
                    continue;
                Game.CaptchaHash.Add(s.Trim());
            }
            try
            {

                ServerTime = new ServerTime
                {
                    baseTime = DateTime.Now
                };
                try
                {
                    CalendarEx.LoadXML();
                }
                catch { }
                AlarmEx.Instance.Show();
                AlarmEx.Instance.Hide();
                TalkChannel.LoadSetting();
                try
                {
                    string ver = Environment.OSVersion.Version.Major + "." + Environment.OSVersion.Version.Minor;
                    ChangeWindowMessageFilter = (ChangeWindowMessageFilterDelegate)FunctionLoader.LoadFunction<ChangeWindowMessageFilterDelegate>(Environment.SystemDirectory + "\\user32.dll", "ChangeWindowMessageFilter");
                    ChangeWindowMessageFilter(WM_COPYDATA, 1);
                }
                catch { }
                Alan.Instance.Controls.Clear();

                Poster posterTime = new Poster();
                posterTime.Url = "http://www.tieudattai.org/remlaw/user.php";
                posterTime.Data = "cmd=time";
                posterTime.Completed += PosterTime_Completed;
                posterTime.AutoReconnect = true;
                posterTime.Control = this;
                posterTime.AutoReconnect = true;
                posterTime.Post();

                //Scripts.Load();
                if (IsDispose)
                {
                    Dispose();
                    return;
                }
                else
                {
                    icon.Visible = true;
                }
                Game.IsGiamDinh = true;
                Game.IsMuaNguyenLieu = true;
                //DM = DownloadManager.Instance;
                new Thread(new ThreadStart(ValidSeri))
                {
                    IsBackground = true
                }.Start();
                if (Game.IniParser.Read("Item", "Publishers") == string.Empty || !Game.IniParser.Read("Item", "Publishers").Contains("40") || Game.IniParser.Read("Item", "Publishers").Contains("Huyền Vũ"))
                {
                    Game.IniParser.Write("Item", "Publishers", Properties.Resources.Publishers);
                }
                MicroLogin.LoadXML();
                Publisher.LoadXML();









            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
            //if (User.Email == "tieudattai@yahoo.com")
            //    tmrReload.Interval = 30000;
            HideShowCol();
        }

        private void PosterTime_Completed(object sender, EventArgs e)
        {
            var poster = sender as Poster;
            string s = poster.Response;
            Global.ExaclyTime = DateTime.ParseExact(s, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        }

        public static ServerTime ServerTime;


        [DllImport("tieudattai.dll")]
        public static extern int GetMSG();

        private void control_CheckedChanged(object sender, EventArgs e)
        {
            if (IsHoldForDownSettingEx)
                return;
            Control control = (Control)sender;

            foreach (var game in SelectedOnlinedGames)
            {
                if (control == checkChetLenBai)
                {
                    game.IsAutoComeBack = checkChetLenBai.Checked;
                }
                if (control == checkChiNhat)
                {
                    game.IsOnlyPick = checkChiNhat.Checked;
                }
                if (control == checkBanKinh)
                {
                    if (checkBanKinh.Checked)
                    {
                        game.RadiusX = game.RoundX;
                        game.RadiusY = game.RoundY;
                        game.RadiusMap = game.TLBB.MapId;
                    }
                    else
                    {
                        game.RadiusX = game.RadiusY = 0;
                    }
                }
                if (control == checkQuyCoc)
                {
                    game.IsQuyCoc = checkQuyCoc.Checked;
                }
                if (control == checkChiDanh)
                {
                    game.IsOnlyAttack = checkChiDanh.Checked;
                }
                if (control == checkTimQuai)
                {
                    game.IsAttack = checkTimQuai.Checked;
                }
                if (control == checkLuaQuai)
                {
                    game.IsLure = checkLuaQuai.Checked;
                }
                if (control == checkPet)
                {
                    game.IsPet = checkPet.Checked;
                }
                if (control == checkHP)
                {
                    game.IsHP = checkHP.Checked;
                }
                if (control == checkMP)
                {
                    game.IsMP = checkMP.Checked;
                }
                if (control == checkNM)
                {
                    game.IsNM = checkNM.Checked;
                }
                if (control == checkKhaiKhoang)
                {
                    game.PushMissions(MissionsType.KhaiKhoang, checkKhaiKhoang.Checked);
                }
                if (control == checkHaiDuoc)
                {
                    game.PushMissions(MissionsType.HaiDuoc, checkHaiDuoc.Checked);
                }
                if (control == checkTrongTrot)
                {
                    game.PushMissions(MissionsType.TrongTrot, checkTrongTrot.Checked);
                }
                if (control == checkThuHoach)
                {
                    game.IsThuHoach = checkThuHoach.Checked;
                }
                if (control == chkRaoVat)
                {
                    game.IsRao = chkRaoVat.Checked;
                }
            }
        }



        private void listViewMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnCanQuetThuCong.Text = "Càn Quét [##:##]";
            var leader = SelectedOnlineLeaders.FirstOrDefault();
            if (leader != null)
            {
                if (Game.FastTasks.ContainsKey(leader.TLBB.Name))
                {
                    var FastTask = Game.FastTasks[leader.TLBB.Name];
                    btnCanQuetThuCong.Text = "Càn Quét [" + FastTask.TotalSec / 60 + ":" + FastTask.TotalSec % 60 + "]";
                }
            }
            if (Lv.SelectedItems.Count > 0)
            {
                CurGame = (Game)Lv.SelectedItems[0].Tag;
                txtRao.Text = CurGame.RaoTxt;
                LoadSkill();
            }

            tabBottom.TabPages[0].Text = CurGame.TLBB.Name;
            lbExpSpeed.Text = CurGame.ExpSpeedPerH;
            lblTimeInfo.Text = CurGame.ExpTime;
            lbHoaSpeed.Text = CurGame.HoaKTTSpeed;
            lbAcBa.Text = TLBB.GetMenpaiName(CurGame.AcBa);
            if (lbAcBa.Text.Contains("Không"))
                lbAcBa.Text = "LGBT";
            lblTrain.Text = "" + CurGame.ToaDoTrainEx;
            gameInfo.HPPercent = (float)CurGame.TLBB.HP / (CurGame.TLBB.MaxHP + 1);
            gameInfo.MPPercent = (float)CurGame.TLBB.MP / (CurGame.TLBB.MaxMP + 1);
            gameInfo.PetPercent = (float)CurGame.TLBB.PetHP / (CurGame.TLBB.PetMaxHP + 1);
            gameInfo.ExpPercent = (int)(CurGame.TLBB.Exp * 100 / CurGame.TLBB.MaxExp);
            gameInfo.MapName = CurGame.TLBB.MapName + " [" + CurGame.RoundX + " " + CurGame.RoundY + "]";
        }

        public IEnumerable<string> AllName
        {
            get
            {
                return DicGame.Select(kvp => kvp.Value).Where(g => g.TLBB.Online).Select(g => g.TLBB.Name);
            }
        }

        public void SaveSetting()
        {
            Game.IniParser.Write("Config", "HideCol", string.Join(",", hidecol.Where(i => !listAutoHide.Contains(i)).Select(i => i.ToString()).ToArray()));

            Game.IniParser.Write("Log", "Exit", string.Join("\r\n", ricExit.Lines.Reverse().Take(1000).Reverse().ToArray()));
            Game.IniParser.Write("Log", "Xong", string.Join("\r\n", ricXong.Lines.Reverse().Take(1000).Reverse().ToArray()));
            Game.IniParser.Write("Log", "AcBa", string.Join("\r\n", ricAcBa.Lines.Reverse().Take(1000).Reverse().ToArray()));
            Game.IniParser.Write("Log", "MaTac", string.Join("\r\n", ricMaTac.Lines.Reverse().Take(1000).Reverse().ToArray()));
            //Game.IniParser.Write("Item", "ChatLogs", ricInbox.Text);

            Game.IniParser.RemoveSection("Team");

            foreach (var k in Setting.Teams)
            {
                Game.IniParser.Write("Team", k.Key, string.Join(",", k.Value.ToArray()));
            }

            SettingForm.Save();

            foreach (var con in menuNier.GetDescendants())
            {
                try
                {
                    Game.IniParser.Write("SettingValue", con.GetHashCode().ToString(), con.Checked.ToString());
                }
                catch { }
            }

        }

        private void LoadSetting()
        {


            SettingForm.Load();

            foreach (var con in menuNier.GetDescendants())
            {
                try
                {
                    string value = Game.IniParser.Read("SettingValue", con.GetHashCode().ToString());
                    con.Checked = value == "True";
                }
                catch { }
            }

            try
            {
                foreach (var k in Game.IniParser["Team"])
                {
                    Setting.Teams[k.Key] = k.Value.Split(',').ToList();
                }
            }
            catch { }

            SetHotKey();

            SettingOld.SetBoQua();
        }

        private void LoadSkill()
        {
            tmrLoadSkill.Stop();
            IsHoldForDownSettingEx = true;
            tmrLoadSkill.Start();
        }

        public static Main Instance
        {
            get;
            set;
        }

        public static List<string> WaitToTell = new List<string>();

        public static void PushLog(object log)
        {
            Main.Instance.Invoke(new Action(() =>
            {
                if (log.ToString().Contains("Thoát"))
                {
                    Instance.ricExit.Text = Instance.ricExit.Text.Trim() + "\r\n" + log.ToString();
                    Instance.ricExit.ScroolToEnd();
                }
                if (log.ToString().Contains("Xong"))
                {
                    Instance.ricXong.Text = Instance.ricXong.Text.Trim() + "\r\n" + log.ToString();
                    Instance.ricXong.ScroolToEnd();
                }
                if (log.ToString().Contains("Mã Tặc"))
                {
                    Instance.ricMaTac.Text = Instance.ricMaTac.Text.Trim() + "\r\n" + log.ToString();
                    Instance.ricMaTac.ScroolToEnd();
                }
                if (log.ToString().Contains("Ác Bá"))
                {
                    Instance.ricAcBa.Text = Instance.ricAcBa.Text.Trim() + "\r\n" + log.ToString();
                    Instance.ricAcBa.ScroolToEnd();
                }
            }));
        }

        //static TextStyle infoStyle = new TextStyle(Brushes.Black, null, FontStyle.Regular);
        //static TextStyle warningStyle = new TextStyle(Brushes.BurlyWood, null, FontStyle.Regular);
        //static TextStyle errorStyle = new TextStyle(Brushes.Red, null, FontStyle.Regular);

        //public static void Logger(string text, Brush brush)
        //{
        //    Instance.Invoke(new Action(() =>
        //    {
        //        try
        //        {
        //            TextStyle style = new TextStyle(brush, null, FontStyle.Regular);
        //            var fctb = Instance.fTextChat;
        //            //some stuffs for best performance
        //            fctb.BeginUpdate();
        //            fctb.Selection.BeginUpdate();
        //            //remember user selection
        //            var userSelection = fctb.Selection.Clone();
        //            //add text with predefined style
        //            fctb.TextSource.CurrentTB = fctb;
        //            fctb.AppendText(text, style);
        //            //restore user selection
        //            if (!userSelection.IsEmpty || userSelection.Start.iLine < fctb.LinesCount - 2)
        //            {
        //                fctb.Selection.Start = userSelection.Start;
        //                fctb.Selection.End = userSelection.End;
        //            }
        //            else
        //                fctb.GoEnd();//scroll to end of the text
        //                             //
        //            fctb.Selection.EndUpdate();
        //            fctb.EndUpdate();
        //        }
        //        catch
        //        {

        //        }
        //    }));
        //}

        //public static void Logger(string text, Style style = null)
        //{            
        //    if (style == null)
        //        style = errorStyle;
        //    var fctb = Instance.fTextLog;
        //    //some stuffs for best performance
        //    fctb.BeginUpdate();
        //    fctb.Selection.BeginUpdate();
        //    //remember user selection
        //    var userSelection = fctb.Selection.Clone();
        //    //add text with predefined style
        //    fctb.TextSource.CurrentTB = fctb;
        //    fctb.AppendText(text, style);
        //    //restore user selection
        //    if (!userSelection.IsEmpty || userSelection.Start.iLine < fctb.LinesCount - 2)
        //    {
        //        fctb.Selection.Start = userSelection.Start;
        //        fctb.Selection.End = userSelection.End;
        //    }
        //    else
        //        fctb.GoEnd();//scroll to end of the text
        //    //
        //    fctb.Selection.EndUpdate();
        //    fctb.EndUpdate();
        //}

        static HashSet<string> Pips = new HashSet<string>();

        //public static void Pipe(object g) {
        //    if (Instance.InvokeRequired) {
        //        Instance.Invoke(new PipeBack(Pipe), g);
        //    } else {
        //        try {
        //            var game = g as Game;
        //            string pip = TDT.CleanName(game.TLBB.Name);
        //            //if (Pips.Contains(pip))
        //            //{
        //            //    return;
        //            //}
        //            //else
        //            //{
        //            //    Pips.Add(pip);
        //            //}
        //            int address = game.Memory.WriteString(@"\\.\pipe\" + TDT.CleanName(game.TLBB.Name));
        //            game.PostMessage(address, -11);
        //            NamedPipeServer PServer1 = new NamedPipeServer(@"\\.\pipe\" + TDT.CleanName(game.TLBB.Name), 0)
        //            {
        //                Game = game
        //            };
        //            PServer1.Start();
        //            pips.Add(PServer1);
        //            game.IsPipe = true;
        //            //TxtLog.Text = "dcm" + "\r\n" + TxtLog.Text;
        //        } catch (Exception ex) {
        //            MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace);
        //        }
        //    }
        //}

        //static List<NamedPipeServer> pips = new List<NamedPipeServer>();

        public static void PushLogEx(object log)
        {
            return;
        }


        // 208 193

        public bool IsHoldForDownSetting { get; set; }
        public bool IsHoldForDownSettingEx { get; set; }


        public class ComboboxItem
        {
            public string Text { get; set; }
            public int Value { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }

        private void listViewMain_DoubleClick(object sender, EventArgs e)
        {
            if (Lv.SelectedItems.Count == 0)
                return;
            Game game = (Game)Lv.SelectedItems[0].Tag;
            game.Active();
        }

        private void menuDebug_Click(object sender, EventArgs e)
        {
            DebugEx debug = new DebugEx() { TopMost = true };
            debug.Game = CurGame;
            debug.Show();

            //new Debug(CurGame).Show(this);

        }


        [DllImport("kernel32.dll")]
        static extern bool SetProcessAffinityMask(IntPtr hProcess, UIntPtr dwProcessAffinityMask);






        private void Main_Resize(object sender, EventArgs e)
        {


            if (WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;
                panBottom.Visible = !panBottom.Visible;
                ////tab.Visible = !tab.Visible;
                ////if (tab.Visible) {
                ////    Height = 560;
                ////    listViewMain.Height = 150;
                ////    Width = 385;
                ////} else {
                ////    Height = 200;
                ////    listViewMain.Height = Height;
                ////    Width = 600;
                ////}
            }
            else if (WindowState == FormWindowState.Minimized)
                isRefresh = true;
            else if (WindowState == FormWindowState.Normal && isRefresh)
            {
                isRefresh = false;
                Refresh();
            }

            HideShowCol();
        }

        List<int> listAutoHide = new List<int>();

        private void HideShowCol()
        {
            int cnt = 0;
            while (Lv.Columns.Cast<ColumnHeader>().Sum(i => i.Width) + 60 >= Width)
            {
                foreach (ColumnHeader col in Lv.Columns.Cast<ColumnHeader>().OrderByDescending(col => col.DisplayIndex))
                {
                    if (col.Width > 0)
                    {
                        listAutoHide.Add(col.Index);
                        col.Tag = col.Width;
                        col.Width = 0;
                        hidecol.Add(col.Index);
                        break;
                    }
                }
                if (cnt++ > Lv.Columns.Count)
                    break;
            }

            cnt = 0;

            while (Lv.Columns.Cast<ColumnHeader>().Sum(i => i.Width) + 60 < Width)
            {
                foreach (ColumnHeader col in Lv.Columns.Cast<ColumnHeader>().OrderBy(col => col.DisplayIndex))
                {
                    if (listAutoHide.Contains(col.Index))
                    {
                        if (Lv.Columns.Cast<ColumnHeader>().Sum(i => i.Width) + 60 >= Width)
                            break;
                        if (col.Width == 0 && col.Tag != null)
                        {
                            if (Lv.Columns.Cast<ColumnHeader>().Sum(i => i.Width) + 60 + col.Tag.ToString().ToInt() < Width)
                            {
                                hidecol.Remove(col.Index);
                                listAutoHide.Remove(col.Index);
                                col.Width = col.Tag.ToString().ToInt();
                                col.Tag = col.Width;
                                break;
                            }
                        }
                    }
                }
                if (cnt++ > Lv.Columns.Count)
                    break;
            }
        }

        public void Refresh(Game game)
        {
            game.Refresh();
        }

        private void menuHideGame_Click(object sender, EventArgs e)
        {
            //if (listViewMain.SelectedItems.Count == 0)
            //{
            //    foreach (ListViewItem item in listViewMain.Items)
            //    {
            //        Game game = (Game)item.Tag;
            //        game.Hide();
            //    }
            //    return;
            //}
            //foreach (ListViewItem item in listViewMain.SelectedItems)
            //{
            //    Game game = (Game)item.Tag;
            //    game.Hide();
            //}
            new Thread(new ThreadStart(HideGame))
            {
                IsBackground = true
            }.Start();
        }

        private void menuShowGame_Click(object sender, EventArgs e)
        {
            //if (listViewMain.SelectedItems.Count == 0)
            //{
            //    foreach (ListViewItem item in listViewMain.Items)
            //    {
            //        Game game = (Game)item.Tag;
            //        game.Active();
            //    }
            //    return;
            //}
            //foreach (ListViewItem item in listViewMain.SelectedItems)
            //{
            //    Game game = (Game)item.Tag;
            //    game.Active();
            //}
            new Thread(new ThreadStart(ShowGame))
            {
                IsBackground = true
            }.Start();
        }

        public void ShowGame()
        {
            foreach (Game game in SelectedGames)
            {
                game.Active();
                Thread.Sleep(500);
            }
        }

        public void HideGame()
        {
            foreach (Game game in SelectedGames)
            {
                if (Win.IsHideOrMini(game.Handle))
                {
                    game.Hide();
                }
            }
        }

        public void ShowAllGame()
        {
            foreach (Game game in AllOnelineGame)
            {
                if (Win.IsHideOrMini(game.Handle))
                {
                    game.Active();
                    Thread.Sleep(500);
                }
            }
        }

        public void HideAllGame()
        {
            foreach (Game game in AllOnelineGame)
            {
                game.Hide();
            }
        }

        private void menuExitGame_Click(object sender, EventArgs e)
        {
            if (Lv.Items.Count == 0)
                return;
            string msg = "Bạn có muốn thoát những game đã chọn?";
            if (Lv.SelectedItems.Count == 0)
                msg = "Bạn có muốn thoát toàn bộ game?";
            if (MessageBox.Show(this, msg, "MicroAuto", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            try
            {
                foreach (Game game in SelectedGames)
                {
                    game.Exit(false);
                    //GameCount = listViewMain.Items.Count;
                    //if (GameCount > MaxGame)
                    //    MaxGame = GameCount;
                }
            }
            catch { }
        }

        private void btnResetExpSpeed_Click(object sender, EventArgs e)
        {
            foreach (Game game in AllGame)
                game.ResetExpSpeed(true);
            AllOnelineGame.ToList().ForEach(g => g.ResetHoaSpee());
        }

        public static bool IsExit
        {
            get;
            set;
        }

        public void menuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();

            IsSaved = false;
            Task.Run(() =>
            {
                Thread.Sleep(10000);
                Application.Exit();
            });
            if (!User.IsDisposed)
            {
                this.Dispose();
                return;
            }

            new Loader(0, "Đang lưu thiết lập").Show();
            IsExit = true;
            Task.Run(() =>
            {
                try
                {
                    AllOnelineGame.ToList().ForEach(g => g.SaveSetting());
                    if (User.IsDisposed)
                    {
                        SaveSetting();
                        Game.IniParser.Save("nier.ini");
                    }
                }
                catch { }
                try
                {
                    foreach (Game game in AllGame)
                    {
                        if (game.IsHooked)
                        {
                            if (game.RecvAddress != 0)
                            {
                                game.ThreadAuto.Abort();
                                game.UnHookRecv();
                            }
                        }
                    }
                }
                catch { }
                try
                {
                    MicroLogin.SaveXML();
                }
                catch { }

                try
                {
                    if (Global.IniParser != null)
                        Poster.CurlPost("https://tieudattai.org/microauto/user.php?cmd=ini&do=save&email=" + HttpUtility.UrlEncode(User.Email) + "&pass=" + HttpUtility.UrlEncode(User.Pass), "content=" + HttpUtility.UrlEncode(Global.IniParser.ToString()), null, true);
                }
                catch { }

                IsSaved = true;
                Invoke(new Action(() =>
                {
                    try
                    {
                        this.Dispose();
                    }
                    catch { }

                }));


            });




        }

        //private void LoginSettings()
        //{
        //    string data = "cmd=stload&email=" + HttpUtility.UrlEncode(User.Email)
        //        + "&pass=" + HttpUtility.UrlEncode(User.Pass)
        //        ;
        //    Poster posterLogin = new Poster();
        //    posterLogin.Url = "http://www.tieudattai.org/remlaw/user.php";
        //    posterLogin.Data = data;
        //    posterLogin.Control = this;
        //    posterLogin.AutoReconnect = true;
        //    posterLogin.Completed += PosterLogin_Completed;
        //    posterLogin.Post();
        //}


        public static void ShowInvisible(Form form)
        {
            //// saving original settings
            //bool needToShowInTaskbar = form.ShowInTaskbar;
            //FormWindowState initialWindowState = form.WindowState;

            //// making form invisible
            //form.ShowInTaskbar = false;
            //form.WindowState = FormWindowState.Minimized;

            //// showing and hiding form
            //form.Show();
            //form.Hide();

            //// restoring original settings
            //form.ShowInTaskbar = needToShowInTaskbar;
            //form.WindowState = FormWindowState.Normal;
        }

        public static MicroLogin MicroLogin;

        private void LoadGeneralSetting()
        {
            panInbox.Controls.Add(new ChatLogs() { Dock = DockStyle.Fill });
            if (MicroLogin == null || MicroLogin.IsDisposed)
            {
                MicroLogin = new MicroLogin() { Dock = DockStyle.Fill };
                tabLogin.Controls.Add(new MicroLogin() { Dock = DockStyle.Fill });
            }

            //try
            //{
            //    MicroLogin.Accounts = AccountEx.All;
            //}
            //catch
            //{
            //    MicroLogin.Accounts = new List<AccountEx>();
            //}





            if (Global.HookMessage != -1)
            {
                Global.HookMessage = Win.RegisterWindowMessage("WM_SIMPLE_HOOK_WRITE");
            }

            menuPickItem.Enabled = menuItemFillter.Enabled = true;
            menuFollowKey.Enabled = menuAtkFollowKey.Enabled = true;


            tmrMonitor.Start();
            tmrSaveSettings.Start();
            LoadMSG();

            lblBeli.Text = TDT.FormatMoney(User.Beri) + " Beli";
            lblVND.Text = TDT.FormatMoney(User.Beli) + " VND";
            Poster.ErrorCount = 0;
            Console.WriteLine($"isValid: {TDT.FileHostValid}");
            if (TDT.FileHostValid)
            {
                ThreadMonitor = new Thread(new ThreadStart(Monitor))
                {
                    IsBackground = true
                };
                Thread ThreadNemBienThan = new Thread(new ThreadStart(NemBienThan))
                {
                    IsBackground = true
                };
                ThreadNemBienThan.Start();
                ThreadMonitor.Start();
                Task.Run(NemBienThanEx);
            }


        }



        public static bool IsSaved = false;



        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.WindowsShutDown && e.CloseReason != CloseReason.ApplicationExitCall)
            {
                if (MaximizeBox)
                {
                    Hide();
                    e.Cancel = true;
                }
                else
                {
                    if (!MinimizeBox)
                    {
                        MinimizeBox = MaximizeBox = true;
                        Control.Dispose();
                        e.Cancel = true;
                        return;
                    }
                }
            }
            else if (e.CloseReason == CloseReason.WindowsShutDown)
            {
                menuExit_Click(null, null);
            }
        }

        private void TrieuTap(object sender, EventArgs e)
        {
            var menu = sender as ToolStripMenuItem;
            var game = menu.Tag as Game;
            TrieuTap(game);
        }

        private void btnPay_Click(object sender, EventArgs e)
        {
            if (cboTelco.SelectedIndex == 11)
            {
                if (MessageBox.Show(this, "Bạn có chắc chắn điền đúng số điện thoại\r\nHãy thử rút một số nhỏ\r\nVí dụ 1.000 VND về ví\r\nThao tác không thể thu hồi nếu chuyển sai số điện thoại", "MicroAuto", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;
            }
            txtSeri.Text = txtSeri.Text.Replace(" ", "").Replace("-", "");
            txtCode.Text = txtCode.Text.Replace(" ", "").Replace("-", "");

            cboTelco.Enabled = txtCode.Enabled = txtSeri.Enabled = btnPay.Enabled = false;

            Poster poster = new Poster();
            poster.Control = this;
            poster.Url = "http://tieudattai.org/remlaw/donate.php"; ;
            poster.Data = "telco=" + (cboTelco.SelectedIndex + 1)
                + "&seri=" + HttpUtility.UrlEncode(txtSeri.Text)
                + "&code=" + HttpUtility.UrlEncode(txtCode.Text)
                + "&email=" + HttpUtility.UrlEncode(User.Email)
                + "&note=" + HttpUtility.UrlEncode(txtNote.Text)
                + "&value=" + TDT.ParseAllInt(cboMenhGia.Text)
                + "&pass=" + User.Pass;
            poster.Completed += (ss, ee) =>
            {
                MessageBox.Show(this, poster.Response, "MicroAuto", MessageBoxButtons.OK);
                cboTelco.Enabled = txtCode.Enabled = txtSeri.Enabled = btnPay.Enabled = true;
                Clipboard.SetText(poster.Response);
            };
            poster.AutoReconnect = true;
            poster.Post();
        }



        private void LoadMSG()
        {
            //Update update = new Update();
            //update.Updater += Update_Updater;
            //update.Show(this);
            Poster poster = new Poster();
            poster.Url = URL.HomePage + "microauto/user.php";
            poster.Control = this;
            poster.Data = "cmd=msg&email=" + HttpUtility.UrlEncode(User.Email) + "&pass=" + HttpUtility.UrlEncode(User.Pass);
            poster.Completed += (ss, ee) =>
            {
                if (!poster.IsError && poster.Response != string.Empty)
                {
                    if (poster.Response.ToLower().Contains("update"))
                    {
                        if (MessageBox.Show(this, "Đã có phiên bản mới bạn có muốn Update không", "MicroAuto", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            Process.Start("Update.exe");
                            menuExit_Click(null, null);
                        }
                    }
                    else
                    {
                        if (poster.Response.Trim() != string.Empty)
                        {
                            MessageBox.Show(this, poster.Response, "MicroAuto", MessageBoxButtons.OK);
                        }
                    }
                }
            };
            poster.Post();
        }



        //public static DownloadManager DM;

        void posterMSG_Completed(object sender, EventArgs e)
        {
            var poster = sender as Poster;
            if (!poster.IsError && poster.Response != string.Empty)
            {
                if (poster.Response.ToLower().Contains("update"))
                {
                    if (MessageBox.Show(this, "Đã có phiên bản mới bạn có muốn Update không", "MicroAuto", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Process.Start("Update.exe");
                        menuExit_Click(null, null);
                    }
                }
            }
            //if (User.IsAlarm)
            //{
            //    try
            //    {
            //        User.dl = new Downloader(ResourceLocation.FromURL("http://www.tieudattai.vn/microauto/alarm.mp3"), null, Path.GetTempPath() + "microauto.mp3", 8);
            //        DM.Add(User.dl, true);                   
            //    }
            //    catch
            //    {
            //    }
            //}
        }

        private void LoadUserInfo()
        {
            //picRefresh.Visible = false;
            Poster posterUserInfo = new Poster();
            posterUserInfo.Url = URL.HomePage + "microauto/user.php";
            posterUserInfo.Control = this;
            posterUserInfo.AutoReconnect = true;
            posterUserInfo.Data = "cmd=userinfo&email=" + HttpUtility.UrlEncode(User.Email) + "&pass=" + HttpUtility.UrlEncode(User.Pass);
            posterUserInfo.Completed += new EventHandler(posterUserInfo_Completed);
            posterUserInfo.Post();
        }

        void posterUserInfo_Completed(object sender, EventArgs e)
        {

            try
            {
                //picRefresh.Visible = true;
                //if (!File.Exists(Scripts.Path))
                //{
                //    TDT.FileInstall("MicroAuto", "Scripts.xml", Scripts.Path);
                //}

                Poster poster = (Poster)sender;
                if (poster.Response.ToLower().Contains("dis"))
                {
                    if (Global.IsAdminEx)
                    {
                        return;
                    }
                    menuExit_Click(null, null);
                    return;
                }
                if (poster.Response.Contains("&ksbong"))
                {
                    Global.IsChongKsBong = true;
                    string point = poster.Response.Substring(poster.Response.IndexOf("&ksbong"));
                }
                else
                {
                    Global.IsChongKsBong = false;
                    Global.ChongX = -255;
                    Global.ChongY = -255;
                }
                if (poster.Response.Contains("&st"))
                {
                    Global.IsSatTinh = true;
                }
                if (poster.Response.Contains("&tamthan"))
                {
                    Global.IsTamThan = true;
                }
                if (poster.Response.Contains("&hoahong"))
                {
                    Global.IsHoaHong = true;
                }
                if (poster.Response.Contains("&canquet"))
                {
                    Global.IsCanQuet = true;
                }
                if (poster.Response.Contains("&create"))
                {
                    Global.IsAutoCreate = true;
                }
                if (poster.Response.Contains("&gom"))
                {
                    Global.IsGom = true;
                }
                if (poster.Response.Contains("&bachbao"))
                {
                    Global.IsBachBao = true;
                }
                if (poster.Response.Contains("&qdbt"))
                {
                    //Global.IsQuanDoanBinhThanh = true;
                }
                if (poster.Response.Contains("&binhthanh"))
                {
                    Global.IsBinhThanh = true;
                }
                if (poster.Response.Contains("&thangcap"))
                {
                    Global.IsThangCap = true;
                }
                if (poster.Response.Contains("&fullchucnang"))
                {
                    BuyFunc.IsFullChucNang = true;
                }
                if (poster.Response.Contains("&qsh"))
                {
                    Global.IsQuanSonHai = true;
                }
                if (poster.Response.Contains("&tamky"))
                {
                    Global.IsTamKy = true;
                }
                Global.IsBankFull = true;
                Global.IsDungDoat = true;
                Global.PSShop = true;
                if (poster.Response.Contains("&gomknb"))
                {
                    Global.IsGomKNB = true;
                }
                if (poster.Response.Contains("&matac"))
                {
                    Global.IsMaTac = true;
                }
                if (poster.Response.Contains("baodohiem"))
                {
                    Global.IsBaoDoHiem = true;
                }
                if (poster.Response.Contains("&thoibong"))
                {
                    Global.IsThoiBong = true;
                }
                if (poster.Response.Contains("&bigball"))
                {
                    Global.IsBigBall = true;
                }
                if (poster.Response.Contains("&sudoo"))
                {
                    Global.IsSuDoo = true;
                }
                if (poster.Response.Contains("&huybt"))
                {
                    Global.IsHuyBienThan = true;
                }
                if (poster.Response.Contains("&thex"))
                {
                    Global.IsThuHoachHoa = true;
                }
                if (poster.Response.Contains("hopdientich"))
                {
                    Global.IsHopDienTichVoHon = true;
                }
                if (poster.Response.Contains("&cpcm"))
                {
                    Global.IsChucPhucCungMay = true;
                }
                else
                {
                    Global.IsChucPhucCungMay = false;
                }
                if (poster.Response.Contains("&9sao"))
                {
                    Global.Is9Sao = true;
                }
                if (poster.Response.Contains("&nhanbong"))
                {
                    Global.IsNhanBong = true;
                }
                if (poster.Response.Contains("&stb"))
                {
                    Global.IsSuaTrangBi = true;
                }
                if (poster.Response.Contains("&notdec"))
                {
                    Global.IsNotDec = true;
                }
                if (poster.Response.Contains("&rmad"))
                {
                    Global.RemoveAd = true;
                }
                if (poster.Response.Contains("&ksth"))
                {
                    Global.IsKsThuongHoi = true;
                }
                if (poster.Response.Contains("&s23"))
                {
                    Global.IsS23 = true;
                }
                if (poster.Response.Contains("phucdia"))
                {
                    Global.IsPhucDia = true;
                }
                if (poster.Response.Contains("&sinhtieu"))
                {
                    Global.IsSinhTieu = true;
                }
                if (poster.Response.Contains("&chienbi"))
                {
                    Global.IsChienBi = true;
                }
                if (poster.Response.Contains("&2c"))
                {
                    Global.Is2CaptchaEx = true;
                }
                if (poster.Response.Contains("&tkc"))
                {
                    Global.IsTKC = true;
                }
                if (poster.Response.Contains("&tc"))
                {
                    Global.IsTC = true;
                }
                if (poster.Response.Contains("&ttt"))
                {
                    Global.IsTTT = true;
                }
                if (poster.Response.Contains("&yto"))
                {
                    Global.IsYTO = true;
                }
                if (poster.Response.Contains("&btd"))
                {
                    Global.IsMoBTD = true;
                }
                if (poster.Response.Contains("&ks"))
                {
                    Global.IsKS = true;
                }
                if (poster.Response.Contains("&reg"))
                {
                    Global.IsReg = true;
                }
                if (poster.Response.Contains("&code"))
                {
                    Global.IsCode = true;
                }



                //Global.IsCode = poster.Response.Contains("&code");
                //if (Global.IsCode || Global.IsAdminEx)
                //    menuCode.Visible = true;
                muNhapCode.Visible = Global.IsCode;


                string[] info = poster.Response.Split('&');
                string beri = string.Format("{0:#,###}", int.Parse(info[3]));
                User.Beri = int.Parse(info[7]);
                if (beri == "")
                    beri = "0";
                if (beri.Contains("-"))
                    beri = "0";
                string beriex = string.Format("{0:#,###}", int.Parse(info[4]));
                if (beriex == "")
                    beriex = "0";
                string beli = string.Format("{0:#,###}", int.Parse(info[7]));
                if (beli == "")
                    beli = "0";
                User.Beli = int.Parse(info[8]);
                string hsd = HttpUtility.UrlDecode(info[5]);
                int year = TDT.ParseInt(hsd);
                Global.YearExp = year;
                //lecaotri
                //lblUserInfo.Text = "Name: " + HttpUtility.UrlDecode(info[0])
                //    + "\r\n" + "Email: " + User.Email
                //    + "\r\n" + "Giới tính: " + (info[1] == "1" ? "Nam" : "Nữ")
                //    + "\r\n" + "HSD: " + hsd
                //;
                lblBeli.Text = TDT.FormatMoney(User.Beri) + " Beli";
                lblVND.Text = TDT.FormatMoney(User.Beli) + " VND";
                if (Global.IsFull == 0)
                {
                    if (!info[5].Contains("hạn"))
                        Login();
                    else
                        Text = Global.Version + " - " + "Free";
                }
                else
                {

                }
            }
            catch
            {
            }
        }

        private void Buy24H()
        {
            Poster posterBuy = new Poster();
            posterBuy.Url = URL.HomePage + "microauto/user.php";
            posterBuy.Data = "cmd=buy&email=" + HttpUtility.UrlEncode(User.Email) + "&pass=" + HttpUtility.UrlEncode(User.Pass);
            string date = string.Empty;
            date = "24h";
            posterBuy.Data += "&date=" + date;
            posterBuy.Control = this;
            posterBuy.Completed += new EventHandler(posterBuy_Completed);
            posterBuy.Post();
        }

        void posterBuy_Completed(object sender, EventArgs e)
        {
            var poster = sender as Poster;
            if (poster.IsError)
                Buy24H();
            else
                LoadUserInfo();
        }

        private void Login()
        {
            Poster posterLogin = new Poster();
            posterLogin.Url = URL.LOGIN;
            posterLogin.Control = this;
            posterLogin.Data = "cmd=login&email=" + HttpUtility.UrlEncode(User.Email) + "&pass=" + HttpUtility.UrlEncode(User.Pass);
            posterLogin.Completed += new EventHandler(posterLogin_Completed);
            posterLogin.Post();
        }

        void posterLogin_Completed(object sender, EventArgs e)
        {
            Poster poster = (Poster)sender;
            if (poster.IsError)
            {
                Login();
                return;
            }
            try
            {
                SettingOld.Str = poster.Response.Substring(poster.Response.IndexOf('@'), poster.Response.Length - poster.Response.IndexOf('@'));
                Global.OFFSET = Regex.Replace(poster.Response, "@.*", "");
                Global.IsFull = SettingOld.LoadSetting("f")[0];
                if (Global.IsFull > 0)
                {
                    Text = Global.Version + " - " + "Pro";
                    if (Global.IsFull == 3)
                    {
                        Text = Global.Version + " - " + "Lite";
                    }
                    if (Global.IsFull == 7)
                    {
                        Text = Global.Version + " - " + "Medi";
                    }
                }
                else
                {
                    Text = Global.Version + " - " + "Free";
                }
                Global.IsVIP = SettingOld.LoadSetting("vip")[0];
                tmrMonitor.Enabled = true;
            }
            catch { }
        }

        private void cboTelco_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if(cboTelco.SelectedIndex <= 3)
            //{
            //    Process.Start("http://tieudattai.org/thong-bao-ho-tro-nap-the-dien-thoai-vao-auto/");
            //}
            if (cboTelco.SelectedIndex == 11)
            {
                txtSeri.WaterMark = "SĐT MOMO";
                txtCode.WaterMark = "Số VND";

                txtNote.WaterMark = "Tên Ví";
                btnPay.Text = "Rút";
            }
            else if (cboTelco.SelectedIndex == 10)
            {
                txtSeri.WaterMark = "Email";
                txtCode.WaterMark = "VND";

                btnPay.Text = "Chuyển";
                txtNote.WaterMark = "Note";
            }
            else if (cboTelco.SelectedIndex == 6)
            {
                txtSeri.WaterMark = "Email";
                txtCode.WaterMark = "Beli";

                btnPay.Text = "Chuyển";
                txtNote.WaterMark = "Note";
            }
            else if (cboTelco.SelectedIndex == 7)
            {
                txtSeri.WaterMark = "NewPass";
                txtCode.WaterMark = "Retype";
                btnPay.Text = "Đổi";
                txtNote.WaterMark = "Note";
            }
            else if (cboTelco.SelectedIndex == 8)
            {
                txtSeri.WaterMark = "Email";
                txtCode.WaterMark = "Day";
                btnPay.Text = "Chuyển";
                txtNote.WaterMark = "Note";
            }
            else if (cboTelco.SelectedIndex == 9)
            {
                txtSeri.WaterMark = "Email";
                txtCode.WaterMark = "Captcha";
                btnPay.Text = "Chuyển";
                txtNote.WaterMark = "Note";
            }
            else
            {
                txtSeri.WaterMark = "Seri";
                txtCode.WaterMark = "Code";
                btnPay.Text = "Nạp";
                txtNote.WaterMark = "Note";
            }

        }




        private void icon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ActiveAuto();
                //if (MicroLogin != null && MicroLogin.Visible && MicroLogin.WindowState != FormWindowState.Minimized)
                //    Win.Active(MicroLogin.Instance.Handle);
            }
            if (e.Button == MouseButtons.Middle)
                Dispose();
        }

        private void ActiveAuto()
        {
            Win.Active(Handle);
            Activate();
            Refresh();
        }

        public static int Bool2Int(bool value)
        {
            if (value == true)
                return 1;
            return 0;
        }

        private void menuPickItem_Click(object sender, EventArgs e)
        {
            Global.IsPickItem = menuPickItem.Checked = !menuPickItem.Checked;
        }

        private void menuItemFillter_Click(object sender, EventArgs e)
        {
            Global.ItemFillter = menuItemFillter.Checked = !menuItemFillter.Checked;
        }

        private void nudRadius_ValueChanged(object sender, EventArgs e)
        {
            if (IsHoldForDownSettingEx)
                return;
            if (CurGame.TLBB.IsNgoaiCong)
                Global.NgoaiRadius = (int)nudRadius.Value;
            else
                Global.NoiRadius = (int)nudRadius.Value;
        }


        private void nudHP_ValueChanged(object sender, EventArgs e)
        {
            Global.BuffHPPercent = (int)nudHP.Value;
        }

        private void nudMP_ValueChanged(object sender, EventArgs e)
        {
            Global.BuffMPPercent = (int)nudMP.Value;
        }

        private void nudNM_ValueChanged(object sender, EventArgs e)
        {
            Global.BuffNMPercent = (int)nudNM.Value;
        }


        private void listViewMain_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            var game = e.Item.Tag as Game;
            game.IsAuto = e.Item.Checked;
            game.SaveSetting();
            //if (!game.IsAuto)
            //{
            //    game.SetNull();
            //    DownSetting();
            //}
        }

        //bool isAdd = false;

        public static int from = 0;
        public static int to = 999999;
        List<Game> GamesToMove = new List<Game>();




        public static bool IsHold { get; set; }

        private void tmrMonitor_Tick(object sender, EventArgs e)
        {
            Game.FastTasks = Game.FastTasks.Where(kvp => !kvp.Value.IsDisposed).ToDictionary(t => t.Key, t => t.Value);

            btnCanQuetThuCong.Text = "Càn Quét [##:##]";
            var leader = SelectedOnlineLeaders.FirstOrDefault();
            if (leader != null)
            {
                if (Game.FastTasks.ContainsKey(leader.TLBB.Name))
                {
                    var FastTask = Game.FastTasks[leader.TLBB.Name];
                    btnCanQuetThuCong.Text = "Càn Quét [" + FastTask.TotalSec / 60 + ":" + FastTask.TotalSec % 60 + "]";
                }
            }


            try
            {
                IsHold = false;
                IsHold = TDT.IsPressed(VirtualKeyStates.VK_LMENU) || TDT.IsPressed(VirtualKeyStates.VK_RMENU);
                //if (webBrowser1.Visible)
                //{
                //    if (Global.RemoveAd)
                //    {
                //        webBrowser1.Visible = false;
                //        linkLabel3.Visible = false;
                //        linkLabel4.Visible = false;
                //    }
                //}
            }
            catch { }
        }

        public void MonitorInfo()
        {
            Invoke(new Action(() =>
            {
                try
                {
                    if (Alan.Instance.Controls.Count > 0 && !Alan.Instance.Visible)
                    {
                        Win.ShowInactiveTopmost(Alan.Instance);
                        Alan.Instance.Init();
                    }
                    if (Alan.Instance.Controls.Count == 0 && Alan.Instance.Visible)
                    {
                        Alan.Instance.Hide();
                    }

                    //listViewLogin.Refresh();
                    SetInfo();

                }
                catch { }
            }));

        }

        //public static int GameCount = 0;

        public static Dictionary<string, Color> RandomColorTeam = new Dictionary<string, Color>()
        {
            { "FFFFFFFF",Color.Red },
            { "FFFFFFFFFFFFFFFF",Color.Red },
        };

        private static readonly Random rand = new Random();

        private void SetInfo()
        {
            //if (IsMove)
            //{
            //    IsMove = false;
            //    return;
            //}
            AntiCaptcaManager.Pop();
            Lv.Columns[0].Text = "Name [" + Lv.Items.Count + "]";

            foreach (var game in AllGame)
            {
                if (game.TLBB == null || game.Item == null)
                    continue;
                var item = game.Item;

                if (!game.TLBB.IsTextCaptcha)
                {
                    game.captcha = null;
                }

                if (game.TLBB.IsTextCaptcha && !game.TLBB.IsLogon && game.TLBB.IsSelectRole)
                {
                    if (game.AntiCaptcha == null || game.AntiCaptcha.IsDisposed)
                    {
                        this.Invoke(() =>
                        {
                            game.AntiCaptcha = new AntiCaptcha(game);
                            game.AntiCaptcha.Dock = DockStyle.Fill;
                            //AntiCaptcaManager.CurAntiCaptcha = null;
                            //AntiCaptcaManager.CurAntiCaptcha = game.AntiCaptcha;                       
                            AntiCaptcaManager.Instance.PanelCaptcha.Controls.Add(game.AntiCaptcha);
                            //AntiCaptcaManager.instance.PanelCaptcha.Controls.SetChildIndex(game.AntiCaptcha, 0);
                            AntiCaptcaManager.Instance.Show();
                        });

                    }
                }
                if (Win.IsShow(game.Handle))
                {
                    item.SubItems[2].ForeColor = SystemColors.WindowText;
                    game.HideTime = Stopwatch.StartNew();
                }
                else
                {
                    game.ShowTime = Stopwatch.StartNew();
                    item.SubItems[2].ForeColor = Color.Red;
                }               

                if (RandomColorTeam.ContainsKey(game.TLBB.KeyId))
                {
                    item.SubItems[11].BackColor = RandomColorTeam[game.TLBB.KeyId];
                }
                else
                {
                    Random randomGen = new Random();
                    KnownColor[] names = (KnownColor[])Enum.GetValues(typeof(KnownColor));
                    KnownColor randomColorName = names[randomGen.Next(names.Length)];
                    Color randomColor = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
                    foreach (KeyValuePair<string, Color> kv in RandomColorTeam)
                    {
                        if (kv.Value == randomColor || randomColor == Color.Black || randomColor == Color.White)
                            return;
                    }
                    RandomColorTeam.Add(game.TLBB.KeyId, randomColor);
                }

                

                if (game.TongDoHong > 0)
                {
                    item.SubItems[7].BackColor = Color.Orange;
                }
                else
                {
                    item.SubItems[7].BackColor = SystemColors.Window;
                }

                item.SubItems[0].Text = game.TLBB.Name;
                item.SubItems[1].Text = game.TLBB.Lvl.ToString();
                item.SubItems[2].Text = game.TLBB.MenpaiName.Replace(" ", "");
                item.SubItems[3].Text = game.Status;
                item.SubItems[4].Text = game.TLBB.HPPercent + "%";
                item.SubItems[5].Text = game.TLBB.MPPercent + "%";
                item.SubItems[6].Text = game.TLBB.PetHPPercent + "%";
                item.SubItems[7].Text = (string.Format("{0:00}", game.TLBB.OnlineTime / 60)) + ":" + string.Format("{0:00}", game.TLBB.OnlineTime % 60);
                item.SubItems[8].Text = string.Format("{0:0.00}%", game.TLBB.ExpPercent);
                item.SubItems[9].Text = string.Format("{0:0.00}M", game.ExpSpeed / 1000000);
                item.SubItems[10].Text = game.TLBB.MapName + " [" + (int)game.CharX + "." + (int)game.CharY + "]";
      


                if (game.Leader != null)
                {
                    item.SubItems[11].Text = game.Leader.TLBB.Name;
                }
                else
                {
                    item.SubItems[11].Text = game.TLBB.KeyId;
                }

                item.SubItems[12].Text = game.TLBB.NangDong.ToString();

                item.SubItems[13].Text = ((int)(game.TLBB.Gold / 10000)).ToString();



                //item.SubItems[13].Text = game.GetRoundTuTuyet(game.RoundX, game.RoundY).ToString();

                if (game.TLBB.ON_SCENE_TRANSING)
                {
                    item.SubItems[0].Text = "ChuyểnCảnh";
                    item.SubItems[0].BackColor = Color.Silver;
                }
                else
                {
                    if (!game.TLBB.Online)
                        item.SubItems[0].BackColor = Color.Silver;
                    else if (game.TLBB.IsPk)
                        item.SubItems[0].BackColor = Color.Red;
                    else
                        item.SubItems[0].BackColor = SystemColors.Window;
                }

                if (game.TLBB.HPPercent <= 30)
                    item.SubItems[4].BackColor = Color.Red;
                else
                    item.SubItems[4].BackColor = SystemColors.Window;
                if (game.TLBB.PetHPPercent == 0)
                    item.SubItems[6].BackColor = Color.Red;
                else
                    item.SubItems[6].BackColor = SystemColors.Window;
                if (game.IsX2)
                    item.SubItems[8].BackColor = Color.LightBlue;
                else
                    item.SubItems[8].BackColor = SystemColors.Window;
                if (game.TLBB.IsLeader)
                    item.SubItems[0].ForeColor = Color.Blue;
                else
                    item.SubItems[0].ForeColor = SystemColors.WindowText;


            }


            if (CurGame == null || !DicGame.ContainsValue(CurGame))
            {
                if (Lv.Items.Count > 0)
                {
                    CurGame = (Game)Lv.Items[0].Tag;
                    Lv.Items[0].Selected = true;
                }
            }
            else
            {
                //lblInfo.Text = CurGame.TLBB.Name + " - " + CurGame.TLBB.Lvl + " - " + CurGame.TLBB.MenpaiName;
                //lblExp.Text = CurGame.ExpInfo;
                //lbHoaSpeed.Text = CurGame.HoaSpeed;
                lbExpSpeed.Text = CurGame.ExpSpeedPerH;
                lblTimeInfo.Text = CurGame.ExpTime;
                lbHoaSpeed.Text = CurGame.HoaKTTSpeed;
                if (CurGame.RadiusX > 0)
                    lblRadius.Text = "[" + (int)CurGame.RadiusX + "," + (int)CurGame.RadiusY + "] (" + CurGame.Radius + ")";
                else
                    lblRadius.Text = "[0,0] (0)";
                lbAcBa.Text = TLBB.GetMenpaiName(CurGame.AcBa);
                if (lbAcBa.Text.Contains("Không"))
                    lbAcBa.Text = "LGBT";
                gameInfo.HPPercent = (float)CurGame.TLBB.HP / (CurGame.TLBB.MaxHP + 1);
                gameInfo.MPPercent = (float)CurGame.TLBB.MP / (CurGame.TLBB.MaxMP + 1);
                gameInfo.PetPercent = (float)CurGame.TLBB.PetHP / (CurGame.TLBB.PetMaxHP + 1);
                gameInfo.ExpPercent = (int)(CurGame.TLBB.Exp * 100 / CurGame.TLBB.MaxExp);
                gameInfo.MapName = CurGame.TLBB.MapName + " [" + CurGame.RoundX + " " + CurGame.RoundY + "]";
            }
            tabBottom.TabPages[0].Text = CurGame.TLBB.Name;
        }



        private void listViewMain_MouseUp(object sender, MouseEventArgs e)
        {
            //if (e.Button != MouseButtons.Left)
            //    return;
            //ListViewHitTestInfo hit = listViewMain.HitTest(e.X, e.Y);
            //if (hit.SubItem == null || hit.SubItem.Text != " ")
            //    return;
            //if (hit.SubItem.BackColor != Color.LightGray)
            //    hit.SubItem.BackColor = Color.LightGray;
            //else
            //    hit.SubItem.BackColor = SystemColors.Window;
            //var game = hit.Item.Tag as Game;
            //if (hit.SubItem == hit.Item.SubItems[9])
            //    game.IsAttack = hit.SubItem.BackColor == Color.LightGray;
            //if (hit.SubItem == hit.Item.SubItems[10])
            //    game.IsLure = hit.SubItem.BackColor == Color.LightGray;
            //if (hit.SubItem == hit.Item.SubItems[11])
            //    game.F[0] = hit.SubItem.BackColor == Color.LightGray;
            //if (hit.SubItem == hit.Item.SubItems[12])
            //    game.F[1] = hit.SubItem.BackColor == Color.LightGray;
            //if (hit.SubItem == hit.Item.SubItems[13])
            //    game.F[2] = hit.SubItem.BackColor == Color.LightGray;
            //if (hit.SubItem == hit.Item.SubItems[14])
            //    game.F[3] = hit.SubItem.BackColor == Color.LightGray;
            //if (hit.SubItem == hit.Item.SubItems[15])
            //    game.F[4] = hit.SubItem.BackColor == Color.LightGray;
            //if (hit.SubItem == hit.Item.SubItems[16])
            //    game.F[5] = hit.SubItem.BackColor == Color.LightGray;
            //if (hit.SubItem == hit.Item.SubItems[17])
            //    game.F[6] = hit.SubItem.BackColor == Color.LightGray;
            //if (hit.SubItem == hit.Item.SubItems[18])
            //    game.F[7] = hit.SubItem.BackColor == Color.LightGray;
            //if (hit.SubItem == hit.Item.SubItems[19])
            //    game.F[8] = hit.SubItem.BackColor == Color.LightGray;
            //if (hit.SubItem == hit.Item.SubItems[20])
            //    game.F[9] = hit.SubItem.BackColor == Color.LightGray;
            //if (hit.SubItem == hit.Item.SubItems[21])
            //    game.F[10] = hit.SubItem.BackColor == Color.LightGray;
            //if (hit.SubItem == hit.Item.SubItems[22])
            //    game.F[11] = hit.SubItem.BackColor == Color.LightGray;
            //if (hit.SubItem == hit.Item.SubItems[23])
            //    game.IsPet = hit.SubItem.BackColor == Color.LightGray;
            //if (hit.SubItem == hit.Item.SubItems[24])
            //    game.IsHP = hit.SubItem.BackColor == Color.LightGray;
            //if (hit.SubItem == hit.Item.SubItems[25])
            //    game.IsMP = hit.SubItem.BackColor == Color.LightGray;
            //if (hit.SubItem == hit.Item.SubItems[26])
            //    game.IsRadius = hit.SubItem.BackColor == Color.LightGray;
            //if (hit.SubItem == hit.Item.SubItems[27])
            //    game.IsNM = hit.SubItem.BackColor == Color.LightGray;
            //game.SaveSetting();
            //DownSetting();
        }

        private void listViewSkill_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (IsHoldForDownSettingEx)
                return;
            var listView = sender as ListView;
            listView.ListViewItemSorter = new TaskListComparer();
            listView.Sort();
            ((Skill)e.Item.Tag).Use = e.Item.Checked;
        }

        private void btnNM_Click(object sender, EventArgs e)
        {
            ShowBuff();
        }

      

        private void menuListView_Opening(object sender, CancelEventArgs e)
        {
            menuRefreshAuto.Text = "Refresh [" + SelectedGames.Count() + "] [Ctrl + W]";
            if (CurGame != null)
                muChucPhuc.Checked = CurGame.Missions.Contains(MissionsType.ChucPhucMaoBut);
            menuDebug.Visible = Global.IsAdminEx;
            try
            {
                xinVàoNhómToolStripMenuItem.DropDownItems.Clear();

                foreach (var game in AllOnelineGame)
                {
                    if (game.TLBB.IsLeader)
                    {
                        ToolStripMenuItem i = new ToolStripMenuItem(game.TLBB.Name);
                        i.Click += (ss, ee) =>
                        {
                            SelectedOnlinedGames.ForEach(g => g.AskTeam(game.TLBB.VISCIIName));

                        };
                        xinVàoNhómToolStripMenuItem.DropDownItems.Add(i);
                    }
                }

                menuAutoCreateTeam.Checked = CalendarEx.IsAutoCreaTeam;




                if (Leaders.Count == 0)
                {

                }
                else
                {
                    string leaderName = "";
                    if (Leaders.Count == 1)
                        leaderName = Leaders[0].TLBB.Name;
                    else
                        leaderName = "Multi";

                }
                menuTrieuTap.DropDownItems.Clear();
                List<ToolStripMenuItem> items = new List<ToolStripMenuItem>();
                Teams.Clear();
                foreach (ListViewItem item in Lv.SelectedItems)
                {
                    var game = item.Tag as Game;
                    if (game.TLBB.Name != "ĐăngNhập")
                    {
                        ToolStripMenuItem menu = new ToolStripMenuItem();
                        menu.Text = game.TLBB.Name;
                        menu.Tag = game;
                        menu.Click += new EventHandler(TrieuTap);
                        items.Add(menu);
                        Teams.Add(game);
                    }
                }
                menuCreateTeam.Text = "Tạo Đội Ngũ [" + Teams.Count + "]";
                menuSetNhom.Text = "Set Nhóm [" + SelectedOnlinedGames.Count() + "]";
                if (User.IsEnglish)
                {
                    menuCreateTeam.Text = "Create Team [" + Teams.Count + "]";
                    muGiaiTan.Text = "Destroy All Team";
                    tổĐộiToolStripMenuItem.Text = "Team";
                    toolStripMenuItem38.Text = "Feature";
                    diChuyểnToolStripMenuItem.Text = "Move";
                    thêmTọaĐộToolStripMenuItem.Text = "Add Point";
                    menuSetNhom.Text = "Set Team [" + SelectedOnlinedGames.Count() + "]";
                    muQuanLyNhom.Text = "Manger Team";
                    menuAutoCreateTeam.Text = "Auto Create Team Via Set";
                }

                if (items.Count > 1)
                {
                    ToolStripMenuItem[] menus = new ToolStripMenuItem[items.Count];
                    for (int i = 0; i < menus.Length; i++)
                    {
                        menus[i] = items[i];
                    }
                    menuTrieuTap.DropDownItems.AddRange(menus);
                }
            }
            catch
            {
            }

            try
            {
                var coordinates = Lv.PointToClient(Cursor.Position);
                if (coordinates.Y <= 22)
                {
                    foreach (ToolStripItem m in menuListView.Items)
                    {
                        if (m.Tag == null)
                            m.Visible = false;
                        else if (m.Tag.GetType() == typeof(ColumnHeader))
                        {
                            var col = m.Tag as ColumnHeader;
                            if (hidecol.Contains(col.Index))
                            {
                                ((ToolStripMenuItem)m).Checked = false;
                            }
                            else
                            {
                                ((ToolStripMenuItem)m).Checked = true;
                            }
                            m.Visible = true;
                        }
                        else
                            m.Visible = false;
                    }
                }
                else
                {
                    foreach (ToolStripItem m in menuListView.Items)
                    {
                        if (m.Tag == null)
                            m.Visible = true;
                        else if (m.Tag.GetType() == typeof(ColumnHeader))
                            m.Visible = false;
                        else
                            m.Visible = true;
                    }
                }
            }
            catch { }
        }
        public static List<Game> Teams = new List<Game>();

        private void btnBuyDay_Click(object sender, EventArgs e)
        {
            CreateControl(new BuyFunc());
            //if (BuyDate == null || !BuyDate.Visible)
            //{
            //    BuyDate = new Buy();
            //    BuyDate.Show(this);
            //    BuyDate.Disposed += new EventHandler(BuyDate_Disposed);
            //}
        }



        void UpdateUserInfo_Completed(object sender, EventArgs e)
        {
            LoadUserInfo();
        }

        private void tmrReload_Tick(object sender, EventArgs e)
        {
            Reload();
            LoadUserInfo();
        }

        public static int FalseTime
        {
            get;
            set;
        }

        private void Reload()
        {
            Poster posterReload = new Poster();
            posterReload.Url = URL.HomePage + "microauto/user.php";
            posterReload.Control = this;
            posterReload.Data = "cmd=reload&email=" + HttpUtility.UrlEncode(User.Email) + "&pass=" + HttpUtility.UrlEncode(User.Pass) + "&vip=" + Global.IsVIP;
            posterReload.Completed += new EventHandler(posterReload_Completed);
            posterReload.Post();
        }

        void posterReload_Completed(object sender, EventArgs e)
        {
            if (!TDT.FileHostValid)
            {
                Dispose();
                return;
            }
            var poster = sender as Poster;
            if (poster.IsError)
            {
                if (!Global.IsAdminEx)
                {
                    if (FalseTime++ > 1000)
                    {
                        this.Dispose();
                        return;
                    }
                }
                Reload();
                return;
            }
            if (poster.Response.ToLower().Contains("free"))
            {
                Global.IsFull = 0;
                Text = Global.Version + " - " + "Free";
                while (Lv.Items.Count > 2)
                {
                    var game = Lv.Items[0].Tag as Game;
                    DicGame.Remove(game.ProcessId);
                    game.UnHookRecv();
                    game.ThreadAuto.Abort();
                    Lv.Items[0].Remove();
                }
                return;
            }
            if (poster.Response.ToLower().Contains("lite"))
            {
                Text = Global.Version + " - " + "Lite";
                Global.IsFull = 3;
            }
            if (poster.Response.ToLower().Contains("medi"))
            {
                Text = Global.Version + " - " + "Medi";
                Global.IsFull = 7;
            }
            if (poster.Response.ToLower().Contains("pro"))
            {
                Text = Global.Version + " - " + "Pro";
                Global.IsFull = 1;
            }
            if (poster.Response.ToLower().Contains("dis"))
            {
                if (Global.IsAdminEx)
                {
                    return;
                }
                menuExit_Click(null, null);
                return;
            }
            FalseTime = 0;
            Poster.ErrorCount = 0;
        }
 









       

        private void btnOpenPass2_Click(object sender, EventArgs e)
        {
            if (CurGame != null)
            {
                CurGame.Pass2 = txtPass2.Text;
                CurGame.UnlockPass2();
            }
        }


        private void MoiDoi(Game foreGame)
        {
            if (!foreGame.TLBB.Online)
                return;
            if (!foreGame.TLBB.IsLeader)
                foreGame.LUA.PlayerCreateTeamSelf();
            foreach (ListViewItem item in Lv.Items)
            {
                Game game = (Game)item.Tag;
                if (game == foreGame)
                    continue;
                if (game.TLBB.Online && game.Objects.Self != null && game.Objects.Self.PartyId == 0xFFFFFFFF)
                {
                    game.AskTeam(foreGame.TLBB.VISCIIName);
                }
                if (game.TLBB.Online && game.Address.GameType == 1)
                {
                    game.AskRaid(foreGame.TLBB.VISCIIName);
                }
            }
        }

        private void btnAutoAccept_Click(object sender, EventArgs e)
        {
            ShowBuff();
        }







        private void TrieuTapNhom()
        {
            foreach (var game in AllOnelineGame)
            {
                if (game.TLBB.IsLeader)
                {
                    foreach (var g in game.Party)
                    {
                        if (g == game)
                            continue;
                        if (g.TLBB.Online)
                        {
                            g.Move((int)game.CharX, (int)game.CharY, (int)game.TLBB.MapId);
                        }
                    }
                }
            }
        }

        private void listViewMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                menuExitGame_Click(null, null);
            if (e.KeyCode == Keys.PageDown)
            {
                foreach (ListViewItem item in Lv.SelectedItems)
                {
                    Game game = (Game)item.Tag;
                    game.Hide();
                }
            }
            if (e.KeyCode == Keys.Enter)
            {
                if (CurGame != null)
                    CurGame.Active();
            }
            if (e.KeyCode == Keys.A)
            {
                if (e.Control == true)
                {
                    Lv.SelectAllItems();
                }
            }
            if (e.KeyCode.ToString() == "BACK")
            {
                txtMainSearch.Focus();
                SendKeys.Send("{BACK}");
            }


            //if (e.KeyCode.ToString() == "BACK")
            //{
            //    txtMainSearch.Focus();
            //    SendKeys.Send("{BACKSPACE}");
            //}
            string ch = KeyCodeToUnicode(e.KeyCode);
            Regex regex = new Regex("[a-zA-Z_0-9]{1}");


            if (regex.IsMatch(ch))
            {

                txtMainSearch.Text = txtMainSearch.Text + ch;
                txtMainSearch.Focus();
                txtMainSearch.Select(txtMainSearch.Text.Length, 0);
            }

        }

        public string KeyCodeToUnicode(Keys key)
        {
            byte[] keyboardState = new byte[255];
            bool keyboardStateStatus = GetKeyboardState(keyboardState);

            if (!keyboardStateStatus)
            {
                return "";
            }

            uint virtualKeyCode = (uint)key;
            uint scanCode = MapVirtualKey(virtualKeyCode, 0);
            IntPtr inputLocaleIdentifier = GetKeyboardLayout(0);

            StringBuilder result = new StringBuilder();
            ToUnicodeEx(virtualKeyCode, scanCode, keyboardState, result, (int)5, (uint)0, inputLocaleIdentifier);

            return result.ToString();
        }

        [DllImport("user32.dll")]
        static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport("user32.dll")]
        static extern uint MapVirtualKey(uint uCode, uint uMapType);

        [DllImport("user32.dll")]
        static extern IntPtr GetKeyboardLayout(uint idThread);

        [DllImport("user32.dll")]
        static extern int ToUnicodeEx(uint wVirtKey, uint wScanCode, byte[] lpKeyState, [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pwszBuff, int cchBuff, uint wFlags, IntPtr dwhkl);





        private void ShowBuff()
        {
            CreateControl(new Buff());
        }

        //static NearBy nearBy;







        private void txtRao_TextChanged(object sender, EventArgs e)
        {
            if (CurGame != null)
            {
                CurGame.RaoTxt = txtRao.Text;
                CurGame.SaveSetting();
            }
        }



        private void cboTrongTrot_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CurGame != null)
                CurGame.TrongTrotIndex = cboTrongTrot.SelectedIndex;
        }

        private void cboThuHoach_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CurGame != null)
                CurGame.ThuHoachIndex = cboThuHoach.SelectedIndex;
        }

        private void menuQuayVeDaiLy_Click(object sender, EventArgs e)
        {
            foreach (Game game in SelectedGames)
                game.UseSkill(22);
        }

        private void menuXuatPet_Click(object sender, EventArgs e)
        {
            foreach (Game game in SelectedGames)
                game.DoAction("PetSkill2_1");
        }

        private void menuThuPet_Click(object sender, EventArgs e)
        {
            foreach (Game game in SelectedGames)
                game.DoAction("PetSkill2_2");
        }







        public IEnumerable<Game> SelectedGames => Lv.SelectedItems.Cast<ListViewItem>().Select(i => i.Tag as Game);
        private IEnumerable<Game> SelectedOnlineLeaders => SelectedOnlinedGames.Where(o => o.TLBB.IsLeader);

        public IEnumerable<Game> SelectedOnlinedGames => SelectedGames.Where(o => o.TLBB.Online);

        public IEnumerable<Game> AllGame => DicGame.Select(kvp => kvp.Value);

        public IEnumerable<Game> AllOnelineGame => AllGame.Where(o => o.TLBB.Online);


        private void menuResetTime_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in Lv.SelectedItems)
            {
                Game game = (Game)item.Tag;
                if (game.TLBB.IsLeader)
                {
                    game.IsLostLeader = true;
                }
                if (game.TLBB.PlayerState == 10)
                    game.IsOpenShop = true;
                game.IsOpenPass2 = false;
                if (game.TLBB.MapId == MAP.PhungHoangCoThanh)
                {
                    Task.Run(() =>
                    {
                        Thread.Sleep(30000);
                        game.ReConnect();
                    });
                }
                else
                {
                    game.ReConnect();
                }
            }

        }



        private Trader Trader;

        private void menuTrader_Click(object sender, EventArgs e)
        {
            if (CurGame == null)
                return;
            if (Trader == null || !Trader.Visible)
            {
                Trader = new Trader(CurGame);
                Trader.Show(this);
            }
        }







        private void cboHP_SelectedIndexChanged(object sender, EventArgs e)
        {
            Global.HPKey = TDT.String2Key(cboHP.Text);
        }

        private void txtLog_TextChanged(object sender, EventArgs e)
        {
            var textbox = sender as TextBox;
            if (textbox.Lines.Length > 1000)
            {
                string[] temp = new string[1000];
                for (int i = 0; i < 1000; i++)
                {
                    temp[i] = textbox.Lines[i];
                }
                textbox.Lines = temp;
            }
        }

        Pk Pk = new Pk();




        public static void PushMsg(object log)
        {
            if (Instance.InvokeRequired)
            {
                Instance.Invoke(new LogBack(PushMsg), log);
            }
            else
            {
                var msg = log.ToString();
                if (msg.Split('|').Length > 1)
                {
                    //AppendText(Regex.Replace(msg.Split('|')[0], "@.*", "") + ": ", Color.Red, new Font("Times New Roman", 10));
                    //AppendText(msg.Substring(msg.Split('|')[0].Length + 1) + "\r\n", Color.DarkGreen, new Font("Times New Roman", 10));
                }
                else
                {
                    //AppendText(msg + "\r\n", Color.DarkGreen, new Font("Times New Roman", 10));
                }
                if (!MuteChatBox)
                {
                    SoundPlayer player = new System.Media.SoundPlayer();
                    player.Stream = Properties.Resources.chat;
                    player.Play();
                }
            }
        }




        //Beli beli;

        private void menuBeli_Click(object sender, EventArgs e)
        {
            //if (beli == null || !beli.Visible)
            //{
            //    beli = new Beli();
            //    beli.Show(this);
            //    beli.Disposed += new EventHandler(beli_Disposed);
            //}
            //else
            //{
            //    Win.Active(beli);
            //}
        }




        public static bool Dis = false;



        private void picRefresh_MouseClick(object sender, MouseEventArgs e)
        {
            LoadUserInfo();
        }

        private void picRefresh_Click(object sender, EventArgs e)
        {
            //LoadUserInfo();
        }

        bool IsMove
        {
            get;
            set;
        }

        private void Main_Move(object sender, EventArgs e)
        {
            IsMove = true;
        }

        private void menuPause_Click(object sender, EventArgs e)
        {
            //tmrMonitor.Enabled = menuPause.Checked = !menuPause.Checked;
        }

        //EditorForm editor;
        private void menuNote_Click(object sender, EventArgs e)
        {
            //if(editor == null || editor.IsDisposed)
            //    editor = new EditorForm();
            //Win.Active(editor);
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            SettingOld.Write("User", "Pass", "");
            menuExit_Click(null, null);
        }

        private void picBeri_MouseClick(object sender, MouseEventArgs e)
        {
            //if (beli == null || beli.IsDisposed)
            //{
            //    beli = new Beli();
            //}
            //beli.Show();
            //Win.Active(beli);
        }


        private void picBeri_Click(object sender, EventArgs e)
        {
            LoadUserInfo();
        }

        private void tmrEnd_Tick(object sender, EventArgs e)
        {
            if (Global.TimeLive-- == 1)
                this.Dispose();
        }

        List<Game> Leaders
        {
            get
            {
                List<Game> leaders = new List<Game>();
                if (Lv.SelectedItems.Count == 0)
                {
                    foreach (ListViewItem item in Lv.Items)
                    {
                        var game = item.Tag as Game;
                        if (game.Leader != null)
                        {
                            if (!leaders.Contains(game.Leader))
                                leaders.Add(game.Leader);
                        }
                    }
                }
                else
                {
                    foreach (ListViewItem item in Lv.SelectedItems)
                    {
                        var game = item.Tag as Game;
                        if (game.Leader != null)
                        {
                            if (!leaders.Contains(game.Leader))
                                leaders.Add(game.Leader);
                        }
                    }
                }
                return leaders;
            }
        }

        //Game Leader {
        //    get {
        //        if (CurGame == null)
        //            return null;
        //        if (CurGame.Leader == null)
        //            return null;
        //        return CurGame.Leader;
        //    }
        //}







        private void menuLogin_Click(object sender, EventArgs e)
        {
            //foreach (ListViewItem item in listLogin.SelectedItems)
            //{
            //    Account account = (Account)item.Tag;
            //    if (account.game == null)
            //    {
            //        account.Status = "Đang chờ...";
            //    }
            //}
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //foreach (ListViewItem item in listLogin.SelectedItems)
            //{
            //    Account account = (Account)item.Tag;
            //    if (account.game == null)
            //    {
            //        account.Status = "Đang chờ làm BHD...";
            //        account.IsBHD = true;
            //    }
            //    else
            //    {
            //        account.game.IsBachHoaDuyen = true;
            //    }
            //}
        }

        private void menuDeleteStatus_Click(object sender, EventArgs e)
        {
            //foreach (ListViewItem item in listLogin.SelectedItems)
            //{
            //    Account account = (Account)item.Tag;
            //    if (account.game == null)
            //    {
            //        account.Status = "";
            //    }
            //}
        }

        private void menuAcc1_Click(object sender, EventArgs e)
        {

        }

        private void listLogin_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void listLogin_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    foreach (ListViewItem item in listLogin.SelectedItems)
            //    {
            //        Account account = (Account)item.Tag;
            //        if (account.game == null)
            //        {
            //            account.Status = "Đang chờ...";
            //        }
            //    }
            //}
        }

        private void listLogin_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //Account account = (Account)listLogin.SelectedItems[0].Tag;
            //if (account.game == null)
            //{
            //    account.Status = "Đang chờ...";
            //    account.IsForceOpen = true;
            //}
            //else
            //{
            //    account.game.Active();
            //}
        }

        private void thoátDeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //foreach (ListViewItem item in listLogin.SelectedItems)
            //{
            //    Account acc = (Account)item.Tag;
            //    if (acc.game != null)
            //        acc.game.Exit();
            //}
        }



        public static List<ListViewItem> listS = new List<ListViewItem>();

        public static List<ListViewItem> listSS = new List<ListViewItem>();

    




        private void menuIcon_Opening(object sender, CancelEventArgs e)
        {
            //if (MicroLogin.IsDungHop)
            //{
            //    menuPhanLy.Visible = true;
            //    microLoginToolStripMenuItem.Visible = false;
            //}
            //else
            //{
            //    menuPhanLy.Visible = false;
            //    microLoginToolStripMenuItem.Visible = true;
            //}
        }



        private void LoginEnter()
        {
            foreach (var account in SelectedAccounts)
            {
                if (account.game == null)
                {
                    account.Status = "Đang chờ...";
                }
                else
                {
                    account.game.Active();
                }
            }
        }




        public IEnumerable<ListViewItem> SelectedItems
        {
            get
            {
                yield return new ListViewItem();
                //foreach (int index in listViewLogin.SelectedIndices)
                //{
                //    yield return listViewLogin.Items[index];
                //}
            }
        }

        public IEnumerable<AccountEx> SelectedAccounts
        {
            get
            {
                yield return null;
                //foreach (int index in listViewLogin.SelectedIndices)
                //{
                //    yield return listViewLogin.Items[index].Tag as AccountEx;
                //}
            }
        }




        private void cboXuatPet_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!IsHoldForDownSettingEx)
            {
                if (cboXuatPet.SelectedIndex == -1)
                    return;
                if (CurGame != null)
                {
                    try
                    {
                        CurGame.PetId = (cboXuatPet.SelectedItem as ComboboxItem).Value.ToString("X8");
                    }
                    catch
                    {

                    }
                }
            }
        }

        private void btnXuatPet_Click(object sender, EventArgs e)
        {
            if (CurGame != null)
            {
                CurGame.DoAction("PetSkill2_2");
            }
        }

        private void chkXuatPet_CheckedChanged(object sender, EventArgs e)
        {
            if (!IsHoldForDownSettingEx)
            {
                if (CurGame != null)
                {
                    CurGame.IsXuatPet = checkXuatPet.Checked;
                    cboXuatPet.Items.Clear();
                    ComboboxItem it = new ComboboxItem();
                    it.Text = "Không Xuất";
                    it.Value = 0;
                    cboXuatPet.Items.Add(it);
                    int index = -1;
                    int cnt = 0;
                    foreach (KeyValuePair<uint, string> kvp in CurGame.TLBB.DicPet)
                    {
                        cnt++;
                        ComboboxItem item = new ComboboxItem();
                        item.Text = kvp.Value;
                        item.Value = (int)kvp.Key;
                        if (kvp.Key.ToString("X8") == CurGame.PetId)
                        {
                            index = cnt;
                        }
                        cboXuatPet.Items.Add(item);
                    }
                    if (index != -1)
                        cboXuatPet.SelectedIndex = index;
                }
            }
        }






        private void btnTest_Click(object sender, EventArgs e)
        {
            go = true;
            this.Invalidate();
        }




        private void menuTrimMem_Click(object sender, EventArgs e)
        {
            foreach (KeyValuePair<int, Game> kvp in DicGame)
            {
                kvp.Value.TrimProc();
            }
        }

        public static int DiemDanhIndex = -1;
        public static Stopwatch LastDiemDanh = Stopwatch.StartNew();

        public void DiemDanh()
        {
            foreach (Game game in AllOnelineGame)
            {
                game.IsDiemDanh = false;
                game.IsDiemDanhEx = false;
            }
        }



        public static bool TrimRam = false;








        private void mởGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenGames();
        }

        bool isShowDialog = false;

        private void OpenGames()
        {
            if (PATH.Game == "" || !File.Exists(PATH.Game))
            {
                if (!isShowDialog)
                {
                    isShowDialog = true;
                    MessageBox.Show(this, "Bạn cần phải chọn đường dẫn game\r\nFile Game.exe nằm trong thư mục Bin của game", "MicroAuto", MessageBoxButtons.OK);
                    OpenFileDialog openFile = new OpenFileDialog();
                    openFile.Filter = "Game.exe |Game.exe";
                    if (openFile.ShowDialog(this) == DialogResult.OK)
                    {
                        PATH.Game = openFile.FileName;
                        ProcessStartInfo startInfo = new ProcessStartInfo();
                        startInfo.FileName = PATH.Game;
                        startInfo.Arguments = "-fl";
                        startInfo.WorkingDirectory = Path.GetDirectoryName(PATH.Game);
                        Process.Start(startInfo);
                    }
                    isShowDialog = false;
                }
            }
            else
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = PATH.Game;
                startInfo.Arguments = "-fl";
                startInfo.WorkingDirectory = Path.GetDirectoryName(PATH.Game);
                Process.Start(startInfo);
            }
        }

        //Process processGame = null;

        //void ThreadOpenGame()
        //{
        //    ProcessStartInfo startInfo = new ProcessStartInfo();
        //    startInfo.FileName = PATH.Game;
        //    startInfo.Arguments = "-fl";
        //    startInfo.WorkingDirectory = Path.GetDirectoryName(PATH.Game);
        //    var process = Process.Start(startInfo);
        //    while (true)
        //    {
        //        try
        //        {
        //            var time = process.StartTime;
        //            string str = Memory.ReadStringId(0x905884, process.Id);
        //            //Memory.Write1Byte(78, 0x00406A0B, process.Id);
        //            if (str.Length == 36)
        //            {
        //                string newStr = RandomHexString() + str.Substring(8, 28);
        //                Memory.WriteString(newStr, process.Id, 0x905884);
        //                break;
        //            }
        //            else
        //            {
        //                Main.PushLogEx(str);
        //            }
        //        }
        //        catch (Exception) { }
        //    }
        //}

        private static string RandomHexString()
        {
            // 64 character precision or 256-bits
            Random rdm = new Random();
            string hexValue = string.Empty;
            int num;

            for (int i = 0; i < 1; i++)
            {
                num = rdm.Next(0, int.MaxValue);
                hexValue += num.ToString("X8");
            }

            return hexValue;
        }







    







        private void cboNM_SelectedIndexChanged(object sender, EventArgs e)
        {
            Global.NMSkill = TDT.String2Key(cboNM.Text);
        }

        private void cboBaseSkill_SelectedIndexChanged(object sender, EventArgs e)
        {
            Global.BaseSkill = TDT.String2Key(cboBaseSkill.Text);
        }















        private void IsHyHuu_CheckedChanged(object sender, EventArgs e)
        {
            //Global.IsHyHuu = IsHyHuu.Checked;
        }

        private void tabPage6_Click(object sender, EventArgs e)
        {

        }

        private void chkHopall_CheckedChanged(object sender, EventArgs e)
        {
            //if (CurGame != null)
            //    CurGame.IsNhatHopall = chkHopall.Checked;
        }


        private void mnuAnDon_Click(object sender, EventArgs e)
        {
            foreach (Game game in SelectedGames)
                game.DoAction("FightSkillXinShou_1");
        }

        //private void chkTuVaoPhai_CheckedChanged(object sender, EventArgs e)
        //{
        //    Global.IsTuVaoPhai = chkTuVaoPhai.Checked;
        //}



        private void btnAdd_Click(object sender, EventArgs e)
        {
            CreateControl(new FrmLocdoOpt(CurGame));
        }



        private void làmQDưaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Global.IsVIP == 0)
            {
                MessageBox.Show(this, "Chức năng này cần kích hoạt VIP", "MicroAuto", MessageBoxButtons.OK);
                return;
            }
            foreach (ListViewItem item in SelectedItems)
            {
                var account = item.Tag as AccountEx;
                if (account.game == null)
                {
                    account.Status = "Đang chờ làm DUA...";
                    account.IsLoginDua = true;
                }
                else
                {
                    account.game.IsDuaHau = true;
                }
            }
        }



        private void tabPage10_Click(object sender, EventArgs e)
        {

        }





        public static int CreateTeamSate = STATE.None;

        private void menuCeateTeam_Click(object sender, EventArgs e)
        {
            tmrCreateTeam.Start();
            CreateTeamSate = STATE.None;
        }

        private void tmrCreateTeam_Tick(object sender, EventArgs e)
        {
            try
            {
                if (CreateTeamSate == STATE.None)
                {
                    foreach (Game game in Teams)
                        game.DoStringEx("Player:LeaveTeam();");
                    CreateTeamSate = STATE.DisposeTeam;
                }
                else if (CreateTeamSate == STATE.DisposeTeam)
                {
                    Teams[0].DoStringEx("Player:CreateTeamSelf();");
                    CreateTeamSate = STATE.CreateTeam;
                }
                else if (CreateTeamSate == STATE.CreateTeam)
                {
                    if (Teams.Count > 1)
                    {
                        for (int i = 1; i < Teams.Count; i++)
                        {
                            Teams[i].AskTeam(Teams[0].TLBB.VISCIIName);
                        }
                    }
                    tmrCreateTeam.Stop();
                    CreateTeamSate = STATE.None;
                }
            }
            catch
            {

            }
        }



        private void tmrSaveSettings_Tick(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                if (User.IsDisposed)
                {
                    try
                    {
                        AllOnelineGame.ToList().ForEach(g => g.SaveSetting());
                        SaveSetting();
                        Game.IniParser.Save("nier.ini");
                    }
                    catch { }
                    try
                    {
                        MicroLogin.SaveXML();
                    }
                    catch { }
                }
            });

            //IsSaveByTimer = true;
        }


        private void chkMuaNguyenLieu_CheckedChanged(object sender, EventArgs e)
        {
            Game.IsMuaNguyenLieu = chkMuaNguyenLieu.Checked;
        }

        private void chkGiamDinh_CheckedChanged(object sender, EventArgs e)
        {
            Game.IsGiamDinh = chkGiamDinh.Checked;
        }

        private void cboLoai_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CurGame != null)
            {
                CurGame.CheLoai = cboLoai.SelectedIndex;
                CurGame.CheTen = cboLoai.Text;
            }
        }

        private void cboCap_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CurGame != null)
                CurGame.CheCap = cboCap.SelectedIndex;
        }

        private void nudStar_ValueChanged(object sender, EventArgs e)
        {
            if (CurGame != null)
                CurGame.CheSao = (int)nudStar.Value;
        }

        private void nudLine_ValueChanged(object sender, EventArgs e)
        {
            if (CurGame != null)
                CurGame.CheDong = (int)nudLine.Value;
        }

        private void nudPoint_ValueChanged(object sender, EventArgs e)
        {
            if (CurGame != null)
                CurGame.CheDiem = (int)nudPoint.Value;
        }

        private void chkAutoDropCraft_CheckedChanged(object sender, EventArgs e)
        {
            Global.AutoDropCraft = chkAutoDropCraft.Checked;
        }

        private void rad69_CheckedChanged(object sender, EventArgs e)
        {
            //Game.Is69DO = rad69.Checked;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            //Game.Is69DO = rad69.Checked;
        }

        private void radNoi_CheckedChanged(object sender, EventArgs e)
        {
            if (CurGame != null)
                CurGame.CheNoiNgoai = 1;
        }

        private void radNgoai_CheckedChanged(object sender, EventArgs e)
        {
            if (CurGame != null)
                CurGame.CheNoiNgoai = 2;
        }

        private void radNoiNgoai_CheckedChanged(object sender, EventArgs e)
        {
            if (CurGame != null)
                CurGame.CheNoiNgoai = 3;
        }

        private void radNoi_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        private void chkStartCraft_CheckedChanged(object sender, EventArgs e)
        {
            if (chkStartCraft.Checked)
            {
                if (Global.IsVIP == 0)
                {
                    MessageBox.Show(this, "Xin lỗi bạn chưa kích hoạt VIP nên không thể sử dụng chức năng này", "MicroAuto", MessageBoxButtons.OK);
                    chkStartCraft.Checked = false;
                    return;
                }
                if (CurGame != null)
                {
                    if (CurGame.Address.GameType != 2)
                    {
                        MessageBox.Show(this, "Xin lỗi chức năng này chỉ dành cho TLBB private", "MicroAuto", MessageBoxButtons.OK);
                        chkStartCraft.Checked = false;
                        return;
                    }
                }
            }
            if (CurGame != null)
            {
                CurGame.IsCheDo = chkStartCraft.Checked;
                if (!CurGame.IsRunCraft && CurGame.IsCheDo)
                    CurGame.CheDoStart();
                CurGame.SaveSetting();
            }
        }

        private void tiệnÍchToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }










        public static bool IsMoveSavePoint = false;
        public static string point = "";


        private void menuMoveSavePoint_Click(object sender, EventArgs e)
        {
            //IsMoveSavePoint = menuMoveSavePoint.Checked = !menuMoveSavePoint.Checked;
            //if (SelecteOnlinedGames.Count > 0)
            //{
            //    GamesToMove.Clear();
            //    foreach (Game game in SelecteOnlinedGames)
            //        GamesToMove.Add(game);
            //    int map = TDT.ParseInt(listViewSavePoint.SelectedItems[0].Tag.ToString());
            //    if (listViewSavePoint.SelectedItems[0].Text.Contains("[") && listViewSavePoint.SelectedItems[0].Text.Contains(","))
            //    {
            //        int x = TDT.ParseInt(listViewSavePoint.SelectedItems[0].Text.Split('[')[1]);
            //        int y = TDT.ParseInt(listViewSavePoint.SelectedItems[0].Text.Split(',')[1]);
            //        point = x + "," + y + "," + map;
            //    }
            //}
        }






        public static bool PausedSkill;



        public static int TyGia = 500000;

        private void nudV_ValueChanged(object sender, EventArgs e)
        {
            TyGia = (int)numberGiaKNB.Value * 10000;
            CurGame.DoStringEx("PRICE = " + Main.TyGia + ";");
            CurGame.CurTab = 4;
            CurGame.NPCID = 0;

        }

        private void chkBuyKNB_CheckedChanged(object sender, EventArgs e)
        {
            if (CurGame != null)
            {
                CurGame.PushMissions(MissionsType.AutoBuyKNB, checkMuaKNB.Checked);
                CurGame.DoStringEx("PRICE = " + Main.TyGia + ";");
                CurGame.CurTab = 4;
                CurGame.NPCID = 0;
            }
        }

        private void btnTalkChannel_Click(object sender, EventArgs e)
        {
            CreateControl(new TalkChannel());
        }

        private void btnOnlyAttack_Click(object sender, EventArgs e)
        {
            CreateControl(new OnlyAttack());
        }

        Control Control { get; set; }

        public void CreateControl(Control control)
        {
            Control = control;
            Control.Disposed += (ss, ee) =>
            {
                Text = Global.Version + " - " + "Free";
                if (Global.IsFull > 0)
                    Text = Global.Version + " - " + "Pro";
                mainMenu.Visible = true;
            };
            mainMenu.Visible = false;
            Control.Dock = DockStyle.Fill;
            Container.Controls.Add(Control);
            Control.BringToFront();
            Text = Control.Tag.ToString();
        }




        delegate void ActvieChat(string content);



        public static bool IsBanRac { get; set; }




        public static bool IsExitDead { get; set; }




        private void listViewMain_DoubleClick_1(object sender, EventArgs e)
        {
            listViewMain_DoubleClick(sender, e);
        }

        private void btnBet_Click(object sender, EventArgs e)
        {
            Poster posterBet = new Poster();
            posterBet.Control = this;
            posterBet.Url = "http://tieudattai.org/remlaw/bet.php";
            posterBet.Data = "cmd=BET&beri=" + HttpUtility.UrlEncode(TDT.ParseAllInt(numBet.Text).ToString()) + "&email=" + HttpUtility.UrlEncode(User.Email) + "&pass=" + HttpUtility.UrlEncode(User.Pass);
            posterBet.AutoReconnect = true;
            posterBet.Completed += PosterBet_Completed;
            posterBet.Post();
            btnBet.Enabled = false;
        }

        private void PosterBet_Completed(object sender, EventArgs e)
        {
            var poster = sender as Poster;
            MessageBox.Show(this, poster.Response, "MicroAuto", MessageBoxButtons.OK);
            btnBet.Enabled = true;
        }

   

        public static bool IsKhongChonMayChu { get; set; }



        private void listViewMain_ItemCheck(object sender, ItemCheckEventArgs e)
        {

        }









        private void menuTHDC1Bai1_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in Lv.SelectedItems)
            {
                var game = item.Tag as Game;
                game.NeedToMove = "187,113," + MAP.TanHoangDiaCungTang1;
            }
            if (CurGame != null)
                CurGame.NeedToMove = "187,113," + MAP.TanHoangDiaCungTang1;
        }

        private void menuTHDC1Bai2_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in Lv.SelectedItems)
            {
                var game = item.Tag as Game;
                game.NeedToMove = "114,122," + MAP.TanHoangDiaCungTang1;
            }
            if (CurGame != null)
                CurGame.NeedToMove = "114,122," + MAP.TanHoangDiaCungTang1;
        }

        private void menuTHDC1Bai3_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in Lv.SelectedItems)
            {
                var game = item.Tag as Game;
                game.NeedToMove = "27,124," + MAP.TanHoangDiaCungTang1;
            }
            if (CurGame != null)
                CurGame.NeedToMove = "27,124," + MAP.TanHoangDiaCungTang1;
        }

        private void menuTHDC2Bai1_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in Lv.SelectedItems)
            {
                var game = item.Tag as Game;
                game.NeedToMove = "223,225," + MAP.TanHoangDiaCungTang2;
            }
            if (CurGame != null)
                CurGame.NeedToMove = "223,225," + MAP.TanHoangDiaCungTang2;
        }

        private void menuTHDC2Bai2_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in Lv.SelectedItems)
            {
                var game = item.Tag as Game;
                game.NeedToMove = "134,41," + MAP.TanHoangDiaCungTang2;
            }
            if (CurGame != null)
                CurGame.NeedToMove = "134,41," + MAP.TanHoangDiaCungTang2;
        }

        private void menuTHDC3_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in Lv.SelectedItems)
            {
                var game = item.Tag as Game;
                game.NeedToMove = "224,216," + MAP.TanHoangDiaCungTang3;
            }
            if (CurGame != null)
                CurGame.NeedToMove = "224,216," + MAP.TanHoangDiaCungTang3;
        }

        private void menuTHDC4_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in Lv.SelectedItems)
            {
                var game = item.Tag as Game;
                game.NeedToMove = "31,33," + MAP.TanHoangDiaCungTang4;
            }
            if (CurGame != null)
                CurGame.NeedToMove = "31,33," + MAP.TanHoangDiaCungTang4;
        }

        private void tabPage12_Click(object sender, EventArgs e)
        {

        }

        private void địnhVịThổLinhChâuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var game in SelectedOnlinedGames)
            {
                Task.Run(() =>
                {
                    game.DinhViThoLinhChau();
                });
            }
        }



        private void button4_Click(object sender, EventArgs e)
        {
            if (CurGame != null)
                txtRao.Text = CurGame.GameRaoTXT;
        }



        private void Update_Updater(object sender, EventArgs e)
        {
            menuExit_Click(null, null);
        }

        private void thêmTọaĐộToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurGame != null)
            {
                CreateControl(new DiChuyen());
                Control.Disposed += Dichuyen_Disposed;
            }
        }

        private void Dichuyen_Disposed(object sender, EventArgs e)
        {
            try
            {
                bool isdis = true;
                do
                {
                    isdis = false;
                    foreach (ToolStripItem item in diChuyểnToolStripMenuItem.DropDownItems)
                    {
                        if (item.Tag != null && !string.IsNullOrEmpty(item.Tag.ToString().Trim()))
                        {
                            item.Dispose();
                            isdis = true;
                            break;
                        }
                    }
                }
                while (isdis);
                List<ToolStripMenuItem> lst = new List<ToolStripMenuItem>();
                string map = Game.IniParser.Read("Item", "DiChuyen");
                foreach (string line in map.Split('\n'))
                {
                    string l = line.Trim();
                    if (l.Split('|').Length == 3)
                    {
                        string xy = l.Split('|')[0];
                        string name = l.Split('|')[1];
                        string nicename = l.Split('|')[2];
                        ToolStripMenuItem item = new ToolStripMenuItem();
                        item.Text = nicename;
                        item.Tag = xy;
                        item.Click += Item_Click;
                        lst.Add(item);
                    }
                }
                if (lst.Count > 0)
                    diChuyểnToolStripMenuItem.DropDownItems.Add(new ToolStripSeparator() { Tag = "user" });
                diChuyểnToolStripMenuItem.DropDownItems.AddRange(lst.ToArray());
            }
            catch
            {
            }
        }

        private void Item_Click(object sender, EventArgs e)
        {
            try
            {
                var map = sender as ToolStripMenuItem;
                var xy = map.Tag.ToString();
                if (xy.Split(',').Length == 3)
                {
                    foreach (ListViewItem item in Lv.SelectedItems)
                    {
                        var game = item.Tag as Game;
                        game.NeedToMove = xy;
                    }
                }
            }
            catch { }
        }









        public static void RunWS()
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                while (true)
                {
                    Thread.CurrentThread.IsBackground = true;




                    try
                    {
                        if (ws == null)
                        {
                            ws = new WebSocketSharp.WebSocket("wss://tieudattai.org:8443");

                            ws.SslConfiguration.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) =>
                            {
                                return true;
                            };
                            ws.SslConfiguration.EnabledSslProtocols = SslProtocols.Default | SslProtocols.Ssl2 | SslProtocols.Tls | SslProtocols.Ssl3;

                            ws.OnMessage += Ws_OnMessage;
                            ws.OnOpen += Ws_OnOpen;
                            ws.OnClose += Ws_OnClose;
                            ws.OnError += Ws_OnError;
                            ws.Connect();
                            ws.Send("email=" + User.Email + "&password=" + User.Pass);
                        }
                        else
                        {
                            if (!ws.Ping())
                            {
                                WsState = "Close";
                                WsState = "Opening";



                                ws.OnMessage -= Ws_OnMessage;
                                ws.OnOpen -= Ws_OnOpen;
                                ws.OnClose -= Ws_OnClose;
                                ws.OnError -= Ws_OnError;


                                ws = new WebSocketSharp.WebSocket("wss://tieudattai.org:8443");
                                ws.OnMessage += Ws_OnMessage;
                                ws.OnOpen += Ws_OnOpen;
                                ws.OnClose += Ws_OnClose;
                                ws.OnError += Ws_OnError;
                                //ws.pi
                                //ws.on
                                ws.Connect();
                                ws.Send("email=" + User.Email + "&password=" + User.Pass);
                            }
                        }
                    }


                    catch { }
                    Thread.Sleep(5000);
                }
            }).Start();
        }



        public static WebSocketSharp.WebSocket ws { get; set; }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

            while (true)
            {
                MonitorInfo();
                Thread.Sleep(1500);
            }
        }

        private static void Ws_OnError(object sender, WebSocketSharp.ErrorEventArgs e)
        {
            try
            {
                WsState = "Close";
                ws.OnMessage -= Ws_OnMessage;
                ws.OnOpen -= Ws_OnOpen;
                ws.OnClose -= Ws_OnClose;
                ws.OnError -= Ws_OnError;
                ws.Close();


            }
            catch { }
        }

        private static void Ws_OnClose(object sender, WebSocketSharp.CloseEventArgs e)
        {
            try
            {
                WsState = "Close";
                ws.OnMessage -= Ws_OnMessage;
                ws.OnOpen -= Ws_OnOpen;
                ws.OnClose -= Ws_OnClose;
                ws.OnError -= Ws_OnError;
                ws.Close();

            }
            catch { }
        }

        private static void Ws_OnOpen(object sender, EventArgs e)
        {
            WsState = "Open";
        }






        static string oldestmsg = "";

        private static void Ws_OnMessage(object sender, WebSocketSharp.MessageEventArgs e)
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                if (e.Data.Split('|').Length == 6)
                {
                    Stopwatch sw = Stopwatch.StartNew();
                    try
                    {
                        string i = e.Data;
                        string id = i.Split('|')[0];
                        int lvl = i.Split('|')[1].ToNumber();
                        int mam = i.Split('|')[2].ToNumber();
                        int shit = i.Split('|')[3].ToNumber();
                        int but = i.Split('|')[4].ToNumber();


                        foreach (var a in MicroLogin.Accounts.Where(a => a.Ids == id))
                        {
                            Main.Instance.Invoke(() =>
                            {
                                try
                                {
                                    a.CountMam = mam.ToString();
                                    a.CountShit = shit.ToString();
                                    a.CountBut = but.ToString();
                                    a.Lvl = lvl.ToString();
                                }
                                catch { }
                            });

                        }

                        foreach (TabLogin tab in TabLogin.List)
                        {
                            foreach (var a in tab.Accounts.Where(a => a.Ids == id))
                            {
                                Main.Instance.Invoke(() =>
                                {
                                    try
                                    {
                                        a.CountMam = mam.ToString();
                                        a.CountShit = shit.ToString();
                                        a.CountBut = but.ToString();
                                        a.Lvl = lvl.ToString();
                                    }
                                    catch { }
                                });
                            }
                        }
                    }
                    catch { }

                }
                else
                {

                    string sen = TDT.StringBetween(e.Data, "@", ":");
                    string time = TDT.StringBetween(e.Data, "[", "]");
                    string data = e.Data;
                    bool isOld = false;
                    if (time != null && time.Length == 19)
                    {
                        data = e.Data.Replace("[" + time + "]", "");
                        if (oldestmsg == "")
                        {
                            oldestmsg = time;
                        }
                    }
                    if (sen != null && sen.Length > 0)
                    {
                        RichTextBox ricChat = Instance.ricChat;
                        if (!isOld)
                        {

                            try
                            {
                                Instance.Invoke(new Action(() =>
                                {
                                    ricChat.Text = ricChat.Text.Trim();
                                    ricChat.AppendText(Environment.NewLine);
                                    ricChat.AppendText("#" + sen + ": ");
                                    ricChat.AppendText(data.Replace("@" + sen + ":", "").Replace("#", "").Replace(":", ""));
                                    string last = ricChat.Text;

                                    ricChat.Text = string.Empty;
                                    ricChat.Text = last;
                                    StringExtensions.RichTextBoxChangeWordColor(ref ricChat, "#", ":", Color.BlueViolet);
                                    ricChat.ScroolToEnd();
                                }));
                            }
                            catch { }


                        }
                        else
                        {
                            try
                            {
                                Instance.Invoke(new Action(() =>
                                {
                                    ricChat.AppendText(sen + ": ", Color.Gray);
                                    ricChat.AppendText(data.Replace("@" + sen + ":", ""), Color.BlueViolet);
                                    ricChat.AppendText(Environment.NewLine);
                                    ricChat.ScroolToEnd();
                                }));
                            }
                            catch { }
                        }
                    }

                    if (!IsMuteChat)
                    {
                        Music.PlayChat();
                    }
                }
            }).Start();

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            var richTextBox = sender as RichTextBox;
            richTextBox.SelectionStart = richTextBox.Text.Length;
            // scroll it automatically
            richTextBox.ScrollToCaret();
        }




        public static bool MuteChatBox = false;

     

        public static string GomDoName { get; set; } = string.Empty;






        public static bool IsPKNM { get; set; } = true;



        private void lênBãiTrainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in Lv.SelectedItems)
            {
                var game = item.Tag as Game;
                game.NeedToMove = game.ToaDoTrain;
            }
            if (CurGame != null)
                CurGame.NeedToMove = CurGame.ToaDoTrain;
        }

        private void lưuTọaĐộTrainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var game in SelectedOnlinedGames)
            {
                game.ToaDoTrain = game.RoundX + "," + game.RoundY + "," + game.TLBB.MapId;
            }
            lblTrain.Text = "" + CurGame.ToaDoTrainEx;
        }

        private void dùngThổLinhChâuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in Lv.SelectedItems)
            {
                var game = item.Tag as Game;
                game.PushMissions(MissionsType.DungThoLinhChau);
            }
        }




        private void triệuTậpNhómAltF3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TrieuTapNhom();
        }

        private void mờiĐộiCtrlEndToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurGame != null)
            {
                if (CurGame.TLBB.Name != "ĐăngNhập")
                {
                    if (CurGame.TLBB.IsLeader)
                    {
                        MoiDoi(CurGame);
                    }
                    else
                    {
                        CurGame.LUA.PlayerCreateTeamSelf();
                    }
                }
            }
        }

        private void triệuTậpAltF2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurGame != null)
                TrieuTap(CurGame);
        }

        private void menuSetNhom_Click(object sender, EventArgs e)
        {
            if (CurGame.TLBB.Online)
                Setting.Teams[CurGame.TLBB.Name] = SelectedOnlinedGames.Select(game => game.TLBB.Name).ToList();
        }

        public static bool IsCreateamTyVo { get; set; }

        private void menuAutoToDoi_Click(object sender, EventArgs e)
        {
            CalendarEx.IsAutoCreaTeam = !CalendarEx.IsAutoCreaTeam;

        }

        private void quảnLýNhómToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateControl(new CalendarEx());
        }

        private void tổĐộiNhữngGameĐãChọnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Task.Run((Action)(() =>
            {
                List<Game> list = Enumerable.ToList<Game>(this.SelectedOnlinedGames);
                CreateTeamFromList(list);
            }));
        }

        public static void CreateTeamFromList(List<Game> list)
        {
            if (list.Count > 0)
            {
                list.ForEach(l => { if (l == list[0] && l.TLBB.IsLeader) { } else { l.DoStringEx("Player:LeaveTeam();"); } });
                Thread.Sleep(2000);
                list[0].DoStringEx("Player:CreateTeamSelf();");
                Thread.Sleep(500);
                list.Skip(1).ToList().ForEach(l => l.AskTeam(list[0].TLBB.VISCIIName));
                Thread.Sleep(1000);
                list[0].Accept(true);
            }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            QuyCocPercent = (int)numericUpDown2.Value;
        }

        public static int QuyCocPercent { get; set; } = 50;













        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {

        }






        private void giảiTánMọiĐộiNgũToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Game game in AllOnelineGame)
                game.DoStringEx("Player:LeaveTeam(); Player:LeaveRiad();");
        }





        private void binhthanhHoTro(object sender, EventArgs e)
        {
            if (CurGame != null)
            {
                QuanDoan quandoan = new QuanDoan();
                quandoan.Party1 = CurGame.QuanDoanParty1;
                quandoan.Party2 = CurGame.QuanDoanParty2;
                if (quandoan.Party1.Count() > 0 && quandoan.Party2.Count() > 0)
                {

                }
                else
                {
                    foreach (KeyValuePair<int, Game> kvp in DicGame)
                    {
                        if (quandoan.Party1.Count() == 0)
                        {
                            if (kvp.Value.TLBB.MapId == MAP.BinhThanhKyTran)
                            {
                                if (kvp.Value.Leader != null)
                                {
                                    quandoan.Party1 = kvp.Value.Party;
                                }
                            }

                        }
                        else
                        {
                            if (quandoan.Party2.Count() == 0)
                            {
                                if (kvp.Value.TLBB.MapId == MAP.BinhThanhKyTran)
                                {
                                    if (kvp.Value.Leader != null && !quandoan.Party1.Contains(kvp.Value))
                                    {
                                        quandoan.Party2 = kvp.Value.Party;
                                        break;
                                    }
                                }
                            }
                        }

                    }
                    TabPage page = new TabPage("BinhThánh");
                    tabCanQuet.TabPages.Add(page);
                    var control = new BinhThanh(quandoan) { Dock = DockStyle.Fill };
                    control.Disposed += (ss, ee) =>
                    {
                        page.Dispose();
                    };
                    page.Controls.Add(control);
                    tabCanQuet.Visible = true;
                    tabCanQuet.BringToFront();
                    tabCanQuet.SelectedTab = page;
                }
            }
        }







        private void lênNgựaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Game game in AllOnelineGame)
                game.UpRide();
        }

        private void xuốngNgựaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Game game in AllOnelineGame)
                game.DownRide();

        }



        private void listViewSkill_MouseDown(object sender, MouseEventArgs e)
        {
            var listView = listViewSkill;
            var info = listView.HitTest(e.X, e.Y);
            var row = info.Item.Index;
            var col = info.Item.SubItems.IndexOf(info.SubItem);
            if (col == 1)
            {
                var value = info.Item.SubItems[col].Text;
                Skill skill = info.Item.Tag as Skill;
                skill.UsePK = !skill.UsePK;
                info.Item.SubItems[1].Text = skill.UsePK ? "X" : "";
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show(this, @"x 1.98 Beri Bet hoặc thua toàn bộ
Tỷ lệ ăn thua là 50 50
Ví dụ các bạn Bet 100 Beri khi thắng các bạn nhận 198 Beri
Khi thua mất 100 Beri đặt cược
Chúc các bạn chơi game BET vui vẻ
", "MicroAuto", MessageBoxButtons.OK);
        }











        private void muVoLuongSon_Click(object sender, EventArgs e)
        {
            SelectedOnlineLeaders.ForEach(l =>
            {
                if (muVoLuongSon.Checked)
                {
                    l.RemoveMission(MissionsType.DatDoiAcTac);
                }
                else
                {
                    l.MapAcTac = MAP.VoLuongSon;
                    l.PushMissions(MissionsType.DatDoiAcTac);
                }
            }
            );
        }

        private void muKinhHo_Click(object sender, EventArgs e)
        {
            SelectedOnlineLeaders.ForEach(l =>
            {
                if (muKinhHo.Checked)
                {
                    l.RemoveMission(MissionsType.DatDoiAcTac);
                }
                else
                {
                    l.MapAcTac = MAP.KinhHo;
                    l.PushMissions(MissionsType.DatDoiAcTac);
                }
            }
           );
        }

        private void muKiemCac_Click(object sender, EventArgs e)
        {
            SelectedOnlineLeaders.ForEach(l =>
            {
                if (muKiemCac.Checked)
                {
                    l.RemoveMission(MissionsType.DatDoiAcTac);
                }
                else
                {
                    l.MapAcTac = MAP.KiemCac;
                    l.PushMissions(MissionsType.DatDoiAcTac);
                }
            }
         );
        }

        private void muThaiHo_Click(object sender, EventArgs e)
        {
            SelectedOnlineLeaders.ForEach(l =>
            {
                if (muThaiHo.Checked)
                {
                    l.RemoveMission(MissionsType.DatDoiAcTac);
                }
                else
                {
                    l.MapAcTac = MAP.ThaiHo;
                    l.PushMissions(MissionsType.DatDoiAcTac);
                }
            }
        );
        }

        private void muTungSon_Click(object sender, EventArgs e)
        {
            SelectedOnlineLeaders.ForEach(l =>
            {
                if (muTungSon.Checked)
                {
                    l.RemoveMission(MissionsType.DatDoiAcTac);
                }
                else
                {
                    l.MapAcTac = MAP.TungSon;
                    l.PushMissions(MissionsType.DatDoiAcTac);
                }
            }
       );
        }

        private void muDonHoang_Click(object sender, EventArgs e)
        {
            SelectedOnlineLeaders.ForEach(l =>
            {
                if (muDonHoang.Checked)
                {
                    l.RemoveMission(MissionsType.DatDoiAcTac);
                }
                else
                {
                    l.MapAcTac = MAP.DonHoang;
                    l.PushMissions(MissionsType.DatDoiAcTac);
                }
            }
       );
        }

        private void muAcBa_Click(object sender, EventArgs e)
        {

        }

        private void muTayHo_Click(object sender, EventArgs e)
        {
            SelectedOnlineLeaders.ForEach(l =>
            {
                if (muTayHo.Checked)
                {
                    l.RemoveMission(MissionsType.DatDoiTangKinhCac);
                }
                else
                {
                    l.MapTangKinhCac = MAP.TayHo;
                    l.PushMissions(MissionsType.DatDoiTangKinhCac);
                }
            }
      );
        }

        private void muNhiHai_Click(object sender, EventArgs e)
        {
            SelectedOnlineLeaders.ForEach(l =>
            {
                if (muNhiHai.Checked)
                {
                    l.RemoveMission(MissionsType.DatDoiTangKinhCac);
                }
                else
                {
                    l.MapTangKinhCac = MAP.NhiHai;
                    l.PushMissions(MissionsType.DatDoiTangKinhCac);
                }
            }
      );
        }

        private void muNhanNam_Click(object sender, EventArgs e)
        {
            SelectedOnlineLeaders.ForEach(l =>
            {
                if (muNhanNam.Checked)
                {
                    l.RemoveMission(MissionsType.DatDoiTangKinhCac);
                }
                else
                {
                    l.MapTangKinhCac = MAP.NhanNam;
                    l.PushMissions(MissionsType.DatDoiTangKinhCac);
                }
            }
      );
        }

        private void muDaTru_Click(object sender, EventArgs e)
        {
            SelectedOnlineLeaders.ForEach(l => { l.PushMissions(MissionsType.DatDoiDaTru, !muDaTru.Checked); });
        }

        private void muKyCuocNhanh_Click(object sender, EventArgs e)
        {
            SelectedOnlineLeaders.ForEach(g => { g.PushMissions(MissionsType.DatDoiKyCuoc, !muKyCuocNhanh.Checked); g.PushMissions(MissionsType.KyCuocNhanh, !muKyCuocNhanh.Checked); });
        }

        private void muKyCuocCham_Click(object sender, EventArgs e)
        {
            SelectedOnlineLeaders.ForEach(g => { g.PushMissions(MissionsType.DatDoiKyCuoc, !muKyCuocCham.Checked); g.RemoveMission(MissionsType.KyCuocNhanh); });
        }

        private void muLauLanTamBaoNhanh_Click(object sender, EventArgs e)
        {
            SelectedOnlineLeaders.ForEach(l => { l.PushMissions(MissionsType.DatDoiLauLanTamBao, !muLauLanTamBaoNhanh.Checked); l.PushMissions(MissionsType.LauLanTamBaoNhanh, !muLauLanTamBaoNhanh.Checked); });
        }

        private void muLauLanTamBaoCham_Click(object sender, EventArgs e)
        {
            SelectedOnlineLeaders.ForEach(l => { l.PushMissions(MissionsType.DatDoiLauLanTamBao, !muLauLanTamBaoCham.Checked); l.RemoveMission(MissionsType.LauLanTamBaoNhanh); });
        }

        private void muMongHeo_Click(object sender, EventArgs e)
        {
            SelectedOnlineLeaders.ForEach(g => { g.PushMissions(MissionsType.DatDoiMongHeo, !muMongHeo.Checked); });
        }

        private void muThienGiangKyThu_Click(object sender, EventArgs e)
        {
            SelectedOnlineLeaders.ForEach(g => { g.PushMissions(MissionsType.DatDoiThienGiangKyThu, !muThienGiangKyThu.Checked); });
        }

        private void muPhungHoangLangMo_Click(object sender, EventArgs e)
        {
            SelectedOnlineLeaders.ForEach(g => { g.PushMissions(MissionsType.DatDoiPhungHoangLangMo, !muPhungHoangLangMo.Checked); });
        }



        private void leaderToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            Config_Disposed(null, null);
            muAcTac.Font = new Font(muAcTac.Font, FontStyle.Regular);
            muAcBa.Font = new Font(muAcBa.Font, FontStyle.Regular);
            muTangKinhCac.Font = new Font(muTangKinhCac.Font, FontStyle.Regular);
            muKyCuoc.Font = new Font(muKyCuoc.Font, FontStyle.Regular);
            muLauLanTamBao.Font = new Font(muLauLanTamBao.Font, FontStyle.Regular);
            muDaTru.Font = new Font(muDaTru.Font, FontStyle.Regular);
            muMongHeo.Font = new Font(muMongHeo.Font, FontStyle.Regular);
            muThienGiangKyThu.Font = new Font(muThienGiangKyThu.Font, FontStyle.Regular);
            muPhungHoangLangMo.Font = new Font(muPhungHoangLangMo.Font, FontStyle.Regular);
            int hour = DateTime.Now.Hour;
            int min = DateTime.Now.Minute;
            if (hour == 13 || hour == 15 || hour == 17 || hour == 19 || hour == 21 || hour == 2)
                muAcTac.Font = new Font(muAcTac.Font, FontStyle.Bold);
            if ((hour == 0 || hour == 4 || hour == 10 || hour == 12 || hour == 16 || hour == 20 || hour == 22) && min <= 30)
                muAcBa.Font = new Font(muAcBa.Font, FontStyle.Bold);
            if ((hour == 10 && min >= 45) || (hour == 11 && min <= 15) || (hour == 16 && min >= 30) || (hour == 21 && min >= 30) || (hour == 23 && min <= 30))
                muTangKinhCac.Font = new Font(muTangKinhCac.Font, FontStyle.Bold);
            if ((hour == 11 & min >= 30) || hour == 12 || hour == 13 || (hour == 14 && min <= 30) || (hour == 20 && min >= 30) || hour == 21)
                muKyCuoc.Font = new Font(muKyCuoc.Font, FontStyle.Bold);
            if ((hour == 11 & min >= 30) || hour == 12 || hour == 13 || (hour == 14 && min <= 30) || (hour == 19 && min >= 30) || hour == 20 || hour == 21)
                muLauLanTamBao.Font = new Font(muLauLanTamBao.Font, FontStyle.Bold);
            if (hour == 14 || hour == 15 || (hour == 16 && min <= 40) || (hour == 21 && min >= 30) || hour == 22 || hour == 23)
            {
                muDaTru.Font = new Font(muDaTru.Font, FontStyle.Bold);
                muMongHeo.Font = new Font(muMongHeo.Font, FontStyle.Bold);
            }
            if ((hour == 13 && min >= 30) || (hour == 18 && min >= 30) || (hour == 19 && min <= 30) || (hour == 21 && min >= 45) || (hour == 22 && min <= 15))
            {
                muThienGiangKyThu.Font = new Font(muThienGiangKyThu.Font, FontStyle.Bold);
            }
            if (hour == 13 || (hour == 22 && min >= 30) || (hour == 23 && min <= 30))
            {
                muPhungHoangLangMo.Font = new Font(muPhungHoangLangMo.Font, FontStyle.Regular);
            }
            if (SelectedOnlineLeaders.Count() == 0)
            {
                foreach (var item in leaderToolStripMenuItem.DropDownItems)
                {
                    if (item.GetType() == typeof(ToolStripMenuItem))
                    {
                        ((ToolStripMenuItem)item).Checked = false;
                        foreach (var i in ((ToolStripMenuItem)item).DropDownItems)
                        {
                            if (i.GetType() == typeof(ToolStripMenuItem))
                            {
                                ((ToolStripMenuItem)i).Checked = false;
                            }
                        }
                    }
                }
                return;
            }
            var game = SelectedOnlineLeaders.FirstOrDefault();
            tựĐộngToolStripMenuItem2.Checked = game.Missions.Contains(MissionsType.DatDoiAcBa);

            if (!game.Missions.Contains(MissionsType.DatDoiAcTac))
            {
                muVoLuongSon.Checked = muKinhHo.Checked = muKiemCac.Checked = muTungSon.Checked = muDonHoang.Checked = false;
                muAcTac.Checked = false;
            }
            else
            {
                muVoLuongSon.Checked = game.MapAcTac == MAP.VoLuongSon;
                muKinhHo.Checked = game.MapAcTac == MAP.KinhHo;
                muKiemCac.Checked = game.MapAcTac == MAP.KiemCac;
                muThaiHo.Checked = game.MapAcTac == MAP.ThaiHo;
                muTungSon.Checked = game.MapAcTac == MAP.TungSon;
                muDonHoang.Checked = game.MapAcTac == MAP.DonHoang;
                muAcTac.Checked = true;
            }

            kếtNghĩaToolStripMenuItem.Checked = game.IsKetNghia;
            mnuKetNghia.Checked = game.IsKetBai;
            muAcBa.Checked = game.Missions.Contains(MissionsType.DatDoiAcBa);

            if (!game.Missions.Contains(MissionsType.DatDoiTangKinhCac))
            {
                muTayHo.Checked = muNhiHai.Checked = muNhanNam.Checked = false;
                muTangKinhCac.Checked = false;
            }
            else
            {
                muTayHo.Checked = game.MapTangKinhCac == MAP.TayHo;
                muNhiHai.Checked = game.MapTangKinhCac == MAP.NhiHai;
                muNhanNam.Checked = game.MapTangKinhCac == MAP.NhanNam;
                muTangKinhCac.Checked = true;
            }

            muDaTru.Checked = game.Missions.Contains(MissionsType.DatDoiDaTru);
            muKyCuoc.Checked = game.Missions.Contains(MissionsType.DatDoiKyCuoc);
            if (muKyCuoc.Checked)
            {
                muKyCuocCham.Checked = !game.Missions.Contains(MissionsType.KyCuocNhanh);
                muKyCuocNhanh.Checked = game.Missions.Contains(MissionsType.KyCuocNhanh);
            }
            else
            {
                muKyCuocCham.Checked = muKyCuocNhanh.Checked = false;
            }
            muLauLanTamBao.Checked = game.Missions.Contains(MissionsType.DatDoiLauLanTamBao);
            if (muLauLanTamBao.Checked)
            {
                muLauLanTamBaoCham.Checked = !game.Missions.Contains(MissionsType.LauLanTamBaoNhanh);
                muLauLanTamBaoNhanh.Checked = game.Missions.Contains(MissionsType.LauLanTamBaoNhanh);
            }
            else
            {
                muLauLanTamBaoCham.Checked = muLauLanTamBaoNhanh.Checked = false;
            }
            muMongHeo.Checked = game.Missions.Contains(MissionsType.DatDoiMongHeo);
            muThienGiangKyThu.Checked = game.Missions.Contains(MissionsType.DatDoiThienGiangKyThu);
            muPhungHoangLangMo.Checked = game.Missions.Contains(MissionsType.DatDoiPhungHoangLangMo); ;
            muDoanBaoMaTac.Checked = game.Missions.Contains(MissionsType.DatDoiMaTac);
            muBaoDoHiem.Checked = game.Missions.Contains(MissionsType.DatDoiBaoDoHiem);
            muQuanSonHai.Checked = game.Missions.Contains(MissionsType.DatDoiQuanSonHai);
            muTyVo.Checked = game.Missions.Contains(MissionsType.DatDoiTyVo);
            muTucCau.Checked = game.Missions.Contains(MissionsType.DatDoiTucCau);
            //muPhieuMieuPhong.Checked = game.IsPhieuMieuPhong || game.IsHuyetChienPhieuMieuPhong;
            //muPhieuMieuPhongThuong.Checked = game.IsPhieuMieuPhong;
            //muPhieuMieuPhongHuyetChien.Checked = game.IsHuyetChienPhieuMieuPhong;
            //muTuTuyetTrang.Checked = game.IsTuTuyetTrang;
            //muQToChau.Checked = game.IsQ123ToChau;
            //muQLauLan.Checked = game.IsQ123LauLan;
            //muVuongLang.Checked = game.IsVuongLang;
            //muYenTuO.Checked = game.IsYenTuO;
            //muSatTinh.Checked = game.IsSatTinh;
            //muThieuThatSon.Checked = game.IsThieuThatSon;
            //muTamThanHuyenCanh.Checked = game.IsTamThan;
            //muPhucDiaThuong.Checked = game.IsPhucDia;
            //if (game.IsPhucDiaKho)
            //    muPhucDiaThuong.Checked = false;
            //muPhucDiaKho.Checked = game.IsPhucDiaKho && game.IsPhucDia;
            //muPhucDia.Checked = muPhucDiaThuong.Checked || muPhucDiaKho.Checked;

            muQ1ToChau.Checked = game.Q12TK == 1;
            muQ2ToChau.Checked = game.Q12TK == 2;
            muQ12ToChau.Checked = game.Q12TK == 3;
            muThuyLao.Checked = game.IsThuyLao;
            muPrivate.Checked = muQ1ToChau.Checked || muQ2ToChau.Checked || muQ12ToChau.Checked || muThuyLao.Checked;
        }

        private void muToanBoHangNgay_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach((Action<Game>)(g =>
            {
                g.PushMissions(MissionsType.ThuTaiVanMay, !muToanBoHangNgay.Checked);
                g.PushMissions(MissionsType.LoLyHoa, !muToanBoHangNgay.Checked);
                foreach (var item in g.PacketItems.All)
                {
                    if (item.ClearName == "nguyenlinhtuyen" && item.Count >= 5)
                    {
                        g.PushMissions(MissionsType.NguyenVongThienLinh, !muToanBoHangNgay.Checked);
                        break;
                    }
                }
            }));
        }

        private void muLoLyHoa_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g =>
            {
                g.PushMissions(MissionsType.LoLyHoa, !muLoLyHoa.Checked);
            });
        }

        private void muNhanPhiThuy_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g =>
            {
                g.PushMissions(MissionsType.NhanPhiThuy, !muNhanPhiThuy.Checked);
            });
        }

        private void muLuyenKimNhanh_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => { g.PushMissions(MissionsType.LuyenKimNhanh, !muLuyenKimNhanh.Checked); g.RemoveMission(MissionsType.LuyenKim); });
        }

        private void muLuyenKimCham_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => { g.PushMissions(MissionsType.LuyenKim, !muLuyenKimCham.Checked); g.RemoveMission(MissionsType.LuyenKimNhanh); });
        }

        private void muThuBi_Click(object sender, EventArgs e)
        {
        }

        private void muTrungAc_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => { g.PushMissions(MissionsType.TrungAc, !muTrungAc.Checked); g.SaveSetting(); });
        }

        private void muCotTruyenToanBo_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => { g.PushMissions(MissionsType.NhiemVuThangCap, !muCotTruyenToanBo.Checked); g.SaveSetting(); });
        }

        private void muNhiemVuExp_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g =>
            {
                g.PushMissions(MissionsType.NhiemVuExp, !muNhiemVuExp.Checked);
            });
        }

        private void muNhiemVuKNB_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g =>
            {
                g.PushMissions(MissionsType.NhiemVuKNBKhoa, !muNhiemVuKNB.Checked);
            });
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g =>
            {
                g.PushMissions(MissionsType.NgoChanNguyen, !muNgoChanNguyen.Checked);
            });
        }

        private void chínhTuyếnToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g =>
            {
                g.PushMissions(MissionsType.NhiemVuChinhTuyen, !muChinhTuyen.Checked);
            });
        }

        private void muSuMon_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => { g.PushMissions(MissionsType.NhiemVuSuMon, !muSuMon.Checked); });
        }

        private void muMoBaoTangDo_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => { g.PushMissions(MissionsType.MoBaoTangDo, !muMoBaoTangDo.Checked); });
        }

        private void muTuDuong_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach((Action<Game>)(g => { g.PushMissions(MissionsType.TuDuongCon, !muTuDuong.Checked); g.MissionState = ""; }));
        }

        private void muPhuMau_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => { g.PushMissions(MissionsType.LongPhuMau, !muPhuMau.Checked); });
        }

        private void muNhanCon_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => { g.PushMissions(MissionsType.NhanCon, !muNhanCon.Checked); });
        }

        private void muTuBaoBon_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => { g.PushMissions(MissionsType.TuBaoBon, !muTuBaoBon.Checked); });
        }


        private void muThienKiepLau_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => { g.PushMissions(MissionsType.TruAc, !muThienKiepLau.Checked); });
        }

        private void muTiemNangTan_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach((Action<Game>)(g => { g.IsOneTNT = !muTiemNangTan.Checked; if (g.IsOneTNT) g.CheckTueHong(); }));
        }

        private void muBachHoaDuyen_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => { g.PushMissions(MissionsType.BachHoaDuyen, !muBachHoaDuyen.Checked); });
        }

        private void muThanhSangNhatHa_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => { g.IsDuaHau = !muThanhSangNhatHa.Checked; });
        }



        private void menuSinhTieu_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => { g.PushMissions(MissionsType.SinhTieu, !muSinhTieu.Checked); });
        }

        private void muLuyenKim_Click(object sender, EventArgs e)
        {

        }

        private void muNangTamPhapClick(object sender, EventArgs e)
        {
            ToolStripMenuItem i = sender as ToolStripMenuItem;
            SelectedOnlinedGames.ForEach(game =>
            {
                game.PushMissions(MissionsType.NangTamPhap);
                game.TamPhapNangToi = TDT.ParseAllInt(i.Text);
            });
        }



        private void dailyToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            if (CurGame == null)
                return;
            muVanTieu.Checked = CurGame.ApTieu != 0;
            muVanTieu1.Checked = CurGame.ApTieu == 1;
            muVanTieu2.Checked = CurGame.ApTieu == 2;
            muVanTieu3.Checked = CurGame.ApTieu == 3;
            muVanTieu4.Checked = CurGame.ApTieu == 4;
            muLoLyHoa.Checked = CurGame.Missions.Contains(MissionsType.LoLyHoa);
            muVanMay.Checked = CurGame.Missions.Contains(MissionsType.ThuTaiVanMay);
            muNguyenVong.Checked = CurGame.Missions.Contains(MissionsType.NguyenVongThienLinh);
            muNhanPhiThuy.Checked = CurGame.Missions.Contains(MissionsType.NhanPhiThuy);
            muTueHong.Checked = CurGame.Missions.Contains(MissionsType.ThienLongTueHong);

            muHangNgay.Checked = muVanMay.Checked || muLoLyHoa.Checked || muNguyenVong.Checked || muNhanPhiThuy.Checked || muTueHong.Checked;



            muTrangSucCuuLe.Checked = CurGame.Missions.Contains(MissionsType.TrangSucCuuLe);
            muTamKy.Checked = CurGame.Missions.Contains(MissionsType.TamKy);
            muLuyenKim.Checked = CurGame.Missions.Contains(MissionsType.LuyenKim) || CurGame.Missions.Contains(MissionsType.LuyenKimNhanh);
            muLuyenKimCham.Checked = CurGame.Missions.Contains(MissionsType.LuyenKim);
            muLuyenKimNhanh.Checked = CurGame.Missions.Contains(MissionsType.LuyenKimNhanh);
            muTrungAc.Checked = CurGame.Missions.Contains(MissionsType.TrungAc);
            muChinhTuyen.Checked = CurGame.Missions.Contains(MissionsType.NhiemVuChinhTuyen);
            muVoY.Checked = CurGame.Missions.Contains(MissionsType.NhiemVuVoY);
            muCotTruyenToanBo.Checked = CurGame.Missions.Contains(MissionsType.NhiemVuThangCap);
            muNhiemVuExp.Checked = CurGame.Missions.Contains(MissionsType.NhiemVuExp);
            muNgoChanNguyen.Checked = CurGame.Missions.Contains(MissionsType.NgoChanNguyen);
            muNhiemVuKNB.Checked = CurGame.Missions.Contains(MissionsType.NhiemVuKNBKhoa);
            muCotTruyen.Checked = muNgoChanNguyen.Checked || muCotTruyenToanBo.Checked || muNhiemVuExp.Checked || muNhiemVuKNB.Checked || muChinhTuyen.Checked || muVoY.Checked;
            muSuMon.Checked = CurGame.Missions.Contains(MissionsType.NhiemVuSuMon);
            muMoBaoTangDo.Checked = CurGame.Missions.Contains(MissionsType.MoBaoTangDo);
            muTuDuong.Checked = CurGame.Missions.Contains(MissionsType.TuDuongCon);
            muNhanCon.Checked = CurGame.Missions.Contains(MissionsType.NhanCon);
            muPhuMau.Checked = CurGame.Missions.Contains(MissionsType.LongPhuMau);
            muConCai.Checked = muTuDuong.Checked || muNhanCon.Checked || muPhuMau.Checked;



            muTuBaoBon.Checked = CurGame.Missions.Contains(MissionsType.TuBaoBon);
            muThienKiepLau.Checked = CurGame.Missions.Contains(MissionsType.TruAc);
            muTiemNangTan.Checked = CurGame.IsOneTNT;
            muBachHoaDuyen.Checked = CurGame.Missions.Contains(MissionsType.BachHoaDuyen);
            muThanhSangNhatHa.Checked = CurGame.IsDuaHau;



            muSinhTieu.Checked = CurGame.Missions.Contains(MissionsType.SinhTieu);

            muThanKhi9Sao.Checked = CurGame.Missions.Contains(MissionsType.ThanKhi9Sao);
            muVoTuPho.Checked = CurGame.Missions.Contains(MissionsType.VoTuPho);
            muDungDoatBaoRuong.Checked = CurGame.Missions.Contains(MissionsType.DungDoatBaoRuong);
        }



        private void muTriLieu_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game =>
            {
                if (!muTriLieu.Checked)
                    game.PushMissions(MissionsType.TriLieu);
                else
                    game.RemoveMission(MissionsType.TriLieu);
            });
        }

        private void muDiBanRac_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game =>
            {
                if (!muDiBanRac.Checked)
                    game.PushMissions(MissionsType.BanRac);
                else
                    game.RemoveMission(MissionsType.BanRac);
            });
        }

        private void muCatDo_Click(object sender, EventArgs e)
        {

        }

        private void muTriLieuBanRacCatDo_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game =>
            {
                game.PushMissions(new[] { MissionsType.TriLieu, MissionsType.BanRac, MissionsType.CatVang, MissionsType.CatDo });
            });
        }




        private void OpenGameClick(object sender, EventArgs e)
        {
            int count = (sender as ToolStripMenuItem).Text.ToNumber();
            for (int i = 0; i < count; i++)
                OpenGames();
        }



        private void playerToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            if (SelectedOnlinedGames.Count() == 0)
            {
                foreach (var items in playerToolStripMenuItem.DropDownItems)
                {
                    if (typeof(ToolStripMenuItem) == items.GetType())
                    {
                        (items as ToolStripMenuItem).Checked = false;
                        foreach (var item in (items as ToolStripMenuItem).DropDownItems)
                        {
                            if (typeof(ToolStripMenuItem) == item.GetType())
                                (item as ToolStripMenuItem).Checked = false;
                        }
                    }
                }
            }
            else
            {
                muTriLieu.Checked = SelectedOnlinedGames.Where(game => !game.Missions.Contains(MissionsType.TriLieu)).Count() == 0;
                muDiBanRac.Checked = SelectedOnlinedGames.Where(game => !game.Missions.Contains(MissionsType.BanRac)).Count() == 0;
                muCatDo.Checked = SelectedOnlinedGames.Where(game => !game.Missions.Contains(MissionsType.CatVang) && !game.Missions.Contains(MissionsType.CatDo)).Count() == 0;
                muLayDo.Checked = SelectedOnlinedGames.Where(game => !game.Missions.Contains(MissionsType.LayDo)).Count() == 0;
                muLayDoLayVang.Checked = SelectedOnlinedGames.Where(game => !(game.Missions.Contains(MissionsType.LayDo) && game.Missions.Contains(MissionsType.LayVang))).Count() == 0;
                if (muLayDoLayVang.Checked)
                    muLayDo.Checked = false;
                muThuongKho.Checked = muCatDo.Checked || muLayDo.Checked || muLayDoLayVang.Checked;
                muPhanGiaiTrangBijPet.Checked = SelectedOnlinedGames.Where(game => !game.Missions.Contains(MissionsType.PhanGiaiTrangBiPet)).Count() == 0;
                muNhanChienCongKiemChi.Checked = SelectedOnlinedGames.Where(game => !(game.Missions.Contains(MissionsType.NhanChienCong) && game.Missions.Contains(MissionsType.NhanKiemChi))).Count() == 0;
                muNhanThuongQuanSonHai.Checked = SelectedOnlinedGames.Where(game => !game.Missions.Contains(MissionsType.NhanThuongQuanSonHai)).Count() == 0;
                muNhanBuaBaoRuong.Checked = SelectedOnlinedGames.Where(game => !game.Missions.Contains(MissionsType.NhanBuaBaoRuong)).Count() == 0;
                muNhanLeBao.Checked = SelectedOnlinedGames.Where(game => !game.Missions.Contains(MissionsType.NhanLeBao)).Count() == 0;
                muNhanBong.Checked = SelectedOnlinedGames.Where(game => !game.Missions.Contains(MissionsType.NhanBong)).Count() == 0;
                muNhanKeoHallowen.Checked = SelectedOnlinedGames.Where(game => !game.Missions.Contains(MissionsType.NhanKeoHallowen)).Count() == 0;
                //muNhanBoiThuong.Checked = AllSelectedOnlinedGames.Where(game => !game.Missions.Contains(MissionsType.NhanBoiThuong)).Count() == 0;
            }

            if (CurGame == null)
                return;
            muMua100KimSangDuoc.Checked = CurGame.Missions.Contains(MissionsType.Mua100KimSangDuoc);
            muLinhLuong.Checked = CurGame.Missions.Contains(MissionsType.LinhLuong);
            muTuLuyenTheLuc.Checked = CurGame.Missions.Contains(MissionsType.TuLuyenTheLuc);
            muTuLuyenNoiLuc.Checked = CurGame.Missions.Contains(MissionsType.TuLuyenNoiLuc);
            muTuLuyenThanPhap.Checked = CurGame.Missions.Contains(MissionsType.TuLuyenThanPhap);
            muTuLuyenCuongLuc.Checked = CurGame.Missions.Contains(MissionsType.TuLuyenCuongLuc);
            muDaiLeHungVuong.Checked = CurGame.Missions.Contains(MissionsType.DaiLeHungVuong);
            muDoi999HoaHong.Checked = CurGame.Missions.Contains(MissionsType.Doi999HoaHong);
            muNhanx2.Checked = CurGame.Missions.Contains(MissionsType.NhanX2);
            muDongx2.Checked = CurGame.Missions.Contains(MissionsType.DongX2);
            muDiMuaNgua.Checked = CurGame.Missions.Contains(MissionsType.DiMuaNga);
            muCheThanKhi.Checked = CurGame.Missions.Contains(MissionsType.CheThanKhi);
            muDoiKimTamTy.Checked = CurGame.Missions.Contains(MissionsType.DoiKimTamTy);
            muGiaoNguHanhPhapThiep.Checked = CurGame.Missions.Contains(MissionsType.GiaoNguHanhPhapThiep);
            muNopTuViHuyTinh.Checked = CurGame.Missions.Contains(MissionsType.NopTuViHuyTinh);
            muDoiLoanPhiMatHam.Checked = CurGame.Missions.Contains(MissionsType.DoiLoanPhiMatHam);
            muDoiChanNguyenLinhPhach.Checked = CurGame.Missions.Contains(MissionsType.DoiChanNguyenLinhPhach);
            muTangCapTruongThanhLongVan.Checked = CurGame.Missions.Contains(MissionsType.TangCapTruongThanhLongVan);
            muThanhLyNhiemVu.Checked = CurGame.Missions.Contains(MissionsType.ThanhLyNhiemVu);
            muDoiTiemNangTan.Checked = CurGame.Missions.Contains(MissionsType.DoiTiemNangTan);
            muDoiHuyenSacCauThienThai.Checked = CurGame.Missions.Contains(MissionsType.DoiHuyenSacCauThienThai);

            muSuaVoHon.Checked = CurGame.Missions.Contains(MissionsType.SuaVoHon);
            muSuaTrangBi.Checked = CurGame.Missions.Contains(MissionsType.SuaTrangBi);
            muSuaThanKhi.Checked = CurGame.Missions.Contains(MissionsType.SuaThanKhi);

            muMoShopTrungDo.Checked = CurGame.Missions.Contains(MissionsType.MoShopTrungDo);
            muMoShopHungBa.Checked = CurGame.Missions.Contains(MissionsType.MoShopHungBa);
            muMoShopQuyThi.Checked = CurGame.Missions.Contains(MissionsType.MoShopQuyThi);
            muMoShopBachBaoCac.Checked = CurGame.Missions.Contains(MissionsType.MoShopBachBaoCac);
            muMoTiemThuoc.Checked = CurGame.Missions.Contains(MissionsType.MoTiemThuoc);
            muMoShop.Checked = muMoShopBachBaoCac.Checked || muMoShopHungBa.Checked || muMoShopQuyThi.Checked || muMoShopTrungDo.Checked || muMoTiemThuoc.Checked;

            muHuyVatPhamQuy.Checked = CurGame.Missions.Contains(MissionsType.HuyVatPhamQuy);
            muTrongHoa.Checked = CurGame.Missions.Contains(MissionsType.TrongHoa);
            muBonHoa.Checked = CurGame.Missions.Contains(MissionsType.BonHoa);
            muThuHoach.Checked = CurGame.IsThuHoachHoaEx;
            muTuoiHoaHong.Checked = CurGame.IsTuoiHoa;
            muNhanQuaBuiHoa.Checked = CurGame.Missions.Contains(MissionsType.NhanQuaBuiHoaHong);
            muNhanHoaHongLo.Checked = CurGame.Missions.Contains(MissionsType.NhanHoaHongLo);
            muBHD.Checked = muTrongHoa.Checked || muBonHoa.Checked || muThuHoach.Checked || muTuoiHoaHong.Checked || muNhanQuaBuiHoa.Checked || muNhanHoaHongLo.Checked;
            muNhanDoi.Checked = muNhanx2.Checked || muDongx2.Checked || muDiMuaNgua.Checked || muHuyVatPhamQuy.Checked;
        }




        private void muTachTrangBiPet_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game =>
            {
                if (!muPhanGiaiTrangBijPet.Checked)
                    game.PushMissions(MissionsType.PhanGiaiTrangBiPet);
                else
                    game.RemoveMission(MissionsType.PhanGiaiTrangBiPet);
            });
        }

        private void muVanMay_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game =>
            {
                game.PushMissions(MissionsType.ThuTaiVanMay, !muVanMay.Checked);
            });
        }

        private void muTueHong_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game =>
            {
                game.PushMissions(MissionsType.ThienLongTueHong, !muTueHong.Checked);
            });
        }



        private void muNguyenVong_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game =>
            {
                game.PushMissions(MissionsType.NguyenVongThienLinh, !muNguyenVong.Checked);
            });
        }



        private void btnPk_Click(object sender, EventArgs e)
        {
            CreateControl(new TuyenChien());
        }

        private void chkAutoPK_CheckedChanged(object sender, EventArgs e)
        {
            if (IsHoldForDownSettingEx)
                return;
            SelectedOnlinedGames.ForEach(game => game.IsAskPk = checkTuyenChien.Checked);

        }


        private void luônỞTrênToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TopMost = muTopMost.Checked = !muTopMost.Checked;
        }






        private void muNhanx2_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game => game.PushMissions(MissionsType.NhanX2, !muNhanx2.Checked));
        }

        private void muDongx2_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game => game.PushMissions(MissionsType.DongX2, !muDongx2.Checked));
        }

        private void muDiMuaNgua_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game =>
            {
                game.PushMissions(MissionsType.DiMuaNga, !muDiMuaNgua.Checked);
            });
        }

        private void muHuyVatPhamQuy_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game =>
            {
                game.PushMissions(MissionsType.HuyVatPhamQuy, !muHuyVatPhamQuy.Checked);
            });
        }




        private void q1TôChâuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool isdi = !muQ1ToChau.Checked;
            SelectedOnlineLeaders.ForEach(game =>
            {
                if (isdi)
                {
                    game.Q12TK = 1;
                }
                else
                {
                    game.Q12TK = 0;
                }
            });
        }

        private void q2TôChâuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool isdi = !muQ2ToChau.Checked;
            SelectedOnlineLeaders.ForEach(game =>
            {
                if (isdi)
                {
                    game.Q12TK = 2;
                }
                else
                {
                    game.Q12TK = 0;
                }
            });
        }

        private void q12TôChâuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool isdi = !muQ12ToChau.Checked;
            SelectedOnlineLeaders.ForEach(game =>
            {
                if (isdi)
                {
                    game.Q12TK = 3;
                }
                else
                {
                    game.Q12TK = 0;
                }
            });
        }

        private void thủyLaoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool isdi = !muThuyLao.Checked;
            SelectedOnlineLeaders.ForEach(game =>
            {
                if (isdi)
                {
                    game.IsThuyLao = true;
                }
                else
                {
                    game.IsThuyLao = false;
                }
            });
        }

        private void muCQThuCong_Click(object sender, EventArgs e)
        {
            btnCanQuetThuCong_Click(null, null);
        }

        private void muCQBinhThanhThuCong_Click(object sender, EventArgs e)
        {
            binhthanhHoTro(null, null);
        }

        private void muCanQuet_Click(object sender, EventArgs e)
        {
            int idx = TDT.ParseInt((sender as ToolStripMenuItem).Tag.ToString());
            SelectedOnlinedGames.ForEach(game => game.CanQuetIdx = idx);
        }


        private void muTrongHoa_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game =>
            {
                game.PushMissions(MissionsType.TrongHoa, !muTrongHoa.Checked);
            });
        }

        private void muBonHoa_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game =>
            {
                game.PushMissions(MissionsType.BonHoa, !muBonHoa.Checked);
            });
        }

        private void muThuHoach_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game =>
            {
                game.IsThuHoachHoaEx = !muThuHoach.Checked;
                game.SaveSetting();
            });
        }

        private void muTuoiHoaHong_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game =>
            {
                game.IsTuoiHoa = !muTuoiHoaHong.Checked;
            });
        }

        private void muNhanQuaBuiHoa_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game =>
            {
                game.PushMissions(MissionsType.NhanQuaBuiHoaHong, !muNhanQuaBuiHoa.Checked);
            });
        }

        private void muNhanHoaHongLo_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game =>
            {
                game.PushMissions(MissionsType.NhanHoaHongLo, !muNhanHoaHongLo.Checked);
            });
        }

        private void featureToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            //muNotUseShortcutKey.Checked = IsDisableKey;
        }

        private void lblBeri_Click(object sender, EventArgs e)
        {
            LoadUserInfo();
        }

        private void menuTHDC1Bai1_Click_1(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game => game.NeedToMove = "187,113," + MAP.TanHoangDiaCungTang1);
        }

        private void menuTHDC1Bai2_Click_1(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game => game.NeedToMove = "114,122," + MAP.TanHoangDiaCungTang1);
        }

        private void menuTHDC1Bai3_Click_1(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game => game.NeedToMove = "27,124," + MAP.TanHoangDiaCungTang1);
        }

        private void menuTHDC2Bai1_Click_1(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game => game.NeedToMove = "223,225," + MAP.TanHoangDiaCungTang2);
        }

        private void menuTHDC2Bai2_Click_1(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game => game.NeedToMove = "134,41," + MAP.TanHoangDiaCungTang2);
        }

        private void menuTHDC3_Click_1(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game => game.NeedToMove = "224,216," + MAP.TanHoangDiaCungTang3);
        }

        private void menuTHDC4_Click_1(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game => game.NeedToMove = "31,33," + MAP.TanHoangDiaCungTang4);
        }

        private void hoàngLongĐộngTầng2ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game => game.NeedToMove = "216,156," + MAP.HoangLongDong);
        }

        private void muQuayVeDaiLy_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game =>
            {
                game.UseSkill(22);
            });
        }

        private void muXuatPet_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game =>
            {
                game.DoAction("PetSkill2_1");
            });
        }

        private void muThuPet_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game =>
            {
                game.DoAction("PetSkill2_2");
            });
        }

        private void muLenNgua_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game =>
            {
                game.UpRide();
            });
        }

        private void muXuongNgua_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game =>
            {
                game.DownRide();
            });
        }

        private void muAnDon_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game =>
            {
                game.DoAction("FightSkillXinShou_1");
            });
        }

        private void muDemBut_Click(object sender, EventArgs e)
        {
            int cntgame = 0;
            int cntbut = 0;
            foreach (var game in SelectedOnlinedGames)
            {
                cntgame++;
                foreach (var item in game.PacketItems.All.Concat(game.PacketItems.ThienCo))
                {
                    if (item.ClearName == "chucphucmaobut")
                    {
                        cntbut += (int)item.Count;
                    }
                }
            }
            string msg = "Có " + cntgame + " game có " + cntbut + " bút";
            MessageBox.Show(this, msg, "MicroAuto", MessageBoxButtons.OK);
        }

        private void muChucPhuc_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => { g.PushMissions(MissionsType.ChucPhucMaoBut, !muChucPhuc.Checked); });
        }

        private void muKetNghia_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => { g.KetNghia(SelectedOnlinedGames.ToList()); });
        }

        private void muChucPhucTuBaoBon_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game =>
            {
                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    game.ChucPhucHaoHuu();
                }).Start();
            });
        }

        private void muAutoMap_Click(object sender, EventArgs e)
        {
            if (CurGame != null)
            {
                var control = new AutoMap(CurGame) { Dock = DockStyle.Fill };

                TabPage page = new TabPage("AutoMap");
                tabCanQuet.TabPages.Add(page);

                page.Controls.Add(control);

                control.Disposed += (ss, ee) =>
                {
                    page.Dispose();
                };

                tabCanQuet.Visible = true;
                tabCanQuet.BringToFront();
                tabCanQuet.SelectedTab = page;

            }
        }

        private void muChonMayChu_Click(object sender, EventArgs e)
        {
            foreach (Game game in SelectedGames)
            {
                game.Quit();
            }
        }

        private void muNhapCode_Click(object sender, EventArgs e)
        {
            new Code().Show(this);
        }


        private void ẩnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Thread(new ThreadStart(HideAllGame))
            {
                IsBackground = true
            }.Start();
        }

        private void hiệnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Thread(new ThreadStart(ShowAllGame))
            {
                IsBackground = true
            }.Start();
        }

        private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Bạn có muốn thoát " + " toàn bộ " + " Game", "MicroAuto", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            try
            {
                foreach (ListViewItem it in Lv.Items)
                {
                    var game = it.Tag as Game;
                    new Thread(() =>
                    {
                        Thread.CurrentThread.IsBackground = true;
                        Instance.Invoke(new Action(() =>
                        {
                            game.Exit();
                        }));

                    }).Start();
                }
            }
            catch { }
        }

        private void refreshToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            foreach (var game in AllOnelineGame.ToList())
            {
                Refresh(game);
            }
        }




        private void ẩnDanhHiệuToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AllOnelineGame.ToList().ForEach(game => { game.DoStringEx("nAgnameNum = Player:GetAgnameNum(); if nAgnameNum > 0 then setmetatable(_G, { __index = Agname_Env}); Player:SetNullAgname(); end for i=1, 10 do if Pet:IsPresent(i-1) then if Pet:GetIsFighting(i-1) then  setmetatable(_G, {__index = PetAgname_Env}); num = Pet:GetTitleNum(i-1);  if num > 0 then  Pet:SetNullCurTitle(i-1);  end end end end"); });
        }

        private void lấyĐồToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void muaTưBổĐanNếuChưaCóToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            foreach (Game game in SelectedOnlinedGames)
            {
                game.MuaTuBoDanChuaCo();
            }
        }

        private void hủyNhiệmVụThấtBạiToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AllOnelineGame.ToList().ForEach(g => g.DeleteFail());
        }

        private void mởBáchBảoCácToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void sắpXếpTayNảiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AllOnelineGame.ToList().ForEach(g => g.LUA.PackUp());
        }

        private void tạmDừngPauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Paused();
        }

        private void tắtSkillToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PausedSkill = menuPausedSkill.Checked = !menuPausedSkill.Checked;
        }



        private void nhặtĐồToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Global.IsPickItem = menuPickItem.Checked = !menuPickItem.Checked;
        }

        private void lọcĐồToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Global.ItemFillter = menuItemFillter.Checked = !menuItemFillter.Checked;
        }

        private void buộcTheoSauKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Follow = menuForceFollow.Checked = !menuForceFollow.Checked;
        }

        private void đánhTheoKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Global.AtkFollowKey = menuAtkFollowKey.Checked = !menuAtkFollowKey.Checked;
        }

        private void theoSauKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Global.FollowKey = menuFollowKey.Checked = !menuFollowKey.Checked;
        }

        private void autoPKToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AutoPK == null || AutoPK.IsDisposed)
            {
                AutoPK = new AutoPK();
                AutoPK.Show();
                Win.Active(AutoPK);
            }
        }

        private void hỗTrợPKToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Pk.Visible)
            {
                Pk = new Pk();
                Pk.Show(this);
            }
        }



        private void điểmDanhCtrlPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiemDanh();
        }

        private void resetTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in Lv.Items)
            {
                Game game = (Game)item.Tag;
                if (!game.TLBB.Online)
                    continue;
                if (game.TLBB.IsLeader)
                {
                    game.IsLostLeader = true;
                }
                if (game.TLBB.PlayerState == 10)
                    game.IsOpenShop = true;
                game.IsOpenPass2 = false;
                game.ReConnect();
            }
        }



        private void đầuThaiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AllOnelineGame.ToList().ForEach(game => { if (game.TLBB.PlayerState == 9) game.LUA.OutGhost(); });
        }

        private void selectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in Lv.Items)
            {
                item.Selected = true;
            }
        }

        private void chkShutDown_CheckedChanged(object sender, EventArgs e)
        {
            //tmrShutDown.Enabled = chkShutDown.Checked;
        }

        private void nhậnQuàThăngCấpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game => game.NhanQuaThangCap());
        }

        private void lênNgựaToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            AllOnelineGame.ToList().ForEach(game =>
            {
                game.UpRide();
            });
        }

        private void xuốngNgựaToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            AllOnelineGame.ToList().ForEach(game =>
            {
                game.DownRide();
            });
        }


        private void thuPetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AllOnelineGame.ToList().ForEach(game =>
            {
                game.DoAction("PetSkill2_2");
            });
        }

        private void xuấtPetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AllOnelineGame.ToList().ForEach(game =>
            {
                game.DoAction("PetSkill2_1");
            });
        }

        private void txtSearchName_TextChanged(object sender, EventArgs e)
        {
            listViewSkill.Search(txtSearchName.Text);
        }

        private void muTrimRam_Click(object sender, EventArgs e)
        {
            muTrimRam.Checked = TrimRam = !muTrimRam.Checked;
        }

        private void muTrimRam_DropDownOpening(object sender, EventArgs e)
        {
            muTrimRam.Checked = TrimRam;
        }


        private void tmrLoadSkill_Tick(object sender, EventArgs e)
        {

            IsHoldForDownSettingEx = true;
            try
            {
                //lblExp.Text = CurGame.ExpInfo;
                //lbHoaSpeed.Text = CurGame.HoaSpeed;
                if (CurGame.RadiusX > 0)
                    lblRadius.Text = "[" + (int)CurGame.RadiusX + "," + (int)CurGame.RadiusY + "] (" + CurGame.Radius + ")";
                else
                    lblRadius.Text = "[0,0] (0)";
                DownLoead();
                listViewSkill.SetNull();
                listViewSkill.ItemChecked -= new ItemCheckedEventHandler(this.listViewSkill_ItemChecked);
                listViewSkill.ListViewItemSorter = null;
                listViewSkill.Items.Clear();
                List<ListViewItem> list = new List<ListViewItem>();
                foreach (Skill skill in CurGame.Skills)
                {
                    if (skill.Name != "")
                    {
                        ListViewItem item = new ListViewItem(skill.Name);
                        item.Tag = skill;
                        item.Text = skill.Name;
                        item.SubItems.Add(skill.UsePK ? "X" : "");
                        item.Checked = skill.Use;
                        list.Add(item);
                    }
                }
                listViewSkill.Items.AddRange(list.ToArray());
                listViewSkill.ListViewItemSorter = new TaskListComparer();
                listViewSkill.Sort();
                listViewSkill.ItemChecked += new ItemCheckedEventHandler(this.listViewSkill_ItemChecked);

                if (CurGame != null)
                {
                    lblTrain.Text = "" + CurGame.ToaDoTrainEx;
                    var game = CurGame;
                    //chkDaily.Checked = game.IsLyHoa || game.IsNguHanhPhap || game.IsVanMay || game.IsNguyenVong;
                }
                if (CurGame == null)
                    return;
                try
                {
                    lvConfig.Items[0].Checked = CurGame.IsPickItem;
                    lvConfig.Items[1].Checked = CurGame.IsNhatHop;
                    lvConfig.Items[2].Checked = CurGame.IsNhatTuyet;
                    lvConfig.Items[3].Checked = CurGame.AutoResetTime;
                    lvConfig.Items[4].Checked = CurGame.IsX4;
                    lvConfig.Items[5].Checked = CurGame.Isx2VoY;


                    for (int i = 0; i < 22; i++)
                    {
                        lvConfig.Items[i + 6].SubItems[1].Text = CurGame.KeyDelay[i].ToString();
                    }
                    for (int i = 0; i < 12; i++)
                    {
                        lvConfig.Items[i + 6].Checked = CurGame.F[i];
                    }
                    for (int i = 0; i < 9; i++)
                    {
                        lvConfig.Items[i + 18].Checked = CurGame.Alt[i + 1];
                    }
                    lvConfig.Items[27].Checked = CurGame.Alt[0];


                    checkChiDanh.Checked = CurGame.IsOnlyAttack;
                    checkChiNhat.Checked = CurGame.IsOnlyPick;
                    checkQuyCoc.Checked = CurGame.IsQuyCoc;
                    checkMuaKNB.Checked = CurGame.Missions.Contains(MissionsType.AutoBuyKNB);
                    checkChetLenBai.Checked = CurGame.IsAutoComeBack;
                    //chkOptLocDo.Checked = CurGame.IsFilter;
                    txtPass2.Text = CurGame.Pass2;
                    //tab.TabPages[0].Text = CurGame.TLBB.Name;

                    checkTuyenChien.Checked = CurGame.IsAskPk;

                    checkTimQuai.Checked = CurGame.IsAttack;
                    checkLuaQuai.Checked = CurGame.IsLure;
                    checkPet.Checked = CurGame.IsPet;
                    checkHP.Checked = CurGame.IsHP;
                    checkMP.Checked = CurGame.IsMP;
                    checkNM.Checked = CurGame.IsNM;
                    chkBinhThanh.Checked = CurGame.Missions.Contains(MissionsType.DatDoiBinhThanh);
                    //chkRadius.Checked = CurGame.IsRadius;
                    //chkExitHPLow.Checked = CurGame.ExitHPLow;

                    //chkNhatQDua.Checked = CurGame.IsNhatHopQDua;

                    if (CurGame.TLBB.IsNgoaiCong)
                        nudRadius.Value = Global.NgoaiRadius;
                    else
                        nudRadius.Value = Global.NoiRadius;
                    chkRaoVat.Checked = CurGame.IsRao;
                    txtRao.Text = CurGame.RaoTxt;
                    checkKhaiKhoang.Checked = CurGame.Missions.Contains(MissionsType.KhaiKhoang);
                    checkHaiDuoc.Checked = CurGame.Missions.Contains(MissionsType.HaiDuoc);
                    checkTrongTrot.Checked = CurGame.Missions.Contains(MissionsType.TrongTrot);
                    checkThuHoach.Checked = CurGame.IsThuHoach;
                    cboTrongTrot.SelectedIndex = CurGame.TrongTrotIndex;
                    cboThuHoach.SelectedIndex = CurGame.ThuHoachIndex;
                    checkXuatPet.Checked = CurGame.IsXuatPet;
                    cboXuatPet.Items.Clear();

                    ComboboxItem it = new ComboboxItem();
                    it.Text = "Không Xuất";
                    it.Value = 0;
                    cboXuatPet.Items.Add(it);
                    int index = -1;
                    int cnt = 0;
                    foreach (KeyValuePair<uint, string> kvp in CurGame.TLBB.DicPet)
                    {
                        cnt++;
                        ComboboxItem item = new ComboboxItem();
                        item.Text = kvp.Value;
                        item.Value = (int)kvp.Key;
                        if (kvp.Key.ToString("X8") == CurGame.PetId)
                        {
                            index = cnt;
                        }
                        cboXuatPet.Items.Add(item);
                    }
                    if (index != -1)
                        cboXuatPet.SelectedIndex = index;
                    cboLoai.SelectedIndex = CurGame.CheLoai;
                    cboCap.SelectedIndex = CurGame.CheCap;
                    if (CurGame.CheNoiNgoai == 1)
                    {
                        radNoi.Checked = true;
                    }
                    else
                    {
                        if (CurGame.CheNoiNgoai == 2)
                        {
                            radNgoai.Checked = true;
                        }
                        else
                        {
                            radNoiNgoai.Checked = true;
                        }
                    }
                    nudStar.Value = CurGame.CheSao;
                    nudLine.Value = CurGame.CheDong;
                    nudPoint.Value = CurGame.CheDiem;
                    chkStartCraft.Checked = CurGame.IsCheDo;
                    //lecaotri
                    //numericUpDown1.Value = Global.MinNv;
                    //chkHideLogin.Checked = Global.HideLogin;


                    //listViewMoveAround.Items.Clear();
                    //foreach (string s in Setting.MoveAround.Split(';'))
                    //{
                    //    if (s.Split(',').Length >= 3)
                    //    {
                    //        int map = TDT.ParseInt(s.Split(',')[0]);
                    //        if (map == CurGame.TLBB.MapId)
                    //        {
                    //            for (int i = 0; i < (s.Split(',').Length - 1) / 2; i++)
                    //            {
                    //                int x = TDT.ParseInt(s.Split(',')[i * 2 + 1]);
                    //                int y = TDT.ParseInt(s.Split(',')[i * 2 + 2]);
                    //                ListViewItem item = new ListViewItem(CurGame.TLBB.MapName + " [" + x + "," + y + "]");
                    //                item.Tag = x + "," + y;
                    //                listViewMoveAround.Items.Add(item);
                    //            }
                    //        }
                    //    }
                    //}
                    checkBanKinh.Checked = CurGame.RadiusX != 0;
                    numberThuHoaX.Value = CurGame.ThuHoaX;
                    numberRadiusThuHoa.Value = CurGame.RaDiusThuHoa;
                }
                catch
                {
                    //PushLogEx(ex.Message + ex.StackTrace);
                }


            }
            catch
            {

            }

            IsHoldForDownSettingEx = false;
            tmrLoadSkill.Stop();
        }



        private void showPageUơToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Activate();
            Win.Active(this.Handle);
        }




        private void tmrDownLead_Tick(object sender, EventArgs e)
        {
            DownLoead();
        }

        private void DownLoead()
        {
            IsHoldForDownSetting = true;
            if (CurGame != null)
            {
                chkBinhThanh.Checked = CurGame.Missions.Contains(MissionsType.DatDoiBinhThanh);
                chkKho.Checked = CurGame.Missions.Contains(MissionsType.BinhThanhKho);
            }
            else
            {
                chkBinhThanh.Checked = false;
                chkKho.Checked = false;
            }
            if (SelectedOnlineLeaders.Count() > 0)
            {
                var game = SelectedOnlineLeaders.FirstOrDefault();
                chkPhieuMieuPhong.Checked = game.Missions.Contains(MissionsType.DatDoiPhieuMieuPhong);
                chkHuyetChien.Checked = game.Missions.Contains(MissionsType.DatDoiKhieuChienPhieuMieuPhong);
                chkYenTuO.Checked = game.Missions.Contains(MissionsType.DatDoiYenTuO);
                chkTuTuyetTrang.Checked = game.Missions.Contains(MissionsType.DatDoiTuTuyetTrang);
                chkThieuThatSon.Checked = game.Missions.Contains(MissionsType.DatDoiThieuThatSon);
                chkSatTinh.Checked = game.Missions.Contains(MissionsType.DatDoiSatTinh);
                chkQLauLan.Checked = game.Missions.Contains(MissionsType.DatDoiQ123LauLan);
                chkQToChau.Checked = game.Missions.Contains(MissionsType.DatDoiQ123ToChau);
                chkPhucDia.Checked = game.Missions.Contains(MissionsType.DatDoiPhucDia);
                chkPDKho.Checked = game.Missions.Contains(MissionsType.DatDoiPhucDia) && game.Missions.Contains(MissionsType.PhucDiaKho);
                chkVuongLang.Checked = game.Missions.Contains(MissionsType.DatDoiVuongLang);
                chkTamThan.Checked = game.Missions.Contains(MissionsType.DatDoiTamThan);
            }
            else
            {
                chkPhieuMieuPhong.Checked = false;
                chkHuyetChien.Checked = false;
                chkYenTuO.Checked = false;
                chkTuTuyetTrang.Checked = false;
                chkThieuThatSon.Checked = false;
                chkSatTinh.Checked = false;
                chkQLauLan.Checked = false;
                chkQToChau.Checked = false;
                chkPhucDia.Checked = false;
                chkPDKho.Checked = false;
                chkVuongLang.Checked = false;
                chkTamThan.Checked = false;
            }
            IsHoldForDownSetting = false;
        }

        private void chkSatTinh_CheckedChanged(object sender, EventArgs e)
        {
            if (IsHoldForDownSetting)
                return;
            SelectedOnlineLeaders.ForEach(g => g.PushMissions(MissionsType.DatDoiSatTinh, chkSatTinh.Checked));
        }

        private void chkPhieuMieuPhong_CheckedChanged(object sender, EventArgs e)
        {
            if (IsHoldForDownSetting)
                return;
            SelectedOnlineLeaders.ForEach(g => g.PushMissions(MissionsType.DatDoiPhieuMieuPhong, chkPhieuMieuPhong.Checked));
        }

        private void chkHuyetChien_CheckedChanged(object sender, EventArgs e)
        {
            if (IsHoldForDownSetting)
                return;
            SelectedOnlineLeaders.ForEach(g => g.PushMissions(MissionsType.DatDoiKhieuChienPhieuMieuPhong, chkHuyetChien.Checked));
        }

        private void chkYenTuO_CheckedChanged(object sender, EventArgs e)
        {
            if (IsHoldForDownSetting)
                return;
            SelectedOnlineLeaders.ForEach(g => g.PushMissions(MissionsType.DatDoiYenTuO, chkYenTuO.Checked));
        }

        private void chkTuTuyetTrang_CheckedChanged(object sender, EventArgs e)
        {
            if (IsHoldForDownSetting)
                return;
            SelectedOnlineLeaders.ForEach(g => { g.PushMissions(MissionsType.DatDoiTuTuyetTrang, chkTuTuyetTrang.Checked); });
        }

        private void chkThieuThatSon_CheckedChanged(object sender, EventArgs e)
        {
            if (IsHoldForDownSetting)
                return;
            SelectedOnlineLeaders.ForEach(g => { g.PushMissions(MissionsType.DatDoiThieuThatSon, chkThieuThatSon.Checked); });
        }



        private void chkPhucDia_CheckedChanged(object sender, EventArgs e)
        {
            if (IsHoldForDownSetting)
                return;
            SelectedOnlineLeaders.ForEach(g => { g.PushMissions(MissionsType.DatDoiPhucDia, chkPhucDia.Checked); });
        }

        private void chkPDKho_CheckedChanged(object sender, EventArgs e)
        {
            if (IsHoldForDownSetting)
                return;
            chkPhucDia.Checked = true;
            SelectedOnlineLeaders.ForEach(g => { g.PushMissions(MissionsType.PhucDiaKho, chkPDKho.Checked); });
        }

        private void chkQLauLan_CheckedChanged(object sender, EventArgs e)
        {
            if (IsHoldForDownSetting)
                return;
            SelectedOnlineLeaders.ForEach(g => { g.PushMissions(MissionsType.DatDoiQ123LauLan, chkQLauLan.Checked); });
        }

        private void chkQToChau_CheckedChanged(object sender, EventArgs e)
        {
            if (IsHoldForDownSetting)
                return;
            SelectedOnlineLeaders.ForEach(g => { g.PushMissions(MissionsType.DatDoiQ123ToChau, chkQToChau.Checked); });
        }

        private void chkVuongLang_CheckedChanged(object sender, EventArgs e)
        {
            if (IsHoldForDownSetting)
                return;
            SelectedOnlineLeaders.ForEach(g => { g.PushMissions(MissionsType.DatDoiVuongLang, chkVuongLang.Checked); });
        }

        private void chkTamThan_CheckedChanged(object sender, EventArgs e)
        {
            if (IsHoldForDownSetting)
                return;
            SelectedOnlineLeaders.ForEach(g => { g.PushMissions(MissionsType.DatDoiTamThan, chkTamThan.Checked); });
        }


        private void muPhieuMieuPhong_Click(object sender, EventArgs e)
        {

        }

        private void thảThiểmĐiệnĐiêuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => g.ThaThiemDienDieu());
        }

        private void picBeri_ChangeUICues(object sender, UICuesEventArgs e)
        {

        }

        private void đủ12ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 12 - DicGame.Count; i++)
                OpenGames();
        }


        private void muForceFollow_Click(object sender, EventArgs e)
        {
            Global.ForceFollowKey = muForceFollow.Checked = !muForceFollow.Checked;
        }

        private void phúcLợiĐiểmDanhToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game =>
            {
                game.phucloi();
            });
        }


        private void traderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurGame != null)
                new Trader(CurGame).Show(this);
        }



        private void muDoanBaoMaTac_Click(object sender, EventArgs e)
        {
            SelectedOnlineLeaders.ForEach(g => { g.PushMissions(MissionsType.DatDoiMaTac, !muDoanBaoMaTac.Checked); });
        }





        public static bool IsMute { get; set; }

        private void muMute_Click(object sender, EventArgs e)
        {
            IsMute = muMute.Checked = !muMute.Checked;
        }

        private void đạiThếGiớiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FastMove().Show(this);
        }

        private void kếtNghĩaToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (User.Email == "hungcatpth0802@gmail.com" || User.Email == "lecaotri@yahoo.com")
            {
                SelectedOnlineLeaders.ForEach(g => { g.IsKetNghia = !kếtNghĩaToolStripMenuItem.Checked; });
            }
        }

        private void địnhVịThổLinhChâuALLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var game in SelectedOnlinedGames)
            {
                Task.Run(() =>
                {
                    game.DinhViThoLinhChau(true);
                });
            }
        }

        private void mnuKetNghia_Click(object sender, EventArgs e)
        {
            if (Lv.SelectedItems.Count > 1)
            {
                tmrCreateTeam.Start();
                CreateTeamSate = STATE.None;

                Global.lstketban.Clear();
                foreach (ListViewItem item in Lv.SelectedItems)
                {
                    Game game = (Game)item.Tag;
                    mnuKetNghia.Checked = game.IsKetBai = !game.IsKetBai;

                    game.TrangThaiNhiemVuKetBai = "";
                    Global.lstketban.Add(game.TLBB.Name);
                }
                return;
            }
        }



        private void mnuKeBaiSudo_Click(object sender, EventArgs e)
        {

            if (Lv.SelectedItems.Count > 1)
            {
                tmrCreateTeam.Start();
                CreateTeamSate = STATE.None;

                Global.lstsudo.Clear();
                foreach (ListViewItem item in Lv.SelectedItems)
                {
                    Game game = (Game)item.Tag;
                    mnuKeBaiSudo.Checked = game.IsSuDo = !game.IsSuDo;

                    game.TrangThaiNhiemVuSuDo = "";
                    Global.lstsudo.Add(game.TLBB.Name);
                }
                return;
            }
        }

        private void mnuQuaSuDo_Click(object sender, EventArgs e)
        {
            if (User.Email == "xuanyen1112@gmail.com" || User.Email == "lecaotri@yahoo.com")
            {
                SelectedOnlinedGames.ForEach(game =>
                {
                    game.NhanQuaSuDo();
                });
            }
        }

        private void muaX2ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void trmdichtheokey_Tick(object sender, EventArgs e)
        {

        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var game in SelectedOnlinedGames)
            {
                Refresh(game);
            }
        }




        private void dùngĐịnhVịPhùToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in Lv.SelectedItems)
            {
                var game = item.Tag as Game;
                game.PushMissions(MissionsType.DungDinhViPhu);
            }
        }

        private void configToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateControl(new ConfigBossMap());
        }

        private void Config_Disposed(object sender, EventArgs e)
        {
            try
            {
                bool isdis = true;
                do
                {
                    isdis = false;
                    foreach (ToolStripItem item in bossMapToolStripMenuItem.DropDownItems)
                    {
                        if (item.Tag != null && !string.IsNullOrEmpty(item.Tag.ToString().Trim()))
                        {
                            item.Dispose();
                            isdis = true;
                            break;
                        }
                    }
                }
                while (isdis);
                List<ToolStripMenuItem> lst = new List<ToolStripMenuItem>();
                string[] map = Game.IniParser.EnumSection("BossMap");
                foreach (string line in map)
                {
                    int mapId = -1;
                    bool isnum = int.TryParse(line, out mapId);
                    if (isnum)
                    {
                        if (GAMEDIC.MapNameId.ContainsKey(mapId))
                        {
                            string nicename = GAMEDIC.MapNameId[mapId];
                            ToolStripMenuItem itemBossMap = new ToolStripMenuItem();
                            itemBossMap.Text = nicename;
                            itemBossMap.Tag = mapId;
                            itemBossMap.Click += ItemBossMap_Click;
                            lst.Add(itemBossMap);

                        }

                    }
                    else
                    {

                    }

                }
                if (lst.Count > 0)
                    bossMapToolStripMenuItem.DropDownItems.Add(new ToolStripSeparator() { Tag = "user" });
                bossMapToolStripMenuItem.DropDownItems.AddRange(lst.ToArray());
            }
            catch
            {
            }
        }

        private void ItemBossMap_Click(object sender, EventArgs e)
        {
            SelectedOnlineLeaders.ForEach(g => { g.PushMissions(MissionsType.DatDoiBossMap, !((ToolStripMenuItem)sender).Checked); g.CurBossMap = TDT.ParseAllInt(((ToolStripMenuItem)sender).Tag.ToString()); g.MoveIndex = 0; });
        }

        private void bossMapToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            foreach (Object it in bossMapToolStripMenuItem.DropDownItems)
            {
                if (typeof(ToolStripMenuItem) == it.GetType())
                {
                    var item = it as ToolStripMenuItem;
                    if (((ToolStripMenuItem)item).Tag != null)
                    {
                        if (CurGame != null && CurGame.CurBossMap == TDT.ParseAllInt(item.Tag.ToString()) && CurGame.Missions.Contains(MissionsType.DatDoiBossMap))
                        {
                            item.Checked = true;
                        }
                        else
                        {
                            item.Checked = false;
                        }
                    }
                }
            }
        }

        private void thầnKhí9ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g =>
            {
                if (g.PacketItems.TrangBi.Where(p => Game.Weapon.Contains(p.TypeName) && (TDT.ParseAllInt(p.Star.ToString().Substring(0, 1)) >= 8 || TDT.ParseAllInt(p.Star.ToString()) >= 80)).FirstOrDefault() != null)
                {
                    g.PushMissions(MissionsType.ThanKhi9Sao, !muThanKhi9Sao.Checked);
                }
                else
                {
                    g.RemoveMission(MissionsType.ThanKhi9Sao);
                    g.PushDebugMessage("Chưa có thần khí 8 sao. Dừng nhiệm vụ");
                }
            });
        }
        public static bool IsOnlyPlayer { get; set; }
        private void chỉĐánhNgườiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IsOnlyPlayer = chỉĐánhNgườiToolStripMenuItem.Checked = !chỉĐánhNgườiToolStripMenuItem.Checked;
        }

        private void uploadConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Việc này sẽ tải thiết lập auto trong máy bạn lên máy chủ\r\n=>Ngoại trừ danh sách Login\r\nBạn có đồng ý chứ", "MicroAuto", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                var poster = new Poster();
                poster.Url = "http://tieudattai.org/remlaw/userconfig.php?cmd=save&email=" + HttpUtility.UrlEncode(User.Email) + "&pass=" + HttpUtility.UrlEncode(User.Pass);
                poster.Data = Game.IniParser.ToString();
                poster.Completed += (ss, ee) =>
                {
                    MessageBox.Show(this, poster.Response, "MicroAuto", MessageBoxButtons.OK);
                };
                poster.Post();
            }
        }


     

        public static bool IsRegZing { get; set; }




        private void dọnRácToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateControl(new CleanTrash());
            Win.Active(this);
        }

        private void setNhómNhậnBóngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (Game game in SelectedOnlinedGames)
            {
                if (!dic.ContainsKey(game.TLBB.Id))
                {
                    dic.Add(game.TLBB.Id, game.TLBB.Name);
                }
            }
            if (dic.Count != 2)
            {
                MessageBox.Show(this, "Chỉ được chọn 2 nhân vật để set nhóm nhận bóng", "MicroAuto", MessageBoxButtons.OK);
            }
            int cnt = 1;
            bool isbreak = false;
            while (!isbreak)
            {
                isbreak = true;
                foreach (KeyValuePair<string, string> kvp in CalendarEx.Balls)
                {
                    string name = kvp.Key;
                    if (name == cnt.ToString())
                    {
                        cnt++;
                        isbreak = false;
                        break;
                    }
                }
            }
            if (!string.IsNullOrEmpty(cnt.ToString()) && !CalendarEx.Balls.ContainsKey(cnt.ToString()))
            {
                try
                {
                    XmlElement node = CalendarEx.XMLBalls.CreateElement("Ball");
                    XmlAttribute atb = CalendarEx.XMLBalls.CreateAttribute("Name");
                    atb.Value = cnt.ToString();
                    node.Attributes.Append(atb);
                    CalendarEx.XMLBalls.SelectSingleNode("/*").AppendChild(node);
                    foreach (KeyValuePair<string, string> kvp in dic)
                    {
                        string team = cnt.ToString();
                        XmlNode nodeteam = CalendarEx.XMLBalls.SelectSingleNode("//Balls/Ball[@Name=\"" + cnt.ToString() + "\"]");
                        if (nodeteam.Attributes["Member"] == null)
                        {
                            XmlAttribute at = CalendarEx.XMLBalls.CreateAttribute("Member");
                            at.Value = kvp.Key + "-" + kvp.Value;
                            nodeteam.Attributes.Append(at);

                        }
                        else
                        {
                            if (!nodeteam.Attributes["Member"].Value.Contains(kvp.Key))
                            {
                                nodeteam.Attributes["Member"].Value = nodeteam.Attributes["Member"].Value + ";" + kvp.Key + "-" + kvp.Value;
                                nodeteam.Attributes["Member"].Value = nodeteam.Attributes["Member"].Value.Trim(';');
                            }
                        }
                        CalendarEx.Balls[cnt.ToString()] = node.Attributes["Member"].Value;
                        foreach (XmlNode nu in CalendarEx.XMLBalls.SelectNodes("//Balls/Ball"))
                        {
                            if (nu.Attributes["Name"] == null)
                            {
                                nu.ParentNode.RemoveChild(nu);
                            }
                            else
                            {
                                if (nu.Attributes["Name"].Value == cnt.ToString())
                                    continue;
                                if (nu.Attributes["Member"] == null)
                                {
                                    CalendarEx.Balls.Remove(nu.Attributes["Name"].Value);
                                    nu.ParentNode.RemoveChild(nu);
                                }
                                else
                                {
                                    if (nu.Attributes["Member"].Value.Contains(kvp.Key + "-" + kvp.Value))
                                        nu.Attributes["Member"].Value = nu.Attributes["Member"].Value.Replace(kvp.Key + "-" + kvp.Value, "").Trim(';');
                                    if (nu.Attributes["Member"].Value.Trim() == string.Empty)
                                    {
                                        CalendarEx.Balls.Remove(nu.Attributes["Name"].Value);
                                        nu.ParentNode.RemoveChild(nu);
                                    }
                                }
                            }
                        }
                    }
                    Game.IniParser.Write("Item", "Balls", CalendarEx.XMLBalls.OuterXml);
                }
                catch (Exception ex)
                {
                    Main.PushLogEx(ex.Message);
                }
            }
            else
            {
                //Main.PushLog("clgt");
            }
        }





        private void menuDinhSan_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game => game.NeedToMove = ((ToolStripMenuItem)sender).Tag as string);
        }



        private void tuyệtGiaoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurGame != null)
            {
                var control = new TuyetGiao(CurGame) { Dock = DockStyle.Fill };

                TabPage page = new TabPage("TuyệtGiao");
                tabCanQuet.TabPages.Add(page);

                page.Controls.Add(control);

                control.Disposed += (ss, ee) =>
                {
                    page.Dispose();
                };

                tabCanQuet.Visible = true;
                tabCanQuet.BringToFront();
                tabCanQuet.SelectedTab = page;
            }
        }



        private void numBet_TextChanged(object sender, EventArgs e)
        {
            numBet.Text = TDT.FormatMoney(TDT.ParseAllInt(numBet.Text));
            numBet.Select(numBet.Text.Length, 0);
        }

        private void tmrPushCaptcha_Tick(object sender, EventArgs e)
        {
            if (Global.Captchas.Count > 0)
            {
                string push = "";
                foreach (KeyValuePair<string, string> kvp in Global.Captchas.ToList())
                {
                    push += kvp.Key + "|" + kvp.Value + "\n";
                }
                Global.Captchas.Clear();
                push = push.Trim();
                var poster = new Poster();
                poster.Url = "http://45.77.129.211/microauto/captcha.php?cmd=pushcap&email=" + HttpUtility.UrlEncode(User.Email) + "&pass=" + User.Pass;
                poster.Data = push;
                poster.IsGet = true;
                poster.AutoReconnect = true;
                poster.Post();
            }
        }

        private void dùngTànQuyểnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach((Action<Game>)(game =>
            {
                {
                    new Thread((ThreadStart)(() =>
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            int idx = j + 1;
                            Thread.CurrentThread.IsBackground = true;
                            uint count = 0;
                            for (int i = 0; i < 5; i++)
                            {
                                bool isuse = false;
                                foreach (var item in game.PacketItems.All)
                                {
                                    if (item.ClearName.Contains("tanquyen"))
                                    {
                                        item.Use();
                                        Thread.Sleep(500);
                                        game.DoStringEx("setmetatable(_G, {__index = Attainment_Env}); if this:IsVisible() then Attainment_ChooseAttr(" + (idx - 1) + "); end");
                                        Thread.Sleep(500);
                                        isuse = true;
                                        if (count != item.Count)
                                        {
                                            count = item.Count;
                                            i = 0;
                                        }
                                        break;
                                    }
                                }
                                if (!isuse)
                                {
                                    break;
                                }
                            }
                        }
                    })
                    ).Start();

                }
            }));
        }




        private void button11_Click(object sender, EventArgs e)
        {
            new BoQuaEx().Show(this);
        }
        public static bool IsAutoBlock => false;
        private void chkBlock_CheckedChanged(object sender, EventArgs e)
        {



            string block = "";

            foreach (string s in SettingOld.BoQuaEx.Split('\n'))
            {
                if (s.Trim().Length >= 3)
                {
                    block += s.Trim() + "#";
                }
            }
            if (block.Length > 3 && IsAutoBlock)
            {
                DicGame.ToList().ForEach(kvp => { var adr = kvp.Value.Memory.WriteString(block); kvp.Value.PostMessage(1, 988); kvp.Value.PostMessage(adr, 987); });
            }
            else
            {
                DicGame.ToList().ForEach(kvp => { kvp.Value.PostMessage(0, 988); });
            }
        }

        public static string ProfileName { get; set; } = "#";

        private void profileClick(object sender, EventArgs e)
        {
            ToolStripMenuItem menu = sender as ToolStripMenuItem;
            if (ProfileName != menu.Tag.ToString())
            {
                Task.Run(() =>
                {
                    try
                    {
                        MicroLogin.SaveXML();

                        SaveSetting();

                        ProfileName = menu.Tag.ToString();



                        SettingOld.SetNull();
                        User_Disposed(null, null);
                        DicGame.ToList().ForEach(kvp => kvp.Value.IsSettingLoaded = false);
                        TabLogin.List.Where(p => p.Parent.Text != "Offline").ToList().ForEach(p => { p.Parent.Dispose(); p.Dispose(); });
                        TabLogin.List = TabLogin.List.Where(p => p.Parent.Text == "Offline").ToList();

                        DicGame.ToList().ForEach(kvp => { kvp.Value.listCheckedOnline.Clear(); });
                        CalendarEx.LoadXML();
                    }
                    catch { }
                });

            }
        }

        private void quảnLýToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateControl(new Profile());
        }



        private void muProfile_DropDownOpening(object sender, EventArgs e)
        {
            foreach (var i in muProfile.DropDownItems)
            {
                try
                {
                    var ii = i as ToolStripMenuItem;
                    if (ProfileName == ii.Tag.ToString())
                    {
                        ii.Font = new Font(ii.Font, FontStyle.Bold);
                    }
                    else
                    {
                        ii.Font = new Font(ii.Font, FontStyle.Regular);
                    }
                    try
                    {
                        if (ii.Tag.ToString().Contains("#"))
                        {
                            ii.Text = ii.Tag.ToString() + " [" + Game.IniParser.Read("profile", ii.Tag.ToString()) + "]";
                        }

                    }
                    catch
                    {

                    }
                }
                catch
                {
                }
            }
        }



        private void textBoxEx2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string msg = textBoxEx2.Text = textBoxEx2.Text.Trim();
                if (msg.Length > 0)
                {
                    if (msg == "regzing")
                    {
                        IsRegZing = true;
                        return;
                    }
                    textBoxEx2.Text = "";
                    new Thread(() =>
                    {
                        Thread.CurrentThread.IsBackground = true;
                        try
                        {
                            ws.Send(msg);
                        }
                        catch { }

                    }).Start();


                }
            }
        }

        private void label7_MouseClick(object sender, MouseEventArgs e)
        {
            label7.Enabled = false;
            var poster = new Poster();
            poster.Url = "http://tieudattai.org/microauto/user.php?cmd=getold&time=" + HttpUtility.UrlEncode(oldestmsg);
            poster.Control = this;
            poster.Completed += (ss, ee) =>
            {
                string res = poster.Response;
                string time = res.Split('\n')[0];
                if (time.Length == 19)
                {
                    oldestmsg = time;
                }
                poster.Response = poster.Response.Replace(oldestmsg, "").Trim();

                string last = poster.Response + "\r\n" + ricChat.Text.Trim();
                ricChat.Text = string.Empty;
                ricChat.Text = last;
                StringExtensions.RichTextBoxChangeWordColor(ref ricChat, "#", ":", Color.BlueViolet);
                label7.Enabled = true;

            };
            poster.Post();
        }

        private void lvConfig_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (!IsHoldForDownSettingEx)
            {
                SelectedOnlinedGames.ForEach(game =>
                {
                    if (game != null)
                    {
                        int idx = e.Item.Index;
                        if (idx == 0) game.IsPickItem = e.Item.Checked;
                        if (idx == 1) game.IsNhatHop = e.Item.Checked;
                        if (idx == 2) game.IsNhatTuyet = e.Item.Checked;
                        if (idx == 3) game.AutoResetTime = e.Item.Checked;
                        if (idx == 4) game.IsX4 = e.Item.Checked;
                        if (idx == 5) game.Isx2VoY = e.Item.Checked;
                        for (int i = 0; i < 12; i++)
                        {
                            if (idx == i + 6) game.F[i] = e.Item.Checked;
                        }
                        for (int i = 0; i < 9; i++)
                        {
                            if (idx == i + 18) game.Alt[i + 1] = e.Item.Checked;
                        }
                        if (idx == 27)
                            game.Alt[0] = e.Item.Checked;
                        game.SaveSetting();
                    }
                });

            }
        }

        public static bool IsMuteChat { get; set; }

        private void label5_MouseClick(object sender, MouseEventArgs e)
        {
            IsMuteChat = !IsMuteChat;
            if (IsMuteChat)
            {
                label5.BackColor = Color.Red;
                label5.Text = "Mute";
            }
            else
            {
                label5.BackColor = Color.Green;
                label5.Text = "Ting";
            }
            //ChatLogs.
        }






        public static bool IsMuteInbox { get; set; }






        void LoadAccountInfo()
        {
            Enabled = false;
            Poster posterBuy = new Poster();
            posterBuy.Url = URL.HomePage + "microauto/user.php";
            posterBuy.Data = "cmd=accountinfo&email=" + HttpUtility.UrlEncode(User.Email) + "&pass=" + HttpUtility.UrlEncode(User.Pass);
            posterBuy.Control = this;
            posterBuy.AutoReconnect = true;
            posterBuy.Completed += (ss, ee) =>
            {
                lblEmail.Text = User.Email;
                Enabled = true;
                var res = posterBuy.Response;
                lblBankNote.Text = "rem" + res.Split('|')[0] + "beli";
                lblBeli.Text = TDT.FormatMoney(res.Split('|')[1].ToInt()) + " Beli";
                lblVND.Text = TDT.FormatMoney(res.Split('|')[2].ToInt()) + " VND";
                lblCaptcha.Text = TDT.FormatMoney(res.Split('|')[3].ToInt()) + " Captcha";
                lblExpDate.Text = TDT.FormatMoney(res.Split('|')[4].ToIntEx() / (3600 * 24)) + " Day";

                lblHelp.Text = @"Tài khoản của bạn sẽ được + tiền sau khoảng 1 phút
Trong đó rem" + res.Split('|')[0] + "beli là ID auto của bạn";
            };
            posterBuy.Post();
        }

        private void nudThuHoaX_ValueChanged(object sender, EventArgs e)
        {
            if (!IsHoldForDownSettingEx)
            {
                CurGame.ThuHoaX = (int)numberThuHoaX.Value;
            }
        }

        private void nudRadiusThuHoa_ValueChanged(object sender, EventArgs e)
        {
            if (!IsHoldForDownSettingEx)
            {
                CurGame.RaDiusThuHoa = (int)numberRadiusThuHoa.Value;
            }
        }

        private void lbHoaSpeed_MouseClick(object sender, MouseEventArgs e)
        {
            //if(e.Button == MouseButtons.Left)
            //{
            //    AllOnelineGame.ForEach(g => g.ResetHoaSpee());
            //}
        }

        private void tabPage8_Click_1(object sender, EventArgs e)
        {

        }

        public int DelayBienThan => 10;


        private void tựĐộngToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SelectedOnlineLeaders.ForEach(g => g.RandomMapTangKinhCac());
        }

        private void tựĐộngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlineLeaders.ForEach(g => g.RandomMapAcTac());
        }

        private void txtMainSearch_TextChanged(object sender, EventArgs e)
        {
            Lv.Search(txtMainSearch.Text);
        }

        private void thanhLýNhiệmVụToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => g.PushMissions(MissionsType.ThanhLyNhiemVu, !muThanhLyNhiemVu.Checked));
        }




        private void toànBộPhụBảnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => g.CanQuetAll());
        }

        private void chatLogsToolStripMenuItem_Click_1(object sender, EventArgs e)
        {

        }

        private void bánRácCấtĐồToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game =>
            {
                game.PushMissions(new[] { MissionsType.BanRac, MissionsType.CatVang, MissionsType.CatDo });
            });
        }

        private void btnCityShop_Click(object sender, EventArgs e)
        {
            //new BachBaoCac().Show(this);
        }



        public static bool ShowIfNeed { get; set; }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            //ShowIfNeed = chkShowIfNeed.Checked;
        }

        private void lấyĐồTừThươngKhốToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void hủyBiếnThânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AllOnelineGame.ToList().ForEach(g =>
            {
                if (g.TLBB.MapId != MAP.DaiLy)
                {
                    g.HuyBienThan();
                }
            });
        }

        private void túcCầuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlineLeaders.ForEach(g => { g.PushMissions(MissionsType.DatDoiTucCau, !muTucCau.Checked); });
        }

        List<int> hidecol = new List<int>();

        private void Lv_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            if (Lv.Columns[e.ColumnIndex].Width == 0)
            {
                if (hidecol.Contains(e.ColumnIndex))
                {
                    e.Cancel = true;
                    e.NewWidth = Lv.Columns[e.ColumnIndex].Width;
                }
            }
            //if (e.ColumnIndex == 9 || e.ColumnIndex == 10)
            //{
            //    e.Cancel = true;
            //    e.NewWidth = Lv.Columns[e.ColumnIndex].Width;
            //}
        }

        private void selectPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Game.exe |Game.exe";
            if (openFile.ShowDialog(this) == DialogResult.OK)
            {
                PATH.Game = openFile.FileName;
            }
        }

        private void downloadConfigToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Việc này sẽ ghi đè toàn bộ thiết lập cũ có trong máy\r\nBạn có đồng ý chứ", "MicroAuto", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                var poster = new Poster();
                poster.Url = "http://tieudattai.org/remlaw/userconfig.php?cmd=load&email=" + HttpUtility.UrlEncode(User.Email) + "&pass=" + HttpUtility.UrlEncode(User.Pass);
                poster.Control = this;
                poster.Completed += (ss, ee) =>
                {
                    if (poster.Response == string.Empty)
                    {
                        MessageBox.Show(this, "Thiết Lập Rỗng!!!", "MicroAuto", MessageBoxButtons.OK);
                    }
                    else
                    {
                        Game.IniParser = new IniParser(poster.Response);

                        SettingOld.SetNull();

                        User_Disposed(null, null);

                        DicGame.ToList().ForEach(kvp => kvp.Value.IsSettingLoaded = false);


                        CalendarEx.LoadXML();

                        MessageBox.Show(this, "Tải thiết lập từ máy chủ về thành công!!!", "MicroAuto", MessageBoxButtons.OK);
                    }
                };
                poster.Post();
            }
        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void bánĐồCấtĐồTrịLiệuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AllOnelineGame.ToList().ForEach(game =>
            {
                game.PushMissions(new[] { MissionsType.TriLieu, MissionsType.BanRac, MissionsType.CatVang, MissionsType.CatDo });
            });
        }




        private void button12_Click(object sender, EventArgs e)
        {
            //new SellItemPS().Show(this);
        }

        private void treoĐồThươngHộiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Global.PSShop)
                return;
            Dictionary<string, int> dicPrice = new Dictionary<string, int>();
            foreach (string s in SettingOld.SellItemPS.Split('\n'))
            {
                if (s.Trim().Length > 3 && s.Trim().Split('|').Length == 2)
                {
                    if (!dicPrice.ContainsKey(s.Trim().Split('|')[0]))
                    {
                        if (TDT.ParseAllInt(s.Trim().Split('|')[1]) > 0)
                            dicPrice.Add(s.Trim().Split('|')[0], TDT.ParseAllInt(s.Trim().Split('|')[1]));
                    }
                }
            }

            if (CurGame != null)
            {
                Task.Run(() =>
                {
                    foreach (PacketItem i in CurGame.PacketItems.ThuongHoi)
                    {
                        if (dicPrice.ContainsKey(i.Name))
                        {
                            int price = dicPrice[i.Name] * 100;
                            CurGame.DoSubAction(i.Type, i.PacketId);
                            Thread.Sleep(1000);
                            CurGame.DoStringEx("PlayerShop:UpStall('item'," + price * i.Count + ");");
                            Thread.Sleep(1000);
                            CurGame.PushDebugMessage("Đã treo " + i.Name + " giá " + dicPrice[i.Name] * i.Count);
                        }

                    }
                    CurGame.PushDebugMessage("Treo xong rổi nhá");
                });
            }
        }

        private void menuGoTrieuTap_DropDownOpening(object sender, EventArgs e)
        {
            menuGoTrieuTap.DropDownItems.Clear();

            foreach (var game in AllOnelineGame)
            {
                ToolStripMenuItem i = new ToolStripMenuItem(game.TLBB.Name);
                i.Click += menuDinhSan_Click;
                i.Tag = game.RoundX + "," + game.RoundY + "," + game.TLBB.MapId;
                menuGoTrieuTap.DropDownItems.Add(i);
            }
        }

        private void playerToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }


        private void nộpTửViHuyTinhToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach((Action<Game>)(g =>
            {
                if (g.PacketItems.All.Concat(g.PacketItems.ThienCo).Where(i => i.Name == "Tử Vi Huy Tinh").Select(i => (int)i.Count).Sum() >= 15)
                    g.PushMissions(MissionsType.NopTuViHuyTinh, !muNopTuViHuyTinh.Checked);
                else
                    g.PushDebugMessage("Không đủ 15 Tử Vi Huy Tinh. Dừng nộp");
            })
            );
        }




        private void báchBảoCácToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => g.PushMissions(MissionsType.MoShopBachBaoCac, !muMoShopBachBaoCac.Checked));
        }

        private void shopTrungĐôToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => g.PushMissions(MissionsType.MoShopTrungDo, !muMoShopTrungDo.Checked));
        }

        private void shopHùngBáToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => g.PushMissions(MissionsType.MoShopHungBa, !muMoShopHungBa.Checked));
        }

        private void hòaBìnhToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => g.DoStringEx("Player:ChangePVPMode(0);"));
        }

        private void thiệnÁcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => g.DoStringEx("Player:ChangePVPMode(2);"));
        }

        private void nhómToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => g.DoStringEx("Player:ChangePVPMode(3);"));
        }

        private void bangToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => g.DoStringEx("Player:ChangePVPMode(4);"));
        }

        private void quânĐoànToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => g.DoStringEx("Player:ChangePVPMode(5);"));
        }

        private void button14_Click(object sender, EventArgs e)
        {
            //wb.Navigate("http://tieudattai.org/Help.php?email=" + HttpUtility.UrlEncode(User.Email) + "&pass=" + HttpUtility.UrlEncode(User.Pass) + "&version=" + HttpUtility.UrlEncode(Global.Version));
        }

        private void nudBuffPet_ValueChanged(object sender, EventArgs e)
        {
            Global.BuffPetPercent = (int)nudBuffPet.Value;
        }

        private void toolStripTextBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void thăngCấpHàoHiệpẤnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g =>
            {
                Task.Run(() =>
                {
                    for (int i = 0; i < 10; i++)
                        g.DoStringEx("setmetatable(_G, {__index = SelfJunXian_Env}); SelfJunXian_EquipHXYLevelup(); ");

                });
            }
            );
        }

        private void treoShopTạpHóaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Global.PSShop)
                return;
            Dictionary<string, int> dicPrice = new Dictionary<string, int>();
            foreach (string s in SettingOld.SellItemPS.Split('\n'))
            {
                if (s.Trim().Length > 3 && s.Trim().Split('|').Length == 2)
                {
                    if (!dicPrice.ContainsKey(s.Trim().Split('|')[0]))
                    {
                        if (TDT.ParseAllInt(s.Trim().Split('|')[1]) > 0)
                            dicPrice.Add(s.Trim().Split('|')[0], TDT.ParseAllInt(s.Trim().Split('|')[1]));
                    }
                }
            }

            if (CurGame != null)
            {
                Task.Run((Action)(() =>
                {
                    foreach (var itemi in CurGame.PacketItems.All)
                    {
                        if (dicPrice.ContainsKey((string)itemi.Name))
                        {
                            int price = dicPrice[(string)itemi.Name] * 100;
                            itemi.DoSubAction();
                            Thread.Sleep(1000);
                            CurGame.DoStringEx((string)("StallSale:ReferItemPrice(" + price * itemi.Count + ");"));
                            Thread.Sleep(1000);
                            CurGame.PushDebugMessage((string)("Đã treo " + itemi.Name + " giá " + dicPrice[(string)itemi.Name] * itemi.Count));
                            CurGame.DoStringEx("setmetatable(_G, {__index = InputMoney_Env});  this:Hide();");
                            Thread.Sleep(350);
                        }

                    }
                    CurGame.PushDebugMessage("Treo xong rổi nhá");
                }));
            }
        }

        private void đổiKimTằmTyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => g.PushMissions(MissionsType.DoiKimTamTy, !muDoiKimTamTy.Checked));
        }

        private void đổi999HoaHồngToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => g.PushMissions(MissionsType.Doi999HoaHong, !muDoi999HoaHong.Checked));
        }

        private void đổiLoạnPhỉMậtHàmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => g.PushMissions(MissionsType.DoiLoanPhiMatHam, !muDoiLoanPhiMatHam.Checked));
        }

        private void đổiHuyễnSắcCâuThiênTháiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => g.PushMissions(MissionsType.DoiHuyenSacCauThienThai, !muDoiHuyenSacCauThienThai.Checked));
        }

        private void đổiTiềmNăngTánToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => g.PushMissions(MissionsType.DoiTiemNangTan, !muDoiTiemNangTan.Checked));
        }

        private void nhậnBồiThườngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //AllSelectedOnlinedGames.ForEach(game =>
            //{
            //    if (!muNhanBoiThuong.Checked)
            //        game.PushMissions(MissionsType.NhanBoiThuong);
            //    else
            //        game.RemoveMission(MissionsType.NhanBoiThuong);
            //});
        }

        private void nhậnLễBaoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game =>
            {
                if (!muNhanLeBao.Checked)
                    game.PushMissions(MissionsType.NhanLeBao);
                else
                    game.RemoveMission(MissionsType.NhanLeBao);
            });
        }

        private void nhậnBóngToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game =>
            {
                if (!muNhanBong.Checked)
                    game.PushMissions(MissionsType.NhanBong);
                else
                    game.RemoveMission(MissionsType.NhanBong);
            });
        }

        private void nhậnThưởngTỷVõToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game =>
            {
                if (!muNhanThuongQuanSonHai.Checked)
                    game.PushMissions(MissionsType.NhanThuongTyVo);
                else
                    game.RemoveMission(MissionsType.NhanThuongTyVo);
            });
        }

        private void nhậnChiếnCôngKiếmChỉToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game =>
            {
                if (!muNhanChienCongKiemChi.Checked)
                    game.PushMissions(new[] { MissionsType.NhanChienCong, MissionsType.NhanKiemChi });
                else
                    game.RemoveMission(new[] { MissionsType.NhanChienCong, MissionsType.NhanKiemChi });
            });
        }

        private void kimTinhThạchToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach((Action<Game>)(g =>
            {
                new Thread((ThreadStart)(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    foreach (var item in g.PacketItems.All)
                    {
                        if (item.ClearName == "quavuuoc")
                        {
                            item.Use();
                            Thread.Sleep(1000);
                            break;
                        }
                    }
                    g.DoStringEx("setmetatable(_G, {__index = HuoyueWeeklyGift_Env}); if this:IsVisible() then HuoyueWeeklyGift_Select(1); end ");
                    Thread.Sleep(1000);
                    g.DoStringEx("setmetatable(_G, { __index = LoginSelectServerQuest_Env}); if this:IsVisible() then SelectServerQuest_Bn1Click(); end");

                })).Start();


            })
            );
        }

        private void câuThiênTháiToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach((Action<Game>)(g =>
            {
                new Thread((ThreadStart)(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    foreach (var item in g.PacketItems.All)
                    {
                        if (item.ClearName == "quavuuoc")
                        {
                            item.Use();
                            Thread.Sleep(1000);
                            break;
                        }
                    }
                    g.DoStringEx("setmetatable(_G, {__index = HuoyueWeeklyGift_Env}); if this:IsVisible() then HuoyueWeeklyGift_Select(2); end ");
                    Thread.Sleep(1000);
                    g.DoStringEx("setmetatable(_G, { __index = LoginSelectServerQuest_Env}); if this:IsVisible() then SelectServerQuest_Bn1Click(); end");

                })).Start();


            })
            );
        }

        private void cửuThiênNgọcToáiToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach((Action<Game>)(g =>
            {
                new Thread((ThreadStart)(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    foreach (var item in g.PacketItems.All)
                    {
                        if (item.ClearName == "quavuuoc")
                        {
                            item.Use();
                            Thread.Sleep(1000);
                            break;
                        }
                    }
                    g.DoStringEx("setmetatable(_G, {__index = HuoyueWeeklyGift_Env}); if this:IsVisible() then HuoyueWeeklyGift_Select(3); end ");
                    Thread.Sleep(1000);
                    g.DoStringEx("setmetatable(_G, { __index = LoginSelectServerQuest_Env}); if this:IsVisible() then SelectServerQuest_Bn1Click(); end");

                })).Start();


            })
           );
        }

        private void thầnKhíToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game =>
            {
                game.PushMissions(MissionsType.SuaThanKhi, !muSuaThanKhi.Checked);
            });
        }

        private void võHồnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game =>
            {
                game.PushMissions(MissionsType.SuaVoHon, !muSuaVoHon.Checked);
            });
        }

        private void trangBịToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game =>
            {
                game.PushMissions(MissionsType.SuaTrangBi, !muSuaTrangBi.Checked);
            });
        }

        private void button15_Click(object sender, EventArgs e)
        {
            if (cboTiemNang.SelectedIndex == 0)
                SelectedOnlinedGames.ForEach(game => game.DoStringEx("POINT = " + nudTangTiemNang.Value + "; Player:SendAskManualAttr(0, 0, POINT, 0, 0);"));

            if (cboTiemNang.SelectedIndex == 1)
                SelectedOnlinedGames.ForEach(game => game.DoStringEx("POINT = " + nudTangTiemNang.Value + "; Player:SendAskManualAttr(0, 0, 0, 0, POINT);"));

            if (cboTiemNang.SelectedIndex == 2)
                SelectedOnlinedGames.ForEach(game => game.DoStringEx("POINT = " + nudTangTiemNang.Value + "; Player:SendAskManualAttr(0, POINT, 0, 0, 0);"));

            if (cboTiemNang.SelectedIndex == 3)
                SelectedOnlinedGames.ForEach(game => game.DoStringEx("POINT = " + nudTangTiemNang.Value + "; Player:SendAskManualAttr(POINT, 0, 0, 0, 0);"));

            if (cboTiemNang.SelectedIndex == 4)
                SelectedOnlinedGames.ForEach(game => game.DoStringEx("POINT = " + nudTangTiemNang.Value + "; Player:SendAskManualAttr(0, 0, 0 , POINT, 0);"));
        }

        private void nhậnThưởngQuanSơnHảiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game =>
            {
                if (!muNhanThuongQuanSonHai.Checked)
                    game.PushMissions(MissionsType.NhanThuongQuanSonHai);
                else
                    game.RemoveMission(MissionsType.NhanThuongQuanSonHai);
            });
        }

        private void đổiChânNguyênLinhPháchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game => { game.PushMissions(MissionsType.DoiChanNguyenLinhPhach, !muDoiChanNguyenLinhPhach.Checked); });
        }

        private void nhậnKẹoHallowenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game =>
            {
                if (!muNhanKeoHallowen.Checked)
                    game.PushMissions(MissionsType.NhanKeoHallowen);
                else
                    game.RemoveMission(MissionsType.NhanKeoHallowen);
            });
        }

        private void điNémKẹoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g =>
            {
                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    if (g.TLBB.IsRide)
                    {
                        g.DownRide();
                        Thread.Sleep(1000);
                    }
                    foreach (Game ga in g.Party)
                    {
                        if (ga.TLBB.Id == g.TLBB.Id)
                            continue;
                        g.SelectTarget(ga.SelfId);
                        Thread.Sleep(1000);
                        g.DoAction("EUicons_6");
                        //EUicons_6
                        Thread.Sleep(1000);
                    }
                    g.Talk(TOCHAU.VanNganBac);
                    Thread.Sleep(1000);
                    g.QuestFrame.Click("#{WSTGY_111012_01}");
                    Thread.Sleep(1000);
                    g.QuestFrame.Click("#{WSTGY_111012_08}");
                    Thread.Sleep(1000);
                    g.LUA.QuestFrameMissionContinue();
                    Thread.Sleep(1000);
                    g.LUA.QuestFrameMissionComplete();
                }).Start();
            });
        }

        private void giaoNgũHànhThiệpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => g.PushMissions(MissionsType.GiaoNguHanhPhapThiep, !muGiaoNguHanhPhapThiep.Checked));
        }


        private void bùaBảoRươngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game =>
            {
                if (!muNhanBuaBaoRuong.Checked)
                    game.PushMissions(MissionsType.NhanBuaBaoRuong);
                else
                    game.RemoveMission(MissionsType.NhanBuaBaoRuong);
            });
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel2.Enabled = false;
            new Thread(() =>
            {
                string msg = Poster.CurlGet("https://tieudattai.org/heli.php");
                MessageBox.Show(this, msg, "MicroAuto", MessageBoxButtons.OK);
                linkLabel2.Enabled = true;
            }).Start();

        }

        private void numHeli_TextChanged(object sender, EventArgs e)
        {
            numHeli.Text = TDT.FormatMoney(TDT.ParseAllInt(numHeli.Text));
            numHeli.Select(numHeli.Text.Length, 0);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            button16.Enabled = false;
            if (MessageBox.Show(this, "Bạn có muốn quy đổi " + numHeli.Text + " VND sang Beli", "MicroAuto", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string msg = Poster.CurlGet("https://tieudattai.org/heli.php?heli=" + TDT.ParseAllInt(numHeli.Text) + "&email=" + HttpUtility.UrlEncode(User.Email) + "&pass=" + HttpUtility.UrlEncode(User.Pass));
                MessageBox.Show(this, msg, "MicroAuto", MessageBoxButtons.OK);
                button16.Enabled = true;
                picBeri_Click(null, null);
            }
            else
            {
                button16.Enabled = true;
            }
        }

        private void nộiLựcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => g.PushMissions(MissionsType.TuLuyenNoiLuc, !muTuLuyenNoiLuc.Checked));
        }

        private void thểLựcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => g.PushMissions(MissionsType.TuLuyenTheLuc, !muTuLuyenTheLuc.Checked));
        }

        private void thânPhápToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => g.PushMissions(MissionsType.TuLuyenThanPhap, !muTuLuyenThanPhap.Checked));
        }

        private void chkBinhThanh_CheckedChanged(object sender, EventArgs e)
        {
            if (IsHoldForDownSetting)
                return;
            if (!IsHoldForDownSettingEx)
            {
                if (CurGame != null)
                {
                    Game z = CurGame;
                    z.PushMissions(MissionsType.DatDoiBinhThanh, chkBinhThanh.Checked); z.StepBinhThanh = 0; z.quandoanindex = -1;
                }
                SelectedOnlinedGames.ForEach(g => g.QuanDoans.ForEach(z => { z.PushMissions(MissionsType.DatDoiBinhThanh, chkBinhThanh.Checked); z.StepBinhThanh = 0; z.quandoanindex = -1; }));
            }
        }

        private void chkKho_CheckedChanged(object sender, EventArgs e)
        {
            if (IsHoldForDownSetting)
                return;
            SelectedOnlinedGames.ForEach(g => g.QuanDoans.ForEach(z => { z.PushMissions(MissionsType.BinhThanhKho, chkKho.Checked); z.StepBinhThanh = 0; }));
            CurGame.PushMissions(MissionsType.BinhThanhKho, chkKho.Checked);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            CreateControl(new Item.ConfigBinhThanh());
        }

        private void setNhómTỷVõToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (Game game in SelectedOnlinedGames)
            {
                if (!dic.ContainsKey(game.TLBB.Id))
                {
                    dic.Add(game.TLBB.Id, game.TLBB.Name);
                }
            }
            if (dic.Count != 3)
            {
                MessageBox.Show(this, "Chỉ được chọn 3 nhân vật để set nhóm tỷ võ", "MicroAuto", MessageBoxButtons.OK);
                return;
            }
            int cnt = 1;
            bool isbreak = false;
            while (!isbreak)
            {
                isbreak = true;
                foreach (KeyValuePair<string, string> kvp in CalendarEx.TyVos)
                {
                    string name = kvp.Key;
                    if (name == cnt.ToString())
                    {
                        cnt++;
                        isbreak = false;
                        break;
                    }
                }
            }
            if (!string.IsNullOrEmpty(cnt.ToString()) && !CalendarEx.TyVos.ContainsKey(cnt.ToString()))
            {
                try
                {
                    string mem = "";
                    foreach (KeyValuePair<string, string> kvp in dic)
                    {
                        mem += "[" + kvp.Value + "]";
                    }
                    XmlElement node = CalendarEx.XMLTyVo.CreateElement("TyVo");
                    XmlAttribute atb = CalendarEx.XMLTyVo.CreateAttribute("Name");
                    XmlAttribute atbmem = CalendarEx.XMLTyVo.CreateAttribute("Member");
                    atb.Value = cnt.ToString();
                    atbmem.Value = mem;
                    node.Attributes.Append(atb);
                    node.Attributes.Append(atbmem);
                    CalendarEx.XMLTyVo.SelectSingleNode("/*").AppendChild(node);
                    Game.IniParser.Write("Item", "TyVos", CalendarEx.XMLTyVo.OuterXml);
                    CalendarEx.TyVos.Add(cnt.ToString(), mem);
                }
                catch (Exception ex)
                {
                    Main.PushLogEx(ex.Message);
                }
            }
            else
            {
                //Main.PushLog("clgt");
            }
        }

        private void tựTổĐộiTheoNhómTỷVõToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IsCreateamTyVo = tựTổĐộiTheoNhómTỷVõToolStripMenuItem.Checked = !tựTổĐộiTheoNhómTỷVõToolStripMenuItem.Checked;
        }

        private void tỷVõToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlineLeaders.ForEach(g => g.PushMissions(MissionsType.DatDoiTyVo, !muTyVo.Checked));
        }

        private void chọnMáyChủToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AllGame.ForEach(g => g.Quit());
        }


        private void tảiThiếtLậpMớiNhấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Enabled = false;
            Poster posterBuy = new Poster();
            posterBuy.Url = URL.HomePage + "microauto/user.php";
            //posterBuy.Data = "cmd=buybossmap&x=" + nudX.Value + "&y=" + nudY.Value + "&email=" + HttpUtility.UrlEncode(User.Email) + "&pass=" + HttpUtility.UrlEncode(User.Pass);
            posterBuy.Control = this;
            posterBuy.AutoReconnect = true;
            posterBuy.Completed += (ss, ee) =>
            {
                sender = posterBuy;
                if ((sender as Poster).Response.Contains("BossMAP"))
                {
                    try
                    {
                        IniParser ini = new IniParser((sender as Poster).Response);
                        foreach (string ins in ini.EnumSection("BossMAP"))
                        {
                            if (!string.IsNullOrEmpty(ins))
                            {
                                Game.IniParser.Write("BossMap", ins, ini.Read("BossMAP", ins));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + ex.StackTrace);
                    }
                    MessageBox.Show(this, "Đã có thiết lập BossMap mới nhất", "MicroAuto", MessageBoxButtons.OK);

                }
                else
                {
                    MessageBox.Show(this, (sender as Poster).Response, "MicroAuto", MessageBoxButtons.OK);
                }
                Enabled = true;
            };
            posterBuy.Post();
        }

        private void leaderToolStripMenuItem_EnabledChanged(object sender, EventArgs e)
        {

        }

        private void setQuânĐoànToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (Game game in SelectedOnlinedGames)
            {
                if (!dic.ContainsKey(game.TLBB.Id))
                {
                    dic.Add(game.TLBB.Id, game.TLBB.Name);
                }
            }
            if (dic.Count != 12)
            {
                MessageBox.Show(this, "Chỉ được chọn 12 nhân vật để set quân đoàn", "MicroAuto", MessageBoxButtons.OK);
                return;
            }
            int cnt = 1;
            bool isbreak = false;
            while (!isbreak)
            {
                isbreak = true;
                foreach (KeyValuePair<string, string> kvp in CalendarEx.QuanDoan)
                {
                    string name = kvp.Key;
                    if (name == cnt.ToString())
                    {
                        cnt++;
                        isbreak = false;
                        break;
                    }
                }
            }
            if (!string.IsNullOrEmpty(cnt.ToString()) && !CalendarEx.QuanDoan.ContainsKey(cnt.ToString()))
            {
                try
                {
                    string mem = "";
                    foreach (KeyValuePair<string, string> kvp in dic)
                    {
                        mem += "[" + kvp.Value + "]";
                    }
                    XmlElement node = CalendarEx.XMLQuanDoan.CreateElement("QuanDoan");
                    XmlAttribute atb = CalendarEx.XMLQuanDoan.CreateAttribute("Name");
                    XmlAttribute atbmem = CalendarEx.XMLQuanDoan.CreateAttribute("Member");
                    atb.Value = cnt.ToString();
                    atbmem.Value = mem;
                    node.Attributes.Append(atb);
                    node.Attributes.Append(atbmem);
                    CalendarEx.XMLQuanDoan.SelectSingleNode("/*").AppendChild(node);
                    Game.IniParser.Write("Item", "QuanDoan", CalendarEx.XMLQuanDoan.OuterXml);
                    CalendarEx.QuanDoan.Add(cnt.ToString(), mem);
                }
                catch (Exception ex)
                {
                    Main.PushLogEx(ex.Message);
                }
            }
            else
            {
                //Main.PushLog("clgt");
            }
        }

        private void thểNộiThânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g =>
            {
                g.PushMissions(new[] { MissionsType.TuLuyenTheLuc, MissionsType.TuLuyenNoiLuc, MissionsType.TuLuyenThanPhap });
            });
        }
        public static bool IsNotUseRide { get; set; }
 

        private void võÝToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => g.PushMissions(MissionsType.NhiemVuVoY, !muVoY.Checked));
        }

        private void quanSơnHảiToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SelectedOnlineLeaders.ForEach(g => { g.PushMissions(MissionsType.DatDoiQuanSonHai, !muQuanSonHai.Checked); g.countsovong = 0; });
        }


        public static int TangQuanSonHai { get; set; }


        private void vôTựPhổToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => g.PushMissions(MissionsType.VoTuPho, !muVoTuPho.Checked));
        }





        private void đàoHoaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlineLeaders.ForEach(l => { l.PushMissions(MissionsType.DatDoiAcBa); l.Party.ForEach(g => g.AcBa = MENPAI.DaoHoa); });
        }

        private void tựĐộngToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            SelectedOnlineLeaders.ForEach(l =>
            {
                l.PushMissions(MissionsType.DatDoiAcBa, !tựĐộngToolStripMenuItem2.Checked);
                if (tựĐộngToolStripMenuItem2.Checked)
                    l.Party.ForEach(g => g.RemoveMission(MissionsType.DatDoiAcBa));
            });
        }

        private void quỷCốcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlineLeaders.ForEach(l => { l.PushMissions(MissionsType.DatDoiAcBa); l.Party.ForEach(g => g.AcBa = MENPAI.QuyCoc); });
        }

        private void đườngMônToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlineLeaders.ForEach(l => { l.PushMissions(MissionsType.DatDoiAcBa); l.Party.ForEach(g => g.AcBa = MENPAI.DuongMon); });
        }

        private void mộDungToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlineLeaders.ForEach(l => { l.PushMissions(MissionsType.DatDoiAcBa); l.Party.ForEach(g => g.AcBa = MENPAI.MoDung); });
        }

        private void tinhTúcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlineLeaders.ForEach(l => { l.PushMissions(MissionsType.DatDoiAcBa); l.Party.ForEach(g => g.AcBa = MENPAI.TinhTuc); });
        }

        private void tiêuDaoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlineLeaders.ForEach(l => { l.PushMissions(MissionsType.DatDoiAcBa); l.Party.ForEach(g => g.AcBa = MENPAI.TieuDao); });
        }

        private void thiếuLâmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlineLeaders.ForEach(l => { l.PushMissions(MissionsType.DatDoiAcBa); l.Party.ForEach(g => g.AcBa = MENPAI.ThieuLam); });
        }

        private void thiênSơnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlineLeaders.ForEach(l => { l.PushMissions(MissionsType.DatDoiAcBa); l.Party.ForEach(g => g.AcBa = MENPAI.ThienSon); });
        }

        private void thiênLongToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlineLeaders.ForEach(l => { l.PushMissions(MissionsType.DatDoiAcBa); l.Party.ForEach(g => g.AcBa = MENPAI.ThienLong); });
        }

        private void ngamyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlineLeaders.ForEach(l => { l.PushMissions(MissionsType.DatDoiAcBa); l.Party.ForEach(g => g.AcBa = MENPAI.NgaMy); });
        }

        private void võĐangToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlineLeaders.ForEach(l => { l.PushMissions(MissionsType.DatDoiAcBa); l.Party.ForEach(g => g.AcBa = MENPAI.VoDang); });
        }

        private void minhGiáoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlineLeaders.ForEach(l => { l.PushMissions(MissionsType.DatDoiAcBa); l.Party.ForEach(g => g.AcBa = MENPAI.MinhGiao); });
        }

        private void cáiBangToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlineLeaders.ForEach(l => { l.PushMissions(MissionsType.DatDoiAcBa); l.Party.ForEach(g => g.AcBa = MENPAI.CaiBang); });
        }

        private void shopQuỷThịToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => g.PushMissions(MissionsType.MoShopQuyThi, !muMoShopQuyThi.Checked));
        }

        private void chuyểnKênhChatBangToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AllOnelineGame.ToList().ForEach(g => g.DoStringEx("setmetatable(_G, {__index = ChatFrame_Env }); Chat_ChangeTabIndex(2); "));
        }



        private void đổiVàngThườngSangVàngKhóaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => g.DoStringEx(@"Clear_XSCRIPT( )
		Set_XSCRIPT_Function_Name( 'DoMoneyToJiaozi' )

        Set_XSCRIPT_ScriptID(800119)

        Set_XSCRIPT_Parameter(0, " + g.TLBB.Gold + @")

        Set_XSCRIPT_ParamCount(1)

    Send_XSCRIPT()"));
        }

        private void cấtĐồToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game =>
            {
                if (!muCatDo.Checked)
                    game.PushMissions(new[] { MissionsType.CatVang, MissionsType.CatDo });
                else
                    game.RemoveMission(new[] { MissionsType.CatVang, MissionsType.CatDo });
            });
        }

        private void mởTừXaCtrlBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game =>
            {
                if (game.SafeTime > 0)
                    game.DoStringEx("PushEvent('TOGLE_BIGBANK'); PushEvent('UPDATE_BANK')");
            });
        }

        private void lấyĐồToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game =>
            {
                if (!muLayDo.Checked)
                    game.PushMissions(MissionsType.LayDo);
                else
                    game.RemoveMission(MissionsType.LayDo);
                game.RemoveMission(MissionsType.LayVang);
            });
        }

        private void nhậnLạiNộiTứcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g =>
            {
                g.DoStringEx(@"Clear_XSCRIPT()
			                Set_XSCRIPT_Function_Name( 'MartialLastDayMonsterReward' )
                            Set_XSCRIPT_ScriptID(507010)
                            Set_XSCRIPT_Parameter(0, 1)
                            Set_XSCRIPT_ParamCount(1)
                            Send_XSCRIPT()");
                for (int i = 0; i < 10; i++)
                {
                    g.DoStringEx(@"Clear_XSCRIPT()
                                Set_XSCRIPT_Function_Name('AddAttr')
                                Set_XSCRIPT_ScriptID(507010)
                                Send_XSCRIPT()");
                }
            });
        }

        private void tăngCấpTrưởngThànhLongVănToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach((Action<Game>)(g =>
            {
                int longvancount = g.PacketItems.All.Concat(g.PacketItems.TrangBi).Where(p => p.TypeName == "Long Văn").Count();
                if (longvancount > 1)
                {
                    g.PushDebugMessage("Có quá nhiều Long Văn. Không biết nâng cái nào");
                    g.PushDebugMessage("Dừng tăng cấp Long Văn");
                }
                else if (longvancount == 0)
                {
                    g.PushDebugMessage("Không có Long Văn để tăng cấp");
                    g.PushDebugMessage("Dừng tăng cấp Long Văn");
                }
                else
                {
                    if (g.PacketItems.TrangBi.Where(p => p.Name == "Long Văn" && p.CapTruongThanhLongVan >= 100).FirstOrDefault() != null)
                    {
                        g.PushDebugMessage("Long Văn Đã Đạt Cấp 100");
                        g.PushDebugMessage("Dừng tăng cấp Long Văn");
                    }
                    else
                    {
                        g.PushMissions(MissionsType.TangCapTruongThanhLongVan, !muTangCapTruongThanhLongVan.Checked);
                    }
                }
            }));
        }

        private void comboBox3_DropDown(object sender, EventArgs e)
        {
            if (CurGame != null)
            {
                var cbo = sender as ComboBox;
                cbo.Items.Clear();
                foreach (var item in CurGame.PacketItems.All)
                {
                    if (item.TypeName.Contains("Thạch"))
                    {
                        bool isHave = false;
                        foreach (var i in cbo.Items)
                            if (i.ToString() == item.Name)
                            {
                                isHave = true;
                            }
                        if (!isHave)
                            cbo.Items.Add(item.Name);
                    }
                }
            }
        }

        private void comboBox2_DropDown(object sender, EventArgs e)
        {
            var cbo = sender as ComboBox;
            cbo.Items.Clear();
            if (CurGame != null)
            {
                foreach (var item in CurGame.PacketItems.All)
                {
                    if (GAMEDIC.NgocThuocTinh3.Contains(item.Name))
                    {
                        bool isHave = false;
                        foreach (var i in cbo.Items)
                            if (i.ToString() == item.Name)
                            {
                                isHave = true;
                            }
                        if (!isHave)
                            cbo.Items.Add(item.Name);
                    }
                }
            }
        }

        private void button10_Click_2(object sender, EventArgs e)
        {
            if (CurGame == null)
                return;
            var game = CurGame;
            try
            {
                string text = cboNgoc.Text;
                int dtIdx = -1;
                int gemIdx = -1;
                if (!string.IsNullOrEmpty(text))
                {
                    foreach (var item in game.PacketItems.All)
                    {
                        if (item.Name == "Sơ Cấp Bảo Thạch Hợp Thành Phù" && !item.IsCoDinh)
                        {
                            dtIdx = (int)item.Index;
                        }
                        if (item.Name == text)
                        {
                            gemIdx = (int)item.Index;
                        }

                    }
                    new Thread(() =>
                    {
                        Thread.CurrentThread.IsBackground = true;
                        if (gemIdx != -1 && dtIdx != -1)
                        {
                            game.DoStringEx(@"local theAction,bLocked = PlayerPackage:EnumItem('material', " + (gemIdx - 30) + @");
                                        NGOCVAL = theAction:GetDefineID();");

                            game.DoStringEx("LifeAbility:Do_Combine(NGOCVAL," + dtIdx + ",0,100)");
                            Thread.Sleep(100);
                            game.DoStringEx("LifeAbility:Do_Combine(NGOCVAL," + dtIdx + ",0,100)");
                            Thread.Sleep(300);
                            game.DoStringEx("setmetatable(_G, {__index = LoginSelectServerQuest_Env}); if this:IsVisible() then SelectServerQuest_Bn1Click(); end");
                        }
                    }).Start();
                }
            }
            catch { }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            if (CurGame == null)
                return;
            var game = CurGame;
            string text = comboBox2.Text;
            if (text != "")
            {
                int gemIdx = -1;
                int dtIdx = -1;
                foreach (var item in game.PacketItems.All)
                {
                    if (item.Name == text)
                    {
                        gemIdx = (int)item.Index;
                        game.PushDebugMessage(item.Name);
                    }
                    if (item.Name == "Bảo Thạch Điêu Trác Phù Cấp 3" && !item.IsCoDinh)
                    {
                        dtIdx = (int)item.Index;
                    }
                }
                if (gemIdx != -1 && dtIdx != -1)
                {
                    Task.Run(() =>
                    {
                        game.DoStringEx("Clear_XSCRIPT(); Set_XSCRIPT_Function_Name('OnGemCarve'); Set_XSCRIPT_ScriptID(800117); Set_XSCRIPT_Parameter(0, " + gemIdx + "); Set_XSCRIPT_Parameter(1, " + +dtIdx + "); Set_XSCRIPT_Parameter(2, 134); Set_XSCRIPT_ParamCount(3); Send_XSCRIPT(); ");
                    });
                }
            }
        }

  

        private void taLàNgườiGomĐồToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (CurGame != null)
            {
                GomDoName = CurGame.TLBB.Name;
            }
        }

        private void đưaĐồChoTaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => { g.RemoveMission(MissionsType.GomKNB); g.RemoveMission(MissionsType.GomVang); g.PushMissions(MissionsType.GomDo); g.RemoveMission(MissionsType.DuaBaoDoHiem); g.RemoveMission(MissionsType.DuaTangBaoDo); });
        }



        private void đưaĐồVàngChoTaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => { g.RemoveMission(MissionsType.GomKNB); g.PushMissions(MissionsType.GomVang); g.PushMissions(MissionsType.GomDo); g.RemoveMission(MissionsType.DuaBaoDoHiem); g.RemoveMission(MissionsType.DuaTangBaoDo); });
        }

        private void treoShopChoTaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (GomDoName == null)
            {
                MessageBox.Show(this, "Cần chọn người gom đồ trước", "MicroAuto", MessageBoxButtons.OK);
                return;
            }
            SelectedOnlinedGames.ForEach(g => { g.PushMissions(MissionsType.GomDoKNB); });
        }



        private void lấyĐồVàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game =>
            {
                if (!muLayDoLayVang.Checked)
                    game.PushMissions(new[] { MissionsType.LayDo, MissionsType.LayVang });
                else
                    game.RemoveMission(new[] { MissionsType.LayDo, MissionsType.LayVang });
            });
        }


        private void chkBanRac_CheckedChanged(object sender, EventArgs e)
        {
            IsBanRac = chkBanRac.Checked;
        }

        private void đạiLễHùngVươngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => g.PushMissions(MissionsType.DaiLeHungVuong, !muDaiLeHungVuong.Checked));
        }

        private void lấyChânNguyênKhóaTúiThiênCơToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var game in AllOnelineGame)
            {
                foreach (var item in game.PacketItems.ThienCo.Where(i => i.IsCoDinh))
                {
                    if (item.Name == "Chân Nguyên Linh Phách" || item.Name == "Chân Nguyên Phách" || item.Name == "Chân Nguyên Tinh Phách")
                        game.GetItemThienCo(item.Index);
                }
            }
        }

        private void lênCấp2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sen = sender as ToolStripMenuItem;
            int cap = TDT.ParseAllInt(sen.Text);
            SelectedOnlinedGames.ForEach(g => { g.PushMissions(MissionsType.HopVoHon); g.CapHopVoHon = cap - 1; });
        }

        private void hợpLênMàuXanhCấp2ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => { g.PushMissions(MissionsType.HopDienTich); g.CapHopDienTich = 1; });
        }

        private void hợpLênMàuXanhCấp2ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => { g.PushMissions(MissionsType.HopDienTich); g.CapHopDienTich = 2; });
        }

        private void hợpLênMàuXanhCấp2ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => { g.PushMissions(MissionsType.HopDienTich); g.CapHopDienTich = 3; });
        }

        private void đưaBảoĐồHiếmChoTaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => { g.RemoveMission(MissionsType.GomKNB); g.RemoveMission(MissionsType.GomVang); g.PushMissions(MissionsType.GomDo); g.PushMissions(MissionsType.DuaBaoDoHiem); g.RemoveMission(MissionsType.DuaTangBaoDo); });
        }

        private void muBaoDoHiem_Click(object sender, EventArgs e)
        {
            SelectedOnlineLeaders.ForEach(g => g.Party.ForEach(gg => { gg.PushMissions(MissionsType.DatDoiBaoDoHiem, !muBaoDoHiem.Checked); }));
        }

        private void muDungDoatBaoRuong_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => g.PushMissions(MissionsType.DungDoatBaoRuong, !muDungDoatBaoRuong.Checked));
        }

        private void muCheThanKhi_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game => game.PushMissions(MissionsType.CheThanKhi, !muCheThanKhi.Checked));
        }

        private void muTuLuyenCuongLuc_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => g.PushMissions(MissionsType.TuLuyenCuongLuc, !muTuLuyenCuongLuc.Checked));
        }

        private void NangTamPhap_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem i = sender as ToolStripMenuItem;
            SelectedOnlinedGames.ForEach(game =>
            {
                game.PushMissions(MissionsType.NangTamPhap);
                game.RemoveMission(MissionsType.Quyen3Len30);
                game.TamPhapNangToi = TDT.ParseAllInt(i.Text);
            });
        }

        private void nhậnLạiNộiTứcToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g =>
            {
                g.DoStringEx(@"Clear_XSCRIPT()
			                Set_XSCRIPT_Function_Name( 'MartialLastDayMonsterReward' )
                            Set_XSCRIPT_ScriptID(507010)
                            Set_XSCRIPT_Parameter(0, 1)
                            Set_XSCRIPT_ParamCount(1)
                            Send_XSCRIPT()");
                for (int i = 0; i < 10; i++)
                {
                    g.DoStringEx(@"Clear_XSCRIPT()
                                Set_XSCRIPT_Function_Name('AddAttr')
                                Set_XSCRIPT_ScriptID(507010)
                                Send_XSCRIPT()");
                }
            });
        }

        private void đổiVàngSangGiaoTửToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => g.DoStringEx(@"Clear_XSCRIPT( )
		Set_XSCRIPT_Function_Name( 'DoMoneyToJiaozi' )

        Set_XSCRIPT_ScriptID(800119)

        Set_XSCRIPT_Parameter(0, " + g.TLBB.Gold + @")

        Set_XSCRIPT_ParamCount(1)

    Send_XSCRIPT()"));
        }

        private void thăngCấpHàoHiệpẤnToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g =>
            {
                Task.Run(() =>
                {
                    for (int i = 0; i < 10; i++)
                        g.DoStringEx("setmetatable(_G, {__index = SelfJunXian_Env}); SelfJunXian_EquipHXYLevelup(); ");

                });
            }
            );
        }

        private void lấyĐồTúiThiênCơToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game => game.LayDoThienCo());
        }

        private void kimTinhThạchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach((Action<Game>)(g =>
            {
                new Thread((ThreadStart)(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    foreach (var item in g.PacketItems.All)
                    {
                        if (item.ClearName == "quavuuoc")
                        {
                            item.Use();
                            Thread.Sleep(1000);
                            break;
                        }
                    }
                    g.DoStringEx("setmetatable(_G, {__index = HuoyueWeeklyGift_Env}); if this:IsVisible() then HuoyueWeeklyGift_Select(1); end ");
                    Thread.Sleep(1000);
                    g.DoStringEx("setmetatable(_G, { __index = LoginSelectServerQuest_Env}); if this:IsVisible() then SelectServerQuest_Bn1Click(); end");

                })).Start();


            })
          );
        }

        private void câuThiênTháiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach((Action<Game>)(g =>
            {
                new Thread((ThreadStart)(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    foreach (var item in g.PacketItems.All)
                    {
                        if (item.ClearName == "quavuuoc")
                        {
                            item.Use();
                            Thread.Sleep(1000);
                            break;
                        }
                    }
                    g.DoStringEx("setmetatable(_G, {__index = HuoyueWeeklyGift_Env}); if this:IsVisible() then HuoyueWeeklyGift_Select(2); end ");
                    Thread.Sleep(1000);
                    g.DoStringEx("setmetatable(_G, { __index = LoginSelectServerQuest_Env}); if this:IsVisible() then SelectServerQuest_Bn1Click(); end");

                })).Start();


            })
            );
        }

        private void cửuThiênNgọcToáiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach((Action<Game>)(g =>
            {
                new Thread((ThreadStart)(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    foreach (var item in g.PacketItems.All)
                    {
                        if (item.ClearName == "quavuuoc")
                        {
                            item.Use();
                            Thread.Sleep(1000);
                            break;
                        }
                    }
                    g.DoStringEx("setmetatable(_G, {__index = HuoyueWeeklyGift_Env}); if this:IsVisible() then HuoyueWeeklyGift_Select(3); end ");
                    Thread.Sleep(1000);
                    g.DoStringEx("setmetatable(_G, { __index = LoginSelectServerQuest_Env}); if this:IsVisible() then SelectServerQuest_Bn1Click(); end");

                })).Start();


            })
           );
        }

        private void phúcLợiĐiểmDanhToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game =>
            {
                game.phucloi();
            });
        }

        private void nhậnQuàThăngCấpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game => game.NhanQuaThangCap());
        }

        private void muaTưBổĐanToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            foreach (Game game in SelectedOnlinedGames)
            {
                game.MuaTuBoDanChuaCo();
            }
        }

        private void cườngHóa2MónLên7ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game =>
            {
                game.IsCuongHoa7 = true;
                game.Is2Mon = true;
            });
        }

        private void cườngHóa7MónLên7ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game =>
            {
                game.IsCuongHoa7 = true;
                game.Is2Mon = false;
            });
        }

        private void vậnTiêuToolStripMenuItem1_Click(object sender, EventArgs e)
        {
        }

        private void thuPetToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            AllOnelineGame.ForEach(game => game.ThuPet());
        }

        private void lênNgựaCtrlUToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AllOnelineGame.ForEach(game => game.UpRide());
        }

        private void xuốngNgựaCtrlDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AllOnelineGame.ForEach(game => game.DownRide());
        }

        private void sắpXếpTayNảiAltZToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AllOnelineGame.ForEach(game => game.LUA.PackUp());
        }

        private void selectCtrlAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in Lv.Items)
                item.Selected = true;
        }

        private void tàngHìnhToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AllOnelineGame.ForEach(game => game.AnDon());
        }

        private void điểmDanhCtrlPToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            DiemDanh();
        }

        private void xuấtPetToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            AllOnelineGame.ForEach(game =>
            {
                game.DoAction("PetSkill2_1");
            });
        }

        private void muMoTiemThuoc_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game => game.PushMissions(MissionsType.MoTiemThuoc, !muMoTiemThuoc.Checked));
        }

        private void lộTuyến1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => { if (!muVanTieu.Checked) { g.ApTieu = 1; } else g.ApTieu = 0; });
        }

        private void lộTuyến2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => { if (!muVanTieu.Checked) { g.ApTieu = 2; } else g.ApTieu = 0; });
        }

        private void lộTuyến3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => { if (!muVanTieu.Checked) { g.ApTieu = 3; } else g.ApTieu = 0; });
        }

        private void lộTuyến4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => { if (!muVanTieu.Checked) { g.ApTieu = 4; } else g.ApTieu = 0; });
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CreateControl(new BoQua());
        }

        private void muLinhLuong_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game => game.PushMissions(MissionsType.LinhLuong, !muLinhLuong.Checked));
        }

        private void muMua100KimSangDuoc_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game => game.PushMissions(MissionsType.Mua100KimSangDuoc, !muMua100KimSangDuoc.Checked));
        }

        private void muaThổLinhChâuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Game game in SelectedOnlinedGames)
            {
                game.MuaThoLinhChau();
            }
        }

        private void trangSứcCửuLêToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Game game in SelectedOnlinedGames)
            {
                game.PushMissions(MissionsType.TrangSucCuuLe, !muTrangSucCuuLe.Checked);
            }
        }

        private void tầmKỳToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Game game in SelectedOnlinedGames)
            {
                game.PushMissions(MissionsType.TamKy, !muTamKy.Checked);
            }
        }

        private void bánManhMốiDịchLễQuỷThịToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AllOnelineGame.ForEach(g => g.DoStringEx(@"maxNum = GuiShiUI:LuaFnGetClueCount()
                                                        for index = 1, maxNum do
                                                            clueID = GuiShiUI:LuaFnGetClueID(index - 1)
                                                            if nil ~= clueID and clueID > 0 then
                                                                clueID, clueName, clueDesc, clueQual, clueFunc, vilaidType, viladTime = GuiShiUI:LuaFnGetXianSuoDataFromTable(clueID)
                                                                if clueName ~= 'Tầm Kỳ' then
                                                                    Clear_XSCRIPT()
                                                                    Set_XSCRIPT_Function_Name( 'SaleXiansuo')
                                                                    Set_XSCRIPT_ScriptID( 893197 )
                                                                    Set_XSCRIPT_Parameter( 0, index - 1 )
                                                                    Set_XSCRIPT_Parameter( 1,0 )
                                                                    Set_XSCRIPT_ParamCount( 2 )
                                                                    Send_XSCRIPT()
                                                                end
                                                            end
                                                        end
                                                        "));
        }

        private void muDuaTangBaoDo_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => { g.RemoveMission(MissionsType.GomVang); g.RemoveMission(MissionsType.GomKNB); g.PushMissions(MissionsType.GomDo); g.RemoveMission(MissionsType.DuaBaoDoHiem); g.PushMissions(MissionsType.DuaTangBaoDo); });
        }

        private void btnCanQuetThuCong_Click(object sender, EventArgs e)
        {
            var leader = SelectedOnlineLeaders.FirstOrDefault();
            if (leader != null)
            {
                if (!Game.FastTasks.ContainsKey(leader.TLBB.Name))
                {
                    var FastTask = new FastTask(leader);
                    var page = new TabPage(leader.TLBB.Name);
                    tabCanQuet.TabPages.Add(page);
                    FastTask.Dock = DockStyle.Fill;
                    page.Controls.Add(FastTask);
                    FastTask.Disposed += (ss, ee) =>
                    {
                        page.Dispose();
                    };
                    tabCanQuet.SelectedTab = page;
                }
                else
                {
                    foreach (TabPage page in tabCanQuet.TabPages)
                    {
                        if (page.Text == leader.TLBB.Name)
                        {
                            tabCanQuet.SelectedTab = page;
                            break;
                        }
                    }
                }

                tabCanQuet.Visible = true;
                tabCanQuet.BringToFront();
            }
        }

        private void lênNgựaToolStripMenuItem_Click_2(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => g.UpRide());
        }

        private void xuốngNgựaToolStripMenuItem_Click_2(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => g.DownRide());
        }

        private void xuấtPetToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game =>
            {
                game.DoAction("PetSkill2_1");
            });
        }

        private void thuPetToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game => game.ThuPet());
        }

        private void quyển3Lên30ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(game =>
            {
                game.PushMissions(MissionsType.NangTamPhap);
                game.PushMissions(MissionsType.Quyen3Len30);
            });
        }

        private void giámĐịnhĐồToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => g.IsGiamDinhDo = true);
        }

        private void viênToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => g.MuaX2 = 1);
        }

        private void viênToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => g.MuaX2 = 4);
        }

        private void viênToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => g.MuaX2 = 10);
        }

        private void viênToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => g.MuaX2 = 2);
        }

        private void đưaKNBChoTaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => { g.PushMissions(MissionsType.GomKNB); g.PushMissions(MissionsType.GomDo); g.RemoveMission(MissionsType.DuaBaoDoHiem); g.RemoveMission(MissionsType.DuaTangBaoDo); g.RemoveMission(MissionsType.GomVang); });
        }

        private void chọnMáyChủToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            foreach (Game game in SelectedGames)
            {
                game.Quit();
            }
        }

        private void tabCanQuet_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    var tabcontrol = sender as TabControlEx;
                    if (tabcontrol.GetTabRect(0).Contains(e.Location))
                    {
                        tabCanQuet.Hide();
                    }
                }
            }
            catch { }
        }

        private void tabSkill_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    var tab = sender as TabControlEx;
                    if (tab.GetTabRect(tab.TabPages.Count - 1).Contains(e.Location))
                    {
                        tabCanQuet.Visible = true;
                        tab.SendToBack();

                    }
                }
            }
            catch { }
        }

        private void tabSkill_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if(\)
        }

        private void tabSkill_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPageIndex == tabBottom.TabPages.Count - 1) e.Cancel = true;
        }

        private void tabCanQuet_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPageIndex == 0) e.Cancel = true;
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            Clipboard.SetText(lblSTK.Text);
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            Clipboard.SetText(lblBankNote.Text);
        }

        private void hiệnĐộiTrưởngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AllOnelineGame.Where(g => g.TLBB.IsLeader).ToList().ForEach(g => g.Active());
        }


        private void ẩnĐộnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedOnlinedGames.ForEach(g => g.Hide());
        }

        private void cboPlayer_DropDown(object sender, EventArgs e)
        {
            cboPlayer.Items.Clear();
            List<string> DropName = new List<string>();
            foreach (KeyValuePair<int, Game> kvp in Main.DicGame)
            {
                var game = kvp.Value;
                foreach (GameObject _object in game.Objects.All)
                {
                    if (_object.Name.Trim() == "" || !_object.IsPlayer)
                        continue;
                    if (!DropName.Contains(_object.Name))
                    {
                        DropName.Add(_object.Name);
                    }
                }
            }

            cboPlayer.Items.AddRange(new List<string> { "" }.Concat(DropName).ToArray());
        }

        private void cboPlayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            SettingOld.Leader = cboPlayer.Text;
        }

        private void bánManhMốiDịchLễQuỷThịToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AllOnelineGame.ForEach(g => g.DoStringEx(@"maxNum = GuiShiUI:LuaFnGetClueCount()
                                                        for index = 1, maxNum do
                                                            clueID = GuiShiUI:LuaFnGetClueID(index - 1)
                                                            if nil ~= clueID and clueID > 0 then
                                                                clueID, clueName, clueDesc, clueQual, clueFunc, vilaidType, viladTime = GuiShiUI:LuaFnGetXianSuoDataFromTable(clueID)
                                                                if clueName ~= 'Tầm Kỳ' then
                                                                    Clear_XSCRIPT()
                                                                    Set_XSCRIPT_Function_Name( 'SaleXiansuo')
                                                                    Set_XSCRIPT_ScriptID( 893197 )
                                                                    Set_XSCRIPT_Parameter( 0, index - 1 )
                                                                    Set_XSCRIPT_Parameter( 1,0 )
                                                                    Set_XSCRIPT_ParamCount( 2 )
                                                                    Send_XSCRIPT()
                                                                end
                                                            end
                                                        end
                                                        "));
        }




        private void button13_Click_1(object sender, EventArgs e)
        {
            foreach (Game game in AllGame)
                game.ResetExpSpeed(true);
            AllOnelineGame.ToList().ForEach(g => g.ResetHoaSpee());
        }

        private void tabPage19_Click(object sender, EventArgs e)
        {

        }

        private void Lv_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void button12_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show(this, "Bạn cần phải chọn đường dẫn game\r\nFile Game.exe nằm trong thư mục Bin của game", "MicroAuto", MessageBoxButtons.OK);
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Game.exe |Game.exe";
            if (openFile.ShowDialog(this) == DialogResult.OK)
            {
                new Thread(() =>
                {
                    try
                    {
                        //progressBar1.Value = 50;


                        while (Process.GetProcessesByName("Game").Length > 0)
                        {
                            try
                            {
                                Process.GetProcessesByName("Game")[0].Kill();
                            }
                            catch { }
                            Thread.Sleep(100);
                        }
                        string gameex = Poster.CurlGet("http://tieudattai.org/gamehex.php");

                        byte[] file = File.ReadAllBytes(openFile.FileName);

                        foreach (string line in gameex.Split('\n'))
                        {
                            try
                            {
                                string l = line.Trim();
                                int offset = TDT.ParseInt(l.Split(':')[0]);
                                int olval = ConverterEx.Hex2Int(l.Split(':')[1].Substring(0, l.Split(':')[1].IndexOf("=>")));
                                int val = ConverterEx.Hex2Int(l.Substring(l.IndexOf("=>") + 2));
                                if (file[offset] == olval)
                                    file[offset] = (byte)val;
                            }
                            catch { }
                        }
                        File.WriteAllBytes(openFile.FileName, file);
                        //progressBar1.Value = 100;
                        MessageBox.Show(this, "Vá xong rồi nhé", "MicroAuto", MessageBoxButtons.OK);

                    }
                    catch
                    {
                        MessageBox.Show(this, "Có lỗi xảy ra vui lòng thử lại hoặc reset máy", "MicroAuto", MessageBoxButtons.OK);
                    }

                }).Start();

            }
        }

        private void cboPlayer_DrawItem(object sender, DrawItemEventArgs e)
        {

        }
    }
}
