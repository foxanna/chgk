using System;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using Android.Animation;
using Android.Graphics;
using Android.OS;

namespace ChGK.Droid.Controls.SwipeToDismiss
{
	public interface IDismissCallbacks
	{
		bool canDismiss (int position);

		void onDismiss (ListView listView, int[] reverseSortedPositions);
	}

	class PendingDismissData : IComparable
	{
		public int position;
		public View view;

		public PendingDismissData (int position, View view)
		{
			this.position = position;
			this.view = view;
		}

		public int CompareTo (object obj)
		{
			if (obj == null)
				return 1;

			var other = obj as PendingDismissData;
			if (other != null)
				return other.position - this.position;
			else
				throw new ArgumentException ("Object is not a PendingDismissData");
		}
	}

	public class SwipeDismissListViewTouchListener : Java.Lang.Object, View.IOnTouchListener, AbsListView.IOnScrollListener
	{
		private int mSlop;
		private int mMinFlingVelocity;
		private int mMaxFlingVelocity;
		private long mAnimationTime;

		// Fixed properties
		private ListView mListView;
		private IDismissCallbacks mCallbacks;
		private int mViewWidth = 1;
		// 1 and not 0 to prevent dividing by zero

		// Transient properties
		private List<PendingDismissData> mPendingDismisses = new List<PendingDismissData> ();
		private int mDismissAnimationRefCount = 0;
		private float mDownX;
		private float mDownY;
		private bool mSwiping;
		private int mSwipingSlop;
		private VelocityTracker mVelocityTracker;
		private int mDownPosition;
		private View mDownView;
		private bool mPaused;

		public event EventHandler OnDismiss;

		public SwipeDismissListViewTouchListener (ListView listView, IDismissCallbacks callbacks)
		{
			ViewConfiguration vc = ViewConfiguration.Get (listView.Context);
			mSlop = vc.ScaledTouchSlop;
			mMinFlingVelocity = vc.ScaledMinimumFlingVelocity * 16;
			mMaxFlingVelocity = vc.ScaledMaximumFlingVelocity;
			mAnimationTime = listView.Context.Resources.GetInteger (Android.Resource.Integer.ConfigShortAnimTime);
			mListView = listView;
			mCallbacks = callbacks;
		}

		public bool Enabled {
			set {
				mPaused = !value;
			}
		}

		public bool OnTouch (View view, MotionEvent motionEvent)
		{
			if (mViewWidth < 2) {
				mViewWidth = mListView.Width;
			}

			switch (motionEvent.ActionMasked) {
			case MotionEventActions.Down:
				{
					if (mPaused) {
						return false;
					}

					Rect rect = new Rect ();
					int childCount = mListView.ChildCount;
					int[] listViewCoords = new int[2];
					mListView.GetLocationOnScreen (listViewCoords);
					int x = (int)motionEvent.RawX - listViewCoords [0];
					int y = (int)motionEvent.RawY - listViewCoords [1];
					View child;
					for (int i = 0; i < childCount; i++) {
						child = mListView.GetChildAt (i);
						child.GetHitRect (rect);
						if (rect.Contains (x, y)) {
							mDownView = child;
							break;
						}
					}

					if (mDownView != null) {
						mDownX = motionEvent.RawX;
						mDownY = motionEvent.RawY;
						mDownPosition = mListView.GetPositionForView (mDownView);
						if (mCallbacks.canDismiss (mDownPosition)) {
							mVelocityTracker = VelocityTracker.Obtain ();
							mVelocityTracker.AddMovement (motionEvent);
						} else {
							mDownView = null;
						}
					}
					return false;
				}

			case MotionEventActions.Cancel:
				{
					if (mVelocityTracker == null) {
						break;
					}

					if (mDownView != null && mSwiping) {
						// cancel
						mDownView.Animate ()
							.TranslationX (0)
							.Alpha (1)
							.SetDuration (mAnimationTime)
							.SetListener (null);
					}
					mVelocityTracker.Recycle ();
					mVelocityTracker = null;
					mDownX = 0;
					mDownY = 0;
					mDownView = null;
					mDownPosition = AdapterView.InvalidPosition;
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
					if (Math.Abs (deltaX) > mViewWidth / 2 && mSwiping) {
						dismiss = true;
						dismissRight = deltaX > 0;
					} else if (mMinFlingVelocity <= absVelocityX && absVelocityX <= mMaxFlingVelocity
					           && absVelocityY < absVelocityX && mSwiping) {
						// dismiss only if flinging in the same direction as dragging
						dismiss = (velocityX < 0) == (deltaX < 0);
						dismissRight = mVelocityTracker.XVelocity > 0;
					}
					if (dismiss && mDownPosition != AdapterView.InvalidPosition) {
						// dismiss
						View downView = mDownView; // mDownView gets null'd before animation ends
						int downPosition = mDownPosition;
						++mDismissAnimationRefCount;
						mDownView.Animate ()
							.TranslationX (dismissRight ? mViewWidth : -mViewWidth)
							.Alpha (0)
							.SetDuration (mAnimationTime)
							.SetListener (new SwipeAnimatorListenerAdapter (_ => performDismiss (downView, downPosition)));
					} else {
						// cancel
						mDownView.Animate ()
							.TranslationX (0)
							.Alpha (1)
							.SetDuration (mAnimationTime)
							.SetListener (null);
					}
					mVelocityTracker.Recycle ();
					mVelocityTracker = null;
					mDownX = 0;
					mDownY = 0;
					mDownView = null;
					mDownPosition = AdapterView.InvalidPosition;
					mSwiping = false;
					break;
				}

			case MotionEventActions.Move:
				{
					if (mVelocityTracker == null || mPaused) {
						break;
					}

					mVelocityTracker.AddMovement (motionEvent);
					float deltaX = motionEvent.RawX - mDownX;
					float deltaY = motionEvent.RawY - mDownY;
					if (Math.Abs (deltaX) > mSlop && Math.Abs (deltaY) < Math.Abs (deltaX) / 2) {
						mSwiping = true;
						mSwipingSlop = (deltaX > 0 ? mSlop : -mSlop);
						mListView.RequestDisallowInterceptTouchEvent (true);

						// Cancel ListView's touch (un-highlighting the item)
						var cancelEvent = MotionEvent.Obtain (motionEvent);
						cancelEvent.Action = MotionEventActions.Cancel |
						(MotionEventActions)((int)motionEvent.ActionIndex << (int)MotionEventActions.PointerIndexShift);
						mListView.OnTouchEvent (cancelEvent);
						cancelEvent.Recycle ();
					}

					if (mSwiping) {
						mDownView.TranslationX = deltaX - mSwipingSlop;
						mDownView.Alpha = Math.Max (0f, Math.Min (1f, 1f - 2f * Math.Abs (deltaX) / mViewWidth));
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
					dismissPositions [i] = mPendingDismisses [i].position;
				}
				mCallbacks.onDismiss (mListView, dismissPositions);

				// Reset mDownPosition to avoid MotionEvent.ACTION_UP trying to start a dismiss 
				// animation with a stale position
				mDownPosition = AdapterView.InvalidPosition;

				ViewGroup.LayoutParams lp;
				foreach (var pendingDismiss in mPendingDismisses) {
					// Reset view presentation
					pendingDismiss.view.Alpha = 1;
					pendingDismiss.view.TranslationX = 0;
					lp = pendingDismiss.view.LayoutParameters;
					lp.Height = originalHeight;
					pendingDismiss.view.LayoutParameters = lp;
				}

				// Send a cancel event
				long time = SystemClock.UptimeMillis ();
				MotionEvent cancelEvent = MotionEvent.Obtain (time, time, MotionEventActions.Cancel, 0, 0, 0);
				mListView.DispatchTouchEvent (cancelEvent);

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

