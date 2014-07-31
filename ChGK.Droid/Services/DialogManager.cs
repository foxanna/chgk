using System;
using System.Windows.Input;
using Android.App;
using Android.Content;
using Android.Widget;
using ChGK.Core;
using ChGK.Core.Services;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Droid.Platform;

namespace ChGK.Droid.Services
{
	public class DialogManager : IDialogManager
	{
		public void ShowDialog (DialogType type, ICommand yesAction)
		{
			switch (type) {
			case DialogType.AddTeamDialog:
				AddTeamDialog (yesAction).Show ();
				break;
			default:
				throw new ArgumentException ();
			}
		}

		static Dialog AddTeamDialog (ICommand yesAction)
		{
			var builder = new AlertDialog.Builder (Mvx.Resolve<IMvxAndroidCurrentTopActivity> ().Activity);

			var editText = new EditText (Application.Context);

			builder.SetView (editText)
				.SetCancelable (true)
				.SetTitle (StringResources.TeamName);
			builder.SetPositiveButton (StringResources.Add, new EventHandler<DialogClickEventArgs> ((s, e) => {
				if (yesAction.CanExecute (editText.Text)) {
					yesAction.Execute (editText.Text);
				}
			})).SetNegativeButton (StringResources.Cancel, (EventHandler<DialogClickEventArgs>)null);

			
			return builder.Create ();
		}
	}
}

