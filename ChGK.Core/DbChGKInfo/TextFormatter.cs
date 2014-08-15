using System.Text.RegularExpressions;

namespace ChGK.Core.DbChGKInfo
{
	public static class TextFormatter
	{
		public static string FormatQuestion (string text)
		{
			text = Regex.Replace (text, @"(\(pic: )\d+\.\w*(\))\n( *)", ""); // remove (pic:...) tag

			var questionPattern = "(?<=,|-|\\)|\\(|\\[|\\]|[а-я]|[А-Я]|[a-z]|[A-Z]|[0-9])( *)(\n)( *)(?=[а-я]|[a-z]|-|\\)|\\(|\\[|\\]|\")";
			text = Regex.Replace (text, questionPattern, " ");

			questionPattern = "( *)(\n)( *)(?=[A-Z]{2,}|[А-Я]{2,}|[0-9]{2,})";
			text = Regex.Replace (text, questionPattern, " ");

			return text;
		}

		public static string GetPicture (string text)
		{
			var regex = new Regex ("(?<=pic: )\\d+\\.\\w*(?=\\))");
			return regex.Match (text).Value;
		}

		public static string FormatEditors (string text)
		{
			return Regex.Replace (text, "\n", " ");
		}

		public static string FormatComments (string text)
		{
            var commentPattern = "( *)(\n)( *)(?=[A-Z]{2,}|[А-Я]{2,}|[0-9]{2,})";
            return Regex.Replace(text, commentPattern, " ");
		}

		public static string FormatAnswer (string text)
		{
            var answerPattern = "( *)(\n)( *)(?=[A-Z]{2,}|[А-Я]{2,}|[0-9]{2,})";
            return Regex.Replace(text, answerPattern, " ");
		}

        public static string FormatPassCriteria(string text)
        {
            return Regex.Replace(text, "\n", " ");
        }
	}
}

