
using EmailService.Domain.Entities;
using EmailService.Domain.Queries;

namespace EmailService.Domain.Interfaces
{
    public interface IEmailRepository
    {
        Task AddAsync(EmailMessage message);

        Task<EmailMessage?> GetByIdAsync(Guid id);

        Task UpdateAsync(EmailMessage mressage);

        Task <PagedResult<EmailMessage>> GetMessagesAsync(EmailQuery query);
    }
}
