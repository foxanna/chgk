using ChGK.Core.Services;

namespace ChGK.Droid.Services
{
	public class AppInfoProvider : IAppInfoProvider
	{
		public string AppVersion {
			get {
				return Android.App.Application.Context.PackageManager.GetPackageInfo (Android.App.Application.Context.PackageName, 0).VersionName;
			}
		}
	}
}

