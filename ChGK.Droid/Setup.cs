using Android.Content;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Droid.Views;
using Cirrious.CrossCore.Plugins;
using Cirrious.CrossCore;
using ChGK.Core.Services;
using ChGK.Droid.Services;

namespace ChGK.Droid
{
	public class Setup : MvxAndroidSetup
	{
		public Setup (Context applicationContext) : base (applicationContext)
		{
		}

		protected override IMvxApplication CreateApp ()
		{
			return new Core.App ();
		}

		protected override IMvxTrace CreateDebugTrace ()
		{
			return new DebugTrace ();
		}

		protected override IMvxAndroidViewPresenter CreateViewPresenter ()
		{
			return new ChGKPresenter ();
		}

		protected override void InitializeApp (IMvxPluginManager pluginManager)
		{
			Mvx.RegisterSingleton<IDeviceConnectivityService> (new DeviceConnectivityService ());

			base.InitializeApp (pluginManager);
		}
	}
}