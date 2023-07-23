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

    public void Handle(AttendanceReportUpdatedEvent message)
    {
        _nativeWidgetProxy.UpdateWidgetState(INativeWidgetProxy.NativeWidget.AttendanceStats,
            new { Percentage = message.OverallAttendancePercentage });
    }
}