using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.Community.Plugins.Sqlite;
using Cirrious.MvvmCross.Plugins.Messenger;
using ChGK.Core.Messages;
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
		readonly IMvxMessenger _messenger;

		public TeamsService (ISQLiteConnectionFactory factory, IMvxMessenger messenger)
		{
			_messenger = messenger;

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

			_messenger.Publish (new TeamsChangedMessage (this));
		}

		public void RemoveTeam (Team team)
		{
			_connection.Delete (team);

			_messenger.Publish (new TeamsChangedMessage (this));
		}

		public void IncrementScore (string questionId, int teamId)
		{
			var existingAnswer = _connection.Table<Answer> ().FirstOrDefault (answer => answer.QuestionId.Equals (questionId) && answer.TeamID == teamId);
			if (existingAnswer == null) {
				_connection.Insert (new Answer{ QuestionId = questionId, TeamID = teamId });

				_messenger.Publish (new ResultsChangedMessage (this, questionId));
			}
		}

		public void DecrementScore (string questionId, int teamId)
		{
			var answersToDelete = _connection.Table<Answer> ().Where (answer => answer.QuestionId.Equals (questionId) && answer.TeamID == teamId).ToList ();
			foreach (var answer in answersToDelete) {
				_connection.Delete (answer);
			}

			if (answersToDelete.Count != 0) {
				_messenger.Publish (new ResultsChangedMessage (this, questionId));
			}
		}

		public void CleanResults ()
		{
			_connection.DeleteAll<Answer> ();

			_messenger.Publish (new ResultsChangedMessage (this, ResultsChangedMessage.ResultsCleared));
		}

		public void RemoveAllTeams ()
		{
			CleanResults ();

			_connection.DeleteAll<Team> ();

			_messenger.Publish (new TeamsChangedMessage (this));
		}

		public int GetTeamScore (Team team)
		{
			return _connection.Table<Answer> ().Where (answer => answer.TeamID == team.ID).Count ();
		}

		public List<int> GetAllResults (string questionId)
		{
			return _connection.Table<Answer> ().Where (a => a.QuestionId.Equals (questionId)).ToList ().Select (a => a.TeamID).ToList ();
		}
	}
}