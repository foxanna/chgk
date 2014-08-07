using Android.App;
using Android.Content.Res;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Widget;
using Android.Views;
using ChGK.Core.Utils;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Droid.Fragging;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;
using Cirrious.MvvmCross.Plugins.Messenger;

namespace ChGK.Droid.Views
{
	[Activity (Label = "", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
	public class HomeView : MvxFragmentActivity
	{
		DrawerLayout mDrawerLayout;
		ActionBarDrawerToggle mDrawerToggle;
		View mDrawerView;

		#pragma warning disable 414
		MvxSubscriptionToken _closeDrawerToken;
		#pragma warning restore 414

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.HomeView);

			mDrawerLayout = FindViewById<DrawerLayout> (Resource.Id.drawer);
			mDrawerView = FindViewById (Resource.Id.items);
			mDrawerToggle = new ActionBarDrawerToggle (this, mDrawerLayout, Resource.Drawable.ic_navigation_drawer, 0, 0);
			mDrawerLayout.SetDrawerListener (mDrawerToggle);

			ActionBar.SetHomeButtonEnabled (true);
			ActionBar.SetDisplayHomeAsUpEnabled (true);

			_closeDrawerToken = Mvx.Resolve<IMvxMessenger> ().SubscribeOnMainThread<CloseDrawerMessage> (
				message => mDrawerLayout.CloseDrawer (mDrawerView));     
		}

		public void ShowMenuItem (MvxFragment fragment)
		{
			var topFragment = SupportFragmentManager.FindFragmentById (Resource.Id.content_frame);
			if (topFragment != null && topFragment.GetType ().Equals (fragment.GetType ())) {
				return;
			}

			SupportFragmentManager.BeginTransaction ().Replace (Resource.Id.content_frame, fragment).Commit ();
		}

		protected override void OnPostCreate (Bundle savedInstanceState)
		{
			base.OnPostCreate (savedInstanceState);

			mDrawerToggle.SyncState ();
		}

		public override void OnConfigurationChanged (Configuration newConfig)
		{
			base.OnConfigurationChanged (newConfig);

			mDrawerToggle.OnConfigurationChanged (newConfig);
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			return mDrawerToggle.OnOptionsItemSelected (item) || base.OnOptionsItemSelected (item);
		}
	}
}

