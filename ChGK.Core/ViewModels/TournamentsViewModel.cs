using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using ChGK.Core.Models;
using ChGK.Core.Utils;

namespace ChGK.Core.ViewModels
{
    public abstract class TournamentsViewModel : MenuItemViewModel
    {
        private ICommand _onTourClick;
        private List<TournamentViewModel> _tournaments;

        public List<TournamentViewModel> Tournaments
        {
            get { return _tournaments; }
            set
            {
                _tournaments = value;
                RaisePropertyChanged(() => Tournaments);
            }
        }

        public ICommand OnTourClick => _onTourClick ?? (_onTourClick = new Command<ITour>(ClickTour));

        private void ClickTour(ITour tour)
        {
            ShowViewModel<TourViewModel>(new {name = tour.Name, id = tour.Id});
        }

        protected abstract Task LoadItems(CancellationToken token);
    }
}