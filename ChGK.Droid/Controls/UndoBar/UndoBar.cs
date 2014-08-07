using System;
using Android.Widget;
using Android.Views;
using Android.Content;
using Android.App;
using System.Threading.Tasks;
using System.Threading;

namespace ChGK.Droid.Controls.UndoBar
{
	public class UndoEventArgs : EventArgs
	{
		public int EventID { get; private set; }

		public UndoEventArgs (int eventID)
		{
			EventID = eventID;
		}
	}


	public class UndoBar
	{
		public event EventHandler<UndoEventArgs> Undo;

		public event EventHandler<UndoEventArgs> Discard;

		readonly int _id;

		readonly PopupWindow _popup;

		readonly View _parentView;

		readonly object lockObject = new object ();

		CancellationTokenSource _cancellationTokenSource;

		public UndoBar (Context context, int id, string text, View parentView)
		{
			_id = id;
			_parentView = parentView;

			var view = (context.GetSystemService (Context.LayoutInflaterService) as LayoutInflater).Inflate (Resource.Layout.undo_bar, null);

			var undoButton = view.FindViewById (Resource.Id.undo_button);
			undoButton.Click += OnUndoClick;

			var titleTextView = view.FindViewById<TextView> (Resource.Id.undo_message);
			titleTextView.Text = text;

			_popup = new PopupWindow (view, ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent, false);
			_popup.DismissEvent += (sender, e) => OnDiscard ();
		}

		public void Show ()
		{
			_popup.Width = (int)Math.Min (Application.Context.Resources.DisplayMetrics.Density * 400, _parentView.Width * 0.9f);
			_popup.ShowAtLocation (_parentView, GravityFlags.CenterHorizontal | GravityFlags.Bottom, 0, 60);

			_cancellationTokenSource = new CancellationTokenSource ();

			var ui = TaskScheduler.FromCurrentSynchronizationContext ();
			Task.Delay (5000, _cancellationTokenSource.Token)
				.ContinueWith (OnPopupTimeOut, _cancellationTokenSource.Token, TaskContinuationOptions.OnlyOnRanToCompletion, ui);
		}

		void OnUndoClick (object sender, EventArgs e)
		{
			_undone = true;

			lock (lockObject) {
				_cancellationTokenSource.Cancel ();

				if (_popup.IsShowing) {
					_popup.Dismiss ();
				}
			
				var handler = Undo;
				if (handler != null) {
					handler (this, new UndoEventArgs (_id));
				}
			}
		}

		void OnDiscard ()
		{
			_cancellationTokenSource.Cancel ();

			if (_undone) {
				return;
			}

			var handler = Discard;
			if (handler != null) {
				handler (this, new UndoEventArgs (_id));
			}
		}

		bool _undone;

		void OnPopupTimeOut (Task t)
		{
			if (_cancellationTokenSource.IsCancellationRequested || _undone)
				return;

			lock (lockObject) {
				Hide ();
			}
		}

		public void Hide ()
		{
			if (_popup.IsShowing) {
				_popup.Dismiss ();
			}
		}
	}
}

