using System.Reactive.Linq;
using ReactiveUI;

namespace Vulcanova.Features.Attendance;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class LessonView
{
    public static readonly BindableProperty LessonProperty =
        BindableProperty.Create(nameof(Lesson), typeof(Lesson), typeof(LessonView));

    public Lesson Lesson
    {
        get => (Lesson) GetValue(LessonProperty);
        set => SetValue(LessonProperty, value);
    }

    public static readonly BindableProperty JustificationModeProperty =
        BindableProperty.Create(nameof(JustificationMode), typeof(bool), typeof(LessonView));

    public bool JustificationMode
    {
        get => (bool) GetValue(JustificationModeProperty);
        set => SetValue(JustificationModeProperty, value);
    }
    
    public static readonly BindableProperty IsSelectedProperty =
        BindableProperty.Create(nameof(IsSelected), typeof(bool), typeof(LessonView));

    public bool IsSelected
    {
        get => (bool) GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }

    public LessonView()
    {
        InitializeComponent();

        var showCol = new Animation(v => Col1.Width = v, 0, 40, Easing.Linear);
        var hideCol = new Animation(v => Col1.Width = v, 40, 0, Easing.Linear);

        this.WhenAnyValue(v => v.JustificationMode)
            .WithLatestFrom(this.WhenAnyValue(l => l.Lesson).WhereNotNull())
            .Where(o => o.Second.CanBeJustified)
            .Select(o => o.First)
            .Subscribe(v => (v ? showCol : hideCol).Commit(this, "GridColWidthAnim", length: 150));
    }
}