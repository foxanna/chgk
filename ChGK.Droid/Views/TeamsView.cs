using Android.App;
using Android.Views;
using Android.Widget;
using ChGK.Core.ViewModels;
using ChGK.Droid.Controls.SwipeToDismiss;

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

		protected override void OnCreate (Android.OS.Bundle bundle)
		{
			base.OnCreate (bundle);

			var listView = FindViewById <ListView> (Resource.Id.teams);

			var touchListener = new SwipeDismissListViewTouchListener (listView, 
				                    (ViewModel as TeamsViewModel).RemoveCommand);
			listView.SetOnTouchListener (touchListener);
			listView.SetOnScrollListener (touchListener);
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
	}
}

