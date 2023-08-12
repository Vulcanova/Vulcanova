using Android.Content;
using Vulcanova.Android;
using Vulcanova.Core.Layout;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

// TODO Xamarin.Forms.ExportRendererAttribute is not longer supported. For more details see https://github.com/dotnet/maui/wiki/Using-Custom-Renderers-in-.NET-MAUI
[assembly:ExportRenderer(typeof(SheetPage), typeof(SheetPageRenderer))]
namespace Vulcanova
{
    public class SheetPageRenderer : PageRenderer
    {
        public SheetPageRenderer(Context context) : base(context)
        {
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            Element.Layout(new Rect(Context.FromPixels(l), Context.FromPixels(t), Context.FromPixels(r), Context.FromPixels(b)));
            
            base.OnLayout(changed, l, t, r, b);
        }
    }
}