using System;
using Cirrious.MvvmCross.Plugins.Email;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Droid.Platform;
using Android.Content;
using ChGK.Core;

namespace ChGK.Droid
{
	public class MyComposeEmailTask : IMvxComposeEmailTask
	{
		public void ComposeEmail (string to, string cc, string subject, string body, bool isHtml)
		{
            var email = new Intent(Intent.ActionSendto);
            email.SetData(Android.Net.Uri.FromParts("mailto", to, null));

            //var intent = Intent.CreateChooser(email, StringResources.EmailDeveloper);
			Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity.StartActivity(email);
		}
	}
}

