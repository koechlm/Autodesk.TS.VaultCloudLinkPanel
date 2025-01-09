using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;
using CefSharp.WinForms;
using CefSharp;
using CefSharp.Handler;
using CefSharp.Enums;


namespace HelloWorld
{
    public partial class BrowserControl : UserControl
    {
        ChromiumWebBrowser chromiumWebBrowser1;

        public BrowserControl()
        {
            try
            {
                InitializeComponent();
                MessageBox.Show("Initialized BrowserControl");

            }
            catch (Exception)
            {
                MessageBox.Show("Failed to initialize the BrowserControl");
                return;
            }

            try
            {
                bool initialized = Cef.Initialize(new CefSettings());
                if (!initialized)
                {
                    MessageBox.Show("Failed to initialize Cef");
                    return;
                }
                chromiumWebBrowser1 = new ChromiumWebBrowser("www.autodesk.com");
                this.Controls.Add(chromiumWebBrowser1);
                chromiumWebBrowser1.Dock = DockStyle.Fill;
                chromiumWebBrowser1.Visible = true;
                MessageBox.Show("added BrowserCOntrol");
            }
            catch (Exception)
            {
                MessageBox.Show("Unhandled exception adding chromium Cef");
                return;
            }

        }

        public void mNavigate(string url)
        {
            chromiumWebBrowser1.Load(url);
        }

    }
}
