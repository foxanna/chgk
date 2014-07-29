using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChGK.Core.DbChGKInfo.Dto;
using ChGK.Core.Models;
using ChGK.Core.NetworkService;
using ChGK.Core.Services;

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

		void PreLoad ()
		{
			if (!_reachabilityService.HasInternet ()) {
				throw new NoConnectionException ();
			}
		}

		public async Task<List<IQuestion>> GetRandomPackage ()
		{
			PreLoad ();

			var randomPackage = await _simpleRestService.GetAsync<RandomPackageDto> (host, 
				                    "xml/random", new XmlDeserializer<RandomPackageDto> ());
			return randomPackage.questions.Select (dto => dto.ToModel ()).ToList ();
		}

		public async Task<List<ITournament>> GetLastAddedTournaments (int? page)
		{
			PreLoad ();

			var lastAddedTournaments = await _simpleRestService.GetAsync<LastAddedTournamentsDto> (host, 
				                           "", new HtmlDeserializer<LastAddedTournamentsDto> ());

			return lastAddedTournaments.Tournaments;
		}

		public async Task<ITour> GetTourDetails (string filename)
		{
			PreLoad ();

			var tourDto = await _simpleRestService.GetAsync<TourDto> (host, 
				              string.Format ("{0}/xml", filename), new XmlDeserializer<TourDto> ());

			return tourDto.ToModel ();
		}
	}
}

