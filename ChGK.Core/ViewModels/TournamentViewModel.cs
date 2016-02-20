using System.Collections;
using System.Collections.Generic;
using ChGK.Core.Models;

namespace ChGK.Core.ViewModels
{
    public class TournamentViewModel : LoadMoreOnScrollListViewItemViewModel<ITournament>, IEnumerable<ITour>
    {
        public TournamentViewModel(ITournament tournament)
        {
            Item = tournament;

            Name = tournament.Name;
            FileName = tournament.FileName;
            Dates = $"Отыгран&nbsp;{tournament.PlayedAt.Replace(" ", "&nbsp;")}  Добавлен&nbsp;{tournament.AddedAt}";
            Tours = tournament.Tours;
        }

        private List<ITour> Tours { get; }

        public string Name { get; set; }

        public string FileName { get; set; }

        public string Dates { get; set; }

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