using Cirrious.MvvmCross.ViewModels;
using ChGK.Core.Models;
using ChGK.Core.Services;

namespace ChGK.Core.ViewModels
{
	public class TeamViewModel : MvxViewModel
	{
		readonly ITeamsService _service;

		public TeamViewModel (ITeamsService service, Team team)
		{
			_service = service;

			Name = team.Name;
			Score = StringResources.TeamScoreTitle + " " + _service.GetTeamScore (team);
		}

		public string Name {
			get;
			private set;
		}

		public string Score {
			get;
			private set;
		}
	}
}

