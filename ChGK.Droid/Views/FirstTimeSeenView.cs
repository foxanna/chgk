using Android.App;
using Android.OS;
using Cirrious.MvvmCross.Droid.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChGK.Droid.Views
{
    [Activity(Label = "", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize,
        Theme = "@android:style/Theme.Translucent.NoTitleBar")]		
    public class FirstTimeSeenView : MvxActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.FirstTimeSeenView);
        }
    }
}
