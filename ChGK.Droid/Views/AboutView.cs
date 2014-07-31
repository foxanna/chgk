using Android.App;
using Android.Views;
using Android.Text.Method;
using Android.Widget;

namespace ChGK.Droid.Views
{
	[Activity (Label = "", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]			
	public class AboutView : MenuItemIndependentView
	{
		protected override int LayoutId {
			get {
				return Resource.Layout.AboutView;
			}
		}

		protected override void OnCreate (Android.OS.Bundle bundle)
		{
			base.OnCreate (bundle);

			FindViewById<TextView> (Resource.Id.a1).MovementMethod = LinkMovementMethod.Instance;
			FindViewById<TextView> (Resource.Id.a2).MovementMethod = LinkMovementMethod.Instance;
		}
	}
}

