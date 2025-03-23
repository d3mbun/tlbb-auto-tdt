using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Windows.Forms;

namespace _i
{
    public partial class BuyFunc : UserControl
    {
        public BuyFunc()
        {
            InitializeComponent();
        }

        private void BuyFunc_Load(object sender, EventArgs e)
        {
            AutoRMF();
        }

        public static bool IsFullChucNang { get; set; }

        private void AutoRMF()
        {
            if (Global.IsBankFull)
            {
                ReMoveFunc("buybankfull");                
            }
            if (Global.IsBinhThanh)
            {
                ReMoveFunc("buybinhthanh");
            }
            if (Global.IsThuHoachHoa)
                ReMoveFunc("buythex");
            if (Global.IsChucPhucCungMay)
                ReMoveFunc("buycpcm");
            if (Global.IsHopDienTichVoHon)
                ReMoveFunc("hopdientich");
            if (Global.Is9Sao)
                ReMoveFunc("buy9sao");
            if (Global.IsMaTac)
                ReMoveFunc("buymatac");
            if (Global.IsBaoDoHiem)
                ReMoveFunc("baodohiem");
            if (Global.IsSuaTrangBi)
                ReMoveFunc("buystb");
            if (Global.IsPhucDia)
                ReMoveFunc("buyphucdia");
            if (Global.IsChienBi)
                ReMoveFunc("buychienbi");
            if (Global.IsTamThan)
                ReMoveFunc("buytam");
            if (Global.IsGom)
                ReMoveFunc("buygom");
            if (Global.RemoveAd)
                ReMoveFunc("rmad");
            if (Global.IsCanQuet)
                ReMoveFunc("buycanquet");
            if (Global.IsAutoCreate)
                ReMoveFunc("buycreate");
            if (Global.IsSinhTieu)
                ReMoveFunc("buysinhtieu");
            if (Global.IsCode)
                ReMoveFunc("buynhapcode");
            if (Global.IsGomKNB)
                ReMoveFunc("buyshopknb");
            if (Global.IsBachBao)
                ReMoveFunc("buybachbao");
            if (Global.IsQuanDoanBinhThanh)
                ReMoveFunc("buyqdbt");
            if(Global.IsDungDoat)
                ReMoveFunc("dungdoat");
            if (Global.PSShop)
                ReMoveFunc("psshop");
            if (Global.IsThangCap)
                ReMoveFunc("buythangcap");
            if (Global.IsQuanSonHai)
                ReMoveFunc("buyqsh");
            if (Global.IsTamKy)
                ReMoveFunc("tamky");
            if (IsFullChucNang)
                ReMoveFunc("fullchucnang");
        }

        void ReMoveFunc(string tag)
        {
            try
            {
                foreach (ListViewItem i in lvFunc.Items)
                {
                    if (i.Tag != null)
                    {
                        if (i.Tag.ToString() == tag)
                        {
                            i.ForeColor = Color.Gray;
                            i.Tag = null;
                        }
                    }
                }
            }
            catch { }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(lvFunc.SelectedItems.Count > 0)
            {
                if(lvFunc.SelectedItems[0].Tag == null)
                {
                    MessageBox.Show(this, "Vui lòng chọn mục cần mua", "MicroAuto", MessageBoxButtons.OK);
                }
                else
                {
                   if( MessageBox.Show(this, "Việc mua " + lvFunc.SelectedItems[0].Text 
                        + "\r\nSẽ tiêu hao "
                        + lvFunc.SelectedItems[0].SubItems[1].Text + "\r\nBạn có chắc chắn muốn mua", "MicroAuto", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Enabled = false;
                        Poster posterBuy = new Poster();
                        posterBuy.Url = URL.HomePage + "microauto/user.php";
                        posterBuy.Data = "cmd=" + lvFunc.SelectedItems[0].Tag + "&email=" + HttpUtility.UrlEncode(User.Email) + "&pass=" + HttpUtility.UrlEncode(User.Pass);
                        posterBuy.Control = this;
                        posterBuy.AutoReconnect = true;
                        posterBuy.Completed += (ss, ee) =>
                        {
                            string msg = posterBuy.Response;
                            Poster posterUserInfo = new Poster();
                            posterUserInfo.Url = URL.HomePage + "microauto/user.php";
                            posterUserInfo.Control = this;
                            posterUserInfo.AutoReconnect = true;
                            posterUserInfo.Data = "cmd=userinfo&email=" + HttpUtility.UrlEncode(User.Email) + "&pass=" + HttpUtility.UrlEncode(User.Pass);
                            posterUserInfo.Completed += (xx, yy) =>
                            {
                                MessageBox.Show(this, msg, "MicroAuto", MessageBoxButtons.OK);
                                Enabled = true;
                                var poster = posterUserInfo;              
                                if (poster.Response.Contains("&ksbong"))
                                {
                                    Global.IsChongKsBong = true;
                                    string point = poster.Response.Substring(poster.Response.IndexOf("&ksbong"));
                                    Global.ChongX = TDT.ParseInt(point);
                                    Global.ChongY = TDT.ParseInt(point.Replace(Global.ChongX + ",", ""));
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
                                if (poster.Response.Contains("&bankfull"))
                                {
                                    Global.IsBankFull = true;
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
                                if (poster.Response.Contains("&binhthanh"))
                                {
                                    Global.IsBinhThanh = true;
                                }
                                if (poster.Response.Contains("&bankfull"))
                                {
                                    Global.IsBankFull = true;
                                }
                                if (poster.Response.Contains("&bachbao"))
                                {
                                    Global.IsBachBao = true;
                                }
                                if (poster.Response.Contains("&qdbt"))
                                {
                                    //Global.IsQuanDoanBinhThanh = true;
                                }
                                if (poster.Response.Contains("&dungdoat"))
                                {
                                    Global.IsDungDoat = true;
                                }
                                if (poster.Response.Contains("&psshop"))
                                {
                                    Global.PSShop = true;
                                }




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
                                AutoRMF();

                            };
                            posterUserInfo.Post();



                         



                        };
                        posterBuy.Post();
                    }
                }
            }
        }

    }
}
