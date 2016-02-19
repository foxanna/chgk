using System.Collections.Generic;
using System.Xml.Serialization;

namespace ChGK.Core.DbChGKInfo.Dto
{
    [XmlRoot("search")]
    public class SearchResultsDto
    {
        [XmlElement(ElementName = "question")]
        public List<SearchResultDto> Questions { get; set; }
    }
}