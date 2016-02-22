using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using ChGK.Core.Models;
using ChGK.Core.Services;
using ChGK.Core.Utils;
using Newtonsoft.Json;

namespace ChGK.Core.ViewModels
{
    public class TourViewModel : MenuItemViewModel
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly IChGKService _service;
        private string _id;
        private string _info;
        private List<IQuestion> _questions;
        private ICommand _showQuestionCommand;

        public TourViewModel(IChGKService service)
        {
            _service = service;

            DataLoader = new DataLoader();
        }

        public DataLoader DataLoader { get; set; }

        public string Info
        {
            get { return _info; }
            set
            {
                _info = value;
                RaisePropertyChanged(() => Info);
            }
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

        public ICommand ShowQuestionCommand
            => _showQuestionCommand ?? (_showQuestionCommand = new Command<IQuestion>(ShowQuestion));

        private async Task LoadItems()
        {
            Questions = null;

            var tour = await _service.GetTourDetails(_id, _cancellationTokenSource.Token);

            Questions = tour.Questions;

            var infoSB = new StringBuilder();
            if (!string.IsNullOrEmpty(tour.Editors))
            {
                infoSB.Append($"Редакторы:\n{tour.Editors}\n");
            }

            infoSB.Append($"\nКоличество вопросов: {Questions.Count}\n");

            Info = infoSB.ToString();
        }

        public async void Init(string name, string id)
        {
            Title = name;
            _id = id;

            await DataLoader.LoadItemsAsync(LoadItems);
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