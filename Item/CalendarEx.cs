using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace _i
{   

    partial class CalendarEx : UserControl
    {
        public static XmlDocument XMLBalls = new XmlDocument();
        public static XmlDocument XMLTyVo = new XmlDocument();
        public static XmlDocument XMLQuanDoan = new XmlDocument();

        public static List<List<string>> Tea = new List<List<string>>();



        public static Dictionary<string, string> Teams { get; set; } = new Dictionary<string, string>();


        public static Dictionary<string, string> Balls = new Dictionary<string, string>();
        public static Dictionary<string, string> TyVos = new Dictionary<string, string>();
        public static Dictionary<string, string> QuanDoan = new Dictionary<string, string>();


        public static bool IsAutoLogin = false;

        public static void LoadXML()
        {   
           
            try
            {
                XMLBalls.LoadXml(Game.IniParser.Read("Item", "Balls"));
            }
            catch
            {
                if (XMLBalls.SelectSingleNode("Balls") == null)
                {
                    XMLBalls.InsertBefore(XMLBalls.CreateXmlDeclaration("1.0", "UTF-8", null), XMLBalls.DocumentElement);
                    XMLBalls.AppendChild(XMLBalls.CreateNode(XmlNodeType.Element, "Balls", ""));                    
                    Game.IniParser.Write("Item", "Balls", XMLBalls.OuterXml);
                }
            }
            try
            {
                XMLTyVo.LoadXml(Game.IniParser.Read("Item", "TyVos"));
            }
            catch
            {
                if (XMLTyVo.SelectSingleNode("TyVos") == null)
                {
                    XMLTyVo.InsertBefore(XMLTyVo.CreateXmlDeclaration("1.0", "UTF-8", null), XMLTyVo.DocumentElement);
                    XMLTyVo.AppendChild(XMLTyVo.CreateNode(XmlNodeType.Element, "TyVos", ""));
                    Game.IniParser.Write("Item", "TyVos", XMLTyVo.OuterXml);
                }
            }
            try
            {
                XMLQuanDoan.LoadXml(Game.IniParser.Read("Item", "QuanDoan"));
            }
            catch
            {
                if (XMLQuanDoan.SelectSingleNode("QuanDoans") == null)
                {
                    XMLQuanDoan.InsertBefore(XMLQuanDoan.CreateXmlDeclaration("1.0", "UTF-8", null), XMLQuanDoan.DocumentElement);
                    XMLQuanDoan.AppendChild(XMLQuanDoan.CreateNode(XmlNodeType.Element, "QuanDoans", ""));
                    Game.IniParser.Write("Item", "QuanDoan", XMLQuanDoan.OuterXml);
                }
            }
            Balls.Clear();
            TyVos.Clear();
            QuanDoan.Clear();
            try
            {
                if (XMLBalls.SelectSingleNode("Balls").SelectNodes("Ball") != null)
                {
                    foreach (XmlElement node in XMLBalls.SelectSingleNode("Balls").SelectNodes("Ball"))
                    {
                        if (node.Attributes["Name"] != null)
                        {
                            string name = node.Attributes["Name"].Value;
                            if (!Balls.ContainsKey(name))
                            {
                                if (node.Attributes["Member"] == null)
                                {
                                    Balls.Add(name, "");
                                }
                                else
                                {
                                    Balls.Add(name, node.Attributes["Member"].Value);
                                }
                            }
                        }
                    }
                }
                if (XMLTyVo.SelectSingleNode("TyVos").SelectNodes("TyVo") != null)
                {
                    foreach (XmlElement node in XMLTyVo.SelectSingleNode("TyVos").SelectNodes("TyVo"))
                    {
                        if (node.Attributes["Name"] != null)
                        {
                            string name = node.Attributes["Name"].Value;
                            if (!TyVos.ContainsKey(name))
                            {
                                if (node.Attributes["Member"] == null)
                                {
                                    TyVos.Add(name, "");
                                }
                                else
                                {
                                    TyVos.Add(name, node.Attributes["Member"].Value);
                                }
                            }
                        }
                    }
                }
                if (XMLQuanDoan.SelectSingleNode("QuanDoans").SelectNodes("QuanDoan") != null)
                {
                    foreach (XmlElement node in XMLQuanDoan.SelectSingleNode("QuanDoans").SelectNodes("QuanDoan"))
                    {
                        if (node.Attributes["Name"] != null)
                        {
                            string name = node.Attributes["Name"].Value;
                            if (!QuanDoan.ContainsKey(name))
                            {
                                if (node.Attributes["Member"] == null)
                                {
                                    QuanDoan.Add(name, "");
                                }
                                else
                                {
                                    QuanDoan.Add(name, node.Attributes["Member"].Value);
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        public CalendarEx()
        {
            InitializeComponent();
        }

        public static bool IsAutoCreaTeam = false;

        private void CalenderEx_Load(object sender, EventArgs e)
        {
            //lvTeamName.VirtualItems = Settings.Teams.Select(k => { var i = new ListViewItem(k.Key); i.Tag = k.Value; return i; }).ToList();
            //catch(Exception ex)
            //{
            //    MessageBox.Show(ex.Message + ex.StackTrace);
            //}
            //foreach (KeyValuePair<string, string> kvp in Balls)
            //{
            //    string name = kvp.Key;
            //    ListViewItem item = new ListViewItem(name);
            //    item.Tag = kvp.Value;
            //    listViewBallID.Items.Add(item);
            //}
            //foreach (KeyValuePair<string, string> kvp in TyVos)
            //{
            //    string name = kvp.Key;
            //    ListViewItem item = new ListViewItem(name);
            //    item.Tag = kvp.Value;
            //    lvTyVoId.Items.Add(item);
            //}
            //foreach (KeyValuePair<string, string> kvp in QuanDoan)
            //{
            //    string name = kvp.Key;
            //    ListViewItem item = new ListViewItem(name);
            //    item.Tag = kvp.Value;
            //    lvQuanDoan.Items.Add(item);
            //}
        }

        private void listViewTeamName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //listViewTeamMem.Items.Clear();
            //if (lvTeamName.SelectedVitualItem != null)
            //{            
            //    listViewTeamMem.Items.AddRange(Settings.Teams[lvTeamName.SelectedVitualItem.Text].Select(i => new ListViewItem(i)).ToArray());
            //    listViewTeamMem.Columns[0].Text = lvTeamName.SelectedVitualItem.Text;
            //}
            //else
            //{
            //    listViewTeamMem.Columns[0].Text = "Member";
            //}
        }

        private void btnAddTeam_Click(object sender, EventArgs e)
        {
            //if (!string.IsNullOrEmpty(txtTeam.Text))
            //{
            //    Settings.Teams[txtTeam.Text] = new List<string>();
            //    lvTeamName.Items.Add(new ListViewItem(txtTeam.Text));
            //}   
        }
     


        private void btnAllBallName_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtBallName.Text) && !Balls.ContainsKey(txtBallName.Text))
            {
                try
                {
                    XmlElement node = XMLBalls.CreateElement("Ball");
                    XmlAttribute atb = XMLBalls.CreateAttribute("Name");
                    atb.Value = txtBallName.Text;
                    node.Attributes.Append(atb);
                    XMLBalls.SelectSingleNode("/*").AppendChild(node);
                    ListViewItem item = new ListViewItem(txtBallName.Text);
                    item.Tag = "";
                    listViewBallID.Items.Add(item);
                    item.Selected = true;
                    Game.IniParser.Write("Item", "Balls", XMLBalls.OuterXml);
                }
                catch { }
            }
        }

        private void listViewBallID_SelectedIndexChanged(object sender, EventArgs e)
        {
            listViewBallMem.Items.Clear();
            if (listViewBallID.SelectedItems.Count > 0)
            {
                string members = listViewBallID.SelectedItems[0].Tag as string;
                foreach (string member in members.Split(';'))
                {
                    if (member.Split('-').Length == 2)
                    {
                        string id = member.Split('-')[0];
                        string name = member.Split('-')[1];
                        ListViewItem item = new ListViewItem(name);
                        item.Tag = id;
                        listViewBallMem.Items.Add(item);
                    }
                }
            }
        }

        private void deleteBall_Click(object sender, EventArgs e)
        {
            while (listViewBallID.SelectedItems.Count > 0)
            {
                try
                {
                    string ball = listViewBallID.SelectedItems[0].Text;
                    XmlNode node = XMLBalls.SelectSingleNode("//Balls/Ball[@Name=\"" + ball + "\"]");
                    XMLBalls.SelectSingleNode("/*").RemoveChild(node);
                    Balls.Remove(ball);
                    listViewBallID.SelectedItems[0].Remove();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + ex.StackTrace);
                    break;
                }
            }
            Game.IniParser.Write("Item", "Balls", XMLBalls.OuterXml);
        }

        private void deleteBallMem_Click(object sender, EventArgs e)
        {
            try
            {
                while (listViewBallMem.SelectedItems.Count > 0)
                {
                    string ball = listViewBallID.SelectedItems[0].Text;
                    XmlNode node = XMLBalls.SelectSingleNode("//Balls/Ball[@Name=\"" + ball + "\"]");
                    node.Attributes["Member"].Value = node.Attributes["Member"].Value.Replace(listViewBallMem.SelectedItems[0].Tag + "-" + listViewBallMem.SelectedItems[0].Text, "").Trim(';');
                    Balls[ball] = node.Attributes["Member"].Value;
                    listViewBallID.SelectedItems[0].Tag = node.Attributes["Member"].Value;
                    listViewBallMem.SelectedItems[0].Remove();
                }
                Game.IniParser.Write("Item", "Balls", XMLBalls.OuterXml);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        private void lvTyVoId_SelectedIndexChanged(object sender, EventArgs e)
        {
            lvTyVoMem.Items.Clear();
            if (lvTyVoId.SelectedItems.Count > 0)
            {
                string members = lvTyVoId.SelectedItems[0].Tag as string;
                foreach (string member in TDT.ListStringBetween(members, "[", "]"))
                {
                    ListViewItem item = new ListViewItem(member);
                    item.Tag = member;
                    lvTyVoMem.Items.Add(item);
                }
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            while (lvTyVoId.SelectedItems.Count > 0)
            {
                try
                {
                    string ball = lvTyVoId.SelectedItems[0].Text;
                    XmlNode node = XMLTyVo.SelectSingleNode("//TyVos/TyVo[@Name=\"" + ball + "\"]");
                    XMLTyVo.SelectSingleNode("/*").RemoveChild(node);
                    TyVos.Remove(ball);
                    lvTyVoId.SelectedItems[0].Remove();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + ex.StackTrace);
                    break;
                }
            }
            Game.IniParser.Write("Item", "TyVos", XMLTyVo.OuterXml);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            while (lvQuanDoan.SelectedItems.Count > 0)
            {
                try
                {
                    string ball = lvQuanDoan.SelectedItems[0].Text;
                    XmlNode node = XMLQuanDoan.SelectSingleNode("//QuanDoans/QuanDoan[@Name=\"" + ball + "\"]");
                    XMLQuanDoan.SelectSingleNode("/*").RemoveChild(node);
                    QuanDoan.Remove(ball);
                    lvQuanDoan.SelectedItems[0].Remove();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + ex.StackTrace);
                    break;
                }
            }
            Game.IniParser.Write("Item", "QuanDoan", XMLQuanDoan.OuterXml);
        }

        private void lvQuanDoan_SelectedIndexChanged(object sender, EventArgs e)
        {
            lvQuanDoanMem.Items.Clear();
            if (lvQuanDoan.SelectedItems.Count > 0)
            {
                string members = lvQuanDoan.SelectedItems[0].Tag as string;
                foreach (string member in TDT.ListStringBetween(members, "[", "]"))
                {
                    ListViewItem item = new ListViewItem(member);
                    item.Tag = member;
                    lvQuanDoanMem.Items.Add(item);
                }
            }
        }

        private void lvQuanDoanMem_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                string na = "";
                if (XMLQuanDoan.SelectSingleNode("QuanDoans").SelectNodes("QuanDoan") != null)
                {
                    foreach (XmlElement node in XMLQuanDoan.SelectSingleNode("QuanDoans").SelectNodes("QuanDoan"))
                    {
                        if (node.Attributes["Name"] != null)
                        {
                            string name = node.Attributes["Name"].Value;
                      
                            if (name == lvQuanDoan.SelectedItems[0].Text)
                            {
                                na = name;
                                string mem = "";
                                foreach (ListViewItem i in lvQuanDoanMem.Items)
                                {
                                    mem += "[" + i.Text + "]";
                                }
                                node.Attributes["Member"].Value = mem;
                            }
                        }
                    }
                }
                Game.IniParser.Write("Item", "QuanDoan", CalendarEx.XMLQuanDoan.OuterXml);
                LoadXML();
                lvQuanDoan.Items.Clear();
                lvQuanDoanMem.Items.Clear();
                foreach (KeyValuePair<string, string> kvp in QuanDoan)
                {
                    string name = kvp.Key;
                    ListViewItem item = new ListViewItem(name);
                    item.Tag = kvp.Value;
                    lvQuanDoan.Items.Add(item);
                }
                foreach(ListViewItem i in lvQuanDoan.Items)
                {
                    if(i.Text == na)
                    {
                        i.Selected = true;
                    }
                }
            }
            catch
            {
             
            }
        }

        private void cboSearchName_DropDown(object sender, EventArgs e)
        {
            cboPlayer.Items.Clear();
            cboPlayer.Items.AddRange(Main.Instance.AllOnelineGame.Select(o => o.TLBB.Name).ToArray());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //if (!string.IsNullOrEmpty(cboPlayer.Text) && !string.IsNullOrEmpty(lvTeamName.SelectedItems[0].Text))
            //{
            //    listViewTeamMem.Items.Add(new ListViewItem(cboPlayer.Text));
            //    Settings.Teams[lvTeamName.SelectedItems[0].Text].Add(cboPlayer.Text);
            //}
        }

        private void listViewTeamMem_DoubleClick(object sender, EventArgs e)
        {
            //while(listViewTeamMem.SelectedItems.Count > 0)
            //{
            //    Settings.Teams[lvTeamName.SelectedItems[0].Text].Remove(listViewTeamMem.SelectedItems[0].Text);
            //    listViewTeamMem.SelectedItems[0].Remove();
            //}
        }

        private void listViewTeamName_DoubleClick(object sender, EventArgs e)
        {
            //lvTeamName.VitualItems = lvTeamName.VitualItems.Except(lvTeamName.SelectedVitualItems).ToList();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
           // (((ContextMenuStrip)((ToolStripItem)sender).Owner).SourceControl as ListViewEx).PerformDoubleClick(e);
        }

        private void lvTeamName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
