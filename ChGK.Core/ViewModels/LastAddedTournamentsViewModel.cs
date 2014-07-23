using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using ChGK.Core.Models;
using ChGK.Core.Services;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;

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

		protected override async Task LoadItemsInternal ()
		{
			Tournaments = await ChGKService.GetLastAddedTournaments (0);
		}

		List<ITournament> _tournaments;

		public List<ITournament> Tournaments {
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

