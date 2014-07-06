using System;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Localization;
using ChGK.Core.Services;

namespace ChGK.Core.ViewModels
{
	public abstract class BaseViewModel
		: MvxViewModel
	{
		//		public IMvxLanguageBinder TextSource {
		//			get {
		//				return new MvxLanguageBinder (
		//					Constants.GeneralNamespace,
		//					GetType ().Name);
		//			}
		//		}

		protected readonly IChGKWebService _chGKService;

		public BaseViewModel (IChGKWebService service)
		{
			_chGKService = service;
		}

	}
}

