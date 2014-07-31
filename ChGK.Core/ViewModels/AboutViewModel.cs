using ChGK.Core.Services;
using System.Threading.Tasks;

namespace ChGK.Core.ViewModels
{
	public class AboutViewModel : MenuItemViewModel
	{
		public AboutViewModel (IAppInfoProvider appInfoProvider)
		{
			Title = StringResources.AboutApp;

			CopyrightUrl = "<a href=" + StringResources.LicenceAgreementUrl + ">" + StringResources.LicenceAgreement + "</a>";

			SomeTitle = StringResources.QuestionsBase + " <a href=" + StringResources.DataBaseUrl + ">\"Что? Где? Когда?\"</a>";

			Version = string.Format ("{0} {1} {2}", StringResources.Version, appInfoProvider.AppName, appInfoProvider.AppVersion);
		}

		public string SomeTitle { get; private set; }

		public string CopyrightUrl { get; private set; }

		public string Version { get; private set; }

		public override Task Refresh ()
		{
			throw new System.NotImplementedException ();
		}
	}
}

