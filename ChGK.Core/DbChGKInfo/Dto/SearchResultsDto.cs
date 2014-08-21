using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using ChGK.Core.DbChGKInfo.Dto;

namespace ChGK.Core.DbChGKInfo.Dto
{
	[XmlRoot ("search")]
    public class SearchResultsDto
	{
		[XmlElement (ElementName = "question")]
        public List<SearchResultDto> questions { get; set; }
	}
}

