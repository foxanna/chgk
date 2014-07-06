using Android.App;
using Android.OS;
using Cirrious.MvvmCross.Droid.Views;

namespace ChGK.Droid.Views
{
	[Activity (Label = "ЧГК")]
	public class HomeView :  MvxActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.HomeView);
		}
	}
}

