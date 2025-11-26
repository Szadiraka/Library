

namespace Authors.Domain.Queries
{
    public class AuthorQuery
    {
        public string? Name { get; set; }

        public string? Nationality { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;


    }
}
