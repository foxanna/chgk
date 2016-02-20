namespace ChGK.Core.Services.Email
{
    public interface IEmailsService
    {
        void SendEmail(string to = null, string subject = null, string message = null);
    }
}