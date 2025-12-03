using AuthService.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Infrastructure.Seed
{
    public static class UserSeeder
    {


        public static async Task SeedAdminAsync(UserManager<AppUser> userManager)
        {
            var adminUser = await userManager.FindByEmailAsync("szadyraka0509@gmail");
            if (adminUser == null)
            {
                adminUser = new AppUser
                {
                    UserName = "admin",
                    Email = "szadyraka0509@gmail.com",
                    EmailConfirmed = true,
                    FirstName = "Admin",
                    LastName = "Admin"
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                if(!result.Succeeded)
                    throw new Exception("Не удалось создать пользователя");

                await userManager.AddToRoleAsync(adminUser, "Admin");
               
            }
        }

    }
}
