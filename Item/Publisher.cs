using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace _i
{
    partial class Publisher : UserControl
    {
        public static XmlDocument XML = new XmlDocument();

        public Publisher()
        {
            InitializeComponent();
        }

        public static void LoadXML()
        {
            try
            {
                try
                {
                    Publisher.XML.LoadXml(Game.IniParser.Read("Item", "Publishers"));
                }
                catch
                {
                    try
                    {
                        Publisher.XML.LoadXml(Properties.Resources.Publishers);
                        SaveXML();
                    }
                    catch
                    {

                    }
                }
            }
            catch
            {
                if (Publisher.XML.SelectSingleNode("/*") == null)
                {
                    Publisher.XML.InsertBefore(Publisher.XML.CreateXmlDeclaration("1.0", "UTF-8", null), Publisher.XML.DocumentElement);
                    Publisher.XML.AppendChild(Publisher.XML.CreateNode(XmlNodeType.Element, "Publishers", ""));
                    SaveXML();
                }
            }
        }

        public static string Version
        {
            get
            {
                try
                {
                    return XML.SelectSingleNode("Publishers").SelectSingleNode("Version").InnerText;
                }
                catch
                {
                    return "1.0";
                }
            }
            set
            {
                try
                {
                    XML.SelectSingleNode("Publishers").SelectSingleNode("Version").InnerText = Global.PublisherVersion;
                    SaveXML();
                }
                catch
                {
                    XmlElement version = XML.CreateElement("Version");
                    version.InnerText = value;
                    XML.SelectSingleNode("Publishers").AppendChild(version);
                    SaveXML();
                }
            }
        }

        public static void SaveXML()
        {
            Game.IniParser.Write("Item", "Publishers", XML.OuterXml);
        }

        private void Publisher_Load(object sender, EventArgs e)
        {            
            LoadNPH();
        }

        private void LoadNPH()
        {
            listViewNPH.Items.Clear();
            try
            {
                foreach (XmlNode node in XML.SelectSingleNode("Publishers").SelectNodes("Publisher"))
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = node.Attributes["Name"].Value;
                    item.Tag = node;
                    listViewNPH.Items.Add(item);
                }
            }
            catch { }
            if (listViewNPH.SelectedItems.Count == 0 && listViewNPH.Items.Count > 0)
            {
                listViewNPH.Items[0].Selected = true;
                listViewNPH.Select();
            }
        }

        private void menuAddServer_Click(object sender, EventArgs e)
        {
            if (listViewNPH.SelectedItems.Count == 0)
                return;
            string input = Microsoft.VisualBasic.Interaction.InputBox("Nhập vào tên Server muốn thêm", "MicroAuto", "", -1, -1);
            if (input != "")
            {
                XmlElement Server = XML.CreateElement("Server");
                Server.InnerText = input;
                try
                {
                    XML.SelectSingleNode("//Publishers/Publisher[@Name=\"" + listViewNPH.SelectedItems[0].Text + "\"]/Servers").AppendChild(Server);
                }
                catch
                {
                    XmlNode Servers = XML.CreateElement("Servers");
                    XML.SelectSingleNode("//Publishers/Publisher[@Name=\"" + listViewNPH.SelectedItems[0].Text + "\"]").AppendChild(Servers);
                    XML.SelectSingleNode("//Publishers/Publisher[@Name=\"" + listViewNPH.SelectedItems[0].Text + "\"]/Servers").AppendChild(Server);
                }
                SaveXML();
                LoadServer();
            }
        }

        private void listViewNPH_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listViewNPH.SelectedItems.Count == 0)
                return;
            listViewServer.Items.Clear();
            try
            {
                txtPath.Text = XML.SelectSingleNode("//Publishers/Publisher[@Name=\"" + listViewNPH.SelectedItems[0].Text + "\"]").Attributes["Path"].Value;
            }
            catch
            {
                txtPath.Text = "";
            }
            Text = "Nhà Phát Hành - " + listViewNPH.SelectedItems[0].Text;
            LoadServer();
            LoadTail();
        }

        private void LoadServer()
        {
            listViewServer.Items.Clear();
            int cnt = 0;
            try
            {
                foreach (XmlNode node in XML.SelectSingleNode("//Publishers/Publisher[@Name=\"" + listViewNPH.SelectedItems[0].Text + "\"]/Servers").SelectNodes("Server"))
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = cnt++.ToString();
                    item.SubItems.Add(node.InnerText);
                    item.Tag = node;
                    listViewServer.Items.Add(item);
                }
            }
            catch { }
        }

        private void LoadTail()
        {
            listViewTail.Items.Clear();
            int cnt = 0;
            try
            {
                foreach (XmlNode node in XML.SelectSingleNode("//Publishers/Publisher[@Name=\"" + listViewNPH.SelectedItems[0].Text + "\"]/Tails").SelectNodes("Tail"))
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = cnt++.ToString();
                    item.SubItems.Add(node.InnerText);
                    item.Tag = node;
                    listViewTail.Items.Add(item);
                }
            }
            catch { }
        }

        private void menuDeleteServer_Click(object sender, EventArgs e)
        {
            if (listViewNPH.SelectedItems.Count == 0 || listViewServer.SelectedItems.Count == 0)
                return;
            if (MessageBox.Show(this, "Bạn có muốn xóa Server đã chọn", "MicroAuto", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                foreach (ListViewItem item in listViewServer.SelectedItems)
                {
                    XmlNode node = (XmlNode)item.Tag;
                    XML.SelectSingleNode("//Publishers/Publisher[@Name=\"" + listViewNPH.SelectedItems[0].Text + "\"]/Servers").RemoveChild(node);
                    SaveXML();
                }
                LoadServer();
            }            
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Bạn có muốn xóa mọi thiết lập và dùng thiết lập mặc định", "MicroAuto", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Game.IniParser.Write("Item", "Publishers", Properties.Resources.Publishers);
            }
            LoadXML();
            LoadNPH();
        }

        private void menuEditServer_Click(object sender, EventArgs e)
        {
            if (listViewNPH.SelectedItems.Count == 0 || listViewServer.SelectedItems.Count == 0)
                return;
            XmlNode node = (XmlNode)listViewServer.SelectedItems[0].Tag;
            string input = Microsoft.VisualBasic.Interaction.InputBox("Nhập vào tên Server muốn đổi", "MicroAuto", node.InnerText, -1, -1);
            if (input != "" && input != node.InnerText)
            {                
                node.InnerText = input;
                SaveXML();
                LoadServer();
            }
        }

        private void menuAddNPH_Click(object sender, EventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("Nhập vào tên Nhà Phát Hành muốn thêm", "MicroAuto", "", -1, -1);
            if (input != "")
            {
                XmlElement NPH = XML.CreateElement("Publisher");
                XmlAttribute atb = XML.CreateAttribute("Name");
                atb.Value = input;
                NPH.Attributes.Append(atb);
                XML.SelectSingleNode("//Publishers").AppendChild(NPH);
                SaveXML();
                LoadNPH();
            }
        }

        private void menuEditNPH_Click(object sender, EventArgs e)
        {
            if (listViewNPH.SelectedItems.Count == 0)
                return;
            XmlNode node = (XmlNode)listViewNPH.SelectedItems[0].Tag;
            string input = Microsoft.VisualBasic.Interaction.InputBox("Nhập vào tên Nhà Phát Hành muốn đổi", "MicroAuto", node.Attributes["Name"].Value, -1, -1);
            if (input != "" && input != node.Attributes["Name"].Value)
            {
                node.Attributes["Name"].Value = input;
                SaveXML();
                LoadNPH();
            }
        }

        private void menuDeleteNPH_Click(object sender, EventArgs e)
        {
            if (listViewNPH.SelectedItems.Count == 0)
                return;
            if (MessageBox.Show(this, "Bạn có muốn xóa Nhà Phát Hành đã chọn", "MicroAuto", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                foreach (ListViewItem item in listViewNPH.SelectedItems)
                {
                    XmlNode node = (XmlNode)item.Tag;
                    XML.SelectSingleNode("//Publishers").RemoveChild(node);
                    SaveXML();
                }
                LoadNPH();
            }
        }

        private void menuAddTail_Click(object sender, EventArgs e)
        {
            if (listViewNPH.SelectedItems.Count == 0)
                return;
            string input = Microsoft.VisualBasic.Interaction.InputBox("Nhập vào tên đuôi muốn thêm", "MicroAuto", "", -1, -1);
            if (input != "")
            {
                XmlElement Tail = XML.CreateElement("Tail");
                Tail.InnerText = input;
                try
                {
                    XML.SelectSingleNode("//Publishers/Publisher[@Name=\"" + listViewNPH.SelectedItems[0].Text + "\"]/Tails").AppendChild(Tail);
                }
                catch
                {
                    XmlNode Tails = XML.CreateElement("Tails");
                    XML.SelectSingleNode("//Publishers/Publisher[@Name=\"" + listViewNPH.SelectedItems[0].Text + "\"]").AppendChild(Tails);
                    XML.SelectSingleNode("//Publishers/Publisher[@Name=\"" + listViewNPH.SelectedItems[0].Text + "\"]/Tails").AppendChild(Tail);
                }
                SaveXML();
                LoadTail();
            }
        }

        private void menuEditTail_Click(object sender, EventArgs e)
        {
            if (listViewNPH.SelectedItems.Count == 0 || listViewTail.SelectedItems.Count == 0)
                return;
            XmlNode node = (XmlNode)listViewTail.SelectedItems[0].Tag;
            string input = Microsoft.VisualBasic.Interaction.InputBox("Nhập vào tên đuôi muốn đổi", "MicroAuto", node.InnerText, -1, -1);
            if (input != "" && input != node.InnerText)
            {
                node.InnerText = input;
                SaveXML();
                LoadTail();
            }
        }

        private void menuDeleteTail_Click(object sender, EventArgs e)
        {
            if (listViewNPH.SelectedItems.Count == 0 || listViewTail.SelectedItems.Count == 0)
                return;
            if (MessageBox.Show(this, "Bạn có muốn xóa đuôi đã chọn", "MicroAuto", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                foreach (ListViewItem item in listViewTail.SelectedItems)
                {
                    XmlNode node = (XmlNode)item.Tag;
                    XML.SelectSingleNode("//Publishers/Publisher[@Name=\"" + listViewNPH.SelectedItems[0].Text + "\"]/Tails").RemoveChild(node);
                    SaveXML();
                }
                LoadTail();
            }  
        }

        private void btnPath_Click(object sender, EventArgs e)
        {
            if (listViewNPH.SelectedItems.Count == 0)
                return;
            OpenFileDialog openFile = new OpenFileDialog();
            //openFile.Filter = ".* | .*";
            if (openFile.ShowDialog(this) == DialogResult.OK)
            {
                try
                {                    
                    XML.SelectSingleNode("//Publishers/Publisher[@Name=\"" + listViewNPH.SelectedItems[0].Text + "\"]").Attributes["Path"].Value = openFile.FileName;
                    txtPath.Text = openFile.FileName;
                    SaveXML();
                }
                catch
                {
                    XmlAttribute atb = XML.CreateAttribute("Path");
                    atb.Value = openFile.FileName;
                    XML.SelectSingleNode("//Publishers/Publisher[@Name=\"" + listViewNPH.SelectedItems[0].Text + "\"]").Attributes.Append(atb);
                    txtPath.Text = openFile.FileName;
                    SaveXML();
                }
            }
        }

        private void listViewServer_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                XmlNode root = XML.SelectSingleNode("//Publishers/Publisher[@Name=\"" + listViewNPH.SelectedItems[0].Text + "\"]/Servers");
                root.RemoveAll();
                foreach (ListViewItem item in listViewServer.Items)
                {
                    XmlElement node = (XmlElement)item.Tag;
                    root.AppendChild(node);
                }
                SaveXML();
                LoadServer();
            }
            catch { }
        }

        private void listViewTail_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                XmlNode root = XML.SelectSingleNode("//Publishers/Publisher[@Name=\"" + listViewNPH.SelectedItems[0].Text + "\"]/Tails");
                root.RemoveAll();
                foreach (ListViewItem item in listViewTail.Items)
                {
                    XmlElement node = (XmlElement)item.Tag;
                    root.AppendChild(node);
                }
                LoadTail();
            }
            catch { }
        }

        private void listViewNPH_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                XmlNode root = XML.SelectSingleNode("//Publishers");
                root.RemoveAll();
                foreach (ListViewItem item in listViewNPH.Items)
                {
                    XmlElement node = (XmlElement)item.Tag;
                    root.AppendChild(node);
                }
                LoadNPH();
            }
            catch { }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}
