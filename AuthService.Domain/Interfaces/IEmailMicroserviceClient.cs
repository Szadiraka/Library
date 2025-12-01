

using AuthService.Domain.Entities;

namespace AuthService.Domain.Interfaces
{
    public interface IEmailMicroserviceClient
    {
        Task SendEmailRequestAsync(EmailRequest request, string apiToken);
    }
}
