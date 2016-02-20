using System.Windows.Input;
using ChGK.Core.Services;
using ChGK.Core.Services.Email;
using ChGK.Core.Services.WebBrowser;
using ChGK.Core.Utils;

namespace ChGK.Core.ViewModels
{
    public class AboutViewModel : MenuItemViewModel
    {
        private readonly IAppInfoProvider _appInfoProvider;
        private readonly IWebBrowserService _browserService;
        private readonly IEmailsService _emailsService;

        public AboutViewModel(IAppInfoProvider appInfoProvider,
            IEmailsService emailsService,
            IWebBrowserService browserService)
        {
            _appInfoProvider = appInfoProvider;
            _emailsService = emailsService;
            _browserService = browserService;

            Title = StringResources.AboutApp;

            AdvLink = $"<u>{StringResources.ClickOnAd}</u>";
            EmailDeveloperLink = $"<u>{StringResources.EmailDeveloper}</u>";
            RateUs = $"<u>{StringResources.RateApp}</u>";

            CopyrightUrl = "<a href=" + StringResources.LicenceAgreementUrl + ">" + StringResources.LicenceAgreement +
                           "</a>";
            SomeTitle = StringResources.QuestionsBase + " <a href=" + StringResources.DataBaseUrl + ">" +
                        StringResources.WhatWhenWhere + "</a>";
            Version = $"{appInfoProvider.AppName} v{appInfoProvider.AppVersion}";
        }

        public string SomeTitle { get; private set; }

        public string CopyrightUrl { get; private set; }

        public string Version { get; private set; }

        public string AdvLink { get; private set; }

        public string RateUs { get; private set; }

        public string EmailDeveloperLink { get; private set; }

        public ICommand EmailDeveloperCommand => new Command(() =>
            _emailsService.SendEmail(StringResources.Email, string.Empty, string.Empty));

        public ICommand RateAppCommand => new Command(() =>
            _browserService.OpenInWebBrowser(_appInfoProvider.AppOnMarketLink));
    }
}