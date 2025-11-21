using AuthService.Application.Interfaces;


namespace AuthService.Application.Services
{
    public class EmailService : IEmailService
    {
        public Task SendEmailConfirmationAsync(string toEmail, string confirmationLInk)
        {
            Console.WriteLine($"[Fake Email]  To: {toEmail}, Link: {confirmationLInk}");
            return Task.CompletedTask;
        }

        public Task SendEmailAsync (string toEmail, string message)
        {
            Console.WriteLine(message);
            return Task.CompletedTask;
        }
    }
}
