using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ChGK.Core.Models;
using ChGK.Core.Services;
using ChGK.Core.Utils;
using MvvmCross.Core.ViewModels;
using Newtonsoft.Json;

namespace ChGK.Core.ViewModels
{
    public class RandomQuestionsViewModel : MenuItemViewModel
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly IGAService _gaService;
        private readonly IChGKWebService _service;

        private List<IQuestion> _questions;

        private MvxCommand _refreshCommand;

        private MvxCommand<IQuestion> _showQuestionCommand;

        public RandomQuestionsViewModel(IChGKWebService service, IGAService gaService)
        {
            Title = StringResources.RandomQuestions;

            _service = service;
            _gaService = gaService;

            DataLoader = new DataLoader();
        }

        public DataLoader DataLoader { get; set; }

        public List<IQuestion> Questions
        {
            get { return _questions; }
            set
            {
                _questions = value;
                RaisePropertyChanged(() => Questions);
            }
        }

        public MvxCommand<IQuestion> ShowQuestionCommand
        {
            get
            {
                _showQuestionCommand = _showQuestionCommand ?? new MvxCommand<IQuestion>(ShowQuestion);
                return _showQuestionCommand;
            }
        }

        public MvxCommand RefreshCommand
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

        public override async void Start()
        {
            await DataLoader.LoadItemsAsync(LoadItems);
        }

        private async Task LoadItems()
        {
            Questions = null;

            Questions = await _service.GetRandomPackage(_cancellationTokenSource.Token);
        }

        private void ShowQuestion(IQuestion question)
        {
            ShowViewModel<QuestionsViewModel>(new
            {
                questionsJson = JsonConvert.SerializeObject(Questions),
                index = Questions.IndexOf(question)
            });
        }

        public override void OnViewDestroying()
        {
            _cancellationTokenSource.Cancel();

            base.OnViewDestroying();
        }
    }
}