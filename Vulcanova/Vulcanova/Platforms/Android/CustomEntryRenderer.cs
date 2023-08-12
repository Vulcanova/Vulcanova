using Android.Content;
using Android.Content.Res;
using Vulcanova.Android;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

// TODO Xamarin.Forms.ExportRendererAttribute is not longer supported. For more details see https://github.com/dotnet/maui/wiki/Using-Custom-Renderers-in-.NET-MAUI
[assembly: ExportRenderer(typeof(Entry), typeof(CustomEntryRenderer))]
namespace Vulcanova 
{
    public class CustomEntryRenderer : EntryRenderer
    {
        public CustomEntryRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> element)
        {
            base.OnElementChanged(element);

            if (Control == null || element.NewElement == null) return;

            Control.BackgroundTintList = ColorStateList.ValueOf(Element.TextColor.ToAndroid());
        }

        protected override void OnElementPropertyChanged(object sender,
            System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName != VisualElement.IsFocusedProperty.PropertyName) return;

            Control.BackgroundTintList = ColorStateList.ValueOf(Element.IsFocused
                ? Resources!.GetColor(Resource.Color.colorAccent, Context!.Theme)
                : Element.TextColor.ToAndroid());
        }
    }
}