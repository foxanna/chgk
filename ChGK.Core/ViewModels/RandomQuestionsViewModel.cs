using System;
using Cirrious.MvvmCross.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChGK.Core.Services;
using ChGK.Core.Models;
using Cirrious.CrossCore;
using Newtonsoft.Json;

namespace ChGK.Core.ViewModels
{
	public class RandomQuestionsViewModel : MenuItemViewModel
	{
		public RandomQuestionsViewModel (IChGKWebService service) : base (service)
		{
			Title = "Случайные вопросы";
		}

		public async override void Start ()
		{
			await LoadQuestionsAsync ();
		}

		protected override async void Refresh ()
		{
			await LoadQuestionsAsync ();
		}

		async Task LoadQuestionsAsync ()
		{
			IsLoading = true;

			try {
				Questions = await _chGKService.GetRandomPackage ();
			} catch (Exception e) {
				Mvx.Trace (e.Message);
			} finally {
				IsLoading = false;
			}
		}

		private List<IQuestion> _questions;

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
				_showQuestionCommand = _showQuestionCommand ?? new MvxCommand<IQuestion> (ShowQuestion);
				return _showQuestionCommand;
			}
		}

		private void ShowQuestion (IQuestion question)
		{
			ShowViewModel<QuestionsViewModel> (new {
				questionsJson = JsonConvert.SerializeObject (Questions),
				index = Questions.IndexOf (question),
			});
		}
	}
}

