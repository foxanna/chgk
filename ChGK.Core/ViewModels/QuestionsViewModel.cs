using System;
using System.Linq;
using Cirrious.MvvmCross.ViewModels;
using ChGK.Core.Models;
using System.Collections.Generic;
using Cirrious.MvvmCross.Plugins.Json;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Newtonsoft.Json;
using ChGK.Core.Utils;
using ChGK.Core.Services;
using ChGK.Core.ViewModels.Tutorials;

namespace ChGK.Core.ViewModels
{
    public class QuestionsViewModel : MenuItemViewModel
	{
		public QuestionsViewModel ()
		{
            Title = "Вопрос 1";
		}

        public override void Start()
        {
            base.Start();

            var _firstViewStartInfoProvider = Mvx.Resolve<IFirstViewStartInfoProvider>();
            if (_firstViewStartInfoProvider.IsSeenForTheFirstTime(this.GetType()))
            {
                ShowViewModel(new FirstTimeSeenViewModelsFactory().CreateViewModel(this.GetType()));
                _firstViewStartInfoProvider.SetSeen(this.GetType());
            }
        }

		public void Init (string questionsJson, int index)
		{
			Questions = JsonConvert.DeserializeObject<List<Question>> (questionsJson).Cast<IQuestion> ()
				.Select ((iquestion, i) => new QuestionViewModel (iquestion, i)).ToList ();

			Index = index;
		}

		List<QuestionViewModel> _questions;

		public List<QuestionViewModel> Questions {
			get {
				return _questions;
			}
			set {
				_questions = value; 
				RaisePropertyChanged (() => Questions);
			}
		}

		public int Index { get; private set; }

        public override void OnViewDestroying()
        {
            foreach (var questionViewModel in Questions)
            {
                questionViewModel.OnViewDestroying();
            }
        }
	}
}

