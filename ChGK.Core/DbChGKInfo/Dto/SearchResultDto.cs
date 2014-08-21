using ChGK.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ChGK.Core.DbChGKInfo.Dto
{
    public class SearchResultDto
    {
        [XmlElement("QuestionId")]
        public string ID { get; set; }

        [XmlElement("Number")]
        public int Number { get; set; }

        [XmlElement("Question")]
        public string Text { get; set; }

        [XmlElement("Answer")]
        public string Answer { get; set; }

        [XmlElement("Authors")]
        public string Author { get; set; }

        [XmlElement("PassCriteria")]
        public string PassCriteria { get; set; }

        [XmlElement("Comments")]
        public string Comment { get; set; }

        [XmlElement("Sources")]
        public string Source { get; set; }

        [XmlElement("tourTitle")]
        public string TourName { get; set; }

        [XmlElement("tourFileName")]
        public string TourFileName { get; set; }

        [XmlElement("tournamentTitle")]
        public string TournamentName { get; set; }

        [XmlElement("tournamentFileName")]
        public string TournamentFileName { get; set; }

        public ISearchResult ToModel()
        {
            return new SearchResult
            {
                ID = ID,
                Number = Number,
                Picture = TextFormatter.GetPicture(Text),
                Text = TextFormatter.FormatQuestion(Text),
                Answer = Answer,
                PassCriteria = TextFormatter.FormatPassCriteria(PassCriteria),
                Author = TextFormatter.FormatAnswer(Author),
                Comment = TextFormatter.FormatComments(Comment),
                Source = Source,
                TourFileName = TourFileName,
                TourName = TourName,
                TournamentFileName = TournamentFileName,
                TournamentName = TournamentName
            };
        }
    }
}
