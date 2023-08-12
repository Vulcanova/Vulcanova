using System.Reactive.Disposables;
using ReactiveUI;
using Vulcanova.Core.Rx;

namespace Vulcanova.Features.Grades.Summary;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class GradesSummaryView
{
    public static readonly BindableProperty PeriodIdProperty = BindableProperty.Create(
        nameof(PeriodId), typeof(int?), typeof(GradesSummaryView));

    public int? PeriodId
    {
        get => (int?) GetValue(PeriodIdProperty);
        set => SetValue(PeriodIdProperty, value);
    }

    public GradesSummaryView()
    {
        InitializeComponent();

        this.WhenActivated(disposable =>
        {
            this.BindForceRefresh(RefreshView, v => v.ViewModel.GetGrades, true)
                .DisposeWith(disposable);

            this.WhenAnyValue(v => v.PeriodId)
                .WhereNotNull()
                .Subscribe((val) => ViewModel!.PeriodId = val!.Value)
                .DisposeWith(disposable);
            
            this.OneWayBind(ViewModel, vm => vm.PartialGradesAverage, v => v.PartialAverage.Text,
                    value => value?.ToString("F2"))
                .DisposeWith(disposable);

            this.OneWayBind(ViewModel, vm => vm.PartialGradesAverage, v => v.PartialAverageContainer.IsVisible,
                value => value != null);
        });
    }
}