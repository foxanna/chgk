using System.Collections.Generic;
using System.Threading.Tasks;
using ChGK.Core.Models;
using ChGK.Core.Utils;
using MvvmCross.Core.ViewModels;

namespace ChGK.Core.ViewModels
{
    public abstract class TournamentsViewModel : MenuItemViewModel
    {
        private List<TournamentViewModel> _tournaments;

        private MvxCommand<ITour> onTourCLick;

        public List<TournamentViewModel> Tournaments
        {
            get { return _tournaments; }
            set
            {
                _tournaments = value;
                RaisePropertyChanged(() => Tournaments);
            }
        }

        public MvxCommand<ITour> OnTourCLick
        {
            get
            {
                return onTourCLick ?? (onTourCLick = new MvxCommand<ITour>(
                    tour => ShowViewModel<TourViewModel>(new {name = tour.Name, filename = tour.FileName})));
            }
        }

        public DataLoader DataLoader { get; protected set; }

        public virtual async Task Refresh()
        {
            await DataLoader.LoadItemsAsync(LoadItems);
        }

        protected abstract Task LoadItems();
    }
}