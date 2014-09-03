using System.Collections.Generic;
using System.Threading.Tasks;
using Cirrious.MvvmCross.ViewModels;
using Newtonsoft.Json;
using ChGK.Core.Models;
using ChGK.Core.Services;
using System.Threading;
using System.Windows.Input;

namespace ChGK.Core.ViewModels
{
	public class RandomQuestionsViewModel : MenuItemViewModel
	{
        readonly IChGKWebService _service;
        readonly IGAService _gaService;

		public DataLoader DataLoader { get; set; }

		CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource ();

        public RandomQuestionsViewModel(IChGKWebService service, IGAService gaService)
		{
			Title = StringResources.RandomQuestions;

            _service = service;
            _gaService = gaService;

			DataLoader = new DataLoader ();
		}

		public async override void Start ()
		{
            await DataLoader.LoadItemsAsync(LoadItems);
		}

		async Task LoadItems ()
		{
			Questions = null;	

			Questions = await _service.GetRandomPackage (_cancellationTokenSource.Token);
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

        ICommand _refreshCommand;

        public ICommand RefreshCommand
        {
            get
            {
                return _refreshCommand ?? (_refreshCommand = new MvxCommand(async () =>
                {
                    _gaService.ReportEvent(GACategory.QuestionsList, GAAction.Click, "refresh");
                    await DataLoader.LoadItemsAsync(LoadItems);
                }));
            }
        }
	}
}

