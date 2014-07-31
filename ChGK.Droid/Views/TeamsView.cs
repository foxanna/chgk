using Android.App;
using Android.Views;
using ChGK.Core.ViewModels;

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
}

