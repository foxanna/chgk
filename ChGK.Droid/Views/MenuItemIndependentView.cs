using Android.App;
using Android.OS;
using Android.Views;
using ChGK.Core.ViewModels;
using Cirrious.MvvmCross.Droid.Views;

namespace ChGK.Droid.Views
{
	public abstract class MenuItemIndependentView : MvxActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (LayoutId);

			ActionBar.SetDisplayHomeAsUpEnabled (true);
			ActionBar.Title = (ViewModel as MenuItemViewModel).Title;
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

