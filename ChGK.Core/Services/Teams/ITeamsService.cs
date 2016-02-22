using System.Collections.Generic;
using ChGK.Core.Models.Database;

namespace ChGK.Core.Services.Teams
{
    public interface ITeamsService
    {
        List<TeamDatabaseModel> GetAllTeams();

        List<int> GetAllResults(string questionId);

        void AddTeam(string name);

        void RemoveTeam(int teamId);

        void CleanResults();

        void RemoveAllTeams();

        void IncrementScore(string questionId, int teamId);

        void DecrementScore(string questionId, int teamId);

        int GetTeamScore(int teamId);
    }
}