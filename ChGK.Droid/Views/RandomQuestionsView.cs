using System.Collections.Generic;
using Android.OS;
using Android.Views;
using Android.Widget;
using ChGK.Core.Models;
using ChGK.Core.ViewModels;
using MvvmCross.Binding.BindingContext;

namespace ChGK.Droid.Views
{
    public class RandomQuestionsView : MenuItemView
    {
        private ListView _listView;
        protected override int LayoutId => Resource.Layout.RandomQuestionsView;

        public List<IQuestion> Data
        {
            get { return null; }
            set { _listView.SetSelectionAfterHeaderView(); }
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
                    (ViewModel as RandomQuestionsViewModel)?.RefreshCommand?.Execute(null);
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            _listView = view.FindViewById<ListView>(Resource.Id.items);

            var bindingSet = this.CreateBindingSet<RandomQuestionsView, RandomQuestionsViewModel>();
            bindingSet.Bind(this).For(n => n.Data).To(vm => vm.Questions);
            bindingSet.Apply();
        }
    }
}