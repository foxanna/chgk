using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ChGK.Core.Models;
using ChGK.Core.Services;
using ChGK.Core.Utils;
using MvvmCross.Core.ViewModels;
using Newtonsoft.Json;

namespace ChGK.Core.ViewModels
{
    public class TourViewModel : MenuItemViewModel
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private string _fileName;

        private string _info;

        private List<IQuestion> _questions;
        private readonly IChGKWebService _service;

        private MvxCommand<IQuestion> _showQuestionCommand;

        public TourViewModel(IChGKWebService service)
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

        public MvxCommand<IQuestion> ShowQuestionCommand
        {
            get { return _showQuestionCommand ?? (_showQuestionCommand = new MvxCommand<IQuestion>(ShowQuestion)); }
        }

        private async Task LoadItems()
        {
            Questions = null;

            var tour = await _service.GetTourDetails(_fileName, _cancellationTokenSource.Token);

            Questions = tour.Questions;

            var infoSB = new StringBuilder();
            if (!string.IsNullOrEmpty(tour.Editors))
            {
                infoSB.Append(string.Format("Редакторы:\n{0}\n", tour.Editors));
            }

            infoSB.Append(string.Format("\nКоличество вопросов: {0}\n", Questions.Count));

            Info = infoSB.ToString();
        }

        public async void Init(string name, string filename)
        {
            Title = name;
            if (!filename.StartsWith("tour/"))
            {
                filename = "tour/" + filename;
            }

            _fileName = filename;

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