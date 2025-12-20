using Blob.Api.Extentions;
using Blob.Api.Middleware;
using Blob.Api.Services;
using Blob.Application.Interfaces;
using Blob.Infrastructure.Persistance;
using Blob.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace Blob.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddHealthChecks();
            builder.Services.AddControllers();        
            builder.Services.AddJwtAuth(builder.Configuration);

            builder.Services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            builder.Services.AddBlobService(builder.Configuration);

            builder.Services.AddScoped<IBlobService, BlobService>();
            builder.Services.AddScoped<IFileMetaDataService, FileMetaDataService>();
            builder.Services.AddScoped<IUserContextService, UserContextService>();
            builder.Services.AddScoped<IDbService, DbService>();
     

            var app = builder.Build();
            app.UseMiddleware<CorrelationMiddleware>();
            app.UseMiddleware<RequestLoggingMiddleware>();
            app.UseMiddleware<GlobalExceptionMiddleware>();         


            app.UseAuthentication();
            app.UseAuthorization();

            app.MapHealthChecks("/health");
            app.MapControllers();
            app.Run();
        }
    }
}
