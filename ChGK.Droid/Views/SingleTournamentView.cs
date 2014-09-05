using Android.App;
using Android.OS;
using Android.Widget;
using ChGK.Core.ViewModels;
using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.BindingContext;

namespace ChGK.Droid.Views
{
    [Activity(Label = "", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class SingleTournamentView : MenuItemIndependentView
    {
        protected override int LayoutId
        {
            get
            {
                return Resource.Layout.SingleTournamentView;
            }
        }

        ExpandableListView listView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            listView = FindViewById<ExpandableListView>(Resource.Id.tournamentsList);
                
            Data = (ViewModel as SingleTournamentViewModel).Tournaments;

            var bindingSet = this.CreateBindingSet<SingleTournamentView, SingleTournamentViewModel>();
            bindingSet.Bind(this).For(n => n.Data).To(vm => vm.Tournaments);
            bindingSet.Apply();
        }

        public List<TournamentViewModel> Data
        {
            get
            {
                return null;
            }
            set
            {
                if (value != null && value.Count > 0)
                {
                    listView.ExpandGroup(0);
                }
            }
        }
    }
}