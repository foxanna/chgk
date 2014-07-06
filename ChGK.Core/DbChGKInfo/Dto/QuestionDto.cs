using System;
using System.Xml.Serialization;
using ChGK.Core.Models;

namespace ChGK.Core.DbChGKInfo.Dto
{
	public class QuestionDto
	{
		[XmlElement ("QuestionId")]
		public string ID { get; set; }

		[XmlElement ("Question")]
		public string Text { get; set; }

		[XmlElement ("Answer")]
		public string Answer { get; set; }

		[XmlElement ("Authors")]
		public string Author { get; set; }

		[XmlElement ("Comments")]
		public string Comment { get; set; }

		[XmlElement ("Sources")]
		public string Source { get; set; }

		public IQuestion ToModel ()
		{
			return new Question {
				ID = ID,
				Text = Text,
				Answer = Answer,
				Author = Author,
				Comment = Comment,
				Source = Source
			};
		}
		//		[XmlIgnore]
		//		public IQuestionType Type { get; set; }
	}
}

