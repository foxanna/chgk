using System.Collections.Generic;
using System.Xml.Serialization;

namespace ChGK.Core.DbChGKInfo.Dto
{
    [XmlRoot("search")]
    public class RandomPackageDto
    {
        [XmlElement(ElementName = "question")]
        public List<QuestionDto> Questions { get; set; }
    }
}