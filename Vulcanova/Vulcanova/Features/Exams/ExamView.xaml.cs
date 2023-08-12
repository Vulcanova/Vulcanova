using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Vulcanova.Features.Exams;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ExamView
{
    public static readonly BindableProperty ExamProperty =
        BindableProperty.Create(nameof(Exam), typeof(Exam), typeof(ExamView));

    public Exam Exam
    {
        get => (Exam) GetValue(ExamProperty);
        set => SetValue(ExamProperty, value);
    }

    public ExamView()
    {
        InitializeComponent();
    }
}