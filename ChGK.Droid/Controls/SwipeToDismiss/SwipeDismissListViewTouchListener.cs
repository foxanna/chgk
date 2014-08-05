using System;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using Android.Animation;
using Android.Graphics;
using Android.OS;
using System.Windows.Input;
using System.Linq;

namespace ChGK.Droid.Controls.SwipeToDismiss
{
	class PendingDismissData
	{
		public int Position { get; private set; }

		public View View { get; private set; }

		public PendingDismissData (int position, View view)
		{
			Position = position;
			View = view;
		}

		public void ResetViewPresentation (int originalHeight)
		{
			View.Alpha = 1;
			View.TranslationX = 0;
			var lp = View.LayoutParameters;
			lp.Height = originalHeight;
			View.LayoutParameters = lp;
		}
	}

	public class SwipeDismissListViewTouchListener : Java.Lang.Object, View.IOnTouchListener, AbsListView.IOnScrollListener
	{
		int mSlop;
		int mMinFlingVelocity;
		int mMaxFlingVelocity;

		public long AnimationTime { get; set; }

		// Fixed properties
		readonly ListView _listView;
		readonly ICommand _dismissCommand;

		int _viewWidth = 1;
		// 1 and not 0 to prevent dividing by zero

		// Transient properties
		readonly List<PendingDismissData> mPendingDismisses = new List<PendingDismissData> ();
		int mDismissAnimationRefCount = 0;
		float mDownX;
		float mDownY;
		bool isSwiping;
		int mSwipingSlop;
		VelocityTracker _velocityTracker;
		int _downPosition;
		View _downView;
		bool _paused;

		public SwipeDismissListViewTouchListener (ListView listView, ICommand dismissCommand)
		{
			ViewConfiguration vc = ViewConfiguration.Get (listView.Context);
			mSlop = vc.ScaledTouchSlop;
			mMinFlingVelocity = vc.ScaledMinimumFlingVelocity * 16;
			mMaxFlingVelocity = vc.ScaledMaximumFlingVelocity;

			AnimationTime = listView.Context.Resources.GetInteger (Android.Resource.Integer.ConfigShortAnimTime);

			_listView = listView;
			_dismissCommand = dismissCommand;
		}

		public bool Enabled {
			set {
				_paused = !value;
			}
		}

		void SaveWidth ()
		{
			if (_viewWidth < 2) {
				_viewWidth = _listView.Width;
			}
		}

		public bool OnTouch (View view, MotionEvent motionEvent)
		{
			SaveWidth ();

			switch (motionEvent.ActionMasked) {
			case MotionEventActions.Down:
				return OnDown (motionEvent);
			case MotionEventActions.Cancel:
				return OnCancel (motionEvent);
			case MotionEventActions.Up:
				return OnUp (motionEvent);
			case MotionEventActions.Move:
				return OnMove (motionEvent);
			default:
				return false;
			}
		}

		bool IsSwiping (float deltaX, float deltaY)
		{
			return Math.Abs (deltaX) > mSlop && Math.Abs (deltaY) < Math.Abs (deltaX) / 2;
		}

		bool FlingingSameDirectionAsDragging (float absVelocityX, float absVelocityY)
		{
			return mMinFlingVelocity <= absVelocityX && absVelocityX <= mMaxFlingVelocity && absVelocityY < absVelocityX && isSwiping;
		}

		protected virtual void AnimateDismissCancel (View view)
		{
			Animate (view, 0, 1, null);
		}

		protected virtual void AnimateDismissStart (View view, bool dismissRight, SwipeAnimatorListenerAdapter animatorListener)
		{
			Animate (view, dismissRight ? _viewWidth : -_viewWidth, 0, animatorListener);
		}

		protected void Animate (View view, int xTranslation, int alpha, SwipeAnimatorListenerAdapter animatorListener)
		{
			view.Animate ().TranslationX (xTranslation).Alpha (alpha).SetDuration (AnimationTime).SetListener (animatorListener);
		}

		#region TouchActionsHandlers

		bool OnDown (MotionEvent motionEvent)
		{
			if (_paused) {
				return false;
			}

			var rect = new Rect ();
			int childCount = _listView.ChildCount;

			var listViewCoords = new int[2];
			_listView.GetLocationOnScreen (listViewCoords);

			int x = (int)motionEvent.RawX - listViewCoords [0];
			int y = (int)motionEvent.RawY - listViewCoords [1];

			View child;

			for (int i = 0; i < childCount; i++) {
				child = _listView.GetChildAt (i);
				child.GetHitRect (rect);
				if (rect.Contains (x, y)) {
					_downView = child;
					break;
				}
			}

			if (_downView != null) {
				mDownX = motionEvent.RawX;
				mDownY = motionEvent.RawY;

				_downPosition = _listView.GetPositionForView (_downView);

				if (_dismissCommand.CanExecute (_downPosition)) {
					_velocityTracker = VelocityTracker.Obtain ();
					_velocityTracker.AddMovement (motionEvent);
				} else {
					_downView = null;
				}
			}

			return false;
		}

		bool OnCancel (MotionEvent motionEvent)
		{
			if (_velocityTracker == null) {
				return false;
			}

			if (_downView != null && isSwiping) {
				AnimateDismissCancel (_downView);
			}

			_velocityTracker.Recycle ();
			_velocityTracker = null;

			mDownX = 0;
			mDownY = 0;

			_downView = null;
			_downPosition = AdapterView.InvalidPosition;

			isSwiping = false;

			return false;
		}

		bool OnUp (MotionEvent motionEvent)
		{
			if (_velocityTracker == null) {
				return false;
			}

			float deltaX = motionEvent.RawX - mDownX;

			_velocityTracker.AddMovement (motionEvent);
			_velocityTracker.ComputeCurrentVelocity (1000);

			float velocityX = _velocityTracker.XVelocity;
			float absVelocityX = Math.Abs (velocityX);
			float absVelocityY = Math.Abs (_velocityTracker.YVelocity);

			bool dismiss = false;
			bool dismissRight = false;

			if (Math.Abs (deltaX) > _viewWidth / 2 && isSwiping) {
				dismiss = true;
				dismissRight = deltaX > 0;
			} else if (FlingingSameDirectionAsDragging (absVelocityX, absVelocityY)) {
				dismiss = (velocityX < 0) == (deltaX < 0);
				dismissRight = _velocityTracker.XVelocity > 0;
			}

			if (dismiss && _downPosition != AdapterView.InvalidPosition) {
				View downView = _downView; 
				int downPosition = _downPosition;

				++mDismissAnimationRefCount;

				AnimateDismissStart (_downView, dismissRight, new SwipeAnimatorListenerAdapter (_ => AnimateDismissEnd (downView, downPosition)));
			} else {
				AnimateDismissCancel (_downView);
			}

			_velocityTracker.Recycle ();
			_velocityTracker = null;

			mDownX = 0;
			mDownY = 0;

			_downView = null;
			InvalidateDownPosition ();

			isSwiping = false;

			return false;
		}

		bool OnMove (MotionEvent motionEvent)
		{
			if (_velocityTracker == null || _paused) {
				return false;
			}

			_velocityTracker.AddMovement (motionEvent);

			float deltaX = motionEvent.RawX - mDownX;
			float deltaY = motionEvent.RawY - mDownY;

			if (IsSwiping (deltaX, deltaY)) {
				isSwiping = true;

				mSwipingSlop = (deltaX > 0 ? mSlop : -mSlop);

				_listView.RequestDisallowInterceptTouchEvent (true);

				// Cancel ListView's touch (un-highlighting the item)
				var cancelEvent = MotionEvent.Obtain (motionEvent);
				cancelEvent.Action = MotionEventActions.Cancel | (MotionEventActions)((int)motionEvent.ActionIndex << (int)MotionEventActions.PointerIndexShift);

				_listView.OnTouchEvent (cancelEvent);
				cancelEvent.Recycle ();
			}

			if (isSwiping) {
				_downView.TranslationX = deltaX - mSwipingSlop;
				_downView.Alpha = Math.Max (0f, Math.Min (1f, 1f - 2f * Math.Abs (deltaX) / _viewWidth));

				return true;
			}

			return false;
		}

		#endregion

		public void OnScroll (AbsListView view, int firstVisibleItem, int visibleItemCount, int totalItemCount)
		{
		}

		public void OnScrollStateChanged (AbsListView view, ScrollState scrollState)
		{
			Enabled = scrollState != ScrollState.TouchScroll;
		}

		void AnimateDismissEnd (View dismissView, int dismissPosition)
		{
			ViewGroup.LayoutParams lp = dismissView.LayoutParameters;
			int originalHeight = dismissView.Height;

			var animator = ValueAnimator.OfInt (originalHeight, 1).SetDuration (AnimationTime) as ValueAnimator;

			animator.AddListener (new SwipeAnimatorListenerAdapter (_ => ExecuteDismiss (originalHeight)));

			animator.AddUpdateListener (new SwipeAnimatorUpdateListener (valueAnimator => {
				lp.Height = (int)valueAnimator.AnimatedValue;
				dismissView.LayoutParameters = lp;
			}));

			mPendingDismisses.Add (new PendingDismissData (dismissPosition, dismissView));
			animator.Start ();
		}

		void ExecuteDismiss (int originalHeight)
		{
			--mDismissAnimationRefCount;

			if (mDismissAnimationRefCount == 0) {
				CallDismissCommand ();
				InvalidateDownPosition ();
				ResetViewPresentation (originalHeight);
				SendCancelEvent ();
			}
		}

		void CallDismissCommand ()
		{
			var dismissPositions = mPendingDismisses.Select (data => data.Position).OrderByDescending (t => t).ToList ();

			_dismissCommand.Execute (dismissPositions.ToArray ());
		}

		void ResetViewPresentation (int originalHeight)
		{
			foreach (var pendingDismiss in mPendingDismisses) {
				pendingDismiss.ResetViewPresentation (originalHeight);
			}

			mPendingDismisses.Clear ();
		}

		void SendCancelEvent ()
		{
			long time = SystemClock.UptimeMillis ();
			var cancelEvent = MotionEvent.Obtain (time, time, MotionEventActions.Cancel, 0, 0, 0);
			_listView.DispatchTouchEvent (cancelEvent);
		}

		void InvalidateDownPosition ()
		{
			// Reset mDownPosition to avoid MotionEvent.ACTION_UP trying to start a dismiss 
			// animation with a stale position
			_downPosition = AdapterView.InvalidPosition;
		}

		public class SwipeAnimatorListenerAdapter : AnimatorListenerAdapter
		{
			readonly Action<Animator> _onAnimationEnd;

			public SwipeAnimatorListenerAdapter (Action<Animator> onAnimationEnd)
			{
				_onAnimationEnd = onAnimationEnd;
			}

			public override void OnAnimationEnd (Animator animation)
			{
				_onAnimationEnd (animation);
			}
		}

		public class SwipeAnimatorUpdateListener : Java.Lang.Object,  ValueAnimator.IAnimatorUpdateListener
		{
			readonly Action<ValueAnimator> _onAnimationUpdate;

			public SwipeAnimatorUpdateListener (Action<ValueAnimator> onAnimationUpdate)
			{
				_onAnimationUpdate = onAnimationUpdate;
			}

			public void OnAnimationUpdate (ValueAnimator animation)
			{
				_onAnimationUpdate (animation);
			}
		}
	}
}

