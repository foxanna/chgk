using Android.App;
using Android.Gms.Analytics;
using ChGK.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Java.Lang;

namespace ChGK.Droid.Services
{
	public class GAService : IGAService
	{
		Tracker _tracker;

		public GAService ()
		{
			var analytics = GoogleAnalytics.GetInstance (Application.Context);

			_tracker = analytics.NewTracker ("UA-54114842-2");
			_tracker.EnableExceptionReporting (true);

//			Thread.DefaultUncaughtExceptionHandler = new ExceptionReporter (
//				_tracker, Thread.DefaultUncaughtExceptionHandler, Application.Context);     
		}

		public void ReportScreenEnter (string name)
		{
			_tracker.SetScreenName (name);
			_tracker.Send (new HitBuilders.AppViewBuilder ().Build ());
		}

		public void ReportEvent (ChGK.Core.Services.GACategory category, ChGK.Core.Services.GAAction action, string label)
		{
			_tracker.Send (new HitBuilders.EventBuilder ().SetCategory (category.ToString ()).SetAction (action.ToString ()).SetLabel (label).Build ());
		}
	}
}
