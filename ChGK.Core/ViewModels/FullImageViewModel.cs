using System;
using Cirrious.MvvmCross.ViewModels;

namespace ChGK.Core.ViewModels
{
	public class FullImageViewModel : MvxViewModel
	{
		public string Picture { get; set; }

		public void Init (string image)
		{
			Picture = image;
		}

		MvxCommand _closeCommand;

		public MvxCommand CloseCommand {
			get {
				return _closeCommand ?? (_closeCommand = 
					new MvxCommand (() => Close (this)));
			}
		}
	}
}

