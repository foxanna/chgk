using MvvmCross.Plugins.Messenger;

namespace ChGK.Core.Services.Messenger
{
    public class CloseDrawerMessage : MvxMessage
    {
        public CloseDrawerMessage(object sender) : base(sender)
        {
        }
    }
}