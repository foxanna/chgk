using ChGK.Core.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Email;

namespace ChGK.Core.ViewModels
{
    public class AboutViewModel : MenuItemViewModel
    {
        private readonly IAppInfoProvider _appInfoProvider;

        public AboutViewModel(IAppInfoProvider appInfoProvider)
        {
            _appInfoProvider = appInfoProvider;

            Title = StringResources.AboutApp;

            AdvLink = string.Format("<u>{0}</u>", StringResources.ClickOnAd);
            EmailDeveloperLink = string.Format("<u>{0}</u>", StringResources.EmailDeveloper);
            RateUs = string.Format("<u>{0}</u>", StringResources.RateApp);

            CopyrightUrl = "<a href=" + StringResources.LicenceAgreementUrl + ">" + StringResources.LicenceAgreement +
                           "</a>";
            SomeTitle = StringResources.QuestionsBase + " <a href=" + StringResources.DataBaseUrl + ">" +
                        StringResources.WhatWhenWhere + "</a>";
            Version = string.Format("{0} v{1}", appInfoProvider.AppName, appInfoProvider.AppVersion);
        }

        public string SomeTitle { get; private set; }

        public string CopyrightUrl { get; private set; }

        public string Version { get; private set; }

        public string AdvLink { get; private set; }

        public string RateUs { get; private set; }

        public string EmailDeveloperLink { get; private set; }

        public MvxCommand EmailDeveloperCommand
        {
            get
            {
                return new MvxCommand(() =>
                    Mvx.Resolve<IMvxComposeEmailTask>().ComposeEmail(StringResources.Email,
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        false));
            }
        }

        public MvxCommand RateAppCommand
        {
            get { return new MvxCommand(_appInfoProvider.RateAppOnMarket); }
        }
    }
}