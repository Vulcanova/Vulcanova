using System.Reactive.Disposables;
using ReactiveUI;

namespace Vulcanova.Features.LuckyNumber;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class LuckyNumberView
{
    public LuckyNumberView()
    {
        InitializeComponent();

        this.WhenActivated(disposable =>
        {
            this.OneWayBind(ViewModel, vm => vm.AccountViewModel, v => v.TitleView.ViewModel)
                .DisposeWith(disposable);

            this.OneWayBind(ViewModel, 
                    vm => vm.LuckyNumber, 
                    v => v.LuckyNumberLabel.Text,
                    l => l?.Number.ToString())
                .DisposeWith(disposable);
                
            this.OneWayBind(ViewModel, 
                    vm => vm.LuckyNumber, 
                    v => v.LuckyNumberLabel.IsVisible,
                    l => l?.Number != 0)
                .DisposeWith(disposable);
                
            this.OneWayBind(ViewModel, 
                    vm => vm.LuckyNumber, 
                    v => v.NoLuckyNumberLabel.IsVisible,
                    l => l?.Number == 0)
                .DisposeWith(disposable);
        });
    }
}