using System;
using Android.App;
using Android.Content;
using Android.OS;
using ChGK.Core;
using ChGK.Core.Services;
using ChGK.Core.Utils;
using ChGK.Core.ViewModels;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.FullFragging.Fragments;
using MvvmCross.Platform;

namespace ChGK.Droid.Views
{
    public class EnterResultsView : MvxDialogFragment
    {
        public EnterResultsView()
        {
            RetainInstance = true;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            var viewModel = ViewModel as EnterResultsViewModel;

            BindingContext = new MvxAndroidBindingContext(Activity,
                new MvxSimpleLayoutInflaterHolder(Activity.LayoutInflater));
            DataContext = viewModel;

            var builder = new AlertDialog.Builder(Activity);
            var view = this.BindingInflate(Resource.Layout.EnterResultsView, null);

            builder.SetView(view);
            builder.SetCancelable(true);
            builder.SetPositiveButton(StringResources.Save, (s, e) => viewModel?.SubmitResults())
                .SetNegativeButton(StringResources.Cancel, (EventHandler<DialogClickEventArgs>) null);

            return builder.Create();
        }

        public override void OnStart()
        {
            base.OnStart();

            Mvx.Resolve<IGAService>().ReportScreenEnter(GetType().FullName);
        }

        public override void OnDismiss(IDialogInterface dialog)
        {
            (ViewModel as IViewLifecycle)?.OnViewDestroying();

            DataContext = null;
            BindingContext = null;

            base.OnDismiss(dialog);
        }
    }
}