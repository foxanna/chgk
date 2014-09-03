using Android.App;
using Android.Widget;
using ChGK.Core.ViewModels.Search;

namespace ChGK.Droid.Views.Search
{
    [Activity(Label = "", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]	
    public class SearchQuestionsResultsView : MenuItemIndependentView
        //, AbsListView.IOnScrollListener
    {
        protected override int LayoutId
        {
            get
            {
                return Resource.Layout.SearchQuestionsResultsView;
            }
        }

        //protected override void OnCreate(Android.OS.Bundle bundle)
        //{
        //    base.OnCreate(bundle);

        //    var list = FindViewById<ListView>(Resource.Id.items);
        //    list.SetOnScrollListener(this);
        //}

        //public void OnScroll(AbsListView view, int firstVisibleItem, int visibleItemCount, int totalItemCount)
        //{
        //    var loadMore = firstVisibleItem + visibleItemCount >= totalItemCount - 5;

        //    if (totalItemCount > 0 && loadMore)// && currentScrollState != ScrollState.Idle)
        //    {
        //        try
        //        {
        //            var viewModel = ViewModel as SearchQuestionsResultsViewModel;
        //            if (viewModel.LoadMoreCommand.CanExecute(totalItemCount))
        //            {
        //                viewModel.LoadMoreCommand.Execute(totalItemCount);
        //            }
        //        }
        //        finally
        //        {

        //        }
        //    }            
        //}

        //ScrollState currentScrollState;

        //public void OnScrollStateChanged(AbsListView view, ScrollState scrollState)
        //{
        //    currentScrollState = scrollState;
        //}
    }
}
