using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;
using ChGK.Core.Messages;
using ChGK.Core.Models;
using ChGK.Core.Services;
using ChGK.Core.Utils;

namespace ChGK.Core.ViewModels
{
    class ChGKCommand
    {
        public Action OnUndo { get; set; }

        public Action OnApply { get; set; }
    }

    public class TeamsViewModel : MenuItemViewModel
    {
        readonly ITeamsService _service;

        public DataLoader DataLoader { get; set; }
        
        public TeamsViewModel(ITeamsService service, IMvxMessenger messenger)
        {
            _service = service;

            Title = StringResources.Teams;

            DataLoader = new DataLoader();
        }

        public override void Start()
        {
            base.Start();

            LoadItems();
        }

        void LoadItems()
        {
            Teams = null;
            var teams = _service.GetAllTeams();
            Teams = teams.Select(team => new TeamViewModel(_service, team)).ToList();
        }

        List<TeamViewModel> _teams;

        public List<TeamViewModel> Teams
        {
            get
            {
                return _teams;
            }
            set
            {
                _teams = value;
                RaisePropertyChanged(() => Teams);
                RaisePropertyChanged(() => HasNoTeams);
            }
        }

        public void InitAddTeam()
        {
            Mvx.Resolve<IDialogManager>().ShowDialog(DialogType.AddTeamDialog,
                new MvxCommand<string>(AddTeam, name => !string.IsNullOrWhiteSpace(name)));
        }

        void AddTeam(string name)
        {
            _service.AddTeam(name.Trim());

            LoadItems();
        }

        public override Task Refresh()
        {
            throw new NotImplementedException();
        }

        MvxCommand<object> _removeCommand;

        public MvxCommand<object> RemoveTeamCommand
        {
            get
            {
                return _removeCommand ?? (_removeCommand = new MvxCommand<object>(RemoveTeam, _ => true));
            }
        }

        void RemoveTeam(object parameter)
        {
            lock (UndoActions)
            {
                var position = ((int[])parameter)[0];
                var teamToDelete = Teams[position];

                UndoActions.Add(
                    new ChGKCommand
                    {
                        OnApply = () =>
                        {
                            _service.RemoveTeam(teamToDelete.ID);
                        },
                        OnUndo = () =>
                        {
                            Teams.Insert(position, teamToDelete);
                            Teams = Teams.Where(_ => true).ToList();
                        },
                    });

                Teams = Teams.Where((m, i) => i != position).ToList();                
            }

            UndoBarMetaData = new UndoBarMetadata { Text = StringResources.TeamRemoved };
        }

        readonly List<ChGKCommand> UndoActions = new List<ChGKCommand>();

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
                       },
                       OnUndo = () =>
                       {
                           Teams = Teams.Where((team, i) =>
                           {
                               try
                               {
                                   team.Score = oldTeamScores[i];
                               } catch  {

                               };

                               return true;
                           }).ToList();
                       },
                   });

                Teams = Teams.Where(team =>
                {
                    team.Score = 0;
                    return true;
                }).ToList();
            }

            UndoBarMetaData = new UndoBarMetadata { Text = StringResources.ScoreRemoved };
        }
        
        public bool HasNoTeams
        {
            get
            {
                return Teams.Count == 0;
            }
        }

        UndoBarMetadata _undoBarMetadata;

        public UndoBarMetadata UndoBarMetaData
        {
            get
            {
                return _undoBarMetadata;
            }
            set
            {
                _undoBarMetadata = value;
                RaisePropertyChanged(() => UndoBarMetaData);
            }
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
        readonly MvxSubscriptionToken _resultsChangedToken;
#pragma warning restore 414

        readonly ITeamsService _service;
        int _score;

        public TeamViewModel(ITeamsService service, Team team)
        {
            _service = service;

            _resultsChangedToken = Mvx.Resolve<IMvxMessenger>().Subscribe<ResultsChangedMessage>(OnResultsChanged);

            ID = team.ID;
            Name = team.Name;

            Score = _service.GetTeamScore(ID);
        }

        void OnResultsChanged(ResultsChangedMessage message)
        {
            Score = message.QuestionID.Equals(ResultsChangedMessage.ResultsCleared) ? 0 : _service.GetTeamScore(ID);
        }

        public int ID { get; set; }

        public string Name { get; private set; }

        public string TeamScore
        {
            get { return StringResources.TeamScoreTitle + " " + Score; }
        }

        public int Score
        {
            get
            {
                return _score;
            }
            set
            {
                _score = value;
                RaisePropertyChanged(() => TeamScore);
            }
        }
    }
}

