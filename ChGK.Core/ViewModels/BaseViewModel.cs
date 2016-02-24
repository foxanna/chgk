using ChGK.Core.Utils;
using MvvmCross.Core.ViewModels;

namespace ChGK.Core.ViewModels
{
    public class BaseViewModel : MvxViewModel, IViewLifecycle
    {
        public BaseViewModel()
        {
            DataLoader = new DataLoader();
        }

        public DataLoader DataLoader { get; protected set; }

        public virtual void OnViewDestroying()
        {
            DataLoader?.CancelLoading();
        }
    }
}