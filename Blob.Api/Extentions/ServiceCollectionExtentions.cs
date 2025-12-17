using Blob.Application.Dtos;
using Blob.Application.Exceptions;
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
            var blobSettings = configuration.GetSection("BlobSettings").Get<BlobSettings>();

            if (blobSettings == null)
                throw new _ValidationException("BlobSettings секція не міститься в конфігурації");

            if (string.IsNullOrWhiteSpace(blobSettings.Url))
                throw new _ValidationException("BlobSettings: Url відсутній");

            if (string.IsNullOrWhiteSpace(blobSettings.AccessKey))
                throw new InvalidOperationException("BlobSettings: AccessKey відсутній");

            if (string.IsNullOrWhiteSpace(blobSettings.SecretKey))
                throw new InvalidOperationException("BlobSettings: SecretKey вiсутній");
          
            services.AddSingleton(blobSettings);

         
            services.AddSingleton<IMinioClient>(sp =>
            {
                return new MinioClient()
                    .WithEndpoint(blobSettings.Url)
                    .WithCredentials(blobSettings.AccessKey, blobSettings.SecretKey)
                    .Build();
            });

            return services;


        }

    }
}
