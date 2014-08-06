using System;
using System.Collections.Generic;
using System.Linq;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using ChGK.Core.Services;
using System.Threading.Tasks;
using Cirrious.MvvmCross.Plugins.Messenger;
using ChGK.Core.Messages;

namespace ChGK.Core.ViewModels
{
	public class EnterResultsViewModel : MvxViewModel
	{
		readonly ITeamsService _teamsService;

		#pragma warning disable 414
		readonly MvxSubscriptionToken _teamsChangedToken;
		readonly MvxSubscriptionToken _resultsChangedToken;
		#pragma warning restore 414

		public DataLoader DataLoader { get; set; }

		string _questionId;

		public EnterResultsViewModel (ITeamsService service, IMvxMessenger messenger)
		{
			_teamsService = service;

			_teamsChangedToken = messenger.Subscribe<TeamsChangedMessage> (OnTeamsChanged);
			_resultsChangedToken = messenger.Subscribe<ResultsChangedMessage> (OnResultsChanged);

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

		async void ReloadResults ()
		{
			await DataLoader.LoadItemsAsync (LoadItems);
		}

		public async void Init (string questionId, string results)
		{
			_questionId = questionId;

			ReloadResults ();
		}

		void OnTeamsChanged (TeamsChangedMessage obj)
		{
			ReloadResults ();
		}

		void OnResultsChanged (ResultsChangedMessage obj)
		{
			ReloadResults ();
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

		MvxCommand _clearResultsCommand;

		public MvxCommand ClearResultsCommand {
			get {
				return _clearResultsCommand ?? (_clearResultsCommand = new MvxCommand (ClearResults));
			}
		}

		void ClearResults ()
		{
			_teamsService.CleanResults ();
		}

		MvxCommand _editTeamsCommand;

		public MvxCommand EditTeamsCommand {
			get {
				return _editTeamsCommand ?? (_editTeamsCommand = new MvxCommand (() => ShowViewModel<TeamsViewModel> ()));
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

