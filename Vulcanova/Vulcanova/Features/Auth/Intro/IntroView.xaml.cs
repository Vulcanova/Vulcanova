using System.Reactive.Disposables;
using ReactiveUI;
using Xamarin.Forms.Xaml;

namespace Vulcanova.Features.Auth.Intro;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class IntroView
{
    public IntroView()
    {
        InitializeComponent();

        this.WhenActivated(disposable =>
        {
            this.BindCommand(ViewModel, x => x.AddAccount, x => x.AddAccountButton)
                .DisposeWith(disposable);

            this.OneWayBind(ViewModel, x => x.OpenGitHubLink, x => x.GithubLinkTapRecognizer.Command)
                .DisposeWith(disposable);
        });
    }
}