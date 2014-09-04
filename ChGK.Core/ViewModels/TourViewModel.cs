using System.Collections.Generic;
using System.Text;
using System.Threading;
using Cirrious.MvvmCross.ViewModels;
using Newtonsoft.Json;
using ChGK.Core.Models;
using ChGK.Core.Services;
using System.Threading.Tasks;

namespace ChGK.Core.ViewModels
{
    public class TourViewModel : MenuItemViewModel
	{
		IChGKWebService _service;

		public DataLoader DataLoader { get; set; }

		CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource ();

		public TourViewModel (IChGKWebService service)
		{
			_service = service;

			DataLoader = new DataLoader ();
		}

		async Task LoadItems ()
		{
			Questions = null;

			var tour = await _service.GetTourDetails (_fileName, _cancellationTokenSource.Token);

			Questions = tour.Questions;

			var infoSB = new StringBuilder ();
			if (!string.IsNullOrEmpty (tour.Editors)) {
				infoSB.Append (string.Format ("Редакторы:\n{0}\n", tour.Editors));
			}

			infoSB.Append (string.Format ("\nКоличество вопросов: {0}\n", Questions.Count));

			Info = infoSB.ToString ();
		}

		string _fileName;

		public async void Init (string name, string filename)
		{
			Title = name;
            if (!filename.StartsWith("tour/"))
            {
                filename = "tour/" + filename;
            }

			_fileName = filename;
	
			await DataLoader.LoadItemsAsync (LoadItems);
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

        public override void OnViewDestroying()
        {
            _cancellationTokenSource.Cancel();

            base.OnViewDestroying();
        }
	}
}

