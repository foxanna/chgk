using System;
using Cirrious.MvvmCross.ViewModels;
using ChGK.Core.Models;

namespace ChGK.Core.ViewModels
{
	public class QuestionViewModel : MvxViewModel
	{
		public QuestionViewModel (IQuestion question, int index)
		{
			Text = question.Text;
			Answer = question.Answer;
			Comment = question.Comment;
			Author = question.Author;
			Source = question.Source;
			Index = index;

			HasComments = !string.IsNullOrEmpty (Comment);
			HasAuthor = !string.IsNullOrEmpty (Author);
			HasSource = !string.IsNullOrEmpty (Source);
		}

		private string _text;

		public string Text {
			get {
				return _text;
			}
			set {
				_text = value; 
				RaisePropertyChanged (() => Text);
			}
		}

		private string _answer;

		public string Answer {
			get {
				return _answer;
			}
			set {
				_answer = value; 
				RaisePropertyChanged (() => Answer);
			}
		}

		private string _comment;

		public string Comment {
			get {
				return _comment;
			}
			set {
				_comment = value; 
				RaisePropertyChanged (() => Comment);
			}
		}

		private string _author;

		public string Author {
			get {
				return _author;
			}
			set {
				_author = value; 
				RaisePropertyChanged (() => Author);
			}
		}

		private string _source;

		public string Source {
			get {
				return _source;
			}
			set {
				_source = value; 
				RaisePropertyChanged (() => Source);
			}
		}

		private int _index;

		public int Index {
			get {
				return _index;
			}
			set {
				_index = value; 
				RaisePropertyChanged (() => Index);
			}
		}

		private bool _hasComments;

		public bool HasComments {
			get {
				return _hasComments;
			}
			set {
				_hasComments = value; 
				RaisePropertyChanged (() => HasComments);
			}
		}

		private bool _hasAuthor;

		public bool HasAuthor {
			get {
				return _hasAuthor;
			}
			set {
				_hasAuthor = value; 
				RaisePropertyChanged (() => HasAuthor);
			}
		}

		private bool _hasSource;

		public bool HasSource {
			get {
				return _hasSource;
			}
			set {
				_hasSource = value; 
				RaisePropertyChanged (() => HasSource);
			}
		}

		private bool _isAnswerShown;

		public bool IsAnswerShown {
			get {
				return _isAnswerShown;
			}
			set {
				_isAnswerShown = value; 
				RaisePropertyChanged (() => IsAnswerShown);
			}
		}

		private MvxCommand _showAnswerCommand;

		public MvxCommand ShowAnswerCommand {
			get {
				_showAnswerCommand = _showAnswerCommand ??
				new MvxCommand (() => IsAnswerShown = true);
				return _showAnswerCommand;
			}
		}
	}
}