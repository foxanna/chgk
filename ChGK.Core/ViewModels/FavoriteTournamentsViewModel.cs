using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChGK.Core.Services;
using ChGK.Core.Services.Favorites;

namespace ChGK.Core.ViewModels
{
    public class FavoriteTournamentsViewModel : TournamentsViewModel
    {
        private readonly IChGKService _chGkService;
        private readonly IFavoritesService _favoritesService;

        public FavoriteTournamentsViewModel(IFavoritesService favoritesService,
            IChGKService chGkService)
        {
            _favoritesService = favoritesService;
            _chGkService = chGkService;
        }

        protected override async Task LoadItems(CancellationToken token)
        {
            var favoriteTournaments = await Task.WhenAll(_favoritesService.GetFavoriteTournaments()
                .Select(favoriteTournament => _chGkService.GetTournament(favoriteTournament.Id, token)));

            Tournaments = favoriteTournaments.Select(tournament =>
                new TournamentViewModel(_favoritesService, tournament)).ToList();
        }
    }
}