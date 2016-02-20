using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Widget;
using Android.Views;
using ChGK.Core.Services.Messenger;
using MvvmCross.Droid.FullFragging.Fragments;
using MvvmCross.Droid.FullFragging.Views;
using MvvmCross.Platform;

namespace ChGK.Droid.Views
{
    [Activity(Label = "", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class HomeView : MvxActivity
    {
#pragma warning disable 414
        private object _closeDrawerToken;
#pragma warning restore 414
        private DrawerLayout mDrawerLayout;
        private ActionBarDrawerToggle mDrawerToggle;
        private View mDrawerView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.HomeView);

            mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer);
            mDrawerView = FindViewById(Resource.Id.items);
            mDrawerToggle = new ActionBarDrawerToggle(this, mDrawerLayout, Resource.Drawable.ic_navigation_drawer, 0, 0);
            mDrawerLayout.SetDrawerListener(mDrawerToggle);

            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);

            _closeDrawerToken = Mvx.Resolve<IMessagesService>().SubscribeOnMainThread<CloseDrawerMessage>(
                message => mDrawerLayout.CloseDrawer(mDrawerView));
        }

        public void ShowMenuItem(MvxFragment fragment)
        {
            var topFragment = FragmentManager.FindFragmentById(Resource.Id.content_frame);
            if (topFragment != null && topFragment.GetType().Equals(fragment.GetType()))
            {
                return;
            }

            FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, fragment).Commit();
        }

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);

            mDrawerToggle.SyncState();
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);

            mDrawerToggle.OnConfigurationChanged(newConfig);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            return mDrawerToggle.OnOptionsItemSelected(item) || base.OnOptionsItemSelected(item);
        }
    }
}