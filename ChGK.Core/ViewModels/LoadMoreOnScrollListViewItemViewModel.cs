using System;

namespace ChGK.Core.ViewModels
{
    public class LoadMoreOnScrollListViewItemViewModel<T>
    {
        public T Item { get; set; }

        public event EventHandler ShowingForTheFirstTime;

        public void OnShowing()
        {
            var handler = ShowingForTheFirstTime;
            if (handler != null)
            {
                ShowingForTheFirstTime = null;
                handler(this, EventArgs.Empty);
            }
        }
    }
}