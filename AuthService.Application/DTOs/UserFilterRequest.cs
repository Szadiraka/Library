

namespace AuthService.Application.DTOs
{
    public class UserFilterRequest
    {
        public string? Search { get; set; }

        public bool? IsBlocked { get; set; }

        public bool? IsDeleted { get; set; }

        public string? RoleName { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 20;
    }
}
