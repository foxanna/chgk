using Android.Content;
using ChGK.Core.Services;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.CrossCore.Plugins;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.Droid.Views;
using Cirrious.MvvmCross.ViewModels;
using ChGK.Droid.Helpers;
using ChGK.Droid.Services;
using Cirrious.MvvmCross.Plugins.Email;

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
			Mvx.RegisterSingleton<IAudioPlayerService> (new AudioPlayerService ());
			Mvx.RegisterSingleton<IAppInfoProvider> (new AppInfoProvider ());
			Mvx.RegisterSingleton<IDialogManager> (new DialogManager ());
			Mvx.RegisterSingleton<IMvxComposeEmailTask> (new MyComposeEmailTask ());

			base.InitializeApp (pluginManager);
		}
	}
}