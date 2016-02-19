using System;
using System.Collections.Generic;
using System.Linq;
using ChGK.Core.Messages;
using ChGK.Core.Models;
using ChGK.Core.Services;
using ChGK.Core.Utils;
using ChGK.Core.ViewModels.Tutorials;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;

namespace ChGK.Core.ViewModels
{
    internal class ChGKCommand
    {
        public Action OnUndo { get; set; }

        public Action OnApply { get; set; }
    }

    public class TeamsViewModel : MenuItemViewModel
    {
        private readonly IFirstViewStartInfoProvider _firstViewStartInfoProvider;
        private readonly IGAService _gaService;
        private readonly ITeamsService _service;

        private readonly List<ChGKCommand> UndoActions = new List<ChGKCommand>();

        private MvxCommand<object> _removeCommand;

        private List<TeamViewModel> _teams;

        private UndoBarMetadata _undoBarMetadata;

        public TeamsViewModel(ITeamsService service, IMvxMessenger messenger,
            IFirstViewStartInfoProvider firstViewStartInfoProvider, IGAService gaService)
        {
            _service = service;
            _firstViewStartInfoProvider = firstViewStartInfoProvider;
            _gaService = gaService;

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

        public MvxCommand<object> RemoveTeamCommand
        {
            get { return _removeCommand ?? (_removeCommand = new MvxCommand<object>(RemoveTeam, _ => true)); }
        }

        public bool HasNoTeams
        {
            get { return Teams.Count == 0; }
        }

        public bool CanClearScore
        {
            get { return Teams != null && Teams.FirstOrDefault(team => team.Score > 0) != null; }
        }

        public bool CanRemoveTeams
        {
            get { return !HasNoTeams; }
        }

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

            if (_firstViewStartInfoProvider.IsSeenForTheFirstTime(GetType()))
            {
                ShowViewModel(new FirstTimeSeenViewModelsFactory().CreateViewModel(GetType()));
                _firstViewStartInfoProvider.SetSeen(GetType());
            }
        }

        private void LoadTeams()
        {
            Teams = null;
            var teams = _service.GetAllTeams();
            Teams = teams.Select(team => new TeamViewModel(_service, team)).ToList();
        }

        public void InitAddTeam()
        {
            Mvx.Resolve<IDialogManager>().ShowDialog(DialogType.AddTeamDialog,
                new MvxCommand<string>(AddTeam, name => !string.IsNullOrWhiteSpace(name)));
        }

        private void AddTeam(string name)
        {
            _service.AddTeam(name.Trim());

            _gaService.ReportEvent(GACategory.DealWithTeams, GAAction.Click, "team added");

            LoadTeams();
        }

        private void RemoveTeam(object parameter)
        {
            lock (UndoActions)
            {
                var position = ((int[]) parameter)[0];
                var teamToDelete = Teams[position];

                UndoActions.Add(
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
            lock (UndoActions)
            {
                var oldTeamScores = Teams.Select(team => team.Score).ToList();

                UndoActions.Add(
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
                                ;

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
            lock (UndoActions)
            {
                UndoActions.Add(
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
            if (UndoActions.Count > 0)
            {
                lock (UndoActions)
                {
                    var action = UndoActions[0];
                    action.OnUndo();
                    UndoActions.RemoveAt(0);
                }
            }
        }

        public void UndoableActionConfirmed()
        {
            if (UndoActions.Count > 0)
            {
                lock (UndoActions)
                {
                    var action = UndoActions[0];
                    action.OnApply();
                    UndoActions.RemoveAt(0);
                }
            }
        }
    }

    public class TeamViewModel : MvxViewModel
    {
#pragma warning disable 414
        private readonly MvxSubscriptionToken _resultsChangedToken;
#pragma warning restore 414

        private readonly ITeamsService _service;
        private int _score;

        public TeamViewModel(ITeamsService service, Team team)
        {
            _service = service;

            _resultsChangedToken = Mvx.Resolve<IMvxMessenger>().Subscribe<ResultsChangedMessage>(OnResultsChanged);

            ID = team.ID;
            Name = team.Name;

            Score = _service.GetTeamScore(ID);
        }

        public int ID { get; set; }

        public string Name { get; private set; }

        public string TeamScore
        {
            get { return StringResources.TeamScoreTitle + " " + Score; }
        }

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
            Score = message.QuestionID.Equals(ResultsChangedMessage.ResultsCleared) ? 0 : _service.GetTeamScore(ID);
        }
    }
}