using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using ChGK.Core.ViewModels.Search;

namespace ChGK.Droid.Views.Search
{
    [Activity(Label = "", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class SearchQuestionsResultsView : MenuItemIndependentView
    {
        private HeaderViewListAdapter _adapter;

        private ListView _listView;
        protected override int LayoutId => Resource.Layout.SearchQuestionsResultsView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            _listView = FindViewById<ListView>(Resource.Id.items);
            _listView.Scroll += list_Scroll;

            _adapter = ((HeaderViewListAdapter) _listView.Adapter);
        }

        protected override void OnDestroy()
        {
            _listView.Scroll -= list_Scroll;

            base.OnDestroy();
        }

        private void list_Scroll(object sender, AbsListView.ScrollEventArgs e)
        {
            for (var i = e.FirstVisibleItem - _adapter.HeadersCount;
                i < e.FirstVisibleItem + e.VisibleItemCount - _adapter.FootersCount;
                i++)
            {
                (ViewModel as SearchQuestionsResultsViewModel)?.Questions?[i]?.OnShowing();
            }
        }
    }
}