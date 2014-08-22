using Android.App;

namespace ChGK.Droid.Views
{
    [Activity(Label = "", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class SingleTournamentView : MenuItemIndependentView
    {
        protected override int LayoutId
        {
            get
            {
                return Resource.Layout.SingleTournamentView;
            }
        }
    }
}