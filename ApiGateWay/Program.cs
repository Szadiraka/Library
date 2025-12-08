using ApiGateWay.Extensions;
using ApiGateWay.Middlewares;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace ApiGateWay
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration
                .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)               
                .AddOcelot("Ocelot", builder.Environment)                           
                .AddEnvironmentVariables();      



            builder.Services.AddOcelot(builder.Configuration);

            builder.Services.AddJwtAuth(builder.Configuration);
            
                       
            var app = builder.Build();        

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<GatewayResponseValidationMiddleware>();

            await app.UseOcelot();

        
            app.Run();
        }
    }
}
