using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Util;

namespace ChGK.Droid.Controls
{
    public class DatePickerFragment : Android.Support.V4.App.DialogFragment, DatePickerDialog.IOnDateSetListener
    {
        Action<int, int, int> _onDateSet;
        int _year, _monthOfYear, _dayOfMonth;
        
        public static DatePickerFragment NewInstance(Action<int, int, int> OnDateSet, int year, int monthOfYear, int dayOfMonth)
        {
            var fragment = new DatePickerFragment();
            fragment._onDateSet = OnDateSet;

            var args = new Bundle();
            args.PutInt("year", year);
            args.PutInt("monthOfYear", monthOfYear - 1);
            args.PutInt("dayOfMonth", dayOfMonth);
            fragment.Arguments = args;

            return fragment;
        }

        public DatePickerFragment()
        {
            RetainInstance = true;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _year = Arguments.GetInt("year");
            _monthOfYear = Arguments.GetInt("monthOfYear");
            _dayOfMonth = Arguments.GetInt("dayOfMonth");
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            return new DatePickerDialog(Activity, this, _year, _monthOfYear, _dayOfMonth);
        }
        
        void DatePickerDialog.IOnDateSetListener.OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
            _onDateSet(year, monthOfYear + 1, dayOfMonth);
        }
    }
}