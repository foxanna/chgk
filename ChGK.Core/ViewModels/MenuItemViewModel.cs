using Cirrious.MvvmCross.ViewModels;
using System.Threading.Tasks;

namespace ChGK.Core.ViewModels
{
	public abstract class MenuItemViewModel : MvxViewModel
	{
		public string Title {
			get;
			protected set;
		}

		public abstract Task Refresh ();
	}
}

