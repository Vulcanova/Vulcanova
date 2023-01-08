using System;
using System.Globalization;
using Vulcanova.Resources;
using Xamarin.Forms;

namespace Vulcanova.Core.Layout.Converters;

public class BooleanToYesNoConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (bool)value ? Strings.CommonYes : Strings.CommonNo;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}