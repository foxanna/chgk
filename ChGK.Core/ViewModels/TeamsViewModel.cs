using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ChGK.Core.Models.Database;
using ChGK.Core.Services;
using ChGK.Core.Services.Messenger;
using ChGK.Core.Services.Teams;
using ChGK.Core.Utils;

namespace ChGK.Core.ViewModels
{
    public class TeamsViewModel : MenuItemViewModel
    {
        private readonly IDialogManager _dialogManager;
        private readonly IMessagesService _messagesService;
        private readonly ITeamsService _service;

        private ICommand _removeCommand;

        private ObservableCollection<TeamViewModel> _teams;

        public TeamsViewModel(ITeamsService service,
            IMessagesService messagesService,
            IDialogManager dialogManager)
        {
            _service = service;
            _messagesService = messagesService;
            _dialogManager = dialogManager;

            Title = StringResources.Teams;
        }

        public ObservableCollection<TeamViewModel> Teams
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

        public bool HasNoTeams => !Teams.Any();

        public bool CanClearScore => Teams.Any(team => team.Score > 0);

        public bool CanRemoveTeams => !HasNoTeams;

        public override void Start()
        {
            base.Start();

            LoadTeams();
        }

        private void LoadTeams()
        {
            Teams = new ObservableCollection<TeamViewModel>();
            var teams = _service.GetAllTeams();
            Teams = new ObservableCollection<TeamViewModel>(
                teams.Select(team => new TeamViewModel(_service, _messagesService, team)));
        }

        public void InitAddTeam()
        {
            _dialogManager.ShowDialog(DialogType.AddTeamDialog,
                new Command<string>(AddTeam, name => !string.IsNullOrWhiteSpace(name)));
        }

        private void AddTeam(string name)
        {
            _service.AddTeam(name.Trim());

            LoadTeams();
        }

        private void RemoveTeam(object parameter)
        {
            var position = ((int[]) parameter)[0];
            var teamToDelete = Teams[position];

            Teams.RemoveAt(position);

            UndoBarRequested?.Invoke(this, new UndoBarRequestEventArgs(
                StringResources.TeamRemoved,
                () => _service.RemoveTeam(teamToDelete.ID),
                () => Teams.Insert(position, teamToDelete)));
        }

        public event EventHandler<UndoBarRequestEventArgs> UndoBarRequested;

        public void ClearResults()
        {
            var oldTeamScores = Teams.Select(team => team.Score).ToList();

            foreach (var team in Teams)
                team.Score = 0;

            UndoBarRequested?.Invoke(this, new UndoBarRequestEventArgs(
                StringResources.ScoreRemoved,
                () => _service.CleanResults(),
                () =>
                {
                    for (var i = 0; i < Teams.Count; i++)
                        Teams[i].Score = oldTeamScores[i];
                }));
        }

        public void ClearTeams()
        {
            Teams = new ObservableCollection<TeamViewModel>();

            UndoBarRequested?.Invoke(this, new UndoBarRequestEventArgs(
                StringResources.TeamsRemoved,
                () => _service.RemoveAllTeams(),
                LoadTeams));
        }
    }

    public class TeamViewModel : BaseViewModel
    {
#pragma warning disable 414
        private readonly object _resultsChangedToken;
#pragma warning restore 414

        private readonly ITeamsService _service;
        private int _score;

        public TeamViewModel(ITeamsService service,
            IMessagesService messagesService,
            TeamDatabaseModel teamDatabaseModel)
        {
            _service = service;

            _resultsChangedToken = messagesService.Subscribe<ResultsChangedMessage>(OnResultsChanged);

            ID = teamDatabaseModel.Id;
            Name = teamDatabaseModel.Name;

            Score = _service.GetTeamScore(ID);
        }

        public int ID { get; }

        public string Name { get; }

        public string TeamScore => $"{StringResources.TeamScoreTitle} {Score}";

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