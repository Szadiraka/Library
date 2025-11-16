using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Domain.Entities
{
    public  class AppUser : IdentityUser
    {     

        public string FirstName { get; set; } =string.Empty;

        public string LastName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? BirthDate { get; set; }

        public string? AvatarUrl { get; set; }

        
    }
}
