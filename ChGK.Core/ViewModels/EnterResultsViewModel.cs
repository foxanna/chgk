using System;
using System.Collections.Generic;
using System.Linq;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using Newtonsoft.Json;
using ChGK.Core.Services;

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
					_service.IncrementScore (_questionId, team.ID);
				}
				foreach (var team in Teams.Where (t => !t.AnsweredCorrectly)) {
					_service.DecrementScore (_questionId, team.ID);
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

