using Blob.Api.Extentions;
using Blob.Api.Middleware;
using Blob.Application.Interfaces;
using Blob.Infrastructure.Services;

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

            builder.Services.AddBlobService(builder.Configuration);

            builder.Services.AddScoped<IBlobService, BlobService>();
     

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
