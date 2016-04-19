using Android.OS;
using Android.Views;
using Android.Widget;
using ChGK.Core.ViewModels;
using ChGK.Droid.Helpers;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.FullFragging.Fragments;

namespace ChGK.Droid.Views
{
    public class QuestionView : MvxFragment
    {
        private MenuItemWrapper _startButton, _stopButton, _showInWebButton;
        private TextView _timeText;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            SetHasOptionsMenu(true);

            return this.BindingInflate(Resource.Layout.QuestionView, null);
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            base.OnCreateOptionsMenu(menu, inflater);
            inflater.Inflate(Resource.Menu.question, menu);

            _timeText = menu.FindItem(Resource.Id.time).ActionView as TextView;
            _startButton = new MenuItemWrapper(menu.FindItem(Resource.Id.start_timer));
            _stopButton = new MenuItemWrapper(menu.FindItem(Resource.Id.stop_timer));
            _showInWebButton = new MenuItemWrapper(menu.FindItem(Resource.Id.show_in_web));

            var bindingSet = this.CreateBindingSet<QuestionView, QuestionViewModel>();
            bindingSet.Bind(_timeText).For(n => n.Text).To(vm => vm.Time).WithConversion("Timer");
            bindingSet.Bind(_startButton).For(n => n.Visible).To(vm => vm.IsTimerStopped);
            bindingSet.Bind(_stopButton).For(n => n.Visible).To(vm => vm.IsTimerStarted);
            bindingSet.Bind(_showInWebButton).For(n => n.Visible).To(vm => vm.HasWebUrl);
            bindingSet.Apply();
        }

        public override void OnDestroyOptionsMenu()
        {
            this.ClearBindings(_timeText);
            this.ClearBindings(_startButton);
            this.ClearBindings(_stopButton);
            this.ClearBindings(_showInWebButton);

            base.OnDestroyOptionsMenu();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.start_timer:
                    (ViewModel as QuestionViewModel)?.StartTimer();
                    return true;
                case Resource.Id.stop_timer:
                    (ViewModel as QuestionViewModel)?.PauseTimer();
                    return true;
                case Resource.Id.show_in_web:
                    (ViewModel as QuestionViewModel)?.OpenQuestionInWebCommand.Execute(null);
                    Toast.MakeText(Context, Resource.String.spoillers, ToastLength.Long).Show();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}