using System.Reactive;
using System.Threading.Tasks;
using Prism.Navigation;
using ReactiveUI;
using Vulcanova.Core.Mvvm;
using Vulcanova.Features.GoodBye;

namespace Vulcanova.Features.Auth.ScanningQrCode;

public class QrScannerViewModel : ViewModelBase
{
    public ReactiveCommand<string, Unit> ProcessQrCodeCommand { get; }

    public QrScannerViewModel(
        INavigationService navigationService) : base(navigationService)
    {
        ProcessQrCodeCommand = ReactiveCommand.CreateFromTask<string, Unit>(ProcessQrCode);
    }

    private async Task<Unit> ProcessQrCode(string code)
    {
        var (apiAddress, token) = AuthQrCode.FromQrString(code);
        await NavigationService.NavigateAsync($"/{nameof(GoodByeView)}");

        // var instanceUrl = _instanceUrlProvider.ExtractInstanceUrlFromRequestUrl(apiAddress);
        //
        // var navigationParams = new NavigationParameters
        // {
        //     { "instanceUrl", instanceUrl },
        //     { "token", token }
        // };
        //
        // await NavigationService.NavigateAsync(nameof(EnterPinCodeView), navigationParams);

        return Unit.Default;
    }
}