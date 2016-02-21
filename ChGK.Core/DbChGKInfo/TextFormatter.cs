using System.Text.RegularExpressions;

namespace ChGK.Core.DbChGKInfo
{
    public static class TextFormatter
    {
        public static string FormatQuestion(string text)
        {
            if (text == null)
                return null;

            text = Regex.Replace(text, @"(\(pic: )\d+\.\w*(\))\n( *)", ""); // remove (pic:...) tag
            text = Regex.Replace(text, @"(<раздатка>)[\s\S]*(<\/раздатка>)", ""); // remove <раздатка> tag

            var questionPattern =
                "(?<=,|-|\\)|\\(|\\[|\\]|[а-я]|[А-Я]|[a-z]|[A-Z]|[0-9])( *)(\n)( *)(?=[а-я]|[a-z]|-|\\)|\\(|\\[|\\]|\")";
            text = Regex.Replace(text, questionPattern, " ");

            questionPattern = "( *)(\n)( *)(?=[A-Z]{2,}|[А-Я]{2,}|[0-9]{2,})";
            text = Regex.Replace(text, questionPattern, " ");

            return text.Trim();
        }

        public static string GetPicture(string text)
        {
            if (text == null)
                return null;

            var regex = new Regex("(?<=pic: )\\d+\\.\\w*(?=\\))");
            return regex.Match(text).Value.Trim();
        }

        public static string FormatEditors(string text)
        {
            return text == null ? null : Regex.Replace(text, "\n", " ").Trim();
        }

        public static string FormatComments(string text)
        {
            if (text == null)
                return null;

            const string commentPattern =
                "( *)(\n)( *)(?=[A-Z]{2,}|[А-Я]{2,}|[0-9]{2,}|[а-я]|[a-z]-|\\)|\\(|\\[|\\]|\")";
            return Regex.Replace(text, commentPattern, " ").Trim();
        }

        public static string FormatAnswer(string text)
        {
            const string answerPattern = "( *)(\n)( *)(?=[A-Z]{2,}|[А-Я]{2,}|[0-9]{2,}|[а-я]|[a-z]-|\\)|\\(|\\[|\\]|\")";
            return Regex.Replace(text, answerPattern, " ").Trim();
        }

        public static string FormatPassCriteria(string text)
        {
            return Regex.Replace(text, "\n", " ").Trim();
        }

        public static string GetGearbox(string text)
        {
            text = Regex.Replace(text, @"(\(pic: )\d+\.\w*(\))\n( *)", ""); // remove (pic:...) tag
            var regex = new Regex(@"(?<=<раздатка>)[\s\S]*(?=</раздатка>)");
            return regex.Match(text).Value.Trim();
        }
    }
}