using System;
using ChGK.Core.Models;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;
using ChGK.Core.Utils;

namespace ChGK.Core.DbChGKInfo.Dto
{
	[XmlRoot ("tournament")]
	public class TourDto
	{
		[XmlElement ("Title")]
		public string Name { get; set; }

		[XmlElement ("FileName")]
		public string FileName { get; set; }

		[XmlElement ("Editors")]
		public string Editors { get; set; }

		[XmlElement (ElementName = "question")]
		public List<QuestionDto> Questions;

		public ITour ToModel ()
		{
			return new Tour { 
				Name = Name, 
				FileName = FileName, 
				Editors = TextFormatter.FormatEditors (Editors), 
				Questions = Questions.Select (question => question.ToModel ()).ToList (),
			};
		}
	}
}

