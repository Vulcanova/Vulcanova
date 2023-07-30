using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prism.Navigation;
using Vulcanova.Core.Layout;
using Vulcanova.Features.Auth.Accounts;
using Vulcanova.Features.Auth.Intro;
using Vulcanova.Features.Shared;

namespace Vulcanova.Features.Auth;

public sealed class AccountsManager
{
    private readonly IAccountRepository _accountRepository;
    private readonly AccountContext _accountContext;
    private readonly INavigationService _navigationService;

    public AccountsManager(IAccountRepository accountRepository, INavigationService navigationService, AccountContext accountContext)
    {
        _accountRepository = accountRepository;
        _navigationService = navigationService;
        _accountContext = accountContext;
    }

    public async Task OpenAccountAndMarkAsCurrentAsync(int accountId, bool navigateToHomePage = true)
    {
        var accounts = await _accountRepository.GetAccountsAsync();

        foreach (var account in accounts)
        {
            account.IsActive = account.Id == accountId;
        }

        await _accountRepository.UpdateAccountsAsync(accounts);

        _accountContext.Account = accounts.Single(acc => acc.IsActive);

        if (navigateToHomePage)
        {
            await _navigationService.NavigateAsync("/MainNavigationPage/HomeTabbedPage?selectedTab=GradesSummaryView");
        }
    }

    public async Task AddAccountsAsync(IEnumerable<Account> accounts)
    {
        await _accountRepository.AddAccountsAsync(accounts);
    }

    public async Task DeleteAccountAsync(int accountId)
    {
        await _accountRepository.DeleteByIdAsync(accountId);

        var accounts = await _accountRepository.GetAccountsAsync();
        var activeAccount = accounts.FirstOrDefault();

        if (activeAccount is null)
        {
            await _navigationService.NavigateAsync($"/{nameof(MainNavigationPage)}/{nameof(IntroView)}", useModalNavigation: false);
            _accountContext.Account = null;
            return;
        }

        activeAccount.IsActive = true;
        _accountContext.Account = activeAccount;
        await _accountRepository.UpdateAccountAsync(activeAccount);
    }
}