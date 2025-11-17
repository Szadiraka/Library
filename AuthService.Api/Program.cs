using AuthService.Api.Extensions;
using AuthService.Api.Middleware;
using AuthService.Application.Interfaces;
using AuthService.Application.Services;
using AuthService.Domain.Entities;
using AuthService.Infrastructure.Seed;
using Microsoft.AspNetCore.Identity;


namespace AuthService.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

      
            builder.Services.AddAuthDatabase(builder.Configuration);
            builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
            builder.Services.AddScoped<IAuthInterface, AuthUserService>();
       

            builder.Services.AddControllers();


            var app = builder.Build(); 
            
            app.UseMiddleware<RequestLoggingMiddleware>();
            app.UseMiddleware<GlobalExceptionMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthorization();
        

            app.MapControllers();

            //додаємо ролі та адміна при запуску додатку
            await app.SeedIdentity();

            app.Run();
        }
    }
}
