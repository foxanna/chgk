using Android.OS;
using Android.Views;
using ChGK.Core.Services;
using ChGK.Core.Utils;
using ChGK.Core.ViewModels;
using MvvmCross.Droid.FullFragging.Views;
using MvvmCross.Platform;

namespace ChGK.Droid.Views
{
    public abstract class MenuItemIndependentView : MvxActivity
    {
        protected abstract int LayoutId { get; }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(LayoutId);

            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.Title = (ViewModel as MenuItemViewModel)?.Title;
        }

        protected override void OnStart()
        {
            base.OnStart();

            Mvx.Resolve<IGAService>().ReportScreenEnter(GetType().FullName);
        }

        public override bool OnMenuItemSelected(int featureId, IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    OnBackPressed();
                    return true;
                default:
                    return base.OnMenuItemSelected(featureId, item);
            }
        }

        protected override void OnDestroy()
        {
            (ViewModel as IViewLifecycle)?.OnViewDestroying();

            base.OnDestroy();
        }
    }
}