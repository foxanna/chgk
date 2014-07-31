using Android.Views;
using Android.OS;

namespace ChGK.Droid.Views
{
	public class AboutView : MenuItemView
	{
		protected override int LayoutId {
			get {
				return Resource.Layout.AboutView;
			}
		}

		public override void OnViewCreated (View view, Bundle savedInstanceState)
		{
			base.OnViewCreated (view, savedInstanceState);

			HasOptionsMenu = false;
		}
	}
}

