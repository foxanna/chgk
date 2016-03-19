using Android.Views;
using ChGK.Core.ViewModels;

namespace ChGK.Droid.Views
{
    public class FavoriteTournamentsView : MenuItemView
    {
        protected override int LayoutId => Resource.Layout.FavoriteTournamentsView;

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            base.OnCreateOptionsMenu(menu, inflater);
            inflater.Inflate(Resource.Menu.favoritetournamentsview, menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.refresh:
                    (ViewModel as FavoriteTournamentsViewModel)?.RefreshCommand?.Execute(null);
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}