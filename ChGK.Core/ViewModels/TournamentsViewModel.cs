using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using ChGK.Core.Models;
using ChGK.Core.Services.Favorites;
using ChGK.Core.Services.Messenger;
using ChGK.Core.Utils;

namespace ChGK.Core.ViewModels
{
    public abstract class TournamentsViewModel : MenuItemViewModel
    {
        protected readonly IFavoritesService FavoritesService;
        protected readonly IMessagesService MessagesService;

        private ICommand _onTourClick;
        private List<TournamentViewModel> _tournaments;

        protected TournamentsViewModel(IFavoritesService favoritesService,
            IMessagesService messagesService)
        {
            FavoritesService = favoritesService;
            MessagesService = messagesService;
        }

        public List<TournamentViewModel> Tournaments
        {
            get { return _tournaments; }
            set
            {
                _tournaments = value;
                RaisePropertyChanged();

                RaisePropertyChanged(() => HasNoTournaments);
            }
        }

        public bool HasNoTournaments => Tournaments != null && !Tournaments.Any();

        public ICommand OnTourClick => _onTourClick ?? (_onTourClick = new Command<ITour>(ClickTour));

        private void ClickTour(ITour tour)
        {
            ShowViewModel<TourViewModel>(new {name = tour.Name, id = tour.Id});
        }

        protected abstract Task LoadItems(CancellationToken token);
    }
}