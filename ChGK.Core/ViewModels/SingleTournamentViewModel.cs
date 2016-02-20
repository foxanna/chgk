﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ChGK.Core.Services;
using ChGK.Core.Utils;

namespace ChGK.Core.ViewModels
{
    public class SingleTournamentViewModel : TournamentsViewModel
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly IChGKService _service;

        private string _filename;

        public SingleTournamentViewModel(IChGKService service)
        {
            Title = StringResources.Tournament;

            _service = service;

            DataLoader = new DataLoader();
        }

        protected override async Task LoadItems()
        {
            Tournaments = null;

            var tournament = await _service.GetTournament(_filename, _cancellationTokenSource.Token);

            Tournaments = new List<TournamentViewModel> {new TournamentViewModel(tournament)};
        }

        public async void Init(string filename)
        {
            if (filename.EndsWith(".txt"))
            {
                filename = filename.Substring(0, filename.Length - 4);
            }

            if (!filename.StartsWith("tour/"))
            {
                filename = "tour/" + filename;
            }

            _filename = filename;

            await RefreshAsync();
        }

        public override void OnViewDestroying()
        {
            _cancellationTokenSource.Cancel();

            base.OnViewDestroying();
        }
    }
}