using Android.OS;
using Android.Views;
using Android.Widget;
using ChGK.Core.ViewModels;
using ChGK.Droid.Helpers;

namespace ChGK.Droid.Views
{
    public class FavoriteTournamentsView : MenuItemView
    {
        private ExpandableListView _listView;
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
                    _listView.CollapseAllGroups();
                    (ViewModel as FavoriteTournamentsViewModel)?.RefreshCommand?.Execute(null);
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            _listView = view.FindViewById<ExpandableListView>(Resource.Id.TournamentsListView);
        }
    }
}