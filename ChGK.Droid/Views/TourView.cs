using Android.App;
using Android.OS;
using Cirrious.MvvmCross.Droid.Views;
using ChGK.Core.ViewModels;

namespace ChGK.Droid.Views
{
	[Activity ()]		
	public class TourView : MvxActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.TourView);

			ActionBar.Title = ((TourViewModel) ViewModel).Name;
		}
	}
}

