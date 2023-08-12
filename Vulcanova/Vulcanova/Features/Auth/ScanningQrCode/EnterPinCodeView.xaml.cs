using System.Reactive.Disposables;
using ReactiveUI;
using Microsoft.Maui.Controls.Xaml;

namespace Vulcanova.Features.Auth.ScanningQrCode;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class EnterPinCodeView
{
    public EnterPinCodeView()
    {
        InitializeComponent();

        this.WhenActivated(disposable =>
        {
            this.Bind(ViewModel, vm => vm.Pin, v => v.Pin.Text)
                .DisposeWith(disposable);

            this.BindCommand(ViewModel, vm => vm.RegisterDevice, v => v.AddAccountButton)
                .DisposeWith(disposable);

            this.WhenAnyObservable(v => v.ViewModel.RegisterDevice.IsExecuting)
                .BindTo(LoadingIndicator, x => x.IsRunning)
                .DisposeWith(disposable);
        });
    }
}