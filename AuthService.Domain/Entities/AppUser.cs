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


        // оновлення токену
        public string RefreshToken { get; set; } = string.Empty;  

        public DateTime? RefreshTokenExpiryTime { get; set; }  


        // м'яке видалення
        public bool IsDeleted { get; set; }

        public DateTime? DeletedAt { get; set; }

        //  блокування аккаунту

        public bool IsBlocked { get; set; } = false;

        public string? BlockReason { get; set; } = string.Empty ; //причина блокування

        public DateTime? BlockedAt { get; set; }  // заблокувати на завжди

        public DateTime? BlockExpiresAt { get; set; }  // заблокувати тимчасово

        
    }
}
