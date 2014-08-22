using System;
using System.Linq;
using ChGK.Core.Models;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace ChGK.Core.DbChGKInfo.Dto
{
    [XmlRoot("tournament")]
	public class TournamentDto
	{
        [XmlElement("Title")]
        public string Name { get; set; }

        [XmlElement("PlayedAt")]
        public string PlayedAt { get; set; }

        [XmlElement("CreatedAt")]
        public string AddedAt { get; set; }

        [XmlElement("FileName")]
        public string FileName { get; set; }

        [XmlElement(ElementName = "tour")]
        public List<TourDto> Tours;

		public ITournament ToModel ()
		{
			return new Tournament {
                AddedAt = this.AddedAt,
                FileName = this.FileName,
                Name = this.Name,
                PlayedAt = this.PlayedAt,
                Tours = this.Tours.Select(tour => tour.ToModel()).ToList(),
            };
		}
	}
}

