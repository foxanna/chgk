using Android.Views;
using ChGK.Core.ViewModels;

namespace ChGK.Droid.Views
{
	public class RandomQuestionsView : MenuItemView
	{
		protected override int LayoutId {
			get {
				return Resource.Layout.RandomQuestionsView;
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
                    (ViewModel as RandomQuestionsViewModel).RefreshCommand.Execute(null);
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
	}
}