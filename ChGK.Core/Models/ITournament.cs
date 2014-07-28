using System;
using System.Collections.Generic;
using System.Collections;

namespace ChGK.Core.Models
{
	public interface ITournament : IEnumerable<ITour>
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

		public IEnumerator<ITour> GetEnumerator ()
		{
			return Tours.GetEnumerator ();
		}

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return Tours.GetEnumerator ();
		}
	}
}

