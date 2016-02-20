using Plugin.Messaging;

namespace ChGK.Core.Services.Email
{
    public class EmailsService : IEmailsService
    {
        public void SendEmail(string to = null, string subject = null, string message = null)
        {
            if (!MessagingPlugin.EmailMessenger.CanSendEmail)
                return;

            MessagingPlugin.EmailMessenger.SendEmail(to, subject, message);
        }
    }
}