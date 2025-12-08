using BooksService.Api.Extensions;
using BooksService.Api.Middlewares;
using BooksService.Application.Interfaces;
using BooksService.Application.Services;
using BooksService.Domain.Interfaces;
using BooksService.Infrastructure.Persistance;
using BooksService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace BooksService.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);      
            builder.Services.AddControllers();
            builder.Services.AddJwtAuth(builder.Configuration);

            builder.Services.AddDbContext<AppDbContext>(options=>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IBookRepository, BookRepository>();
            builder.Services.AddScoped<IGenreRepository, GenreRepository>();
            builder.Services.AddScoped<IBookAuthorRepository, BookAuthorRepository>();
            builder.Services.AddScoped<IBookService, BookService>();
            builder.Services.AddScoped<IGenreService, GenreService>();
            builder.Services.AddScoped<IBookAuthorService, BookAuthorService>();


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
