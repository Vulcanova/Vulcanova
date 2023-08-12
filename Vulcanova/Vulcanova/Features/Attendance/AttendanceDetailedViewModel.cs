using System.Collections.ObjectModel;
using System.Reactive.Linq;
using DynamicData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Vulcanova.Core.Mvvm;
using Vulcanova.Core.Rx;
using Vulcanova.Features.Attendance.Justification;
using Vulcanova.Features.Attendance.LessonDetails;
using Vulcanova.Features.Shared;
using Vulcanova.Uonet.Api.Auth;
using Unit = System.Reactive.Unit;

namespace Vulcanova.Features.Attendance;

public class AttendanceDetailedViewModel : ViewModelBase, INavigatedAware
{
    public ReactiveCommand<bool, IReadOnlyDictionary<DateTime, List<LessonViewModel>>> GetAttendanceEntries { get; }

    public ReactiveCommand<int, Unit> ShowLessonDetails { get; }

    public ReactiveCommand<Unit, Unit> EnableJustificationMode { get; }
    public ReactiveCommand<Unit, Unit> DisableJustificationMode { get; }
    
    public ReactiveCommand<Unit, Unit> JustifyLessonsAttendance { get; }

    [ObservableAsProperty] private IReadOnlyDictionary<DateTime, List<LessonViewModel>> Entries { get; }

    public ReadOnlyObservableCollection<LessonViewModel> CurrentDayEntries => _currentDayEntries;
    private readonly ReadOnlyObservableCollection<LessonViewModel> _currentDayEntries;


    [Reactive] public DateTime SelectedDay { get; set; } = DateTime.Today;

    [Reactive] public bool JustificationMode { get; set; }


    private readonly ILessonsService _lessonsService;


    public AttendanceDetailedViewModel(
        ILessonsService lessonsService,
        AccountContext accountContext,
        INavigationService navigationService) : base(navigationService)
    {
        _lessonsService = lessonsService;

        var currentEntriesSource = new SourceList<LessonViewModel>();

        currentEntriesSource
            .Connect()
            .Bind(out _currentDayEntries)
            .Subscribe();

        GetAttendanceEntries = ReactiveCommand.CreateFromObservable((bool forceSync) =>
            GetEntries(accountContext.Account.Id, SelectedDay, forceSync));

        GetAttendanceEntries.ToPropertyEx(this, vm => vm.Entries);

        ShowLessonDetails = ReactiveCommand.Create((int lessonId) =>
        {
            var entry = CurrentDayEntries.Single(g => g.Lesson.Id == lessonId);

            if (JustificationMode)
            {
                if (entry.Lesson.CanBeJustified)
                {
                    entry.IsSelected = !entry.IsSelected;
                }

                return default;
            }

            NavigationService.NavigateAsync(nameof(LessonDetailsView), ("Lesson", entry.Lesson));

            return Unit.Default;
        });

        var hasAnyJustifiable = currentEntriesSource
            .Connect()
            .AutoRefresh()
            .ToCollection()
            .Select(x => x.Any(l => l.Lesson.CanBeJustified));

        EnableJustificationMode = ReactiveCommand.Create((Unit _) => { JustificationMode = true; }, accountContext
            .WhenAnyValue(context => context.Account)
            .WhereNotNull()
            .Select(a => a.Capabilities.Contains(AccountCapabilities.JustificationsEnabled))
            .CombineLatest(hasAnyJustifiable, (x, y) => x && y)
            .CombineLatest(this.WhenAnyValue(vm => vm.JustificationMode), (x, y) => x && !y));

        DisableJustificationMode = ReactiveCommand.Create((Unit _) =>
            {
                foreach (var l in Entries.Values.SelectMany(l => l))
                {
                    l.IsSelected = false;
                }
                
                JustificationMode = false;
            },
            this.WhenAnyValue(vm => vm.JustificationMode));

        hasAnyJustifiable
            .Where(v => v == false)
            .BindTo(this, vm => vm.JustificationMode);

        this.WhenAnyValue(vm => vm.SelectedDay)
            .Subscribe((d) =>
            {
                if (Entries != null)
                {
                    foreach (var l in Entries.Values.SelectMany(l => l))
                    {
                        l.IsSelected = false;
                    }
                }

                if (Entries == null || !Entries.TryGetValue(SelectedDay.Date, out _))
                {
                    GetAttendanceEntries.Execute(false).SubscribeAndIgnoreErrors();
                }
            });

        this.WhenAnyValue(vm => vm.Entries)
            .CombineLatest(this.WhenAnyValue(vm => vm.SelectedDay))
            .Subscribe(tuple =>
            {
                var (entries, selectedDay) = tuple;

                if (entries != null && entries.TryGetValue(selectedDay, out var values))
                {
                    currentEntriesSource.Edit(items =>
                    {
                        items.Clear();
                        items.AddRange(values);
                    });
                    return;
                }   

                currentEntriesSource.Edit(i => i.Clear());
            });

        var selectedAnyJustifiable = currentEntriesSource
            .Connect()
            .AutoRefresh(l => l.IsSelected)
            .ToCollection()
            .Select(x => x.Any(l => l.IsSelected));

        JustifyLessonsAttendance = ReactiveCommand.CreateFromTask(async (Unit _) =>
        {
            await navigationService.CreateBuilder()
                .AddSegment(nameof(JustifyAbsenceView), useModalNavigation: true)
                .WithParameters(
                    new NavigationParameters
                    {
                        {
                            nameof(JustifyAbsenceViewModel.Lessons),
                            CurrentDayEntries
                                .Where(e => e.IsSelected)
                                .Select(e => e.Lesson)
                        }
                    }).NavigateAsync();
        }, selectedAnyJustifiable);
        
        accountContext.WhenAnyValue(ctx => ctx.Account)
            .WhereNotNull()
            .Select(_ => false)
            .InvokeCommand(GetAttendanceEntries);
    }

    private IObservable<IReadOnlyDictionary<DateTime, List<LessonViewModel>>> GetEntries(int accountId,
        DateTime monthAndYear, bool forceSync = false)
    {
        return _lessonsService.GetLessonsByMonth(accountId, monthAndYear, forceSync)
            .Select(ToDictionary);
    }

    private static IReadOnlyDictionary<DateTime, List<LessonViewModel>> ToDictionary(IEnumerable<Lesson> lessons)
    {
        return lessons
            .GroupBy(l => l.Date)
            .ToDictionary(g => g.Key, g => g.OrderBy(l => l.No).Select(l => new LessonViewModel(l, false)).ToList());
    }

    public void OnNavigatedFrom(INavigationParameters parameters)
    {
    }

    public void OnNavigatedTo(INavigationParameters parameters)
    {
        if (parameters.TryGetValue("didJustify", out bool reload) && reload)
        {
            DisableJustificationMode.Execute(default).Subscribe();
            GetAttendanceEntries.Execute(true).Subscribe();
        }
    }
}

public class LessonViewModel : ReactiveObject
{
    public Lesson Lesson { get; }

    [Reactive]
    public bool IsSelected { get; set; }

    public LessonViewModel(Lesson lesson, bool isSelected)
    {
        Lesson = lesson;
        IsSelected = isSelected;
    }
}