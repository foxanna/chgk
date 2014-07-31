using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChGK.Core.Models;
using ChGK.Core.Services;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;

namespace ChGK.Core.ViewModels
{
	public class TeamsViewModel : MenuItemViewModel
	{
		readonly ITeamsService _service;

		public TeamsViewModel (ITeamsService service)
		{
			_service = service;

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
	}
}

