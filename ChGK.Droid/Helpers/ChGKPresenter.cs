using System;
using System.Collections.Generic;
using ChGK.Core.ViewModels;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;
using Cirrious.MvvmCross.Droid.Views;
using Cirrious.MvvmCross.ViewModels;
using ChGK.Droid.Views;
using Cirrious.MvvmCross.Droid.Fragging;

namespace ChGK.Droid.Helpers
{
	public class ChGKPresenter : MvxAndroidViewPresenter
	{
		readonly Dictionary<Type, Action<MvxViewModelRequest>> _customPresenterOpenScenarios;

		public ChGKPresenter ()
		{
			_customPresenterOpenScenarios = new Dictionary<Type, Action<MvxViewModelRequest>> { {
					typeof(LastAddedTournamentsViewModel),
					request => ReplaceFragment (new LastAddedTournamentsView (), request)
				}, {
					typeof(RandomQuestionsViewModel),
					request => ReplaceFragment (new RandomQuestionsView (), request)
				}, {
					typeof(EnterResultsViewModel),
					request => ShowDialog (new EnterResultsView (), request)
				},
			};
		}

		protected void PrepareFragment (IMvxFragmentView fragment, MvxViewModelRequest request)
		{
			var loaderService = Mvx.Resolve<IMvxViewModelLoader> ();
			var viewModel = loaderService.LoadViewModel (request, null);

			fragment.ViewModel = viewModel;
		}

		protected void ReplaceFragment (MvxFragment fragment, MvxViewModelRequest request)
		{
			PrepareFragment (fragment, request);
			((HomeView)Activity).ShowMenuItem (fragment);
		}

		protected void ShowDialog (MvxDialogFragment dialog, MvxViewModelRequest request)
		{
			PrepareFragment (dialog, request);
			dialog.Show ((Activity as MvxFragmentActivity).SupportFragmentManager, "tag");
		}

		public override void Show (MvxViewModelRequest request)
		{
			if (_customPresenterOpenScenarios.ContainsKey (request.ViewModelType)) {
				_customPresenterOpenScenarios [request.ViewModelType] (request);
			} else {
				base.Show (request);
			}
		}
	}
}

