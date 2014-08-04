using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChGK.Core.Models;
using ChGK.Core.Services;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Plugins.Messenger;
using ChGK.Core.Messages;

namespace ChGK.Core.ViewModels
{
	public class TeamsViewModel : MenuItemViewModel
	{
		readonly ITeamsService _service;

		#pragma warning disable 414
		readonly MvxSubscriptionToken _teamsChangedToken;
		#pragma warning restore 414

		public TeamsViewModel (ITeamsService service, IMvxMessenger messenger)
		{
			_service = service;
			_teamsChangedToken = messenger.Subscribe<TeamsChangedMessage> (OnTeamsChanged);

			Title = StringResources.Teams;
		}

		public async override void Start ()
		{
			base.Start ();

			await LoadTeams ();
		}

		async Task LoadTeams ()
		{
			Teams = null;
			var teams = await Task.Factory.StartNew<List<Team>> (_service.GetAllTeams);
			Teams = teams.Select (team => new TeamViewModel (_service, team)).ToList ();
		}

		async void OnTeamsChanged (TeamsChangedMessage obj)
		{
			await LoadTeams ();
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

		async void AddTeam (string name)
		{
			_service.AddTeam (name.Trim ());

			await LoadTeams ();
		}

		public override Task Refresh ()
		{
			throw new System.NotImplementedException ();
		}

		public void Remove (int[] positions)
		{
			_service.RemoveTeam (Teams [positions [0]].ID);
		}
	}

	public class TeamViewModel : MvxViewModel
	{
		public TeamViewModel (ITeamsService service, Team team)
		{
			ID = team.ID;
			Name = team.Name;
			Score = StringResources.TeamScoreTitle + " " + service.GetTeamScore (team);
		}

		public int ID { get; set; }

		public string Name { get; private set; }

		public string Score { get; private set; }
	}
}

