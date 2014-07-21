using System;
using Cirrious.MvvmCross.ViewModels;
using ChGK.Core.Services;
using ChGK.Core.Models;
using System.Collections.Generic;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Newtonsoft.Json;

namespace ChGK.Core.ViewModels
{
	public class TourViewModel : LoadableViewModel
	{
		public TourViewModel (IChGKWebService service) : base (service)
		{
		}

		public async void Init (string name, string filename)
		{
			Name = name;

			IsLoading = true;

			try {
				var tour = await _chGKService.GetTourDetails (filename);

				Questions = tour.Questions;
				Editors = tour.Editors;
			} catch (Exception e) {
				Mvx.Trace (e.Message);
			} finally {
				IsLoading = false;
			}
		}

		string _name;

		public string Name {
			get {
				return _name;
			}
			set {
				_name = value; 
				RaisePropertyChanged (() => Name);
			}
		}

		string _editors;

		public string Editors {
			get {
				return _editors;
			}
			set {
				_editors = value; 
				RaisePropertyChanged (() => Editors);
			}
		}

		List<IQuestion> _questions;

		public List<IQuestion> Questions {
			get {
				return _questions;
			}
			set {
				_questions = value; 
				RaisePropertyChanged (() => Questions);
			}
		}

		MvxCommand <IQuestion> _showQuestionCommand;

		public MvxCommand <IQuestion> ShowQuestionCommand {
			get {
				return _showQuestionCommand ?? (_showQuestionCommand = new MvxCommand<IQuestion> (ShowQuestion));
			}
		}

		void ShowQuestion (IQuestion question)
		{
			ShowViewModel<QuestionsViewModel> (new {
				questionsJson = JsonConvert.SerializeObject (Questions),
				index = Questions.IndexOf (question),
			});
		}
	}
}

