

using EmailService.Application.DTOs;
using EmailService.Domain.Entities;
using EmailService.Domain.Interfaces;

namespace EmailService.Application.Mapper
{
    public static class EmailMapper
    {
        public static EmailMessage ToEmailMessage(EmailMessageDto dto)
        {         
            return new EmailMessage
            {
                Body = dto.Body,
                Subject = dto.Subject,
                To = dto.To,
                UserId = dto.UserId,
                IsSent = dto.IsSent,
                CreatedAt = dto.CreatedAt ?? DateTimeOffset.Now ,

            };
        }

        public static EmailMessageDto ToEmailMessageDto(EmailMessage entity)
        {
            return new EmailMessageDto
            {
                Id = entity.Id,
                Body = entity.Body,
                Subject = entity.Subject,
                To = entity.To,
                UserId = entity.UserId,
                IsSent = entity.IsSent,
                CreatedAt = entity.CreatedAt,
                SentAt = entity.SentAt,
                ErrorMessage = entity.ErrorMessage

            };
        }


        public static PagedResult<EmailMessageDto> MapToDtoList(PagedResult<EmailMessage> entities)
        {
            return new PagedResult<EmailMessageDto>
            {
                Page = entities.Page,
                PageSize = entities.PageSize,
                Items = entities.Items.Select(x => ToEmailMessageDto(x)).ToList(),
                TotalCount = entities.TotalCount
            };

        }

    }
}
