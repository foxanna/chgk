using Android.App;
using Android.OS;
using Cirrious.MvvmCross.Droid.Views;
using ChGK.Core.ViewModels;

namespace ChGK.Droid.Views
{
	[Activity (Label = "")]		
	public class TourView : MvxActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.TourView);

			ActionBar.Title = ((TourViewModel) ViewModel).Name;
			ActionBar.SetDisplayHomeAsUpEnabled (true);
		}

		public override bool OnOptionsItemSelected (Android.Views.IMenuItem item)
		{
			if (item.ItemId == Android.Resource.Id.Home) {
				OnBackPressed ();
				return true;
			} else {
				return base.OnOptionsItemSelected (item);
			}
		}
	}
}

