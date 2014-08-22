using System.Collections.Generic;
using System.Threading.Tasks;
using ChGK.Core.Models;
using System.Threading;

namespace ChGK.Core.Services
{
	public interface IChGKWebService
	{
		Task<List<IQuestion>> GetRandomPackage (CancellationToken token);

		Task<List<ITournament>> GetLastAddedTournaments (int? page, CancellationToken token);

        Task<ITournament> GetTournament(string filename, CancellationToken token);

		Task<ITour> GetTourDetails (string filename, CancellationToken token);

        Task<List<ISearchQuestionsResult>> SearchQuestions(SearchParams searchParams, CancellationToken token);
	}
}

