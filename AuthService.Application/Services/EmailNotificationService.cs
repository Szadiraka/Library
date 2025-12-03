
using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Domain.Enums;
using AuthService.Domain.Exceptions;
using AuthService.Domain.Interfaces;

namespace AuthService.Application.Services
{
    public class EmailNotificationService : IEmailNotificationService
    {
        private readonly IEmailMicroserviceClient _sender;
        private readonly IJwtTokenService _jwtTokenService;

        public EmailNotificationService(IEmailMicroserviceClient sender, IJwtTokenService jwtTokenService)
        {
            _sender = sender;
            _jwtTokenService = jwtTokenService;
        }

        public async  Task SendEmailAsync (AppUser user, EmailTemplate template, string payload)
        {
           var dto = CreateEmailRequest(user, template, payload);

            var apiToken = _jwtTokenService.GenerateTokenForEmailService(); 

            await _sender.SendEmailRequestAsync(dto, apiToken);           
       
       
        }


        private EmailRequest CreateEmailRequest(AppUser user, EmailTemplate template, string payload)
        {
            EmailMessage dto = new EmailMessage
            {
                UserId = Guid.Parse(user.Id),
                To = user.Email ?? throw new NotFoundException("Eлектронна адреса користувача не знайдена"),              
                Body = "body",
            };

            var Data = new Dictionary<string, string>();
            Data.Add("Year", DateTime.UtcNow.Year.ToString());

            switch (template)
            {
                case EmailTemplate.EmailConfirmation:
                    dto.Subject ="Підтвердіть електронну адресу";
                    Data.Add("ConfirmLink", payload);
                    Data.Add("UserName", user.FirstName + " " + user.LastName);
                       
                    break;
                case EmailTemplate.ConfirmedEmail:
                    dto.Subject = "Повідомлення про підтвердження електроної адреси";
                    Data.Add("Message", payload);
                    break;
                case EmailTemplate.PasswordForgot:
                    dto.Subject = "Запит на зміну паролю";
                    Data.Add("ResetLink", payload);
                    break;
                case EmailTemplate.ResetPassword:
                    dto.Subject = "Успішне змінення паролю";
                    Data.Add("Message", payload);
                    break;
                case EmailTemplate.ChangeEmail:
                    dto.Subject = "Зміна електроної адреси";
                    Data.Add("Token", payload);
                    Data.Add("Message", "Для зміни вашої електронної адреси у додатку 'Library' скопіюйте наданий пароль");
                    break;
                default:
                    dto.Subject = "Неважливе повідомлення";
                    break;
                   
            }

            var request = new EmailRequest
            {
                Dto = dto,
                Data = Data,
                Template = template

            };
            return request;


        }



    }
    
}
