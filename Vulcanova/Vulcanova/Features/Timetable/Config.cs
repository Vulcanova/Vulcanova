using Vulcanova.Features.Timetable.Changes;

namespace Vulcanova.Features.Timetable;

public static class Config
{
    public static void RegisterTimetable(this IContainerRegistry container)
    {
        container.RegisterForNavigation<TimetableView>();
        container.RegisterForNavigation<TimetableEntryDetailsView>();

        container.RegisterScoped<ITimetableRepository, TimetableRepository>();
        container.RegisterScoped<ITimetableService, TimetableService>();

        container.RegisterScoped<ITimetableChangesRepository, TimetableChangesRepository>();
        container.RegisterScoped<ITimetableChangesService, TimetableChangesService>();
    }
}