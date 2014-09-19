using System.Linq;
using System.Threading.Tasks;
using Cirrious.MvvmCross.ViewModels;
using ChGK.Core.Models;
using ChGK.Core.Services;
using System.Threading;
using System.Windows.Input;
using ChGK.Core.Utils;

namespace ChGK.Core.ViewModels
{
    public class LastAddedTournamentsViewModel : TournamentsViewModel
	{
		readonly IChGKWebService _service;
        readonly IFirstViewStartInfoProvider _firstViewStartInfoProvider;
        readonly IGAService _gaService;
        
		CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource ();

        LoadMoreHelper<ITournament> _loadMoreHelper;

		public LastAddedTournamentsViewModel (IChGKWebService service, IFirstViewStartInfoProvider firstViewStartInfoProvider, IGAService gaService)
		{
            Title = StringResources.LastAdded;

			_service = service;
            _firstViewStartInfoProvider = firstViewStartInfoProvider;
            _gaService = gaService;

			DataLoader = new DataLoader ();
            _loadMoreHelper = new LoadMoreHelper<ITournament>() { OnLastItemShown = OnLastItemShown };
		}
        
		public async override void Start ()
		{
            DataLoader.IsLoadingForTheFirstTime = true;
            await DataLoader.LoadItemsAsync(LoadItems);
		}

        int _page;

        protected override async Task LoadItems()
        {
          //  Tournaments = null;

            var tournaments = await _service.GetLastAddedTournaments(_cancellationTokenSource.Token, _page);

            Tournaments = tournaments.Select(tournament => new TournamentViewModel(tournament)).ToList();
            _page = Tournaments.Count / 101;

            _loadMoreHelper.Subscribe(Tournaments.Cast<LoadMoreOnScrollListViewItemViewModel<ITournament>>().ToList()); 
        }

        ICommand _refreshCommand;

        public ICommand RefreshCommand
        {
            get
            {
                return _refreshCommand ?? (_refreshCommand = new MvxCommand(async () => {
                    _gaService.ReportEvent(GACategory.QuestionsList, GAAction.Click, "refresh"); 
                    await Refresh();
                }));
            }
        }

        public override void OnViewDestroying()
        {
            _cancellationTokenSource.Cancel();

            base.OnViewDestroying();
        }

        async void OnLastItemShown()
        {
            _page++;

            DataLoader.IsLoadingForTheFirstTime = false;
            DataLoader.IsLoadingMoreData = true;

            await DataLoader.LoadItemsAsync(LoadItems);
        }
	}
}

