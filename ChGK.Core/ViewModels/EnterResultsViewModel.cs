using System.Collections.Generic;
using System.Threading.Tasks;
using Cirrious.MvvmCross.ViewModels;
using ChGK.Core.Models;
using ChGK.Core.Services;
using System.Linq;
using Newtonsoft.Json;

namespace ChGK.Core.ViewModels
{
	public class EnterResultsViewModel : MvxViewModel
	{
		readonly ITeamsService _service;

		string _questionId;

		public EnterResultsViewModel (ITeamsService service)
		{
			_service = service;
		}

		public void Init (string questionId, string results)
		{
			_questionId = questionId;

			Teams = JsonConvert.DeserializeObject<List<ResultsTeamViewModel>> (results);
		}

		//		async Task LoadTeams ()
		//		{
		//			Teams = null;
		//			var teams = await Task.Factory.StartNew<List<Team>> (_service.GetAllTeams);
		//			var results = await Task.Factory.StartNew<List<int>> (() =>	_service.GetAllResults (_questionId));
		//			Teams = teams.Select (team => new ResultsTeamViewModel {ID =
		//				team.ID,
		//				Name = team.Name,
		//				AnsweredCorrectly = results.Contains (team.ID),
		//			}).ToList ();
		//		}

		List<ResultsTeamViewModel> _teams;

		public List<ResultsTeamViewModel> Teams {
			get {			
				return _teams;
			}
			set {
				_teams = value; 
				RaisePropertyChanged (() => Teams);
			}
		}

		public async void SubmitResults ()
		{
			await Task.Factory.StartNew (() => {
				foreach (var team in Teams.Where (t => t.AnsweredCorrectly)) {
					_service.IncrementScore (_questionId, team.ID);
				}
				foreach (var team in Teams.Where (t => !t.AnsweredCorrectly)) {
					_service.DecrementScore (_questionId, team.ID);
				}
			});
		}
	}

	public class ResultsTeamViewModel : MvxViewModel
	{
		public int ID { get; set; }

		public string Name { get; set; }

		public bool AnsweredCorrectly { get; set; }
	}
}

