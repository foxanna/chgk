using System;
using Cirrious.MvvmCross.ViewModels;

namespace ChGK.Core.Protocol.Entities
{
	public interface IQuestion : IMvxViewModel
	{
		String ID { get; }

		IQuestionType Type { get; }
	}
}

