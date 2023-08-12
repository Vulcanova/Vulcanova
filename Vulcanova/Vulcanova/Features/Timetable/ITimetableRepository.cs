namespace Vulcanova.Features.Timetable;

public interface ITimetableRepository
{
    Task<IEnumerable<TimetableEntry>> GetEntriesForPupilAsync(int accountId, int pupilId, DateTime monthAndYear);

    Task UpdatePupilEntriesAsync(IEnumerable<TimetableEntry> entries, DateTime monthAndYear);
}