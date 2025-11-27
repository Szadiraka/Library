

namespace BooksService.Domain.Queries
{
    public  class BookQuery
    {
        public string? Title { get; set; }

        public int? PublishedYear { get; set; }

        public Guid? GenreId { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
