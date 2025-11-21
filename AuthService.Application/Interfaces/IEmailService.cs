

namespace AuthService.Application.Interfaces
{
    public  interface IEmailService
    {
        Task SendEmailConfirmationAsync(string toEmail, string confirmationLInk);

        Task SendEmailAsync(string toEmail, string message);
    }
}
