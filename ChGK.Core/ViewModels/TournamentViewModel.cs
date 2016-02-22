using System.Collections;
using System.Collections.Generic;
using ChGK.Core.Models;
using ChGK.Core.Services.Favorites;

namespace ChGK.Core.ViewModels
{
    public class TournamentViewModel : LoadMoreOnScrollListViewItemViewModel<ITournament>, IEnumerable<ITour>
    {
        private readonly IFavoritesService _favoritesService;

        public TournamentViewModel(IFavoritesService favoritesService,
            ITournament tournament)
        {
            _favoritesService = favoritesService;
            Item = tournament;

            Name = tournament.Name;
            Dates = $"Отыгран&nbsp;{tournament.PlayedAt.Replace(" ", "&nbsp;")}  Добавлен&nbsp;{tournament.AddedAt}";
            Tours = tournament.Tours;
        }

        private List<ITour> Tours { get; }

        public string Name { get; set; }

        public string Dates { get; set; }

        public bool IsFavorite
        {
            get { return _favoritesService.IsTournamentFavorite(Item); }
            set { _favoritesService.SetTournamentFavourite(Item, value); }
        }

        public IEnumerator<ITour> GetEnumerator()
        {
            return Tours.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Tours.GetEnumerator();
        }
    }
}