using ApiGateWay.Extensions;
using ApiGateWay.Middlewares;
using ApiGateWay.Models;
using ApiGateWay.Services;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace ApiGateWay
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddHealthChecks();
            builder.Services.AddControllers();
            builder.Services.Configure<AddressesOptions>(builder.Configuration.GetSection("Adresses"));
            builder.Services.AddHttpClient();
            builder.Services.AddScoped<IHealthService, HealthService>();


            builder.Configuration
                .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
                .AddOcelot("Ocelot", builder.Environment)                           
                .AddEnvironmentVariables();      



            builder.Services.AddOcelot(builder.Configuration);

            builder.Services.AddJwtAuth(builder.Configuration);
            
                       
            var app = builder.Build();   
            
            app.UseMiddleware<CorrelationIdMiddleware>();
            app.UseMiddleware<GateWayLoggingMiddleware>();
            app.UseMiddleware<GatewayResponseValidationMiddleware>();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
  
            app.UseEndpoints(endpoints => {
                app.MapHealthChecks("/health");
                app.MapControllers();
            });
           

            await app.UseOcelot();

        
            app.Run();
        }
    }
}
