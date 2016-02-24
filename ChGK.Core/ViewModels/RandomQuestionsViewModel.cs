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
        }

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

        private async Task LoadItems(CancellationToken token)
        {
            Questions = null;
            Questions = await _service.GetRandomPackage(token);
        }

        private void ShowQuestion(IQuestion question)
        {
            ShowViewModel<QuestionsViewModel>(new
            {
                questionsJson = JsonConvert.SerializeObject(Questions),
                index = Questions.IndexOf(question)
            });
        }
    }
}