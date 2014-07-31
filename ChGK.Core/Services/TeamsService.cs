using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.Community.Plugins.Sqlite;
using ChGK.Core.Models;

namespace ChGK.Core.Services
{
	class Answer
	{
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }

		public string QuestionId{ get; set; }

		public int TeamID { get; set; }
	}

	public class TeamsService : ITeamsService
	{
		readonly ISQLiteConnection _connection;

		public TeamsService (ISQLiteConnectionFactory factory)
		{
			_connection = factory.Create ("teams.sql");

			_connection.CreateTable<Team> ();
			_connection.CreateTable<Answer> ();
		}

		public List<Team> GetAllTeams ()
		{
			return _connection.Table<Team> ().ToList ();
		}

		public void AddTeam (string name)
		{
			_connection.Insert (new Team { Name = name });
		}

		public void RemoveTeam (Team team)
		{
			_connection.Delete (team);
		}

		public void IncrementScore (IQuestion question, Team team)
		{
			var existingAnswer = _connection.Table<Answer> ().FirstOrDefault (answer => answer.QuestionId == question.ID && answer.TeamID == team.ID);
			if (existingAnswer == null) {
				_connection.Insert (new Answer{ QuestionId = question.ID, TeamID = team.ID });
			}
		}

		public void DecrementScore (IQuestion question, Team team)
		{
			var answersToDelete = _connection.Table<Answer> ().Where (answer => answer.QuestionId == question.ID && answer.TeamID == team.ID).ToList ();
			foreach (var answer in answersToDelete) {
				_connection.Delete (answer);
			}
		}

		public void CleanResults ()
		{
			_connection.DeleteAll<Answer> ();
		}

		public void RemoveAllTeams ()
		{
			CleanResults ();

			_connection.DeleteAll<Team> ();
		}

		public int GetTeamScore (Team team)
		{
			return _connection.Table<Answer> ().Where (answer => answer.TeamID == team.ID).Count ();
		}
	}
}

