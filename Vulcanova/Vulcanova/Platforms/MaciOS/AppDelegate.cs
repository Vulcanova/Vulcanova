using Foundation;
using UIKit;
using Vulcanova.Core.Layout;
using Vulcanova.Core.NativeWidgets;

namespace Vulcanova.Platforms.MaciOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : Microsoft.Maui.MauiUIApplicationDelegate
    {
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            if (url.Scheme != "widget-deeplink") return false;

            INativeWidgetProxy.NativeWidget? widget = url.Host switch
            {
                "timetable" => INativeWidgetProxy.NativeWidget.Timetable,
                "attendance" => INativeWidgetProxy.NativeWidget.AttendanceStats,
                _ => null
            };

            if (widget is null)
            {
                return false;
            }
                
            ((App) App.Current).OnWidgetInteraction(widget.Value);

            return true;
        }
    }
}