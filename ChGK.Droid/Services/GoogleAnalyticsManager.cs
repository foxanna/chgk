using Android.App;
using Android.Gms.Analytics;
using ChGK.Core.Services;

namespace ChGK.Droid.Services
{
    public class GAService : IGAService
    {
        private readonly Tracker _tracker;

        public GAService()
        {
            var analytics = GoogleAnalytics.GetInstance(Application.Context);

#if (DEBUG)
            analytics.SetDryRun(true);
#endif

            _tracker = analytics.NewTracker("UA-54114842-2");
            _tracker.EnableExceptionReporting(true);
        }

        public void ReportScreenEnter(string name)
        {
            _tracker.SetScreenName(name);
            _tracker.Send(new HitBuilders.AppViewBuilder().Build());
        }

        public void ReportEvent(GACategory category, GAAction action, string label)
        {
            _tracker.Send(
                new HitBuilders.EventBuilder().SetCategory(category.ToString())
                    .SetAction(action.ToString())
                    .SetLabel(label)
                    .Build());
        }
    }
}