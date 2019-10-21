using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Foundation;
using ItemObserveApp.Common;

[assembly: ExportRenderer(typeof(ExtendWebView), typeof(ItemObserveApp.iOS.Renderers.ExtendWebViewRenderer))]
namespace ItemObserveApp.iOS.Renderers
{
    public class ExtendWebViewRenderer : WebViewRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            var webView = e.NewElement as ExtendWebView;
            if (webView != null)
            {
                //NSUrlCache.SharedCache.RemoveAllCachedResponses();
                //NSUrlCache.SharedCache.MemoryCapacity = 0;
                //NSUrlCache.SharedCache.DiskCapacity = 0;

                webView.EvaluateJavascript = (js) =>
                {
                    return Task.FromResult(this.EvaluateJavascript(js));
                };
            }
        }
    }
}
