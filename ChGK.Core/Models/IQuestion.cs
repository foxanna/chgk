using System;
using Cirrious.MvvmCross.ViewModels;

namespace ChGK.Core.Models
{
	public interface IQuestion
	{
		string ID { get; }

		string Text { get; }

		string Answer { get; }

		string Comment { get; }

		string Author { get; }

		string Source { get; }

		string Picture { get; }

		string PassCriteria { get; }
	}

	internal class Question : IQuestion
	{
		public string ID { get; set; }

		public string Text { get; set; }

		public string Answer { get; set; }

		public string Comment { get; set; }

		public string Author { get; set; }

		public string Source { get; set; }

		public string Picture { get; set; }

		public string PassCriteria { get; set; }
	}
}

