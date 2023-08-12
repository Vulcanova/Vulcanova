using Prism;
using Prism.AppModel;
using Prism.Ioc;
using Vulcanova.Features.Settings.About;
using Vulcanova.Features.Settings.Grades;
using Vulcanova.Features.Settings.Grades.Android;
using Vulcanova.Features.Settings.Grades.iOS;
using Vulcanova.Features.Settings.HttpTrafficLogger;

namespace Vulcanova.Features.Settings;

public static class Config
{
    public static void RegisterSettings(this IContainerRegistry container)
    {
        container.RegisterSingleton<AppSettings>();

#if IOS
        container.RegisterForNavigation<ValueOfPlusPickeriOS>();
        container.RegisterForNavigation<ValueOfMinusPickeriOS>();
#endif

#if ANDROID
        container.RegisterForNavigationOnPlatform<ValueOfMinusPickeriOS, ValueOfMinusPickerViewModel>(
            new Platform<ValueOfMinusPickeriOS>(RuntimePlatform.iOS),
            new Platform<ValueOfMinusPickerAndroid>(RuntimePlatform.Android)
        );
#endif

        container.RegisterHttpTrafficLogger();

        container.RegisterForNavigation<AboutPage>();
    }
}