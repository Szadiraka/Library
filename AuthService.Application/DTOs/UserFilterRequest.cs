

namespace AuthService.Application.DTOs
{
    public class UserFilterRequest
    {
        public string? Search { get; set; }

        public bool? IsBlocked { get; set; }

        public bool? EmailConfirmed { get; set; }

        public List<string>? Roles { get; set; }

        public  bool? OnlyDeleted { get; set; }

         public bool? OnlyActive { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 20;
    }
}
