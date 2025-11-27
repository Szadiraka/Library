

namespace BooksService.Domain.Entities
{
    public  class BookAuthor
    {
        public Guid BookId { get; set; }

        public Guid AuthorId { get; set; }
    }
}
