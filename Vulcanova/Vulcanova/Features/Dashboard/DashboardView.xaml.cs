using System.Reactive.Disposables;
using ReactiveUI;
using Vulcanova.Core.Rx;
using Vulcanova.Resources;
using Xamarin.Forms.Xaml;

namespace Vulcanova.Features.Dashboard;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class DashboardView
{
    public DashboardView()
    {
        InitializeComponent();

        this.WhenActivated(disposable =>
        {
            this.OneWayBind(ViewModel, 
                    vm => vm.LuckyNumber, 
                    v => v.LuckyNumberLabel.Text,
                    l => l?.Number != 0 ? l?.Number.ToString() : Strings.NoLuckyNumberShort)
                .DisposeWith(disposable);
            
            this.OneWayBind(ViewModel, vm => vm.SelectedDayLabel, v =>  v.DateAndTime.Text)
                .DisposeWith(disposable);
            this.OneWayBind(ViewModel, vm => vm.CurrentDayTimetable, v => v.TimetableCollection.BindingContext)
                .DisposeWith(disposable);
            this.OneWayBind(ViewModel, vm => vm.CurrentWeekExams, v => v.ExamsCollection.BindingContext)
                .DisposeWith(disposable);
            this.OneWayBind(ViewModel, vm => vm.CurrentWeekHomework, v => v.HomeworkCollection.BindingContext)
                .DisposeWith(disposable);
            this.OneWayBind(ViewModel, vm => vm.CurrentWeekGrades, v => v.GradesCollection.BindingContext)
                .DisposeWith(disposable);
            this.OneWayBind(ViewModel, vm => vm.AccountViewModel, v => v.TitleView.ViewModel)
                .DisposeWith(disposable);
            
            this.BindForceRefresh(RefreshView, v => v.ViewModel.RefreshData)
                .DisposeWith(disposable);
        });
    }
}