using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Vulcanova.Features.Timetable;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class TimetableListEntryView
{
    public static readonly BindableProperty EntryProperty =
        BindableProperty.Create(nameof(Entry), typeof(TimetableListEntry), typeof(TimetableListEntryView));

    public TimetableListEntry Entry
    {
        get => (TimetableListEntry) GetValue(EntryProperty);
        set => SetValue(EntryProperty, value);
    }

    public TimetableListEntryView()
    {
        InitializeComponent();
    }
}