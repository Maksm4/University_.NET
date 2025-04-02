namespace WebApp.externalServices
{
    public class ConsoleEmailSender : IEmailSender
    {
        public void SendEmail(string Content, string emailAddress)
        {
            Console.WriteLine($"to address: {emailAddress}: content: {Content}");
        }
    }
}