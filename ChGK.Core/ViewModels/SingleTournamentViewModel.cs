﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ChGK.Core.Services;
using ChGK.Core.Services.Favorites;

namespace ChGK.Core.ViewModels
{
    public class SingleTournamentViewModel : TournamentsViewModel
    {
        private readonly IFavoritesService _favoritesService;
        private readonly IChGKService _service;

        private string _id;

        public SingleTournamentViewModel(IChGKService service,
            IFavoritesService favoritesService)
        {
            Title = StringResources.Tournament;

            _service = service;
            _favoritesService = favoritesService;
        }

        public async void Init(string id)
        {
            _id = id;

            await DataLoader.LoadItemsAsync(LoadItems);
        }

        protected override async Task LoadItems(CancellationToken token)
        {
            Tournaments = null;

            var tournament = await _service.GetTournament(_id, token);

            Tournaments = new List<TournamentViewModel> {new TournamentViewModel(_favoritesService, tournament)};
        }
    }
}