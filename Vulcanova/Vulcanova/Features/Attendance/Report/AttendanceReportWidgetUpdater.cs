using System.Threading.Tasks;
using Vulcanova.Core.NativeWidgets;
using Vulcanova.Core.Uonet;

namespace Vulcanova.Features.Attendance.Report;

public sealed class AttendanceReportWidgetUpdater : IWidgetUpdater<AttendanceReportUpdatedEvent>
{
    private readonly INativeWidgetProxy _nativeWidgetProxy;

    public AttendanceReportWidgetUpdater(INativeWidgetProxy nativeWidgetProxy)
    {
        _nativeWidgetProxy = nativeWidgetProxy;
    }

    public Task Handle(AttendanceReportUpdatedEvent message)
    {
        _nativeWidgetProxy.UpdateWidgetState(INativeWidgetProxy.NativeWidget.AttendanceStats,
            new { Percentage = float.IsNaN(message.OverallAttendancePercentage) ? 0 : message.OverallAttendancePercentage });

        return Task.CompletedTask;
    }
}