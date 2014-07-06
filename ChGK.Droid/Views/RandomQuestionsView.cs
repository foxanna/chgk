using Android.App;
using Android.OS;
using Cirrious.MvvmCross.Droid.Views;

namespace ChGK.Droid.Views
{
	[Activity (Label = "Случайные вопросы")]
	public class RandomQuestionsView : MvxActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.RandomQuestionsView);
		}
	}
}