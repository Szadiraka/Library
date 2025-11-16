using AuthService.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace AuthService.Infrastructure.Persistence
{
    public class AuthDbContext : IdentityDbContext<AppUser, AppRole, string>
    {

        public AuthDbContext(DbContextOptions<AuthDbContext> options) 
            : base(options)
        {
            
        }

    }
}
