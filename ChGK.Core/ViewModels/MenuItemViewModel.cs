using ChGK.Core.Utils;
using Cirrious.MvvmCross.ViewModels;
using System.Threading.Tasks;

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

