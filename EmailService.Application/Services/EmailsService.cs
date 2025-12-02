using EmailService.Application.DTOs;
using EmailService.Application.Exceptions;
using EmailService.Application.Interfaces;
using EmailService.Application.Mapper;
using EmailService.Domain.Interfaces;
using EmailService.Domain.Queries;


namespace EmailService.Application.Services
{
    public class EmailsService : IEmailService
    {
        private readonly IEmailSender _sender;
        private readonly IEmailRepository _repository;
        private readonly IEmailTemplateService _templateService;

        public EmailsService(IEmailSender sender, IEmailRepository repository, IEmailTemplateService templateService)
        {
            _sender = sender;
            _repository = repository;
            _templateService = templateService;
        }

        public async Task ProcessEmailAsync(Guid messageId, string template, Dictionary<string, string> data)
        {
           var message = await _repository.GetByIdAsync(messageId);
            if (message == null)
                throw new NotFoundException("повідомлення не знайдено");

          
            try
            {
                var html = await _templateService.RenderTemplateAsync(template, data);             

                await _sender.SendEmailAsync(message.To, message.Subject, html);
                message.IsSent = true;
            }
            catch (Exception ex)
            {
                message.ErrorMessage = ex.Message;
                message.IsSent = false;                   
            }
            finally
            {
                message.SentAt = DateTime.Now;
                await _repository.UpdateAsync(message);
                if(message.IsSent == false)
                    throw new ConflictException(message.ErrorMessage??"неможливо відправити повідомлення");
            }         

        }

        public async Task<Guid> AddEmailAsync(EmailMessageDto dto)
        {
           var message = EmailMapper.ToEmailMessage(dto);
          
            await _repository.AddAsync(message);

            return message.Id;
        }

        public async Task<EmailMessageDto> GetByIdAsync(Guid id)
        {
           var result = await _repository.GetByIdAsync(id);         

            return EmailMapper.ToEmailMessageDto(result);
        }

        public async Task UpdateAsync(Guid id, EmailMessageDto dto)
        {           

            var message = EmailMapper.ToEmailMessage(dto);
            message.Id = id;

            await _repository.UpdateAsync(message);
        }

        public async Task<PagedResult<EmailMessageDto>> GetMessagesAsync(EmailQuery query)
        {
            var result = await _repository.GetMessagesAsync(query);
            return EmailMapper.MapToDtoList(result);
        }

        public async Task DeleteAsync(Guid id)
        {
          await _repository.DeleteAsync(id);
        }
    }
}
