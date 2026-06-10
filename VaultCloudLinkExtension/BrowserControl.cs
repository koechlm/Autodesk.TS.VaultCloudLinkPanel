using System;
using System.Windows.Threading;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;

namespace VaultCloudLinkExtension
{
    public partial class BrowserControl : UserControl
    {
        public BrowserControl()
        {
            InitializeComponent();
            Initialize();
        }

        void Initialize()
        {
            var frame = new DispatcherFrame();
            var env = CoreWebView2Environment.CreateAsync(null, Environment.GetEnvironmentVariable("TEMP"), null);

            using (var task = webView.EnsureCoreWebView2Async(env.Result))
            {
                task.ContinueWith((dummy) => frame.Continue = false);
                frame.Continue = true;
                Dispatcher.PushFrame(frame);
            }
        }

        public void Navigate(string mUrl)
        {
            Uri uri = new Uri(mUrl, System.UriKind.Absolute);
            webView.Source = uri;
        }

    }
}
