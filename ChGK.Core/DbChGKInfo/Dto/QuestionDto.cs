using System;
using System.Xml.Serialization;
using ChGK.Core.Models;
using System.Text.RegularExpressions;
using System.Text;
using ChGK.Core.Utils;

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
				Picture = TextFormatter.GetPicture (Text),
				Text = TextFormatter.FormatQuestion (Text),
				Answer = Answer,
				Author = Author,
				Comment = TextFormatter.FormatComments (Comment),
				Source = Source,
			};
		}
	}
}

