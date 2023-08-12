﻿namespace Vulcanova.Features.Notes;

public interface INotesService
{
    IObservable<IEnumerable<Note>> GetNotes(int accountId, bool forceSync = false);
}