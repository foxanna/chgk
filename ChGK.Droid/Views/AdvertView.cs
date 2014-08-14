using Android.App;
//using Android.Gms.Ads;
using Android.Widget;
using Cirrious.MvvmCross.Droid.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChGK.Droid.Views
{
    [Activity(Label = "", 
        ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation
        | Android.Content.PM.ConfigChanges.SmallestScreenSize
        | Android.Content.PM.ConfigChanges.ScreenSize
        | Android.Content.PM.ConfigChanges.Keyboard 
        | Android.Content.PM.ConfigChanges.KeyboardHidden 
        | Android.Content.PM.ConfigChanges.ScreenLayout 
        | Android.Content.PM.ConfigChanges.UiMode)]
    public class AdvertView : MenuItemIndependentView
    {
        protected override int LayoutId
        {
            get
            {
                return Resource.Layout.AdvertView;
            }
        }

        //readonly List<AdView> _banners = new List<AdView>();

        //protected override void OnCreate(Android.OS.Bundle bundle)
        //{
        //    base.OnCreate(bundle);

        //    _banners.Add(FindViewById<AdView>(Resource.Id.adView1));
        //    _banners.Add(FindViewById<AdView>(Resource.Id.adView2));

        //    var adRequest = new AdRequest.Builder().Build();
        //    foreach (var banner in _banners)
        //    {
        //        banner.LoadAd(adRequest);
        //    }
        //}

        //protected override void OnResume()
        //{
        //    base.OnResume();

        //    foreach (var banner in _banners)
        //    {
        //        banner.Resume();
        //    }
        //}

        //protected override void OnPause()
        //{
        //    foreach (var banner in _banners)
        //    {
        //        banner.Pause();
        //    }
            
        //    base.OnPause();
        //}

        //protected override void OnDestroy()
        //{
        //    foreach (var banner in _banners)
        //    {
        //        banner.Destroy();
        //    }

        //    base.OnDestroy();
        //}
    }
}
