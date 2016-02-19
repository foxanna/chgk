using ChGK.Core.Utils;
using MvvmCross.Core.ViewModels;

namespace ChGK.Core.ViewModels
{
    public abstract class MenuItemViewModel : MvxViewModel, IViewLifecycle
    {
        public string Title { get; protected set; }

        public virtual void OnViewDestroying()
        {
        }
    }
}