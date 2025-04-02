namespace WebApp.externalServices
{
    public interface IEmailSender
    {
        public void SendEmail(string Content, string emailAddress);
    }
}