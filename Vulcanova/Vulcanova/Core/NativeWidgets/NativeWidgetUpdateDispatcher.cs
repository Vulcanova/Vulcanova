using System;
using ReactiveUI;
using Vulcanova.Core.Uonet;
using Vulcanova.Features.Attendance.Report;

namespace Vulcanova.Core.NativeWidgets;

public sealed class NativeWidgetUpdateDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public NativeWidgetUpdateDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void Setup()
    {
        MessageBus.Current.Listen<AttendanceReportUpdatedEvent>()
            .Subscribe(ProcessEvent);
    }

    private void ProcessEvent<T>(T @event) where T : UonetDataUpdatedEvent
    {
        var updater = (IWidgetUpdater<T>) _serviceProvider.GetService(typeof(IWidgetUpdater<T>));
        updater?.Handle(@event);
    }
}