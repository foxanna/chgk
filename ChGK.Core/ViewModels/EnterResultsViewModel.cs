using ChGK.Core.Messages;
using ChGK.Core.Services;
using ChGK.Core.Utils;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChGK.Core.ViewModels
{
	public class EnterResultsViewModel : MvxViewModel, IViewLifecycle
	{
		readonly ITeamsService _teamsService;
        readonly IMvxMessenger _messenger;
        readonly IGAService _gaService;

		#pragma warning disable 414
		MvxSubscriptionToken _teamsChangedToken;
		MvxSubscriptionToken _resultsChangedToken;
		#pragma warning restore 414

		public DataLoader DataLoader { get; set; }

		string _questionId;

        public EnterResultsViewModel(ITeamsService service, IMvxMessenger messenger, IGAService gaService)
		{
			_teamsService = service;
            _messenger = messenger;
            _gaService = gaService;

			_teamsChangedToken = messenger.Subscribe<TeamsChangedMessage> (OnTeamsChanged);
			_resultsChangedToken = messenger.Subscribe<ResultsChangedMessage> (OnResultsChanged);

			DataLoader = new DataLoader ();
		}

		void LoadItems ()
		{
            Teams = null;

			var teams = _teamsService.GetAllTeams ();
			var results = _teamsService.GetAllResults (_questionId);

			Teams = teams.Select (team => new ResultsTeamViewModel { 
				Name = team.Name, 
				ID = team.ID, 
				AnsweredCorrectly = results.Contains (team.ID)
			}).ToList ();
            
			IsEmpty = Teams.Count == 0;
		}

		void ReloadResults ()
		{
			DataLoader.LoadItems (LoadItems);
		}

		public void Init (string questionId)
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
            if (_questionId.Equals(obj.QuestionID))
            {
                ReloadResults();
            }
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
            OnViewDestroying();

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

            _gaService.ReportEvent(GACategory.PlayQuestion, GAAction.Click, "results submitted");

            Close(this);
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
                return _editTeamsCommand ?? (_editTeamsCommand = new MvxCommand(() =>
                {
                    _gaService.ReportEvent(GACategory.DealWithTeams, GAAction.Open, "edit teams from enter results screen");
                    ShowViewModel<TeamsViewModel>();
                }));
			}
		}

        public void OnViewDestroying()
        {
            if (_resultsChangedToken != null)
            {
                _messenger.Unsubscribe<ResultsChangedMessage>(_resultsChangedToken);
                _resultsChangedToken = null;
            }
            if (_teamsChangedToken != null)
            {
                _messenger.Unsubscribe<TeamsChangedMessage>(_teamsChangedToken);
                _teamsChangedToken = null;
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

