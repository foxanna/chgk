using Android.OS;
using Android.Views;
using Android.Widget;
using ChGK.Core.ViewModels;
using ChGK.Droid.Helpers;

namespace ChGK.Droid.Views
{
    public class LastAddedTournamentsView : MenuItemView
    {
        private ExpandableListView _listView;

        protected override int LayoutId => Resource.Layout.LastAddedTournamentsView;

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            base.OnCreateOptionsMenu(menu, inflater);
            inflater.Inflate(Resource.Menu.menuitem, menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.refresh:
                    _listView.CollapseAllGroups();
                    _listView.SetSelection(0);
                    (ViewModel as LastAddedTournamentsViewModel)?.RefreshCommand?.Execute(null);
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            _listView = view.FindViewById<ExpandableListView>(Resource.Id.items);
            _listView.Scroll += list_Scroll;
        }

        public override void OnDestroyView()
        {
            _listView.Scroll -= list_Scroll;

            base.OnDestroyView();
        }

        private void list_Scroll(object sender, AbsListView.ScrollEventArgs e)
        {
            var tournaments = (ViewModel as LastAddedTournamentsViewModel)?.Tournaments;
            for (var i = e.FirstVisibleItem - _listView.HeaderViewsCount;
                i < e.FirstVisibleItem + e.VisibleItemCount - _listView.FooterViewsCount && i < tournaments.Count;
                i++)
            {
                tournaments[i]?.OnShowing();
            }
        }
    }
}