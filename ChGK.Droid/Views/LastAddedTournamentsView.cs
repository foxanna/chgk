using Android.Views;
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
                    Refresh();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        async void Refresh()
        {
            await (ViewModel as LastAddedTournamentsViewModel).Refresh();
        }
	}
}

