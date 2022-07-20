using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Prism.Navigation;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Vulcanova.Core.Layout;
using Vulcanova.Core.Mvvm;
using Vulcanova.Core.Rx;
using Vulcanova.Extensions;
using Vulcanova.Features.Exams.ExamDetails;
using Vulcanova.Features.Shared;
using Xamarin.Forms;

namespace Vulcanova.Features.Exams;

public class ExamsViewModel : ViewModelBase
{
    public ReactiveCommand<bool, ImmutableArray<Exam>> GetExams { get; }
    public ReactiveCommand<int, Unit> ShowExamDetails { get; }

    [ObservableAsProperty] public ImmutableArray<Exam> Entries { get; }

    [Reactive] public IReadOnlyCollection<Exam> CurrentWeekEntries { get; private set; }
    [Reactive] public DateTime SelectedDay { get; set; } = DateTime.Today;
    [Reactive] public Exam SelectedExam { get; set; }

    private readonly IExamsService _examsService;

    public ExamsViewModel(
        IExamsService examsService,
        AccountContext accountContext,
        INavigationService navigationService,
        ISheetPopper popper = null) : base(navigationService)
    {
        _examsService = examsService;

        GetExams = ReactiveCommand.CreateFromObservable((bool forceSync) =>
            GetEntries(accountContext.AccountId, SelectedDay, forceSync));

        GetExams.ToPropertyEx(this, vm => vm.Entries);
            
        ShowExamDetails = ReactiveCommand.Create((int lessonId) =>
        {
            SelectedExam = CurrentWeekEntries?.First(g => g.Id == lessonId);

            if (Device.RuntimePlatform == Device.iOS)
            {
                var view = new ExamDetailsView
                {
                    Exam = SelectedExam
                };

                popper!.PopSheet(view);
            }

            return Unit.Default;
        });

        var lastDate = SelectedDay;

        this.WhenAnyValue(vm => vm.SelectedDay)
            .Subscribe((d) =>
            {
                if (Entries == null || lastDate.Month != d.Month)
                {
                    GetExams.Execute(false).SubscribeAndIgnoreErrors();
                }

                lastDate = d;
            });

        this.WhenAnyValue(vm => vm.Entries)
            .Where(e => !e.IsDefaultOrEmpty)
            .CombineLatest(this.WhenAnyValue(vm => vm.SelectedDay))
            .Subscribe(tuple =>
            {
                var (entries, selectedDay) = tuple;

                var monday = selectedDay.LastMonday();
                var sunday = selectedDay.NextSunday();

                CurrentWeekEntries = entries.Where(e => e.Deadline >= monday && e.Deadline < sunday)
                    .ToImmutableList();
            });
    }

    private IObservable<ImmutableArray<Exam>> GetEntries(int accountId, DateTime date, bool forceSync = false)
    {
        var (firstDay, lastDay) = date.GetMondayOfFirstWeekAndSundayOfLastWeekOfMonth();

        return _examsService.GetExamsByDateRange(accountId, firstDay, lastDay, forceSync)
            .Select(e => e.ToImmutableArray());
    }
}