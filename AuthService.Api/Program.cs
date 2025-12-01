using AuthService.Api.Extensions;
using AuthService.Api.Middleware;
using AuthService.Api.Services;
using AuthService.Application.Interfaces;
using AuthService.Application.Services;
using AuthService.Domain.Interfaces;
using AuthService.Infrastructure.Services;

namespace AuthService.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddCustomAuthentication(builder.Configuration);
            builder.Services.AddAuthorization();

            builder.Services.AddAuthDatabase(builder.Configuration);
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
            builder.Services.AddScoped<IUserContextService, UserContextService>();
            builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();
        
            builder.Services.AddScoped<IAuthInterface, Application.Services.AuthService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IEmailNotificationService, EmailNotificationService>();
            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddHttpClient<IEmailMicroserviceClient, EmailMicroserviceClient>(client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["Client:Url_EmailService"]!);
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            var app = builder.Build();

            app.UseMiddleware<GlobalExceptionMiddleware>();
            app.UseMiddleware<RequestLoggingMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHttpsRedirection();

            app.MapControllers();

            await app.SeedIdentity();

            app.Run();
        }
    }
}
