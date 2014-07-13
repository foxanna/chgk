using System;
using Cirrious.MvvmCross.ViewModels;

namespace ChGK.Core.Models
{
	public interface IQuestion
	{
		String ID { get; }

		//		IQuestionType Type { get; }

		String Text { get; }

		String Answer { get; }

		String Comment { get; }

		String Author { get; }

		String Source { get; }
	}

	internal class Question : IQuestion
	{
		public string ID { get; set; }

		public string Text { get; set; }

		public string Answer { get; set; }

		public string Comment { get; set; }

		public string Author { get; set; }

		public string Source { get; set; }
	}
}

