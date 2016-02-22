using System.Collections.Generic;
using System.Linq;
using ChGK.Core.Models.Database;
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

            _databaseService.CreateTable<TeamDatabaseModel>();
            _databaseService.CreateTable<AnswerDatabaseModel>();
        }

        public List<TeamDatabaseModel> GetAllTeams()
        {
            return _databaseService.GetAll<TeamDatabaseModel>().ToList();
        }

        public void AddTeam(string name)
        {
            _databaseService.Insert(new TeamDatabaseModel {Name = name});

            _messenger.Publish(new TeamsChangedMessage(this));
        }

        public void RemoveTeam(int teamId)
        {
            _databaseService.Delete<TeamDatabaseModel>(teamId);

            _messenger.Publish(new TeamsChangedMessage(this));
        }

        public void IncrementScore(string questionId, int teamId)
        {
            var existingAnswer = _databaseService.GetAll<AnswerDatabaseModel>()
                .FirstOrDefault(answer => answer.QuestionId.Equals(questionId) && answer.TeamId == teamId);

            if (existingAnswer != null) return;

            _databaseService.Insert(new AnswerDatabaseModel {QuestionId = questionId, TeamId = teamId});

            _messenger.Publish(new ResultsChangedMessage(this, questionId));
        }

        public void DecrementScore(string questionId, int teamId)
        {
            var answersToDelete = _databaseService.GetAll<AnswerDatabaseModel>()
                .Where(answer => answer.QuestionId.Equals(questionId) && answer.TeamId == teamId)
                .ToList();

            foreach (var answer in answersToDelete)
                _databaseService.Delete(answer);

            if (answersToDelete.Any())
                _messenger.Publish(new ResultsChangedMessage(this, questionId));
        }

        public void CleanResults()
        {
            _databaseService.DeleteAll<AnswerDatabaseModel>();

            _messenger.Publish(new ResultsChangedMessage(this, ResultsChangedMessage.ResultsCleared));
        }

        public void RemoveAllTeams()
        {
            CleanResults();

            _databaseService.DeleteAll<TeamDatabaseModel>();

            _messenger.Publish(new TeamsChangedMessage(this));
        }

        public int GetTeamScore(int teamId)
        {
            return _databaseService.GetAll<AnswerDatabaseModel>().Count(answer => answer.TeamId == teamId);
        }

        public List<int> GetAllResults(string questionId)
        {
            return _databaseService.GetAll<AnswerDatabaseModel>()
                .Where(a => a.QuestionId.Equals(questionId))
                .Select(a => a.TeamId)
                .ToList();
        }
    }
}