using Android.App;
using Android.OS;
using Cirrious.MvvmCross.Droid.Views;

//using ChGK.Core.ViewModels;

namespace ChGK.Droid.Views
{
	[Activity (Label = "some label")]
	public class QuestionView :  MvxActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.QuestionView);
		}
	}
}

