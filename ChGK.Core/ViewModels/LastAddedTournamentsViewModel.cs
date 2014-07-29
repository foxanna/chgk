using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cirrious.MvvmCross.ViewModels;
using ChGK.Core.Models;
using ChGK.Core.Services;
using System.Threading;

namespace ChGK.Core.ViewModels
{
	public class LastAddedTournamentsViewModel : MenuItemViewModel
	{
		public LastAddedTournamentsViewModel (IChGKWebService service) : base (service)
		{
			Title = "Последние добавленные";
		}

		public async override void Start ()
		{
			await LoadItemsAsync ();
		}

		protected override async Task LoadItemsInternal (CancellationToken token)
		{
			var tournaments = await ChGKService.GetLastAddedTournaments (0, token);
			Tournaments = tournaments.Select (tournament => new TournamentViewModel (tournament)).ToList ();
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

