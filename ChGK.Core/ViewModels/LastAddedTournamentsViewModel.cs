using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using ChGK.Core.Models;
using ChGK.Core.Services;
using ChGK.Core.Services.Favorites;
using ChGK.Core.Utils;

namespace ChGK.Core.ViewModels
{
    public class LastAddedTournamentsViewModel : TournamentsViewModel
    {
        private readonly IFavoritesService _favoritesService;
        private readonly IGAService _gaService;
        private readonly LoadMoreHelper<ITournament> _loadMoreHelper;
        private readonly IChGKService _service;

        private int _page;

        private ICommand _refreshCommand;

        public LastAddedTournamentsViewModel(IChGKService service,
            IGAService gaService,
            IFavoritesService favoritesService)
        {
            Title = StringResources.LastAdded;

            _service = service;
            _gaService = gaService;
            _favoritesService = favoritesService;

            _loadMoreHelper = new LoadMoreHelper<ITournament> {OnLastItemShown = OnLastItemShown};
        }

        public ICommand RefreshCommand =>
            _refreshCommand ?? (_refreshCommand = new Command(Refresh));

        private async void Refresh()
        {
            _gaService.ReportEvent(GACategory.QuestionsList, GAAction.Click, "refresh");
            await RefreshAsync();
        }

        public override async void Start()
        {
            DataLoader.IsLoadingForTheFirstTime = true;
            await RefreshAsync();
        }

        protected override async Task LoadItems(CancellationToken token, bool useCache)
        {
            var tournaments = await _service.GetLastAddedTournaments(token, _page, useCache);

            Tournaments = tournaments.Select(tournament =>
                new TournamentViewModel(_favoritesService, tournament)).ToList();
            _page = Tournaments.Count/101;

            _loadMoreHelper.Subscribe(Tournaments.Cast<LoadMoreOnScrollListViewItemViewModel<ITournament>>().ToList());
        }

        private async void OnLastItemShown()
        {
            _page++;

            DataLoader.IsLoadingForTheFirstTime = false;
            DataLoader.IsLoadingMoreData = true;

            await DataLoader.LoadItemsAsync(token => LoadItems(token, true));
        }
    }
}