using Android.App;
using Android.Gms.Ads;
using Android.OS;
using Android.Widget;
using ChGK.Core.ViewModels;
using Cirrious.MvvmCross.Droid.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

            var lParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
            lParams.SetMargins(0, 0, 0, Resources.GetDimensionPixelSize(Resource.Dimension.default_text_padding));

            foreach (var adId in (ViewModel as AdvertViewModel).AsIds)
            {
                var adView = new AdView(this)
                {
                    AdSize = AdSize.Banner,
                    AdUnitId = adId,                    
                };

                adContainer.AddView(adView, lParams);

                _banners.Add(adView);
            }

            var adRequest = new AdRequest.Builder().Build();
            foreach (var banner in _banners)
            {
                banner.LoadAd(adRequest);
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
