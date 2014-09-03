using System;
using Android.App;
using Android.Content;
using Android.OS;
using ChGK.Core;
using ChGK.Core.ViewModels;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;
using ChGK.Core.Utils;
using ChGK.Droid.Helpers;

namespace ChGK.Droid.Views
{
	public class EnterResultsView : MvxDialogFragment
	{
		public EnterResultsView ()
		{
			RetainInstance = true;
		}

		public override Dialog OnCreateDialog (Bundle savedInstanceState)
		{
			var viewModel = ViewModel as EnterResultsViewModel;

			BindingContext = new MvxAndroidBindingContext (Activity, new MvxSimpleLayoutInflater (Activity.LayoutInflater));
			DataContext = viewModel;

			var builder = new AlertDialog.Builder (Activity);
			var view = this.BindingInflate (Resource.Layout.EnterResultsView, null);

			builder.SetView (view);
			builder.SetCancelable (true);
			builder.SetPositiveButton (StringResources.Save, new EventHandler<DialogClickEventArgs> ((s, e) => viewModel.SubmitResults ()))
					.SetNegativeButton (StringResources.Cancel, (EventHandler<DialogClickEventArgs>)null);
            
			return builder.Create ();
		}

        public override void OnStart()
        {
            base.OnStart();

            GoogleAnalyticsManager.SendScreen(this.GetType().FullName);
        }            

        public override void OnDismiss(IDialogInterface dialog)
        {
            (ViewModel as IViewLifecycle).OnViewDestroying();

            DataContext = null;
            BindingContext = null;
            
            base.OnDismiss(dialog);
        }
	}
}

