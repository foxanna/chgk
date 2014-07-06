using System;
using System.Collections.Generic;

namespace ChGK.Core.Models
{
	public interface ITournament
	{
		string FileName { get; }

		string Name { get; }

		string AddedAt { get; }

		string PlayedAt { get; }

		List<ITour> Tours { get; }
	}

	internal class Tournament : ITournament
	{
		public string FileName { get; set; }

		public string Name { get; set; }

		public string AddedAt { get; set; }

		public string PlayedAt { get; set; }

		public List<ITour> Tours { get; set; }
	}
}

