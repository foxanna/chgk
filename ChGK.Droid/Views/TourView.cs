using Android.App;
using Android.OS;
using Android.Widget;
using ChGK.Core.ViewModels;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.Droid.Views;

namespace ChGK.Droid.Views
{
	[Activity (Label = "")]
    public class TourView : MenuItemIndependentView
	{
        protected override int LayoutId
        {
            get 
            {
                return Resource.Layout.TourView; 
            }
        }        
	}
}

