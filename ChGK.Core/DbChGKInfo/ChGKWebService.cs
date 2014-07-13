using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ChGK.Core.DbChGKInfo.Dto;
using ChGK.Core.Models;
using ChGK.Core.NetworkService;
using ChGK.Core.Services;

namespace ChGK.Core.DbChGKInfo
{
	public class ChGKWebService : IChGKWebService
	{
		private readonly ISimpleRestService _simpleRestService;
		private const string host = "http://db.chgk.info";

		public ChGKWebService (ISimpleRestService simpleRestService)
		{
			_simpleRestService = simpleRestService;
		}
		/*	public List<IQuestionType> getQuestionTypes ()
		{
			return ChGKQuestionType.QuestionTypes;
		}*/
		public async Task<List<IQuestion>> GetRandomPackage ()
		{
			var randomPackage = await _simpleRestService.GetAsync<RandomPackageDto> (host, 
				                    "xml/random", new XmlDeserializer<RandomPackageDto> ());
			return randomPackage.questions.Select (dto => dto.ToModel ()).Cast<IQuestion> ().ToList ();
		}

		public async Task<List<ITournament>> GetLastAddedTournaments (int? page)
		{
			var lastAddedTournaments = await _simpleRestService.GetAsync<LastAddedTournamentsDto> (host, 
				                           "", new HtmlDeserializer<LastAddedTournamentsDto> ());

			return lastAddedTournaments.Tournaments;
		}

		public async Task<ITour> GetTourDetails (string filename)
		{
			var tourDto = await _simpleRestService.GetAsync<TourDto> (host, 
				              string.Format ("{0}/xml", filename), new XmlDeserializer<TourDto> ());

			return tourDto.ToModel ();
		}
	}
}

