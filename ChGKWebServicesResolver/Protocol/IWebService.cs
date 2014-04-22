using System;
using System.Collections.Generic;
using ChGK.Core.Protocol.Entities;

namespace ChGK.Core.Protocol
{
	public interface IWebService
	{
		List<IQuestionType> getQuestionTypes ();
	}
}

