using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using ChGK.Core.Models;

namespace ChGK.Core.DbChGKInfo.Dto
{
    [XmlRoot("tournament")]
    public class TournamentDto
    {
        [XmlElement("TextId")]
        public string Id { get; set; }

        [XmlElement("CreatedAt")]
        public string AddedAt { get; set; }

        [XmlElement(ElementName = "tour")]
        public List<TourDto> Tours { get; set; }

        [XmlElement("Title")]
        public string Name { get; set; }

        [XmlElement("PlayedAt")]
        public string PlayedAt { get; set; }

        public ITournament ToModel()
        {
            return new Tournament
            {
                Id = Id,
                Name = Name,
                AddedAt = AddedAt,
                PlayedAt = PlayedAt,
                Tours = Tours.Select(tour => tour.ToModel()).ToList()
            };
        }
    }
}