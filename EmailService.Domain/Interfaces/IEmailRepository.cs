
using EmailService.Domain.Entities;
using EmailService.Domain.Queries;

namespace EmailService.Domain.Interfaces
{
    public interface IEmailRepository
    {
        Task AddAsync(EmailMessage message);

        Task<EmailMessage> GetByIdAsync(Guid id);

        Task UpdateAsync(EmailMessage message);

        Task <PagedResult<EmailMessage>> GetMessagesAsync(EmailQuery query);

        Task DeleteAsync(Guid id);
    }
}
