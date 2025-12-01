using EmailService.Domain.Interfaces;
using MimeKit;
using MailKit.Net.Smtp;
using EmailService.Domain.Settings;
using Microsoft.Extensions.Options;


namespace EmailService.Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _settings;
    

        public EmailSender(IOptions<EmailSettings> options)
        {
            _settings = options.Value;
            
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Library", _settings.From));
            message.To.Add(new MailboxAddress("", to));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = body };

            using var client = new SmtpClient();

            await client.ConnectAsync(_settings.Host,_settings.Port, _settings.SSL);

            await client.AuthenticateAsync(_settings.Username, _settings.Password);

            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }


            
        
    }
}
