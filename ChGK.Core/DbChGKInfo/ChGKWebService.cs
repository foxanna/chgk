using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChGK.Core.DbChGKInfo.Dto;
using ChGK.Core.Models;
using ChGK.Core.NetworkService;
using ChGK.Core.Services;
using System.Threading;
using System.Net;
using System.Globalization;

namespace ChGK.Core.DbChGKInfo
{
	public class NoConnectionException : Exception
	{
	}

	public class ChGKWebService : IChGKWebService
	{
		readonly ISimpleRestService _simpleRestService;
		readonly IDeviceConnectivityService _reachabilityService;

		const string host = "http://db.chgk.info";

		public ChGKWebService (ISimpleRestService simpleRestService, IDeviceConnectivityService reachabilityService)
		{
			_simpleRestService = simpleRestService;
			_reachabilityService = reachabilityService;
		}

		void PreLoad (CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested ();

			if (!_reachabilityService.HasInternet ()) {
				throw new NoConnectionException ();
			}
		}

        #region Random package

        public async Task<List<IQuestion>> GetRandomPackage (CancellationToken cancellationToken)
		{
			PreLoad (cancellationToken);

			var randomPackage = await _simpleRestService.GetAsync<RandomPackageDto> (host, 
				                    "xml/random", new XmlDeserializer<RandomPackageDto> (), cancellationToken);
			return randomPackage.questions.Select (dto => dto.ToModel ()).ToList ();
		}

        #endregion // Random package

        #region Last added tournaments

        Tuple<int, List<ITournament>> _lastTournamentsCache = Tuple.Create(-1, new List<ITournament>());

        public async Task<List<ITournament>> GetLastAddedTournaments(CancellationToken cancellationToken, int page = 0)
		{
			PreLoad (cancellationToken);

            if (page <= _lastTournamentsCache.Item1)
            {
                return _lastTournamentsCache.Item2;
            }

            var lastAddedTournaments = await LoadNewLastAddedTournaments(cancellationToken, page);
            _lastTournamentsCache = Tuple.Create(page, _lastTournamentsCache.Item2);
            _lastTournamentsCache.Item2.AddRange(lastAddedTournaments.Tournaments);

            return _lastTournamentsCache.Item2;
		}

        Task<LastAddedTournamentsDto> LoadNewLastAddedTournaments(CancellationToken cancellationToken, int page = 0)
        {
            return _simpleRestService.GetAsync<LastAddedTournamentsDto>(host,
                                           "last?page=" + page, new HtmlDeserializer<LastAddedTournamentsDto>(), cancellationToken);
        }

        #endregion // Last added tournaments

        public async Task<ITour> GetTourDetails (string filename, CancellationToken cancellationToken)
		{
            var tour = ReadTourFromCache(filename);

            if (tour == null)
            {
                PreLoad(cancellationToken);

                var tourDto = await _simpleRestService.GetAsync<TourDto>(host,
                                  string.Format("{0}/xml", filename), new XmlDeserializer<TourDto>(), cancellationToken);

                tour = tourDto.ToModel();
                CacheTour(tour);
            }
            
            return tour;
		}

        Dictionary<string, ITour> cachedTours = new Dictionary<string, ITour>();

        void CacheTour(ITour tour)
        {
            if (!cachedTours.ContainsKey(tour.FileName))
            {
                cachedTours.Add(tour.FileName, tour);
            }
        }

        ITour ReadTourFromCache(string filename)
        {
            var file = filename.StartsWith("tour/") ? filename.Substring(5) : filename;
            return cachedTours.FirstOrDefault(t => t.Key == file).Value;
        }

        Dictionary<string, ITournament> cachedTournaments = new Dictionary<string, ITournament>();

        void CacheTournament(ITournament tournament)
        {
            if (!cachedTournaments.ContainsKey(tournament.FileName))
            {
                cachedTournaments.Add(tournament.FileName, tournament);
            }
        }

        ITournament ReadTournamentFromCache(string filename)
        {
            var file = filename.StartsWith("tour/") ? filename.Substring(5) : filename;
            if (!filename.EndsWith(".txt")) {
                file += ".txt";
            }
            return cachedTournaments.FirstOrDefault(t => t.Key == file).Value;
        }

        Tuple<SearchParams, List<ISearchQuestionsResult>> cashedSearch;

        public async Task<List<ISearchQuestionsResult>> SearchQuestions(SearchParams searchParams, CancellationToken cancellationToken)
        {
            if (cashedSearch != null && cashedSearch.Item1.Equals(searchParams) && cashedSearch.Item1.Page >= searchParams.Page)
            {
				return cashedSearch.Item2;
            }

            PreLoad(cancellationToken);

            string url = "xml/search/questions/"
                    + Uri.EscapeUriString(searchParams.SearchQuery) + "/"
                    + (searchParams.AnyWord ? "any_word/" : "")
                    + searchParams.Type + "/"
                    + (searchParams.HasQuestion ? "Q" : "") + (searchParams.HasAnswer ? "A" : "") + (searchParams.HasPassCriteria ? "Z" : "") + (searchParams.HasComment ? "C" : "") + (searchParams.HasSourse ? "S" : "") + (searchParams.HasAuthors ? "U" : "") + "/"
                    + "from_" + searchParams.StartDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "/"
                    + "to_" + searchParams.EndDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "/"
                    + "limit" + searchParams.Limit
                    + "?page=" + searchParams.Page;
            
            var searchResults = await _simpleRestService.GetAsync<SearchResultsDto>(host, url,
                                    new XmlDeserializer<SearchResultsDto>(), cancellationToken);

            var questions = searchResults.questions.Select(dto => dto.ToModel()).ToList();

            if (cashedSearch != null && cashedSearch.Item1.Equals(searchParams))
            {
                cashedSearch.Item1.Page = searchParams.Page;
                cashedSearch.Item2.AddRange(questions);
                questions = cashedSearch.Item2;
            }
            else
            {
                cashedSearch = Tuple.Create<SearchParams, List<ISearchQuestionsResult>>(new SearchParams(searchParams), new List<ISearchQuestionsResult>(questions));
            }

            return questions;
        }

        public async Task<ITournament> GetTournament(string filename, CancellationToken cancellationToken)
        {
            var tournament = ReadTournamentFromCache(filename);

            if (tournament == null)
            {
                PreLoad(cancellationToken);

                var tournamentDto = await _simpleRestService.GetAsync<TournamentDto>(host,
                                  string.Format("{0}/xml", filename), new XmlDeserializer<TournamentDto>(), cancellationToken);
                
                tournament = tournamentDto.ToModel();
                CacheTournament(tournament);
            }

            return tournament;
        }
	}
}

