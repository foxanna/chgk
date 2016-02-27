using System;

namespace ChGK.Core.ViewModels
{
    public class LoadMoreOnScrollListViewItemViewModel<T> : BaseViewModel
    {
        public T Item { get; set; }

        public event EventHandler ShowingForTheFirstTime;

        public void OnShowing()
        {
            var handler = ShowingForTheFirstTime;

            if (handler == null)
                return;

            ShowingForTheFirstTime = null;
            handler(this, EventArgs.Empty);
        }
    }
}