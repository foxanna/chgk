using System;
using System.Collections.Generic;
using ChGK.Core.Protocol.Entities;

namespace ChGK.Core.DbChGKInfo.Entities
{
	public class ChGKQuestionType : IQuestionType
	{
		public ChGKQuestionType ()
		{
		}

		public string Code {
			get;
			set;
		}

		public override string ToString ()
		{
			return DisplayName;
		}

		public string DisplayName {
			get;
			set;
		}

		public static List<IQuestionType> QuestionTypes = new List<IQuestionType> () {
			new ChGKQuestionType () { Code = "Ч", DisplayName = "Что? Где? Когда?" }, 
			new ChGKQuestionType () { Code = "Б", DisplayName = "Брейн-ринг" }, 
			new ChGKQuestionType () { Code = "И", DisplayName = "Интернет" }, 
			new ChGKQuestionType () { Code = "Л", DisplayName = "Бескрылка" }, 
			new ChGKQuestionType () { Code = "Я", DisplayName = "Своя игра" }, 
			new ChGKQuestionType () { Code = "Э", DisplayName = "Эрудит" }, 
		};
	}
}

