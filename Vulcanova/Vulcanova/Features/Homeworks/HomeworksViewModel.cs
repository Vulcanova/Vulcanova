﻿using System;
using System.Reactive;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using Prism.Navigation;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Vulcanova.Core.Mvvm;
using Vulcanova.Core.Rx;
using Vulcanova.Extensions;
using Vulcanova.Features.Shared;

namespace Vulcanova.Features.Homeworks
{
    public class HomeworksViewModel : ViewModelBase
    {
        public ReactiveCommand<bool, ImmutableArray<Homework>> GetHomeworks { get; }
        public ReactiveCommand<Unit, ImmutableArray<Homework>> ForceRefreshHomeworks { get; }
        public ReactiveCommand<int, Unit> ShowHomeworkDetails { get; }
        
        [ObservableAsProperty] public ImmutableArray<Homework> Entries { get; }

        [Reactive] public IReadOnlyCollection<Homework> CurrentWeekEntries { get; private set; }
        [Reactive] public DateTime SelectedDay { get; set; } = DateTime.Today;
        [Reactive] public PeriodResult PeriodInfo { get; private set; }
        [Reactive] public Homework SelectedHomework { get; set; }

        private readonly IHomeworksService _homeworksService;

        public HomeworksViewModel(IHomeworksService homeworksService, AccountContext accountContext, INavigationService navigationService, IPeriodService periodService) : base(navigationService)
        {
            _homeworksService = homeworksService;

            var setCurrentPeriod = ReactiveCommand.CreateFromTask(async (int accountId) =>
                PeriodInfo = await periodService.GetCurrentPeriodAsync(accountId));
            
            accountContext.WhenAnyValue(ctx => ctx.AccountId)
                .InvokeCommand(setCurrentPeriod);

            GetHomeworks = ReactiveCommand.CreateFromObservable((bool forceSync) =>
                GetEntries(accountContext.AccountId, PeriodInfo.CurrentPeriod.Id, SelectedDay, forceSync));
            
            ForceRefreshHomeworks = ReactiveCommand.CreateFromObservable(() =>
                GetHomeworks.Execute(true));

            GetHomeworks.ToPropertyEx(this, vm => vm.Entries);

            ShowHomeworkDetails = ReactiveCommand.Create((int lessonId) =>
            {
                SelectedHomework = CurrentWeekEntries?.First(g => g.Id == lessonId);

                return Unit.Default;
            });

            var lastDate = SelectedDay;

            this.WhenAnyValue(vm => vm.SelectedDay)
                .Subscribe((d) =>
                {
                    if (Entries == null || lastDate.Month != d.Month)
                    {
                        GetHomeworks.Execute(false).SubscribeAndIgnoreErrors();
                    }
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

        private IObservable<ImmutableArray<Homework>> GetEntries(int accountId, int periodId, DateTime date,
            bool forceSync = false)
        {
            return _homeworksService.GetHomeworks(accountId, periodId, forceSync)
                .Select(e => e.ToImmutableArray());
        }
    }
}