using System;
using Android.OS;
using Android.Views;
using ChGK.Core.ViewModels;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;

namespace ChGK.Droid.Views
{
	public abstract class MenuItemView : MvxFragment
	{
		public MenuItemView ()
		{
			RetainInstance = true;
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			base.OnCreateView (inflater, container, savedInstanceState);
			return this.BindingInflate (LayoutId, null);
		}

		public override void OnViewCreated (View view, Bundle savedInstanceState)
		{
			base.OnViewCreated (view, savedInstanceState);
			RetainInstance = true;
			HasOptionsMenu = true;
			Activity.Title = (ViewModel as MenuItemViewModel).Title;
		}

		protected abstract int LayoutId {
			get;
		}

		public override void OnCreateOptionsMenu (IMenu menu, MenuInflater inflater)
		{
			base.OnCreateOptionsMenu (menu, inflater);

			inflater.Inflate (Resource.Menu.menuitem, menu);
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			switch (item.ItemId) {
			case Resource.Id.refresh:
				(ViewModel as MenuItemViewModel).RefreshCommand.Execute (null);
				return true;
			default:
				return base.OnOptionsItemSelected (item);
			}
		}
	}
}

