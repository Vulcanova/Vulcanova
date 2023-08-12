using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Vulcanova.Core.Layout.Converters;

public class FallbackStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) 
        => value is string data && !string.IsNullOrEmpty(data) ? data : parameter;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}