using System;
using System.Globalization;
using System.Linq;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Vulcanova.Features.Messages;

public class CorrespondentToInitialsConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        value switch
        {
            string s => string.Join(string.Empty, s.Split(' ')
                .Take(2)
                .Select(x => x[0])),
            { } => throw new ArgumentException($"Unsupported argument of type {value.GetType()}",
                nameof(value)),
            null => throw new ArgumentNullException(nameof(value))
        };

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}