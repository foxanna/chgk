using System;
using System.Xml.Serialization;
using ChGK.Core.Models;
using System.Text.RegularExpressions;
using System.Text;

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
			// TODO: remove \n from text 
			var rg = new Regex (@"(,|\)|\(|\[|\]|-|[a-z]|[A-Z])( *?)(\n)( *?)([a-z])");
			var maches = rg.Matches (Text);

			StringBuilder a = new StringBuilder ();
			foreach (var r in maches) {
				a.Append (r.ToString ());
			}

			return new Question {
				ID = ID,
				Text = Text + "\n" + a.ToString (),
				Answer = Answer,
				Author = Author,
				Comment = Comment,
				Source = Source
			};
		}
	}
}

