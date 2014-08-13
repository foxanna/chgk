using Android.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChGK.Droid.Helpers
{
    public class MenuItemWrapper
    {
        readonly IMenuItem item;

        public MenuItemWrapper(IMenuItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException();
            }

            this.item = item;
        }

        public bool Visible
        {
            get
            {
                return item.IsVisible;
            }
            set
            {
                item.SetVisible(value);
            }
        }
    }
}
