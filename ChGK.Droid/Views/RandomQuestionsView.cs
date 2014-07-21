using Android.OS;
using Android.Views;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;

namespace ChGK.Droid.Views
{
	public class RandomQuestionsView : MenuItemView
	{
		protected override int LayoutId {
			get {
				return Resource.Layout.RandomQuestionsView;
			}
		}
	}
}