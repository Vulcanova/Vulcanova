using System;
using System.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using Vulcanova.Core.Native;
using Vulcanova.Features.Auth;
using Vulcanova.Features.Shared;

namespace Vulcanova.Features.Attendance.Report;

public class AttendanceReportService : IAttendanceReportService
{
    private readonly ILessonsRepository _lessonsRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IAttendanceReportRepository _attendanceReportRepository;
    private readonly INativeWidgetProxy _nativeWidgetProxy;

    public AttendanceReportService(
        ILessonsRepository lessonsRepository,
        IAccountRepository accountRepository,
        IAttendanceReportRepository attendanceReportRepository,
        INativeWidgetProxy nativeWidgetProxy = null)
    {
        _lessonsRepository = lessonsRepository;
        _accountRepository = accountRepository;
        _attendanceReportRepository = attendanceReportRepository;
        _nativeWidgetProxy = nativeWidgetProxy;
    }

    public async Task InvalidateReportsAsync(int accountId)
    {
        var account = await _accountRepository.GetByIdAsync(accountId);
        var (yearStart, yearEnd) = account.GetSchoolYearDuration();

        var entries = (await _lessonsRepository.GetLessonsBetweenAsync(accountId, yearStart, yearEnd))
            .Where(l => l.PresenceType != null && l.CalculatePresence && l.Subject != null)
            .ToArray();

        var entriesBySubject = entries
            .GroupBy(e => e.Subject.Id);

        var generationDate = DateTime.UtcNow;

        var reports = entriesBySubject.Select(g =>
        {
            var subject = g.First().Subject;

            return new AttendanceReport
            {
                Id = $"{accountId}/{subject.Id}",
                AccountId = accountId,
                DateGenerated = generationDate,
                Absence = g.Count(x => x.PresenceType.Absence),
                Late = g.Count(x => x.PresenceType.Late),
                Presence = g.Count(x => x.PresenceType.Presence && !x.PresenceType.Late),
                Subject = subject
            };
        });

        await _attendanceReportRepository.UpdateAttendanceReportsAsync(accountId, reports.ToArray());

        _nativeWidgetProxy?.UpdateWidgetState(INativeWidgetProxy.NativeWidget.AttendanceStats,
            new { Percentage = reports.FirstOrDefault()?.PresencePercentage ?? 77.77 });

        MessageBus.Current.SendMessage(new AttendanceReportUpdatedEvent());
    }
}

public record AttendanceReportUpdatedEvent;