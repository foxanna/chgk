using System;
using Cirrious.MvvmCross.ViewModels;
using ChGK.Core.Protocol.Entities;

namespace ChGK.Core.DbChGKInfo.Entities
{
	public class ChGKQuestion : MvxViewModel, IQuestion
	{
		public ChGKQuestion ()
		{
		}
		// QuestionId
		private string _id = "Hello MvvmCross";

		public string ID { 
			get { return _id; }
			set {
				_id = value;
				RaisePropertyChanged (() => ID);
			}
		}

		public IQuestionType Type {
			get;
			set;
		}
	}
}

