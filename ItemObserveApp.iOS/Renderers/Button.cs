using System;
using ItemObservApp.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Button), typeof(iOSButtonRenderer))]
namespace ItemObservApp.iOS.Renderers
{
    /// <summary>
    /// A custom renderer that adds a bit of padding to either side of a button
    /// </summary>
    public class iOSButtonRenderer : ButtonRenderer
    {
        public iOSButtonRenderer() { }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);
            Control.ContentEdgeInsets = new UIEdgeInsets(Control.ContentEdgeInsets.Top, Control.ContentEdgeInsets.Left + 10, Control.ContentEdgeInsets.Bottom, Control.ContentEdgeInsets.Right + 10);
        }
    }
}