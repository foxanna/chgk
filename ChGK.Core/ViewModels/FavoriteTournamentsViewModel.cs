using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using ChGK.Core.Services;
using ChGK.Core.Services.Favorites;
using ChGK.Core.Services.Messenger;
using ChGK.Core.Utils;

namespace ChGK.Core.ViewModels
{
    public class FavoriteTournamentsViewModel : TournamentsViewModel
    {
        private readonly IChGKService _chGkService;
        private ICommand _refreshCommand;

        public FavoriteTournamentsViewModel(IChGKService chGkService,
            IFavoritesService favoritesService,
            IMessagesService messagesService)
            : base(favoritesService, messagesService)
        {
            Title = StringResources.Favorites;

            _chGkService = chGkService;
        }

        public ICommand RefreshCommand =>
            _refreshCommand ?? (_refreshCommand = new Command(Refresh));

        private async void Refresh()
        {
            await DataLoader.LoadItemsAsync(LoadItems).ConfigureAwait(false);
        }

        protected override async Task LoadItems(CancellationToken token)
        {
            var favoriteTournaments = await Task.WhenAll(FavoritesService.GetFavoriteTournaments()
                .Select(favoriteTournament => _chGkService.GetTournament(favoriteTournament.Id, token)))
                .ConfigureAwait(true);

            Tournaments = favoriteTournaments.Select(tournament =>
                new TournamentViewModel(FavoritesService, MessagesService, tournament)).ToList();
        }

        public override void Start()
        {
            base.Start();

            Refresh();
        }
    }
}