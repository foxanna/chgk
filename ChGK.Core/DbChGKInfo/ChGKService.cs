using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChGK.Core.DbChGKInfo.Dto;
using ChGK.Core.Models;
using ChGK.Core.Services;
using ChGK.Core.Services.Rest;

namespace ChGK.Core.DbChGKInfo
{
    public class NoConnectionException : Exception
    {
    }

    public class ChGKService : IChGKService
    {
        private const string Host = "http://db.chgk.info";

        private readonly IDeviceConnectivityService _reachabilityService;
        private readonly IRestService _restService;

        public ChGKService(IRestService restService, IDeviceConnectivityService reachabilityService)
        {
            _restService = restService;
            _reachabilityService = reachabilityService;
        }

        #region Random package

        public async Task<List<IQuestion>> GetRandomPackage(CancellationToken cancellationToken)
        {
            PreLoad(cancellationToken);

            var randomPackage = await _restService.GetAsync(Host,
                "xml/random", new XmlDeserializer<RandomPackageDto>(), cancellationToken);
            return randomPackage.Questions.Select(dto => dto.ToModel()).ToList();
        }

        #endregion // Random package

        private void PreLoad(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!_reachabilityService.HasInternet())
            {
                throw new NoConnectionException();
            }
        }

        #region Last added tournaments

        private Tuple<int, List<ITournament>> _lastTournamentsCache = Tuple.Create(-1, new List<ITournament>());

        public async Task<List<ITournament>> GetLastAddedTournaments(CancellationToken cancellationToken, int page = 0)
        {
            if (page <= _lastTournamentsCache.Item1)
            {
                return _lastTournamentsCache.Item2;
            }

            try
            {
                PreLoad(cancellationToken);

                var lastAddedTournaments = await LoadNewLastAddedTournaments(cancellationToken, page);
                _lastTournamentsCache = Tuple.Create(page, _lastTournamentsCache.Item2);
                _lastTournamentsCache.Item2.AddRange(lastAddedTournaments.Tournaments);

                return _lastTournamentsCache.Item2;
            }
            catch
            {
                if (_lastTournamentsCache.Item2.Count > 0)
                {
                    return _lastTournamentsCache.Item2;
                }
                throw;
            }
        }

        private Task<LastAddedTournamentsDto> LoadNewLastAddedTournaments(CancellationToken cancellationToken,
            int page = 0)
        {
            return _restService.GetAsync(Host,
                "last?page=" + page, new HtmlDeserializer<LastAddedTournamentsDto>(), cancellationToken);
        }

        #endregion // Last added tournaments

        #region Tours

        public async Task<ITour> GetTourDetails(string filename, CancellationToken cancellationToken)
        {
            var tour = ReadTourFromCache(filename);

            if (tour != null)
                return tour;

            PreLoad(cancellationToken);

            var tourDto = await _restService.GetAsync(Host,
                $"{filename}/xml", new XmlDeserializer<TourDto>(), cancellationToken);

            tour = tourDto.ToModel();
            CacheTour(tour);

            return tour;
        }

        private readonly Dictionary<string, ITour> _cachedTours = new Dictionary<string, ITour>();

        private void CacheTour(ITour tour)
        {
            if (!_cachedTours.ContainsKey(tour.FileName))
                _cachedTours.Add(tour.FileName, tour);
        }

        private ITour ReadTourFromCache(string filename)
        {
            var file = filename.StartsWith("tour/") ? filename.Substring(5) : filename;
            return _cachedTours.FirstOrDefault(t => t.Key == file).Value;
        }

        #endregion // Tours

        #region Tournaments

        private readonly Dictionary<string, ITournament> _cachedTournaments = new Dictionary<string, ITournament>();

        private void CacheTournament(ITournament tournament)
        {
            if (!_cachedTournaments.ContainsKey(tournament.FileName))
                _cachedTournaments.Add(tournament.FileName, tournament);
        }

        private ITournament ReadTournamentFromCache(string filename)
        {
            var file = filename.StartsWith("tour/") ? filename.Substring(5) : filename;
            if (!filename.EndsWith(".txt"))
                file += ".txt";
            return _cachedTournaments.FirstOrDefault(t => t.Key == file).Value;
        }

        public async Task<ITournament> GetTournament(string filename, CancellationToken cancellationToken)
        {
            var tournament = ReadTournamentFromCache(filename);

            if (tournament != null)
                return tournament;

            PreLoad(cancellationToken);

            var tournamentDto = await _restService.GetAsync(Host,
                $"{filename}/xml", new XmlDeserializer<TournamentDto>(), cancellationToken);

            tournament = tournamentDto.ToModel();
            CacheTournament(tournament);

            return tournament;
        }

        #endregion // Tournaments

        #region Search

        private Tuple<SearchParams, List<ISearchQuestionsResult>> _cashedSearch;

        public async Task<List<ISearchQuestionsResult>> SearchQuestions(SearchParams searchParams,
            CancellationToken cancellationToken)
        {
            if (_cashedSearch != null && _cashedSearch.Item1.Equals(searchParams) &&
                _cashedSearch.Item1.Page >= searchParams.Page)
                return _cashedSearch.Item2;

            PreLoad(cancellationToken);

            var url = "xml/search/questions/"
                      + Uri.EscapeUriString(searchParams.SearchQuery) + "/"
                      + (searchParams.AnyWord ? "any_word/" : "")
                      + searchParams.Type + "/"
                      + (searchParams.HasQuestion ? "Q" : "") + (searchParams.HasAnswer ? "A" : "") +
                      (searchParams.HasPassCriteria ? "Z" : "") + (searchParams.HasComment ? "C" : "") +
                      (searchParams.HasSourse ? "S" : "") + (searchParams.HasAuthors ? "U" : "") + "/"
                      + "from_" + searchParams.StartDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "/"
                      + "to_" + searchParams.EndDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "/"
                      + "limit" + searchParams.Limit
                      + "?page=" + searchParams.Page;

            var searchResults = await _restService.GetAsync(Host, url,
                new XmlDeserializer<SearchResultsDto>(), cancellationToken);

            var questions = searchResults.Questions.Select(dto => dto.ToModel()).ToList();

            if (_cashedSearch != null && _cashedSearch.Item1.Equals(searchParams))
            {
                _cashedSearch.Item1.Page = searchParams.Page;
                _cashedSearch.Item2.AddRange(questions);
                questions = _cashedSearch.Item2;
            }
            else
            {
                _cashedSearch = Tuple.Create(new SearchParams(searchParams), new List<ISearchQuestionsResult>(questions));
            }

            return questions;
        }

        #endregion // Search
    }
}