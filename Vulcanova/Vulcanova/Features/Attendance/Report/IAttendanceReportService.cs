namespace Vulcanova.Features.Attendance.Report;

public interface IAttendanceReportService
{
    Task InvalidateReportsAsync(int accountId);
}