using Android.OS;
using Android.Views;
using ChGK.Core.ViewModels;
using ChGK.Droid.Helpers;
using Cirrious.MvvmCross.Droid.Fragging;

namespace ChGK.Droid.Views
{
	public abstract class MenuItemIndependentView : MvxFragmentActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (LayoutId);
            
			ActionBar.SetDisplayHomeAsUpEnabled (true);
			ActionBar.Title = (ViewModel as MenuItemViewModel).Title;
		}

        protected override void OnStart()
        {
            base.OnStart();
            
            GoogleAnalyticsManager.SendScreen(this.GetType().FullName);         
        }

		public override bool OnMenuItemSelected (int featureId, IMenuItem item)
		{
			switch (item.ItemId) {
			case Android.Resource.Id.Home:
				OnBackPressed ();
				return true;
			default:
				return base.OnMenuItemSelected (featureId, item);
			}
		}

		protected abstract int LayoutId {
			get;
		}
	}
}