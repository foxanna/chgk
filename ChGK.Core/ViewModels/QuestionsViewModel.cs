using System.Collections.Generic;
using System.Linq;
using ChGK.Core.Models;
using ChGK.Core.Services;
using MvvmCross.Platform;
using Newtonsoft.Json;

namespace ChGK.Core.ViewModels
{
    public class QuestionsViewModel : MenuItemViewModel
    {
        private readonly IAudioPlayerService _audioPlayerService;
        private readonly IGAService _gaService;
        private readonly IChGKService _service;

        private List<QuestionViewModel> _questions;

        public QuestionsViewModel(IGAService gaService,
            IAudioPlayerService audioPlayerService,
            IChGKService service)
        {
            _gaService = gaService;
            _audioPlayerService = audioPlayerService;
            _service = service;

            Title = "Вопрос 1";
        }

        public List<QuestionViewModel> Questions
        {
            get { return _questions; }
            set
            {
                _questions = value;
                RaisePropertyChanged(() => Questions);
            }
        }

        public int Index { get; private set; }

        public void Init(string questionsJson, int index)
        {
            Questions = JsonConvert.DeserializeObject<List<Question>>(questionsJson).Cast<IQuestion>()
                .Select(question =>
                {
                    var viewModel = Mvx.Create<QuestionViewModel>();
                    viewModel.Question = question;
                    return viewModel;
                })
                .ToList();

            Index = index;
        }

        public override void OnViewDestroying()
        {
            foreach (var questionViewModel in Questions)
            {
                questionViewModel.OnViewDestroying();
            }

            base.OnViewDestroying();
        }
    }
}