using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Prism.Navigation;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Vulcanova.Core.Mvvm;
using Vulcanova.Core.Rx;
using Vulcanova.Features.Auth.AccountPicker;
using Vulcanova.Features.Shared;

namespace Vulcanova.Features.Notes;

public class NotesViewModel : ViewModelBase
{
    private readonly INotesService _notesService;

    public NotesViewModel(
        INavigationService navigationService,
        AccountContext accountContext,
        AccountAwarePageTitleViewModel accountViewModel,
        IPeriodService periodService,
        INotesService notesService) : base(navigationService)
    {
        _notesService = notesService;

        AccountViewModel = accountViewModel;

        var setCurrentPeriod =
            ReactiveCommand.CreateFromTask(async (int accountId) =>
                PeriodInfo = await periodService.GetCurrentPeriodAsync(accountId));

        accountContext.WhenAnyValue(ctx => ctx.Account)
            .WhereNotNull()
            .Select(acc => acc.Id)
            .InvokeCommand(setCurrentPeriod);

        GetNotesEntries = ReactiveCommand.CreateFromObservable((bool forceSync) =>
            GetEntries(accountContext.Account.Id, forceSync));

        GetNotesEntries.ToPropertyEx(this, vm => vm.Entries);

        this.WhenAnyValue(vm => vm.PeriodInfo)
            .WhereNotNull()
            .Subscribe(_ => { CurrentPeriodEntries = SortNotesByPeriod(); });

        this.WhenAnyValue(vm => vm.Entries)
            .Subscribe(entries =>
            {
                if (Entries == null) GetNotesEntries.Execute(false).SubscribeAndIgnoreErrors();
                ;
            });

        NextSemester = ReactiveCommand.CreateFromTask(async () =>
        {
            PeriodInfo =
                await periodService.ChangePeriodAsync(accountContext.Account.Id, PeriodChangeDirection.Next);
        });

        PreviousSemester = ReactiveCommand.CreateFromTask(async () =>
        {
            PeriodInfo =
                await periodService.ChangePeriodAsync(accountContext.Account.Id, PeriodChangeDirection.Previous);
        });
    }

    public ReactiveCommand<bool, IReadOnlyCollection<Note>> GetNotesEntries { get; }
    public ReactiveCommand<Unit, Unit> NextSemester { get; }
    public ReactiveCommand<Unit, Unit> PreviousSemester { get; }

    [ObservableAsProperty] public IReadOnlyCollection<Note> Entries { get; }

    [Reactive] public IReadOnlyCollection<Note> CurrentPeriodEntries { get; private set; }
    [Reactive] public PeriodResult PeriodInfo { get; private set; }
    [Reactive] public AccountAwarePageTitleViewModel AccountViewModel { get; }

    private ImmutableList<Note> SortNotesByPeriod()
    {
        return Entries.Where(n =>
                n.DateModified >= PeriodInfo.CurrentPeriod.Start &&
                n.DateModified <= PeriodInfo.CurrentPeriod.End)
            .ToImmutableList();
    }

    private IObservable<IReadOnlyCollection<Note>> GetEntries(int accountId, bool forceSync = false)
    {
        return _notesService.GetNotes(accountId, forceSync)
            .Select(e => e.ToArray());
    }
}