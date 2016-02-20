using System;
using Android.App;
using Android.OS;
using Android.Widget;

namespace ChGK.Droid.Controls
{
    public class DatePickerFragment : DialogFragment, DatePickerDialog.IOnDateSetListener
    {
        private Action<int, int, int> _onDateSet;
        private int _year, _monthOfYear, _dayOfMonth;

        public DatePickerFragment()
        {
            RetainInstance = true;
        }

        void DatePickerDialog.IOnDateSetListener.OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
            _onDateSet(year, monthOfYear + 1, dayOfMonth);
        }

        public static DatePickerFragment NewInstance(Action<int, int, int> onDateSet, int year, int monthOfYear,
            int dayOfMonth)
        {
            var fragment = new DatePickerFragment {_onDateSet = onDateSet};

            var args = new Bundle();
            args.PutInt("year", year);
            args.PutInt("monthOfYear", monthOfYear - 1);
            args.PutInt("dayOfMonth", dayOfMonth);
            fragment.Arguments = args;

            return fragment;
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
    }
}