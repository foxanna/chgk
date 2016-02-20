using System.Collections.Generic;
using System.Linq;
using ChGK.Core.Models;
using Newtonsoft.Json;

namespace ChGK.Core.ViewModels
{
    public class QuestionsViewModel : MenuItemViewModel
    {
        private List<QuestionViewModel> _questions;

        public QuestionsViewModel()
        {
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
                .Select((iquestion, i) => new QuestionViewModel(iquestion, i)).ToList();

            Index = index;
        }

        public override void OnViewDestroying()
        {
            foreach (var questionViewModel in Questions)
            {
                questionViewModel.OnViewDestroying();
            }
        }
    }
}