using Android.App;
using Android.Gms.Analytics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChGK.Droid.Helpers
{
    public enum TrackerType {
        AppTracker,
        GlobalTracker,
        EcommerceTracker,
    }

    public static class GoogleAnalyticsManager
    {
        static Dictionary<TrackerType, Tracker> _trackers = new Dictionary<TrackerType,Tracker>();

        static Tracker GetTracker(TrackerType trackerType)
        {
            if (!_trackers.ContainsKey(trackerType))
            {
                var analytics = GoogleAnalytics.GetInstance(Application.Context);
                Tracker tracker = null;

                switch (trackerType)
                {
                    case TrackerType.AppTracker:
                        tracker = analytics.NewTracker("UA-54114842-2");
                        tracker.EnableExceptionReporting(true);
                        break;
                }

                _trackers.Add(trackerType, tracker);
            }

            return _trackers[trackerType];
        }

        public static Tracker GetTracker()
        {
            return GetTracker(TrackerType.AppTracker);
        }

        public static void SendScreen(string name)
        {
            var tracker = GoogleAnalyticsManager.GetTracker();
            tracker.SetScreenName(name);
            tracker.Send(new HitBuilders.AppViewBuilder().Build());
        }
    }
}
