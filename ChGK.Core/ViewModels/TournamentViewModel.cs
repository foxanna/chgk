using Cirrious.MvvmCross.ViewModels;
using ChGK.Core.Models;

namespace ChGK.Core.ViewModels
{
	public class TournamentViewModel: MvxViewModel
	{
		public TournamentViewModel (ITournament tournament)
		{
			Name = tournament.Name;
			FileName = tournament.FileName;
			Dates = string.Format ("Отыгран {0}  Добавлен {1}", tournament.PlayedAt, tournament.AddedAt);
		}

		public string Name { get; set; }

		public string FileName { get; set; }

		public string Dates { get; set; }
	}
}

