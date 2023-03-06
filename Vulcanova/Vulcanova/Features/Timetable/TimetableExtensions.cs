using System;
using System.Collections.Generic;
using System.Linq;
using Vulcanova.Features.Timetable.Changes;

namespace Vulcanova.Features.Timetable;

public class TimetableExtensions
{
    public static IReadOnlyDictionary<DateTime, IEnumerable<TimetableListEntry>> ToDictionary(
        IEnumerable<TimetableEntry> lessons, IEnumerable<TimetableChangeEntry> changes)
    {
        var result = new Dictionary<DateTime, IEnumerable<TimetableListEntry>>();

        var groups = lessons.Where(l => l.Visible).GroupBy(e => e.Date.Date);

        // avoid multiple enumerations
        var timetableChangeEntries = changes as TimetableChangeEntry[] ?? changes.ToArray();

        foreach (var group in groups)
        {
            var entries = new List<TimetableListEntry>();
            foreach (var lesson in group.OrderBy(e => e.Start))
            {
                var change = timetableChangeEntries
                    .FirstOrDefault(c => c.TimetableEntryId == lesson.Id);

                var entry = new TimetableListEntry
                {
                    No = lesson.No,
                    Date = lesson.Date,
                    Start = lesson.Start,
                    End = lesson.End,
                    SubjectName = change?.Subject?.Name ?? lesson.Subject?.Name,
                    RoomName = change?.RoomName ?? lesson.RoomName,
                    TeacherName = change?.TeacherName ?? lesson.TeacherName,
                    Change = change?.Change.Type,
                    ChangeNote = change?.Note ?? change?.Reason
                };

                entries.Add(entry);
            }

            result[group.Key] = entries.AsReadOnly();
        }

        return result;
    }
}