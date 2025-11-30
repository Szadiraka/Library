
using EmailService.Application.DTOs;
using EmailService.Domain.Interfaces;
using EmailService.Domain.Queries;

namespace EmailService.Application.Interfaces
{
    public interface IEmailService
    {
        Task<Guid> AddEmailAsync(EmailMessageDto dto);

        Task ProcessEmailAsync(Guid messageId);

        Task<EmailMessageDto> GetByIdAsync(Guid id);

        Task UpdateAsync(Guid id, EmailMessageDto dto);

        Task<PagedResult<EmailMessageDto>> GetMessagesAsync(EmailQuery query);

        Task DeleteAsync(Guid id);
    }
}
