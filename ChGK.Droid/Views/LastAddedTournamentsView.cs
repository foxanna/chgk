using Android.Util;
using Android.Views;
using Android.Widget;
using ChGK.Core.ViewModels;
using ChGK.Droid.Helpers;
using Cirrious.MvvmCross.Binding.BindingContext;

namespace ChGK.Droid.Views
{
	public class LastAddedTournamentsView : MenuItemView
	{
		protected override int LayoutId {
			get {
				return Resource.Layout.LastAddedTournamentsView;
			}
		}

        MenuItemWrapper refreshButton;

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            base.OnCreateOptionsMenu(menu, inflater);

            inflater.Inflate(Resource.Menu.menuitem, menu);

            refreshButton = new MenuItemWrapper(menu.FindItem(Resource.Id.refresh));

            var bindingSet = this.CreateBindingSet<LastAddedTournamentsView, LastAddedTournamentsViewModel>();
            bindingSet.Bind(refreshButton).For(n => n.Visible).To(vm => vm.DataLoader.HasError);
            bindingSet.Apply();            
        }

        public override void OnDestroyOptionsMenu()
        {
            BindingContext.ClearAllBindings();

            base.OnDestroyOptionsMenu();
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

        public override void OnViewCreated(View view, Android.OS.Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            listView = view.FindViewById<ListView>(Resource.Id.items);
            listView.Scroll += list_Scroll;
        }
        
        public override void OnDestroyView()
        {
            listView.Scroll -= list_Scroll;

            base.OnDestroyView();
        }

        void list_Scroll(object sender, AbsListView.ScrollEventArgs e)
        {
            var tournaments = (ViewModel as LastAddedTournamentsViewModel).Tournaments;
            for (int i = e.FirstVisibleItem - listView.HeaderViewsCount; 
                i < e.FirstVisibleItem + e.VisibleItemCount - listView.FooterViewsCount && i < tournaments.Count; i++)
            {
                tournaments[i].OnShowing();
            }
        }
	}
}

