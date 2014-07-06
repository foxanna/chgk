using System;
using Cirrious.MvvmCross.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChGK.Core.Services;
using ChGK.Core.Models;

namespace ChGK.Core.ViewModels
{
	public class RandomQuestionsViewModel: BaseViewModel
	{
		public RandomQuestionsViewModel (IChGKWebService service) : base (service)
		{

		}

		public async override void Start ()
		{
			await LoadQuestionsAsync ();
		}

		async Task LoadQuestionsAsync ()
		{
			IsLoading = true;

			try {
				Questions = await _chGKService.GetRandomPackage ();
			} catch {

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

		private bool _isLoading;

		public bool IsLoading {
			get {
				return _isLoading;
			}
			set {
				_isLoading = value; 
				RaisePropertyChanged (() => IsLoading);
			}
		}

		private int _selectedItemIndex;

		public int SelectedItemIndex {
			get {
				return _selectedItemIndex;
			}
			set {
				_selectedItemIndex = value;
				RaisePropertyChanged (() => SelectedItemIndex);
			}
		}

		private MvxCommand <IQuestion> _showQuestionCommand;

		public MvxCommand <IQuestion> ShowQuestionCommand {
			get {
				_showQuestionCommand = _showQuestionCommand ?? new MvxCommand<IQuestion> (ShowQuestion);
				return _showQuestionCommand;
			}
		}

		private void updateSelectedIndex (IQuestion question)
		{
			SelectedItemIndex = Questions.IndexOf (question);
		}

		private void ShowQuestion (IQuestion question)
		{
			updateSelectedIndex (question);

			ShowViewModel<QuestionViewModel> (new QuestionViewModel.QuestionNav () {
				Question = question.Text,
				Answer = question.Answer,
				Comment = question.Comment,
				Author = question.Author,
				Source = question.Source,
				Index = SelectedItemIndex,
			});
		}
	}
}

