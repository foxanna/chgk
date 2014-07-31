using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cirrious.MvvmCross.ViewModels;
using ChGK.Core.Models;
using ChGK.Core.Services;
using System.Threading;
using System;

namespace ChGK.Core.ViewModels
{
	public class LastAddedTournamentsViewModel : MenuItemViewModel
	{
		IChGKWebService _service;

		public DataLoader DataLoader { get; private set; }

		CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource ();

		public LastAddedTournamentsViewModel (IChGKWebService service)
		{
			Title = "Последние добавленные";

			_service = service;
			DataLoader = new DataLoader ();
		}

		async Task LoadItems ()
		{
			Tournaments = null;

			var tournaments = await _service.GetLastAddedTournaments (0, _cancellationTokenSource.Token);

			Tournaments = tournaments.Select (tournament => new TournamentViewModel (tournament)).ToList ();
		}

		public async override void Start ()
		{
			await Refresh ();
		}

		public async override Task Refresh ()
		{
			await DataLoader.LoadItemsAsync (LoadItems);
		}

		List<TournamentViewModel> _tournaments;

		public List<TournamentViewModel> Tournaments {
			get {			
				return _tournaments;
			}
			set {
				_tournaments = value; 
				RaisePropertyChanged (() => Tournaments);
			}
		}

		MvxCommand <ITour> onTourCLick;

		public MvxCommand <ITour> OnTourCLick {
			get {
				return onTourCLick ?? (onTourCLick = new MvxCommand<ITour> (
					tour => ShowViewModel<TourViewModel> (new { name = tour.Name, filename = tour.FileName })));
			}
		}
	}
}

