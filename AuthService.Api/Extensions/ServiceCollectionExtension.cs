using AuthService.Domain.Entities;
using AuthService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

            services.AddIdentity<AppUser, AppRole>(option =>
            {

            }).AddEntityFrameworkStores<AuthDbContext>()
              .AddDefaultTokenProviders();
            return services;
        }

    }
}
