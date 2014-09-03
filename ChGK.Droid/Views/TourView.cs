using Android.App;

namespace ChGK.Droid.Views
{
	[Activity (Label = "")]
    public class TourView : MenuItemIndependentView
	{
        protected override int LayoutId
        {
            get 
            {
                return Resource.Layout.TourView; 
            }
        }        
	}
}

