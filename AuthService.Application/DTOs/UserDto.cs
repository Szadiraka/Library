

namespace AuthService.Application.DTOs
{
    public  class UserDto
    {
        public string Id { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public DateTime? BirthDate { get; set; }  

        public string? AvatarUrl { get; set; }

        public DateTime CreatedAt { get; set; }


       public List<string> Roles { get; set; } = new List<string>();
        
    }

}

