using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;

namespace VaultCloudLinkExtension
{
    public partial class CloudViewControl : UserControl
    {
        CefSharp.WinForms.ChromiumWebBrowser? mBrowser;

        public CloudViewControl()
        {
            InitializeComponent();

            InitializeBrowser();
        }

        private void InitializeBrowser()
        {
            // Create a new instance of the CefSharp mBrowser
            mBrowser = new CefSharp.WinForms.ChromiumWebBrowser("");
            _ = mBrowser.WaitForInitialLoadAsync();
            _ = mBrowser.WaitForNavigationAsync();

            // Set the custom context menu handler
            mBrowser.MenuHandler = new CustomContextMenuHandler();

            // handle messages from the JavascriptMessageReceived event
            mBrowser.JavascriptMessageReceived += CloudViewControl_JavascriptMessageReceived;

            // Make the mBrowser fill the form
            mBrowser.Dock = DockStyle.Fill;
            mBrowser.Show();

            // Add the mBrowser to the form
            this.Controls.Add(mBrowser);
        }

        private void CloudViewControl_JavascriptMessageReceived(object? sender, JavascriptMessageReceivedEventArgs e)
        {
            //MessageBox.Show(e.Message?.ToString());

            var message = e.Message?.ToString();
            if (!String.IsNullOrEmpty(message))
            {
                // parse the message
            }
        }

        
        //navigate asynch to the specified URL
        public async Task NavigateToUrlAsync(string url)
        {
            if (mBrowser != null)
            {
                await mBrowser.LoadUrlAsync(url);
            }
        }
    }
}
