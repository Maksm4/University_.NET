using Microsoft.AspNetCore.Identity.UI.Services;

namespace WebApp.externalServices
{
    public class ConsoleEmialSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            Console.WriteLine($"email sent to: {email} subject: {subject} {htmlMessage}");
            return Task.CompletedTask;
        }
    }
}
