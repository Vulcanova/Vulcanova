using Prism.Ioc;

namespace Vulcanova.Features.Notes;

public static class Config
{
    public static void RegisterNotes(this IContainerRegistry container)
    {
        container.RegisterScoped<INotesRepository, NotesRepository>();
        container.RegisterScoped<INotesService, NotesService>();
    }
}