namespace Vulcanova.Features.Attendance.Report;

public interface IAttendanceReportRepository
{
    Task<IEnumerable<AttendanceReport>> GetAttendanceReportsAsync(int accountId);

    Task UpdateAttendanceReportsAsync(int accountId, ICollection<AttendanceReport> reports);
}