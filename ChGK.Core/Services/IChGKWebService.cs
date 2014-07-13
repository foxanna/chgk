using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChGK.Core.Models;

namespace ChGK.Core.Services
{
	public interface IChGKWebService
	{
		Task<List<IQuestion>> GetRandomPackage ();

		Task<List<ITournament>> GetLastAddedTournaments (int? page);

		Task<ITour> GetTourDetails (string filename);
	}
}

