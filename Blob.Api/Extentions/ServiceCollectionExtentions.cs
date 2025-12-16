using Blob.Application.Dtos;
using Blob.Domain.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Minio;
using System.Text;
using System.Text.Json;

namespace Blob.Api.Extentions
{
    public static class ServiceCollectionExtentions
    {
        public static IServiceCollection AddJwtAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                 .AddJwtBearer(options =>
                 {

                     options.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateIssuer = true,
                         ValidateAudience = true,
                         ValidateLifetime = true,
                         ValidateIssuerSigningKey = true,
                         ValidIssuer = configuration["Jwt:Issuer"],
                         ValidAudience = configuration["Jwt:Audience"],
                         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),

                     };

                     // добавляем обработку ошибок
                     options.Events = new JwtBearerEvents
                     {
                         OnAuthenticationFailed = context =>
                         {
                             context.Response.StatusCode = 401;
                             context.Response.ContentType = "application/json";
                             var result = JsonSerializer.Serialize(new
                                ApiResponse<object>
                             { Message = "Помилка аутентифікації" });
                             return context.Response.WriteAsync(result);
                         },

                         OnChallenge = context =>
                         {
                             context.HandleResponse();
                             context.Response.StatusCode = 401;
                             context.Response.ContentType = "application/json";
                             var result = JsonSerializer.Serialize(new
                                ApiResponse<object>
                             { Message = "Потрібна авторизація" });
                             return context.Response.WriteAsync(result);
                         },
                         OnForbidden = context =>
                         {
                             context.Response.StatusCode = 403;
                             context.Response.ContentType = "application/json";
                             var result = JsonSerializer.Serialize(new
                                ApiResponse<object>
                             { Message = "Доступ заборонено" });
                             return context.Response.WriteAsync(result);
                         }
                     };

                 });

            services.AddAuthorization();

            return services;
        }


        public static IServiceCollection AddBlobService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<BlobSettings>()
                .Bind(configuration.GetSection("BlobSettings"))
                .ValidateDataAnnotations()
                .Validate(
                    s => !string.IsNullOrWhiteSpace(s.Url),"BlobSettings: Url is required")
                .Validate(
                    s => !string.IsNullOrWhiteSpace(s.AccessKey), "BlobSettings: AccessKey is required")
                .Validate(
                   s => !string.IsNullOrWhiteSpace(s.SecretKey), "BlobSettings: SecretKey is required");

            services.AddSingleton(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<BlobSettings>>().Value;
                return new MinioClient()
                .WithEndpoint(settings.Url)
                .WithCredentials(settings.AccessKey,settings.SecretKey)
                .Build();

            });         

       


            return services;
        }

    }
}
