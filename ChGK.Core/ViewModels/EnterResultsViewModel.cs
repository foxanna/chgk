using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using ChGK.Core.Services;
using ChGK.Core.Services.Messenger;
using ChGK.Core.Services.Teams;
using ChGK.Core.Utils;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

namespace ChGK.Core.ViewModels
{
    public class EnterResultsViewModel : MvxViewModel, IViewLifecycle
    {
        private readonly IGAService _gaService;
        private readonly IMessagesService _messenger;
        private readonly ITeamsService _teamsService;

        private ICommand _clearResultsCommand, _editTeamsCommand;

        private bool _isEmpty;
        private string _questionId;
        private List<ResultsTeamViewModel> _teams;

#pragma warning disable 414
        private object _teamsChangedToken, _resultsChangedToken;
#pragma warning restore 414

        public EnterResultsViewModel(ITeamsService service,
            IMessagesService messenger,
            IGAService gaService)
        {
            _teamsService = service;
            _messenger = messenger;
            _gaService = gaService;

            _teamsChangedToken = messenger.Subscribe<TeamsChangedMessage>(OnTeamsChanged);
            _resultsChangedToken = messenger.Subscribe<ResultsChangedMessage>(OnResultsChanged);

            DataLoader = new DataLoader();
        }

        public DataLoader DataLoader { get; set; }

        public bool IsEmpty
        {
            get { return _isEmpty; }
            set
            {
                _isEmpty = value;
                RaisePropertyChanged(() => IsEmpty);
            }
        }

        public List<ResultsTeamViewModel> Teams
        {
            get { return _teams; }
            set
            {
                _teams = value;
                RaisePropertyChanged(() => Teams);
            }
        }

        public ICommand ClearResultsCommand
            => _clearResultsCommand ?? (_clearResultsCommand = new Command(ClearResults));

        public ICommand EditTeamsCommand =>
            _editTeamsCommand ?? (_editTeamsCommand = new Command(EditTeams));

        public void OnViewDestroying()
        {
            if (_resultsChangedToken != null)
            {
                _messenger.Unsubscribe<ResultsChangedMessage>(_resultsChangedToken);
                _resultsChangedToken = null;
            }
            if (_teamsChangedToken != null)
            {
                _messenger.Unsubscribe<TeamsChangedMessage>(_teamsChangedToken);
                _teamsChangedToken = null;
            }
        }

        private void EditTeams()
        {
            _gaService.ReportEvent(GACategory.DealWithTeams, GAAction.Open, "edit teams from enter results screen");
            ShowViewModel<TeamsViewModel>();
        }

        private void LoadItems()
        {
            Teams = null;

            var teams = _teamsService.GetAllTeams();
            var results = _teamsService.GetAllResults(_questionId);

            Teams = teams.Select(team => new ResultsTeamViewModel
            {
                Name = team.Name,
                ID = team.Id,
                AnsweredCorrectly = results.Contains(team.Id)
            }).ToList();

            IsEmpty = Teams.Count == 0;
        }

        private void ReloadResults()
        {
            DataLoader.LoadItems(LoadItems);
        }

        public void Init(string questionId)
        {
            _questionId = questionId;

            ReloadResults();
        }

        private void OnTeamsChanged(TeamsChangedMessage obj)
        {
            ReloadResults();
        }

        private void OnResultsChanged(ResultsChangedMessage obj)
        {
            if (_questionId.Equals(obj.QuestionId))
            {
                ReloadResults();
            }
        }

        public void SubmitResults()
        {
            OnViewDestroying();

            try
            {
                foreach (var team in Teams.Where(t => t.AnsweredCorrectly))
                {
                    _teamsService.IncrementScore(_questionId, team.ID);
                }
                foreach (var team in Teams.Where(t => !t.AnsweredCorrectly))
                {
                    _teamsService.DecrementScore(_questionId, team.ID);
                }
            }
            catch (Exception e)
            {
                Mvx.Trace(e.Message);
            }

            _gaService.ReportEvent(GACategory.PlayQuestion, GAAction.Click, "results submitted");

            Close(this);
        }

        private void ClearResults()
        {
            _teamsService.CleanResults();
        }
    }

    public class ResultsTeamViewModel : MvxViewModel
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public bool AnsweredCorrectly { get; set; }
    }
}