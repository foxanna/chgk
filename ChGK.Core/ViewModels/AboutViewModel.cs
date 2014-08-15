using ChGK.Core.Services;
using Cirrious.MvvmCross.ViewModels;
using System.Threading.Tasks;

namespace ChGK.Core.ViewModels
{
	public class AboutViewModel : MenuItemViewModel
	{
		public AboutViewModel (IAppInfoProvider appInfoProvider)
		{
			Title = StringResources.AboutApp;

            AdvLink = string.Format("<u>{0}</u>", StringResources.ClickOnAd);
            HasAds = true;

			CopyrightUrl = "<a href=" + StringResources.LicenceAgreementUrl + ">" + StringResources.LicenceAgreement + "</a>";
            SomeTitle = StringResources.QuestionsBase + " <a href=" + StringResources.DataBaseUrl + ">" + StringResources.WhatWhenWhere + "</a>";
			Version = string.Format ("{0} v{1}", appInfoProvider.AppName, appInfoProvider.AppVersion);
		}

		public string SomeTitle { get; private set; }

		public string CopyrightUrl { get; private set; }

		public string Version { get; private set; }

        public string AdvLink { get; private set; }

		public override Task Refresh ()
		{
			throw new System.NotImplementedException ();
		}

        public bool HasAds { get; private set; }

        public MvxCommand OpenAdsCommand
        {
            get
            {
                return new MvxCommand(() => ShowViewModel<AdvertViewModel>());
            }
        }
	}
}

