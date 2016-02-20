using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using ChGK.Core.Models;
using ChGK.Core.Services;
using ChGK.Core.Utils;
using Newtonsoft.Json;

namespace ChGK.Core.ViewModels
{
    public class RandomQuestionsViewModel : MenuItemViewModel
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly IGAService _gaService;
        private readonly IChGKService _service;

        private List<IQuestion> _questions;

        private ICommand _refreshCommand, _showQuestionCommand;

        public RandomQuestionsViewModel(IChGKService service,
            IGAService gaService)
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

        public ICommand ShowQuestionCommand =>
            _showQuestionCommand ?? (_showQuestionCommand = new Command<IQuestion>(ShowQuestion));

        public ICommand RefreshCommand =>
            _refreshCommand ?? (_refreshCommand = new Command(Refresh));

        private async void Refresh()
        {
            _gaService.ReportEvent(GACategory.QuestionsList, GAAction.Click, "refresh");

            await RefreshAsync();
        }

        private Task RefreshAsync()
        {
            return DataLoader.LoadItemsAsync(LoadItems);
        }

        public override async void Start()
        {
            await RefreshAsync();
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