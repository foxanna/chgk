using System;
using Cirrious.CrossCore.Converters;
using Android.Text;

namespace ChGK.Droid.Helpers
{
	public class HtmlToSpannableValueConverter : MvxValueConverter<string, ISpanned>
	{
		protected override ISpanned Convert (string value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return Html.FromHtml (value);
		}
	}
}

