using System;
using System.IO;
using Android.Content;
using ChGK.Core;
using ChGK.Core.Services;
using ChGK.Droid.Helpers;
using ChGK.Droid.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Platform;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform.Plugins;
using SQLite.Net;
using SQLite.Net.Platform.XamarinAndroid;

namespace ChGK.Droid
{
    public class Setup : MvxAndroidSetup
    {
        public Setup(Context applicationContext) : base(applicationContext)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            return new App();
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }

        protected override IMvxAndroidViewPresenter CreateViewPresenter()
        {
            return new ChGKPresenter();
        }

        protected override void InitializeApp(IMvxPluginManager pluginManager)
        {
            Mvx.RegisterSingleton<IDeviceConnectivityService>(new DeviceConnectivityService());
            Mvx.RegisterSingleton<IAudioPlayerService>(new AudioPlayerService());
            Mvx.RegisterSingleton<IAppInfoProvider>(new AppInfoProvider());
            Mvx.RegisterSingleton<IDialogManager>(new DialogManager());
            Mvx.RegisterSingleton<IGAService>(new GAService());

            Mvx.RegisterType(() => new SQLiteConnection(new SQLitePlatformAndroid(),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "chgk.db")));

            base.InitializeApp(pluginManager);
        }
    }
}