using AuthService.Api.Extensions;
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


            // додаемо в DI - бд та identity
            builder.Services.AddAuthDatabase(builder.Configuration);


            builder.Services.AddControllers();


            var app = builder.Build();     

            app.UseHttpsRedirection();

            app.UseAuthorization();
        

            app.MapControllers();

            //додаємо ролі та адміна при запуску додатку
            await app.SeedIdentity();

            app.Run();
        }
    }
}
