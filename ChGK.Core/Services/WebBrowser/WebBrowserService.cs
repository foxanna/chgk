using Plugin.Share;

namespace ChGK.Core.Services.WebBrowser
{
    public class WebBrowserService : IWebBrowserService
    {
        public async void OpenInWebBrowser(string link)
        {
            await CrossShare.Current.OpenBrowser(link);
        }
    }
}