

namespace BooksService.Domain.Queries
{
    public  class BookAuthorQuery
    {    

        public Guid AuthorId { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
