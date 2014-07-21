using System;
using Cirrious.MvvmCross.ViewModels;
using ChGK.Core.Models;
using ChGK.Core.Services;
using Cirrious.CrossCore;
using ChGK.Core.Utils;

namespace ChGK.Core.ViewModels
{
	public class QuestionViewModel : MvxViewModel
	{
		public QuestionViewModel ()
		{
			timer.OneSecond += (sender, e) => Time = e.Seconds;
		}

		public QuestionViewModel (IQuestion question, int index) : this ()
		{
			Text = question.Text;
			Answer = question.Answer;
			Comment = question.Comment;
			Author = question.Author;
			Source = question.Source;
			Index = index;
			HasPicture = !string.IsNullOrEmpty (question.Picture);
			Picture = "http://db.chgk.info/images/db/" + question.Picture;

			HasComments = !string.IsNullOrEmpty (Comment);
			HasAuthor = !string.IsNullOrEmpty (Author);
			HasSource = !string.IsNullOrEmpty (Source);
		}

		public string Text { get; set; }

		public string Answer { get; set; }

		public string Comment { get; set; }

		public string Author { get; set; }

		public string Source { get; set; }

		public string Picture { get; set; }

		public int Index { get; set; }

		public bool HasComments { get; set; }

		public bool HasAuthor { get; set; }

		public bool HasSource { get; set; }

		public bool HasPicture { get; set; }

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
				}));
			}
		}

		ChGKTimer timer = new ChGKTimer ();

		MvxCommand _startTimerCommand;

		public MvxCommand StartTimerCommand {
			get {
				return _startTimerCommand ?? (_startTimerCommand = 
					new MvxCommand (StartTimer));
			}
		}

		MvxCommand _stopTimerCommand;

		public MvxCommand StopTimerCommand {
			get {
				return _stopTimerCommand ?? (_stopTimerCommand = 
					new MvxCommand (PauseTimer));
			}
		}

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

		void StartTimer ()
		{
			timer.Resume ();
			IsTimerStarted = true;
		}

		void PauseTimer ()
		{
			timer.Pause ();
			IsTimerStarted = false;
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
	}
}