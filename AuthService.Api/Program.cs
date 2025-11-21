using AuthService.Api.Extensions;
using AuthService.Api.Middleware;
using AuthService.Api.Services;
using AuthService.Application.Interfaces;
using AuthService.Application.Services;
using AuthService.Domain.Entities;
using AuthService.Infrastructure.Seed;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;


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
            builder.Services.AddScoped<IAuthInterface, _AuthService>();
            builder.Services.AddScoped<IUserContextService, UserContextService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
          


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
