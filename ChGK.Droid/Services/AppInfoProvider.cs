using Android.App;
using ChGK.Core.Services;

namespace ChGK.Droid.Services
{
    public class AppInfoProvider : IAppInfoProvider
    {
        public string AppVersion => Application.Context.PackageManager.GetPackageInfo(
            Application.Context.PackageName, 0)?.VersionName ?? string.Empty;

        public string AppName => Application.Context.PackageManager.GetApplicationLabel(
            Application.Context.PackageManager.GetApplicationInfo(Application.Context.PackageName, 0))
                                 ?? string.Empty;

        public string AppOnMarketLink => "market://details?id=" + Application.Context.PackageName;
    }
}