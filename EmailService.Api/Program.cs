using EmailService.Api.Extentions;
using EmailService.Api.Middleware;
using EmailService.Application.Interfaces;
using EmailService.Application.Services;
using EmailService.Domain.Interfaces;
using EmailService.Infrastructure.Services;
using Hangfire;


namespace EmailService.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);   
            builder.Services.AddControllers();
            builder.Services.AddJwtAuth(builder.Configuration);
            builder.Services.AddEmailService(builder.Configuration);

            builder.Services.AddScoped<IEmailService, EmailsService>();
            builder.Services.AddScoped<IEmailRepository, EmailRepository>();
            builder.Services.AddScoped<IEmailSender, EmailSender>();
            builder.Services.AddScoped<IEmailTemplateService, EmailTemplateService>();



            var app = builder.Build();
            app.UseMiddleware<RequestLoggingMiddleware>();
            app.UseMiddleware<GlobalExceptionMiddleware>();


            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHangfireDashboard("/hangfire");
            app.MapControllers();

            app.Run();
        }
    }
}
