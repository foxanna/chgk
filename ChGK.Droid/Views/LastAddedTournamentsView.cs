
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Cirrious.MvvmCross.Droid.Views;

namespace ChGK.Droid.Views
{
	[Activity (Label = "LastAddedTournamentsView")]			
	public class LastAddedTournamentsView : MvxActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			//SetContentView (Resource.Layout.LastAddedTournamentsView);
			//SetContentView (Resource.Layout.LastTournaments);
		}
	}
}

