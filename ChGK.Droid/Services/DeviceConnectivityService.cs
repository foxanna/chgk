using Android.Content;
using Android.Net;
using ChGK.Core.Services;

namespace ChGK.Droid.Services
{
	public class DeviceConnectivityService: IDeviceConnectivityService
	{
		public bool HasInternet ()
		{
			var connectivityManager = (ConnectivityManager)Android.App.Application.Context.GetSystemService (Context.ConnectivityService);
			var activeConnection = connectivityManager.ActiveNetworkInfo;

			return activeConnection != null && activeConnection.IsConnected;
		}
	}
}

