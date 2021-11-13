﻿using GoogleVisionBarCodeScanner;
using Prism.Ioc;
using Vulcanova.Core.Data;
using Vulcanova.Core.Layout;
using Vulcanova.Core.Mapping;
using Vulcanova.Core.Uonet;
using Vulcanova.Features.Auth;
using Vulcanova.Features.Grades;
using Vulcanova.Features.LuckyNumber;
using Vulcanova.Features.Shared;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace Vulcanova
{
    public partial class App
    {
        public App()
        {
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            Methods.SetSupportBarcodeFormat(BarcodeFormats.QRCode);

            var accRepo = Container.Resolve<IAccountRepository>();
            var activeAccount = accRepo.GetActiveAccount();

            if (activeAccount != null)
            {
                var ctx = Container.Resolve<AccountContext>();
                ctx.AccountId = activeAccount.Id;

                await NavigationService.NavigateAsync("NavigationPage/HomeTabbedPage?selectedTab=GradesSummaryView");
            }
            else
            {
                await NavigationService.NavigateAsync("NavigationPage/IntroView");
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

            containerRegistry.RegisterUonet();
        }
    }
}