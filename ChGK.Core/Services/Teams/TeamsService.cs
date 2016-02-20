using System.Collections.Generic;
using System.Linq;
using ChGK.Core.Models;
using ChGK.Core.Services.Database;
using ChGK.Core.Services.Messenger;

namespace ChGK.Core.Services.Teams
{
    public class TeamsService : ITeamsService
    {
        private readonly IDatabaseService _databaseService;
        private readonly IMessagesService _messenger;

        public TeamsService(IDatabaseService databaseService,
            IMessagesService messenger)
        {
            _databaseService = databaseService;
            _messenger = messenger;

            _databaseService.CreateTable<Team>();
            _databaseService.CreateTable<Answer>();
        }

        public List<Team> GetAllTeams()
        {
            return _databaseService.GetAll<Team>().ToList();
        }

        public void AddTeam(string name)
        {
            _databaseService.Insert(new Team {Name = name});

            _messenger.Publish(new TeamsChangedMessage(this));
        }

        public void RemoveTeam(int teamId)
        {
            _databaseService.Delete<Team>(teamId);

            _messenger.Publish(new TeamsChangedMessage(this));
        }

        public void IncrementScore(string questionId, int teamId)
        {
            var existingAnswer = _databaseService.GetAll<Answer>()
                .FirstOrDefault(answer => answer.QuestionId.Equals(questionId) && answer.TeamId == teamId);

            if (existingAnswer != null) return;

            _databaseService.Insert(new Answer {QuestionId = questionId, TeamId = teamId});

            _messenger.Publish(new ResultsChangedMessage(this, questionId));
        }

        public void DecrementScore(string questionId, int teamId)
        {
            var answersToDelete = _databaseService.GetAll<Answer>()
                .Where(answer => answer.QuestionId.Equals(questionId) && answer.TeamId == teamId)
                .ToList();

            foreach (var answer in answersToDelete)
                _databaseService.Delete(answer);

            if (answersToDelete.Any())
                _messenger.Publish(new ResultsChangedMessage(this, questionId));
        }

        public void CleanResults()
        {
            _databaseService.DeleteAll<Answer>();

            _messenger.Publish(new ResultsChangedMessage(this, ResultsChangedMessage.ResultsCleared));
        }

        public void RemoveAllTeams()
        {
            CleanResults();

            _databaseService.DeleteAll<Team>();

            _messenger.Publish(new TeamsChangedMessage(this));
        }

        public int GetTeamScore(int teamId)
        {
            return _databaseService.GetAll<Answer>().Count(answer => answer.TeamId == teamId);
        }

        public List<int> GetAllResults(string questionId)
        {
            return _databaseService.GetAll<Answer>()
                .Where(a => a.QuestionId.Equals(questionId))
                .Select(a => a.TeamId)
                .ToList();
        }
    }
}