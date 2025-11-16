using AuthService.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Infrastructure.Seed
{
    public static class UserSeeder
    {


        public static async Task SeedAdminAsync(UserManager<AppUser> userManager)
        {
            var adminUser = await userManager.FindByEmailAsync("admin@example.com");
            if (adminUser == null)
            {
                adminUser = new AppUser
                {
                    UserName = "admin",
                    Email = "admin@example.com",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                if(!result.Succeeded)
                    throw new Exception("Не удалось создать пользователя");

                await userManager.AddToRoleAsync(adminUser, "Admin");
               
            }
        }

    }
}
