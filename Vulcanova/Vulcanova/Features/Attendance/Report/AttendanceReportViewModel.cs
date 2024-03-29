using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Vulcanova.Features.Shared;

namespace Vulcanova.Features.Attendance.Report;

public class AttendanceReportViewModel : ReactiveObject
{
    [ObservableAsProperty]
    private IReadOnlyCollection<AttendanceReport> Reports { get; }

    public ReactiveCommand<int, Unit> SelectReport { get; }
    
    [Reactive]
    public AttendanceReport SelectedReport { get; private set; }

    [ObservableAsProperty] public IEnumerable<object> ReportItems { get; }

    public AttendanceReportViewModel(
        IAttendanceReportRepository attendanceReportRepository,
        AccountContext accountContext)
    {
        var fetchReports = ReactiveCommand.CreateFromTask(async (Unit _) =>
        {
            var items = await attendanceReportRepository.GetAttendanceReportsAsync(accountContext.Account.Id);

            return Array.AsReadOnly(items.OrderBy(x => x.Subject.Name).ToArray());
        });

        fetchReports.ToPropertyEx(this, vm => vm.Reports);

        this.WhenAnyValue(vm => vm.Reports)
            .WhereNotNull()
            .Select(reports => reports.Cast<object>().Prepend(reports.CalculateOverallAttendance()))
            .ToPropertyEx(this, vm => vm.ReportItems);

        accountContext.WhenAnyValue(ctx => ctx.Account)
            .WhereNotNull()
            .Select(_ => Unit.Default)
            .InvokeCommand(fetchReports);

        SelectReport = ReactiveCommand.Create((int index) =>
        {
            SelectedReport = Reports.ElementAtOrDefault(index);
        });

        MessageBus.Current.Listen<AttendanceReportUpdatedEvent>()
            .Select(_ => Unit.Default)
            .InvokeCommand(fetchReports);
    }
}