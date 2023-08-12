using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Vulcanova.Features.Timetable;

public class TimetableViewLessonNameConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        => values[0] is TimetableListEntry.OverridableRefValue<string> { Value: null }
            ? values[1]
            : values[0];

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}