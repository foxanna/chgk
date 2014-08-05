using Android.App;
using Android.Views;
using ChGK.Core.ViewModels;
using Android.Widget;
using ChGK.Droid.Controls.SwipeToDismiss;
using System;

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
				                    new A (positions => (ViewModel as TeamsViewModel).Remove (positions)));
			listView.SetOnTouchListener (touchListener);
			listView.SetOnScrollListener (touchListener);
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			switch (item.ItemId) {
			case Resource.Id.add_team: 
				(ViewModel as TeamsViewModel).InitAddTeam ();
				return true;
			default:			
				return base.OnOptionsItemSelected (item);
			}
		}
	}

	class A : IDismissCallbacks
	{
		Action<int[] > a;

		public A (Action<int[] > a)
		{
			this.a = a;
		}

		public bool canDismiss (int position)
		{
			return true;
		}

		public void onDismiss (ListView listView, int[] reverseSortedPositions)
		{
			a (reverseSortedPositions);
		}
	}
}

