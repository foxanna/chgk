using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ChGK.Core.Services;
using ChGK.Core.Services.Favorites;
using ChGK.Core.Utils;

namespace ChGK.Core.ViewModels
{
    public class SingleTournamentViewModel : TournamentsViewModel
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly IFavoritesService _favoritesService;
        private readonly IChGKService _service;

        private string _id;

        public SingleTournamentViewModel(IChGKService service,
            IFavoritesService favoritesService)
        {
            Title = StringResources.Tournament;

            _service = service;
            _favoritesService = favoritesService;

            DataLoader = new DataLoader();
        }

        protected override async Task LoadItems()
        {
            Tournaments = null;

            var tournament = await _service.GetTournament(_id, _cancellationTokenSource.Token);

            Tournaments = new List<TournamentViewModel> {new TournamentViewModel(_favoritesService, tournament)};
        }

        public async void Init(string id)
        {
            _id = id;

            await RefreshAsync();
        }

        public override void OnViewDestroying()
        {
            _cancellationTokenSource.Cancel();

            base.OnViewDestroying();
        }
    }
}