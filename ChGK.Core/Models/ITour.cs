using System;

namespace ChGK.Core.Models
{
	public interface ITour
	{
		string FileName { get; }

		string Name { get; }

		string Editors { get; }

		int QuestionsCount { get; }
	}

	internal class Tour : ITour
	{
		public string FileName { get; set; }

		public string Name { get; set; }

		public string Editors { get; set; }

		public int QuestionsCount { get; set; }
	}
}

