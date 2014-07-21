using System;
using Cirrious.MvvmCross.ViewModels;
using ChGK.Core.Services;

namespace ChGK.Core.ViewModels
{
	public class LoadableViewModel : MvxViewModel
	{
		protected readonly IChGKWebService _chGKService;

		public LoadableViewModel (IChGKWebService service)
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

