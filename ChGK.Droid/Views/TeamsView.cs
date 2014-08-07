using Android.App;
using Android.Views;
using Android.Widget;
using ChGK.Core.ViewModels;
using ChGK.Droid.Controls.SwipeToDismiss;
using ChGK.Droid.Controls.UndoBar;
using Cirrious.MvvmCross.Binding.BindingContext;
using ChGK.Core.Utils;

namespace ChGK.Droid.Views
{
	[Activity (Label = "", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]			
	public class TeamsView : MenuItemIndependentView
	{
		protected override int LayoutId {
			get {
				return Resource.Layout.TeamsView;
			}
		}

		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			MenuInflater.Inflate (Resource.Menu.teamsview, menu);
			return true;
		}

		ListView _listView;

		protected override void OnCreate (Android.OS.Bundle bundle)
		{
			base.OnCreate (bundle);

			_listView = FindViewById <ListView> (Resource.Id.teams);

			var touchListener = new SwipeDismissListViewTouchListener (_listView, 
				                    (ViewModel as TeamsViewModel).RemoveTeamCommand);
			_listView.SetOnTouchListener (touchListener);
			_listView.SetOnScrollListener (touchListener);

			var bindingSet = this.CreateBindingSet<TeamsView, TeamsViewModel> ();
			bindingSet.Bind (this).For (view => view.UndoBarData).To (vm => vm.UndoBarMetaData);
			bindingSet.Apply ();
		}

		UndoBar UndoBar;

		public UndoBarMetadata UndoBarData {
			set {
				if (value == null) {
					return;
				}

				if (UndoBar != null) {
					UndoBar.Hide ();
				}

				UndoBar = new UndoBar (this, value.Id, value.Text, _listView);
				UndoBar.Undo += (sender, e) => (ViewModel as TeamsViewModel).SomeThingUndone (e.EventID);
				UndoBar.Discard += (sender, e) => (ViewModel as TeamsViewModel).SomeThingConfirmed (e.EventID);

				UndoBar.Show ();
			}
			get {
				return null;
			}
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			switch (item.ItemId) {
			case Resource.Id.add_team: 
				(ViewModel as TeamsViewModel).InitAddTeam ();
				return true;
			case Resource.Id.clear_results: 
				(ViewModel as TeamsViewModel).ClearResults ();
				return true;
			default:			
				return base.OnOptionsItemSelected (item);
			}
		}

		protected override void OnDestroy ()
		{
			if (UndoBar != null) {
				UndoBar.Hide ();
			}
		
			base.OnDestroy ();
		}
	}
}

