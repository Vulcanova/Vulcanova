namespace Vulcanova.Features.Auth;

public interface IAccountSyncService
{
    Task SyncAccountsIfRequiredAsync();
}