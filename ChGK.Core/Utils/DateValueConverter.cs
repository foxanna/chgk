﻿using System;
using System.Globalization;
using MvvmCross.Platform.Converters;

namespace ChGK.Core.Utils
{
    public class DateValueConverter : MvxValueConverter<DateTime, string>
    {
        protected override string Convert(DateTime value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString("dd MMM yyyy");
        }
    }
}