using System.Collections.Generic;
using System.Reactive;
using Prism.Navigation;
using ReactiveUI;
using Vulcanova.Core.Mvvm;
using Vulcanova.Features.Settings.About;
using Vulcanova.Features.Settings.Grades.iOS;
using Vulcanova.Features.Settings.HttpTrafficLogger;
using Xamarin.Essentials;

namespace Vulcanova.Features.Settings;

public class SettingsViewModel : ViewModelBase
{
    public decimal ValueOfPlus => _appSettings.Modifiers.PlusSettings.SelectedValue;
    public decimal ValueOfMinus => _appSettings.Modifiers.MinusSettings.SelectedValue;

    private readonly AppSettings _appSettings;

    public ReactiveCommand<Unit, INavigationResult> OpenValueOfPlusPicker { get; }
    public ReactiveCommand<Unit, INavigationResult> OpenValueOfMinusPicker { get; }
    public ReactiveCommand<Unit, INavigationResult> OpenHttpTrafficLogger { get; }
    public ReactiveCommand<Unit, INavigationResult> OpenAboutPage { get; }
    public ReactiveCommand<Unit, Unit> StartComposingContactMail { get; }

    public bool DisableReadReceipts
    {
        get => _appSettings.DisableReadReceipts;
        set => _appSettings.DisableReadReceipts = value;
    }
    
    public bool ForceAverageCalculationByApp
    {
        get => _appSettings.ForceAverageCalculationByApp;
        set => _appSettings.ForceAverageCalculationByApp = value;
    }

    public bool LongPressToShareGrade
    {
        get => _appSettings.LongPressToShareGrade;
        set => _appSettings.LongPressToShareGrade = value;
    }

    public SettingsViewModel(INavigationService navigationService, AppSettings appSettings) : base(navigationService)
    {
        _appSettings = appSettings;

        OpenValueOfPlusPicker =
            ReactiveCommand.CreateFromTask(async () => await NavigationService.NavigateAsync(nameof(ValueOfPlusPickeriOS)));
            
        OpenValueOfMinusPicker =
            ReactiveCommand.CreateFromTask(async () => await NavigationService.NavigateAsync(nameof(ValueOfMinusPickeriOS)));

        OpenHttpTrafficLogger =
            ReactiveCommand.CreateFromTask(async () =>
                await NavigationService.NavigateAsync(nameof(HttpTrafficLoggerView)));

        OpenAboutPage =
            ReactiveCommand.CreateFromTask(async () => await NavigationService.NavigateAsync(nameof(AboutPage)));

        StartComposingContactMail = ReactiveCommand.CreateFromTask(async (Unit _) =>
        {
            try
            {
                var message = new EmailMessage
                {
                    Subject = "Vulcanova - Kontakt",
                    Body = "Wersja aplikacji: " + AppInfo.VersionString,
                    To = new List<string> { "vulcanova@romanowski.me" }
                };
                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException)
            {
                // Email is not supported on this device
            }
        });
    }
}