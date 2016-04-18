using System.Xml.Serialization;
using ChGK.Core.Models;

namespace ChGK.Core.DbChGKInfo.Dto
{
    public class QuestionDto
    {
        [XmlElement("QuestionId")]
        public string Id { get; set; }

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

        [XmlElement("number")]
        public string Number { get; set; }

        public IQuestion ToModel()
        {
            var pictureId = TextFormatter.GetPicture(Text);

            return new Question
            {
                Id = Id,
                Picture = !string.IsNullOrEmpty(pictureId) ? $"{Utils.Host}/images/db/{pictureId}" : null,
                Gearbox = TextFormatter.GetGearbox(Text),
                Text = TextFormatter.FormatQuestion(Text),
                Answer = Answer,
                PassCriteria = TextFormatter.FormatPassCriteria(PassCriteria),
                Author = TextFormatter.FormatAnswer(Author),
                Comment = TextFormatter.FormatComments(Comment),
                Source = Source,
                Number = Number
            };
        }
    }
}