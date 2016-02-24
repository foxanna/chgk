using System.Collections.Generic;
using System.Linq;
using ChGK.Core.Models;
using ChGK.Core.Services;
using Newtonsoft.Json;

namespace ChGK.Core.ViewModels
{
    public class QuestionsViewModel : MenuItemViewModel
    {
        private readonly IAudioPlayerService _audioPlayerService;
        private readonly IGAService _gaService;

        private List<QuestionViewModel> _questions;

        public QuestionsViewModel(IGAService gaService,
            IAudioPlayerService audioPlayerService)
        {
            _gaService = gaService;
            _audioPlayerService = audioPlayerService;

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
                .Select((iquestion, i) => new QuestionViewModel(_gaService, _audioPlayerService, iquestion, i)).ToList();

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