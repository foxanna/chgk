using Android.App;
using Android.Content.PM;

namespace ChGK.Droid.Views.Search
{
    [Activity(Label = "", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class SearchQuestionSingleResultView : MenuItemIndependentView
    {
        protected override int LayoutId => Resource.Layout.SearchQuestionSingleResultView;
    }
}