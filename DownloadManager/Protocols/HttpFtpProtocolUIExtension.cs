using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Update
{
    class HttpFtpProtocolUIExtension : IUIExtension
    {
        #region IUIExtension Members

        public System.Windows.Forms.Control[] CreateSettingsView()
        {
            return new Control[] { new Proxy() };
        }

        public void PersistSettings(System.Windows.Forms.Control[] settingsView)
        {
            Proxy proxy = new Proxy();

            Settings.UseProxy = proxy.UseProxy;
            Settings.ProxyAddress = proxy.ProxyAddress;
            Settings.ProxyPort = proxy.ProxyPort;
            Settings.ProxyByPassOnLocal = proxy.ProxyByPassOnLocal;
            Settings.ProxyUserName = proxy.ProxyUserName;
            Settings.ProxyPassword = proxy.ProxyPassword;
            Settings.ProxyDomain = proxy.ProxyDomain;
        }

        #endregion
    }
}
