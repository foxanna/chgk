using Android.App;
using Android.OS;
using Android.Views;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;
using Cirrious.MvvmCross.Droid.Views;
using Android.Widget;
using Cirrious.MvvmCross.Binding.BindingContext;
using ChGK.Core.ViewModels;
using System;

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

		public override void OnCreateOptionsMenu (IMenu menu, MenuInflater inflater)
		{
			base.OnCreateOptionsMenu (menu, inflater);
			inflater.Inflate (Resource.Menu.question, menu);

			var timeText = menu.FindItem (Resource.Id.time).ActionView as TextView;
			var startButton = new MenuItemWrapper (menu.FindItem (Resource.Id.start_timer));
			var stopButton = new MenuItemWrapper (menu.FindItem (Resource.Id.stop_timer));

			var bindingSet = this.CreateBindingSet<QuestionView, QuestionViewModel> ();
			bindingSet.Bind (timeText).For (n => n.Text).To (vm => vm.Time).WithConversion ("Timer");
			bindingSet.Bind (startButton).For (n => n.Visible).To (vm => vm.IsTimerStopped);
			bindingSet.Bind (stopButton).For (n => n.Visible).To (vm => vm.IsTimerStarted);
			bindingSet.Apply ();
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			switch (item.ItemId) {
			case Resource.Id.start_timer:
				(ViewModel as QuestionViewModel).StartTimerCommand.Execute (null);
				return true;
			case Resource.Id.stop_timer:
				(ViewModel as QuestionViewModel).StopTimerCommand.Execute (null);
				return true;
			default:
				return base.OnOptionsItemSelected (item);
			}
		}
	}

	public class MenuItemWrapper
	{
		IMenuItem item;

		public MenuItemWrapper (IMenuItem item)
		{
			if (item == null) {
				throw new ArgumentNullException ();
			}

			this.item = item;
		}

		public bool Visible {
			get {
				return item.IsVisible;
			}
			set {
				item.SetVisible (value);
			}
		}
	}
}

