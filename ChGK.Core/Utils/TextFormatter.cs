using System;
using System.Text.RegularExpressions;

namespace ChGK.Core.Utils
{
	public static class TextFormatter
	{
		const string QuestionPattern = "(?<=,|-|\\)|\\(|\\[|\\]|[а-я]|[А-Я]|[a-z]|[A-Z])( *)(\n)( *)(?=[а-я]|[a-z]|-|\\)|\\(|\\[|\\]|\")";
		const string EditorsPattern = "\n";
		const string CommentsPattern = "\n";

		public static string FormatQuestion (string text)
		{
			text = Regex.Replace (text, @"(\(pic: )\d+\.(jpg\))\n( *)", "");
			return Regex.Replace (text, QuestionPattern, " ");
		}

		public static string GetPicture (string text)
		{
			var regex = new Regex ("(?<=pic: )\\d+\\.(jpg)");
			return regex.Match (text).Value;
		}

		public static string FormatEditors (string text)
		{
			return Regex.Replace (text, EditorsPattern, " ");
		}

		public static string FormatComments (string text)
		{
			return Regex.Replace (text, CommentsPattern, " ");
		}
	}
}

