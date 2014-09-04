using Android.App;
using Android.Widget;
using ChGK.Core.ViewModels.Search;
using Cirrious.MvvmCross.ViewModels;

namespace ChGK.Droid.Views.Search
{
    [Activity(Label = "", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]	
    public class SearchQuestionsResultsView : MenuItemIndependentView
    {
        protected override int LayoutId
        {
            get
            {
                return Resource.Layout.SearchQuestionsResultsView;
            }
        }

        ListView listView;

        protected override void OnCreate(Android.OS.Bundle bundle)
        {
            base.OnCreate(bundle);

            listView = FindViewById<ListView>(Resource.Id.items);
            listView.Scroll += list_Scroll;
        }

        protected override void OnDestroy()
        {
            listView.Scroll -= list_Scroll;

            base.OnDestroy();
        }

        void list_Scroll(object sender, AbsListView.ScrollEventArgs e)
        {
            var adapter = ((HeaderViewListAdapter)listView.Adapter);
            for (int i = e.FirstVisibleItem - adapter.HeadersCount; i < e.FirstVisibleItem + e.VisibleItemCount - adapter.FootersCount; i++)
            {
                (ViewModel as SearchQuestionsResultsViewModel).Questions[i].OnShowing();
            }
        }
    }
}
