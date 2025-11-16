using AuthService.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
