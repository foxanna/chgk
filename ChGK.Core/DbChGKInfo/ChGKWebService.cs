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

		public async Task<List<IQuestion>> GetRandomPackage (CancellationToken cancellationToken)
		{
			PreLoad (cancellationToken);

			var randomPackage = await _simpleRestService.GetAsync<RandomPackageDto> (host, 
				                    "xml/random", new XmlDeserializer<RandomPackageDto> (), cancellationToken);
			return randomPackage.questions.Select (dto => dto.ToModel ()).ToList ();
		}

		public async Task<List<ITournament>> GetLastAddedTournaments (int? page, CancellationToken cancellationToken)
		{
			PreLoad (cancellationToken);

			var lastAddedTournaments = await _simpleRestService.GetAsync<LastAddedTournamentsDto> (host, 
				                           "", new HtmlDeserializer<LastAddedTournamentsDto> (), cancellationToken);

			return lastAddedTournaments.Tournaments;
		}

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

        Tuple<SearchParams, List<ISearchQuestionsResult>> cashedSearch;

        public async Task<List<ISearchQuestionsResult>> SearchQuestions(SearchParams searchParams, CancellationToken cancellationToken)
        {
            if (cashedSearch != null && cashedSearch.Item1.Equals(searchParams) && cashedSearch.Item1.Page <= searchParams.Page)
            {
				return cashedSearch.Item2.GetRange(searchParams.Page * cashedSearch.Item1.Limit, 
					Math.Min(cashedSearch.Item1.Limit, cashedSearch.Item2.Count - searchParams.Page * cashedSearch.Item1.Limit));
            }

            PreLoad(cancellationToken);

            string url = "xml/search/";

            //if (searchParams.SearchAmongQuestions)
            //{
                url += "questions/"
                    + Uri.EscapeUriString(searchParams.SearchQuery) + "/"
                    + (searchParams.AnyWord ? "any_word/" : "")
                    + searchParams.Type + "/"
                    + (searchParams.HasQuestion ? "Q" : "") + (searchParams.HasAnswer ? "A" : "") + (searchParams.HasPassCriteria ? "Z" : "") + (searchParams.HasComment ? "C" : "") + (searchParams.HasSourse ? "S" : "") + (searchParams.HasAuthors ? "U" : "") + "/"
                    + "from_" + searchParams.StartDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "/"
                    + "to_" + searchParams.EndDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "/"
                    + "limit" + searchParams.Limit
                    + "?page=" + searchParams.Page;
            //}
            //else if (searchParams.SearchAmongTours)
            //{
            //    url += "tours/"
            //        + Uri.EscapeUriString(searchParams.SearchQuery) + "/"
            //        + "from_" + searchParams.StartDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "/"
            //        + "to_" + searchParams.EndDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) 
            //        + "?page=" + searchParams.Page;
            //}
            //else if (searchParams.SearchAmongUnsorted)
            //{
            //    url += "unsorted/"
            //        + Uri.EscapeUriString(searchParams.SearchQuery)
            //        + "?page=" + searchParams.Page;
            //}
            //else
            //{
            //    throw new ArgumentException();
            //}


            var searchResults = await _simpleRestService.GetAsync<SearchResultsDto>(host, url,
                                    new XmlDeserializer<SearchResultsDto>(), cancellationToken);

            var questions = searchResults.questions.Select(dto => dto.ToModel()).ToList();

            if (cashedSearch != null && cashedSearch.Item1.Equals(searchParams))
            {
                cashedSearch.Item2.AddRange(questions);
            }
            else
            {
                cashedSearch = Tuple.Create<SearchParams, List<ISearchQuestionsResult>>(new SearchParams(searchParams), new List<ISearchQuestionsResult>(questions));
            }

            return questions;
        }
	}
}

