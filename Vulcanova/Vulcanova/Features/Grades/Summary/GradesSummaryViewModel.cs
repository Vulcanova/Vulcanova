using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using Prism.Navigation;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Vulcanova.Core.Mvvm;
using Vulcanova.Features.Shared;
using Vulcanova.Core.Rx;
using Vulcanova.Features.Grades.SubjectDetails;
using Vulcanova.Features.Settings;
using Vulcanova.Uonet.Api.Auth;
using Unit = System.Reactive.Unit;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Vulcanova.Features.Grades.Summary;

public class GradesSummaryViewModel : ViewModelBase
{
    public ReactiveCommand<bool, GradesResult> GetGrades { get; }

    public ReactiveCommand<int, Unit> ShowSubjectGradesDetails { get; }

    [Reactive] public IEnumerable<SubjectGrades> Grades { get; private set; }

    [ObservableAsProperty] public bool IsSyncing { get; }
        
    [Reactive] public int? PeriodId { get; set; }

    [Reactive] public SubjectGrades CurrentSubject { get; private set; }

    [ObservableAsProperty] private GradesResult RawGrades { get; }
    [Reactive] public decimal? PartialGradesAverage { get; private set; }

    private IObservable<ModifiersSettings> ModifiersSettings { get; }

    public GradesSummaryViewModel(
        INavigationService navigationService,
        AccountContext accountContext,
        IGradesService gradesService,
        IAverageGradesService averageGradesService,
        AppSettings settings) : base(navigationService)
    {
        GetGrades = ReactiveCommand.CreateFromObservable((bool forceSync) =>
        {
            var grades = gradesService
                .GetPeriodGrades(accountContext.Account, PeriodId!.Value, forceSync);

            var averageGrades =
                accountContext.Account.Capabilities.Contains(AccountCapabilities.GradesAverageEnabled)
                    ? averageGradesService
                        .GetAverageGrades(accountContext.Account.Id, PeriodId!.Value, forceSync)
                    : Observable.Return(Array.Empty<AverageGrade>());

            return grades.CombineLatest(averageGrades).Select(x => new GradesResult(x.First, x.Second));
        });

        GetGrades.ToPropertyEx(this, vm => vm.RawGrades);

        GetGrades.IsExecuting.ToPropertyEx(this, vm => vm.IsSyncing);

        ShowSubjectGradesDetails = ReactiveCommand.Create((int subjectId) =>
        {
            CurrentSubject = Grades?.First(g => g.SubjectId == subjectId);

            // TODO Xamarin.Forms.Device.RuntimePlatform is no longer supported. Use Microsoft.Maui.Devices.DeviceInfo.Platform instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
            if (Device.RuntimePlatform == Device.iOS)
            {
                navigationService.NavigateAsync(nameof(GradesSubjectDetailsView), 
                    ("Subject", CurrentSubject));
            }

            return Unit.Default;
        });

        this.WhenAnyValue(vm => vm.PeriodId)
            .WhereNotNull()
            .Subscribe(v =>
            {
                GetGrades.Execute(false).SubscribeAndIgnoreErrors();
            });

        var modifiersObservable = settings
            .WhenAnyValue(s => s.Modifiers.PlusSettings.SelectedValue, s => s.Modifiers.MinusSettings.SelectedValue,
                s => s.ForceAverageCalculationByApp)
            .WhereNotNull();

        this.WhenAnyValue(vm => vm.RawGrades)
            .WhereNotNull()
            .CombineLatest(modifiersObservable)
            .Subscribe(values =>
            {
                var (grades, _) = values;
                Grades = ToSubjectGrades(grades.Grades, grades.AverageGrades, settings);
            });
        
        this.WhenAnyValue(vm => vm.RawGrades)
            .WhereNotNull()
            .CombineLatest(modifiersObservable)
            .Subscribe(values =>
            {
                PartialGradesAverage = values.First.Grades.Average(settings.Modifiers);
            });
    }

    private static IEnumerable<SubjectGrades> ToSubjectGrades(IEnumerable<Grade> grades, IEnumerable<AverageGrade> averageGrades, AppSettings settings)
        => grades.GroupBy(g => new
            {
                g.Column.Subject.Id,
                g.Column.Subject.Name
            })
            .Select(g => new SubjectGrades
            {
                SubjectId = g.Key.Id,
                SubjectName = g.Key.Name,
                Average = CalculateAverage(g.Key.Id, g, averageGrades, settings),
                Grades = new ObservableCollection<Grade>(g.ToArray())
            });

    private static decimal? CalculateAverage(int subjectId, IEnumerable<Grade> grades,
        IEnumerable<AverageGrade> averageGrades, AppSettings settings)
    {
        if (settings.ForceAverageCalculationByApp)
        {
            return grades.Average(settings.Modifiers);
        }

        return averageGrades
            .SingleOrDefault(x => x.SubjectId == subjectId)?.Average ?? grades.Average(settings.Modifiers);
    }

    public sealed record GradesResult(IEnumerable<Grade> Grades, IEnumerable<AverageGrade> AverageGrades);
}
