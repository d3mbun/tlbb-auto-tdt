using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Web;
using System.Windows.Forms;

namespace _i
{

    internal class Poster
    {
        public static int MaxConn { get; set; } = 0;
        public static int AllowMaxConn { get; set; } = int.MaxValue;

        public int Delay { get; set; } = 1;

        public object Tag { get; set; }


        public static string CurlPost(string url, string data = null, string cookie = null, bool autoreconnect = false)
        {
            while (true)
            {
                try
                {
                    url = url.Replace("remlaw", "microauto");
                    url = url.Replace("http://", "https://");

                    url = url.Replace("tieudattai.org", "45.77.129.211");
                    url = url.Replace("www.", "");
                    url = url.Replace("https://", "http://");
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.ServicePoint.Expect100Continue = false;
                    request.AllowAutoRedirect = false;
                    request.Method = "POST";
                    request.UserAgent = Poster.UserAgent;
                    request.Headers.Add("api-key: zMXF92H7U8t6ldgTWbjmSXAkGwdwxiA8");
                    request.Headers.Add("speed: -1");
                    request.Headers.Add("voice: lannhi");
                    request.Headers.Add("Accept-Encoding: *");
                    request.Accept = "*/*";
                    request.Connection = "keepalive";
                    request.Referer = "https://www.facebook.com/";
                    request.Headers.Add("Cookie: " + cookie);
                    request.AutomaticDecompression = (DecompressionMethods.GZip | DecompressionMethods.Deflate);
                    request.Proxy = null;
                    request.ContentType = "application/x-www-form-urlencoded";
                    if (data != null)
                    {
                        StreamWriter streamWriter = new StreamWriter(request.GetRequestStream());
                        streamWriter.Write(data);
                        streamWriter.Close();
                    }
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    StreamReader streamReader = new StreamReader(response.GetResponseStream());
                    string re = streamReader.ReadToEnd();
                    streamReader.Close();
                    response.Close();
                    return re;
                }
                catch(Exception ex)
                {
                    if (!autoreconnect)
                        return ex.Message;
                    else
                        continue;
                }
            }
        }

        public static string CurlGet(string url, string cookie = null, bool AutoReconnect = false)
        {
            while (true)
            {
                try
                {
              
                    url = url.Replace("remlaw", "microauto");
                    url = url.Replace("http://", "https://");
                    url = url.Replace("tieudattai.org", "45.77.129.211");
                    url = url.Replace("www.", "");
                    url = url.Replace("https://", "http://");
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    if (cookie != null)
                    {
                        request.Headers.Add("Cookie: " + cookie);
                    }
                    request.Method = "GET";
                    request.AllowAutoRedirect = false;
                    request.UserAgent = Poster.UserAgent;
                    request.Headers.Add("Accept-Encoding: *");
                    request.Connection = "keepalive";
                    request.Proxy = null;
                    request.ServicePoint.Expect100Continue = false;
                    request.AutomaticDecompression = (DecompressionMethods.GZip | DecompressionMethods.Deflate);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    for (int i = 0; i < response.Headers.Count; i++)
                    {
                        if (response.Headers.Keys[i] == "Location")
                        {
                            response.Close();
                            return response.Headers[i];
                        }
                    }
                    StreamReader streamReader = new StreamReader(response.GetResponseStream());
                    string re = streamReader.ReadToEnd();
                    streamReader.Close();
                    response.Close();
                    return re;
                }
                catch (Exception ex)
                {
                    if (!AutoReconnect)
                        return ex.Message;
                    else
                        continue;
                }
                
            }
        }

        public static Bitmap CurlGetStream(string url, string cookie = null)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            if (cookie != null)
            {
                request.Headers.Add("Cookie: " + cookie);
            }
            request.Method = "GET";
            request.AllowAutoRedirect = false;
            request.UserAgent = Poster.UserAgent;
            request.Headers.Add("Accept-Encoding: *");
            request.Connection = "keepalive";
            request.Proxy = null;
            request.ServicePoint.Expect100Continue = false;
            request.AutomaticDecompression = (DecompressionMethods.GZip | DecompressionMethods.Deflate);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream Stream = response.GetResponseStream();
            Image img = Image.FromStream(Stream);
            return new Bitmap(img);
        }

        public string Data { get; set; } = string.Empty;

        public void Post()
        {            
            ChangeHost();
            new Thread(new ThreadStart(PostThread))
            {
                IsBackground = true
            }.Start();
        }

        public Stream Stream { get; set; }

        public bool IsStream { get; set; }

        private void ChangeHost()
        {
            //if (ErrorUrl.Contains("tieudattai.com"))
            //{
            //    Url = Url.Replace("tieudattai.com", "tieudattai.info");
            //    Url = Url.Replace("tieudattai.vn", "tieudattai.info");
            //}
            //if (ErrorUrl.Contains("tieudattai.info"))
            //{
            //    Url = Url.Replace("tieudattai.info", "tieudattai.vn");
            //    Url = Url.Replace("tieudattai.com", "tieudattai.vn");
            //}
            //if (ErrorUrl.Contains("tieudattai.vn"))
            //{
            //    Url = Url.Replace("tieudattai.vn", "tieudattai.com");
            //    Url = Url.Replace("tieudattai.info", "tieudattai.com");
            //}
        }

        public void Get()
        {
            ChangeHost();
            new Thread(new ThreadStart(GetThread))
            {
                IsBackground = true
            }.Start();
        }

        public void Abort()
        {
            try
            {
                request.Abort();
            }
            catch { }
        }

        public string Url = string.Empty;
        public string DataEx { get; set; } = string.Empty;
        public string Referer = string.Empty;
        public string HTML = string.Empty;
        public string Error = string.Empty;
        public string CookieToString = string.Empty;
        public string Heads = string.Empty;
        public Control Control = null;
        public CookieContainer Cookie = new CookieContainer();
        public static int ErrorCount;

        public event EventHandler Completed;

        private delegate void CallBack();

        public bool AutoReconnect
        {
            get;
            set;
        }

        public bool IsError
        {
            get
            {
                return Error != string.Empty;
            }
        }

        public string Response
        {
            get
            {
                if (IsError)
                    return Error;
                return HTML;
            }

            set
            {
                HTML = value;
            }
        }
        public static string IsPri = "&ispri=" + TDT.Bool2Int(false);
        public static string IsMulted = "&multed=" + TDT.Bool2Int(false);
        private HttpWebRequest RequestGet(string url, CookieContainer cookie)
        {
            request = (HttpWebRequest)WebRequest.Create(url);
            request.CookieContainer = cookie;
            request.Method = "GET";
            request.UserAgent = UserAgent;
            request.Headers.Add("Accept-Encoding: *");
            request.Connection = "keepalive";

            request.Proxy = null;
            request.ServicePoint.Expect100Continue = false;
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            return request;
        }

        private HttpWebRequest request;

        private HttpWebRequest RequestPost(string url, string data, CookieContainer cookie, string referer)
        {
            request = (HttpWebRequest)WebRequest.Create(url);
          
            try { request.Referer = referer; }
            catch { }

            request.ServicePoint.Expect100Continue = false;
            request.AllowAutoRedirect = true;
            request.CookieContainer = cookie;
            request.Method = "POST";
            request.UserAgent = UserAgent;
            request.Headers.Add("Accept-Encoding: *");
            request.Accept = "*/*";
            request.Connection = "keepalive";
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            request.Proxy = null;
            request.ContentType = "application/x-www-form-urlencoded";


            StreamWriter writer = new StreamWriter(request.GetRequestStream());

            

            writer.Write(data);
            writer.Close();
            
            return request;
        }
        public bool IsRemove { get; set; }

        public bool IsGet { get; set; }

        private void PostThread()
        {
          
            Thread.Sleep(Delay);
            while (MaxConn >= AllowMaxConn)
                Thread.Sleep(1000);
            MaxConn++;
            if (IsGet)
            {
                Url += "&serial=" + FingerPrint.Serial + "&version=" + HttpUtility.UrlEncode(Global.Version) + "&li=" + "&md=" + Global.SelfMd5 + "&vip=" + Global.IsVIP + IsPri + IsMulted;
            }
            else
            {
                if (!IsRemove)
                    Data += "&serial=" + FingerPrint.Serial + "&version=" + HttpUtility.UrlEncode(Global.Version) + "&li=" + "&md=" + Global.SelfMd5 + "&vip=" + Global.IsVIP + IsPri + IsMulted;
            }
          
            try
            {
                PostFunc();
            }
            catch (Exception ex)
            {
                if (AutoReconnect)
                {
                    while (true)
                    {
                        try
                        {
                            PostFunc();
                            if (Response.Contains("Resource Limit"))
                            {
                                this.Error = ex.Message;
                                ErrorCount++;
                                ErrorUrl = Url;
                                ChangeHost();
                            }
                            else
                            {
                                break;
                            }
                        }
                        catch
                        {
                            this.Error = ex.Message;
                            ErrorCount++;
                            ErrorUrl = Url;
                            ChangeHost();
                        }
                    }
                }
                else
                {
                    this.Error = ex.Message;
                    ErrorCount++;
                    ErrorUrl = Url;
                }
            }
            MaxConn--;
            OnCompleted();
           
        }

        private void PostFunc()
        {
            HttpWebRequest request = RequestPost(this.Url, this.Data, this.Cookie, this.Referer);
            GetResponse(this, request);
        }

        private void GetThread()
        {
            Thread.Sleep(Delay);
            while (MaxConn >= AllowMaxConn)
                Thread.Sleep(1000);
            MaxConn++;
            try
            {
                HttpWebRequest request = RequestGet(this.Url, this.Cookie);
                GetResponse(this, request);
            }
            catch (Exception ex)
            {
                this.Error = ex.Message;
                ErrorCount++;
                ErrorUrl = Url;
            }
            MaxConn--;
            OnCompleted();
        }

        public static string ErrorUrl = "";

        private void OnCompleted()
        {           
            try
            {
                if (Control != null && Control.IsDisposed)
                    return;
                if (Completed == null)
                    return;
                if (Control != null)
                {
                    if (this.Control.InvokeRequired)
                    {
                        this.Control.Invoke(new CallBack(OnCompleted));
                    }
                    else
                    {
                        Completed(this, null);
                    }
                }
                else
                {
                    Completed(this, null);
                }
            }
            catch { }
        }

        private static string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/" + new Random().Next(70, 90) + ".0.4280.141 Safari/537.36";

        public static void DisableValidate()
        {
            //if (new FileInfo(Application.ExecutablePath).Length != 2075136)
            //{
            //    Global.HookMessage = -1;
            //}
            ServicePointManager.DefaultConnectionLimit = int.MaxValue;
            SetAllowUnsafeHeaderParsing20();
            ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback(BypassAllCertificateStuff);
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
        }

        public void SetCookie(string cookie, string domain = "facebook.com")
        {
            foreach (string cooks in cookie.Split(';'))
            {
                if (cooks.Split('=').Length == 2)
                {
                    try
                    {
                        Cookie.Add(new Cookie(HttpUtility.UrlEncode(cooks.Split('=')[0].Trim()), cooks.Split('=')[1].Trim(), "/", domain));
                        Cookie.Add(new Cookie(HttpUtility.UrlEncode(cooks.Split('=')[0].Trim()), cooks.Split('=')[1].Trim(), "/", "www." + domain));

                    }
                    catch { }
                }
            }
        }

        public static bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public static bool SetAllowUnsafeHeaderParsing20()
        {
            //Get the assembly that contains the internal class
            Assembly aNetAssembly = Assembly.GetAssembly(typeof(System.Net.Configuration.SettingsSection));
            if (aNetAssembly != null)
            {
                //Use the assembly in order to get the internal type for the internal class
                Type aSettingsType = aNetAssembly.GetType("System.Net.Configuration.SettingsSectionInternal");
                if (aSettingsType != null)
                {
                    //Use the internal static property to get an instance of the internal settings class.
                    //If the static instance isn't created allready the property will create it for us.
                    object anInstance = aSettingsType.InvokeMember("Section",
                    BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.NonPublic, null, null, new object[] { });

                    if (anInstance != null)
                    {
                        //Locate the private bool field that tells the framework is unsafe header parsing should be allowed or not
                        FieldInfo aUseUnsafeHeaderParsing = aSettingsType.GetField("useUnsafeHeaderParsing", BindingFlags.NonPublic | BindingFlags.Instance);
                        if (aUseUnsafeHeaderParsing != null)
                        {
                            aUseUnsafeHeaderParsing.SetValue(anInstance, true);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private static bool BypassAllCertificateStuff(object sender, X509Certificate cert, X509Chain chain, System.Net.Security.SslPolicyErrors error)
        {
            return true;
        }

        private static Poster GetResponse(Poster poster, HttpWebRequest request)
        {
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            foreach (Cookie cook in response.Cookies)
            {
                string domain = cook.Domain.TrimStart('.').Replace("www.", "");
                poster.Cookie.Add(new Cookie(cook.Name, cook.Value, cook.Path, domain));
                poster.Cookie.Add(new Cookie(cook.Name, cook.Value, cook.Path, "www." + domain));
                poster.CookieToString += cook.Name + "=" + cook.Value + ";";
            }

            for (int i = 0; i < response.Headers.Count; ++i)
                poster.Heads += response.Headers.Keys[i] + ": " + response.Headers[i] + "\n";
            poster.Stream = response.GetResponseStream();
            StreamReader sr = new StreamReader(poster.Stream);
            if (poster.IsImage)
            {
                try
                {
                    Image img = Image.FromStream(poster.Stream);
                    poster.bitmap = new Bitmap(img);
                }
                catch { }
            }
            poster.HTML = sr.ReadToEnd();
            if (!poster.IsStream)
                sr.Close();

            return poster;
        }

        public Bitmap bitmap;

        public bool IsImage { get; set; }
    }
}