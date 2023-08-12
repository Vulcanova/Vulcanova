namespace Vulcanova.Features.Exams;

public class ExamsGroup : List<Exam>
{
    public DateTime Date { get; }

    public ExamsGroup(DateTime date, IEnumerable<Exam> exams) : base(exams)
    {
        Date = date;
    }
}