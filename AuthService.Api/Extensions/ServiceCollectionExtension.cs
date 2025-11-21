using AuthService.Application.DTOs;
using AuthService.Domain.Entities;
using AuthService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace AuthService.Api.Extensions
{
    public static class ServiceCollectionExtension
    {

        public static IServiceCollection AddAuthDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AuthDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddIdentityCore<AppUser>(options =>
            {
                options.User.RequireUniqueEmail = true;
            }).AddRoles<AppRole>()
               .AddEntityFrameworkStores<AuthDbContext>()
              .AddDefaultTokenProviders();

            return services;
        }

        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
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

            return services;
        }

    }
}
