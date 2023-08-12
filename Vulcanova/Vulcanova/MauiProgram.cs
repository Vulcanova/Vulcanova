using CommunityToolkit.Maui;
using Mopups.Hosting;
using Vulcanova.Core.Data;
using Vulcanova.Core.Layout;
using Vulcanova.Core.Mapping;
using Vulcanova.Core.NativeWidgets;
using Vulcanova.Core.Uonet;
using Vulcanova.Features.Attendance;
using Vulcanova.Features.Auth;
using Vulcanova.Features.Exams;
using Vulcanova.Features.Grades;
using Vulcanova.Features.Homework;
using Vulcanova.Features.LuckyNumber;
using Vulcanova.Features.Messages;
using Vulcanova.Features.Notes;
using Vulcanova.Features.Settings;
using Vulcanova.Features.Timetable;

namespace Vulcanova;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiCommunityToolkit()
            .UseMauiApp<App>()
            .UsePrism(prism =>
            {
                prism.RegisterTypes(containerRegistry =>
                {
                    containerRegistry.RegisterLiteDb();

                    containerRegistry.RegisterAutoMapper();

                    containerRegistry.RegisterLayout();

                    containerRegistry.RegisterNativeWidgetsCommunication();

                    containerRegistry.RegisterAuth();
                    containerRegistry.RegisterLuckyNumber();
                    containerRegistry.RegisterGrades();
                    containerRegistry.RegisterTimetable();
                    containerRegistry.RegisterAttendance();
                    containerRegistry.RegisterExams();
                    containerRegistry.RegisterHomework();
                    containerRegistry.RegisterMessages();
                    containerRegistry.RegisterNotes();

                    containerRegistry.RegisterSettings();

                    containerRegistry.RegisterUonet();

                    containerRegistry.RegisterSingleton<ISheetPopper, SheetPopper>();
                    containerRegistry.RegisterSingleton<INativeWidgetProxy, NativeWidgetProxy>();
                });
            })
            .ConfigureMopups();

        return builder.Build();
    }
}
