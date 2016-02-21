using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using ChGK.Core.Models;

namespace ChGK.Core.DbChGKInfo.Dto
{
    [XmlRoot("tournament")]
    public class TourDto
    {
        [XmlElement("FileName")]
        public string FileName { get; set; }

        [XmlElement("Editors")]
        public string Editors { get; set; }

        [XmlElement(ElementName = "question")]
        public List<QuestionDto> Questions { get; set; }

        [XmlElement("Title")]
        public string Name { get; set; }

        public ITour ToModel()
        {
            return new Tour
            {
                Name = Name,
                Id = FileName,
                Editors = TextFormatter.FormatEditors(Editors),
                Questions = Questions?.Select(question => question.ToModel())?.ToList(),
                Path = (!FileName.StartsWith("tour/")) ? "tour/" + FileName : FileName
            };
        }
    }
}