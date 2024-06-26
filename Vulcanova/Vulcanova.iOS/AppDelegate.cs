﻿using FFImageLoading.Forms.Platform;
using Foundation;
using LabelHtml.Forms.Plugin.iOS;
using Prism;
using Prism.Ioc;
using UIKit;
using Vulcanova.Core.Layout;
using Vulcanova.Core.NativeWidgets;

namespace Vulcanova.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate,
        IPlatformInitializer
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            HtmlLabelRenderer.Initialize();

            global::Xamarin.Forms.Forms.Init();
            
            Xamarin.Essentials.Platform.Init(UiHelper.GetTopViewController);

            Rg.Plugins.Popup.Popup.Init();

            CachedImageRenderer.Init();
            
            Sharpnado.Tabs.iOS.Preserver.Preserve();

            XamEffects.iOS.Effects.Init();

            GoogleVisionBarCodeScanner.iOS.Initializer.Init();
            Firebase.Core.App.Configure();

            LoadApplication(new App(this));

            return base.FinishedLaunching(app, options);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ISheetPopper, SheetPopper>();
            containerRegistry.RegisterSingleton<INativeWidgetProxy, NativeWidgetProxy>();
        }

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