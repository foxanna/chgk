using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using ChGK.Core.Models;
using ChGK.Core.Services;
using Cirrious.CrossCore;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;

namespace ChGK.Core.ViewModels
{
	public class LastAddedTournamentsViewModel : BaseViewModel
	{
		public LastAddedTournamentsViewModel (IChGKWebService service) : base (service)
		{

		}

		public async override void Start ()
		{
			await LoadTournamentsAsync ();
		}

		async Task LoadTournamentsAsync ()
		{
			IsLoading = true;

			try {
				Tournaments = await _chGKService.GetLastAddedTournaments (0);
			} catch (Exception e) {
				Mvx.Trace (e.Message);
			} finally {
				IsLoading = false;
			}
		}

		private List<ITournament> _tournaments;

		public List<ITournament> Tournaments {
			get {			
				return _tournaments;
			}
			set {
				_tournaments = value; 
				RaisePropertyChanged (() => Tournaments);
			}
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

		MvxCommand <ITour> onTourCLick;

		public MvxCommand <ITour> OnTourCLick {
			get {
				return onTourCLick ?? (onTourCLick = new MvxCommand<ITour> (ClickOnTour));
			}
		}

		void ClickOnTour (ITour tour)
		{

		}
	}
}

