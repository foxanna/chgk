using MvvmCross.Plugins.Messenger;

namespace ChGK.Core.Services.Messenger
{
    public class TeamsChangedMessage : MvxMessage
    {
        public TeamsChangedMessage(object sender) : base(sender)
        {
        }
    }
}