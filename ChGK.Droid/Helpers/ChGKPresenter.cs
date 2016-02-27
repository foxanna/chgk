using System;
using System.Collections.Generic;
using ChGK.Core.ViewModels;
using ChGK.Core.ViewModels.Search;
using ChGK.Droid.Views;
using ChGK.Droid.Views.Search;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.FullFragging.Fragments;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;
using MvxActivity = MvvmCross.Droid.FullFragging.Views.MvxActivity;

namespace ChGK.Droid.Helpers
{
    public class ChGKPresenter : MvxAndroidViewPresenter
    {
        private readonly Dictionary<Type, Action<MvxViewModelRequest>> _customPresenterOpenScenarios;

        public ChGKPresenter()
        {
            _customPresenterOpenScenarios = new Dictionary<Type, Action<MvxViewModelRequest>>
            {
                {
                    typeof (LastAddedTournamentsViewModel),
                    request => ReplaceFragment(new LastAddedTournamentsView(), request)
                },
                {
                    typeof (RandomQuestionsViewModel),
                    request => ReplaceFragment(new RandomQuestionsView(), request)
                },
                {
                    typeof (FavoriteTournamentsViewModel),
                    request => ReplaceFragment(new FavoriteTournamentsView(), request)
                },
                {
                    typeof (EnterResultsViewModel),
                    request => ShowDialog(new EnterResultsView(), request)
                },
                {
                    typeof (SearchParamsViewModel),
                    request => ReplaceFragment(new SearchParamsView(), request)
                }
            };
        }

        protected void PrepareFragment(IMvxFragmentView fragment, MvxViewModelRequest request)
        {
            var loaderService = Mvx.Resolve<IMvxViewModelLoader>();
            var viewModel = loaderService.LoadViewModel(request, null);

            fragment.ViewModel = viewModel;
        }

        protected void ReplaceFragment(MvxFragment fragment, MvxViewModelRequest request)
        {
            PrepareFragment(fragment, request);
            ((HomeView) Activity).ShowMenuItem(fragment);
        }

        protected void ShowDialog(MvxDialogFragment dialog, MvxViewModelRequest request)
        {
            PrepareFragment(dialog, request);
            dialog.Show((Activity as MvxActivity).FragmentManager, "tag");
        }

        public override void Show(MvxViewModelRequest request)
        {
            if (_customPresenterOpenScenarios.ContainsKey(request.ViewModelType))
            {
                _customPresenterOpenScenarios[request.ViewModelType](request);
            }
            else
            {
                base.Show(request);
            }
        }
    }
}