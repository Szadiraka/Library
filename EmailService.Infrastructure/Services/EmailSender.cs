using EmailService.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;


namespace EmailService.Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;

        public EmailSender(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("My App", _config["Email:From"]));
            message.To.Add(new MailboxAddress("", to));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = body };

            using var client = new SmtpClient();

            await client.ConnectAsync(
                _config["Email:Smtp:Host"],
                int.Parse(_config["Email:Smtp:Port"]),
                bool.Parse(_config["Email:Smtp:SSl"]));

            await client.AuthenticateAsync(
                _config["Email:Smtp:Username"],
                _config["Email:Smtp:Password"]
            );

            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }


            
        
    }
}
