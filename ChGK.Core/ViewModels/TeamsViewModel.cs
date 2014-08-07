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
	public class TeamsViewModel : MenuItemViewModel
	{
		readonly ITeamsService _service;

		public DataLoader DataLoader { get; set; }

		#pragma warning disable 414
		readonly MvxSubscriptionToken _teamsChangedToken;
		#pragma warning restore 414

		public TeamsViewModel (ITeamsService service, IMvxMessenger messenger)
		{
			_service = service;

			_teamsChangedToken = messenger.Subscribe<TeamsChangedMessage> (OnTeamsChanged);

			Title = StringResources.Teams;

			DataLoader = new DataLoader ();
		}

		public override void Start ()
		{
			base.Start ();

			ReloadTeams ();
		}

		async Task LoadItems ()
		{
			Teams = null;
			var teams = await Task.Factory.StartNew<List<Team>> (_service.GetAllTeams);
			Teams = teams.Select (team => new TeamViewModel (_service, team)).ToList ();
			HasNoTeams = Teams.Count == 0;
		}

		async void ReloadTeams ()
		{
			try {
				await DataLoader.LoadItemsAsync (LoadItems);
			} catch (Exception e) {
				Mvx.Trace (e.Message);
			}
		}

		void OnTeamsChanged (TeamsChangedMessage obj)
		{
			ReloadTeams ();
		}

		List<TeamViewModel> _teams;

		public List<TeamViewModel> Teams {
			get {			
				return _teams;
			}
			set {
				_teams = value; 
				RaisePropertyChanged (() => Teams);
			}
		}

		public void InitAddTeam ()
		{
			Mvx.Resolve<IDialogManager> ().ShowDialog (DialogType.AddTeamDialog, 
				new MvxCommand<string> (AddTeam, name => !string.IsNullOrWhiteSpace (name)));
		}

		void AddTeam (string name)
		{
			_service.AddTeam (name.Trim ());
		}

		public override Task Refresh ()
		{
			throw new NotImplementedException ();
		}

		MvxCommand<object> _removeCommand;

		public MvxCommand<object> RemoveTeamCommand {
			get {
				return _removeCommand ?? (_removeCommand = new MvxCommand<object> (RemoveTeam, _ => true));
			}
		}

		void RemoveTeam (object parameter)
		{
			lock (RemoveTeamActions) {
				var position = ((int[])parameter) [0];
				var id = Teams [position].ID;

				UndoBarMetaData = new UndoBarMetadata { Id = TeamRemoved, Text = StringResources.TeamRemoved };

				Teams = Teams.Where ((m, i) => i != position).ToList ();

			
				RemoveTeamActions.Add (() => _service.RemoveTeam (id));
			}
		}

		readonly List<Action> RemoveTeamActions = new List<Action> ();

		public void ClearResults ()
		{
			Teams = Teams.Select (team => {
				team.ClearScore ();
				return team;
			}).ToList ();

			UndoBarMetaData = new UndoBarMetadata { Id = ResultsCleared, Text = StringResources.ScoreRemoved };
		}

		bool _hasNoTeams;

		public bool HasNoTeams {
			get {
				return _hasNoTeams;
			}
			set {
				_hasNoTeams = value;
				RaisePropertyChanged (() => HasNoTeams);
			}
		}

		UndoBarMetadata _undoBarMetadata;

		public UndoBarMetadata UndoBarMetaData {
			get {
				return _undoBarMetadata;
			}
			set {
				_undoBarMetadata = value;
				RaisePropertyChanged (() => UndoBarMetaData);
			}
		}

		const int TeamRemoved = 1;
		const int ResultsCleared = 2;

		public void SomeThingUndone (int id)
		{
			ReloadTeams ();
		}

		public void SomeThingConfirmed (int id)
		{
			switch (id) {
			case TeamRemoved:
				if (RemoveTeamActions.Count > 0) {
					lock (RemoveTeamActions) {
						var action = RemoveTeamActions [0];
						action ();
						RemoveTeamActions.RemoveAt (0);
					}
				}
				break;
			case ResultsCleared: 
				_service.CleanResults ();
				break;
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

		public TeamViewModel (ITeamsService service, Team team)
		{
			_service = service;

			_resultsChangedToken = Mvx.Resolve<IMvxMessenger> ().Subscribe<ResultsChangedMessage> (OnResultsChanged);

			ID = team.ID;
			Name = team.Name;

			_score = _service.GetTeamScore (ID);
		}

		void OnResultsChanged (ResultsChangedMessage message)
		{
			_score = message.QuestionID.Equals (ResultsChangedMessage.ResultsCleared) ? 0 : _service.GetTeamScore (ID);

			RaisePropertyChanged (() => Score);
		}

		public int ID { get; set; }

		public string Name { get; private set; }

		public string Score {
			get { return StringResources.TeamScoreTitle + " " + _score; }
		}

		public void ClearScore ()
		{
			_score = 0;
			RaisePropertyChanged (() => Score);
		}
	}
}

