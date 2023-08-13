using CommunityToolkit.Maui;
using FFImageLoading.Maui;
using Microsoft.Maui.Controls.Compatibility.Hosting;
using Mopups.Hosting;
using Mopups.Services;
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
using Vulcanova.Features.Shared;
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
                        containerRegistry.RegisterInstance(MopupService.Instance);
                    })
                    .OnAppStart(
                        (Func<IContainerProvider, INavigationService, Task>)(async (container, navigationService) =>
                        {
                            var accRepo = container.Resolve<IAccountRepository>();

                            var activeAccount = accRepo.GetActiveAccountAsync().Result;

                            if (activeAccount != null)
                            {
                                var ctx = container.Resolve<AccountContext>();
                                ctx.Account = activeAccount;

                                await navigationService.NavigateAsync(
                                    "MainNavigationPage/HomeTabbedPage");
                            }
                            else
                            {
                                await navigationService.NavigateAsync("MainNavigationPage/IntroView");
                            }
                        }));
            })
            .UseMauiCompatibility()
            .UseFFImageLoading()
            .ConfigureMopups();

        return builder.Build();
    }
}
