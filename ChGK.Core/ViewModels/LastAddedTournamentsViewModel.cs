using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using ChGK.Core.Models;
using ChGK.Core.Services;
using ChGK.Core.Utils;
using MvvmCross.Core.ViewModels;

namespace ChGK.Core.ViewModels
{
    public class LastAddedTournamentsViewModel : TournamentsViewModel
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly IFirstViewStartInfoProvider _firstViewStartInfoProvider;
        private readonly IGAService _gaService;

        private readonly LoadMoreHelper<ITournament> _loadMoreHelper;
        private readonly IChGKWebService _service;

        private int _page;

        private ICommand _refreshCommand;

        public LastAddedTournamentsViewModel(IChGKWebService service,
            IFirstViewStartInfoProvider firstViewStartInfoProvider, IGAService gaService)
        {
            Title = StringResources.LastAdded;

            _service = service;
            _firstViewStartInfoProvider = firstViewStartInfoProvider;
            _gaService = gaService;

            DataLoader = new DataLoader();
            _loadMoreHelper = new LoadMoreHelper<ITournament> {OnLastItemShown = OnLastItemShown};
        }

        public ICommand RefreshCommand
        {
            get
            {
                return _refreshCommand ?? (_refreshCommand = new MvxCommand(async () =>
                {
                    _gaService.ReportEvent(GACategory.QuestionsList, GAAction.Click, "refresh");
                    await Refresh();
                }));
            }
        }

        public override async void Start()
        {
            DataLoader.IsLoadingForTheFirstTime = true;
            await DataLoader.LoadItemsAsync(LoadItems);
        }

        protected override async Task LoadItems()
        {
            //  Tournaments = null;

            var tournaments = await _service.GetLastAddedTournaments(_cancellationTokenSource.Token, _page);

            Tournaments = tournaments.Select(tournament => new TournamentViewModel(tournament)).ToList();
            _page = Tournaments.Count/101;

            _loadMoreHelper.Subscribe(Tournaments.Cast<LoadMoreOnScrollListViewItemViewModel<ITournament>>().ToList());
        }

        public override void OnViewDestroying()
        {
            _cancellationTokenSource.Cancel();

            base.OnViewDestroying();
        }

        private async void OnLastItemShown()
        {
            _page++;

            DataLoader.IsLoadingForTheFirstTime = false;
            DataLoader.IsLoadingMoreData = true;

            await DataLoader.LoadItemsAsync(LoadItems);
        }
    }
}