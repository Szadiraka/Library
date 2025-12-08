using Authors.Api.Extensions;
using Authors.Api.Middleware;
using Authors.Application.Interfaces;
using Authors.Application.Services;
using Authors.Domain.Interfaces;
using Authors.Infrastructure.Persistance;
using Authors.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace Authors.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.Services.AddJwtAuth(builder.Configuration);           

         
            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
            builder.Services.AddScoped<IAuthorService, AuthorService>();
       

            var app = builder.Build();
            app.UseMiddleware<RequestLoggingMiddleware>();
            app.UseMiddleware<GlobalExceptionMiddleware>();


            //app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
