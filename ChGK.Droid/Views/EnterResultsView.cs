using System;
using Android.App;
using Android.Content;
using Android.OS;
using ChGK.Core;
using ChGK.Core.ViewModels;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;

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
	}
}

