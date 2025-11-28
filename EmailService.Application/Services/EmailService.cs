using EmailService.Application.Interfaces;
using EmailService.Domain.Entities;
using EmailService.Domain.Interfaces;


namespace EmailService.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IEmailSender _sender;
        private readonly IEmailRepository _repository;

        public EmailService(IEmailSender sender, IEmailRepository repository)
        {
            _sender = sender;
            _repository = repository;
        }

        public async Task ProcessEmailAsync(Guid messageId)
        {
           var message = await _repository.GetByIdAsync(messageId);
            if (message == null)
            {
                // сгенерировать исключение
            }
            try
            {
                await _sender.SendEmailAsync(message.To, message.Subject, message.Body);

                message.IsSent = true;
                message.SentAt = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                message.ErrorMessage = ex.Message;
            }

            await _repository.UpdateAsync(message);

        }

        public async Task<Guid> QueueEmailAsync(string to, string subject, string body)
        {
            var message = new EmailMessage
            {
                To = to,
                Subject = subject,
                Body = body
            };
            // создать EmailMessage - использовать Mapper
            await _repository.AddAsync(message);

            return message.Id;
        }

        
    }
}
