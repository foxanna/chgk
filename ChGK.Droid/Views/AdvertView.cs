using Android.App;
using Android.Gms.Ads;
using Android.OS;
using Android.Widget;
using System.Collections.Generic;

namespace ChGK.Droid.Views
{
    [Activity(Label = "")]
    public class AdvertView : MenuItemIndependentView
    {
        protected override int LayoutId
        {
            get
            {
                return Resource.Layout.AdvertView;
            }
        }

        readonly List<AdView> _banners = new List<AdView>();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            
            var adContainer = FindViewById<LinearLayout>(Resource.Id.ad_container);
            
            foreach (var adId in Resources.GetStringArray(Resource.Array.ads_ids))
            {
                var adView = new AdView(this)
                {
                    AdSize = AdSize.SmartBanner,
                    AdUnitId = adId,                    
                };

                adContainer.AddView(adView);

                _banners.Add(adView);
            }

            foreach (var banner in _banners)
            {
                var adRequestBuilder = new AdRequest.Builder();
                adRequestBuilder.AddTestDevice(AdRequest.DeviceIdEmulator).AddTestDevice("10FC024FD754F8EA6E8B5E391CDCBE92");
                banner.LoadAd(adRequestBuilder.Build());
            }
        }

        protected override void OnResume()
        {
            base.OnResume();

            foreach (var banner in _banners)
            {
                banner.Resume();
            }
        }

        protected override void OnPause()
        {
            foreach (var banner in _banners)
            {
                banner.Pause();
            }

            base.OnPause();
        }

        protected override void OnDestroy()
        {
            foreach (var banner in _banners)
            {
                banner.Destroy();
            }

            base.OnDestroy();
        }
    }
}
