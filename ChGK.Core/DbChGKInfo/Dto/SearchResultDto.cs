using System.Xml.Serialization;
using ChGK.Core.Models;

namespace ChGK.Core.DbChGKInfo.Dto
{
    public class SearchResultDto
    {
        [XmlElement("QuestionId")]
        public string Id { get; set; }

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

        public ISearchQuestionsResult ToModel()
        {
            return new SearchQuestionsResult
            {
                Id = Id,
                Number = Number,
                Picture = TextFormatter.GetPicture(Text),
                Text = TextFormatter.FormatQuestion(Text),
                Answer = Answer,
                PassCriteria = TextFormatter.FormatPassCriteria(PassCriteria),
                Author = TextFormatter.FormatAnswer(Author),
                Comment = TextFormatter.FormatComments(Comment),
                Source = Source,
                TourFileName = (!TourFileName.StartsWith("tour/")) ? "tour/" + TourFileName : TourFileName,
                TourName = TourName,
                TournamentFileName = TournamentFileName,
                TournamentName = TournamentName
            };
        }
    }
}