using System;
using Cirrious.MvvmCross.ViewModels;

namespace ChGK.Core.ViewModels
{
	public class QuestionViewModel : MvxViewModel
	{
		public class QuestionNav
		{
			public String Question { get; set; }

			public String Answer { get; set; }

			public String Comment { get; set; }

			public String Source { get; set; }

			public String Author { get; set; }

			public int Index  { get; set; }
		}

		public void Init (QuestionNav navigation)
		{
			Text = navigation.Question;
			Answer = navigation.Answer;
			Comment = navigation.Comment;
			Author = navigation.Author;
			Source = navigation.Source;
			Index = navigation.Index;
			HasComments = !string.IsNullOrEmpty (Comment);
			HasAuthor = !string.IsNullOrEmpty (Author);
			HasSource = !string.IsNullOrEmpty (Source);
		}

		private String _text;

		public String Text {
			get {
				return _text;
			}
			set {
				_text = value; 
				RaisePropertyChanged (() => Text);
			}
		}

		private String _answer;

		public String Answer {
			get {
				return _answer;
			}
			set {
				_answer = value; 
				RaisePropertyChanged (() => Answer);
			}
		}

		private String _comment;

		public String Comment {
			get {
				return _comment;
			}
			set {
				_comment = value; 
				RaisePropertyChanged (() => Comment);
			}
		}

		private String _author;

		public String Author {
			get {
				return _author;
			}
			set {
				_author = value; 
				RaisePropertyChanged (() => Author);
			}
		}

		private String _source;

		public String Source {
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