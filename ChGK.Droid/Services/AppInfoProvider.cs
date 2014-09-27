using Android.Content;
using Android.Net;
using ChGK.Core.Services;

namespace ChGK.Droid.Services
{
	public class AppInfoProvider : IAppInfoProvider
	{
		public string AppVersion {
			get {
				var packageInfo = Android.App.Application.Context.PackageManager.GetPackageInfo (Android.App.Application.Context.PackageName, 0);
				return packageInfo != null ? packageInfo.VersionName + " (" + packageInfo.VersionCode + ")" : string.Empty;
			}
		}

		public string AppName {
			get {
				var appInfo = Android.App.Application.Context.PackageManager.GetApplicationInfo (Android.App.Application.Context.PackageName, 0);
				return appInfo != null ? Android.App.Application.Context.PackageManager.GetApplicationLabel (appInfo) : string.Empty;
			}
		}

		public void RateAppOnMarket ()
		{
			var intent = new Intent (Intent.ActionView);
			intent.SetData (Uri.Parse ("market://details?id=" + Android.App.Application.Context.PackageName));
			Android.App.Application.Context.StartActivity (intent.SetFlags (ActivityFlags.NewTask));
		}
	}
}

