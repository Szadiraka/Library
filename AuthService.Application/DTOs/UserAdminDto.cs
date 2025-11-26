

namespace AuthService.Application.DTOs
{
    public  class UserAdminDto
    {
        public string Id { get; set; }=string.Empty;
        public string? UserName { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public bool EmailConfirmed { get; set; }

        public bool IsBlocked { get; set; }
        public DateTime? BlockedAt { get; set; }
        public DateTime? BlockExpiresAt { get; set; }
        public string? BlockReason { get; set; }

        public DateTimeOffset? LockoutEnd { get; set; }


        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }

        public List<string>? Roles { get; set; } 
 
    }
}
