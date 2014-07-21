using System;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Localization;
using ChGK.Core.Services;

namespace ChGK.Core.ViewModels
{
	public abstract class MenuItemViewModel : LoadableViewModel
	{
		public MenuItemViewModel (IChGKWebService service) : base (service)
		{
		}

		public string Title {
			get;
			protected set;
		}

		MvxCommand _refreshCommand;

		public MvxCommand RefreshCommand {
			get {
				return _refreshCommand ?? (_refreshCommand = new MvxCommand (Refresh));
			}
		}

		protected abstract void Refresh ();
	}
}

