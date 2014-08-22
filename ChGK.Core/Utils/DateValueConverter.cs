using Cirrious.CrossCore.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChGK.Core.Utils
{
    public class DateValueConverter : MvxValueConverter<DateTime, string>
    {
        protected override string Convert(DateTime value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.ToString("dd MMM yyyy");
        }
    }
}
