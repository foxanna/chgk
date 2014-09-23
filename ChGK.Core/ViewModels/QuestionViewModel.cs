using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;
using Newtonsoft.Json;
using ChGK.Core.Messages;
using ChGK.Core.Models;
using ChGK.Core.Services;
using ChGK.Core.Utils;

namespace ChGK.Core.ViewModels
{
	public class QuestionViewModel : MvxViewModel, IViewLifecycle
	{
        readonly IGAService _gaService;

		public QuestionViewModel ()
		{
            _gaService = Mvx.Resolve<IGAService>();
		}

        void OnTimerOneSecond(object sender, TimerEventArgs e)
        {
            Time = e.Seconds;

            if (e.Seconds == new TimeSpan(0, 0, 50))
            {
                Mvx.Resolve<IAudioPlayerService>().PlayShort();
            }
            else if (e.Seconds == new TimeSpan(0, 1, 0))
            {
                PauseTimer();

                Mvx.Resolve<IAudioPlayerService>().PlayLong();
            }
        }

		public QuestionViewModel (IQuestion question, int index) : this ()
		{
			ID = question.ID;
			Text = question.Text;
			Answer = question.Answer;
			PassCriteria = question.PassCriteria;
			Comment = question.Comment;
			Author = question.Author;
			Source = question.Source;
			Index = index;
			HasPicture = !string.IsNullOrEmpty (question.Picture);
			Picture = "http://db.chgk.info/images/db/" + question.Picture;

			HasComments = !string.IsNullOrEmpty (Comment);
			HasAuthor = !string.IsNullOrEmpty (Author);
			HasSource = !string.IsNullOrEmpty (Source);
			HasPassCriteria = !string.IsNullOrEmpty (PassCriteria);
		}

		public string ID { get; set; }

		public string Text { get; set; }

		public string Answer { get; set; }

		public string PassCriteria { get; set; }

		public string Comment { get; set; }

		public string Author { get; set; }

		public string Source { get; set; }

		public string Picture { get; set; }

		public int Index { get; set; }

		public bool HasComments { get; set; }

		public bool HasAuthor { get; set; }

		public bool HasSource { get; set; }

		public bool HasPicture { get; set; }

		public bool HasPassCriteria { get; set; }

		bool _isAnswerShown;

		public bool IsAnswerShown {
			get {
				return _isAnswerShown;
			}
			set {
				_isAnswerShown = value; 
				RaisePropertyChanged (() => IsAnswerShown);
			}
		}

		MvxCommand _showAnswerCommand;

		public MvxCommand ShowAnswerCommand {
			get {
				return _showAnswerCommand ??
				(_showAnswerCommand = new MvxCommand (() => {
					IsAnswerShown = true;
					PauseTimer ();

                    _gaService.ReportEvent(GACategory.PlayQuestion, GAAction.Click, "answer shown");
				}));
			}
		}

		ChGKTimer timer = new ChGKTimer ();

		TimeSpan _timeSpan;

		public TimeSpan Time {
			get {
				return _timeSpan;
			}
			set {
				_timeSpan = value;
				RaisePropertyChanged (() => Time);
			}
		}

		public void StartTimer ()
		{
            timer.OneSecond += OnTimerOneSecond;

			timer.Resume ();
			IsTimerStarted = true;

            _gaService.ReportEvent(GACategory.PlayQuestion, GAAction.Timer, "start");
		}

		public void PauseTimer ()
		{
            timer.Pause ();
            
            timer.OneSecond -= OnTimerOneSecond;
			
            IsTimerStarted = false;

            _gaService.ReportEvent(GACategory.PlayQuestion, GAAction.Timer, "stop");
		}

		bool _isTimerStarted;

		public bool IsTimerStarted {
			get {
				return _isTimerStarted;
			}
			set {
				_isTimerStarted = value; 
				RaisePropertyChanged (() => IsTimerStarted);
				RaisePropertyChanged (() => IsTimerStopped);
			}
		}

		public bool IsTimerStopped {
			get {
				return !IsTimerStarted;
			}
		}

		MvxCommand _openImageCommand;

		public MvxCommand OpenImageCommand {
			get {
				return _openImageCommand ?? (_openImageCommand = 
					new MvxCommand (() => ShowViewModel<FullImageViewModel> (new { image = Picture })));
			}
		}

		public void EnterResults ()
		{
            _gaService.ReportEvent(GACategory.PlayQuestion, GAAction.Click, "enter results dialog opened");

			ShowViewModel<EnterResultsViewModel> (new { questionId = ID });
		}
        
        public void OnViewDestroying()
        {
            PauseTimer();
        }
    }
}