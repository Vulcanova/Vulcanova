using Prism.Navigation;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Vulcanova.Features.Homework.HomeworkDetails;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class HomeworkDetailsView : INavigationAware
{
    public static readonly BindableProperty HomeworkProperty =
        BindableProperty.Create(nameof(Homework), typeof(Homework), typeof(HomeworkDetailsView));

    public Homework Homework
    {
        get => (Homework) GetValue(HomeworkProperty);
        set => SetValue(HomeworkProperty, value);
    }

    public HomeworkDetailsView()
    {
        InitializeComponent();
    }

    public void OnNavigatedFrom(INavigationParameters parameters)
    {
    }

    public void OnNavigatedTo(INavigationParameters parameters)
    {
        Homework = (Homework) parameters[nameof(Homework)];
    }
}