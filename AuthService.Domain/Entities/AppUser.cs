using Microsoft.AspNetCore.Identity;


namespace AuthService.Domain.Entities
{
    public  class AppUser : IdentityUser
    {     

        public string FirstName { get; set; } =string.Empty;

        public string LastName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? BirthDate { get; set; }

        public string? AvatarUrl { get; set; }


        public string RefreshToken { get; set; } = string.Empty;  //токен для оновлення jwt-токену

        public DateTime? RefreshTokenExpiryTime { get; set; }  // дата актуальності цього токена


        public bool IsDeleted { get; set; }

        public DateTime? DeletedAt { get; set; }

        
    }
}
