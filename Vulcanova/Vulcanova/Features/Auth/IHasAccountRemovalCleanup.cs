namespace Vulcanova.Features.Auth;

public interface IHasAccountRemovalCleanup
{
    Task DoPostRemovalCleanUpAsync(int accountId);
}