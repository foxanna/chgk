using System;
using System.Collections.Generic;
using System.Linq;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using ChGK.Core.Services;
using System.Threading.Tasks;

namespace ChGK.Core.ViewModels
{
	public class EnterResultsViewModel : MvxViewModel
	{
		readonly ITeamsService _teamsService;

		public DataLoader DataLoader { get; set; }

		string _questionId;

		public EnterResultsViewModel (ITeamsService service)
		{
			_teamsService = service;
			DataLoader = new DataLoader ();
		}

		async Task LoadItems ()
		{
			Teams = null;	

			Teams = await Task.Factory.StartNew<List<ResultsTeamViewModel>> (() => {
				var teams = _teamsService.GetAllTeams ();
				var results = _teamsService.GetAllResults (_questionId);

				var _results = teams.Select (team => new ResultsTeamViewModel { 
					Name = team.Name, 
					ID = team.ID, 
					AnsweredCorrectly = results.Contains (team.ID)
				}).ToList ();

				return _results;
			});

			IsEmpty = Teams.Count == 0;
		}

		public async void Init (string questionId, string results)
		{
			_questionId = questionId;

			await DataLoader.LoadItemsAsync (LoadItems);
		}

		bool _isEmpty;

		public bool IsEmpty {
			get { return _isEmpty; }
			set {
				_isEmpty = value;
				RaisePropertyChanged (() => IsEmpty);
			}
		}

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

		public void SubmitResults ()
		{
			try {
				foreach (var team in Teams.Where (t => t.AnsweredCorrectly)) {
					_teamsService.IncrementScore (_questionId, team.ID);
				}
				foreach (var team in Teams.Where (t => !t.AnsweredCorrectly)) {
					_teamsService.DecrementScore (_questionId, team.ID);
				}
			} catch (Exception e) {
				Mvx.Trace (e.Message);
			}
		}
	}

	public class ResultsTeamViewModel : MvxViewModel
	{
		public int ID { get; set; }

		public string Name { get; set; }

		public bool AnsweredCorrectly { get; set; }
	}
}

