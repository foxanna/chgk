using Cirrious.MvvmCross.ViewModels;
using ChGK.Core.Models;
using System.Collections.Generic;
using System.Collections;

namespace ChGK.Core.ViewModels
{
	public class TournamentViewModel : MvxViewModel, IEnumerable<ITour>
	{
		List<ITour> Tours { get; set; }

		public TournamentViewModel (ITournament tournament)
		{
			Name = tournament.Name;
			FileName = tournament.FileName;
            Dates = string.Format("Отыгран&nbsp;{0}  Добавлен&nbsp;{1}", tournament.PlayedAt.Replace(" ", "&nbsp;"), tournament.AddedAt);
			Tours = tournament.Tours;
		}

		public string Name { get; set; }

		public string FileName { get; set; }

		public string Dates { get; set; }

		public IEnumerator<ITour> GetEnumerator ()
		{
			return Tours.GetEnumerator ();
		}

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return Tours.GetEnumerator ();
		}
	}
}

