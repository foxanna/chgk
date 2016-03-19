using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ChGK.Core.Services;
using ChGK.Core.Services.Favorites;
using ChGK.Core.Services.Messenger;

namespace ChGK.Core.ViewModels
{
    public class SingleTournamentViewModel : TournamentsViewModel
    {
        private readonly IChGKService _service;

        private string _id;

        public SingleTournamentViewModel(IFavoritesService favoritesService,
            IMessagesService messagesService,
            IChGKService service)
            : base(favoritesService, messagesService)
        {
            Title = StringResources.Tournament;

            _service = service;
        }

        public async void Init(string id)
        {
            _id = id;

            await DataLoader.LoadItemsAsync(LoadItems).ConfigureAwait(false);
        }

        protected override async Task LoadItems(CancellationToken token)
        {
            Tournaments = null;

            var tournament = await _service.GetTournament(_id, token).ConfigureAwait(true);

            Tournaments = new List<TournamentViewModel>
            {
                new TournamentViewModel(FavoritesService, MessagesService, tournament)
            };
        }
    }
}