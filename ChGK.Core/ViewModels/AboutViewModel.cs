using System.Windows.Input;
using ChGK.Core.Services;
using ChGK.Core.Utils;
using MvvmCross.Plugins.Email;

namespace ChGK.Core.ViewModels
{
    public class AboutViewModel : MenuItemViewModel
    {
        private readonly IAppInfoProvider _appInfoProvider;
        private readonly IMvxComposeEmailTask _composeEmailTask;

        public AboutViewModel(IAppInfoProvider appInfoProvider,
            IMvxComposeEmailTask composeEmailTask)
        {
            _appInfoProvider = appInfoProvider;
            _composeEmailTask = composeEmailTask;

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
            _composeEmailTask.ComposeEmail(StringResources.Email,
                string.Empty, string.Empty, string.Empty));

        public ICommand RateAppCommand => new Command(_appInfoProvider.RateAppOnMarket);
    }
}