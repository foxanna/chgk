using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using ChGK.Core.Utils;
using ChGK.Core.ViewModels;
using ChGK.Droid.Controls.SwipeToDismiss;
using ChGK.Droid.Controls.UndoBar;
using ChGK.Droid.Helpers;
using MvvmCross.Binding.BindingContext;

namespace ChGK.Droid.Views
{
    [Activity(Label = "", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class TeamsView : MenuItemIndependentView
    {
        private ListView _listView;
        private MenuItemWrapper _removeAllButton, _clearResults;
        private UndoBar _undoBar;

        protected override int LayoutId => Resource.Layout.TeamsView;

        public UndoBarMetadata UndoBarData
        {
            set
            {
                if (value == null)
                {
                    return;
                }

                _undoBar?.Hide();

                _undoBar = new UndoBar(this, value.Text, _listView);
                _undoBar.Undo += (sender, e) => (ViewModel as TeamsViewModel)?.UndoableActionUndone();
                _undoBar.Discard += (sender, e) => (ViewModel as TeamsViewModel)?.UndoableActionConfirmed();

                _undoBar.Show();
            }
            get { return null; }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.teamsview, menu);

            _removeAllButton = new MenuItemWrapper(menu.FindItem(Resource.Id.clear_teams));
            _clearResults = new MenuItemWrapper(menu.FindItem(Resource.Id.clear_results));

            var bindingSet = this.CreateBindingSet<TeamsView, TeamsViewModel>();
            bindingSet.Bind(_removeAllButton).For(n => n.Visible).To(vm => vm.CanRemoveTeams);
            bindingSet.Bind(_clearResults).For(n => n.Visible).To(vm => vm.CanClearScore);
            bindingSet.Apply();

            return true;
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            _listView = FindViewById<ListView>(Resource.Id.teams);

            var touchListener = new SwipeDismissListViewTouchListener(_listView,
                (ViewModel as TeamsViewModel)?.RemoveTeamCommand);
            _listView.SetOnTouchListener(touchListener);
            _listView.SetOnScrollListener(touchListener);

            var bindingSet = this.CreateBindingSet<TeamsView, TeamsViewModel>();
            bindingSet.Bind(this).For(view => view.UndoBarData).To(vm => vm.UndoBarMetaData);
            bindingSet.Apply();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.add_team:
                    _undoBar?.Hide();

                    (ViewModel as TeamsViewModel)?.InitAddTeam();
                    return true;
                case Resource.Id.clear_results:
                    (ViewModel as TeamsViewModel)?.ClearResults();
                    return true;
                case Resource.Id.clear_teams:
                    (ViewModel as TeamsViewModel)?.ClearTeams();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        protected override void OnDestroy()
        {
            _undoBar?.Hide();

            base.OnDestroy();
        }
    }
}