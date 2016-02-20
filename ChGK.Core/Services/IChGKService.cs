using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ChGK.Core.Models;

namespace ChGK.Core.Services
{
    public interface IChGKService
    {
        Task<List<IQuestion>> GetRandomPackage(CancellationToken token);

        Task<List<ITournament>> GetLastAddedTournaments(CancellationToken token, int page);

        Task<ITournament> GetTournament(string filename, CancellationToken token);

        Task<ITour> GetTourDetails(string filename, CancellationToken token);

        Task<List<ISearchQuestionsResult>> SearchQuestions(SearchParams searchParams, CancellationToken token);
    }
}