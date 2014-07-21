using Android.App;
using Cirrious.MvvmCross.Droid.Views;

namespace ChGK.Droid.Views
{
	[Activity (Label = "", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize, 
		Theme = "@android:style/Theme.Translucent.NoTitleBar")]
	public class FullImageView : MvxActivity
	{
		protected override void OnCreate (Android.OS.Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.FullImageView);
		}
	}
}

