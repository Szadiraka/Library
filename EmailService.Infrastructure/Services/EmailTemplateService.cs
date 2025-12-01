using EmailService.Application.Exceptions;
using EmailService.Domain.Interfaces;
using Microsoft.AspNetCore.Hosting;


namespace EmailService.Infrastructure.Services
{
    public class EmailTemplateService : IEmailTemplateService
    {
        private readonly IWebHostEnvironment _env;

        public EmailTemplateService(IWebHostEnvironment env)
        {
            _env = env;
        }


        public async Task<string> RenderTemplateAsync(string templateName, Dictionary<string, string> data)
        {
            var html = await GetTemplateAsync(templateName);

            foreach(var item in data)
            {
                html = html.Replace($"{{{item.Key}}}", item.Value);
            }

            if (html.Contains("{{") && html.Contains("}}"))
                throw new _ValidationException("Не всі плейсхолдери заповненні");
            return html;
        }


        private async Task<string> GetTemplateAsync(string templateName)
        {
            var path = Path.Combine(_env.ContentRootPath,"Templates","Emails", $"{templateName}.html");

            if (!File.Exists(path))
                throw new NotFoundException("Шаблон не знайдений");

            var html = await File.ReadAllTextAsync(path);          

            return html;
        }

       
    }
}
