using ReactiveUI;

namespace Vulcanova.Features.Auth.ScanningQrCode;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class QrScannerView
{
    public QrScannerView()
    {
        InitializeComponent();

        this.WhenActivated(disposable =>
        {
            // this.CameraView.Events().OnDetected
            //     .Select(args => args.BarcodeResults.First().RawValue)
            //     .InvokeCommand(ViewModel, vm => vm.ProcessQrCodeCommand)
            //     .DisposeWith(disposable);
        });
    }
}