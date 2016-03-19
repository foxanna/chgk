using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChGK.Core.Services;
using ChGK.Core.Services.Favorites;
using ChGK.Core.Services.Messenger;

namespace ChGK.Core.ViewModels
{
    public class FavoriteTournamentsViewModel : TournamentsViewModel
    {
        private readonly IChGKService _chGkService;

        public FavoriteTournamentsViewModel(IChGKService chGkService,
            IFavoritesService favoritesService,
            IMessagesService messagesService)
            : base(favoritesService, messagesService)
        {
            Title = StringResources.Favorites;

            _chGkService = chGkService;
        }

        protected override async Task LoadItems(CancellationToken token)
        {
            var favoriteTournaments = await Task.WhenAll(FavoritesService.GetFavoriteTournaments()
                .Select(favoriteTournament => _chGkService.GetTournament(favoriteTournament.Id, token)))
                .ConfigureAwait(true);

            Tournaments = favoriteTournaments.Select(tournament =>
                new TournamentViewModel(FavoritesService, MessagesService, tournament)).ToList();
        }

        public override async void Start()
        {
            base.Start();

            await DataLoader.LoadItemsAsync(LoadItems).ConfigureAwait(false);
        }
    }
}