using EmailService.Application.DTOs;
using EmailService.Domain.Settings;
using EmailService.Infrastructure.Persistance;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;

namespace EmailService.Api.Extentions
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
                                ApiResponse
                             { Message = "Помилка аутентифікації" });
                             return context.Response.WriteAsync(result);
                         },

                         OnChallenge = context =>
                         {
                             context.HandleResponse();
                             context.Response.StatusCode = 401;
                             context.Response.ContentType = "application/json";
                             var result = JsonSerializer.Serialize(new
                                ApiResponse
                             { Message = "Потрібна авторизація" });
                             return context.Response.WriteAsync(result);
                         },
                         OnForbidden = context =>
                         {
                             context.Response.StatusCode = 403;
                             context.Response.ContentType = "application/json";
                             var result = JsonSerializer.Serialize(new
                                ApiResponse
                             { Message = "Доступ заборонено" });
                             return context.Response.WriteAsync(result);
                         }
                     };

                 });

            services.AddAuthorization();

            return services;
        }


        public static IServiceCollection AddEmailService( this IServiceCollection services , IConfiguration configuration)
        {
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));


           services.AddDbContext<EmailDbContext>(options =>
           options.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]));

            //подключаем hangHire SqlServer
            services.AddHangfire(config => config
            .UseRecommendedSerializerSettings()
             .UseSqlServerStorage(configuration["ConnectionStrings:DefaultConnection"]));

            //Запускаем сервер Hangfire
            services.AddHangfireServer();

            return services;
        }
       

    }
}
