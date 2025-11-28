

namespace EmailService.Domain.Queries
{
    public class EmailQuery
    {
        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public Guid? UserId { get; set; }

        public string? To { get; set; }

        public bool? IsSent { get; set; }
    }
}
