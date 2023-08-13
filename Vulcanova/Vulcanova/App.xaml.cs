using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Vulcanova.Core.NativeWidgets;
using Vulcanova.Features.Attendance;
using Vulcanova.Features.Timetable;
using AppSettings = Vulcanova.Helpers.AppSettings;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace Vulcanova;

public partial class App
{
    public App() : base()
    {
        InitializeComponent();
    }

    protected override void OnStart()
    {
        if (AppSettings.EnableAnalytics)
        {
            AppCenter.Start($"ios={AppSettings.AppCenterSecret}", typeof(Analytics), typeof(Crashes));
        }

        base.OnStart();
    }

    public async void OnWidgetInteraction(INativeWidgetProxy.NativeWidget widget)
    {
        var tabName = widget switch
        {
            INativeWidgetProxy.NativeWidget.Timetable => nameof(TimetableView),
            INativeWidgetProxy.NativeWidget.AttendanceStats => nameof(AttendanceView),
            _ => null
        };

        if (tabName is null)
        {
            return;
        }
        
        var navigationService = Handler!.MauiContext!.Services.GetRequiredService<INavigationService>();

        // https://github.com/PrismLibrary/Prism/discussions/2841
        // await navigationService.SelectTabAsync(tabName);
    }
}