using System;
using System.Linq;
using Cirrious.MvvmCross.ViewModels;
using ChGK.Core.Models;
using System.Collections.Generic;
using Cirrious.MvvmCross.Plugins.Json;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Newtonsoft.Json;

namespace ChGK.Core.ViewModels
{
	public class QuestionsViewModel : MvxViewModel
	{
		public QuestionsViewModel ()
		{
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
	}
}

