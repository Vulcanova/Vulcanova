using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Prism.Navigation;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Vulcanova.Core.Mvvm;
using Vulcanova.Features.Auth.AccountPicker;
using Vulcanova.Features.Exams;
using Vulcanova.Features.Grades;
using Vulcanova.Features.Homework;
using Vulcanova.Features.LuckyNumber;
using Vulcanova.Features.Shared;
using Vulcanova.Features.Timetable;
using Vulcanova.Features.Timetable.Changes;

namespace Vulcanova.Features.Dashboard
{
    public class DashboardViewModel : ViewModelBase
    {
        public ReactiveCommand<bool, Unit> RefreshData { get; private set; }
        private ReactiveCommand<bool, IEnumerable<Grade>> GetGrades { get; }
        private ReactiveCommand<bool, IReadOnlyCollection<Homework.Homework>> GetHomework { get; }
        private ReactiveCommand<bool, ImmutableArray<Exam>> GetExams { get; }
        private ReactiveCommand<int, LuckyNumber.LuckyNumber> GetLuckyNumber { get; }
        private ReactiveCommand<bool, IReadOnlyDictionary<DateTime, IEnumerable<TimetableListEntry>>> GetTimetable { get; set; }
        
        private readonly ITimetableService _timetableService;
        private readonly ITimetableChangesService _timetableChangesService;

        [ObservableAsProperty] public LuckyNumber.LuckyNumber LuckyNumber { get; private set; }
        [ObservableAsProperty] private IReadOnlyDictionary<DateTime, IEnumerable<TimetableListEntry>> TimatableEntries { get; }
        [ObservableAsProperty] private ImmutableArray<Exam> ExamsEntries { get; }
        [ObservableAsProperty] private IReadOnlyCollection<Homework.Homework> HomeworkEntries { get; }
        [ObservableAsProperty] private IEnumerable<Grade> RawGrades { get; }

        [Reactive] public AccountAwarePageTitleViewModel AccountViewModel { get; set; }
        [Reactive] public IEnumerable<TimetableListEntry> CurrentDayTimetable { get; private set; }
        [Reactive] public IReadOnlyCollection<Exam> CurrentWeekExams { get; private set; }
        [Reactive] public IReadOnlyCollection<Homework.Homework> CurrentWeekHomework { get; private set; }
        [Reactive] public IEnumerable<Grade> CurrentWeekGrades { get; private set; }
        [Reactive] public DateTime SelectedDay { get; private set; } = DateTime.Now;

        private readonly IExamsService _examsService;
        private readonly ILuckyNumberService _luckyNumberService;
        private readonly IHomeworkService _homeworksService;
        private readonly IGradesService _gradesService;

        public DashboardViewModel(
            INavigationService navigationService,
            AccountContext accountContext,
            AccountAwarePageTitleViewModel accountViewModel,
            ILuckyNumberService luckyNumberService,
            ITimetableService timetableService,
            ITimetableChangesService timetableChangesService,
            IExamsService examsService,
            IGradesService gradesService,
            IHomeworkService homeworksService) : base(navigationService)
        {
            _luckyNumberService = luckyNumberService;
            _timetableService = timetableService;
            _timetableChangesService = timetableChangesService;
            _examsService = examsService;
            _homeworksService = homeworksService;
            _gradesService = gradesService;

            AccountViewModel = accountViewModel;

            GetLuckyNumber = ReactiveCommand.CreateFromTask((int accountId) => GetLuckyNumberAsync(accountId));
            GetLuckyNumber.ToPropertyEx(this, vm => vm.LuckyNumber);
            
            GetTimetable = ReactiveCommand.CreateFromObservable((bool forceSync) =>
                GetTimetableEntries(accountContext.Account.Id, SelectedDay, forceSync)
                    .SubscribeOn(RxApp.TaskpoolScheduler));

            GetTimetable.ToPropertyEx(this, vm => vm.TimatableEntries);
            
            GetExams = ReactiveCommand.CreateFromObservable((bool forceSync) =>
                _examsService.GetExamsByDateRange(accountContext.Account.Id, SelectedDay, SelectedDay.AddDays(7), forceSync)
                    .Select(e => e.ToImmutableArray()));
            
            GetExams.ToPropertyEx(this, vm => vm.ExamsEntries);

            GetGrades = ReactiveCommand.CreateFromObservable((bool forceSync) =>
            {
                var currentPeriod = accountContext.Account.Periods.Single(x => x.Current);
                return _gradesService
                    .GetPeriodGrades(accountContext.Account.Id, currentPeriod.Id, forceSync);
            });

            GetGrades.ToPropertyEx(this, vm => vm.RawGrades);
            
            GetHomework = ReactiveCommand.CreateFromTask(async (bool forceSync) =>
            {
                var currentPeriod = accountContext.Account.Periods.Single(x => x.Current);
                return await GetHomeworkEntries(accountContext.Account.Id, currentPeriod.Id, forceSync);
            });

            GetHomework.ToPropertyEx(this, vm => vm.HomeworkEntries);
            
            RefreshData = ReactiveCommand.CreateFromTask(async (bool forceRefresh) =>
            {
                await GetLuckyNumber.Execute(accountContext.Account.Id);
                await GetTimetable.Execute(forceRefresh);
                await GetExams.Execute(forceRefresh);
                await GetGrades.Execute(forceRefresh);
                await GetHomework.Execute(forceRefresh);
            });

            this.WhenAnyValue(vm => vm.TimatableEntries)
                .CombineLatest(this.WhenAnyValue(vm => vm.SelectedDay.Date))
                .Subscribe(tuple =>
                {
                    var (entries, selectedDay) = tuple;

                    if (entries != null && entries.TryGetValue(selectedDay, out var values))
                    {
                        CurrentDayTimetable = values;
                        return;
                    }

                    CurrentDayTimetable = null;
                });
            
            this.WhenAnyValue(vm => vm.ExamsEntries)
                .Where(e => !e.IsDefaultOrEmpty)
                .CombineLatest(this.WhenAnyValue(vm => vm.SelectedDay))
                .Subscribe(tuple =>
                {
                    var (entries, selectedDay) = tuple;

                    CurrentWeekExams = entries
                        .OrderBy(e => e.Deadline)
                        .ToImmutableList();
                });
            
            this.WhenAnyValue(vm => vm.HomeworkEntries)
                .WhereNotNull()
                .CombineLatest(this.WhenAnyValue(vm => vm.SelectedDay))
                .Subscribe(tuple =>
                {
                    var (entries, selectedDay) = tuple;

                    var firstDate = SelectedDay;
                    var lastDate = SelectedDay.AddDays(7);

                    CurrentWeekHomework = Array.AsReadOnly(entries
                        .Where(h => h.Deadline.Date >= firstDate.Date && lastDate.Date >= h.Deadline.Date)
                        .OrderBy(h => h.Deadline)
                        .ToArray());
                });
            
            this.WhenAnyValue(vm => vm.RawGrades)
                .WhereNotNull()
                .CombineLatest(this.WhenAnyValue(vm => vm.SelectedDay))
                .Subscribe(values =>
                {
                    var (grades, _) = values;
                    
                    CurrentWeekGrades = 
                        grades.Where(grade => 
                            (grade.DateCreated > SelectedDay.AddDays(-7) && grade.DateCreated <= SelectedDay) || 
                            (grade.DateModify > SelectedDay.AddDays(-7) && grade.DateModify <= SelectedDay))
                            .OrderByDescending(g => g.DateModify).ToList();
                });

            accountContext.WhenAnyValue(ctx => ctx.Account)
                .WhereNotNull()
                .Select(_ => false)
                .InvokeCommand(RefreshData);
        }
        
        private async Task<LuckyNumber.LuckyNumber> GetLuckyNumberAsync(int accountId)
        {
            return await _luckyNumberService.GetLuckyNumberAsync( accountId, SelectedDay );
        }
        
        private IObservable<IReadOnlyDictionary<DateTime, IEnumerable<TimetableListEntry>>> GetTimetableEntries(int accountId,
            DateTime monthAndYear, bool forceSync = false)
        {
            var changes = _timetableChangesService.GetChangesEntriesByMonth(accountId, monthAndYear, forceSync);

            return _timetableService.GetPeriodEntriesByMonth(accountId, monthAndYear, forceSync)
                .CombineLatest(changes)
                .Select(items => TimetableExtensions.ToDictionary(items.First, items.Second));
        }

        private IObservable<IReadOnlyCollection<Homework.Homework>> GetHomeworkEntries(int accountId, int periodId,
            bool forceSync = false)
        {
            return _homeworksService.GetHomework(accountId, periodId, forceSync)
                .Select(e => Array.AsReadOnly(e.ToArray()));
        }
    }
}