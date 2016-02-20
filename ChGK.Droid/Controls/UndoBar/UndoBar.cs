using System;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace ChGK.Droid.Controls.UndoBar
{
    public class UndoBar
    {
        private readonly View _parentView;

        private readonly PopupWindow _popup;

        private CancellationTokenSource _cancellationTokenSource;

        private bool _undone;

        public UndoBar(Context context, string text, View parentView)
        {
            _parentView = parentView;

            var view = LayoutInflater.FromContext(context).Inflate(Resource.Layout.undo_bar, null);

            var undoButton = view.FindViewById(Resource.Id.undo_button);
            undoButton.Click += OnUndoClick;

            var titleTextView = view.FindViewById<TextView>(Resource.Id.undo_message);
            titleTextView.Text = text;

            _popup = new PopupWindow(view, ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent, false)
            {
                AnimationStyle = Resource.Style.popup_fade_animation
            };
        }

        public event EventHandler Undo;

        public event EventHandler Discard;

        private void Popup_DismissEvent(object sender, EventArgs e)
        {
            OnDiscard();
        }

        public void Show()
        {
            _popup.Width =
                (int) Math.Min(Application.Context.Resources.DisplayMetrics.Density*400, _parentView.Width*0.9f);
            _popup.DismissEvent += Popup_DismissEvent;
            _popup.ShowAtLocation(_parentView, GravityFlags.CenterHorizontal | GravityFlags.Bottom, 0, 60);

            SchedulePopupClose();
        }

        private void OnUndoClick(object sender, EventArgs e)
        {
            _undone = true;

            Hide();
        }

        private void OnDiscard()
        {
            _popup.DismissEvent -= Popup_DismissEvent;
            _cancellationTokenSource.Cancel();

            var handler = _undone ? Undo : Discard;
            handler?.Invoke(this, EventArgs.Empty);
        }

        private async void SchedulePopupClose()
        {
            _cancellationTokenSource = new CancellationTokenSource();

            try
            {
                var ui = TaskScheduler.FromCurrentSynchronizationContext();
                await Task.Delay(5000, _cancellationTokenSource.Token);
                await
                    Task.Factory.StartNew(OnPopupTimeOut, _cancellationTokenSource.Token, TaskCreationOptions.None, ui);
            }
            catch (OperationCanceledException)
            {
                MvvmCross.Platform.Mvx.Trace("Undobar: OperationCanceledException");
            }
        }

        private void OnPopupTimeOut()
        {
            if (_cancellationTokenSource.IsCancellationRequested || _undone)
                return;

            Hide();
        }

        public void Hide()
        {
            if (_popup.IsShowing)
            {
                _popup.Dismiss();
            }
        }
    }
}