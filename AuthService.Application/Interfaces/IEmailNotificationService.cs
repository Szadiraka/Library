
using AuthService.Domain.Entities;
using AuthService.Domain.Enums;

namespace AuthService.Application.Interfaces
{
    public  interface IEmailNotificationService
    {

        Task SendEmailAsync(AppUser user, EmailTemplate template, string payload);
    }
}
