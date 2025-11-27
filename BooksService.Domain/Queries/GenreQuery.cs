

namespace BooksService.Domain.Queries
{
    public class GenreQuery
    {
        public string? Name { get; set; }       

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 5;

    }
}
