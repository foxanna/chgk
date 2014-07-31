using System.Collections.Generic;
using ChGK.Core.Models;

namespace ChGK.Core.Services
{
	public interface ITeamsService
	{
		List<Team> GetAllTeams ();

		void AddTeam (string name);

		void RemoveTeam (Team team);

		void CleanResults ();

		void RemoveAllTeams ();

		void IncrementScore (IQuestion question, Team team);

		void DecrementScore (IQuestion question, Team team);

		int GetTeamScore (Team team);
	}
}

