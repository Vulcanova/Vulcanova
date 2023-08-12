using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Vulcanova.Core.Mvvm;
using Vulcanova.Features.Auth;
using Unit = System.Reactive.Unit;

namespace Vulcanova.Core.Layout;

public class HomeTabbedPageViewModel : ViewModelBase
{
    public ReactiveCommand<string, string> UpdateTitle { get; }
    public ReactiveCommand<Unit, Unit> SyncAccounts { get; }

    [ObservableAsProperty]
    public string Title { get; }

    public HomeTabbedPageViewModel(INavigationService navigationService, AccountSyncService accountSyncService) : base(navigationService)
    {
        UpdateTitle = ReactiveCommand.Create<string, string>(t => t);

        UpdateTitle.ToPropertyEx(this, vm => vm.Title);

        SyncAccounts = ReactiveCommand.CreateFromTask<Unit, Unit>(async _ =>
        {
            await accountSyncService.SyncAccountsIfRequiredAsync();

            return default;
        });

        SyncAccounts.Execute().Subscribe();
    }
}