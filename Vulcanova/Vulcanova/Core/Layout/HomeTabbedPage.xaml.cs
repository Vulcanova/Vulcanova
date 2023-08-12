using Vulcanova.Features.Exams;
using Vulcanova.Features.Homework;
using Vulcanova.Features.LuckyNumber;
using Vulcanova.Features.Messages;
using Vulcanova.Features.Notes;
using Vulcanova.Features.Settings;

namespace Vulcanova.Core.Layout;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class HomeTabbedPage
{
    public HomeTabbedPage()
    {
        InitializeComponent();

        // These pages are expected to be placed in "More" tab.
        // On iOS this breaks Prism's NavigationService if a page is wrapped in a NavigationPage
        Page[] pages = {new ExamsView(), new HomeworkView(), new MessagesView(), new LuckyNumberView(), new NotesView(), new SettingsView()};

        foreach (var page in pages)
        {
            var toBeAdded = page;
                
            // TODO Xamarin.Forms.Device.RuntimePlatform is no longer supported. Use Microsoft.Maui.Devices.DeviceInfo.Platform instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
                            if (Device.RuntimePlatform != Device.iOS)
            {
                var navigationPage = new NavigationPage(toBeAdded)
                {
                    Title = toBeAdded.Title,
                    IconImageSource = toBeAdded.IconImageSource
                };

                toBeAdded = navigationPage;
            }

            Children.Add(toBeAdded);
        }
    }
}