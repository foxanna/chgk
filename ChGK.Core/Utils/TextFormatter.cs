using System;
using System.Text.RegularExpressions;

namespace ChGK.Core.Utils
{
	public static class TextFormatter
	{
		const string QuestionPattern = "(?<=,|-|\\)|\\(|\\[|\\]|[а-я]|[А-Я]|[a-z]|[A-Z])( *?)(\n)( *?)(?=[а-я]|[a-z]|-|\\)|\\(|\\[|\\]|\")";
		const string EditorsPattern = "\n";
		const string CommentsPattern = "\n";

		public static string FormatQuestion (string text)
		{
			return Format (text, QuestionPattern, " ");
		}

		public static string FormatEditors (string text)
		{
			return Format (text, EditorsPattern, " ");
		}

		public static string FormatComments (string text)
		{
			return Format (text, CommentsPattern, " ");
		}

		private static string Format (string text, string pattern, string replace)
		{
			return Regex.Replace (text, pattern, replace);
		}
	}
}

