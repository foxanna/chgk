using System;
using System.Globalization;
using MvvmCross.Platform.Converters;

namespace ChGK.Core.Converters
{
    public class TimerValueConverter : MvxValueConverter<TimeSpan, string>
    {
        protected override string Convert(TimeSpan value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString("m\\:ss");
        }
    }
}