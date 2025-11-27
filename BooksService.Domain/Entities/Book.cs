

namespace BooksService.Domain.Entities
{
    public class Book
    {
        public Guid Id { get; set; }

        public string Title { get; set; } =  string.Empty;

        public string? Description { get; set; }     

        public int PublishedYear { get; set; }      

        public Guid GenreId { get; set; }
        public Genre? Genre { get; set; }

        public bool IsDeleted { get; set; }

        public DateTimeOffset? DeletedAt { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }

        public List<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();

    }
}
