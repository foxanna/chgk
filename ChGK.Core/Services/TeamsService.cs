using System.Collections.Generic;
using System.IO;
using System.Linq;
using ChGK.Core.Messages;
using ChGK.Core.Models;
using MvvmCross.Plugins.Messenger;
using SQLite.Net;
using SQLite.Net.Attributes;
using SQLite.Net.Interop;
using System;

namespace ChGK.Core.Services
{
    internal class Answer
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string QuestionId { get; set; }

        public int TeamId { get; set; }
    }

    public class TeamsService : ITeamsService
    {
        private readonly SQLiteConnection _connection;
        private readonly IMvxMessenger _messenger;

        public TeamsService(SQLiteConnection connection, IMvxMessenger messenger)
        {
            _messenger = messenger;
            _connection = connection;

            _connection.CreateTable<Team>();
            _connection.CreateTable<Answer>();
        }

        public List<Team> GetAllTeams()
        {
            return _connection.Table<Team>().ToList();
        }

        public void AddTeam(string name)
        {
            _connection.Insert(new Team {Name = name});

            _messenger.Publish(new TeamsChangedMessage(this));
        }

        public void RemoveTeam(int teamID)
        {
            _connection.Delete<Team>(teamID);

            _messenger.Publish(new TeamsChangedMessage(this));
        }

        public void IncrementScore(string questionId, int teamId)
        {
            var existingAnswer =
                _connection.Table<Answer>()
                    .FirstOrDefault(answer => answer.QuestionId.Equals(questionId) && answer.TeamId == teamId);
            if (existingAnswer == null)
            {
                _connection.Insert(new Answer {QuestionId = questionId, TeamId = teamId});

                _messenger.Publish(new ResultsChangedMessage(this, questionId));
            }
        }

        public void DecrementScore(string questionId, int teamId)
        {
            var answersToDelete =
                _connection.Table<Answer>()
                    .Where(answer => answer.QuestionId.Equals(questionId) && answer.TeamId == teamId)
                    .ToList();
            foreach (var answer in answersToDelete)
            {
                _connection.Delete(answer);
            }

            if (answersToDelete.Count != 0)
            {
                _messenger.Publish(new ResultsChangedMessage(this, questionId));
            }
        }

        public void CleanResults()
        {
            _connection.DeleteAll<Answer>();

            _messenger.Publish(new ResultsChangedMessage(this, ResultsChangedMessage.ResultsCleared));
        }

        public void RemoveAllTeams()
        {
            CleanResults();

            _connection.DeleteAll<Team>();

            _messenger.Publish(new TeamsChangedMessage(this));
        }

        public int GetTeamScore(int teamID)
        {
            return _connection.Table<Answer>().Where(answer => answer.TeamId == teamID).Count();
        }

        public List<int> GetAllResults(string questionId)
        {
            return
                _connection.Table<Answer>()
                    .Where(a => a.QuestionId.Equals(questionId))
                    .ToList()
                    .Select(a => a.TeamId)
                    .ToList();
        }
    }
}