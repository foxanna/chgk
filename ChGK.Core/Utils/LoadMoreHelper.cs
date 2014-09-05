using ChGK.Core.ViewModels;
using System;
using System.Collections.Generic;

namespace ChGK.Core.Utils
{
    public class LoadMoreHelper<T>
    {
        public LoadMoreHelper()
        {
            LoadBefore = 5;
        }

        public int LoadBefore { get; set; }

        public Action OnLastItemShown { get; set; }
                
        public void Subscribe(List<LoadMoreOnScrollListViewItemViewModel<T>> items)
        {
            if (items.Count > LoadBefore)
            {
                items[items.Count - LoadBefore].ShowingForTheFirstTime += (s, e) => OnLastItemShown();
            }
        }
    }
}
