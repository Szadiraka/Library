
namespace Authors.Domain.Domains
{
    public  class Author
    {
        public Guid Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string? Bio { get; set; }

        public DateOnly BirthDate { get; set; }

        public DateOnly? DeathDate { get; set; }

        public string? Nationality { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
