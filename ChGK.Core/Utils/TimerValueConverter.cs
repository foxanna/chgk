using System;
using Cirrious.CrossCore.Converters;

namespace ChGK.Core.Utils
{
	public class TimerValueConverter : MvxValueConverter<TimeSpan, string>
	{
		protected override string Convert (TimeSpan value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return value.ToString ("m\\:ss");
		}
	}
}

