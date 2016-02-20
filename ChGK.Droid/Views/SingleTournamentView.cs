using System.Collections.Generic;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using ChGK.Core.ViewModels;
using MvvmCross.Binding.BindingContext;

namespace ChGK.Droid.Views
{
    [Activity(Label = "", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class SingleTournamentView : MenuItemIndependentView
    {
        private ExpandableListView _listView;
        protected override int LayoutId => Resource.Layout.SingleTournamentView;

        public List<TournamentViewModel> Data
        {
            get { return null; }
            set
            {
                if (value != null && value.Count > 0)
                {
                    _listView.ExpandGroup(0);
                }
            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            _listView = FindViewById<ExpandableListView>(Resource.Id.tournamentsList);

            Data = (ViewModel as SingleTournamentViewModel)?.Tournaments;

            var bindingSet = this.CreateBindingSet<SingleTournamentView, SingleTournamentViewModel>();
            bindingSet.Bind(this).For(n => n.Data).To(vm => vm.Tournaments);
            bindingSet.Apply();
        }
    }
}