using System;
using Android.App;
using Android.Content;
using ChGK.Core;
using ChGK.Core.ViewModels;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;

namespace ChGK.Droid.Views
{
	public class EnterResultsView : MvxDialogFragment
	{
		public override Dialog OnCreateDialog (Android.OS.Bundle savedInstanceState)
		{
			var builder = new AlertDialog.Builder (Activity);
			var viewModel = ViewModel as EnterResultsViewModel;

			builder//.SetView (Activity.LayoutInflater.Inflate (Resource.Layout.item_result_team, null))
				
				.SetCancelable (true);
			builder.SetPositiveButton (StringResources.Save, new EventHandler<DialogClickEventArgs> ((s, e) => viewModel.SubmitResults ()))
				.SetNegativeButton (StringResources.Cancel, (EventHandler<DialogClickEventArgs>)null);


			return builder.Create ();
		}
	}
}

