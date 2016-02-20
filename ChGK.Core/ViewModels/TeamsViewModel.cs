using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using ChGK.Core.Models;
using ChGK.Core.Services;
using ChGK.Core.Services.Messenger;
using ChGK.Core.Services.Teams;
using ChGK.Core.Utils;
using MvvmCross.Core.ViewModels;

namespace ChGK.Core.ViewModels
{
    internal class ChGKCommand
    {
        public Action OnUndo { get; set; }

        public Action OnApply { get; set; }
    }

    public class TeamsViewModel : MenuItemViewModel
    {
        private readonly IDialogManager _dialogManager;
        private readonly IGAService _gaService;
        private readonly IMessagesService _messagesService;
        private readonly ITeamsService _service;

        private readonly List<ChGKCommand> _undoActions = new List<ChGKCommand>();

        private ICommand _removeCommand;

        private List<TeamViewModel> _teams;

        private UndoBarMetadata _undoBarMetadata;

        public TeamsViewModel(ITeamsService service,
            IMessagesService messagesService,
            IGAService gaService,
            IDialogManager dialogManager)
        {
            _service = service;
            _messagesService = messagesService;
            _gaService = gaService;
            _dialogManager = dialogManager;

            Title = StringResources.Teams;

            DataLoader = new DataLoader();
        }

        public DataLoader DataLoader { get; set; }

        public List<TeamViewModel> Teams
        {
            get { return _teams; }
            set
            {
                _teams = value;
                RaisePropertyChanged(() => Teams);
                RaisePropertyChanged(() => HasNoTeams);

                RaisePropertyChanged(() => CanClearScore);
                RaisePropertyChanged(() => CanRemoveTeams);
            }
        }

        public ICommand RemoveTeamCommand =>
            _removeCommand ?? (_removeCommand = new Command<object>(RemoveTeam));

        public bool HasNoTeams => Teams.Count == 0;

        public bool CanClearScore => Teams?.FirstOrDefault(team => team.Score > 0) != null;

        public bool CanRemoveTeams => !HasNoTeams;

        public UndoBarMetadata UndoBarMetaData
        {
            get { return _undoBarMetadata; }
            set
            {
                _undoBarMetadata = value;
                RaisePropertyChanged(() => UndoBarMetaData);
            }
        }

        public override void Start()
        {
            base.Start();

            LoadTeams();
        }

        private void LoadTeams()
        {
            Teams = null;
            var teams = _service.GetAllTeams();
            Teams = teams.Select(team => new TeamViewModel(_service, _messagesService, team)).ToList();
        }

        public void InitAddTeam()
        {
            _dialogManager.ShowDialog(DialogType.AddTeamDialog,
                new Command<string>(AddTeam, name => !string.IsNullOrWhiteSpace(name)));
        }

        private void AddTeam(string name)
        {
            _service.AddTeam(name.Trim());

            _gaService.ReportEvent(GACategory.DealWithTeams, GAAction.Click, "team added");

            LoadTeams();
        }

        private void RemoveTeam(object parameter)
        {
            lock (_undoActions)
            {
                var position = ((int[]) parameter)[0];
                var teamToDelete = Teams[position];

                _undoActions.Add(
                    new ChGKCommand
                    {
                        OnApply = () =>
                        {
                            _service.RemoveTeam(teamToDelete.ID);

                            _gaService.ReportEvent(GACategory.DealWithTeams, GAAction.Click, "team removed");
                        },
                        OnUndo = () =>
                        {
                            Teams.Insert(position, teamToDelete);
                            Teams = Teams.Where(_ => true).ToList();
                        }
                    });

                Teams = Teams.Where((m, i) => i != position).ToList();
            }

            UndoBarMetaData = new UndoBarMetadata {Text = StringResources.TeamRemoved};
        }

        public void ClearResults()
        {
            lock (_undoActions)
            {
                var oldTeamScores = Teams.Select(team => team.Score).ToList();

                _undoActions.Add(
                    new ChGKCommand
                    {
                        OnApply = () =>
                        {
                            _service.CleanResults();

                            _gaService.ReportEvent(GACategory.DealWithTeams, GAAction.Click, "results cleaned");
                        },
                        OnUndo = () =>
                        {
                            Teams = Teams.Where((team, i) =>
                            {
                                try
                                {
                                    team.Score = oldTeamScores[i];
                                }
                                catch
                                {
                                }

                                return true;
                            }).ToList();
                        }
                    });

                Teams = Teams.Where(team =>
                {
                    team.Score = 0;
                    return true;
                }).ToList();
            }

            UndoBarMetaData = new UndoBarMetadata {Text = StringResources.ScoreRemoved};
        }

        public void ClearTeams()
        {
            lock (_undoActions)
            {
                _undoActions.Add(
                    new ChGKCommand
                    {
                        OnApply = () =>
                        {
                            _service.RemoveAllTeams();

                            _gaService.ReportEvent(GACategory.DealWithTeams, GAAction.Click, "all teams removed");
                        },
                        OnUndo = () => { LoadTeams(); }
                    });

                Teams = new List<TeamViewModel>();
            }

            UndoBarMetaData = new UndoBarMetadata {Text = StringResources.TeamsRemoved};
        }

        public void UndoableActionUndone()
        {
            lock (_undoActions)
            {
                if (!_undoActions.Any())
                    return;

                var action = _undoActions[0];
                action.OnUndo();
                _undoActions.RemoveAt(0);
            }
        }

        public void UndoableActionConfirmed()
        {
            lock (_undoActions)
            {
                if (!_undoActions.Any())
                    return;

                var action = _undoActions[0];
                action.OnApply();
                _undoActions.RemoveAt(0);
            }
        }
    }

    public class TeamViewModel : MvxViewModel
    {
#pragma warning disable 414
        private readonly object _resultsChangedToken;
#pragma warning restore 414

        private readonly ITeamsService _service;
        private int _score;

        public TeamViewModel(ITeamsService service,
            IMessagesService messagesService,
            Team team)
        {
            _service = service;

            _resultsChangedToken = messagesService.Subscribe<ResultsChangedMessage>(OnResultsChanged);

            ID = team.Id;
            Name = team.Name;

            Score = _service.GetTeamScore(ID);
        }

        public int ID { get; set; }

        public string Name { get; private set; }

        public string TeamScore => StringResources.TeamScoreTitle + " " + Score;

        public int Score
        {
            get { return _score; }
            set
            {
                _score = value;
                RaisePropertyChanged(() => TeamScore);
            }
        }

        private void OnResultsChanged(ResultsChangedMessage message)
        {
            Score = message.QuestionId.Equals(ResultsChangedMessage.ResultsCleared) ? 0 : _service.GetTeamScore(ID);
        }
    }
}