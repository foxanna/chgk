using Android.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChGK.Droid.Views.Search
{
    [Activity(Label = "", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]	
    public class SearchResultsView : MenuItemIndependentView
    {
        protected override int LayoutId
        {
            get
            {
                return Resource.Layout.SearchResultsView;
            }
        }
    }
}
