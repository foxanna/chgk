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
		readonly IChGKWebService _service;

		public DataLoader DataLoader { get; set; }

		CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource ();

		public RandomQuestionsViewModel (IChGKWebService service)
		{
			Title = StringResources.RandomQuestions;

			_service = service;

			DataLoader = new DataLoader ();
		}

		public async override void Start ()
		{
			await Refresh ();
		}

		async Task LoadItems ()
		{
			Questions = null;	

			Questions = await _service.GetRandomPackage (_cancellationTokenSource.Token);
		}

		public override Task Refresh ()
		{
			return DataLoader.LoadItemsAsync (LoadItems);
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

