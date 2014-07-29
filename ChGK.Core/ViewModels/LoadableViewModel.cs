using System;
using System.Threading;
using System.Threading.Tasks;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using ChGK.Core.DbChGKInfo;
using ChGK.Core.Services;

namespace ChGK.Core.ViewModels
{
	public abstract class LoadableViewModel : MvxViewModel
	{
		protected readonly IChGKWebService ChGKService;

		protected CancellationTokenSource tokenSource;

		protected LoadableViewModel (IChGKWebService chgkWebService)
		{
			ChGKService = chgkWebService;
		}

		bool _isLoading;

		public bool IsLoading {
			get {
				return _isLoading;
			}
			set {
				_isLoading = value; 

				RaisePropertyChanged (() => IsLoading);
				RaisePropertyChanged (() => HasData);
			}
		}

		void StopIfLoading ()
		{
			HasError = false;
			IsLoading = false;

			if (tokenSource != null) {
				tokenSource.Cancel ();
			}

			tokenSource = new CancellationTokenSource ();
		}

		protected async Task LoadItemsAsync ()
		{
			StopIfLoading ();

			HasError = false;
			IsLoading = true;

			try {
				await LoadItemsInternal ();
			} catch (NoConnectionException e) {
				HasError = true;
				Mvx.Trace (e.Message);
				Error = "Проверьте интернет соединение.";
			} catch (Exception e) {
				HasError = true;
				Mvx.Trace (e.Message);
				Error = "Не удалось загрузить данные.\n" + e.Message + "\nПопробуйте еще раз.";
			} finally {
				IsLoading = false;
			}
		}

		protected abstract Task LoadItemsInternal ();

		string _error;

		public string Error {
			get {
				return _error;
			}
			set {
				_error = value;
				RaisePropertyChanged (() => Error);
			}
		}

		bool _hasError;

		public bool HasError {
			get{ return _hasError; }
			set {
				_hasError = value;
				RaisePropertyChanged (() => HasError);
				RaisePropertyChanged (() => HasData);
			}
		}

		public bool HasData {
			get {
				return !HasError && !IsLoading; 
			}
		}
	}
}

