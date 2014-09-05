using Android.Views;
using Android.Widget;
using ChGK.Core.Models;
using ChGK.Core.ViewModels;
using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.BindingContext;

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

        ListView _listView;

        public override void OnViewCreated(View view, Android.OS.Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            _listView = view.FindViewById<ListView>(Resource.Id.items);

            var bindingSet = this.CreateBindingSet<RandomQuestionsView, RandomQuestionsViewModel>();
            bindingSet.Bind(this).For(n => n.Data).To(vm => vm.Questions);
            bindingSet.Apply();
        }

        public List<IQuestion> Data
        {
            get
            {
                return null;
            }
            set
            {
                _listView.SetSelectionAfterHeaderView();
            }
        }
	}
}