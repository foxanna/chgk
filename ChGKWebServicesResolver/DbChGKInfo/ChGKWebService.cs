using System;
using ChGK.Core.Protocol;
using ChGK.Core.Protocol.Entities;
using ChGK.Core.DbChGKInfo.Entities;

namespace ChGK.Core.DbChGKInfo
{
	public class ChGKWebService : IWebService
	{
		public ChGKWebService ()
		{
		}

		public System.Collections.Generic.List<IQuestionType> getQuestionTypes ()
		{
			return ChGKQuestionType.QuestionTypes;
		}
	}
}

