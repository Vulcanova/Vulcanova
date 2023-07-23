using System.Linq;
using System.Reflection;
using Prism.Ioc;
using Vulcanova.Core.Uonet;

namespace Vulcanova.Core.NativeWidgets;

public static class Config
{
    public static void RegisterNativeWidgetsCommunication(this IContainerRegistry container)
    {
        foreach (var type in Assembly.GetExecutingAssembly().GetTypes()
                     .Where(t => !t.IsAbstract))
        {
            var widgetUpdaterInterface = type.GetInterfaces().SingleOrDefault(x =>
                x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IWidgetUpdater<>));

            if (widgetUpdaterInterface == null)
            {
                continue;
            }

            var eventType = widgetUpdaterInterface.GetGenericArguments()[0];
            var registerAs = typeof(IWidgetUpdater<>).MakeGenericType(eventType);
            
            container.RegisterScoped(registerAs, type);
        }

        container.RegisterSingleton<NativeWidgetUpdateDispatcher>();
    }
}