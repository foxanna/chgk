using System;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Localization;
using ChGK.Core.Services;

namespace ChGK.Core.ViewModels
{
	public abstract class BaseViewModel
		: MvxViewModel
	{
		protected readonly IChGKWebService _chGKService;

		public BaseViewModel (IChGKWebService service)
		{
			_chGKService = service;
		}

		private bool _isLoading;

		public bool IsLoading {
			get {
				return _isLoading;
			}
			set {
				_isLoading = value; 
				RaisePropertyChanged (() => IsLoading);
			}
		}
	}
}

