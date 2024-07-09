using Prism.Ioc;

namespace Vulcanova.Features.GoodBye;

public static class Config
{
    public static void RegisterGoodBye(this IContainerRegistry container)
    {
        container.RegisterForNavigation<GoodByeView>();
    }
}