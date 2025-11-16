using AuthService.Domain.Entities;
using AuthService.Infrastructure.Seed;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Api.Extensions
{
    public static class IdentitySeedExtensions
    {


        public static async Task SeedIdentity (this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
     
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

            await RoleSeeder.SeedRolesAsync(roleManager);

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            await UserSeeder.SeedAdminAsync(userManager);
            
        }
    }
}
