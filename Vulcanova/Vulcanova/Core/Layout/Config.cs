namespace Vulcanova.Core.Layout;

public static class Config
{
    public static void RegisterLayout(this IContainerRegistry container)
    {
        container.RegisterScoped<INavigationService, SheetsNavigationService>();

        container.RegisterForNavigation<MainNavigationPage>();
        container.RegisterForNavigation<HomeTabbedPage>();
    }
}