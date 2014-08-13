using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChGK.Core.DbChGKInfo.Dto;
using ChGK.Core.Models;
using ChGK.Core.NetworkService;
using ChGK.Core.Services;
using System.Threading;

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
	}
}

