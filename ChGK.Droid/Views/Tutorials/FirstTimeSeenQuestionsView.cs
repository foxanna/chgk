// ReSharper disable once RedundantUsingDirective

using Android.App;
using Android.OS;
using MvvmCross.Droid.Views;

namespace ChGK.Droid.Views.Tutorials
{
    [Activity(Label = "",
        ConfigurationChanges =
            Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize,
        Theme = "@android:style/Theme.Translucent.NoTitleBar")]
    public class FirstTimeSeenQuestionsView : MvxActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.FirstTimeSeenQuestionsView);
        }
    }
}