using Android.Views;
using Android.Widget;
using ChGK.Core.ViewModels;

namespace ChGK.Droid.Views
{
	public class LastAddedTournamentsView : MenuItemView
	{
		protected override int LayoutId {
			get {
				return Resource.Layout.LastAddedTournamentsView;
			}
		}
        
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
                    (ViewModel as LastAddedTournamentsViewModel).RefreshCommand.Execute(null);
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        ListView listView;
        HeaderViewListAdapter adapter;

        public override void OnViewCreated(View view, Android.OS.Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            listView = view.FindViewById<ListView>(Resource.Id.items);
            listView.Scroll += list_Scroll;

            adapter = ((HeaderViewListAdapter)listView.Adapter);
        }

        public override void OnDestroyView()
        {
            listView.Scroll -= list_Scroll;

            base.OnDestroyView();
        }

        void list_Scroll(object sender, AbsListView.ScrollEventArgs e)
        {
            var tournaments = (ViewModel as LastAddedTournamentsViewModel).Tournaments;
            for (int i = e.FirstVisibleItem - adapter.HeadersCount; i < e.FirstVisibleItem + e.VisibleItemCount - adapter.FootersCount && i < tournaments.Count; i++)
            {
                tournaments[i].OnShowing();
            }
        }
	}
}

