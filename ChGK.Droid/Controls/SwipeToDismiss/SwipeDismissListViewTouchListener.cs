using System;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using Android.Animation;
using Android.Graphics;
using Android.OS;
using System.Windows.Input;

namespace ChGK.Droid.Controls.SwipeToDismiss
{
	class PendingDismissData : IComparable
	{
		public int Position { get; private set; }

		public View View { get; private set; }

		public PendingDismissData (int position, View view)
		{
			Position = position;
			View = view;
		}

		public int CompareTo (object obj)
		{
			if (obj == null)
				return 1;

			var other = obj as PendingDismissData;
			if (other != null)
				return other.Position - this.Position;

			throw new ArgumentException ("Object is not a PendingDismissData");
		}
	}

	public class SwipeDismissListViewTouchListener : Java.Lang.Object, View.IOnTouchListener, AbsListView.IOnScrollListener
	{
		int mSlop;
		int mMinFlingVelocity;
		int mMaxFlingVelocity;
		long mAnimationTime;

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
		bool mSwiping;
		int mSwipingSlop;
		VelocityTracker mVelocityTracker;
		int _downPosition;
		View _downView;
		bool _paused;

		public SwipeDismissListViewTouchListener (ListView listView, ICommand dismissCommand)
		{
			ViewConfiguration vc = ViewConfiguration.Get (listView.Context);
			mSlop = vc.ScaledTouchSlop;
			mMinFlingVelocity = vc.ScaledMinimumFlingVelocity * 16;
			mMaxFlingVelocity = vc.ScaledMaximumFlingVelocity;
			mAnimationTime = listView.Context.Resources.GetInteger (Android.Resource.Integer.ConfigShortAnimTime);

			_listView = listView;
			_dismissCommand = dismissCommand;
		}

		public bool Enabled {
			set {
				_paused = !value;
			}
		}

		public bool OnTouch (View view, MotionEvent motionEvent)
		{
			if (_viewWidth < 2) {
				_viewWidth = _listView.Width;
			}

			switch (motionEvent.ActionMasked) {
			case MotionEventActions.Down:
				{
					if (_paused) {
						return false;
					}

					Rect rect = new Rect ();
					int childCount = _listView.ChildCount;
					int[] listViewCoords = new int[2];
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
							mVelocityTracker = VelocityTracker.Obtain ();
							mVelocityTracker.AddMovement (motionEvent);
						} else {
							_downView = null;
						}
					}
					return false;
				}

			case MotionEventActions.Cancel:
				{
					if (mVelocityTracker == null) {
						break;
					}

					if (_downView != null && mSwiping) {
						// cancel
						_downView.Animate ()
							.TranslationX (0)
							.Alpha (1)
							.SetDuration (mAnimationTime)
							.SetListener (null);
					}
					mVelocityTracker.Recycle ();
					mVelocityTracker = null;
					mDownX = 0;
					mDownY = 0;
					_downView = null;
					_downPosition = AdapterView.InvalidPosition;
					mSwiping = false;
					break;
				}

			case MotionEventActions.Up:
				{
					if (mVelocityTracker == null) {
						break;
					}

					float deltaX = motionEvent.RawX - mDownX;
					mVelocityTracker.AddMovement (motionEvent);
					mVelocityTracker.ComputeCurrentVelocity (1000);
					float velocityX = mVelocityTracker.XVelocity;
					float absVelocityX = Math.Abs (velocityX);
					float absVelocityY = Math.Abs (mVelocityTracker.YVelocity);
					bool dismiss = false;
					bool dismissRight = false;
					if (Math.Abs (deltaX) > _viewWidth / 2 && mSwiping) {
						dismiss = true;
						dismissRight = deltaX > 0;
					} else if (mMinFlingVelocity <= absVelocityX && absVelocityX <= mMaxFlingVelocity
					           && absVelocityY < absVelocityX && mSwiping) {
						// dismiss only if flinging in the same direction as dragging
						dismiss = (velocityX < 0) == (deltaX < 0);
						dismissRight = mVelocityTracker.XVelocity > 0;
					}
					if (dismiss && _downPosition != AdapterView.InvalidPosition) {
						// dismiss
						View downView = _downView; // mDownView gets null'd before animation ends
						int downPosition = _downPosition;
						++mDismissAnimationRefCount;
						_downView.Animate ()
							.TranslationX (dismissRight ? _viewWidth : -_viewWidth)
							.Alpha (0)
							.SetDuration (mAnimationTime)
							.SetListener (new SwipeAnimatorListenerAdapter (_ => performDismiss (downView, downPosition)));
					} else {
						// cancel
						_downView.Animate ()
							.TranslationX (0)
							.Alpha (1)
							.SetDuration (mAnimationTime)
							.SetListener (null);
					}
					mVelocityTracker.Recycle ();
					mVelocityTracker = null;
					mDownX = 0;
					mDownY = 0;
					_downView = null;
					_downPosition = AdapterView.InvalidPosition;
					mSwiping = false;
					break;
				}

			case MotionEventActions.Move:
				{
					if (mVelocityTracker == null || _paused) {
						break;
					}

					mVelocityTracker.AddMovement (motionEvent);
					float deltaX = motionEvent.RawX - mDownX;
					float deltaY = motionEvent.RawY - mDownY;
					if (Math.Abs (deltaX) > mSlop && Math.Abs (deltaY) < Math.Abs (deltaX) / 2) {
						mSwiping = true;
						mSwipingSlop = (deltaX > 0 ? mSlop : -mSlop);
						_listView.RequestDisallowInterceptTouchEvent (true);

						// Cancel ListView's touch (un-highlighting the item)
						var cancelEvent = MotionEvent.Obtain (motionEvent);
						cancelEvent.Action = MotionEventActions.Cancel |
						(MotionEventActions)((int)motionEvent.ActionIndex << (int)MotionEventActions.PointerIndexShift);
						_listView.OnTouchEvent (cancelEvent);
						cancelEvent.Recycle ();
					}

					if (mSwiping) {
						_downView.TranslationX = deltaX - mSwipingSlop;
						_downView.Alpha = Math.Max (0f, Math.Min (1f, 1f - 2f * Math.Abs (deltaX) / _viewWidth));
						return true;
					}
					break;
				}
			}
			return false;
		}

		public void OnScroll (AbsListView view, int firstVisibleItem, int visibleItemCount, int totalItemCount)
		{
		}

		public void OnScrollStateChanged (AbsListView view, ScrollState scrollState)
		{
			Enabled = scrollState != ScrollState.TouchScroll;
		}

		void performDismiss (View dismissView, int dismissPosition)
		{
			ViewGroup.LayoutParams lp = dismissView.LayoutParameters;
			int originalHeight = dismissView.Height;

			ValueAnimator animator = ValueAnimator.OfInt (originalHeight, 1).SetDuration (mAnimationTime) as ValueAnimator;

			animator.AddListener (new SwipeAnimatorListenerAdapter (_ => onanimationendA (originalHeight)));

			animator.AddUpdateListener (new SwipeAnimatorUpdateListener ((valueAnimator) => {
				lp.Height = (int)valueAnimator.AnimatedValue;
				dismissView.LayoutParameters = lp;
			}));

			mPendingDismisses.Add (new PendingDismissData (dismissPosition, dismissView));
			animator.Start ();
		}

		void onanimationendA (int originalHeight)
		{
			--mDismissAnimationRefCount;
			if (mDismissAnimationRefCount == 0) {
				// No active animations, process all pending dismisses.
				// Sort by descending position
				mPendingDismisses.Sort ();

				int[] dismissPositions = new int[mPendingDismisses.Count];
				for (int i = mPendingDismisses.Count - 1; i >= 0; i--) {
					dismissPositions [i] = mPendingDismisses [i].Position;
				}
				_dismissCommand.Execute (dismissPositions);

				// Reset mDownPosition to avoid MotionEvent.ACTION_UP trying to start a dismiss 
				// animation with a stale position
				_downPosition = AdapterView.InvalidPosition;

				ViewGroup.LayoutParams lp;
				foreach (var pendingDismiss in mPendingDismisses) {
					// Reset view presentation
					pendingDismiss.View.Alpha = 1;
					pendingDismiss.View.TranslationX = 0;
					lp = pendingDismiss.View.LayoutParameters;
					lp.Height = originalHeight;
					pendingDismiss.View.LayoutParameters = lp;
				}

				// Send a cancel event
				long time = SystemClock.UptimeMillis ();
				MotionEvent cancelEvent = MotionEvent.Obtain (time, time, MotionEventActions.Cancel, 0, 0, 0);
				_listView.DispatchTouchEvent (cancelEvent);

				mPendingDismisses.Clear ();
			}
		}

		class SwipeAnimatorListenerAdapter : AnimatorListenerAdapter
		{
			readonly Action<Animator> onAnimationEnd;

			public SwipeAnimatorListenerAdapter (Action<Animator> onAnimationEnd)
			{
				this.onAnimationEnd = onAnimationEnd;
			}

			public override void OnAnimationEnd (Animator animation)
			{
				onAnimationEnd (animation);
			}
		}

		class SwipeAnimatorUpdateListener : Java.Lang.Object,  ValueAnimator.IAnimatorUpdateListener
		{
			readonly Action<ValueAnimator> onAnimationUpdate;

			public SwipeAnimatorUpdateListener (Action<ValueAnimator> onAnimationUpdate)
			{
				this.onAnimationUpdate = onAnimationUpdate;
			}

			public void OnAnimationUpdate (ValueAnimator animation)
			{
				onAnimationUpdate (animation);
			}
		}
	}
}

