using System.Collections.Generic;
using System.Threading.Tasks;
using LiteDB.Async;

namespace Vulcanova.Features.Notes;

public class NotesRepository : INotesRepository
{
    private readonly LiteDatabaseAsync _db;

    public NotesRepository(LiteDatabaseAsync db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Note>> GetNotesByPupilAsync(int accountId, int pupilId)
    {
        return await _db.GetCollection<Note>()
            .FindAsync(n => n.AccountId == accountId);
    }

    public async Task UpdateNoteEntriesAsync(IEnumerable<Note> entries, int accountId)
    {
        await _db.GetCollection<Note>()
            .DeleteManyAsync(h => h.AccountId == accountId);

        await _db.GetCollection<Note>().UpsertAsync(entries);
    }
}