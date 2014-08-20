using ChGK.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChGK.Droid.Services
{
    public class FirstViewStartInfoProvider : IFirstViewStartInfoProvider
    {
        const string PrefsFileName = "PrefsFileName";

        public bool IsSeenForTheFirstTime(Type type)
        {
            var settings = Android.App.Application.Context.GetSharedPreferences(PrefsFileName, 0);
            var seen = //settings.GetBoolean("seen_" + type.ToString(), false);
                false;
            return !seen;
        }

        public void SetSeen(Type type)
        {
            var settings = Android.App.Application.Context.GetSharedPreferences(PrefsFileName, 0).Edit();
            var seen = settings.PutBoolean("seen_" + type.ToString(), true).Commit();
        }
    }
}
