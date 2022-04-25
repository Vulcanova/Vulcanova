﻿using GoogleVisionBarCodeScanner;
using Prism.Ioc;
using ReactiveUI;
using Sharpnado.Tabs;
using Vulcanova.Core.Data;
using Vulcanova.Core.Layout;
using Vulcanova.Core.Mapping;
using Vulcanova.Core.Rx;
using Vulcanova.Core.Uonet;
using Vulcanova.Features.Attendance;
using Vulcanova.Features.Auth;
using Vulcanova.Features.Exams;
using Vulcanova.Features.Grades;
using Vulcanova.Features.Homeworks;
using Vulcanova.Features.LuckyNumber;
using Vulcanova.Features.Settings;
using Vulcanova.Features.Shared;
using Vulcanova.Features.Timetable;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace Vulcanova
{
    public partial class App
    {
        protected override async void OnInitialized()
        {
            InitializeComponent();

            RxApp.DefaultExceptionHandler = new ReactiveExceptionHandler();

            Initializer.Initialize(false, false);
            Sharpnado.Shades.Initializer.Initialize(false);

            Methods.SetSupportBarcodeFormat(BarcodeFormats.QRCode);

            var accRepo = Container.Resolve<IAccountRepository>();

            // breaks app startup when executed asynchronously
            var activeAccount = accRepo.GetActiveAccountAsync().Result;

            if (activeAccount != null)
            {
                var ctx = Container.Resolve<AccountContext>();
                ctx.AccountId = activeAccount.Id;

                await NavigationService.NavigateAsync(
                    "MainNavigationPage/HomeTabbedPage?selectedTab=GradesSummaryView");
            }
            else
            {
                await NavigationService.NavigateAsync("MainNavigationPage/IntroView");
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterLiteDb();

            containerRegistry.RegisterAutoMapper();

            containerRegistry.RegisterLayout();

            containerRegistry.RegisterAuth();
            containerRegistry.RegisterLuckyNumber();
            containerRegistry.RegisterGrades();
            containerRegistry.RegisterTimetable();
            containerRegistry.RegisterAttendance();
            containerRegistry.RegisterExams();
            containerRegistry.RegisterHomeworks();

            containerRegistry.RegisterSettings();

            containerRegistry.RegisterUonet();
        }
    }
}