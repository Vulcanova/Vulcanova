using System;
using System.Globalization;
using System.Linq;
using Vulcanova.Core.Layout;
using Vulcanova.Uonet.Api.Schedule;
using Xamarin.Forms;

namespace Vulcanova.Features.Timetable;

public class TimetableChangeColorConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        var value = values.FirstOrDefault();
        
        if (value is TimetableListEntry)
        {
            var entry = (TimetableListEntry)values.FirstOrDefault();
            var nowDate = DateTime.Now;
            var lessonDate = entry.Date;
            var startDate = entry.Start;
            var endDate = entry.End;
            if(nowDate.Date == lessonDate.Date && startDate.TimeOfDay < nowDate.TimeOfDay && nowDate.TimeOfDay < endDate.TimeOfDay)
                return ThemeUtility.GetThemedColorByResourceKey("PrimaryColor");
            
            value = entry.Change;
        }

        if (value is TimetableListEntry.ChangeDetails change)
        {
            var baseColor = change switch
            {
                {ChangeType: ChangeType.Exemption or ChangeType.ClassAbsence} => "ErrorColor",
                {ChangeType: ChangeType.Substitution} => "WarningColor",
                {
                    ChangeType: ChangeType.Rescheduled, RescheduleKind: TimetableListEntry.RescheduleKind.Added
                } => "WarningColor",
                {
                    ChangeType: ChangeType.Rescheduled, RescheduleKind: TimetableListEntry.RescheduleKind.Removed
                } => "ErrorColor",
                _ => "WarningColor"
            };

            return ThemeUtility.GetThemedColorByResourceKey(baseColor);
        }

        return ThemeUtility.GetDefaultTextColor();
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}