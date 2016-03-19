using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using ChGK.Core.Models;
using ChGK.Core.Services;
using ChGK.Core.Services.Favorites;
using ChGK.Core.Services.Messenger;
using ChGK.Core.Utils;

namespace ChGK.Core.ViewModels
{
    public class LastAddedTournamentsViewModel : TournamentsViewModel
    {
        private readonly IGAService _gaService;
        private readonly LoadMoreHelper<ITournament> _loadMoreHelper;
        private readonly IChGKService _service;

        private int _page;

        private ICommand _refreshCommand;

        public LastAddedTournamentsViewModel(IFavoritesService favoritesService,
            IMessagesService messagesService,
            IGAService gaService,
            IChGKService service)
            : base(favoritesService, messagesService)
        {
            Title = StringResources.LastAdded;

            _service = service;
            _gaService = gaService;

            _loadMoreHelper = new LoadMoreHelper<ITournament> {OnLastItemShown = OnLastItemShown};
        }

        public ICommand RefreshCommand =>
            _refreshCommand ?? (_refreshCommand = new Command(Refresh));

        private void Refresh()
        {
            _gaService.ReportEvent(GACategory.QuestionsList, GAAction.Click, "refresh");

            LoadForTheFirstTime();
        }

        public override void Start()
        {
            LoadForTheFirstTime();
        }

        private async void LoadForTheFirstTime()
        {
            _service.ClearLastAddedTournamentsCache();

            Tournaments = null;
            _page = 0;

            DataLoader.IsLoadingForTheFirstTime = true;
            DataLoader.IsLoadingMoreData = false;

            await DataLoader.LoadItemsAsync(LoadItems).ConfigureAwait(false);
        }

        protected override async Task LoadItems(CancellationToken token)
        {
            var tournaments = await _service.GetLastAddedTournaments(token, _page).ConfigureAwait(true);

            Tournaments = tournaments.Select(tournament =>
                new TournamentViewModel(FavoritesService, MessagesService, tournament)).ToList();
            _page = Tournaments.Count/101;

            _loadMoreHelper.Subscribe(Tournaments.Cast<LoadMoreOnScrollListViewItemViewModel<ITournament>>().ToList());
        }

        private async void OnLastItemShown()
        {
            _page++;

            DataLoader.IsLoadingForTheFirstTime = false;
            DataLoader.IsLoadingMoreData = true;

            await DataLoader.LoadItemsAsync(LoadItems).ConfigureAwait(false);
        }
    }
}