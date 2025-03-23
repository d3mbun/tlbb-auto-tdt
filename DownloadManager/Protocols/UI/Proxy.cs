using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Update
{
    partial class Proxy : UserControl
    {
        public Proxy()
        {
            this.Text = "Proxy";
            InitializeComponent();

            UseProxy = Settings.UseProxy;
            ProxyAddress = Settings.ProxyAddress;
            ProxyPort = Settings.ProxyPort;
            ProxyByPassOnLocal = Settings.ProxyByPassOnLocal;
            ProxyUserName = Settings.ProxyUserName;
            ProxyPassword = Settings.ProxyPassword;
            ProxyDomain = Settings.ProxyDomain;

            UpdateControls();
        }

        public bool UseProxy
        {
            get
            {
                return chkUseProxy.Checked;
            }
            set
            {
                chkUseProxy.Checked = value;
            }
        }

        public string ProxyAddress
        {
            get
            {
                return txtProxtAddress.Text;
            }
            set
            {
                txtProxtAddress.Text = value;
            }
        }

        public int ProxyPort
        {
            get
            {
                return (int)numProxyPort.Value;
            }
            set
            {
                numProxyPort.Value = value;
            }
        }

        public bool ProxyByPassOnLocal
        {
            get
            {
                return chkBypass.Checked;
            }
            set
            {
                chkBypass.Checked = value;
            }
        }

        public string ProxyUserName
        {
            get
            {
                return txtUsername.Text;
            }
            set
            {
                txtUsername.Text = value;
            }
        }

        public string ProxyPassword
        {
            get
            {
                return txtPassword.Text;
            }
            set
            {
                txtPassword.Text = value;
            }
        }

        public string ProxyDomain
        {
            get
            {
                return txtDomain.Text;
            }
            set
            {
                txtDomain.Text = value;
            }
        }

        private void chkUseProxy_CheckedChanged(object sender, EventArgs e)
        {
            UpdateControls();
        }

        private void UpdateControls()
        {
            for (int i = 0; i < this.Controls.Count; i++)
            {
                if (this.Controls[i] != chkUseProxy)
                {
                    this.Controls[i].Enabled = chkUseProxy.Checked;
                }
            }
        }
    }
}
