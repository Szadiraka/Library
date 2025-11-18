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
       

            builder.Services.AddControllers();
            builder.Services.AddAuthentication();
            builder.Services.AddAuthorization();


            builder.Services.AddAuthDatabase(builder.Configuration);
            builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
            builder.Services.AddScoped<IAuthInterface, AuthUserService>();


            var app = builder.Build();
            app.UseMiddleware<GlobalExceptionMiddleware>();

            app.UseMiddleware<RequestLoggingMiddleware>();
        

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
        

            app.MapControllers();

            //додаємо ролі та адміна при запуску додатку
            await app.SeedIdentity();

            app.Run();
        }
    }
}
