using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cirrious.MvvmCross.ViewModels;
using ChGK.Core.Models;
using ChGK.Core.Services;
using System.Threading;
using System;
using ChGK.Core.ViewModels.Tutorials;

namespace ChGK.Core.ViewModels
{
    public class LastAddedTournamentsViewModel : TournamentsViewModel
	{
		readonly IChGKWebService _service;
        readonly IFirstViewStartInfoProvider _firstViewStartInfoProvider;
        
		CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource ();

		public LastAddedTournamentsViewModel (IChGKWebService service, IFirstViewStartInfoProvider firstViewStartInfoProvider)
		{
            Title = StringResources.LastAdded;

			_service = service;
            _firstViewStartInfoProvider = firstViewStartInfoProvider;

			DataLoader = new DataLoader ();
		}

		protected override async Task LoadItems ()
		{
			Tournaments = null;

			var tournaments = await _service.GetLastAddedTournaments (0, _cancellationTokenSource.Token);

			Tournaments = tournaments.Select (tournament => new TournamentViewModel (tournament)).ToList ();
		}

		public async override void Start ()
		{
			await Refresh ();

            if (_firstViewStartInfoProvider.IsSeenForTheFirstTime(this.GetType()))
            {
                ShowViewModel(new FirstTimeSeenViewModelsFactory().CreateViewModel(this.GetType()));
                _firstViewStartInfoProvider.SetSeen(this.GetType());
            }
		}
	}
}

