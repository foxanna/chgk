using System;
using Android.Content;
using ChGK.Core;
using ChGK.Droid.Controls;
using MvvmCross.Plugins.Email;
using MvvmCross.Platform.Droid.Platform;

namespace ChGK.Droid.Services
{
	public class MyComposeEmailTask : IMvxComposeEmailTask
	{
        public void ComposeEmail(string to, string cc = null, string subject = null, string body = null, bool isHtml = false, string dialogTitle = null)
        {
            var email = new Intent(Intent.ActionSendto);
            email.SetData(Android.Net.Uri.FromParts("mailto", to, null));

            //var intent = Intent.CreateChooser(email, StringResources.EmailDeveloper);
            MvvmCross.Platform.Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity.StartActivity(email);
        }
    }
}

