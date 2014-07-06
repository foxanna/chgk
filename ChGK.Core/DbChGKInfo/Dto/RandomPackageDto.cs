using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using ChGK.Core.DbChGKInfo.Dto;

namespace ChGK.Core.DbChGKInfo.Dto
{
	[XmlRoot ("search")]
	public class RandomPackageDto
	{
		[XmlElement (ElementName = "question")]
		public List<QuestionDto> questions { get; set; }
	}
}

