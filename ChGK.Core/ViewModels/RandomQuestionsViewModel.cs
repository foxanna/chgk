using System.Collections.Generic;
using System.Threading.Tasks;
using Cirrious.MvvmCross.ViewModels;
using Newtonsoft.Json;
using ChGK.Core.Models;
using ChGK.Core.Services;
using System.Threading;

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
			await LoadItemsAsync ();
		}

		protected override async Task LoadItemsInternal (CancellationToken token)
		{
			Questions = await ChGKService.GetRandomPackage (token);
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
				_showQuestionCommand = _showQuestionCommand ?? new MvxCommand<IQuestion> (ShowQuestion);
				return _showQuestionCommand;
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

