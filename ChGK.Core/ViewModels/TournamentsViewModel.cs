using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using ChGK.Core.Models;
using ChGK.Core.Utils;

namespace ChGK.Core.ViewModels
{
    public abstract class TournamentsViewModel : MenuItemViewModel
    {
        private ICommand _onTourCLick;
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

        public ICommand OnTourCLick =>
            _onTourCLick ?? (_onTourCLick = new Command<ITour>(
                tour => ShowViewModel<TourViewModel>(new {name = tour.Name, filename = tour.FileName})));

        public DataLoader DataLoader { get; protected set; }

        public virtual Task RefreshAsync()
        {
            return DataLoader.LoadItemsAsync(LoadItems);
        }

        protected abstract Task LoadItems();
    }
}