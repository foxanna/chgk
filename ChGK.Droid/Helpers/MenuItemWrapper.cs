using System;
using Android.Views;

namespace ChGK.Droid.Helpers
{
    public class MenuItemWrapper
    {
        private readonly IMenuItem _item;

        public MenuItemWrapper(IMenuItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException();
            }

            _item = item;
        }

        public bool Visible
        {
            get { return _item.IsVisible; }
            set { _item.SetVisible(value); }
        }
    }
}