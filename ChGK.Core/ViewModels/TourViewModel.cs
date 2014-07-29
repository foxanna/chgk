using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Cirrious.MvvmCross.ViewModels;
using Newtonsoft.Json;
using ChGK.Core.Models;
using ChGK.Core.Services;

namespace ChGK.Core.ViewModels
{
	public class TourViewModel : LoadableViewModel
	{
		public TourViewModel (IChGKWebService service) : base (service)
		{
		}

		string _fileName;

		public async void Init (string name, string filename)
		{
			Name = name;
			_fileName = filename;

			await LoadItemsAsync ();
		}

		protected override async Task LoadItemsInternal ()
		{
			var tour = await ChGKService.GetTourDetails (_fileName);

			Questions = tour.Questions;

			var infoSB = new StringBuilder ();
			if (!string.IsNullOrEmpty (tour.Editors)) {
				infoSB.Append (string.Format ("Редакторы:\n{0}\n", tour.Editors));
			}

			infoSB.Append (string.Format ("\nКоличество вопросов: {0}\n", Questions.Count));

			Info = infoSB.ToString ();
		}

		string _name;

		public string Name {
			get {
				return _name;
			}
			set {
				_name = value; 
				RaisePropertyChanged (() => Name);
			}
		}

		string _info;

		public string Info {
			get {
				return _info;
			}
			set {
				_info = value; 
				RaisePropertyChanged (() => Info);
			}
		}

		List<IQuestion> _questions;

		public List<IQuestion> Questions {
			get {
				return _questions;
			}
			set {
				_questions = value; 
				RaisePropertyChanged (() => Questions);
			}
		}

		MvxCommand <IQuestion> _showQuestionCommand;

		public MvxCommand <IQuestion> ShowQuestionCommand {
			get {
				return _showQuestionCommand ?? (_showQuestionCommand = new MvxCommand<IQuestion> (ShowQuestion));
			}
		}

		void ShowQuestion (IQuestion question)
		{
			ShowViewModel<QuestionsViewModel> (new {
				questionsJson = JsonConvert.SerializeObject (Questions),
				index = Questions.IndexOf (question),
			});
		}
	}
}

