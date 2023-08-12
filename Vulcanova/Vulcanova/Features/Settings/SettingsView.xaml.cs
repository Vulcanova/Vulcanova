using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Disposables;
using ReactiveUI;

namespace Vulcanova.Features.Settings;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class SettingsView
{
    public SettingsView()
    {
        InitializeComponent();

        this.WhenActivated(disposable =>
        {
            Observable.FromEventPattern(
                    handler => ValueOfPlusCell.Tapped += handler,
                    handler => ValueOfPlusCell.Tapped -= handler)
                .Select(_ => Unit.Default)
                .InvokeCommand(ViewModel, vm => vm.OpenValueOfPlusPicker)
                .DisposeWith(disposable);
                
            Observable.FromEventPattern(
                    handler => ValueOfMinusCell.Tapped += handler,
                    handler => ValueOfMinusCell.Tapped -= handler)
                .Select(_ => Unit.Default)
                .InvokeCommand(ViewModel, vm => vm.OpenValueOfMinusPicker)
                .DisposeWith(disposable);

            this.OneWayBind(ViewModel, vm => vm.ValueOfPlus, v => v.ValueOfPlusLabel.Text)
                .DisposeWith(disposable);
                
            this.OneWayBind(ViewModel, vm => vm.ValueOfMinus, v => v.ValueOfMinusLabel.Text)
                .DisposeWith(disposable);

            this.BindCommand(ViewModel, vm => vm.OpenHttpTrafficLogger, v => v.NetworkDebuggingCell)
                .DisposeWith(disposable);

            this.BindCommand(ViewModel, vm => vm.OpenAboutPage, v => v.AboutCell)
                .DisposeWith(disposable);
        });
    }
}