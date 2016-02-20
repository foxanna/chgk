using MvvmCross.Plugins.Messenger;

namespace ChGK.Core.Services.Messenger
{
    public class ResultsChangedMessage : MvxMessage
    {
        public static string ResultsCleared = "ResultsCleared";

        public ResultsChangedMessage(object sender, string questionId) : base(sender)
        {
            QuestionId = questionId;
        }

        public string QuestionId { get; private set; }
    }
}