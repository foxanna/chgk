using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Android.Animation;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using Object = Java.Lang.Object;

namespace ChGK.Droid.Controls.SwipeToDismiss
{
    internal class PendingDismissData
    {
        public PendingDismissData(int position, View view)
        {
            Position = position;
            View = view;
        }

        public int Position { get; }

        public View View { get; }

        public void ResetViewPresentation(int originalHeight)
        {
            View.Alpha = 1;
            View.TranslationX = 0;
            var lp = View.LayoutParameters;
            lp.Height = originalHeight;
            View.LayoutParameters = lp;
        }
    }

    public class SwipeDismissListViewTouchListener : Object, View.IOnTouchListener, AbsListView.IOnScrollListener
    {
        private readonly ICommand _dismissCommand;

        // Fixed properties
        private readonly ListView _listView;
        private readonly int _mMaxFlingVelocity;
        private readonly int _mMinFlingVelocity;
        // 1 and not 0 to prevent dividing by zero

        // Transient properties
        private readonly List<PendingDismissData> _mPendingDismisses = new List<PendingDismissData>();
        private readonly int _mSlop;
        private int _downPosition;
        private View _downView;
        private bool _isSwiping;
        private int _mDismissAnimationRefCount;
        private float _mDownX;
        private float _mDownY;
        private int _mSwipingSlop;
        private bool _paused;
        private VelocityTracker _velocityTracker;

        private int _viewWidth = 1;

        public SwipeDismissListViewTouchListener(ListView listView, ICommand dismissCommand)
        {
            var vc = ViewConfiguration.Get(listView.Context);
            _mSlop = vc.ScaledTouchSlop;
            _mMinFlingVelocity = vc.ScaledMinimumFlingVelocity*16;
            _mMaxFlingVelocity = vc.ScaledMaximumFlingVelocity;

            AnimationTime = listView.Context.Resources.GetInteger(Android.Resource.Integer.ConfigShortAnimTime);

            _listView = listView;
            _dismissCommand = dismissCommand;
        }

        public long AnimationTime { get; set; }

        public bool Enabled
        {
            set { _paused = !value; }
        }

        public void OnScroll(AbsListView view, int firstVisibleItem, int visibleItemCount, int totalItemCount)
        {
        }

        public void OnScrollStateChanged(AbsListView view, ScrollState scrollState)
        {
            Enabled = scrollState != ScrollState.TouchScroll;
        }

        public bool OnTouch(View view, MotionEvent motionEvent)
        {
            SaveWidth();

            switch (motionEvent.ActionMasked)
            {
                case MotionEventActions.Down:
                    return OnDown(motionEvent);
                case MotionEventActions.Cancel:
                    return OnCancel();
                case MotionEventActions.Up:
                    return OnUp(motionEvent);
                case MotionEventActions.Move:
                    return OnMove(motionEvent);
                default:
                    return false;
            }
        }

        private void SaveWidth()
        {
            if (_viewWidth < 2)
            {
                _viewWidth = _listView.Width;
            }
        }

        private bool IsSwiping(float deltaX, float deltaY)
        {
            return Math.Abs(deltaX) > _mSlop && Math.Abs(deltaY) < Math.Abs(deltaX)/2;
        }

        private bool FlingingSameDirectionAsDragging(float absVelocityX, float absVelocityY)
        {
            return _mMinFlingVelocity <= absVelocityX && absVelocityX <= _mMaxFlingVelocity &&
                   absVelocityY < absVelocityX &&
                   _isSwiping;
        }

        protected virtual void AnimateDismissCancel(View view)
        {
            Animate(view, 0, 1, null);
        }

        protected virtual void AnimateDismissStart(View view, bool dismissRight,
            SwipeAnimatorListenerAdapter animatorListener)
        {
            Animate(view, dismissRight ? _viewWidth : -_viewWidth, 0, animatorListener);
        }

        protected void Animate(View view, int xTranslation, int alpha, SwipeAnimatorListenerAdapter animatorListener)
        {
            view.Animate()
                .TranslationX(xTranslation)
                .Alpha(alpha)
                .SetDuration(AnimationTime)
                .SetListener(animatorListener);
        }

        private void AnimateDismissEnd(View dismissView, int dismissPosition)
        {
            var lp = dismissView.LayoutParameters;
            var originalHeight = dismissView.Height;

            var animator = ValueAnimator.OfInt(originalHeight, 1).SetDuration(AnimationTime) as ValueAnimator;

            animator.AddListener(new SwipeAnimatorListenerAdapter(_ => ExecuteDismiss(originalHeight)));

            animator.AddUpdateListener(new SwipeAnimatorUpdateListener(valueAnimator =>
            {
                lp.Height = (int) valueAnimator.AnimatedValue;
                dismissView.LayoutParameters = lp;
            }));

            _mPendingDismisses.Add(new PendingDismissData(dismissPosition, dismissView));
            animator.Start();
        }

        private void ExecuteDismiss(int originalHeight)
        {
            --_mDismissAnimationRefCount;

            if (_mDismissAnimationRefCount == 0)
            {
                CallDismissCommand();
                InvalidateDownPosition();
                ResetViewPresentation(originalHeight);
                SendCancelEvent();
            }
        }

        private void CallDismissCommand()
        {
            var dismissPositions = _mPendingDismisses.Select(data => data.Position).OrderByDescending(t => t).ToList();

            _dismissCommand.Execute(dismissPositions.ToArray());
        }

        private void ResetViewPresentation(int originalHeight)
        {
            foreach (var pendingDismiss in _mPendingDismisses)
            {
                pendingDismiss.ResetViewPresentation(originalHeight);
            }

            _mPendingDismisses.Clear();
        }

        private void SendCancelEvent()
        {
            var time = SystemClock.UptimeMillis();
            var cancelEvent = MotionEvent.Obtain(time, time, MotionEventActions.Cancel, 0, 0, 0);
            _listView.DispatchTouchEvent(cancelEvent);
        }

        private void InvalidateDownPosition()
        {
            // Reset mDownPosition to avoid MotionEvent.ACTION_UP trying to start a dismiss 
            // animation with a stale position
            _downPosition = AdapterView.InvalidPosition;
        }

        public class SwipeAnimatorListenerAdapter : AnimatorListenerAdapter
        {
            private readonly Action<Animator> _onAnimationEnd;

            public SwipeAnimatorListenerAdapter(Action<Animator> onAnimationEnd)
            {
                _onAnimationEnd = onAnimationEnd;
            }

            public override void OnAnimationEnd(Animator animation)
            {
                _onAnimationEnd(animation);
            }
        }

        public class SwipeAnimatorUpdateListener : Object, ValueAnimator.IAnimatorUpdateListener
        {
            private readonly Action<ValueAnimator> _onAnimationUpdate;

            public SwipeAnimatorUpdateListener(Action<ValueAnimator> onAnimationUpdate)
            {
                _onAnimationUpdate = onAnimationUpdate;
            }

            public void OnAnimationUpdate(ValueAnimator animation)
            {
                _onAnimationUpdate(animation);
            }
        }

        #region TouchActionsHandlers

        private bool OnDown(MotionEvent motionEvent)
        {
            if (_paused)
            {
                return false;
            }

            var rect = new Rect();
            var childCount = _listView.ChildCount;

            var listViewCoords = new int[2];
            _listView.GetLocationOnScreen(listViewCoords);

            var x = (int) motionEvent.RawX - listViewCoords[0];
            var y = (int) motionEvent.RawY - listViewCoords[1];

            for (var i = 0; i < childCount; i++)
            {
                var child = _listView.GetChildAt(i);
                child.GetHitRect(rect);
                if (rect.Contains(x, y))
                {
                    _downView = child;
                    break;
                }
            }

            if (_downView != null)
            {
                _mDownX = motionEvent.RawX;
                _mDownY = motionEvent.RawY;

                _downPosition = _listView.GetPositionForView(_downView);

                if (_dismissCommand.CanExecute(_downPosition))
                {
                    _velocityTracker = VelocityTracker.Obtain();
                    _velocityTracker.AddMovement(motionEvent);
                }
                else
                {
                    _downView = null;
                }
            }

            return false;
        }

        private bool OnCancel()
        {
            if (_velocityTracker == null)
            {
                return false;
            }

            if (_downView != null && _isSwiping)
            {
                AnimateDismissCancel(_downView);
            }

            _velocityTracker.Recycle();
            _velocityTracker = null;

            _mDownX = 0;
            _mDownY = 0;

            _downView = null;
            _downPosition = AdapterView.InvalidPosition;

            _isSwiping = false;

            return false;
        }

        private bool OnUp(MotionEvent motionEvent)
        {
            if (_velocityTracker == null)
            {
                return false;
            }

            var deltaX = motionEvent.RawX - _mDownX;

            _velocityTracker.AddMovement(motionEvent);
            _velocityTracker.ComputeCurrentVelocity(1000);

            var velocityX = _velocityTracker.XVelocity;
            var absVelocityX = Math.Abs(velocityX);
            var absVelocityY = Math.Abs(_velocityTracker.YVelocity);

            var dismiss = false;
            var dismissRight = false;

            if (Math.Abs(deltaX) > _viewWidth/2 && _isSwiping)
            {
                dismiss = true;
                dismissRight = deltaX > 0;
            }
            else if (FlingingSameDirectionAsDragging(absVelocityX, absVelocityY))
            {
                dismiss = (velocityX < 0) == (deltaX < 0);
                dismissRight = _velocityTracker.XVelocity > 0;
            }

            if (dismiss && _downPosition != AdapterView.InvalidPosition)
            {
                var downView = _downView;
                var downPosition = _downPosition;

                ++_mDismissAnimationRefCount;

                AnimateDismissStart(_downView, dismissRight,
                    new SwipeAnimatorListenerAdapter(_ => AnimateDismissEnd(downView, downPosition)));
            }
            else
            {
                AnimateDismissCancel(_downView);
            }

            _velocityTracker.Recycle();
            _velocityTracker = null;

            _mDownX = 0;
            _mDownY = 0;

            _downView = null;
            InvalidateDownPosition();

            _isSwiping = false;

            return false;
        }

        private bool OnMove(MotionEvent motionEvent)
        {
            if (_velocityTracker == null || _paused)
            {
                return false;
            }

            _velocityTracker.AddMovement(motionEvent);

            var deltaX = motionEvent.RawX - _mDownX;
            var deltaY = motionEvent.RawY - _mDownY;

            if (IsSwiping(deltaX, deltaY))
            {
                _isSwiping = true;

                _mSwipingSlop = (deltaX > 0 ? _mSlop : -_mSlop);

                _listView.RequestDisallowInterceptTouchEvent(true);

                // Cancel ListView's touch (un-highlighting the item)
                var cancelEvent = MotionEvent.Obtain(motionEvent);
                cancelEvent.Action = MotionEventActions.Cancel |
                                     (MotionEventActions)
                                         (motionEvent.ActionIndex << (int) MotionEventActions.PointerIndexShift);

                _listView.OnTouchEvent(cancelEvent);
                cancelEvent.Recycle();
            }

            if (_isSwiping)
            {
                _downView.TranslationX = deltaX - _mSwipingSlop;
                _downView.Alpha = Math.Max(0f, Math.Min(1f, 1f - 2f*Math.Abs(deltaX)/_viewWidth));

                return true;
            }

            return false;
        }

        #endregion
    }
}