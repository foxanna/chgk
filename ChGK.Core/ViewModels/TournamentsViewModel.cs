using ChGK.Core.Models;
using ChGK.Core.Utils;
using Cirrious.MvvmCross.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChGK.Core.ViewModels
{
    public abstract class TournamentsViewModel : MenuItemViewModel
    {
        List<TournamentViewModel> _tournaments;

        public List<TournamentViewModel> Tournaments
        {
            get
            {
                return _tournaments;
            }
            set
            {
                _tournaments = value;
                RaisePropertyChanged(() => Tournaments);
            }
        }

        MvxCommand<ITour> onTourCLick;

        public MvxCommand<ITour> OnTourCLick
        {
            get
            {
                return onTourCLick ?? (onTourCLick = new MvxCommand<ITour>(
                    tour => ShowViewModel<TourViewModel>(new { name = tour.Name, filename = tour.FileName })));
            }
        }
        
        public virtual async Task Refresh()
        {
            await DataLoader.LoadItemsAsync(LoadItems);
        }

        protected abstract Task LoadItems();

        public DataLoader DataLoader { get; protected set; }
    }
}
