using Android.App;
using Android.Gms.Analytics;
using ChGK.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChGK.Droid.Helpers
{
    public class GAService : IGAService
    {
        enum TrackerType
        {
            AppTracker,
            GlobalTracker,
            EcommerceTracker,
        }

        Dictionary<TrackerType, Tracker> _trackers = new Dictionary<TrackerType,Tracker>();

        Tracker GetTracker(TrackerType trackerType)
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

        Tracker GetTracker()
        {
            return GetTracker(TrackerType.AppTracker);
        }

        public void ReportScreenEnter(string name)
        {
            var tracker = GetTracker();
            tracker.SetScreenName(name);
            tracker.Send(new HitBuilders.AppViewBuilder().Build());
        }

        public void ReportEvent(ChGK.Core.Services.GACategory category, ChGK.Core.Services.GAAction action, string label)
        {
            var tracker = GetTracker();
            tracker.Send(new HitBuilders.EventBuilder().SetCategory(category.ToString()).SetAction(action.ToString()).SetLabel(label).Build());
        }
    }
}
