using AuthService.Domain.Entities;
using Microsoft.AspNetCore.Identity;


namespace AuthService.Infrastructure.Seed
{
    public static class RoleSeeder
    {
        private static readonly string[] Roles = new[]
        {
            "Admin",
            "User",
            "Librarian"
        };


        public static  async Task SeedRolesAsync(RoleManager<AppRole> roleManager)
        {
            foreach(var roleName in Roles)
            {
                if(! await roleManager.RoleExistsAsync(roleName))
                {
                    var role = new AppRole { Name = roleName };
                    await roleManager.CreateAsync(role);

                }
            }
        }
        


    }
}
