using System.Reactive.Linq;
using System.Reactive.Disposables;
using ReactiveUI;
using Vulcanova.Core.Rx;

namespace Vulcanova.Features.Attendance;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class AttendanceDetailedView
{
    public AttendanceDetailedView()
    {
        InitializeComponent();

        this.WhenActivated(disposable =>
        {
            this.Bind(ViewModel, vm => vm.SelectedDay, v => v.Calendar.SelectedDate)
                .DisposeWith(disposable);

            // TODO Xamarin.Forms.Device.RuntimePlatform is no longer supported. Use Microsoft.Maui.Devices.DeviceInfo.Platform instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
            if (Device.RuntimePlatform == Device.iOS)
            {
                this.OneWayBind(ViewModel, vm => vm.CurrentDayEntries, v => v.EntriesList.ItemsSource,
                        TimeSpan.FromMilliseconds(50))
                    .DisposeWith(disposable);
            }
            else
            {
                this.OneWayBind(ViewModel, vm => vm.CurrentDayEntries, v => v.EntriesList.ItemsSource)
                    .DisposeWith(disposable);
            }

            this.WhenAnyObservable(v => v.ViewModel.GetAttendanceEntries.IsExecuting)
                .Select(v => !v)
                .BindTo(NoElementsView, x => x.IsVisible)
                .DisposeWith(disposable);

            this.BindForceRefresh(RefreshView, v => v.ViewModel.GetAttendanceEntries)
                .DisposeWith(disposable);

            this.OneWayBind(ViewModel, vm => vm.EnableJustificationMode, v => v.EnableJustificationButton.Command)
                .DisposeWith(disposable);

            this.WhenAnyObservable(v => v.ViewModel.EnableJustificationMode.CanExecute)
                .BindTo(this, v => v.EnableJustificationButton.IsVisible)
                .DisposeWith(disposable);

            this.OneWayBind(ViewModel, vm => vm.JustifyLessonsAttendance, v => v.JustifyButton.Command)
                .DisposeWith(disposable);

            this.WhenAnyObservable(v => v.ViewModel.JustifyLessonsAttendance.CanExecute)
                .BindTo(this, v => v.JustifyButton.IsVisible)
                .DisposeWith(disposable);
        });
    }
}