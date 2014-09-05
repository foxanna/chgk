using Android.OS;
using Android.Views;
using Android.Widget;
using ChGK.Core.ViewModels;
using ChGK.Droid.Helpers;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;

namespace ChGK.Droid.Views
{
	public class QuestionView : MvxFragment
	{
		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			base.OnCreateView (inflater, container, savedInstanceState);

			HasOptionsMenu = true;

			return this.BindingInflate (Resource.Layout.QuestionView, null);
		}

        MenuItemWrapper startButton, stopButton;
        TextView timeText;

		public override void OnCreateOptionsMenu (IMenu menu, MenuInflater inflater)
		{
			base.OnCreateOptionsMenu (menu, inflater);
			inflater.Inflate (Resource.Menu.question, menu);

			timeText = menu.FindItem (Resource.Id.time).ActionView as TextView;
			startButton = new MenuItemWrapper (menu.FindItem (Resource.Id.start_timer));
			stopButton = new MenuItemWrapper (menu.FindItem (Resource.Id.stop_timer));

			var bindingSet = this.CreateBindingSet<QuestionView, QuestionViewModel> ();
			bindingSet.Bind (timeText).For (n => n.Text).To (vm => vm.Time).WithConversion ("Timer");
			bindingSet.Bind (startButton).For (n => n.Visible).To (vm => vm.IsTimerStopped);
			bindingSet.Bind (stopButton).For (n => n.Visible).To (vm => vm.IsTimerStarted);
			bindingSet.Apply ();
		}

        public override void OnDestroyOptionsMenu()
        {
            BindingContext.ClearAllBindings();

            base.OnDestroyOptionsMenu();
        }

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			switch (item.ItemId) {
			case Resource.Id.start_timer:
				(ViewModel as QuestionViewModel).StartTimer ();
				return true;
			case Resource.Id.stop_timer:
				(ViewModel as QuestionViewModel).PauseTimer ();
				return true;			
			default:
				return base.OnOptionsItemSelected (item);
			}
		}
	}	
}

