using MvvmCross.Plugins.Messenger;

namespace ChGK.Core.Messages
{
    public class ResultsChangedMessage : MvxMessage
    {
        public static string ResultsCleared = "ResultsCleared";

        public ResultsChangedMessage(object sender, string questionID) : base(sender)
        {
            QuestionID = questionID;
        }

        public string QuestionID { get; private set; }
    }
}